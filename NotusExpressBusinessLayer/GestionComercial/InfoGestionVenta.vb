Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class InfoGestionVenta

#Region "Atributos"

    Private _idGestionVenta As Integer
    Private _idPreventa As Integer
    Private _idCliente As Integer
    Private _fechaRegistro As Date
    Private _fechaGestion As Date
    Private _idUsuarioRegistra As Integer
    Private _operadorCall As String
    Private _identificacionOperadorCall As String
    Private _idResultadoProceso As Short
    Private _idTipoVenta As Short
    Private _idUsuarioAsesor As Integer
    Private _idPdv As Integer
    Private _idSubproducto As Integer
    Private _serial As String
    Private _numeroPlanillaPreAnalisis As Integer
    Private _numeroVentaPlanilla As Integer
    Private _numeroPagare As String
    Private _observacionCallCenter As String
    Private _observacionVendedor As String
    Private _fechaRecepcionDocumentos As Date
    Private _idReceptorDocumento As Integer
    Private _fechaLegalizacion As Date
    Private _idLegalizador As Integer
    Private _fechaUltimaModificacion As Date
    Private _idModificador As Integer
    Private _esNovedad As Boolean
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdGestionVenta() As Integer
        Get
            Return _idGestionVenta
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idGestionVenta = value
        End Set
    End Property

    Public Property IdPreventa() As Integer
        Get
            Return _idPreventa
        End Get
        Set(ByVal value As Integer)
            _idPreventa = value
        End Set
    End Property

    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Set(ByVal value As Integer)
            _idCliente = value
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

    Public Property FechaGestion() As Date
        Get
            Return _fechaGestion
        End Get
        Set(ByVal value As Date)
            _fechaGestion = value
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

    Public Property OperadorCall() As String
        Get
            Return _operadorCall
        End Get
        Set(ByVal value As String)
            _operadorCall = value
        End Set
    End Property

    Public Property IdentificacionOperadorCall() As String
        Get
            Return _identificacionOperadorCall
        End Get
        Set(ByVal value As String)
            _identificacionOperadorCall = value
        End Set
    End Property

    Public Property IdResultadoProceso() As Short
        Get
            Return _idResultadoProceso
        End Get
        Set(ByVal value As Short)
            _idResultadoProceso = value
        End Set
    End Property

    Public Property IdTipoVenta() As Short
        Get
            Return _idTipoVenta
        End Get
        Set(ByVal value As Short)
            _idTipoVenta = value
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

    Public Property IdSubproducto() As Integer
        Get
            Return _idSubproducto
        End Get
        Set(ByVal value As Integer)
            _idSubproducto = value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property

    Public Property NumeroPlanillaPreAnalisis() As Integer
        Get
            Return _numeroPlanillaPreAnalisis
        End Get
        Set(ByVal value As Integer)
            _numeroPlanillaPreAnalisis = value
        End Set
    End Property

    Public Property NumeroVentaPlanilla() As Integer
        Get
            Return _numeroVentaPlanilla
        End Get
        Set(ByVal value As Integer)
            _numeroVentaPlanilla = value
        End Set
    End Property

    Public Property NumeroPagare() As String
        Get
            Return _numeroPagare
        End Get
        Set(ByVal value As String)
            _numeroPagare = value
        End Set
    End Property

    Public Property ObservacionCallCenter() As String
        Get
            Return _observacionCallCenter
        End Get
        Set(ByVal value As String)
            _observacionCallCenter = value
        End Set
    End Property

    Public Property ObservacionVendedor() As String
        Get
            Return _observacionVendedor
        End Get
        Set(ByVal value As String)
            _observacionVendedor = value
        End Set
    End Property

    Public Property FechaRecepcionDocumentos() As Date
        Get
            Return _fechaRecepcionDocumentos
        End Get
        Set(ByVal value As Date)
            _fechaRecepcionDocumentos = value
        End Set
    End Property

    Public Property IdReceptorDocumento() As Integer
        Get
            Return _idReceptorDocumento
        End Get
        Set(ByVal value As Integer)
            _idReceptorDocumento = value
        End Set
    End Property

    Public Property FechaLegalizacion() As Date
        Get
            Return _fechaLegalizacion
        End Get
        Set(ByVal value As Date)
            _fechaLegalizacion = value
        End Set
    End Property

    Public Property IdLegalizador() As Integer
        Get
            Return _idLegalizador
        End Get
        Set(ByVal value As Integer)
            _idLegalizador = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property EsNovedad As Boolean
        Get
            Return _esNovedad
        End Get
        Set(value As Boolean)
            _esNovedad = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Registrar(ByVal _infoCliente As ClienteFinal) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idUsuarioAsesor > 0 AndAlso _infoCliente IsNot Nothing AndAlso Not String.IsNullOrEmpty(_operadorCall) _
            AndAlso _idResultadoProceso > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                dbManager.iniciarTransaccion()
                If Not _infoCliente.Registrado Then
                    _infoCliente.IdCreador = _idUsuarioRegistra
                    resultado = _infoCliente.Registrar(dbManager)
                Else
                    _infoCliente.IdModificador = _idUsuarioRegistra
                    resultado = _infoCliente.Actualizar(dbManager)
                End If

                If resultado.Valor = 0 Then
                    If _infoCliente.Registrado AndAlso _infoCliente.IdCliente > 0 Then
                        With dbManager
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _infoCliente.IdCliente
                            .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                            .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                            .SqlParametros.Add("@fechaGestion", SqlDbType.DateTime).Value = _fechaGestion
                            .SqlParametros.Add("@operadorCall", SqlDbType.VarChar, 100).Value = _operadorCall
                            If Not String.IsNullOrEmpty(_identificacionOperadorCall) Then _
                                .SqlParametros.Add("@identificacionOperadorCall", SqlDbType.VarChar, 15).Value = _identificacionOperadorCall
                            .SqlParametros.Add("@idResultadoProceso", SqlDbType.SmallInt).Value = _idResultadoProceso
                            If _idPdv > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv
                            If _idTipoVenta > 0 Then .SqlParametros.Add("@idTipoVenta", SqlDbType.SmallInt).Value = _idTipoVenta
                            If _idSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = _idSubproducto
                            If Not String.IsNullOrEmpty(_serial) Then _
                                .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                            If _numeroPlanillaPreAnalisis > 0 Then .SqlParametros.Add("@numeroPlanillaPreAnalisis", SqlDbType.Int).Value = _numeroPlanillaPreAnalisis
                            If _numeroVentaPlanilla > 0 Then .SqlParametros.Add("@numeroVentaPlanilla", SqlDbType.Int).Value = _numeroVentaPlanilla
                            If Not String.IsNullOrEmpty(_numeroPagare) Then .SqlParametros.Add("@numeroPagare", SqlDbType.VarChar, 15).Value = _numeroPagare
                            If Not String.IsNullOrEmpty(_observacionCallCenter) Then _
                                .SqlParametros.Add("@observacionCallCenter", SqlDbType.VarChar, 3000).Value = _observacionCallCenter
                            If Not String.IsNullOrEmpty(_observacionVendedor) Then _
                                .SqlParametros.Add("@observacionVendedor", SqlDbType.VarChar, 3000).Value = _observacionVendedor
                            .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .SqlParametros.Add("@esNovedad", SqlDbType.Bit).Value = _esNovedad
                            
                            .ejecutarNonQuery("RegistrarInfoGestionVenta", CommandType.StoredProcedure)

                            If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                If resultado.Valor = 0 Then
                                    _idGestionVenta = CInt(.SqlParametros("@idGestionVenta").Value.ToString)
                                    _registrado = True
                                End If
                                .confirmarTransaccion()
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                            Else
                                .abortarTransaccion()
                                resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de la venta. Por favor intente nuevamente")
                            End If
                        End With
                    Else
                        Throw New Exception("Imposible registrar la información de la venta. Por favor intente nuevamente.")
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
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para realizar el registro de la venta. Por favor verifique")
        End If

        Return resultado
    End Function

    Public Function Actualizar(ByVal _infoCliente As ClienteFinal) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idUsuarioAsesor > 0 AndAlso _infoCliente IsNot Nothing AndAlso Not String.IsNullOrEmpty(_operadorCall) _
            AndAlso _idResultadoProceso > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                dbManager.iniciarTransaccion()
                If Not _infoCliente.Registrado Then
                    _infoCliente.IdCreador = _idUsuarioRegistra
                    resultado = _infoCliente.Registrar(dbManager)
                Else
                    _infoCliente.IdModificador = _idUsuarioRegistra
                    resultado = _infoCliente.Actualizar(dbManager)
                End If

                If resultado.Valor = 0 Then
                    If _infoCliente.Registrado AndAlso _infoCliente.IdCliente > 0 Then
                        With dbManager
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _infoCliente.IdCliente
                            .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                            .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                            .SqlParametros.Add("@fechaGestion", SqlDbType.DateTime).Value = _fechaGestion
                            .SqlParametros.Add("@operadorCall", SqlDbType.VarChar, 100).Value = _operadorCall
                            If Not String.IsNullOrEmpty(_identificacionOperadorCall) Then _
                                .SqlParametros.Add("@identificacionOperadorCall", SqlDbType.VarChar, 15).Value = _identificacionOperadorCall
                            .SqlParametros.Add("@idResultadoProceso", SqlDbType.SmallInt).Value = _idResultadoProceso
                            If _idPdv > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv
                            If _idTipoVenta > 0 Then .SqlParametros.Add("@idTipoVenta", SqlDbType.SmallInt).Value = _idTipoVenta
                            If _idSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = _idSubproducto
                            If Not String.IsNullOrEmpty(_serial) Then _
                                .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                            If _numeroPlanillaPreAnalisis > 0 Then .SqlParametros.Add("@numeroPlanillaPreAnalisis", SqlDbType.Int).Value = _numeroPlanillaPreAnalisis
                            If _numeroVentaPlanilla > 0 Then .SqlParametros.Add("@numeroVentaPlanilla", SqlDbType.Int).Value = _numeroVentaPlanilla
                            If Not String.IsNullOrEmpty(_numeroPagare) Then .SqlParametros.Add("@numeroPagare", SqlDbType.VarChar, 15).Value = _numeroPagare
                            If Not String.IsNullOrEmpty(_observacionCallCenter) Then _
                                .SqlParametros.Add("@observacionCallCenter", SqlDbType.VarChar, 3000).Value = _observacionCallCenter
                            If Not String.IsNullOrEmpty(_observacionVendedor) Then _
                                .SqlParametros.Add("@observacionVendedor", SqlDbType.VarChar, 3000).Value = _observacionVendedor
                            .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .SqlParametros.Add("@esNovedad", SqlDbType.Bit).Value = _esNovedad
                            .SqlParametros.Add("@idPreventa", SqlDbType.Int).Value = _idPreventa
                            .ejecutarNonQuery("ActualizarInfoGestionVenta", CommandType.StoredProcedure)

                            If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                If resultado.Valor = 0 Then
                                    _registrado = True
                                End If
                                .confirmarTransaccion()
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                            Else
                                .abortarTransaccion()
                                resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de la venta. Por favor intente nuevamente")
                            End If
                        End With
                    Else
                        Throw New Exception("Imposible registrar la información de la venta. Por favor intente nuevamente.")
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
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para realizar el registro de la venta. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region
End Class
