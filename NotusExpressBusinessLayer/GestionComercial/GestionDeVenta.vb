Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace GestionComercial

    Public Class GestionDeVenta

#Region "Atributos"

        Private _idGestionVenta As Long
        Private _idCliente As Integer
        Private _cliente As String
        Private _identificacionCliente As String
        Private _idCiudadCliente As Integer
        Private _ciudadCliente As String
        Private _fechaRegistro As Date
        Private _fechaGestion As Date
        Private _idUsuarioRegistra As Integer
        Private _usuarioRegistra As String
        Private _operadorCall As String
        Private _identificacionOperadorCall As String
        Private _idResultadoProceso As Integer
        Private _resultadoProceso As String
        Private _idTipoVenta As Integer
        Private _tipoVenta As String
        Private _idUsuarioAsesor As Integer
        Private _idPdv As Integer
        Private _pdv As String
        Private _idEstado As Integer
        Private _idSubproducto As Integer
        Private _subProducto As String
        Private _serial As String
        Private _numPlanillaPreAnalisis As Integer
        Private _numVentaPlanilla As Integer
        Private _numPagare As String
        Private _observacionCallCenter As String
        Private _observacionVendedor As String
        Private _fechaRecepcionDocumentos As Date
        Private _idReceptorDocumento As Integer
        Private _fechaLegalizacion As Date
        Private _idLegalizador As Integer
        Private _esNovedad As Boolean
        Private _observacionDeclinar As String
        Private _idEstrategia As Integer
        Private _idNovedad As Integer
        Private _observacionNovedad As String
        Private _idCampaniaNotus As Integer
        Private _idServicioNotus As Long
        Private _idModificador As Integer
        Private _fechaAgenda As Date
        Private _fechaAgendaVer As String
        Private _numeroIdentificacion As String
        Private _estado As String
        Private _estrategia As String
        Private _idEstadoServicioMensajeria As Integer
        Private _estadoNotus As String
        Private _codigoEstrategia As String
        Private _observacionRecepcion As String
        Private _numeroGuia As String
        Private _puedeRadicar As Integer
        Private _fechaRadicacion As DateTime
        Private _idUsuarioRadicacion As Integer
        Private _idRadicado As Integer
        Private _actividadLaboral As String
        Private _idOportunidad As String
        Private _listaNovedades As ArrayList
        Private _idEstadoCalidad As Integer
        Private _idEstadoRechazoCalidad As Integer

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdGestionVenta As Long
            Get
                Return _idGestionVenta
            End Get
            Set(value As Long)
                _idGestionVenta = value
            End Set
        End Property

        Public Property IdCliente As Integer
            Get
                Return _idCliente
            End Get
            Set(value As Integer)
                _idCliente = value
            End Set
        End Property

        Public Property Cliente As String
            Get
                Return _cliente
            End Get
            Set(value As String)
                _cliente = value
            End Set
        End Property

        Public Property IdentificacionCliente As String
            Get
                Return _identificacionCliente
            End Get
            Set(value As String)
                _identificacionCliente = value
            End Set
        End Property

        Public Property IdCiudadCliente As Integer
            Get
                Return _idCiudadCliente
            End Get
            Set(value As Integer)
                _idCiudadCliente = value
            End Set
        End Property

        Public Property CiudadCliente As String
            Get
                Return _ciudadCliente
            End Get
            Set(value As String)
                _ciudadCliente = value
            End Set
        End Property

        Public Property FechaRegistro As Date
            Get
                Return _fechaRegistro
            End Get
            Set(value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property FechaGestion As Date
            Get
                Return _fechaGestion
            End Get
            Set(value As Date)
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

        Public Property UsuarioRegistra As String
            Get
                Return _usuarioRegistra
            End Get
            Set(value As String)
                _usuarioRegistra = value
            End Set
        End Property

        Public Property OperadorCall As String
            Get
                Return _operadorCall
            End Get
            Set(value As String)
                _operadorCall = value
            End Set
        End Property

        Public Property IdentificacionOperadorCall As String
            Get
                Return _identificacionOperadorCall
            End Get
            Set(value As String)
                _identificacionOperadorCall = value
            End Set
        End Property

        Public Property IdResultadoProceso As Integer
            Get
                Return _idResultadoProceso
            End Get
            Set(value As Integer)
                _idResultadoProceso = value
            End Set
        End Property

        Public Property ResultadoProceso As String
            Get
                Return _resultadoProceso
            End Get
            Set(value As String)
                _resultadoProceso = value
            End Set
        End Property

        Public Property IdTipoVenta As Integer
            Get
                Return _idTipoVenta
            End Get
            Set(value As Integer)
                _idTipoVenta = value
            End Set
        End Property

        Public Property TipoVenta As String
            Get
                Return _tipoVenta
            End Get
            Set(value As String)
                _tipoVenta = value
            End Set
        End Property

        Public Property IdUsuarioAsesor As Integer
            Get
                Return _idUsuarioAsesor
            End Get
            Set(value As Integer)
                _idUsuarioAsesor = value
            End Set
        End Property

        Public Property IdPdv As Integer
            Get
                Return _idPdv
            End Get
            Set(value As Integer)
                _idPdv = value
            End Set
        End Property

        Public Property Pdv As String
            Get
                Return _pdv
            End Get
            Set(value As String)
                _pdv = value
            End Set
        End Property

        Public Property IdEstado As Integer
            Get
                Return _idEstado
            End Get
            Set(value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property IdSubproducto As Integer
            Get
                Return _idSubproducto
            End Get
            Set(value As Integer)
                _idSubproducto = value
            End Set
        End Property

        Public Property SubProducto As String
            Get
                Return _subProducto
            End Get
            Set(value As String)
                _subProducto = value
            End Set
        End Property

        Public Property Serial As String
            Get
                Return _serial
            End Get
            Set(value As String)
                _serial = value
            End Set
        End Property

        Public Property NumPlanillaPreAnalisis As Integer
            Get
                Return _numPlanillaPreAnalisis
            End Get
            Set(value As Integer)
                _numPlanillaPreAnalisis = value
            End Set
        End Property

        Public Property NumVentaPlanilla As Integer
            Get
                Return _numVentaPlanilla
            End Get
            Set(value As Integer)
                _numVentaPlanilla = value
            End Set
        End Property

        Public Property NumPagare As String
            Get
                Return _numPagare
            End Get
            Set(value As String)
                _numPagare = value
            End Set
        End Property

        Public Property ObservacionCallCenter As String
            Get
                Return _observacionCallCenter
            End Get
            Set(value As String)
                _observacionCallCenter = value
            End Set
        End Property

        Public Property ObservacionVendedor As String
            Get
                Return _observacionVendedor
            End Get
            Set(value As String)
                _observacionVendedor = value
            End Set
        End Property

        Public Property FechaRecepcionDocumentos As Date
            Get
                Return _fechaRecepcionDocumentos
            End Get
            Set(value As Date)
                _fechaRecepcionDocumentos = value
            End Set
        End Property

        Public Property IdReceptorDocumento As Integer
            Get
                Return _idReceptorDocumento
            End Get
            Set(value As Integer)
                _idReceptorDocumento = value
            End Set
        End Property

        Public Property FechaLegalizacion As Date
            Get
                Return _fechaLegalizacion
            End Get
            Set(value As Date)
                _fechaLegalizacion = value
            End Set
        End Property

        Public Property IdLegalizador As Integer
            Get
                Return _idLegalizador
            End Get
            Set(value As Integer)
                _idLegalizador = value
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

        Public Property ObservacionDeclinar As String
            Get
                Return _observacionDeclinar
            End Get
            Set(value As String)
                _observacionDeclinar = value
            End Set
        End Property

        Public Property IdEstrategia As Integer
            Get
                Return _idEstrategia
            End Get
            Set(value As Integer)
                _idEstrategia = value
            End Set
        End Property

        Public Property IdNovedad As Integer
            Get
                Return _idNovedad
            End Get
            Set(value As Integer)
                _idNovedad = value
            End Set
        End Property

        Public Property ObservacionNovedad As String
            Get
                Return _observacionNovedad
            End Get
            Set(value As String)
                _observacionNovedad = value
            End Set
        End Property

        Public Property IdCampaniaNotus As Integer
            Get
                Return _idCampaniaNotus
            End Get
            Set(value As Integer)
                _idCampaniaNotus = value
            End Set
        End Property

        Public Property IdServicioNotus As Long
            Get
                Return _idServicioNotus
            End Get
            Set(value As Long)
                _idServicioNotus = value
            End Set
        End Property

        Public Property IdModificador As Integer
            Get
                Return _idModificador
            End Get
            Set(value As Integer)
                _idModificador = value
            End Set
        End Property

        Public Property FechaAgenda As Date
            Get
                Return _fechaAgenda
            End Get
            Set(value As Date)
                _fechaAgenda = value
            End Set
        End Property

        Public Property FechaAgendaVer As String
            Get
                Return _fechaAgendaVer
            End Get
            Set(value As String)
                _fechaAgendaVer = value
            End Set
        End Property

        Public Property NumeroIdentificacion As String
            Get
                Return _numeroIdentificacion
            End Get
            Set(value As String)
                _numeroIdentificacion = value
            End Set
        End Property

        Public Property Estado As String
            Get
                Return _estado
            End Get
            Set(value As String)
                _estado = value
            End Set
        End Property

        Public Property Estrategia As String
            Get
                Return _estrategia
            End Get
            Set(value As String)
                _estrategia = value
            End Set
        End Property

        Public Property IdEstadoServicioMensajeria As Integer
            Get
                Return _idEstadoServicioMensajeria
            End Get
            Set(value As Integer)
                _idEstadoServicioMensajeria = value
            End Set
        End Property

        Public Property EstadoNotus As String
            Get
                Return _estadoNotus
            End Get
            Set(value As String)
                _estadoNotus = value
            End Set
        End Property

        Public Property CodigoEstrategia As String
            Get
                Return _codigoEstrategia
            End Get
            Set(value As String)
                _codigoEstrategia = value
            End Set
        End Property

        Public Property ObservacionRecepcion As String
            Get
                Return _observacionRecepcion
            End Get
            Set(value As String)
                _observacionRecepcion = value
            End Set
        End Property

        Public Property NumeroGuia As String
            Get
                Return _numeroGuia
            End Get
            Set(value As String)
                _numeroGuia = value
            End Set
        End Property

        Public Property ListaNovedades As ArrayList
            Get
                If _listaNovedades Is Nothing Then _listaNovedades = New ArrayList
                Return _listaNovedades
            End Get
            Set(value As ArrayList)
                _listaNovedades = value
            End Set
        End Property

        Public Property PuedeRadicar As Integer
            Get
                Return _puedeRadicar
            End Get
            Set(value As Integer)
                _puedeRadicar = value
            End Set
        End Property

        Public Property FechaRadicacion As DateTime
            Get
                Return _fechaRadicacion
            End Get
            Set(value As DateTime)
                _fechaRadicacion = value
            End Set
        End Property

        Public Property IdUsuarioRadicacion As Integer
            Get
                Return _idUsuarioRadicacion
            End Get
            Set(value As Integer)
                _idUsuarioRadicacion = value
            End Set
        End Property

        Public Property IdRadicado As Integer
            Get
                Return _idRadicado
            End Get
            Set(value As Integer)
                _idRadicado = value
            End Set
        End Property

        Public Property ActividadLaboral As String
            Get
                Return _actividadLaboral
            End Get
            Set(value As String)
                _actividadLaboral = value
            End Set
        End Property

        Public Property IdOportunidad As String
            Get
                Return _idOportunidad
            End Get
            Set(value As String)
                _idOportunidad = value
            End Set
        End Property

        Public Property IdEstadoCalidad() As Integer
            Get
                Return _idEstadoCalidad
            End Get
            Set(value As Integer)
                _idEstadoCalidad = value
            End Set
        End Property

        Public Property IdEstadoRechazoCalidad() As Integer
            Get
                Return _idEstadoRechazoCalidad
            End Get
            Set(value As Integer)
                _idEstadoRechazoCalidad = value
            End Set
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

        Public Sub New(ByVal idGestionVenta As Long)
            MyBase.New()
            _idGestionVenta = idGestionVenta
            CargarDatos()
        End Sub

        Public Sub New(ByVal idServicioNotus As Integer)
            MyBase.New()
            _idServicioNotus = idServicioNotus
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idGestionVenta > 0 Then .SqlParametros.Add("@listGestionVenta", SqlDbType.VarChar, 2000).Value = CStr(_idGestionVenta)
                    If _idServicioNotus > 0 Then .SqlParametros.Add("@listServicioNotus", SqlDbType.VarChar, 2000).Value = CStr(_idServicioNotus)
                    .ejecutarReader("ObtenerInfoGestionVenta", CommandType.StoredProcedure)
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

#End Region

#Region "Métodos Públicos"

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                        .Add("@idModificador", SqlDbType.Int).Value = _idModificador
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If _idCliente > 0 Then .Add("@idCliente", SqlDbType.Int).Value = _idCliente
                        If _fechaAgenda > Date.MinValue Then .Add("@fechaAgenda", SqlDbType.Date).Value = _fechaAgenda
                        If _idResultadoProceso > 0 Then .Add("@idResultadoProceso", SqlDbType.Int).Value = _idResultadoProceso
                        If _idServicioNotus > 0 Then .Add("@idServicioNotus", SqlDbType.BigInt).Value = _idServicioNotus
                        If _idEstadoServicioMensajeria > 0 Then .Add("@idEstadoServicioMensajeria", SqlDbType.Int).Value = _idEstadoServicioMensajeria
                        If Not String.IsNullOrEmpty(_observacionCallCenter) Then .Add("@observacionCallCenter", SqlDbType.VarChar, 2000).Value = _observacionCallCenter
                        If Not String.IsNullOrEmpty(_observacionVendedor) Then .Add("@observacionVendedor", SqlDbType.VarChar, 2000).Value = _observacionVendedor
                        If _fechaRecepcionDocumentos > Date.MinValue Then .Add("@fechaRecepcionDocumentos", SqlDbType.Date).Value = _fechaRecepcionDocumentos
                        If _idReceptorDocumento > 0 Then .Add("@idReceptorDocumento", SqlDbType.Int).Value = _idReceptorDocumento
                        If Not String.IsNullOrEmpty(_observacionRecepcion) Then .Add("@observacionRecepcion", SqlDbType.VarChar, 2000).Value = _observacionRecepcion
                        If Not String.IsNullOrEmpty(_numeroGuia) Then .Add("@numeroGuia", SqlDbType.VarChar, 2000).Value = _numeroGuia
                        If _fechaLegalizacion > Date.MinValue Then .Add("@fechaLegalizacion", SqlDbType.Date).Value = _fechaLegalizacion
                        If _idLegalizador > 0 Then .Add("@idLegalizador", SqlDbType.Int).Value = _idLegalizador
                        If Not String.IsNullOrEmpty(_observacionDeclinar) Then .Add("@observacionDeclinar", SqlDbType.VarChar, 2000).Value = _observacionDeclinar
                        If _idNovedad > 0 Then .Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                        If Not String.IsNullOrEmpty(_observacionNovedad) Then .Add("@observacionNovedad", SqlDbType.VarChar, 2000).Value = _observacionNovedad
                        If _idCampaniaNotus > 0 Then .Add("@idCampaniaNotus", SqlDbType.Int).Value = _idCampaniaNotus
                        If _listaNovedades IsNot Nothing AndAlso _listaNovedades.Count > 0 Then .Add("@listNovedades", SqlDbType.VarChar).Value = Join(_listaNovedades.ToArray, ",")
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ActualizaGestionVenta", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al actualizar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ConsultaPoolGestionPorAgente() As DataTable
            Dim dt As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        .Add("@NumeroDocumento", SqlDbType.VarChar).Value = _identificacionOperadorCall
                    End With
                    dt = .ejecutarDataTable("ObtenerInfoPoolGestionVenta", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Function ObtenerFechaInicialAgenda() As DataTable
            Dim dt As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        .Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                    End With
                    dt = .EjecutarDataTable("ObtenerFechaInicialAgenda", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Function RegistrarGestionCalidad() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                        .Add("@idModificador", SqlDbType.Int).Value = _idModificador
                        If _idEstadoCalidad > 0 Then .Add("@idEstadoCalidad", SqlDbType.Int).Value = _idEstadoCalidad
                        If _idEstadoRechazoCalidad > 0 Then .Add("@idEstadoRechazoCalidad", SqlDbType.Int).Value = _idEstadoRechazoCalidad
                        If Not String.IsNullOrEmpty(_observacionCallCenter) Then .Add("@obsCalidad", SqlDbType.VarChar, 2000).Value = _observacionCallCenter
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistrarGestionCalidadVenta", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al actualizar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Long.TryParse(reader("idGestionVenta"), _idGestionVenta)
                    If Not IsDBNull(reader("idCliente")) Then Integer.TryParse(reader("idCliente"), _idCliente)
                    If Not IsDBNull(reader("cliente")) Then _cliente = (reader("cliente").ToString)
                    If Not IsDBNull(reader("numeroIdentificacion")) Then _identificacionCliente = (reader("numeroIdentificacion").ToString)
                    If Not IsDBNull(reader("idCiudadCliente")) Then Integer.TryParse(reader("idCiudadCliente"), _idCiudadCliente)
                    If Not IsDBNull(reader("ciudadCliente")) Then _ciudadCliente = (reader("ciudadCliente").ToString)
                    If Not IsDBNull(reader("fechaRegistro")) Then _fechaRegistro = CDate(reader("fechaRegistro").ToString)
                    If Not IsDBNull(reader("fechaGestion")) Then _fechaGestion = CDate(reader("fechaGestion").ToString)
                    If Not IsDBNull(reader("idUsuarioRegistra")) Then Integer.TryParse(reader("idUsuarioRegistra"), _idUsuarioRegistra)
                    If Not IsDBNull(reader("usuarioRegistra")) Then _usuarioRegistra = (reader("usuarioRegistra").ToString)
                    If Not IsDBNull(reader("operadorCall")) Then _operadorCall = (reader("operadorCall").ToString)
                    If Not IsDBNull(reader("idResultadoProceso")) Then Integer.TryParse(reader("idResultadoProceso"), _idResultadoProceso)
                    If Not IsDBNull(reader("resultadoProceso")) Then _resultadoProceso = (reader("resultadoProceso").ToString)
                    If Not IsDBNull(reader("idTipoVenta")) Then Integer.TryParse(reader("idTipoVenta"), _idTipoVenta)
                    If Not IsDBNull(reader("tipoVenta")) Then _tipoVenta = (reader("tipoVenta").ToString)
                    If Not IsDBNull(reader("idUsuarioAsesor")) Then Integer.TryParse(reader("idUsuarioAsesor"), _idUsuarioAsesor)
                    If Not IsDBNull(reader("idPdv")) Then Integer.TryParse(reader("idPdv"), _idPdv)
                    If Not IsDBNull(reader("pdv")) Then _pdv = (reader("pdv").ToString)
                    If Not IsDBNull(reader("idEstado")) Then Integer.TryParse(reader("idEstado"), _idEstado)
                    If Not IsDBNull(reader("idSubproducto")) Then Integer.TryParse(reader("idSubproducto"), _idSubproducto)
                    If Not IsDBNull(reader("serial")) Then _serial = (reader("serial").ToString)
                    If Not IsDBNull(reader("numPlanillaPreAnalisis")) Then _numPlanillaPreAnalisis = (reader("numPlanillaPreAnalisis").ToString)
                    If Not IsDBNull(reader("numVentaPlanilla")) Then _numVentaPlanilla = (reader("numVentaPlanilla").ToString)
                    If Not IsDBNull(reader("numPagare")) Then _numPagare = (reader("numPagare").ToString)
                    If Not IsDBNull(reader("observacionCallCenter")) Then _observacionCallCenter = (reader("observacionCallCenter").ToString)
                    If Not IsDBNull(reader("observacionVendedor")) Then _observacionVendedor = (reader("observacionVendedor").ToString)
                    If Not IsDBNull(reader("fechaRecepcionDocumentos")) Then _fechaRecepcionDocumentos = CDate((reader("fechaRecepcionDocumentos").ToString))
                    If Not IsDBNull(reader("idReceptorDocumento")) Then Integer.TryParse(reader("idReceptorDocumento"), _idReceptorDocumento)
                    If Not IsDBNull(reader("fechaLegalizacion")) Then _fechaLegalizacion = CDate((reader("fechaLegalizacion").ToString))
                    If Not IsDBNull(reader("fechaAgenda")) Then _fechaAgenda = CDate((reader("fechaAgenda").ToString))
                    If Not IsDBNull(reader("fechaAgenda")) Then _fechaAgendaVer = ((reader("fechaAgenda").ToString))
                    If Not IsDBNull(reader("idLegalizador")) Then Integer.TryParse(reader("idLegalizador"), _idLegalizador)
                    If Not IsDBNull(reader("idCampaniaNotus")) Then Integer.TryParse(reader("idCampaniaNotus"), _idCampaniaNotus)
                    If Not IsDBNull(reader("idServicioNotus")) Then Integer.TryParse(reader("idServicioNotus"), _idServicioNotus)
                    If Not IsDBNull(reader("numeroIdentificacion")) Then _numeroIdentificacion = (reader("numeroIdentificacion").ToString)
                    If Not IsDBNull(reader("estado")) Then _estado = (reader("estado").ToString)
                    If Not IsDBNull(reader("estrategia")) Then _estrategia = (reader("estrategia").ToString)
                    If Not IsDBNull(reader("idEstadoServicioMensajeria")) Then Integer.TryParse(reader("idEstadoServicioMensajeria"), _idEstadoServicioMensajeria)
                    If Not IsDBNull(reader("estadoNotus")) Then _estadoNotus = (reader("estadoNotus").ToString)
                    If Not IsDBNull(reader("codigoEstrategia")) Then _codigoEstrategia = (reader("codigoEstrategia").ToString)
                    If Not IsDBNull(reader("observacionRecepcion")) Then _observacionRecepcion = (reader("observacionRecepcion").ToString)
                    If Not IsDBNull(reader("numeroGuia")) Then _numeroGuia = (reader("numeroGuia").ToString)
                    If Not IsDBNull(reader("fechaRadicacion")) Then _fechaRadicacion = CDate((reader("fechaRadicacion").ToString))
                    If Not IsDBNull(reader("idUsuarioRadicacion")) Then Integer.TryParse(reader("idUsuarioRadicacion"), _idUsuarioRadicacion)
                    If Not IsDBNull(reader("idRadicado")) Then Integer.TryParse(reader("idRadicado"), _idRadicado)
                    If Not IsDBNull(reader("puedeRadicar")) Then Integer.TryParse(reader("puedeRadicar"), _puedeRadicar)
                    If Not IsDBNull(reader("actividadLaboral")) Then _actividadLaboral = (reader("actividadLaboral").ToString)
                    If Not IsDBNull(reader("idOportunidad")) Then _idOportunidad = (reader("idOportunidad").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace