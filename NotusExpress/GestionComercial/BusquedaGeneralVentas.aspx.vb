Imports DevExpress.Web
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.General

Public Class BusquedaGeneralVentas
    Inherits System.Web.UI.Page

#Region "Eventos"

    Private Sub BusquedaGeneralVentas_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        deFechaInicio.MaxDate = DateTime.Now.Date
        deFechaFin.MaxDate = DateTime.Now.Date
        deFechaInicioR.MaxDate = DateTime.Now.Date
        deFechaFinR.MaxDate = DateTime.Now.Date
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Búsqueda General de Ventas")
            Session.Remove("listaBusqueda")
        End If
        CargaInicial()
        If cbFiltroCiudad.IsCallback Then
            CargaInicial()
        End If
        cbFiltroCiudad.Focus()
        If gluDocumentos.IsCallback OrElse gluDocumentos.GridView.IsCallback OrElse Not Me.IsPostBack Then CargarNovedades()
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Dim idRadicado As Integer = 0
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "filtrarDatos"
                    CargarListadoDeServiciosRegistrados(True)
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        CType(sender, ASPxCallbackPanel).JSProperties("cpListaRadicado") = idRadicado
    End Sub

    Private Sub callback_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles callback.Callback
        Dim resultado As New ResultadoProceso
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "DescargarExtendido"
                    resultado = DescargarReporteExtendido()
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack de descarga: " & ex.Message)
        End Try
        CType(source, ASPxCallback).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        CType(source, ASPxCallback).JSProperties("cpResultado") = resultado.Valor
    End Sub

    Protected Sub gvDetalle_DataSelect(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Session("IdGestionVenta") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
            CargarDetalle(TryCast(sender, ASPxGridView))
        Catch ex As Exception
            epNotificador.showError("Se presento un error al consultar el detalle de la venta: " & ex.Message)
        End Try
    End Sub

    Private Sub gvVentas_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvVentas.CustomCallback
        Dim idRadicado As Integer = 0
        Try
            Dim mensajeErr As String = "Error al tratar de procesar solicitud. "
            Try
                Dim arrayAccion As String()
                arrayAccion = e.Parameters.Split(":")
                Select Case arrayAccion(0)
                    Case "expandir"
                        mensajeErr = "Error al tratar de Expandir Todo. "
                        gvVentas.DetailRows.ExpandAllRows()
                        CargarListadoDeServiciosRegistrados()
                    Case "contraer"
                        mensajeErr = "Error al tratar de Contraer Todo. "
                        gvVentas.DetailRows.CollapseAllRows()
                        CargarListadoDeServiciosRegistrados()
                    Case "verPlanilla"
                        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=CInt(arrayAccion(1)))
                        idRadicado = CInt(miGestion.IdRadicado)
                End Select
            Catch ex As Exception
                epNotificador.showError("Error en el CustomCallBack")
            End Try
        Catch ex As Exception
            epNotificador.showError("Error en el CustomCallBack")
        End Try
        CType(sender, ASPxGridView).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        CType(sender, ASPxGridView).JSProperties("cpListaRadicado") = idRadicado
    End Sub

    Private Sub gvVentas_DataBinding(sender As Object, e As System.EventArgs) Handles gvVentas.DataBinding
        If Session("listaBusqueda") Is Nothing Then
        Else
            gvVentas.DataSource = Session("listaBusqueda")
        End If
    End Sub

    Private Sub gvNovedades_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvNovedades.CustomCallback
        Dim resultado As New ResultadoProceso
        Dim result As Integer = 0
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameters.Split(":")
            Select Case arrayAccion(0)
                Case "Eliminar"
                    Dim miNovedad As New NovedadPorServicio(idNovedadServicio:=CInt(arrayAccion(1)))
                    Dim idGestion As Integer = miNovedad.IdGestionVenta
                    resultado = EliminarNovedad(CInt(arrayAccion(1)))
                    If resultado.Valor = 0 Then
                        ConsultarNovedadesGestion(idGestion)
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                    Else
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                    End If
                    result = resultado.Valor
                Case "Carga"
                    ConsultarNovedadesGestion(CInt(arrayAccion(1)))
                    result = 10
            End Select
        Catch ex As Exception
            mensajero.MostrarMensajePopUp("Se preseto un error en el callback: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
            result = 1
        End Try
        CType(sender, ASPxGridView).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje
        CType(sender, ASPxGridView).JSProperties("cpResultado") = result
    End Sub

    Private Sub gvNovedades_DataBinding(sender As Object, e As System.EventArgs) Handles gvNovedades.DataBinding
        gvNovedades.DataSource = Session("Novedades")
    End Sub

    Private Sub cbFormatoExportar_ButtonClick(source As Object, e As DevExpress.Web.ButtonEditClickEventArgs) Handles cbFormatoExportar.ButtonClick
        Try
            'CargarListadoDeServiciosRegistrados(True)
            Dim formato As String = cbFormatoExportar.Value
            If Not String.IsNullOrEmpty(formato) Then
                With gveExportador
                    .FileName = "ReporteGeneralVentas"
                    .Landscape = False
                    With .Styles.Default.Font
                        .Name = "Arial"
                        .Size = FontUnit.Point(10)
                    End With
                    .DataBind()
                End With
                Select Case formato
                    Case "xls"
                        gveExportador.WriteXlsToResponse()
                    Case "pdf"
                        With gveExportador
                            .Landscape = True
                            .WritePdfToResponse()
                        End With
                    Case "xlsx"
                        gveExportador.WriteXlsxToResponse()
                    Case "csv"
                        gveExportador.WriteCsvToResponse()
                End Select
            End If
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de exportar datos. ", "Reporte Inventario Gerencial", ex)
        End Try
    End Sub

    Protected Sub Link_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim idEstadoNotus As Integer
        Dim idRadicado As Integer
        Try
            Dim linkVer As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkVer.NamingContainer, GridViewDataItemTemplateContainer)
            Dim lnkGestionar As ASPxHyperLink = templateContainer.FindControl("lnkGestionar")
            Dim lnkPlanilla As ASPxHyperLink = templateContainer.FindControl("lnkPlanilla")

            linkVer.ClientSideEvents.Click = linkVer.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)
            idEstadoNotus = CInt(gvVentas.GetRowValuesByKeyValue(templateContainer.KeyValue, "IdEstadoServicioMensajeria"))
            idRadicado = CInt(gvVentas.GetRowValuesByKeyValue(templateContainer.KeyValue, "IdRadicado"))

            If idEstadoNotus = Enumerados.EstadosServicioMensajeria.Entregadolegalización And idRadicado = 0 Then
                lnkGestionar.Visible = True
            Else
                lnkGestionar.Visible = False
            End If

            If idRadicado = 0 Then
                lnkPlanilla.Visible = False
            Else
                lnkPlanilla.Visible = True
            End If

        Catch ex As Exception
            epNotificador.showError("Se presento un error al generar los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub

    Protected Sub LinkNovedad_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim idEstado As Integer
        Try
            Dim linkVer As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkVer.NamingContainer, GridViewDataItemTemplateContainer)
            Dim lnkNovedad As ASPxHyperLink = templateContainer.FindControl("lnkNovedad")
            Dim lnkEliminar As ASPxHyperLink = templateContainer.FindControl("lnkEliminar")

            linkVer.ClientSideEvents.Click = linkVer.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)
            idEstado = CInt(gvNovedades.GetRowValuesByKeyValue(templateContainer.KeyValue, "IdEstado"))

            If idEstado = Enumerados.EstadosNovedades.Pendiente Then
                lnkNovedad.Visible = True
                lnkEliminar.Visible = True
            Else
                lnkNovedad.Visible = False
                lnkEliminar.Visible = False
            End If
        Catch ex As Exception
            epNotificador.showError("Se presento un error al generar los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub

    Protected Sub Chk_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim puedeRadicar As Integer
        Dim idGestion As Long
        Try
            Dim chkPic As CheckBox = CType(sender, CheckBox)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(chkPic.NamingContainer, GridViewDataItemTemplateContainer)

            'chkPic.Attributes.Item  = chkPic.Checked.("{0}", templateContainer.KeyValue)
            puedeRadicar = CInt(gvVentas.GetRowValuesByKeyValue(templateContainer.KeyValue, "PuedeRadicar"))
            idGestion = CLng(gvVentas.GetRowValuesByKeyValue(templateContainer.KeyValue, "IdGestionVenta"))

            Dim chkRadica As CheckBox = templateContainer.FindControl("chkRadica")

            chkRadica.CssClass = chkRadica.CssClass.Replace("{0}", idGestion)
            chkRadica.ID = idGestion
            If puedeRadicar = 0 Then
                chkRadica.Enabled = False
            Else
                chkRadica.Enabled = True
            End If

        Catch ex As Exception
            epNotificador.showError("No fué posible establecer los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub

    Private Sub dialogoVerNovedad_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles dialogoVerNovedad.WindowCallback
        Dim resultado As New ResultadoProceso
        Dim result As Integer = 0
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Inicial"
                    resultado = ConsultaGestion(CInt(arrayAccion(1)))
                    ConsultarNovedadesGestion(CInt(arrayAccion(1)))
                    result = 10
                Case "Novedad"
                    lblIdGestion.Text = arrayAccion(1)
                    ConsultaGestion(CInt(arrayAccion(1)))
                    If rblNovedad.Value = 1 Then
                        tdNovedad.Visible = True
                        tdNovedad1.Visible = True
                        CargarNovedades()
                    Else
                        tdNovedad.Visible = False
                        tdNovedad1.Visible = False
                    End If
                    result = 10
                Case "Actualizar"
                    lblIdGestion.Text = arrayAccion(1)
                    resultado = ActualizarGestion(CInt(arrayAccion(1)))
                    If resultado.Valor = 0 Then
                        ConsultarNovedadesGestion(CInt(arrayAccion(1)))
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                    Else
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                    End If
                    result = resultado.Valor
            End Select
        Catch ex As Exception
            mensajero.MostrarMensajePopUp("Se preseto un error en el callback: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
            result = 1
        End Try
        CType(source, ASPxPopupControl).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje
        CType(source, ASPxPopupControl).JSProperties("cpResultado") = result
    End Sub

    Private Sub dialogoGestion_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles dialogoGestion.WindowCallback
        Dim resultado As New ResultadoProceso
        Dim result As Integer = 0
        Dim consult As Integer = 0
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Inicial"
                    IniciadorGestion(CInt(arrayAccion(1)))
                    cmbGestion.Focus()
                    result = 10
                Case "Gestionar"
                    lblNovedad.Text = arrayAccion(1)
                    resultado = GestionarNovedad(CInt(arrayAccion(1)))
                    If resultado.Valor = 0 Then
                        ConsultarNovedadesGestion(CInt(arrayAccion(1)))
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                        consult = 1
                    Else
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                    End If
                    result = resultado.Valor
            End Select
        Catch ex As Exception
            mensajero.MostrarMensajePopUp("Se preseto un error en el callback: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
            result = 1
        End Try
        CType(source, ASPxPopupControl).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje
        CType(source, ASPxPopupControl).JSProperties("cpResultado") = result
        CType(source, ASPxPopupControl).JSProperties("cpConsulta") = consult
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargaInicial()
        Try
            ' *** Se cargan las ciudades
            Dim infoCiudad As New CiudadColeccion
            With infoCiudad
                .IdPais = Enumerados.CodigoPais.Colombia
            End With
            CargarComboDX(cbFiltroCiudad, CType(infoCiudad.GenerarDataTable, DataTable), "IdCiudad", "CiudadDepartamento")

            ' *** Se cargan los causales
            Dim infoCausal As New CausalGenericaColeccion
            CargarComboDX(cbFiltroCausal, CType(infoCausal.GenerarDataTable, DataTable), "IdCausal", "Descripcion")

            ' Se cargan los listados de novedad causal 
            Dim infoNovedad As New NovedadCausalColeccion
            CargarComboDX(cbFiltroNovedad, CType(infoNovedad.GenerarDataTable, DataTable), "IdNovedad", "Descripcion")

            ' Se carga el listado de Asesores Comerciales
            Dim listaAsesor As New AsesorComercialColeccion
            With listaAsesor
                .IdEstado = Enumerados.EstadoBinario.Activo
            End With
            CargarComboDX(cbFiltroAsesor, CType(listaAsesor.GenerarDataTable, DataTable), "IdUsuarioSistema", "NombreAsesor")

            ' Se carga el listado de estados
            Dim listaEstados As New ListadoEstadosColeccion
            With listaEstados
                .IdEntidad = Enumerados.Entidad.NovedadServicio
            End With
            CargarComboDX(cbFiltroEstado, CType(listaEstados.GenerarDataTable, DataTable), "IdEstado", "Descripcion")

        Catch ex As Exception
            epNotificador.showError("Se presento un error en la carga inicial: " & ex.Message)
        End Try
    End Sub

    Private Sub CargarListadoDeServiciosRegistrados(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable = Nothing
        Try
            If Session("listaBusqueda") Is Nothing OrElse forzarConsulta Then
                Dim infoReporte As New GestionDeVentaColeccion
                With infoReporte
                    If Not String.IsNullOrEmpty(txtFiltroIdentificacion.Text) Then .IdentificacionCliente.Add(txtFiltroIdentificacion.Text.Trim)
                    If cbFiltroAsesor.Value > 0 Then .IdAsesor.Add(cbFiltroAsesor.Value)
                    If cbFiltroCiudad.Value > 0 Then .ListIdCiudad.Add(cbFiltroCiudad.Value)
                    If deFechaInicio.Date > Date.MinValue Then .FechaInicio = deFechaInicio.Date
                    If deFechaFin.Date > Date.MinValue Then .FechaFin = deFechaFin.Date
                    If deFechaInicioR.Date > Date.MinValue Then .FechaInicioRecepcion = deFechaInicioR.Date
                    If deFechaFin.Date > Date.MinValue Then .FechaFinRecepcion = deFechaFin.Date
                    If cbFiltroEstado.Value > 0 Then .ListIdEstadoNovedad.Add(cbFiltroEstado.Value)
                    If cbFiltroNovedad.Value > 0 Then .ListIdNovedad.Add(cbFiltroNovedad.Value)
                    If cbFiltroCausal.Value > 0 Then .ListIdCausal.Add(cbFiltroCausal.Value)
                    dtDatos = .GenerarDataTable()
                End With
                Session("listaBusqueda") = dtDatos
            Else
                dtDatos = CType(Session("listaBusqueda"), DataTable)
            End If

            With gvVentas
                .DataSource = dtDatos
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la información del reporte.")
        End Try
    End Sub

    Private Function DescargarReporteExtendido() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dtDatos As DataTable = Nothing
        Dim objReporte As New ReporteExtendidoLegalizacion

        With objReporte
            If Not String.IsNullOrEmpty(txtFiltroIdentificacion.Text) Then .IdentificacionCliente.Add(txtFiltroIdentificacion.Text.Trim)
            If cbFiltroAsesor.Value > 0 Then .IdAsesor.Add(cbFiltroAsesor.Value)
            If cbFiltroCiudad.Value > 0 Then .ListIdCiudad.Add(cbFiltroCiudad.Value)
            If deFechaInicio.Date > Date.MinValue Then .FechaInicio = deFechaInicio.Date
            If deFechaFin.Date > Date.MinValue Then .FechaFin = deFechaFin.Date
            If deFechaInicioR.Date > Date.MinValue Then .FechaInicioRecepcion = deFechaInicioR.Date
            If deFechaFin.Date > Date.MinValue Then .FechaFinRecepcion = deFechaFin.Date
            If cbFiltroEstado.Value > 0 Then .ListIdEstadoNovedad.Add(cbFiltroEstado.Value)
            If cbFiltroNovedad.Value > 0 Then .ListIdNovedad.Add(cbFiltroNovedad.Value)
            If cbFiltroCausal.Value > 0 Then .ListIdCausal.Add(cbFiltroCausal.Value)
            dtDatos = .GenerarReporte()
        End With
        If dtDatos.Rows.Count > 0 Then
            resultado.Valor = 0
        Else
            resultado.Valor = 1
            epNotificador.showWarning("No se encontraron resultados, según los filtros aplicados.")
        End If
        Session("dtReporteExtendido") = dtDatos

        Return resultado
    End Function

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
            epNotificador.showError("Se presento un error al consultar el detalle: " & ex.Message)
        End Try
        Return dtResultado
    End Function

    Private Function ConsultaGestion(ByVal idGestion As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestion)

        With miGestion
            If Not String.IsNullOrEmpty(.ObservacionRecepcion) Then meObservacion.Text = .ObservacionRecepcion
            If Not String.IsNullOrEmpty(.NumeroGuia) Then txtGuia.Text = .NumeroGuia
            If .IdReceptorDocumento <> 0 Then
                rblTipoRecepcion.Enabled = False
            End If
        End With

        rblTipoRecepcion.Focus()
        lblIdGestion.Text = idGestion
        tdNovedad.Visible = False
        tdNovedad1.Visible = False
        Return resultado
    End Function

    Private Sub CargarNovedades()
        Dim listaNovedad As New NovedadServicioColeccion

        With listaNovedad
            .IdEstado = Enumerados.EstadoBinario.Activo
            .CargarDatos()
        End With

        With gluDocumentos
            .DataSource = listaNovedad
            .DataBind()
        End With
    End Sub

    Private Function ActualizarGestion(ByVal idGestion As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=idGestion)
        With miGestion
            .IdModificador = CInt(Session("userId"))
            .ObservacionRecepcion = meObservacion.Text.Trim
            If Not String.IsNullOrEmpty(txtGuia.Text) Then .NumeroGuia = txtGuia.Text.Trim
            If .IdReceptorDocumento = 0 Then
                If rblTipoRecepcion.Value = 1 Then
                    .FechaRecepcionDocumentos = Date.Now
                    .IdReceptorDocumento = CInt(Session("userId"))
                End If
            End If
            If rblNovedad.Value = 1 Then
                Dim listaDoc As List(Of Object) = gluDocumentos.GridView().GetSelectedFieldValues("IdNovedad")
                .ListaNovedades.AddRange(listaDoc)
            End If
            resultado = .Actualizar()
        End With
        Return resultado
    End Function

    Private Function EliminarNovedad(ByVal idNovedadServicio As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miNovedad As New NovedadPorServicio

        With miNovedad
            .IdNovedadServicio = idNovedadServicio
            resultado = .Eliminar()
        End With
        Return resultado
    End Function

    Private Function GestionarNovedad(ByVal idNovedadServicio As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miNovedad As New NovedadPorServicio
        With miNovedad
            .IdNovedadServicio = idNovedadServicio
            .IdEstado = CInt(cmbGestion.Value)
            .IdUsuarioGestion = CInt(Session("UserId"))
            .FechaGestion = Date.Now
            .ObservacionGestion = meGestion.Text.Trim
            resultado = .Actualizar()
        End With
        Return resultado
    End Function

    Private Sub ConsultarNovedadesGestion(ByVal idGestion As Integer)
        Dim miNovedad As New NovedadPorServicioColeccion
        With miNovedad
            .ListIdGestionVenta.Add(idGestion)
            Session("Novedades") = .GenerarDataTable()
        End With

        With gvNovedades
            .DataSource = CType(Session("Novedades"), DataTable)
            .DataBind()
        End With
    End Sub

    Private Sub IniciadorGestion(ByVal idNovedadServicio As Integer)
        ' Se carga el listado de estados
        Dim listaEstados As New ListadoEstadosColeccion
        With listaEstados
            .IdEntidad = Enumerados.Entidad.NovedadServicio
        End With
        CargarComboDX(cmbGestion, CType(listaEstados.GenerarDataTable, DataTable), "IdEstado", "Descripcion")
        lblNovedad.Text = idNovedadServicio
    End Sub

#End Region

End Class