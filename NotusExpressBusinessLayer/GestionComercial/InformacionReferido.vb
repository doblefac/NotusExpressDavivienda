Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class InformacionReferido

#Region "Atributos"

    Private _idInfoReferido As Integer
    Private _idCliente As Integer
    Private _referidoPor As String
    Private _idAsesor As Integer
    Private _idPdv As Integer
    Private _fechaRegistro As Date
    Private _registrado As Boolean
    Private _infoCliente As ClienteFinal

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _referidoPor = ""
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdInfoReferido() As Integer
        Get
            Return _idInfoReferido
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idInfoReferido = value
        End Set
    End Property

    Protected Friend Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Set(ByVal value As Integer)
            _idCliente = value
        End Set
    End Property

    Public Property ReferidoPor() As String
        Get
            Return _referidoPor
        End Get
        Set(ByVal value As String)
            _referidoPor = value
        End Set
    End Property

    Public Property IdAsesor() As Integer
        Get
            Return _idAsesor
        End Get
        Set(ByVal value As Integer)
            _idAsesor = value
        End Set
    End Property

    Protected Friend Property IdPdv() As Integer
        Get
            Return _idPdv
        End Get
        Set(ByVal value As Integer)
            _idPdv = value
        End Set
    End Property

    Public Property FechaRegistro() As Date
        Get
            Return _fechaRegistro
        End Get
        Protected Friend Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property InfoCliente() As ClienteFinal
        Get
            If _infoCliente Is Nothing Then _infoCliente = New ClienteFinal
            Return _infoCliente
        End Get
        Set(ByVal value As ClienteFinal)
            _infoCliente = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idAsesor > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                dbManager.iniciarTransaccion()
                If Not _infoCliente.Registrado Then
                    _infoCliente.IdCreador = _idAsesor
                    resultado = _infoCliente.Registrar(dbManager)
                Else
                    _infoCliente.IdModificador = _idAsesor
                    resultado = _infoCliente.Actualizar(dbManager)
                End If

                If resultado.Valor = 0 Then
                    If _infoCliente.Registrado AndAlso _infoCliente.IdCliente > 0 Then
                        With dbManager
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _infoCliente.IdCliente
                            .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idAsesor
                            If Not String.IsNullOrEmpty(_referidoPor) Then _
                                .SqlParametros.Add("@referidoPor", SqlDbType.VarChar, 100).Value = _referidoPor
                            .SqlParametros.Add("@idInfoReferido", SqlDbType.Int).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("RegistrarInformacionDeReferido", CommandType.StoredProcedure)

                            If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                If resultado.Valor = 0 Then
                                    .confirmarTransaccion()
                                    _idInfoReferido = CInt(.SqlParametros("@idInfoReferido").Value.ToString)
                                    _registrado = True
                                Else
                                    .abortarTransaccion()
                                End If
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                            Else
                                resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de los datos del referido. Por favor intente nuevamente")
                            End If

                        End With
                    Else
                        Throw New Exception("Imposible registrar la información del cliente. Por favor intente nuevamente.")
                    End If
                Else
                    dbManager.abortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para realizar el registro del referido. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region

End Class
