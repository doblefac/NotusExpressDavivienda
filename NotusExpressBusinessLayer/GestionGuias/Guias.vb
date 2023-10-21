Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports System.String
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Configuration
Imports LMMailSenderLibrary

Namespace Guias

    Public Class Guias

#Region "Atributos"

        Private _idGuia As Long
        Private _guia As String
        Private _idEstado As Integer
        Private _listGestionVentas As List(Of Integer)
        Private _idUsuario As Integer
        Private _direccionDestino As ArrayList
        Private _registrado As Boolean

        Private _nombreUsuario As String
        Private _password As String
        Private _dominio As String
        Private _nombreServidor As String
        Private _cuentaOrigen As String

#End Region

#Region "Propiedades"

        Public Property IdGuia As Long
            Get
                Return _idGuia
            End Get
            Set(value As Long)
                _idGuia = value
            End Set
        End Property

        Public Property Guia As String
            Get
                Return _guia
            End Get
            Set(value As String)
                _guia = value
            End Set
        End Property

        Public Property IdEstado As String
            Get
                Return _idEstado
            End Get
            Set(value As String)
                _idEstado = value
            End Set
        End Property

        Public Property ListGestionVenta As List(Of Integer)
            Get
                If _listGestionVentas Is Nothing Then _listGestionVentas = New List(Of Integer)
                Return _listGestionVentas
            End Get
            Set(value As List(Of Integer))
                _listGestionVentas = value
            End Set
        End Property

        Public Property IdUsuario As Integer
            Get
                Return _idUsuario
            End Get
            Set(value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public ReadOnly Property DireccionDestino() As ArrayList
            Get
                If _direccionDestino Is Nothing Then _direccionDestino = New ArrayList
                Return _direccionDestino
            End Get
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idGuia As Long)
            MyBase.New()
            _idGuia = idGuia
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idGuia > 0 Then .SqlParametros.Add("@IdGuia", SqlDbType.Int).Value = _idGuia
                    .ejecutarReader("ObtenerInfoGuia", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub

        Private Sub InicializarCadenasDeTexto()
            _nombreUsuario = "system.notifier"
            _password = "12345.LM"
            _dominio = "LM"
            _nombreServidor = ConfigurationSettings.AppSettings("mailServer")
            _cuentaOrigen = ConfigurationSettings.AppSettings("mailSender")
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function ConsultaCedulasSinGuias() As DataTable
            Dim dt As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                    End With
                    dt = .ejecutarDataTable("ObtenerCedulasSinGuias", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Function RegistrarEnvio() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idGuia", SqlDbType.Int).Value = _idGuia
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        If _listGestionVentas IsNot Nothing AndAlso _listGestionVentas.Count > 0 Then _
                            .Add("@listidGestionVenta", SqlDbType.VarChar).Value = Join(",", _listGestionVentas.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarEnvioGuiaCedula", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                        End If
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                    End If

                End With
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se presentó un error al generar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ObtenerMailNotificaciones(ByVal idNotificacion As Integer) As DataTable
            Dim _dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With _dbManager
                    .SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.Int).Value = idNotificacion
                End With
                Return _dbManager.ejecutarDataTable("ObtenerUsuarioNotificacion", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
        End Function

        Public Function ObtenerGuias(ByVal idGuias As Integer) As DataTable
            Dim _dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With _dbManager
                    .SqlParametros.Add("@idGuia", SqlDbType.Int).Value = idGuias
                End With
                Return _dbManager.ejecutarDataTable("ObtenerGuiasEnviadas", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
        End Function

        Public Sub NotificarViaMailEnvioGuia(ByVal mensaje As String, Optional ByVal ruta As String = "")
            If _direccionDestino IsNot Nothing AndAlso _direccionDestino.Count > 0 Then
                Dim mailSender As New MailMessage
                Dim mailHandler As New LMMailSender
                InicializarCadenasDeTexto()
                Try
                    Dim mailBody As New StringBuilder
                    With mailBody
                        .Append("<html>")
                        .Append("<head><title>Notificación envio de guias</title>")
                        .Append("<style type='text/css'>")
                        .Append("body {font: 70%/1.5em Verdana, Tahoma, arial, sans-serif; margin: 5px 10px; padding: 5px 5px; background-color: #ffffff;}")
                        .Append(".tabla {font-family: Helvetica, Arial, sans-serif; font-size:9pt; border-color:Gray}")
                        .Append("</style></head>")
                        .Append("<body>")
                        .Append("<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'>")
                        .Append("<td align='center' bgcolor='#883485' width='80%'><font size='3' face='arial' color='white'>" & _
                                "<b>NOTIFICACIÓN ENVIO DE GUIAS</b></font></td></tr>")
                        .Append("</table><br><br>")
                        .Append("<table><tr>")
                        .Append("<td><font size='2'><b>&nbsp; Acontinuación se relaciona la guia y los números de identificacion incluidos en el envio</b></td>")
                        .Append("<tr><td><font size='3'><b>&nbsp; " & mensaje & " </b></td></tr>")
                        .Append("</tr></table>")
                        .Append("<font name='Verdana' size='2'>")
                        .Append("<br><br><font name='Verdana' size='2'><b>Atentamente,<br>IT Development - Logytech Mobile S.A.S</b></font><br><br>")
                        .Append("<font class='Verdana' size='1'><i>Nota: Este correo es generado automaticamente, ")
                        .Append("si tiene alguna duda, inquietud o comentario por favor envíe un e-mail a 'IT Development' <ITDevelopment@logytechmobile.com>")
                        .Append("</body>")
                        .Append("</html>")
                    End With
                    Dim htmlBody As AlternateView = AlternateView.CreateAlternateViewFromString(mailBody.ToString, Nothing, MediaTypeNames.Text.Html)
                    With mailHandler
                        .ServidorCorreo = _nombreServidor
                        .EstablecerCredenciales(_nombreUsuario, _password, _dominio)
                        .EstablecerCuentaOrigen(_cuentaOrigen)
                        For index As Integer = 0 To _direccionDestino.Count - 1
                            .Destanatarios.Add(_direccionDestino(index))
                        Next
                        If Not String.IsNullOrEmpty(ruta) Then
                            Dim _adjuntos As New ArrayList
                            _adjuntos.Add(ruta)
                            .AdjuntoUrl = _adjuntos
                            .AdjuntarArchivos()
                        End If
                        .Prioridad = Net.Mail.MailPriority.High
                        .CuerpoEsHtml = True
                        .Asunto = "Notificación Envio de Guias." & Date.Now
                        .VistaAlternativa.Add(htmlBody)
                        .Enviar()
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Long.TryParse(reader("idGuia"), _idGuia)
                    If Not IsDBNull(reader("guia")) Then _guia = (reader("guia").ToString)
                    Long.TryParse(reader("idEstado"), _idEstado)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace