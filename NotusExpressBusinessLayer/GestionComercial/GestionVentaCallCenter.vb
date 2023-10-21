Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.Comunes
Imports System.String
Imports System.Collections.Generic

Public Class GestionVentaCallCenter

#Region "Atributos"

    Private _idCliente As Integer
    Private _idTipoIdentificacion As Integer
    Private _idEstadoAnimo As Integer
    Private _identificacionCliente As String
    Private _nombresCliente As String
    Private _primerApellido As String
    Private _segundoApellido As String
    Private _idUsuarioAsesor As Integer
    Private _idGestionVenta As Integer
    Private _celular As String
    Private _idProducto As Integer?
    Private _idServicio As Integer
    Private _idEstado As Integer
    Private _idEstrategiaComercial As Integer
    Private _idCiudad As Integer
    Private _direccionResidencia As String
    Private _telefonoResidencia As String
    Private _telefonoAdicional As String
    Private _ingresosAproximados As String
    Private _idResultadoProcesoCliente As Integer
    Private _email As String
    Private _sexo As String
    Private _idValorPrima As String
    Private _valorCupo As Integer
    Private _idJornada As Integer
    Private _idPdv As Integer
    Private _idResultado As Integer
    Private _idCausal As Integer
    Private _observaciones As String
    Private _idGestion As Integer
    Private _idCampania As Integer
    Private _campania As String
    Private _listDocumento As List(Of Integer)
    Private _listDocumentoPresencial As String
    Private _listDocumentoPresenciallist As List(Of Integer)
    Private _idEmpresa As Integer
    Private _resultadoUbica As String
    Private _resultadoEvidente As String
    Private _resultadoDataCredito As String
    Private _numConsultaUbica As String
    Private _numConsultaEvidente As String
    Private _numConsultaDataCredito As String
    Private _consecutivoDataCredito As String
    Private _valorPreaprobadocliente As Integer
    Private _valorPreaprobadoDataCredito As Integer
    Private _registrado As Boolean
    Private _idEstadoServicioMensajeria As Integer
    Private _codOficinaCliente As String
    Private _actividadLaboralCliente As String
    Private _idServicioNotus As Long
    Private _fechaAgenda As Date

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Set(value As Integer)
            _idCliente = value
        End Set
    End Property

    Public Property IdTipoIdentificacion As Integer
        Get
            Return _idTipoIdentificacion
        End Get
        Set(value As Integer)
            _idTipoIdentificacion = value
        End Set
    End Property

    Public Property IdEstadoAnimo As Integer
        Get
            Return _idEstadoAnimo
        End Get
        Set(value As Integer)
            _idEstadoAnimo = value
        End Set
    End Property

    Public Property IdentificacionCliente() As String
        Get
            Return _identificacionCliente
        End Get
        Set(ByVal value As String)
            _identificacionCliente = value
        End Set
    End Property

    Public Property NombresCliente() As String
        Get
            Return _nombresCliente
        End Get
        Set(ByVal value As String)
            _nombresCliente = value
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

    Public Property IdGestionVenta() As Integer
        Get
            Return _idGestionVenta
        End Get
        Set(ByVal value As Integer)
            _idGestionVenta = value
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

    Public Property ConsecutivoDataCredito() As String
        Get
            Return _consecutivoDataCredito
        End Get
        Set(value As String)
            _consecutivoDataCredito = value
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

    Public Property IdProducto() As Integer
        Get
            Return _idProducto
        End Get
        Set(ByVal value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property IdServicio() As Integer
        Get
            Return _idServicio
        End Get
        Set(ByVal value As Integer)
            _idServicio = value
        End Set
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
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

    Public Property IngresosAproximados() As String
        Get
            Return _ingresosAproximados
        End Get
        Set(ByVal value As String)
            _ingresosAproximados = value
        End Set
    End Property

    Public Property IdResultadoProcesoCliente() As Integer
        Get
            Return _idResultadoProcesoCliente
        End Get
        Set(value As Integer)
            _idResultadoProcesoCliente = value
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

    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
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

    Public Property IdValorPrima() As Integer
        Get
            Return _idValorPrima
        End Get
        Set(ByVal value As Integer)
            _idValorPrima = value
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

    Public Property IdResultado() As Integer
        Get
            Return _idResultado
        End Get
        Set(ByVal value As Integer)
            _idResultado = value
        End Set
    End Property

    Public Property IdCausal As Integer
        Get
            Return _idCausal
        End Get
        Set(value As Integer)
            _idCausal = value
        End Set
    End Property

    Public Property IdEstrategiaComercial() As Integer
        Get
            Return _idEstrategiaComercial
        End Get
        Set(ByVal value As Integer)
            _idEstrategiaComercial = value
        End Set
    End Property

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property Observaciones() As String
        Get
            Return _observaciones
        End Get
        Set(ByVal value As String)
            _observaciones = value
        End Set
    End Property

    Public Property IdGestion() As Integer
        Get
            Return _idGestion
        End Get
        Set(ByVal value As Integer)
            _idGestion = value
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

    Public Property Campania As String
        Get
            Return _campania
        End Get
        Set(value As String)
            _campania = value
        End Set
    End Property

    Public Property ListDocumento As List(Of Integer)
        Get
            If _listDocumento Is Nothing Then _listDocumento = New List(Of Integer)
            Return _listDocumento
        End Get
        Set(value As List(Of Integer))
            _listDocumento = value
        End Set
    End Property

    Public Property ListDocumentoPresencialList As List(Of Integer)
        Get
            Return _listDocumentoPresenciallist
        End Get
        Set(value As List(Of Integer))
            _listDocumentoPresenciallist = value
        End Set
    End Property
    Public Property ListDocumentoPresencial As String
        Get
            Return _listDocumentoPresencial
        End Get
        Set(value As String)
            _listDocumentoPresencial = value
        End Set
    End Property

    Public Property ValorPreaprobadocliente As Integer
        Get
            Return _valorPreaprobadocliente
        End Get
        Set(value As Integer)
            _valorPreaprobadocliente = value
        End Set
    End Property

    Public Property ValorPreaprobadoDataCredito As Integer
        Get
            Return _valorPreaprobadoDataCredito
        End Get
        Set(value As Integer)
            _valorPreaprobadoDataCredito = value
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

    Public Property IdEstadoServicioMensajeria As Integer
        Get
            Return _idEstadoServicioMensajeria
        End Get
        Set(value As Integer)
            _idEstadoServicioMensajeria = value
        End Set
    End Property

    Public Property CodOficinaCliente As String
        Get
            Return _codOficinaCliente
        End Get
        Set(value As String)
            _codOficinaCliente = value
        End Set
    End Property

    Public Property ValorCupo As Integer
        Get
            Return _valorCupo
        End Get
        Set(value As Integer)
            _valorCupo = value
        End Set
    End Property

    Public Property ActividadLaboralCliente As String
        Get
            Return _actividadLaboralCliente
        End Get
        Set(value As String)
            _actividadLaboralCliente = value
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

    Public Property FechaAgenda As Date
        Get
            Return _fechaAgenda
        End Get
        Set(value As Date)
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


#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idServicioNotus > 0 Then .SqlParametros.Add("@idServicioNotus", SqlDbType.BigInt).Value = _idServicioNotus
                If _fechaAgenda > Date.MinValue Then .SqlParametros.Add("@fechaAgenda", SqlDbType.Date).Value = _fechaAgenda
                If _idJornada > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                .SqlParametros.Add("@idEstadoAnimo", SqlDbType.Int).Value = _idEstadoAnimo
                .SqlParametros.Add("@idTipoIdentificacion", SqlDbType.Int).Value = _idTipoIdentificacion
                .SqlParametros.Add("@identificacionCliente", SqlDbType.VarChar, 20).Value = _identificacionCliente.Trim
                .SqlParametros.Add("@nombresCliente", SqlDbType.VarChar, 30).Value = _nombresCliente.Trim
                .SqlParametros.Add("@primerApellido", SqlDbType.VarChar, 30).Value = _primerApellido.Trim
                If (_segundoApellido IsNot Nothing) Then
                    .SqlParametros.Add("@segundoApellido", SqlDbType.VarChar, 30).Value = _segundoApellido.Trim
                End If
                .SqlParametros.Add("@celular", SqlDbType.VarChar, 10).Value = _celular.Trim
                .SqlParametros.Add("@idCiudadResidencia", SqlDbType.Int).Value = _idCiudad
                .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                .SqlParametros.Add("@idEstrategiaComercial", SqlDbType.Int).Value = _idEstrategiaComercial
                .SqlParametros.Add("@direccionResidencia", SqlDbType.VarChar, 200).Value = _direccionResidencia
                .SqlParametros.Add("@telefonoResidencia", SqlDbType.VarChar, 20).Value = _telefonoResidencia
                .SqlParametros.Add("@observaciones", SqlDbType.VarChar, 400).Value = _observaciones
                .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = _email
                .SqlParametros.Add("@codOficina", SqlDbType.VarChar, 100).Value = _codOficinaCliente
                .SqlParametros.Add("@ingresoAproximado", SqlDbType.Money).Value = CDbl(_ingresosAproximados)
                .SqlParametros.Add("@valorCupo", SqlDbType.Int).Value = _valorCupo
                .SqlParametros.Add("@idResultado", SqlDbType.Int).Value = _idResultado
                .SqlParametros.Add("@idCausal", SqlDbType.Int).Value = _idCausal
                .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv
                If _idEstadoServicioMensajeria <> 0 Then
                    .SqlParametros.Add("@idEstadoServicioMensajeria", SqlDbType.Int).Value = _idEstadoServicioMensajeria
                End If
                If _idEmpresa > 0 Then .SqlParametros.Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                If _resultadoUbica IsNot Nothing Then .SqlParametros.Add("@resultadoUbica", SqlDbType.VarChar).Value = _resultadoUbica
                If _resultadoEvidente IsNot Nothing Then .SqlParametros.Add("@resultadoEvidente", SqlDbType.VarChar).Value = _resultadoEvidente
                If _resultadoDataCredito IsNot Nothing Then .SqlParametros.Add("@resultadoDataCredito", SqlDbType.VarChar).Value = _resultadoDataCredito
                If _numConsultaUbica IsNot Nothing Then .SqlParametros.Add("@numConsultaUbica", SqlDbType.VarChar).Value = _numConsultaUbica
                If _numConsultaEvidente IsNot Nothing Then .SqlParametros.Add("@numconsultaEvidente", SqlDbType.VarChar).Value = _numConsultaEvidente
                If _numConsultaDataCredito IsNot Nothing Then .SqlParametros.Add("@numConsultaDataCredito", SqlDbType.VarChar).Value = _numConsultaDataCredito
                If _consecutivoDataCredito IsNot Nothing Then .SqlParametros.Add("@ConsecutivoDataCredito", SqlDbType.VarChar).Value = _consecutivoDataCredito
                If _valorPreaprobadoDataCredito > 0 Then .SqlParametros.Add("@cupoPreaprobadoDataCredito", SqlDbType.BigInt).Value = _valorPreaprobadoDataCredito
                'If _idCausal > 0 Then .SqlParametros.Add("@idCausal", SqlDbType.Int).Value = _idCausal
                If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                If _campania IsNot Nothing Then .SqlParametros.Add("@campania", SqlDbType.VarChar, 450).Value = _campania
                If _actividadLaboralCliente IsNot Nothing Then .SqlParametros.Add("@actividadLaboral", SqlDbType.VarChar, 50).Value = _actividadLaboralCliente
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .TiempoEsperaComando = 0
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarGestionVentaCallCenter", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    If resultado.Valor = 0 Or resultado.Valor = 3 Then
                        _registrado = True
                        _idGestionVenta = CInt(.SqlParametros("@idGestionVenta").Value.ToString)
                        resultado.Mensaje = CType(.SqlParametros("@mensaje").Value, String)
                        .ConfirmarTransaccion()
                    Else
                        resultado.Mensaje = CType(.SqlParametros("@mensaje").Value, String)
                        .AbortarTransaccion()
                    End If
                Else
                    resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor de BD. Por favor intente nuevamente")
                    .AbortarTransaccion()
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function RegistrarGestionPresencial() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@identificacionCliente", SqlDbType.VarChar, 20).Value = _identificacionCliente.Trim
                .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .iniciarTransaccion()
                .ejecutarNonQuery("RegistrarGestionVentaPresencial", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    If resultado.Valor = 0 Then
                        _registrado = True
                    End If
                    .confirmarTransaccion()
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                Else
                    resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor de BD. Por favor intente nuevamente")
                    .abortarTransaccion()
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function RegistrarHerramientasCredito() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _idCliente
                .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = _idGestion
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioAsesor
                .SqlParametros.Add("@idResultadoProceso", SqlDbType.Int).Value = _idResultado
                .SqlParametros.Add("@resultadoUbica", SqlDbType.VarChar).Value = _resultadoUbica
                .SqlParametros.Add("@resultadoEvidente", SqlDbType.VarChar).Value = _resultadoEvidente
                .SqlParametros.Add("@resultadoDataCredito", SqlDbType.VarChar).Value = _resultadoDataCredito
                .SqlParametros.Add("@numConsultaUbica", SqlDbType.VarChar).Value = _numConsultaUbica
                .SqlParametros.Add("@numconsultaEvidente", SqlDbType.VarChar).Value = _numConsultaEvidente
                .SqlParametros.Add("@numConsultaDataCredito", SqlDbType.VarChar).Value = _numConsultaDataCredito
                .SqlParametros.Add("@cupoPreaprobadoDataCredito", SqlDbType.BigInt).Value = _valorPreaprobadoDataCredito
                .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                .SqlParametros.Add("@idEstrategia", SqlDbType.Int).Value = _idEstrategiaComercial
                .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv
                .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = _observaciones
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .iniciarTransaccion()
                .ejecutarNonQuery("RegistrarInfoHerramientasCredito", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    If resultado.Valor = 0 Then
                        _idGestionVenta = CInt(.SqlParametros("@idGestionVenta").Value.ToString)
                        _registrado = True
                    End If
                    .confirmarTransaccion()
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                Else
                    resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor de BD. Por favor intente nuevamente")
                    .abortarTransaccion()
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function CancelarGestion() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager

                .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = _idGestion
                .SqlParametros.Add("@observaciones", SqlDbType.VarChar, 400).Value = _observaciones.Trim
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                .iniciarTransaccion()
                .ejecutarNonQuery("CancelarGestionCallCenter", CommandType.StoredProcedure)

                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    .confirmarTransaccion()
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                Else
                    resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor de BD. Por favor intente nuevamente")
                    .abortarTransaccion()
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function RegistrarServiciosTransitorios(Optional ByVal observacion As String = Nothing) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                    .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioAsesor
                    .Add("@identificacionCliente", SqlDbType.VarChar, 20).Value = _identificacionCliente
                    If Not String.IsNullOrEmpty(_observaciones) Then .Add("@observacion", SqlDbType.VarChar, 2000).Value = _observaciones
                    If _valorCupo > 0 Then .Add("@valorCupo", SqlDbType.Int).Value = _valorCupo

                    If _listDocumento IsNot Nothing AndAlso _listDocumento.Count > 0 Then _
                        .Add("@listDocumentos", SqlDbType.VarChar).Value = Join(",", _listDocumento.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())

                    If _listDocumentoPresencial IsNot Nothing AndAlso _listDocumentoPresencial.Count > 0 Then _
                        .Add("@listDocumentos", SqlDbType.VarChar).Value = _listDocumentoPresencial

                    'If _listDocumentoPresencial IsNot Nothing AndAlso _listDocumentoPresencial.Count > 0 Then .Add("@listDocumentos", SqlDbType.VarChar).Value = Join(_listDocumentoPresenciallist.ToArray, ",")


                    If _valorPreaprobadocliente > 0 Then .Add("@idValorPreaprobadoCliente", SqlDbType.BigInt).Value = _valorPreaprobadocliente
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .iniciarTransaccion()
                .ejecutarNonQuery("RegistrarServiciosTransitorios", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    .confirmarTransaccion()
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presento un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function EliminarServiciosTransitorios(ByVal idRegistro) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idRegistro", SqlDbType.BigInt).Value = idRegistro
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .iniciarTransaccion()
                .ejecutarNonQuery("EliminarServiciosTransitorios", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    .confirmarTransaccion()
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presento un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function ConsultarServiciosTransitorios() As DataTable
        Dim dtDatos As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            With .SqlParametros
                If _idUsuarioAsesor > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioAsesor
                .Add("@identificacionCliente", SqlDbType.VarChar, 20).Value = _identificacionCliente
                If _idCampania > 0 Then .Add("@idCampania", SqlDbType.Int).Value = _idCampania
                If _idGestionVenta > 0 Then .Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
            End With
            dtDatos = .ejecutarDataTable("ConsultarServiciosTransitorios", CommandType.StoredProcedure)
        End With
        Return dtDatos
    End Function

    Public Function ConsultarServiciosGestionVenta() As DataTable
        Dim dtDatos As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            With .SqlParametros
                If _idGestionVenta > 0 Then .Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
            End With
            dtDatos = .ejecutarDataTable("ConsultarServiciosGestionVenta", CommandType.StoredProcedure)
        End With
        Return dtDatos
    End Function

    Public Function EliminarServiciosGestionVenta(ByVal idDetalle As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idDetalle", SqlDbType.BigInt).Value = idDetalle
                    .Add("@idUsuario", SqlDbType.BigInt).Value = IdUsuarioAsesor
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .iniciarTransaccion()
                .ejecutarNonQuery("EliminarServiciosGestionVenta", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    .confirmarTransaccion()
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presento un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function RegistrarServiciosGestionVenta()
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                    .Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                    .Add("@identificacionCliente", SqlDbType.VarChar, 20).Value = _identificacionCliente
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .iniciarTransaccion()
                .ejecutarNonQuery("ActualizacionProductosVentasPresenciales", CommandType.StoredProcedure)
                If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    .confirmarTransaccion()
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presento un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function ActualizarInformacionClienteVentaPresencial() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idTipoIdentificacion", SqlDbType.Int).Value = _idTipoIdentificacion
                .SqlParametros.Add("@identificacionCliente", SqlDbType.VarChar, 20).Value = _identificacionCliente.Trim
                .SqlParametros.Add("@nombresCliente", SqlDbType.VarChar, 30).Value = _nombresCliente.Trim
                .SqlParametros.Add("@primerApellido", SqlDbType.VarChar, 30).Value = _primerApellido.Trim
                .SqlParametros.Add("@segundoApellido", SqlDbType.VarChar, 30).Value = _segundoApellido.Trim
                .SqlParametros.Add("@direccionResidencia", SqlDbType.VarChar, 200).Value = _direccionResidencia
                .SqlParametros.Add("@idCiudadResidencia", SqlDbType.Int).Value = _idCiudad
                .SqlParametros.Add("@telefonoResidencia", SqlDbType.VarChar, 20).Value = _telefonoResidencia
                .SqlParametros.Add("@telefonoAdicional", SqlDbType.VarChar, 20).Value = _telefonoAdicional
                .SqlParametros.Add("@celular", SqlDbType.VarChar, 10).Value = _celular.Trim
                .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = _email
                .SqlParametros.Add("@sexo", SqlDbType.VarChar, 1).Value = _sexo.Trim
                .SqlParametros.Add("@ingresoAproximado", SqlDbType.Money).Value = CDbl(_ingresosAproximados)
                .SqlParametros.Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _idCliente

                .SqlParametros.Add("@resultadoUbica", SqlDbType.VarChar).Value = _resultadoUbica
                .SqlParametros.Add("@resultadoEvidente", SqlDbType.VarChar).Value = _resultadoEvidente
                .SqlParametros.Add("@resultadoDataCredito", SqlDbType.VarChar).Value = _resultadoDataCredito
                .SqlParametros.Add("@numConsultaUbica", SqlDbType.VarChar).Value = _numConsultaUbica
                .SqlParametros.Add("@numconsultaEvidente", SqlDbType.VarChar).Value = _numConsultaEvidente
                .SqlParametros.Add("@numConsultaDataCredito", SqlDbType.VarChar).Value = _numConsultaDataCredito
                .SqlParametros.Add("@cupoPreaprobadoDataCredito", SqlDbType.BigInt).Value = _valorPreaprobadoDataCredito
                .SqlParametros.Add("@observaciones", SqlDbType.VarChar, 400).Value = _observaciones

                .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = _idGestion

                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .iniciarTransaccion()
                .ejecutarNonQuery("ActualizarGestionVentaPresencial", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    If resultado.Valor = 0 Then
                        _registrado = True
                    End If
                    .confirmarTransaccion()
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                Else
                    resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor de BD. Por favor intente nuevamente")
                    .abortarTransaccion()
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

#End Region

End Class
