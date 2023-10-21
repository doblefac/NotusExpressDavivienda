Imports System.Data.SqlClient
Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ClienteFinal

#Region "Atributos"

    Private _idCliente As Integer
    Private _idGestionVenta As Integer
    Private _idTipoIdentificacion As Integer
    Private _numeroIdentificacion As String
    Private _nombreApellido As String
    Private _nombres As String
    Private _primerApellido As String
    Private _segundoApellido As String
    Private _sexo As String
    Private _fechaNacimiento As Date
    Private _idCiudadResidencia As Integer
    Private _ciudadResidencia As String
    Private _direccionResidencia As String
    Private _barrioResidencia As String
    Private _direccionOficina As String
    Private _telefonoResidencia As String
    Private _telefonoAdicional As String
    Private _celular As String
    Private _telefonoOficina As String
    Private _idEmpresa As Integer
    Private _ingresoAproximado As Double
    Private _idEstatusLaboral As Byte
    Private _email As String
    Private _fechaRegistro As Date
    Private _idCreador As Integer
    Private _fechaUltimaModificacion As Date
    Private _idModificador As Integer
    Private _registrado As Boolean
    Private _idPdv As Integer
    Private _idUsuarioAsesor As Integer
    Private _idCampania As Integer
    Private _estadoActual As String
    Private _causal As String
    Private _agente As String
    Private _idAgente As Integer
    Private _codigoEstrategia As String
    Private _estrategia As String
    Private _fechaAgenda As String
    Private _idJornada As Integer
    Private _estadoNotus As String
    Private _idAsesor As Integer
    Private _fechaUltimaGestion As String
    Private _cantidadGestiones As Integer
    Private _actividadLaboral As String
    Private _resultadoProcesoCliente As Integer
    Private _resultadoUbica As String
    Private _resultadoEvidente As String
    Private _resultadoDataCredito As String
    Private _numConsultaUbica As String
    Private _numConsultaEvidente As String
    Private _numConsultaDataCredito As String
    Private _valorPreaprobadoDataCredito As String
    Private _idResultadoProcesoCredito As Integer
    Private _observacionesCallCenter As String
    Private _idEstadoAnimo As Integer
    Private _crearFueraBaseCargue As Boolean
    Public Property Supervisor As String
    Private _codOficina As String
    Private _idEmpresaCliente As Integer
    Private _idEmpresaAsesor As Integer
    Private _esDireccionModificada As Boolean

#End Region

