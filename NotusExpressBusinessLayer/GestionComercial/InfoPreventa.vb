Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class InfoPreventa

#Region "Atributos"

    Private _idPreventa As Integer
    Private _idPreventaA As Integer
    Private _idCliente As Integer
    Private _idUsuarioRegistra As Integer
    Private _idUsuarioAsesor As Integer
    Private _idPdv As Integer
    Private _fechaRegistro As Date
    Private _registrado As Boolean
    Private _infoCliente As ClienteFinal

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdPreventa() As Integer
        Get
            Return _idPreventa
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idPreventa = value
        End Set
    End Property

    Public Property IdPreventaA() As Integer
        Get
            Return _idPreventaA
        End Get
        Set(ByVal value As Integer)
            _idPreventaA = value
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

    Public Property IdUsuarioRegistra As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(value As Integer)
            _idUsuarioRegistra = value
        End Set
    End Property

    Public Property IdUsuarioAsesor() As Integer
        Get
            Return _idUsuarioAsesor
        End Get
        Set(ByVal value As Integer)
            _idUsuarioAsesor = value
        End Set
    End Property

    Public Property IdPdv() As Integer
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

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idUsuarioRegistra > 0 AndAlso _idUsuarioAsesor > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                dbManager.iniciarTransaccion()
                If Not _infoCliente.Registrado Then
                    _infoCliente.IdCreador = _idUsuarioAsesor
                    resultado = _infoCliente.Registrar(dbManager)
                Else
                    _infoCliente.IdModificador = _idUsuarioAsesor
                    resultado = _infoCliente.Actualizar(dbManager)
                End If

                If resultado.Valor = 0 Then
                    If _infoCliente.Registrado AndAlso _infoCliente.IdCliente > 0 Then
                        With dbManager
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _infoCliente.IdCliente
                            .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                            .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                            If _idPdv > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv
                            .SqlParametros.Add("@idPreventa", SqlDbType.Int).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("RegistrarInformacionDePreventa", CommandType.StoredProcedure)

                            If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                If resultado.Valor = 0 Then
                                    .confirmarTransaccion()
                                    _idPreventa = CInt(.SqlParametros("@idPreventa").Value.ToString)
                                    _registrado = True
                                Else
                                    .abortarTransaccion()
                                End If
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                            Else
                                resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de la preventa. Por favor intente nuevamente")
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
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para realizar el registro de la preventa. Por favor verifique")
        End If

        Return resultado
    End Function

    Public Function Eliminar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idPreventa > 0 AndAlso _idUsuarioAsesor > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idPreventa", SqlDbType.Int).Value = _idPreventa
                    .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("EliminarInformacionDePreventa", CommandType.StoredProcedure)

                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante la eliminación de la preventa. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para eliminar la preventa. Por favor verifique")
        End If

        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idUsuarioRegistra > 0 AndAlso _idUsuarioAsesor > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                dbManager.iniciarTransaccion()
                If Not _infoCliente.Registrado Then
                    _infoCliente.IdCreador = _idUsuarioAsesor
                    resultado = _infoCliente.Registrar(dbManager)
                Else
                    _infoCliente.IdModificador = _idUsuarioAsesor
                    resultado = _infoCliente.Actualizar(dbManager)
                End If

                If resultado.Valor = 0 Then
                    If _infoCliente.Registrado AndAlso _infoCliente.IdCliente > 0 Then
                        With dbManager
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _infoCliente.IdCliente
                            .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                            .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                            If _idPdv > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv
                            .SqlParametros.Add("@idPreventa", SqlDbType.Int).Value = _idPreventaA
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("ActualizarInformacionDePreventa", CommandType.StoredProcedure)

                            If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                If resultado.Valor = 0 Then
                                    .confirmarTransaccion()
                                    _registrado = True
                                Else
                                    .abortarTransaccion()
                                End If
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                            Else
                                resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de la preventa. Por favor intente nuevamente")
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
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para realizar el registro de la preventa. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region
End Class
