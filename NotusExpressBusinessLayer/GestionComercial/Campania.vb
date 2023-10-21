Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.General
Imports System.String
Imports LMDataAccessLayer
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Net

Public Class Campania

#Region "Atributos"

    Protected Friend _idCampania As Integer
    Protected Friend _nombre As String
    Protected Friend _fechaInicio As Date
    Protected Friend _fechaFin As Date
    Private _esFinanciero As Boolean
    Protected Friend _activo As Nullable(Of Boolean)
    Private _metaCliente As Integer
    Private _metaCallcenter As Integer
    Private _fechaLlegada As String
    Private _listCiudades As List(Of Integer)
    Private _listBodegas As List(Of Integer)
    Private _listProductoExterno As List(Of Integer)
    Private _listDocumentoFinanciero As List(Of Integer)
    Private _listTiposDeServicio As List(Of Integer)
    Private _idClienteExterno As Integer
    Private _idTipoCampania As Integer
    Private _idSistema As Integer
    Protected Friend _crearClienteFueraBase As String


#End Region

#Region "Propiedades"

    Public Property IdCampania As Integer
        Get
            Return _idCampania
        End Get
        Set(value As Integer)
            _idCampania = value
        End Set
    End Property

    Public Property EsFinanciero As Boolean
        Get
            Return _esFinanciero
        End Get
        Set(value As Boolean)
            _esFinanciero = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property FechaInicio As Date
        Get
            Return _fechaInicio
        End Get
        Set(value As Date)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFin As Date
        Get
            Return _fechaFin
        End Get
        Set(value As Date)
            _fechaFin = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

    Public Property MetaCliente As Integer
        Get
            Return _metaCliente
        End Get
        Set(value As Integer)
            _metaCliente = value
        End Set
    End Property

    Public Property MetaCallcenter As Integer
        Get
            Return _metaCallcenter
        End Get
        Set(value As Integer)
            _metaCallcenter = value
        End Set
    End Property

    Public Property FechaLlegada As String
        Get
            Return _fechaLlegada
        End Get
        Set(value As String)
            _fechaLlegada = value
        End Set
    End Property

    Public Property ListCiudades As List(Of Integer)
        Get
            If _listCiudades Is Nothing Then _listCiudades = New List(Of Integer)
            Return _listCiudades
        End Get
        Set(value As List(Of Integer))
            _listCiudades = value
        End Set
    End Property

    Public Property ListBodegas As List(Of Integer)
        Get
            If _listBodegas Is Nothing Then _listBodegas = New List(Of Integer)
            Return _listBodegas
        End Get
        Set(value As List(Of Integer))
            _listBodegas = value
        End Set
    End Property

    Public Property ListProductoExterno As List(Of Integer)
        Get
            If _listProductoExterno Is Nothing Then _listProductoExterno = New List(Of Integer)
            Return _listProductoExterno
        End Get
        Set(value As List(Of Integer))
            _listProductoExterno = value
        End Set
    End Property

    Public Property ListDocumentoFinanciero As List(Of Integer)
        Get
            If _listDocumentoFinanciero Is Nothing Then _listDocumentoFinanciero = New List(Of Integer)
            Return _listDocumentoFinanciero
        End Get
        Set(value As List(Of Integer))
            _listDocumentoFinanciero = value
        End Set
    End Property

    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
        End Set
    End Property

    Public Property ListTiposDeServicio As List(Of Integer)
        Get
            If _listTiposDeServicio Is Nothing Then _listTiposDeServicio = New List(Of Integer)
            Return _listTiposDeServicio
        End Get
        Set(value As List(Of Integer))
            _listTiposDeServicio = value
        End Set
    End Property

    Public Property IdTipoCampania As Integer
        Get
            Return _idTipoCampania
        End Get
        Set(value As Integer)
            _idTipoCampania = value
        End Set
    End Property

    Public Property IdSistema As Integer
        Get
            Return _idSistema
        End Get
        Set(value As Integer)
            _idSistema = value
        End Set
    End Property

    Public Property CrearClienteFueraBase As String
        Get
            Return _crearClienteFueraBase
        End Get
        Set(value As String)
            _crearClienteFueraBase = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idCampania As Integer)
        MyBase.New()
        _idCampania = idCampania
        CargarDatos()
    End Sub

#End Region

