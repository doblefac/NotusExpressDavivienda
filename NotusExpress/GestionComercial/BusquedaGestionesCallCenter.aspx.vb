Imports DevExpress.Web
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.WMS
Imports System.Xml
Imports System.IO
Imports iTextSharp.text.pdf
Imports System.Net

Public Class BusquedaGestionesCallCenter
    Inherits System.Web.UI.Page

    Private rol As Integer
    Private idPerfil As Integer
    Private _rutaOrigenArchivo As String = String.Empty


#Region "Atributos"
    Private _nombreArchivo As String
    Private _idServicio As String
#End Region

#Region "Propiedades"

    Public Property NombreArchivo As String
        Get
            Return _nombreArchivo
        End Get
        Set(value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public Property IdServicio As String
        Get
            Return _idServicio
        End Get
        Set(value As String)
            _idServicio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If DEBUG Then
        Session("userId") = 1
        Session("idRol") = 2
#End If



        idPerfil = Session("idPerfil")
        rol = Session("idRol")



        If Not Me.IsPostBack Then
            epNotificador.setTitle("Búsqueda General de Ventas Call Center")
            Session.Remove("listaBusqueda")
            deFechaInicio.MaxDate = DateTime.Now.Date
            deFechaFin.MaxDate = DateTime.Now.Date
            CargarListadoEstados()
            CargarListadoUsuarios()
            CargarListadoEstrategias()
            CargarListadoEstadosExternos()
            txtFiltroIdentificacion.Focus()
            CargarListadoEstadosCalidad()
        End If
        If cbFiltroUsuario.IsCallback Then
            CargarListadoUsuarios()
        End If
        If cmbCiudad1.IsCallback Then
            CargarDatos()
        End If
    End Sub



    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Dim resultado As New ResultadoProceso
        Dim result As Integer = 0
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "filtrarDatos"
                    CargarListadoDeServiciosRegistrados(True)
                Case "Registrar"
                    resultado = ValidarProducto(CLng(arrayAccion(1)))
                    If resultado.Valor = 0 Then
                        resultado = RegistrarProductos(CLng(arrayAccion(1)))
                    End If
                    If resultado.Valor = 0 Then
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                        result = 1
                    Else
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                        result = 2
                    End If
                    CargarListadoDeServiciosRegistrados(True)
                Case "Eliminar"
                    resultado = EliminarProducto(CLng(arrayAccion(1)))
                    If resultado.Valor = 0 Then
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                        result = 1
                    Else
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                        result = 2
                    End If
                    CargarListadoDeServiciosRegistrados(True)
                Case "Editar"

                    resultado = ValidarCiudad(CLng(arrayAccion(1)))
                    If resultado.Valor = 0 Then

                        resultado = ModificarDatosServicio(CLng(arrayAccion(1)))

                        If resultado.Valor = 0 Then
                            resultado = GestionVentaCalidad(CLng(arrayAccion(1)))
                        End If
                    End If
                    If resultado.Valor = 0 Then
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                        imgAgrega.ClientEnabled = False
                        result = 1
                    Else
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                        result = 2
                        imgAgrega.ClientEnabled = True
                    End If
                    CargarListadoDeServiciosRegistrados(True)

                Case "ImprimirFormulario"
                    'Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=arrayAccion(1))
                    'Dim infoClienteFinal As New ClienteFinal()
                    GenerarYDescargarSolicitudCredito(arrayAccion(1))
                    'With infoClienteFinal
                    '    DescargarFormularioVentaPDF(arrayAccion(1))
                    'End With
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack: " & ex.Message)
        End Try
        rpFiltros.Enabled = Not dialogoVerCancelar.ShowOnPageLoad
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje
        CType(sender, ASPxCallbackPanel).JSProperties("cpResultado") = result
        CType(sender, ASPxCallbackPanel).JSProperties("cpRutaArchivo") = _rutaOrigenArchivo
    End Sub

    Private Sub dialogoVerCancelar_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles dialogoVerCancelar.WindowCallback
        Dim resultado As New ResultadoProceso
        Dim result As Integer = 0
        Dim arrayAccion As String()
        arrayAccion = e.Parameter.Split(":")
        Select Case arrayAccion(0)
            Case "Inicial"
                meJustificacion.Text = ""
                meJustificacion.Focus()
                lblIdGestion.Text = arrayAccion(1)
            Case "Cancelar"
                resultado = CancelarServicio(CLng(arrayAccion(1)))
                If resultado.Valor = 0 Then
                    mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                    result = 1
                Else
                    mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                    result = 2
                End If
        End Select
        CType(source, ASPxPopupControl).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje
        CType(source, ASPxPopupControl).JSProperties("cpResultado") = result
    End Sub

    Private Sub pcServicio_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles pcServicio.WindowCallback
        Dim resultado As New ResultadoProceso
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Inicial"

                    CargarDatosServicio()
                    limpiarControlesServicio()
                    lblIdGestionVenta.Text = arrayAccion(1)
                Case "Productos"
                    CargarDatosProducto(arrayAccion(1))
                    CargarDatosServicio()
                    lblIdGestionVenta.Text = arrayAccion(2)
            End Select
            cmbTipoServicio.Focus()
        Catch ex As Exception
            mensajero.MostrarMensajePopUp("Ocurrio un error al generar el servicio: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Error")
        End Try
        CType(source, ASPxPopupControl).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje
        CType(source, ASPxPopupControl).JSProperties("cpResultado") = resultado.Valor
    End Sub

    Private Sub dialogoEditar_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles dialogoEditar.WindowCallback
        Dim resultado As New ResultadoProceso
        Dim arrayAccion As String()
        arrayAccion = e.Parameter.Split(":")
        Select Case arrayAccion(0)
            Case "InicialEdita"

                Dim dtCiudad As New DataTable
                Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=arrayAccion(1))
                Dim idCampania As Integer = miGestion.IdCampaniaNotus

                dtCiudad = ObtenerCiudadCampania(idCampania)
                CargarComboDX(cmbCiudad1, dtCiudad, "IdCiudad", "CiudadDepartamento")

                'cmbCiudad1.ClientEnabled = True
                'txtDireccionResidencia.ClientEnabled = True

                resultado = CargarCliente(arrayAccion(1))
                'CargarDatos()
                CargarListadoEstadosCalidad()


                txtNumIdentificacion.Enabled = False
                lblIdGestionEdit.Text = arrayAccion(1)

                Session("IdGestionEdit") = arrayAccion(1)

                cmbEstadoCalidad.SelectedIndex = -1
                cmbEstadoRechazo.SelectedIndex = -1
                txtObservacionCalidad.Value = ""

                If (CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 11 Or CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 2) Then
                    cmbCiudad1.ClientEnabled = True
                    txtDireccionResidencia.ClientEnabled = True
                End If


                cmbEstadoCalidad.Focus()
            Case "ValidaResultado"

                If cmbEstadoCalidad.Value = 234 Then
                    CargarListadoRechazoCalidad()
                    cmbEstadoRechazo.ClientEnabled = True
                    cmbEstadoRechazo.Focus()
                Else
                    cmbEstadoRechazo.SelectedIndex = -1
                    cmbEstadoRechazo.ClientEnabled = False
                    txtObservacionCalidad.Focus()
                End If

                cbModificarDireccion.ClientVisible = CBool(Session("esDireccionModificada"))
                lblIdGestionEdit.Text = Session("IdGestionEdit")


        End Select
    End Sub

    Protected Sub Link_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim idServicio As Integer
        Dim idEstadoNotus As Integer
        Dim idResultadoProceso As Integer
        Dim NumeroDocumento As String

        Try
            Dim linkVer As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkVer.NamingContainer, GridViewDataItemTemplateContainer)
            Dim lnkEliminar As ASPxHyperLink = templateContainer.FindControl("lnkEliminar")
            Dim lnkAgendar As ASPxHyperLink = templateContainer.FindControl("lnkAgendar")
            Dim lnkAgregar As ASPxHyperLink = templateContainer.FindControl("lnkAgregar")
            Dim lnkEditar As ASPxHyperLink = templateContainer.FindControl("lnkEditar")
            Dim lnkCalidad As ASPxHyperLink = templateContainer.FindControl("lnkCalidad")
            Dim linkEditCalidad As ASPxHyperLink = templateContainer.FindControl("linkEditCalidad")

            idServicio = CInt(gvVentas.GetRowValuesByKeyValue(templateContainer.KeyValue, "IdServicioNotus"))
            idEstadoNotus = CInt(gvVentas.GetRowValuesByKeyValue(templateContainer.KeyValue, "IdEstadoServicioMensajeria"))
            idResultadoProceso = CInt(gvVentas.GetRowValuesByKeyValue(templateContainer.KeyValue, "IdResultadoProceso"))

            linkVer.ClientSideEvents.Click = linkVer.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)

            If Session("idRol") = 16 Then 'rol = 15 
                cmbEstadoCalidad.Visible = True
                lnkEditar.Visible = True
            Else
                lnkEditar.Visible = False
            End If

            If idEstadoNotus = Enumerados.EstadosServicioMensajeria.Confirmado Or
                idEstadoNotus = Enumerados.EstadosServicioMensajeria.Anulado Or
                idEstadoNotus = Enumerados.EstadosServicioMensajeria.RechazadoCalidadContactCenter Or
                idEstadoNotus = Enumerados.EstadosServicioMensajeria.Rechazado Then
                lnkEliminar.Visible = False
                lnkAgendar.Visible = False
                lnkAgregar.Visible = False
                lnkEditar.Visible = False
            ElseIf (Session("idRol") = 17) Then
                lnkEliminar.Visible = False
                lnkAgendar.Visible = False
                lnkAgregar.Visible = False
                lnkEditar.Visible = False
            ElseIf idEstadoNotus = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad And Session("idRol") = 3 And CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 11 Then
                lnkEliminar.Visible = True
                lnkAgendar.Visible = True
                lnkAgregar.Visible = True
                lnkEditar.Visible = True
            ElseIf idEstadoNotus = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad And Session("idRol") = 15 Or (Session("idRol") = 16) Or Session("idRol") = 1 Or Session("idRol") = 4 Or Session("idRol") = 9 Then
                lnkEliminar.Visible = True
                lnkAgendar.Visible = True
                lnkAgregar.Visible = True
                lnkEditar.Visible = True
            ElseIf idEstadoNotus = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad And (Session("idRol") = 16) Or Session("idRol") = 1 Or Session("idRol") = 4 Or Session("idRol") = 9 Then
                lnkEliminar.Visible = True
                lnkAgendar.Visible = True
                lnkAgregar.Visible = True
                lnkEditar.Visible = False
            ElseIf idEstadoNotus = Enumerados.EstadosServicioMensajeria.Devolución Then
                lnkAgendar.Visible = True
                lnkAgregar.Visible = False
                lnkEditar.Visible = False
            ElseIf idServicio = 0 Then
                lnkEliminar.Visible = True
                lnkAgendar.Visible = True
                lnkAgregar.Visible = True
                lnkEditar.Visible = True
            Else
                lnkEliminar.Visible = False
                lnkAgendar.Visible = True
                lnkAgregar.Visible = True
                lnkEditar.Visible = False
            End If

        Catch ex As Exception
            epNotificador.showError("Se presento un error al generar los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub

    Protected Sub Link_InitDetalle(ByVal sender As Object, ByVal e As EventArgs)
        Dim IdestadoNotus As Integer
        Dim IdGestion As Integer
        Dim IdServicio As Integer
        Try
            Dim linkEditar As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkEditar.NamingContainer, GridViewDataItemTemplateContainer)
            linkEditar.ClientSideEvents.Click = linkEditar.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)
            Dim adicion As ASPxHyperLink = templateContainer.FindControl("lnkEliminarDetalle")
            Dim objDetalle As New GestionComercial.GestionDeVentaDetalle(templateContainer.KeyValue)
            IdGestion = objDetalle.IdGestionVenta
            Dim objGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=IdGestion)
            IdestadoNotus = objGestion.IdEstadoServicioMensajeria
            IdServicio = objGestion.IdServicioNotus
            If (Session("idRol") Is Nothing) Then
                If Session("userId") Is Nothing Then Response.Redirect("~/Login.aspx?err=true&codErr=5", False)
                If Request.UrlReferrer Is Nothing Then Response.Redirect("~/Administracion/index.aspx", False)
                If Request.UrlReferrer IsNot Nothing AndAlso Request.UrlReferrer.ToString.ToLower.Contains("login.aspx") AndAlso
                Not Request.Url.AbsolutePath.ToLower.Contains("index.aspx") Then Response.Redirect("~/Login.aspx?err=true&codErr=4", False)
            End If

            If ((IdestadoNotus = Enumerados.EstadosServicioMensajeria.Confirmado Or IdestadoNotus = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad) And (Session("idRol") = 16) Or Session("idRol") = 1 Or Session("idRol") = 4 Or Session("idRol") = 9) Then
                adicion.Visible = True
            ElseIf (IdestadoNotus = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad Or IdestadoNotus = Enumerados.EstadosServicioMensajeria.DevoluciónCallCenter) Then
                adicion.Visible = True
            Else
                adicion.Visible = False
            End If

        Catch ex As Exception
            epNotificador.showError("No fué posible establecer los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub

    Protected Sub gvDetalle_DataSelect(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Session("IdGestionVenta") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
            CargarDetalle(TryCast(sender, ASPxGridView))
        Catch ex As Exception
            epNotificador.showError("Se presento un error al consultar el detalle de la venta: " & ex.Message)
        End Try
    End Sub

    Private Sub gvVentas_DataBinding(sender As Object, e As System.EventArgs) Handles gvVentas.DataBinding
        gvVentas.DataSource = Session("dtDatos")
    End Sub

    Private Sub cmbEstadoCalidad_DataBinding(sender As Object, e As EventArgs) Handles cmbEstadoCalidad.DataBinding
        cmbEstadoCalidad.DataSource = Session("dtCalidad")
    End Sub

    Private Sub cmbEstadoRechazo_DataBinding(sender As Object, e As EventArgs) Handles cmbEstadoRechazo.DataBinding
        cmbEstadoRechazo.DataSource = Session("dtRechazo")
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarListadoEstados()
        Try
            Dim infoEstado As New ResultadoProcesoVentaColeccion
            infoEstado.GestionCallCenter = True
            infoEstado.CargarDatos()
            With cbFiltroEstado
                .DataSource = infoEstado
                .TextField = "descripcion"
                .ValueField = "idResultado"
                .DataBindItems()
            End With
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de estados. ")
        End Try
    End Sub

    Private Sub CargarListadoUsuarios()
        Try
            Dim listaAsesor As AsesorComercialColeccion
            listaAsesor = New AsesorComercialColeccion
            With listaAsesor
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                .IdEstado = 1
                .CargarDatos()
            End With

            With cbFiltroUsuario
                .DataSource = listaAsesor
                .TextField = "NombreAsesor"
                .ValueField = "IdUsuarioSistema"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Asesores. ")
        End Try
    End Sub

    Private Sub CargarListadoEstrategias()
        Try
            Dim infoEstrategia As New EstrategiaComercialColeccion
            infoEstrategia.CargarDatos()
            With cbFiltroEstrategia
                .DataSource = infoEstrategia
                .TextField = "nombre"
                .ValueField = "idEstrategia"
                .DataBindItems()
            End With
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de estrategias. ")
        End Try
    End Sub

    Private Sub CargarListadoEstadosExternos()
        Try
            Dim infoEstados As New EstadosExternosColeccion
            infoEstados.CargarDatos()
            With cmbEstadoNotus
                .DataSource = infoEstados
                .TextField = "NombreEstado"
                .ValueField = "IdEstado"
                .DataBindItems()
            End With
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de estados calidad.")
        End Try
    End Sub

    Private Sub CargarListadoEstadosCalidad()
        Try
            Dim dtCalidad As DataTable
            Dim infoEstados As New EstadosExternosColeccion
            With infoEstados
                dtCalidad = .ObtenerEstadoCalidad
            End With

            Session("dtCalidad") = dtCalidad

            With cmbEstadoCalidad
                .DataSource = Session("dtCalidad")
                .TextField = "nombreEstado"
                .ValueField = "idEstado"
                .DataBind()
            End With
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de estados. ")
        End Try
    End Sub

    Private Sub CargarListadoRechazoCalidad()
        Try
            Dim dtRechazo As DataTable
            Dim infoEstados As New EstadosExternosColeccion
            With infoEstados
                dtRechazo = .ObtenerEstadoRechazoCalidad
            End With

            Session("dtRechazo") = dtRechazo

            With cmbEstadoRechazo
                .DataSource = Session("dtRechazo")
                .TextField = "nombreResultado"
                .ValueField = "idResultado"
                .DataBind()
            End With
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de estados. ")
        End Try
    End Sub

    Private Sub CargarListadoDeServiciosRegistrados(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable = Nothing
        Try
            'Dim ruta As String = "\\colbogsa025\portales\NotusExpressDavivienda\GestionComercial\Archivos\FormatoSolicitudCreditoPNFIN_5221256665.pdf"
            'System.Diagnostics.Process.Start(ruta)
            'DescargarFormularioVentaPDF(txtFiltroIdentificacion.Text)

            Dim infoReporte As New GestionDeVentaColeccion
            With infoReporte
                If deFechaInicio.Date > Date.MinValue Then .FechaInicio = deFechaInicio.Date
                If deFechaFin.Date > Date.MinValue Then .FechaFin = deFechaFin.Date
                If cbFiltroEstrategia.Value > 0 Then .IdEstrategiaComercial.Add(CInt(cbFiltroEstrategia.Value))
                If cbFiltroEstado.Value > 0 Then .ListIdResultadoProceso.Add(CInt(cbFiltroEstado.Value))
                If cbFiltroUsuario.Value > 0 Then .IdAsesor.Add(CInt(cbFiltroUsuario.Value))
                If Len(txtFiltroIdentificacion.Text.Trim) > 0 Then .IdentificacionCliente.Add(txtFiltroIdentificacion.Text.Trim)
                If cmbEstadoNotus.Value > 0 Then .ListIdEstadoNotus.Add(CInt(cmbEstadoNotus.Value))
                If Len(txtFiltroIdOportunidad.Text.Trim) > 0 Then .IdOportunidad.Add(txtFiltroIdOportunidad.Text.Trim)
                .IdUsuarioConsulta = Integer.Parse(Session("userId").ToString())
                .ListIdPdv.Add(63)
                dtDatos = .GenerarDataTable()
            End With
            Session("dtDatos") = dtDatos
            With gvVentas
                .DataSource = dtDatos
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la información del reporte: " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDetalle(gv As ASPxGridView)
        If Session("IdGestionVenta") IsNot Nothing Then
            Dim dtDetalle As New DataTable
            Dim idGestionVenta As Long = CLng(Session("IdGestionVenta"))
            dtDetalle = ObtenerDetalle(idGestionVenta)
            Session("dtDetalle") = dtDetalle
            With gv
                .DataSource = Session("dtDetalle")
            End With
        Else
            epNotificador.showWarning("No se pudo establecer el identificador de la gestión, por favor intente nuevamente.")
        End If
    End Sub

    Private Function ObtenerDetalle(ByVal idGestion As Long) As DataTable
        Dim dtResultado As New DataTable
        Dim objGestion As New GestionComercial.GestionDeVentaDetalleColeccion

        Try
            With objGestion
                .listGestionVenta.Add(idGestion)
                dtResultado = .GenerarDataTable()
            End With
        Catch ex As Exception
            epNotificador.showError("Se presento un error al cargar los servicios: " & ex.Message)
        End Try
        Return dtResultado
    End Function

    Private Function CancelarServicio(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestionVenta)

        If miGestion.IdServicioNotus <> 0 Then
            Dim objService As New NotusIlsService.NotusIlsService
            Dim infoWs As New InfoUrlService(objService, True)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
            Dim WSInfoServicio As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
            Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

            With WSInfoServicio
                .IdCampania = miGestion.IdCampaniaNotus
                .IdServicioMensajeria = miGestion.IdServicioNotus
                .IdEstado = NotusIlsService.EstadoServicio.AnuladoPorSistemaDevCallCenter
                Wsresultado = objService.ActualizarServicioWS(WSInfoServicio)
            End With
            resultado.Valor = Wsresultado.Valor
            resultado.Mensaje = Wsresultado.Mensaje
            If Wsresultado.Valor = 0 Then
                resultado = CancelarGestionVenta(idGestionVenta)
            End If
        Else
            resultado = CancelarGestionVenta(idGestionVenta)
        End If
        Return resultado
    End Function


    Public Function ValidarCiudad(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestionVenta)
        Dim idCampania As Integer = miGestion.IdCampaniaNotus
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim WSInfoServicio As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With WSInfoServicio
            .IdCampania = idCampania
            .IdCiudad = CInt(cmbCiudad1.Value)
            Wsresultado = objService.ValidaRegistrosServicioWS(WSInfoServicio)
        End With
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje

        Return resultado
    End Function

    Private Function CancelarGestionVenta(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta
        With miGestion
            .IdGestionVenta = idGestionVenta
            .IdModificador = CInt(Session("userId"))
            .ObservacionDeclinar = meJustificacion.Text
            .IdEstado = Enumerados.EstadosGestionDeVenta.VentaDeclinada
            .IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.Anulado
            resultado = .Actualizar()
        End With
        Return resultado
    End Function

    Private Function ModificarDatosServicio(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestionVenta)

        If miGestion.IdServicioNotus <> 0 Then
            Dim objService As New NotusIlsService.NotusIlsService
            Dim infoWs As New InfoUrlService(objService, True)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
            Dim WSInfoServicio As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
            Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

            With WSInfoServicio
                .IdServicioMensajeria = miGestion.IdServicioNotus
                .IdCampania = miGestion.IdCampaniaNotus
                .Nombre = txtNombres.Text.Trim & " " & txtPrimerApellido.Text.Trim & " " & txtSegundoApellido.Text.Trim
                .Identicacion = txtNumIdentificacion.Text.Trim
                .IdCiudad = cmbCiudad1.Value
                .Direccion = txtDireccionResidencia.Text.Trim

                With WSInfoServicio
                    .IdServicioMensajeria = miGestion.IdServicioNotus
                    If cmbEstadoCalidad.Value = 233 Then
                        .IdEstado = NotusIlsService.EstadoServicio.Confirmado
                    ElseIf cmbEstadoCalidad.Value = 234 Then
                        .IdEstado = NotusIlsService.EstadoServicio.RechazadoCalidadContactCenter
                    ElseIf cmbEstadoCalidad.Value = 165 Then
                        .IdEstado = NotusIlsService.EstadoServicio.DevueltoCallCenter
                    End If
                    Wsresultado = objService.ActualizarServicioWS(WSInfoServicio)

                    If cmbEstadoCalidad.Value = 234 AndAlso Wsresultado.Valor = 0 Then
                        CancelarGestionVenta(idGestionVenta)
                    End If
                End With

                If Not String.IsNullOrEmpty(txtCelular.Text) Then
                    .Telefono = txtCelular.Text.Trim
                ElseIf Not String.IsNullOrEmpty(txtTelFijo.Text) Then
                    .Telefono = txtTelFijo.Text.Trim
                End If

            End With
            resultado.Valor = Wsresultado.Valor
            resultado.Mensaje = Wsresultado.Mensaje
            If Wsresultado.Valor = 0 Then
                resultado = ModificarClienteGestion(idGestionVenta)
            End If
        Else
            resultado = ModificarClienteGestion(idGestionVenta)
            resultado.Valor = 1
            resultado.Mensaje = "No fue posible realizar la gestion de la venta, ID servicio notus 0"
        End If
        Return resultado
    End Function

    Private Function GestionVentaCalidad(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta()
        With miGestion
            .IdModificador = CInt(Session("userId"))
            .IdGestionVenta = idGestionVenta
            .IdEstadoCalidad = cmbEstadoCalidad.Value
            If cmbEstadoRechazo.Value IsNot Nothing AndAlso cmbEstadoRechazo.Value <> -1 Then
                .IdEstadoRechazoCalidad = cmbEstadoRechazo.Value
            End If
            .ObservacionCallCenter = txtObservacionCalidad.Text
            resultado = .RegistrarGestionCalidad()
            'If (cmbEstadoCalidad.Value = 233) Then
            '    DescargarFormularioVentaPDF(txtNumIdentificacion.Text)
            'End If
        End With
        Return resultado
    End Function

    Private Function ModificarClienteGestion(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestionVenta)
        Dim miCliente As New ClienteFinal
        With miCliente
            .IdModificador = CInt(Session("userId"))
            .IdCliente = miGestion.IdCliente
            .NombreApellido = txtNombres.Text.Trim & " " & txtPrimerApellido.Text.Trim & " " & txtSegundoApellido.Text.Trim
            .Nombres = txtNombres.Text.Trim.ToUpper
            .PrimerApellido = txtPrimerApellido.Text.Trim.ToUpper
            .SegundoApellido = txtSegundoApellido.Text.Trim.ToUpper
            '.NumeroIdentificacion = txtNumIdentificacion.Text.Trim
            .IdCiudadResidencia = cmbCiudad1.Value
            .DireccionResidencia = txtDireccionResidencia.Text.Trim

            If Not String.IsNullOrEmpty(txtCelular.Text) Then
                .Celular = txtCelular.Text.Trim
            ElseIf Not String.IsNullOrEmpty(txtTelFijo.Text) Then
                .TelefonoResidencia = txtTelFijo.Text.Trim
            End If
            resultado = .ActualizarClienteGestion()
        End With
        Return resultado
    End Function

    Private Sub CargarDatosServicio()
        'Dim tipoProducto As New TipoProductoColeccion
        'Dim dtDatos As New DataTable

        'Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestionVenta)
        'Dim idCampania As Integer = miGestion.IdCampaniaNotus

        Dim tipoProducto As New ProductoCampaniaColeccion
        Dim dtDatos As New DataTable


        With tipoProducto
            If (Session("dtDatos") IsNot Nothing) Then
                .IdCampania = Integer.Parse(Session("dtDatos").Rows(0)(37))
            End If
            .TipoProducto = True
            dtDatos = .CargarDatos
        End With

        CargarComboDX(cmbTipoServicio, dtDatos, "idProductoComercial", "productoExterno")
    End Sub

    Private Sub limpiarControlesServicio()
        cmbTipoServicio.SelectedIndex = -1
        cmbProducto.SelectedIndex = -1
        memoObservacionServicio.Text = ""
        txtCupo.Text = ""
    End Sub

    Private Sub CargarDatosProducto(ByVal idTipoProducto As Integer)
        Dim miProducto As New ProductoCampaniaColeccion
        Dim dtDatos As New DataTable
        With miProducto
            If (Session("dtDatos") IsNot Nothing) Then
                .IdCampania = Integer.Parse(Session("dtDatos").Rows(0)(37))
            End If
            dtDatos = .CargarDatos
        End With
        CargarComboDX(cmbProducto, dtDatos, "idProductoExterno", "productoExterno")

        'Dim miProducto As New ProductoColeccion
        'Dim dtDatos As New DataTable
        'With miProducto
        '    .IdTipoProducto = idTipoProducto
        '    dtDatos = .GenerarDataTable
        'End With
        'CargarComboDX(cmbProducto, dtDatos, "IdProducto", "NombreProducto")
        cmbProducto.SelectedIndex = -1

        'Dim miValorPrima As New ValorPrimaServicioColeccion
        'Dim dtPrima As New DataTable
        'With miValorPrima
        '    .IdTipoProducto = idTipoProducto
        '    dtPrima = .GenerarDataTable
        'End With
        'If dtPrima.Rows.Count = 0 Then
        '    cmbPrima.Items.Insert(0, New ListEditItem("No existen valores", Nothing))
        'Else
        '    CargarComboDX(cmbPrima, dtPrima, "IdValorPrimaServicio", "ValorPrimaServicio")
        'End If
        'cmbPrima.SelectedIndex = -1

    End Sub

    Private Function RegistrarProductos(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestionVenta)
        Dim miRegistro As New GestionComercial.GestionDeVentaDetalle

        If miGestion.IdServicioNotus <> 0 Then
            'resultado = ValidarDisponibilidad(idGestionVenta, cmbProducto.Value)
            resultado.Valor = 0
            If resultado.Valor = 0 Then
                resultado = RegistrarProductoWS(idGestionVenta, cmbProducto.Value)
            End If
        End If

        If resultado.Valor = 0 Then
            With miRegistro
                .IdGestionVenta = idGestionVenta
                .IdCliente = miGestion.IdCliente
                .IdUsuarioAsesor = CInt(Session("userId"))
                .IdProducto = cmbProducto.Value
                .ValorPrima = txtCupo.Text
                .Observacion = memoObservacionServicio.Text.Trim
                resultado = .Registrar()
            End With
        End If

        Return resultado
    End Function

    Private Function ValidarProducto(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestionVenta)
        Dim idCampania As Integer = miGestion.IdCampaniaNotus
        Dim lisIdProducto As New List(Of Integer)
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim WSInfoServicio As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        lisIdProducto.Add(cmbProducto.Value)

        With WSInfoServicio
            .IdCampania = idCampania
            .ListProductos = lisIdProducto.ToArray()
            Wsresultado = objService.ValidaRegistrosServicioWS(WSInfoServicio)
        End With
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje

        Return resultado

    End Function

    Public Function ValidarDisponibilidad(ByVal IdGestionVenta As Long, ByVal idProducto As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=IdGestionVenta)
        Dim lisIdProducto As New List(Of Integer)
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsInfoDisponibilidad
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        lisIdProducto.Add(idProducto)

        With WSInfoFiltros
            .IdCiudad = miGestion.IdCiudadCliente
            .ListProductos = lisIdProducto.ToArray()
        End With
        Wsresultado = objService.ConsultarDisponibilidadDocumentos(WSInfoFiltros, dsDatos)
        dtDatos = dsDatos.Tables(0)

        If dtDatos.Rows.Count <> 0 Then
            Dim dvDatos As DataView = dtDatos.DefaultView
            dvDatos.RowFilter = "cantidadDisponible = 0 "
            Dim dtAux As DataTable = dvDatos.ToTable()
            If dtAux.Rows.Count <> 0 Then
                resultado.EstablecerMensajeYValor(10, "No existe disponibilidad de documentos para asignar el producto.")
            Else
                resultado.Valor = 0
            End If
        Else
            resultado.EstablecerMensajeYValor(1, "No se logró establecer la disponibilidad de documentos, según la campaña consultada.")
        End If
        Return resultado
    End Function

    Public Function RegistrarProductoWS(ByVal IdGestionVenta As Long, ByVal idProducto As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=IdGestionVenta)
        Dim lisIdProducto As New List(Of Integer)
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        lisIdProducto.Add(idProducto)

        With WSInfoFiltros
            .IdServicioMensajeria = miGestion.IdServicioNotus
            .ListProductos = lisIdProducto.ToArray()
        End With
        Wsresultado = objService.AgregarReferencias(WSInfoFiltros)
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje
        Return resultado
    End Function

    Private Function EliminarProducto(ByVal idDetalle As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Dim miRegistro As New GestionComercial.GestionDeVentaDetalle(idDetalle)
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=miRegistro.IdGestionVenta)


        If miGestion.IdServicioNotus <> 0 Then
            Wsresultado = EliminarProductoWS(miRegistro.IdGestionVenta, miRegistro.IdProducto)
        End If

        If Wsresultado.Valor = 0 Then
            With miRegistro
                .IdDetalle = idDetalle
                resultado = .Eliminar()
            End With
        Else
            resultado.Mensaje = Wsresultado.Mensaje
            resultado.Valor = Wsresultado.Valor
        End If

        Return resultado
    End Function

    Public Function EliminarProductoWS(ByVal IdGestionVenta As Long, ByVal idProducto As Integer) As NotusIlsService.ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=IdGestionVenta)
        Dim lisIdProducto As New List(Of Integer)
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        lisIdProducto.Add(idProducto)

        With WSInfoFiltros
            .IdServicioMensajeria = miGestion.IdServicioNotus
            .ListProductos = lisIdProducto.ToArray()
        End With
        Wsresultado = objService.EliminarReferencias(WSInfoFiltros)
        Return Wsresultado
    End Function

    Private Function CargarCliente(ByVal idGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestionVenta)
        Dim infoCliente As New ClienteFinal(idGestionVenta, miGestion.IdentificacionCliente, Integer.Parse(CInt(Session("userId"))))
        With infoCliente
            If Len(.Nombres) <= 0 Then
                'se separa el nombre
                Dim nombres As String
                nombres = .NombreApellido
                Dim words As String() = nombres.Split(New Char() {" "c})
                Dim i As Integer

                For i = 0 To words.Length - 1
                    If (words.Length - 1) - i = 0 Then
                        txtSegundoApellido.Text = words(i)
                    ElseIf (words.Length - 1) - i = 1 Then
                        txtPrimerApellido.Text = words(i)
                    Else
                        txtNombres.Text = txtNombres.Text & words(i)
                    End If

                Next
            Else
                txtNombres.Text = .Nombres
                txtPrimerApellido.Text = .PrimerApellido
                txtSegundoApellido.Text = .SegundoApellido
            End If
            txtNumIdentificacion.Text = .NumeroIdentificacion
            txtCelular.Text = .Celular
            If .IdCiudadResidencia > 0 Then
                cmbCiudad1.Value = .IdCiudadResidencia
            End If
            txtTelFijo.Text = .TelefonoResidencia
            txtIngresos.Text = .IngresoAproximado
            txtEmail.Text = .Email
            If .IdTipoIdentificacion > 0 Then
                cbTipoId.Value = .IdTipoIdentificacion
                Dim dtTipo As New DataTable
                dtTipo = HerramientasGenerales.ConsultarTipoIdentificacion
                CargarComboDX(cbTipoId, dtTipo, "idTipo", "descripcion")
            End If
            If .DireccionResidencia Is System.DBNull.Value Then
            Else
                txtDireccionResidencia.Text = .DireccionResidencia
            End If

            If CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 1 And .esDireccionModificada Then
                Session("esDireccionModificada") = True
                cbModificarDireccion.ClientVisible = CBool(Session("esDireccionModificada"))
            Else
                Session("esDireccionModificada") = False
            End If

        End With
        Return resultado
    End Function

    Private Sub CargarDatos()
        Dim infoCiudad As New CiudadColeccion
        Dim dtCiudad As New DataTable
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=lblIdGestionEdit.Value)
        Dim idCampania As Integer = miGestion.IdCampaniaNotus

        With infoCiudad
            .IdPais = Enumerados.CodigoPais.Colombia
            dtCiudad = .GenerarDataTable
        End With
        CargarComboDX(cmbCiudad1, dtCiudad, "IdCiudad", "CiudadDepartamento")

        Dim dtTipo As New DataTable
        dtTipo = HerramientasGenerales.ConsultarTipoIdentificacion
        CargarComboDX(cbTipoId, dtTipo, "idTipo", "descripcion")
    End Sub

    Private Function ObtenerCiudadCampania(ByVal idCampania As Integer) As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Wsresultado = objCampania.ConsultarCiudadesCampania(dsDatos, idCampania)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Public Function GenerarYDescargarSolicitudCredito(ByVal idGestionVenta As Integer) As Boolean
        Try

            Dim formulario As New DescargarSolicitudCreditoPersonaNatural()
            Dim dtDatosVenta As New DataSet
            dtDatosVenta = formulario.ConsultarInformacionPrestamoCliente(idGestionVenta)
            Dim fecha = CStr(Date.Now).Replace("/", "_").Replace(":", "_").Replace(" ", "_")
            Dim pdfTemplate As String = Server.MapPath("~/GestionComercial/Templates/DAVSolicitudCreditoPersonaNatural.pdf")
            Dim newFile As String = Server.MapPath("~/ArchivosCargados/SolicitudCreditoPersonaNatural_" & idGestionVenta & "__" & fecha & ".pdf")
            Dim pdfReader As New PdfReader(pdfTemplate)
            Dim pdfStamper As New PdfStamper(pdfReader, New FileStream(newFile, FileMode.Create))
            Dim pdfFormFields As AcroFields = pdfStamper.AcroFields
            If (dtDatosVenta IsNot Nothing AndAlso dtDatosVenta.Tables.Count > 0) Then

            End If

            If (dtDatosVenta IsNot Nothing AndAlso dtDatosVenta.Tables(0).Rows.Count > 0) Then
                ' Asigna los campos
                pdfFormFields.SetField("Nombres_2", dtDatosVenta.Tables(0).Rows(0).Item("nombres").ToString())
                pdfFormFields.SetField("Primer apellido_2", dtDatosVenta.Tables(0).Rows(0).Item("primerApellido").ToString())
                pdfFormFields.SetField("Segundo apellido_2", dtDatosVenta.Tables(0).Rows(0).Item("segundoApellido").ToString())
                pdfFormFields.SetField("TIPO IDEN PRIMER", dtDatosVenta.Tables(0).Rows(0).Item("TipoIdentificacion").ToString())
                pdfFormFields.SetField("No identificación_3", dtDatosVenta.Tables(0).Rows(0).Item("numeroIdentificacion").ToString())

                pdfFormFields.SetField("COD OFICINA", dtDatosVenta.Tables(0).Rows(0).Item("codOficina1").ToString())
                pdfFormFields.SetField("Nombre_oficina", dtDatosVenta.Tables(0).Rows(0).Item("nombreOficina").ToString())
                pdfFormFields.SetField("Codsucursal", dtDatosVenta.Tables(0).Rows(0).Item("Ciudad").ToString())
                pdfFormFields.SetField("Agente vendedor", dtDatosVenta.Tables(0).Rows(0).Item("codAgenteCliente").ToString())
                'pdfFormFields.SetField("Código Daviportátil", dtDatosVenta.Tables(0).Rows(0).Item("codAgenteCliente").ToString())
                pdfFormFields.SetField("Código estrategia", dtDatosVenta.Tables(0).Rows(0).Item("codigoEstrategia").ToString())


                'If dtDatosVenta.Tables(0).Rows(0).Item("CLIENTE_TIENE_CTA_AHORROS").ToString() = "0" Then
                '    pdfFormFields.SetField("SI/NO", "NO")
                'ElseIf dtDatosVenta.Tables(0).Rows(0).Item("CLIENTE_TIENE_CTA_AHORROS").ToString() = "1" Then
                '    pdfFormFields.SetField("SI/NO", "SI")
                'Else
                '    pdfFormFields.SetField("SI/NO", dtDatosVenta.Tables(0).Rows(0).Item("CLIENTE_TIENE_CTA_AHORROS").ToString())
                'End If

                '2.INFORMACIÓN DEL SOLICITANTE TITULAR

                pdfFormFields.SetField("1. Nombres", dtDatosVenta.Tables(0).Rows(0).Item("nombres").ToString())
                pdfFormFields.SetField("1. Primer apellido", dtDatosVenta.Tables(0).Rows(0).Item("primerApellido").ToString())
                pdfFormFields.SetField("1. segundo apellido", dtDatosVenta.Tables(0).Rows(0).Item("segundoApellido").ToString())
                pdfFormFields.SetField("GÉNERO", dtDatosVenta.Tables(0).Rows(0).Item("sexo").ToString())
                'pdfFormFields.SetField("Nacionalidad", dtDatosVenta.Rows(0).Item("idTipoIdentificacion").ToString())
                'pdfFormFields.SetField("Ciudad de nacimiento", dtDatosVenta.Rows(0).Item("idTipoIdentificacion").ToString())
                'pdfFormFields.SetField("FECHA NACIMIENTO PRIMER", dtDatosVenta.Rows(0).Item("idTipoIdentificacion").ToString())
                pdfFormFields.SetField("1. TIPO IDEN PRIMER", dtDatosVenta.Tables(0).Rows(0).Item("TipoIdentificacion").ToString())
                pdfFormFields.SetField("1. No identificacion", dtDatosVenta.Tables(0).Rows(0).Item("numeroIdentificacion").ToString())

                '2.1 Localizacion
                pdfFormFields.SetField("Dirección residencia", dtDatosVenta.Tables(0).Rows(0).Item("direccionResidencia").ToString())
                pdfFormFields.SetField("Ciudad_3", dtDatosVenta.Tables(0).Rows(0).Item("Ciudad").ToString())
                pdfFormFields.SetField("Tel fijo_2", dtDatosVenta.Tables(0).Rows(0).Item("telefonoResidencia").ToString())
                pdfFormFields.SetField("Celular_2", dtDatosVenta.Tables(0).Rows(0).Item("celular").ToString())
                pdfFormFields.SetField("Tel fijo_3", dtDatosVenta.Tables(0).Rows(0).Item("telefonoAdicional").ToString())
                pdfFormFields.SetField("Email", dtDatosVenta.Tables(0).Rows(0).Item("email").ToString())

                '3.INFORMACIÓN OTRO SOLICITANTE
                pdfFormFields.SetField("ACT OTROS", dtDatosVenta.Tables(0).Rows(0).Item("total_activos").ToString())
                pdfFormFields.SetField("DESCIPCION ACTIVOS", dtDatosVenta.Tables(0).Rows(0).Item("descripcion_activos").ToString())
                pdfFormFields.SetFieldProperty("TOTAL ACTIVOS", "flags", PdfFormField.FLAGS_READONLY, Nothing)
                pdfFormFields.SetFieldProperty("TOTAL ACTIVOS", "flags", PdfFormField.FLAGS_PRINT, Nothing)
                pdfFormFields.SetField("TOTAL ACTIVOS", dtDatosVenta.Tables(0).Rows(0).Item("total_activos").ToString())
                pdfFormFields.SetField("PAS OTROS", dtDatosVenta.Tables(0).Rows(0).Item("total_pasivos").ToString())
                pdfFormFields.SetField("DESCIPCION PASIVOS", dtDatosVenta.Tables(0).Rows(0).Item("descripcion_pasivos").ToString())
                pdfFormFields.SetFieldProperty("TOTAL PASIVOS", "flags", PdfFormField.FLAGS_READONLY, Nothing)
                pdfFormFields.SetField("TOTAL PASIVOS", dtDatosVenta.Tables(0).Rows(0).Item("total_pasivos").ToString())

            End If

            pdfFormFields.SetFieldProperty("TC", "flags", PdfFormField.FLAGS_READONLY, Nothing)
            pdfFormFields.SetFieldProperty("TC", "flags", PdfFormField.FLAGS_PRINT, Nothing)
            pdfFormFields.SetFieldProperty("TipoTC_Visa", "flags", PdfFormField.FLAGS_READONLY, Nothing)
            pdfFormFields.SetFieldProperty("TipoTC_Visa", "flags", PdfFormField.FLAGS_PRINT, Nothing)
            'TARJETA DE CREDITO
            If (dtDatosVenta IsNot Nothing AndAlso dtDatosVenta.Tables(1).Rows.Count > 0) Then
                pdfFormFields.SetField("TC", dtDatosVenta.Tables(1).Rows(0).Item("tipo_credito").ToString())
                pdfFormFields.SetField("TipoTC_Visa", dtDatosVenta.Tables(1).Rows(0).Item("subproducto").ToString())
                pdfFormFields.SetField("Cupo solicitado TC", dtDatosVenta.Tables(1).Rows(0).Item("valorCupo").ToString())
                If (dtDatosVenta.Tables(1).Rows.Count > 1) Then

                    pdfFormFields.SetField("2DO TC", dtDatosVenta.Tables(1).Rows(1).Item("tipo_credito").ToString())
                    pdfFormFields.SetField("2DO TipoTC_Diners", dtDatosVenta.Tables(1).Rows(1).Item("subproducto").ToString())
                    pdfFormFields.SetField("2DO Cupo solicitado TC", dtDatosVenta.Tables(1).Rows(1).Item("valorCupo").ToString())
                    If (dtDatosVenta.Tables(1).Rows.Count > 2) Then

                        pdfFormFields.SetField("3ER TC", dtDatosVenta.Tables(1).Rows(2).Item("tipo_credito").ToString())
                        pdfFormFields.SetField("3ER TipoTC_Visa", dtDatosVenta.Tables(1).Rows(2).Item("subproducto").ToString())
                        pdfFormFields.SetField("3ER Cupo solicitado TC", dtDatosVenta.Tables(1).Rows(2).Item("valorCupo").ToString())
                        If (dtDatosVenta.Tables(1).Rows.Count > 3) Then

                            pdfFormFields.SetField("4TO TC", dtDatosVenta.Tables(1).Rows(3).Item("tipo_credito").ToString())
                            pdfFormFields.SetField("4TO TipoTC_Visa", dtDatosVenta.Tables(1).Rows(3).Item("subproducto").ToString())
                            pdfFormFields.SetField("4TO Cupo solicitado TC", dtDatosVenta.Tables(1).Rows(3).Item("valorCupo").ToString())
                            If (dtDatosVenta.Tables(1).Rows.Count > 4) Then

                                pdfFormFields.SetField("5TO TC", dtDatosVenta.Tables(1).Rows(4).Item("tipo_credito").ToString())
                                pdfFormFields.SetField("5TO TipoTC_Visa", dtDatosVenta.Tables(1).Rows(4).Item("subproducto").ToString())
                                pdfFormFields.SetField("5TO Cupo solicitado TC", dtDatosVenta.Tables(1).Rows(4).Item("valorCupo").ToString())
                            End If
                        End If
                    End If
                End If
            End If

            'CREDIEXPRESS FIJO
            If (dtDatosVenta.Tables(2).Rows.Count > 0) Then
                pdfFormFields.SetField("PP CREDIEXPRESS FIJO", dtDatosVenta.Tables(2).Rows(0).Item("producto").ToString())
                pdfFormFields.SetField("CUPO CREDIEXPRESS FIJO", dtDatosVenta.Tables(2).Rows(0).Item("valorCupo").ToString())
                If (dtDatosVenta.Tables(2).Rows.Count > 1) Then
                    pdfFormFields.SetField("2DO PP CREDIEXPRESS FIJO", dtDatosVenta.Tables(2).Rows(1).Item("producto").ToString())
                    pdfFormFields.SetField("2DO CUPO CREDIEXPRESS FIJO", dtDatosVenta.Tables(2).Rows(1).Item("valorCupo").ToString())
                End If
            End If

            pdfFormFields.SetField("FIRMAS ID", dtDatosVenta.Tables(0).Rows(0).Item("TipoIdentificacion").ToString())
            pdfFormFields.SetField("FIRMAS DOC", dtDatosVenta.Tables(0).Rows(0).Item("numeroIdentificacion").ToString())


            'MessageBox.Show(sTmp, "Terminado");
            ' Cambia la propiedad para que no se pueda editar el PDF
            pdfStamper.FormFlattening = False
            'pdfStamper.FreeTextFlattening = True
            ' Cierra el PDF
            ' pdfStamper.SetFullCompression()
            ' pdfStamper.Writer.SetFullCompression()
            pdfStamper.Close()
            _rutaOrigenArchivo = newFile
        Catch ex As Exception
            Dim vg_mensaje As String
            vg_mensaje = "Error Diligenciado Fomulario, Registro No. " & 22222222222.ToString() & " - " & ex.Message.ToString()
            Return False
        End Try

        Return True
    End Function

#End Region


End Class