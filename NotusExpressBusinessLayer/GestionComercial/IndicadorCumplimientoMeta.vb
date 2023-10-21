Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports System.Web

Namespace ConfiguracionComercial

    Public Class IndicadorCumplimientoMeta

#Region "Atributos"

        Private _idIndicador As Short
        Private _valorInicial As Decimal
        Private _valorFinal As Decimal
        Private _rutaImagen As String
        Private _fechaRegistro As Date
        Private _idUsuarioRegistra As Integer
        Private _modificado As Boolean
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _rutaImagen = ""
        End Sub

        Public Sub New(identificador As Integer)
            Me.New()
            _idIndicador = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdIndicador As Short
            Get
                Return _idIndicador
            End Get
            Set(ByVal value As Short)
                _idIndicador = value
            End Set
        End Property

        Public Property ValorInicial As Decimal
            Get
                Return _valorInicial
            End Get
            Set(ByVal value As Decimal)
                _valorInicial = value
            End Set
        End Property

        Public Property ValorFinal As Decimal
            Get
                Return _valorFinal
            End Get
            Set(ByVal value As Decimal)
                _valorFinal = value
            End Set
        End Property

        Public Property RutaImagen As String
            Get
                Return _rutaImagen
            End Get
            Set(ByVal value As String)
                _rutaImagen = value
            End Set
        End Property

        Public Property FechaRegistro As Date
            Get
                Return _fechaRegistro
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdUsuarioRegistra As Integer
            Get
                Return _idUsuarioRegistra
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRegistra = value
            End Set
        End Property

        Public Property Modificado As Boolean
            Get
                Return _modificado
            End Get
            Set(value As Boolean)
                _modificado = value
            End Set
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(value As Boolean)
                _registrado = value
            End Set
        End Property
#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If Me._idIndicador > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idIndicador > 0 Then .SqlParametros.Add("@idIndicador", SqlDbType.SmallInt).Value = Me._idIndicador
                        .ejecutarReader("ObtenerIndicadoresDeCumplimiento", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then AsignarValorAPropiedades(.Reader)
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Portegidos"

        Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idIndicador").ToString, Me._idIndicador)
                    Decimal.TryParse(reader("valorInicial").ToString, Me._valorInicial)
                    Decimal.TryParse(reader("valorFinal").ToString, Me._valorFinal)
                    Me._rutaImagen = reader("rutaImagen").ToString
                    Date.TryParse(reader("fechaRegistro").ToString, Me._fechaRegistro)
                    Integer.TryParse(reader("idUsuarioRegistra").ToString, Me._idUsuarioRegistra)
                    Me._modificado = False
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Eliminar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Me._idIndicador > 0 Then
                Dim dbManager As LMDataAccess = Nothing

                Try
                    dbManager = New LMDataAccess
                    Dim idUsuario As Integer
                    Dim contexto As HttpContext = HttpContext.Current
                    If contexto IsNot Nothing AndAlso contexto.Session IsNot Nothing AndAlso contexto.Session("userId") IsNot Nothing Then _
                        Integer.TryParse(contexto.Session("userId").ToString, idUsuario)
                    If idUsuario = 0 Then idUsuario = 1
                    With dbManager
                        .SqlParametros.Add("@idIndicador", SqlDbType.SmallInt).Value = Me._idIndicador
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output
                        .iniciarTransaccion()

                        .ejecutarNonQuery("EliminarIndicadorDeCumplimiento", CommandType.StoredProcedure)
                        If Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                If .estadoTransaccional Then .confirmarTransaccion()
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                            End If

                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(300, "Imposible evaluar la respuesta proveniente del servidor de bases de datos. Por favor intente nuevamente")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(200, "No se han proporcionado todos los datos requeridos para realizar la eliminación. Por favor verifique")
            End If
            Return resultado
        End Function

#End Region

    End Class

End Namespace