#Region "Metodos Publicos"

    Public Function ObtenerClienteExterno() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Wsresultado = objCampania.ConsultarClientesExternos(dsDatos, _esFinanciero, IdClienteExterno)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Public Function ObtenerTipoCampania() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Wsresultado = objCampania.ConsultarTipoCampanias(dsDatos, True)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Public Function ObtenerServicios() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Wsresultado = objCampania.ConsultarServicios(dsDatos, _esFinanciero)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Public Function ObtenerBodegas() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Wsresultado = objCampania.ConsultarBodegas(dsDatos)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Public Function ObtenerProductosComerciales(ByVal pListIdClienteExterno As Integer()) As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Wsresultado = objCampania.ConsultarProductos(dsDatos, pListIdClienteExterno)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Public Function ObtenerDocumentos() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Wsresultado = objCampania.ConsultarDocumentos(dsDatos)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Public Function RegistrarFinanciero() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                    .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    .Add("@idSistemaOrigen", SqlDbType.Int).Value = _idSistema
                    If _fechaFin <> Date.MinValue Then .Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                    .Add("@activo", SqlDbType.Bit).Value = _activo
                    .Add("@metaCliente", SqlDbType.Int).Value = _metaCliente
                    .Add("@metaCallcenter", SqlDbType.Int).Value = _metaCallcenter
                    .Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    .Add("@idTipoCampania", SqlDbType.Int).Value = _idTipoCampania
                    If _fechaLlegada IsNot Nothing Then .Add("@fechaLlegada", SqlDbType.VarChar).Value = _fechaLlegada
                    If _listTiposDeServicio IsNot Nothing AndAlso _listTiposDeServicio.Count > 0 Then _
                     .Add("@listIdTipoServicio", SqlDbType.VarChar).Value = Join(",", _listTiposDeServicio.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    .Add("@crearUsuarioFueraBaseCargue", SqlDbType.Bit).Value = _crearClienteFueraBase
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarCampaniaFinanciero", CommandType.StoredProcedure)

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
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function Sincronizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim wsDatos As New NotusIlsService.WsRegistroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With wsDatos
            .Nombre = _nombre
            .FechaInicio = _fechaInicio
            .IdClienteExterno = _idClienteExterno
            .IdEmpresa = CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue)
            .IdSistema = _idSistema
            .FechaFinGestionCal = _fechaFin
            .FechaFinGestionCem = _fechaFin
            .FechaFinRadicado = _fechaFin
            .Activo = _activo
            .MetaCliente = _metaCliente
            .MetaCallcenter = _metaCallcenter
            .FechaLlegada = _fechaLlegada
            .IdTipoCampania = _idTipoCampania
            If _listBodegas IsNot Nothing AndAlso _listBodegas.Count > 0 Then _
                .ListBodegas = _listBodegas.ToArray
            If _listProductoExterno IsNot Nothing AndAlso _listProductoExterno.Count > 0 Then _
                .ListProductoExterno = _listProductoExterno.ToArray
            If _listDocumentoFinanciero IsNot Nothing AndAlso _listDocumentoFinanciero.Count > 0 Then _
                .ListDocumentoFinanciero = _listDocumentoFinanciero.ToArray
            If _listTiposDeServicio IsNot Nothing AndAlso _listTiposDeServicio.Count > 0 Then _
                .ListTiposDeServicio = _listTiposDeServicio.ToArray
        End With

        Wsresultado = objCampania.RegistrarCampaniaWS(wsDatos)
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje
        Return resultado
    End Function

    Public Function ActualizarFinanciero() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If Not String.IsNullOrEmpty(_nombre) Then .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    If _fechaInicio > Date.MinValue Then .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                    If _idClienteExterno > 0 Then .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    If _fechaFin <> Date.MinValue Then .Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                    .Add("@activo", SqlDbType.Bit).Value = _activo
                    .Add("@metaCliente", SqlDbType.Int).Value = _metaCliente
                    .Add("@metaCallcenter", SqlDbType.Int).Value = _metaCallcenter
                    .Add("@idTipoCampania", SqlDbType.Int).Value = _idTipoCampania
                    .Add("@idSistemaOrigen", SqlDbType.Int).Value = _idSistema
                    If _fechaLlegada IsNot Nothing Then .Add("@fechaLlegada", SqlDbType.VarChar).Value = _fechaLlegada
                    If _listTiposDeServicio IsNot Nothing AndAlso _listTiposDeServicio.Count > 0 Then _
                        .Add("@listIdTipoServicio", SqlDbType.VarChar).Value = Join(",", _listTiposDeServicio.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    .Add("@crearUsuarioFueraBaseCargue", SqlDbType.Bit).Value = _crearClienteFueraBase
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("ActualizarCampaniaFinanciero", CommandType.StoredProcedure)

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
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function SincronizarActualizacion() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim wsDatos As New NotusIlsService.WsRegistroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With wsDatos
            .IdCampania = _idCampania
            .Nombre = _nombre
            .FechaInicio = _fechaInicio
            .IdClienteExterno = _idClienteExterno
            .IdSistema = _idSistema
            .FechaFinGestionCal = _fechaFin
            .FechaFinGestionCem = _fechaFin
            .FechaFinRadicado = _fechaFin
            Dim Empresa As New ConfigValues("ID_EMPRESA")
            If (Empresa Is Nothing) Then
                Throw New System.Exception("No fué posible Encontra comfiguracion de empresa en ConfigValues ID_EMPRESA Notificar a IT ")
                Exit Function
            End If
            .IdEmpresa = CInt(Empresa.ConfigKeyValue)
            .Activo = _activo
            .MetaCliente = _metaCliente
            .MetaCallcenter = _metaCallcenter
            .FechaLlegada = _fechaLlegada
            .IdTipoCampania = _idTipoCampania
            If _listBodegas IsNot Nothing AndAlso _listBodegas.Count > 0 Then _
                .ListBodegas = _listBodegas.ToArray
            If _listProductoExterno IsNot Nothing AndAlso _listProductoExterno.Count > 0 Then _
                .ListProductoExterno = _listProductoExterno.ToArray
            If _listDocumentoFinanciero IsNot Nothing AndAlso _listDocumentoFinanciero.Count > 0 Then _
                .ListDocumentoFinanciero = _listDocumentoFinanciero.ToArray
            If _listTiposDeServicio IsNot Nothing AndAlso _listTiposDeServicio.Count > 0 Then _
                .ListTiposDeServicio = _listTiposDeServicio.ToArray
        End With

        Wsresultado = objCampania.ActualizarCampaniaWS(wsDatos)
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje
        Return resultado
    End Function

    Public Function ConsultaAutocomplete(Operacion As Integer, Filtro As String) As DataSet

        Dim dbManager As New LMDataAccess
        Dim dsDatosCodigoEstrategia As New DataSet
        With dbManager
            .SqlParametros.Add("@Operacion", SqlDbType.VarChar).Value = Operacion
            .SqlParametros.Add("@Filtro", SqlDbType.VarChar).Value = Filtro
            dsDatosCodigoEstrategia = .EjecutarDataSet("ConsultaAutoComplete", CommandType.StoredProcedure)
        End With
        Return dsDatosCodigoEstrategia
    End Function

    Public Function GuardarNuevoCodigoEstrategia(CodigoEst As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros
                    .Add("@NuevoCodigo", SqlDbType.VarChar).Value = CodigoEst
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("GuardarNuevoCodEstrategia", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
            resultado = WsGuardarNuevoCodigoEstrategia(CodigoEst)
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString() & "|rojo")
        End Try
        Return resultado
    End Function

    Public Function WsGuardarNuevoCodigoEstrategia(ByVal codEstrategia As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim wsDatos As New NotusIlsService.WsRegistroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With wsDatos
            .CodEstrategia = codEstrategia
        End With
        Try
            Wsresultado = objCampania.GuardarCodigoEstrategiaWs(wsDatos)
            resultado.Valor = Wsresultado.Valor
            resultado.Mensaje = Wsresultado.Mensaje
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString() & "|rojo")
        End Try

        Return resultado
    End Function

    Public Function ActualizarCodigoEstrategia(CodigoAnterior As String, CodigoNuevo As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros
                    .Add("@CodigoAnterior", SqlDbType.VarChar).Value = CodigoAnterior
                    .Add("@CodigoNuevo", SqlDbType.VarChar).Value = CodigoNuevo
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("ActualizarCodEstrategia", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
            resultado = WsActualizarCodigoEstrategia(CodigoAnterior, CodigoNuevo)
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString() & "|rojo")
        End Try
        Return resultado
    End Function

    Public Function WsActualizarCodigoEstrategia(ByVal CodigoAnterior As String, ByVal CodigoNuevo As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim wsDatos As New NotusIlsService.WsRegistroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With wsDatos
            .CodEstrategia = CodigoAnterior
            .CodigoEstrategiaActualizar = CodigoNuevo
        End With
        Try
            Wsresultado = objCampania.ActualizarCodigoEstrategiaWs(wsDatos)
            resultado.Valor = Wsresultado.Valor
            resultado.Mensaje = Wsresultado.Mensaje
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString() & "|rojo")
        End Try

        Return resultado
    End Function

    Public Function EliminarCodigoEstrategia(CodigoEst As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros
                    .Add("@CodigoEliminar", SqlDbType.VarChar).Value = CodigoEst
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("EliminarCodEstrategia", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
            resultado = WsEliminarCodigoEstrategia(CodigoEst)
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString() & "|rojo")
        End Try

        Return resultado
    End Function

    Public Function WsEliminarCodigoEstrategia(ByVal CodigoEst As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim objCampania As New NotusIlsService.NotusIlsService
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim wsDatos As New NotusIlsService.WsRegistroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With wsDatos
            .CodEstrategia = CodigoEst
        End With
        Try
            Wsresultado = objCampania.EliminarCodigoEstrategiaWs(wsDatos)
            resultado.Valor = Wsresultado.Valor
            resultado.Mensaje = Wsresultado.Mensaje
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString() & "|rojo")
        End Try

        Return resultado
    End Function

    Public Function AsociarEstrategiaCampania(idCampania As Integer, codEstrategia As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros
                    .Add("@idCampania", SqlDbType.Int).Value = idCampania
                    .Add("@CodEstrategia", SqlDbType.VarChar).Value = codEstrategia
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("AsociarEstrategiaCampania", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
            resultado = WsAsociarEstrategiaCampania(idCampania, codEstrategia)
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString() & "|rojo")
        End Try
        Return resultado
    End Function

    Public Function WsAsociarEstrategiaCampania(ByVal idCampania As Integer, ByVal codEstrategia As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim wsDatos As New NotusIlsService.WsRegistroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With wsDatos
            .IdCampania = idCampania
            .CodEstrategia = codEstrategia
        End With
        Try
            Wsresultado = objCampania.AsociarCodigoEstrategiaWs(wsDatos)
            resultado.Valor = Wsresultado.Valor
            resultado.Mensaje = Wsresultado.Mensaje
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString() & "|rojo")
        End Try

        Return resultado
    End Function

    Public Function ConsultarCampaniaGestionCallCenterComboBox(campaniaAactiva As Boolean, tipoServicio As String) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dsDatosCampania As New DataTable
        With dbManager
            .SqlParametros.Add("@activo", SqlDbType.VarChar).Value = campaniaAactiva
            .SqlParametros.Add("@listIdTipoServicio", SqlDbType.VarChar).Value = tipoServicio
            dsDatosCampania = .EjecutarDataTable("ObtieneCampaniasVentas", CommandType.StoredProcedure)
        End With

        Return dsDatosCampania
    End Function

    Public Function ConsultarCampaniaPermiteCrearClienteFueraBase(idCampania As Integer) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dsDatosCampania As New DataTable
        With dbManager
            .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = idCampania
            dsDatosCampania = .EjecutarDataTable("ObtieneCampaniasVentas", CommandType.StoredProcedure)
        End With
        Return dsDatosCampania
    End Function



#End Region

#Region "Metodos Privados"

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombreCampania", SqlDbType.VarChar).Value = _nombre
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    .ejecutarReader("ObtieneCampaniasVentas", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idCampania").ToString, _idCampania)
                            _nombre = .Reader("nombre").ToString
                            _fechaInicio = .Reader("fechaInicio")
                            If Not IsDBNull(.Reader("fechaFin")) Then _fechaFin = .Reader("fechaFin")
                            _activo = .Reader("activo")
                            If Not String.IsNullOrEmpty(.Reader("esFinanciero")) Then Integer.TryParse(.Reader("esFinanciero").ToString, _esFinanciero)
                            If Not String.IsNullOrEmpty(.Reader("idClienteExterno")) Then Integer.TryParse(.Reader("idClienteExterno").ToString, _idClienteExterno)
                            _metaCliente = .Reader("metaCliente")
                            _metaCallcenter = .Reader("metaCallcenter")
                            Integer.TryParse(.Reader("idTipoCampania").ToString, _idTipoCampania)
                            _fechaLlegada = .Reader("fechaLLegadaProducto")
                            If Not String.IsNullOrEmpty(.Reader("permiteCrearClienteFueraBase")) Then Integer.TryParse(.Reader("permiteCrearClienteFueraBase").ToString, _crearClienteFueraBase)
                        End If
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

End Class