#Region "Propiedades"

    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Set(ByVal value As Integer)
            _idCliente = value
        End Set
    End Property

    Public Property IdEstadoAnimo() As Integer
        Get
            Return _idEstadoAnimo
        End Get
        Set(ByVal value As Integer)
            _idEstadoAnimo = value
        End Set
    End Property

    Public Property IdGestionVenta() As Integer
        Get
            Return _idGestionVenta
        End Get
        Set(ByVal value As Integer)
            _idGestionVenta = value
        End Set
    End Property

    Public Property IdTipoIdentificacion() As Integer
        Get
            Return _idTipoIdentificacion
        End Get
        Set(ByVal value As Integer)
            _idTipoIdentificacion = value
        End Set
    End Property

    Public Property NumeroIdentificacion() As String
        Get
            Return _numeroIdentificacion
        End Get
        Set(ByVal value As String)
            _numeroIdentificacion = value
        End Set
    End Property

    Public Property NombreApellido() As String
        Get
            Return _nombreApellido
        End Get
        Set(ByVal value As String)
            _nombreApellido = value
        End Set
    End Property

    Public Property Nombres() As String
        Get
            Return _nombres
        End Get
        Set(ByVal value As String)
            _nombres = value
        End Set
    End Property

    Public Property PrimerApellido() As String
        Get
            Return _primerApellido
        End Get
        Set(ByVal value As String)
            _primerApellido = value
        End Set
    End Property

    Public Property SegundoApellido() As String
        Get
            Return _segundoApellido
        End Get
        Set(ByVal value As String)
            _segundoApellido = value
        End Set
    End Property

    Public Property Sexo() As String
        Get
            Return _sexo
        End Get
        Set(ByVal value As String)
            _sexo = value
        End Set
    End Property

    Public Property FechaNacimiento() As Date
        Get
            Return _fechaNacimiento
        End Get
        Set(ByVal value As Date)
            _fechaNacimiento = value
        End Set
    End Property

    Public Property IdCiudadResidencia() As Integer
        Get
            Return _idCiudadResidencia
        End Get
        Set(ByVal value As Integer)
            _idCiudadResidencia = value
        End Set
    End Property

    Public Property CiudadResedencia() As String
        Get
            Return _ciudadResidencia
        End Get
        Set(ByVal value As String)
            _ciudadResidencia = value
        End Set
    End Property

    Public Property IdEmpresa() As Integer
        Get
            Return _idEmpresa
        End Get
        Set(value As Integer)
            _idEmpresa = value
        End Set
    End Property

    Public Property DireccionResidencia() As String
        Get
            Return _direccionResidencia
        End Get
        Set(ByVal value As String)
            _direccionResidencia = value
        End Set
    End Property

    Public Property BarrioResidencia() As String
        Get
            Return _barrioResidencia
        End Get
        Set(ByVal value As String)
            _barrioResidencia = value
        End Set
    End Property

    Public Property DireccionOficina() As String
        Get
            Return _direccionOficina
        End Get
        Set(ByVal value As String)
            _direccionOficina = value
        End Set
    End Property

    Public Property TelefonoResidencia() As String
        Get
            Return _telefonoResidencia
        End Get
        Set(ByVal value As String)
            _telefonoResidencia = value
        End Set
    End Property

    Public Property TelefonoAdicional() As String
        Get
            Return _telefonoAdicional
        End Get
        Set(value As String)
            _telefonoAdicional = value
        End Set
    End Property

    Public Property Celular() As String
        Get
            Return _celular
        End Get
        Set(ByVal value As String)
            _celular = value
        End Set
    End Property

    Public Property TelefonoOficina() As String
        Get
            Return _telefonoOficina
        End Get
        Set(ByVal value As String)
            _telefonoOficina = value
        End Set
    End Property

    Public Property IngresoAproximado() As Double
        Get
            Return _ingresoAproximado
        End Get
        Set(ByVal value As Double)
            _ingresoAproximado = value
        End Set
    End Property

    Public Property IdEstatusLaboral() As Byte
        Get
            Return _idEstatusLaboral
        End Get
        Set(ByVal value As Byte)
            _idEstatusLaboral = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
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

    Public Property IdCreador() As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal value As Integer)
            _idCreador = value
        End Set
    End Property

    Public Property FechaUltimaModificacion()
        Get
            Return _fechaUltimaModificacion
        End Get
        Set(ByVal value)
            _fechaUltimaModificacion = value
        End Set
    End Property

    Public Property IdModificador() As Integer
        Get
            Return _idModificador
        End Get
        Set(ByVal value As Integer)
            _idModificador = value
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

    Public Property IdPdv() As Integer
        Get
            Return _idPdv
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idPdv = value
        End Set
    End Property

    Public Property IdUsuarioAsesor() As Integer
        Get
            Return _idUsuarioAsesor
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idUsuarioAsesor = value
        End Set
    End Property

    Public Property IdCampania As Integer
        Get
            Return _idCampania
        End Get
        Set(value As Integer)
            _idCampania = value
        End Set
    End Property

    Public Property EstadoActual As String
        Get
            Return _estadoActual
        End Get
        Set(value As String)
            _estadoActual = value
        End Set
    End Property

    Public Property Causal As String
        Get
            Return _causal
        End Get
        Set(value As String)
            _causal = value
        End Set
    End Property

    Public Property Agente As String
        Get
            Return _agente
        End Get
        Set(value As String)
            _agente = value
        End Set
    End Property

    Public Property IdAgente As Integer
        Get
            Return _idAgente
        End Get
        Set(value As Integer)
            _idAgente = value
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

    Public Property Estrategia As String
        Get
            Return _estrategia
        End Get
        Set(value As String)
            _estrategia = value
        End Set
    End Property

    Public Property FechaAgenda As String
        Get
            Return _fechaAgenda
        End Get
        Set(value As String)
            _fechaAgenda = value
        End Set
    End Property

    Public Property IdJornada As Integer
        Get
            Return _idJornada
        End Get
        Set(value As Integer)
            _idJornada = value
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

    Public Property IdAsesor() As Integer
        Get
            Return _idAsesor
        End Get
        Set(ByVal value As Integer)
            _idAsesor = value
        End Set
    End Property

    Public Property FechaUltimaGestion As String
        Get
            Return _fechaUltimaGestion
        End Get
        Set(value As String)
            _fechaUltimaGestion = value
        End Set
    End Property

    Public Property CantidadGestiones As Integer
        Get
            Return _cantidadGestiones
        End Get
        Set(value As Integer)
            _cantidadGestiones = value
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

    Public Property ResultadoProcesoCliente As Integer
        Get
            Return _resultadoProcesoCliente
        End Get
        Set(value As Integer)
            _resultadoProcesoCliente = value
        End Set
    End Property

    Public Property ResultadoUbica As String
        Get
            Return _resultadoUbica
        End Get
        Set(value As String)
            _resultadoUbica = value
        End Set
    End Property

    Public Property ResultadoEvidente As String
        Get
            Return _resultadoEvidente
        End Get
        Set(value As String)
            _resultadoEvidente = value
        End Set
    End Property

    Public Property ResultadoDataCredito() As String
        Get
            Return _resultadoDataCredito
        End Get
        Set(value As String)
            _resultadoDataCredito = value
        End Set
    End Property

    Public Property NumConsultaUbica As String
        Get
            Return _numConsultaUbica
        End Get
        Set(value As String)
            _numConsultaUbica = value
        End Set
    End Property

    Public Property NumConsultaEvidente As String
        Get
            Return _numConsultaEvidente
        End Get
        Set(value As String)
            _numConsultaEvidente = value
        End Set
    End Property

    Public Property NumConsultaDataCredito As String
        Get
            Return _numConsultaDataCredito
        End Get
        Set(value As String)
            _numConsultaDataCredito = value
        End Set
    End Property

    Public Property ValorPreaprobadoDataCredito As String
        Get
            Return _valorPreaprobadoDataCredito
        End Get
        Set(value As String)
            _valorPreaprobadoDataCredito = value
        End Set
    End Property

    Public Property IdResultadoProcesoCredito() As Integer
        Get
            Return _idResultadoProcesoCredito
        End Get
        Set(value As Integer)
            _idResultadoProcesoCredito = value
        End Set
    End Property

    Public Property ObservacionesCallCenter() As String
        Get
            Return _observacionesCallCenter
        End Get
        Set(value As String)
            _observacionesCallCenter = value
        End Set
    End Property

    Public Property CrearFueraBaseCargue() As Boolean
        Get
            Return _crearFueraBaseCargue
        End Get
        Set(value As Boolean)
            _crearFueraBaseCargue = value
        End Set
    End Property

    Public Property CodOficina As String
        Get
            Return _codOficina
        End Get
        Set(value As String)
            _codOficina = value
        End Set
    End Property

    Public Property IdEmpresaCliente As Integer
        Get
            Return _idEmpresaCliente
        End Get
        Set(value As Integer)
            _idEmpresaCliente = value
        End Set
    End Property

    Public Property IdEmpresaAsesor As Integer
        Get
            Return _idEmpresaAsesor
        End Get
        Set(value As Integer)
            _idEmpresaAsesor = value
        End Set
    End Property

    Public Property esDireccionModificada As Boolean
        Get
            Return _esDireccionModificada
        End Get
        Set(value As Boolean)
            _esDireccionModificada = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _numeroIdentificacion = ""
        _nombreApellido = ""
        _ciudadResidencia = ""
        _direccionResidencia = ""
        _barrioResidencia = ""
        _direccionOficina = ""
        _telefonoResidencia = ""
        _celular = ""
        _telefonoOficina = ""
        _email = ""

    End Sub

    Public Sub New(ByVal idCliente As Integer)
        Me.New()
        _idCliente = idCliente
        CargarDatos()
    End Sub

    Public Sub New(ByVal numeroIdentificacion As String, ByVal idUsuarioConsulta As Integer)
        Me.New()
        _numeroIdentificacion = numeroIdentificacion
        _idUsuarioAsesor = idUsuarioConsulta
        CargarDatos()
    End Sub

    Public Sub New(ByVal idGestionVenta As Integer, ByVal numeroIdentificacion As String, ByVal idUsuarioConsulta As Integer)
        Me.New()
        _idGestionVenta = idGestionVenta
        _numeroIdentificacion = numeroIdentificacion
        _idUsuarioAsesor = idUsuarioConsulta
        CargarDatosClienteGestion()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If _idCliente > 0 OrElse Not String.IsNullOrEmpty(_numeroIdentificacion) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idCliente > 0 Then .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _idCliente
                    If Not String.IsNullOrEmpty(_numeroIdentificacion) Then _
                        .SqlParametros.Add("@identificacion", SqlDbType.VarChar, 20).Value = _numeroIdentificacion

                    If _idUsuarioAsesor > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuarioAsesor

                    .ejecutarReader("ObtenerInfoClienteFinal", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

    Private Sub CargarDatosClienteGestion()
        If _idCliente > 0 OrElse Not String.IsNullOrEmpty(_idGestionVenta) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idGestionVenta > 0 Then .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                    If Not String.IsNullOrEmpty(_numeroIdentificacion) Then _
                        .SqlParametros.Add("@identificacion", SqlDbType.VarChar, 20).Value = _numeroIdentificacion

                    .ejecutarReader("ObtenerInfoClienteGestion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

    Public Function AgregarClienteFueraBaseCargue() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                .SqlParametros.Add("@idTipoIdentificacion", SqlDbType.Int).Value = _idTipoIdentificacion
                .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar).Value = _numeroIdentificacion
                .SqlParametros.Add("@nombreApellido", SqlDbType.VarChar).Value = _nombreApellido
                .SqlParametros.Add("@nombres", SqlDbType.VarChar).Value = _nombres
                .SqlParametros.Add("@primerApellido", SqlDbType.VarChar).Value = _primerApellido
                If Not String.IsNullOrEmpty(_segundoApellido) Then
                    .SqlParametros.Add("@segundoApellido", SqlDbType.VarChar).Value = _segundoApellido
                End If
                .SqlParametros.Add("@idCiudadResidencia", SqlDbType.Int).Value = _idCiudadResidencia
                .SqlParametros.Add("@direccionResidencia", SqlDbType.VarChar).Value = _direccionResidencia
                .SqlParametros.Add("@telefonoResidencia", SqlDbType.VarChar).Value = _telefonoResidencia
                .SqlParametros.Add("@celular", SqlDbType.VarChar).Value = _celular
                .SqlParametros.Add("@ingresoAproximado", SqlDbType.VarChar).Value = _ingresoAproximado
                .SqlParametros.Add("@email", SqlDbType.VarChar).Value = _email
                .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = _idCreador
                .SqlParametros.Add("@idModificador", SqlDbType.Int).Value = _idModificador
                .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                .SqlParametros.Add("@actividadLaboral", SqlDbType.VarChar).Value = _actividadLaboral
                .SqlParametros.Add("@codOficina", SqlDbType.VarChar).Value = _codOficina
                .SqlParametros.Add("@idEstadoAnimo", SqlDbType.Int).Value = IdEstadoAnimo
                .SqlParametros.Add("@creadoFueraBaseCargue", SqlDbType.Bit).Value = _crearFueraBaseCargue
                .ejecutarReader("AgregarClienteFueraBase", CommandType.StoredProcedure)
            End With
            resultado.EstablecerMensajeYValor(0, "Se agregó correctamente el usuario " & _nombreApellido & "|verde")
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(1, "Se generó un error al Agregar el cliente: " & ex.ToString() & "|rojo")
        End Try
        Return resultado

    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idCliente").ToString, _idCliente)
                Integer.TryParse(reader("idGestionVenta").ToString, _idGestionVenta)
                Integer.TryParse(reader("idTipoIdentificacion").ToString, _idTipoIdentificacion)
                Integer.TryParse(reader("idJornada").ToString, _idJornada)
                _numeroIdentificacion = reader("numeroIdentificacion").ToString
                _nombreApellido = reader("nombreApellido").ToString
                _nombres = reader("nombres").ToString
                _primerApellido = reader("primerApellido").ToString
                _segundoApellido = reader("segundoApellido").ToString
                _sexo = reader("sexo").ToString
                _actividadLaboral = reader("actividadLaboral").ToString
                _codOficina = reader("codOficina").ToString
                Date.TryParse(reader("fechaNacimiento").ToString, _fechaNacimiento)
                Integer.TryParse(reader("idCiudadResidencia").ToString, _idCiudadResidencia)
                _ciudadResidencia = reader("ciudadResidencia").ToString
                _direccionResidencia = reader("direccionResidencia").ToString
                _barrioResidencia = reader("barrioResidencia").ToString
                _direccionOficina = reader("direccionOficina").ToString
                _telefonoResidencia = reader("telefonoResidencia").ToString
                _telefonoAdicional = reader("telefonoAdicional").ToString
                _celular = reader("celular").ToString
                _telefonoOficina = reader("telefonoOficina").ToString
                Double.TryParse(reader("ingresoAproximado").ToString, _ingresoAproximado)
                Byte.TryParse(reader("idEstatusLaboral").ToString, _idEstatusLaboral)
                _email = reader("email").ToString
                Date.TryParse(reader("fechaRegistro").ToString, _fechaRegistro)
                Integer.TryParse(reader("idCreador").ToString, _idCreador)
                Date.TryParse(reader("fechaUltimaModificacion").ToString, _fechaUltimaModificacion)
                Integer.TryParse(reader("idModificador").ToString, _idModificador)
                _estadoActual = reader("estadoActual").ToString
                _causal = reader("causal").ToString
                _agente = reader("agente").ToString
                Integer.TryParse(reader("idAgente").ToString, _idAgente)
                _codigoEstrategia = reader("codigoEstrategia").ToString
                _estrategia = reader("estrategia").ToString
                _fechaAgenda = reader("fechaAgenda").ToString
                _estadoNotus = reader("estadoNotus").ToString
                _fechaUltimaGestion = reader("fechaUltimaGestion").ToString
                Boolean.TryParse(reader("esDireccionModificada").ToString, _esDireccionModificada)
                Integer.TryParse(reader("cantidadGestiones").ToString, _cantidadGestiones)
                Integer.TryParse(reader("idEmpresa").ToString, _idEmpresa)
                Integer.TryParse(reader("resultadoProcesoCliente").ToString, _resultadoProcesoCliente)
                _resultadoUbica = reader("resultadoUbica").ToString
                _resultadoEvidente = reader("resultadoEvidente").ToString
                _resultadoDataCredito = reader("resultadoDataCredito").ToString
                _numConsultaUbica = reader("numConsultaUbica").ToString
                _numConsultaEvidente = reader("numConsultaEvidente").ToString
                _numConsultaDataCredito = reader("numConsultaDataCredito").ToString
                _valorPreaprobadoDataCredito = reader("cupoPreaprobadoDataCredito").ToString
                Integer.TryParse(reader("idResultadoProcesoCredito").ToString, _idResultadoProcesoCredito)
                _observacionesCallCenter = reader("observacionCallCenter").ToString
                Integer.TryParse(reader("IdEstadoAnimo").ToString, _idEstadoAnimo)
                Integer.TryParse(reader("idEmpresaUsuarioConsulta").ToString, _idEmpresaAsesor)
                Integer.TryParse(reader("idEmpresaCliente").ToString, _idEmpresaCliente)

                _registrado = True
                If Not String.IsNullOrEmpty(_idCampania) Then Integer.TryParse(reader("idCampania").ToString, _idCampania)
            End If
        End If

    End Sub

    Protected Friend Function Registrar(ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idCreador > 0 AndAlso Not String.IsNullOrEmpty(_nombreApellido) AndAlso
            (Not String.IsNullOrEmpty(_telefonoResidencia) OrElse Not String.IsNullOrEmpty(_telefonoOficina) _
            OrElse Not String.IsNullOrEmpty(_celular)) Then

            Dim finalizarObjAccesoDatos As Boolean = False
            Try
                If dbManager Is Nothing Then
                    dbManager = New LMDataAccess
                    finalizarObjAccesoDatos = True
                End If
                With dbManager
                    If _idTipoIdentificacion > 0 Then _
                        .SqlParametros.Add("@idTipoIdentificacion", SqlDbType.SmallInt).Value = _idTipoIdentificacion
                    If Not String.IsNullOrEmpty(_numeroIdentificacion) Then _
                        .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = _numeroIdentificacion
                    .SqlParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = _nombreApellido
                    If Not String.IsNullOrEmpty(_nombres) Then _
                    .SqlParametros.Add("@nombres", SqlDbType.VarChar, 100).Value = _nombres
                    If Not String.IsNullOrEmpty(_primerApellido) Then _
                    .SqlParametros.Add("@primerApellido", SqlDbType.VarChar, 100).Value = _primerApellido
                    If Not String.IsNullOrEmpty(_segundoApellido) Then _
                    .SqlParametros.Add("@segundoApellido", SqlDbType.VarChar, 100).Value = _segundoApellido
                    If Not String.IsNullOrEmpty(_direccionResidencia) Then _
                        .SqlParametros.Add("@direccionResidencia", SqlDbType.VarChar, 100).Value = _direccionResidencia
                    If _idCiudadResidencia > 0 Then _
                        .SqlParametros.Add("@idCiudadResidencia", SqlDbType.Int).Value = _idCiudadResidencia
                    If Not String.IsNullOrEmpty(_telefonoResidencia) Then _
                        .SqlParametros.Add("@telefonoResidencia", SqlDbType.VarChar, 20).Value = _telefonoResidencia
                    If Not String.IsNullOrEmpty(_celular) Then _
                        .SqlParametros.Add("@celular", SqlDbType.VarChar, 20).Value = _celular
                    If Not String.IsNullOrEmpty(_email) Then _
                        .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = _email
                    If _ingresoAproximado > 0 Then _
                        .SqlParametros.Add("@ingresoAproximado", SqlDbType.Money).Value = _ingresoAproximado
                    If _idEmpresa > 0 Then _
                        .SqlParametros.Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                    If _resultadoProcesoCliente > 0 Then _
                        .SqlParametros.Add("@idResultadoProcesoCliente", SqlDbType.Int).Value = _resultadoProcesoCliente
                    .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = _idCreador
                    If Not String.IsNullOrEmpty(_barrioResidencia) Then _
                        .SqlParametros.Add("@barrioResidencia", SqlDbType.VarChar, 100).Value = _barrioResidencia
                    If Not String.IsNullOrEmpty(_direccionOficina) Then _
                        .SqlParametros.Add("@direccionOficina", SqlDbType.VarChar, 100).Value = _direccionOficina
                    If Not String.IsNullOrEmpty(_telefonoOficina) Then _
                        .SqlParametros.Add("@telefonoOficina", SqlDbType.VarChar, 20).Value = _telefonoOficina
                    If _idEstatusLaboral > 0 Then _
                        .SqlParametros.Add("@idEstatusLaboral", SqlDbType.TinyInt).Value = _idEstatusLaboral
                    .SqlParametros.Add("@idCliente", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .EjecutarNonQuery("RegistrarInfoClienteFinal", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        If resultado.Valor = 0 Then
                            _idCliente = CInt(.SqlParametros("@idCliente").Value.ToString)
                            _registrado = True
                        End If
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de los datos del cliente. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If finalizarObjAccesoDatos AndAlso dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para realizar el registro del cliente. Por favor verifique")
        End If

        Return resultado
    End Function

    Protected Friend Function Actualizar(ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idCliente > 0 AndAlso _idModificador > 0 AndAlso Not String.IsNullOrEmpty(_nombreApellido) AndAlso
            (Not String.IsNullOrEmpty(_telefonoResidencia) OrElse Not String.IsNullOrEmpty(_telefonoOficina) _
            OrElse Not String.IsNullOrEmpty(_celular)) Then

            Dim finalizarObjAccesoDatos As Boolean = False
            Try
                If dbManager Is Nothing Then
                    dbManager = New LMDataAccess
                    finalizarObjAccesoDatos = True
                End If
                With dbManager
                    .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _idCliente
                    .SqlParametros.Add("@idModificador", SqlDbType.Int).Value = _idCreador
                    .SqlParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = _nombreApellido
                    .SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = _nombres
                    .SqlParametros.Add("@apellido1", SqlDbType.VarChar, 100).Value = _primerApellido
                    .SqlParametros.Add("@apellido2", SqlDbType.VarChar, 100).Value = _segundoApellido
                    If _idTipoIdentificacion > 0 Then _
                        .SqlParametros.Add("@idTipoIdentificacion", SqlDbType.SmallInt).Value = _idTipoIdentificacion
                    If _idCiudadResidencia > 0 Then _
                        .SqlParametros.Add("@idCiudadResidencia", SqlDbType.Int).Value = _idCiudadResidencia
                    If Not String.IsNullOrEmpty(_direccionResidencia) Then _
                        .SqlParametros.Add("@direccionResidencia", SqlDbType.VarChar, 200).Value = _direccionResidencia
                    If Not String.IsNullOrEmpty(_barrioResidencia) Then _
                        .SqlParametros.Add("@barrioResidencia", SqlDbType.VarChar, 100).Value = _barrioResidencia
                    If Not String.IsNullOrEmpty(_direccionOficina) Then _
                        .SqlParametros.Add("@direccionOficina", SqlDbType.VarChar, 100).Value = _direccionOficina
                    If Not String.IsNullOrEmpty(_telefonoResidencia) Then _
                        .SqlParametros.Add("@telefonoResidencia", SqlDbType.VarChar, 20).Value = _telefonoResidencia
                    If Not String.IsNullOrEmpty(_celular) Then _
                        .SqlParametros.Add("@celular", SqlDbType.VarChar, 20).Value = _celular
                    If Not String.IsNullOrEmpty(_telefonoOficina) Then _
                        .SqlParametros.Add("@telefonoOficina", SqlDbType.VarChar, 20).Value = _telefonoOficina
                    If _ingresoAproximado > 0 Then .SqlParametros.Add("@ingresoAproximado", SqlDbType.Money).Value = _ingresoAproximado
                    If _idEstatusLaboral > 0 Then .SqlParametros.Add("@idEstatusLaboral", SqlDbType.TinyInt).Value = _idEstatusLaboral
                    If Not String.IsNullOrEmpty(_email) Then .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = _email
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .EjecutarNonQuery("ActualizarInfoClienteFinal", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de los datos del cliente. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If finalizarObjAccesoDatos AndAlso dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para actualizar los datos del cliente. Por favor verifique")
        End If

        Return resultado
    End Function

    Protected Friend Function ActualizarClienteGestion(ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idCliente > 0 AndAlso _idModificador > 0 AndAlso Not String.IsNullOrEmpty(_nombreApellido) AndAlso
            (Not String.IsNullOrEmpty(_telefonoResidencia) OrElse Not String.IsNullOrEmpty(_telefonoOficina) _
            OrElse Not String.IsNullOrEmpty(_celular)) Then

            Dim finalizarObjAccesoDatos As Boolean = False
            Try
                If dbManager Is Nothing Then
                    dbManager = New LMDataAccess
                    finalizarObjAccesoDatos = True
                End If
                With dbManager
                    .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _idCliente
                    .SqlParametros.Add("@idModificador", SqlDbType.Int).Value = _idCreador
                    .SqlParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = _nombreApellido
                    .SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = _nombres
                    .SqlParametros.Add("@apellido1", SqlDbType.VarChar, 100).Value = _primerApellido
                    .SqlParametros.Add("@apellido2", SqlDbType.VarChar, 100).Value = _segundoApellido
                    If _idTipoIdentificacion > 0 Then _
                        .SqlParametros.Add("@idTipoIdentificacion", SqlDbType.SmallInt).Value = _idTipoIdentificacion
                    If _idCiudadResidencia > 0 Then _
                        .SqlParametros.Add("@idCiudadResidencia", SqlDbType.Int).Value = _idCiudadResidencia
                    If Not String.IsNullOrEmpty(_direccionResidencia) Then _
                        .SqlParametros.Add("@direccionResidencia", SqlDbType.VarChar, 200).Value = _direccionResidencia
                    If Not String.IsNullOrEmpty(_barrioResidencia) Then _
                        .SqlParametros.Add("@barrioResidencia", SqlDbType.VarChar, 100).Value = _barrioResidencia
                    If Not String.IsNullOrEmpty(_direccionOficina) Then _
                        .SqlParametros.Add("@direccionOficina", SqlDbType.VarChar, 100).Value = _direccionOficina
                    If Not String.IsNullOrEmpty(_telefonoResidencia) Then _
                        .SqlParametros.Add("@telefonoResidencia", SqlDbType.VarChar, 20).Value = _telefonoResidencia
                    If Not String.IsNullOrEmpty(_celular) Then _
                        .SqlParametros.Add("@celular", SqlDbType.VarChar, 20).Value = _celular
                    If Not String.IsNullOrEmpty(_telefonoOficina) Then _
                        .SqlParametros.Add("@telefonoOficina", SqlDbType.VarChar, 20).Value = _telefonoOficina
                    If _ingresoAproximado > 0 Then .SqlParametros.Add("@ingresoAproximado", SqlDbType.Money).Value = _ingresoAproximado
                    If _idEstatusLaboral > 0 Then .SqlParametros.Add("@idEstatusLaboral", SqlDbType.TinyInt).Value = _idEstatusLaboral
                    If Not String.IsNullOrEmpty(_email) Then .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = _email
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .EjecutarNonQuery("ActualizarInfoClienteGestion", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de los datos del cliente. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If finalizarObjAccesoDatos AndAlso dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para actualizar los datos del cliente. Por favor verifique")
        End If

        Return resultado
    End Function

    Protected Friend Function RegistrarClienteVentaPresencial(ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idCreador > 0 AndAlso Not String.IsNullOrEmpty(_nombreApellido) AndAlso
            (Not String.IsNullOrEmpty(_telefonoResidencia) OrElse Not String.IsNullOrEmpty(_telefonoOficina) _
            OrElse Not String.IsNullOrEmpty(_celular)) Then

            Dim finalizarObjAccesoDatos As Boolean = False
            Try
                If dbManager Is Nothing Then
                    dbManager = New LMDataAccess
                    finalizarObjAccesoDatos = True
                End If
                With dbManager
                    If _idTipoIdentificacion > 0 Then _
                        .SqlParametros.Add("@idTipoIdentificacion", SqlDbType.SmallInt).Value = _idTipoIdentificacion
                    If Not String.IsNullOrEmpty(_numeroIdentificacion) Then _
                        .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = _numeroIdentificacion
                    .SqlParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = _nombreApellido
                    If Not String.IsNullOrEmpty(_nombres) Then _
                    .SqlParametros.Add("@nombres", SqlDbType.VarChar, 100).Value = _nombres
                    If Not String.IsNullOrEmpty(_primerApellido) Then _
                    .SqlParametros.Add("@primerApellido", SqlDbType.VarChar, 100).Value = _primerApellido
                    If Not String.IsNullOrEmpty(_segundoApellido) Then _
                    .SqlParametros.Add("@segundoApellido", SqlDbType.VarChar, 100).Value = _segundoApellido
                    If Not String.IsNullOrEmpty(_direccionResidencia) Then _
                        .SqlParametros.Add("@direccionResidencia", SqlDbType.VarChar, 100).Value = _direccionResidencia
                    If _idCiudadResidencia > 0 Then _
                        .SqlParametros.Add("@idCiudadResidencia", SqlDbType.Int).Value = _idCiudadResidencia
                    If Not String.IsNullOrEmpty(_telefonoResidencia) Then _
                        .SqlParametros.Add("@telefonoResidencia", SqlDbType.VarChar, 20).Value = _telefonoResidencia
                    If Not String.IsNullOrEmpty(_telefonoAdicional) Then _
                        .SqlParametros.Add("@telefonoAdicional", SqlDbType.VarChar, 20).Value = _telefonoAdicional
                    If Not String.IsNullOrEmpty(_celular) Then _
                        .SqlParametros.Add("@celular", SqlDbType.VarChar, 20).Value = _celular
                    If Not String.IsNullOrEmpty(_sexo) Then _
                        .SqlParametros.Add("@sexo", SqlDbType.VarChar, 1).Value = _sexo
                    If Not String.IsNullOrEmpty(_email) Then _
                        .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = _email
                    If _ingresoAproximado > 0 Then _
                        .SqlParametros.Add("@ingresoAproximado", SqlDbType.Money).Value = _ingresoAproximado
                    If _idEmpresa > 0 Then _
                        .SqlParametros.Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                    If _resultadoProcesoCliente > 0 Then _
                        .SqlParametros.Add("@resultadoProcesoCliente", SqlDbType.Int).Value = _resultadoProcesoCliente
                    .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = _idCreador
                    If Not String.IsNullOrEmpty(_barrioResidencia) Then _
                        .SqlParametros.Add("@barrioResidencia", SqlDbType.VarChar, 100).Value = _barrioResidencia
                    If Not String.IsNullOrEmpty(_direccionOficina) Then _
                        .SqlParametros.Add("@direccionOficina", SqlDbType.VarChar, 100).Value = _direccionOficina
                    If Not String.IsNullOrEmpty(_telefonoOficina) Then _
                        .SqlParametros.Add("@telefonoOficina", SqlDbType.VarChar, 20).Value = _telefonoOficina
                    If _idEstatusLaboral > 0 Then _
                        .SqlParametros.Add("@idEstatusLaboral", SqlDbType.TinyInt).Value = _idEstatusLaboral
                    .SqlParametros.Add("@idCliente", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .EjecutarNonQuery("RegistrarInfoClienteGestion", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        If resultado.Valor = 0 Then
                            _idCliente = CInt(.SqlParametros("@idCliente").Value.ToString)
                            _registrado = True
                        End If
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el registro de los datos del cliente. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If finalizarObjAccesoDatos AndAlso dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para realizar el registro del cliente. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resultado As ResultadoProceso
        Try
            resultado = Registrar(dbManager)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function RegistrarClienteVentaPresencial() As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resultado As ResultadoProceso
        Try
            resultado = RegistrarClienteVentaPresencial(dbManager)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resultado As ResultadoProceso
        Try
            resultado = Actualizar(dbManager)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function ActualizarClienteGestion() As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resultado As ResultadoProceso
        Try
            resultado = ActualizarClienteGestion(dbManager)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function ObtenerDatosDashboard() As DataSet
        Dim dsDatos As New DataSet
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idCampania > 0 Then
                    .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                End If
                dsDatos = .EjecutarDataSet("ObtenerGestionVentasGlobal", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dsDatos
    End Function

    Public Function ObtenerDatosAsesores() As DataTable
        Dim dtDatos As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@NombreSupervisor", SqlDbType.VarChar, 50).Value = Supervisor
                dtDatos = .EjecutarDataTable("ObtenerGestionVentasSupervisor", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
