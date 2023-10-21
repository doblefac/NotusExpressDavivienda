Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Reportes
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.RecursoHumano
Imports DevExpress.Web


Partial Public Class ReporteGeneralGestionVentas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            With epNotificador
                .setTitle("Reporte General de Ventas")
                .showReturnLink("~/Administracion/Default.aspx")
            End With
            Session.Remove("htFiltros")
            Session.Remove("dtReporteVentas")
            InicializarTipoFiltradoEncabezado()
            CargarListadoPdv()
            CargarListadoDeAsesores(True)
            CargarListadoEstrategiaComercial()
            CargarListadoResultadoProceso()
            CargarListadoTiposDeVenta()
            pnlResultado.Visible = False
        Else
            If gvReporte.IsCallback Then ObtenerDatosReporte()
            'If cpResultadoReporte.IsCallback Then CargarListadoDeAsesores()
        End If
    End Sub

    Private Sub CargarListadoPdv()
        Dim listaPdv As New PuntoDeVentaColeccion
        Try
            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                If cbPdvActivo.Checked Then .IdEstado = 1
                .CargarDatos()
            End With
            With cboPdv
                .DataSource = listaPdv
                .TextField = "NombrePdv"
                .ValueField = "IdPdv"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Puntos de Venta. ")
        End Try
        With cboPdv
            .Items.Insert(0, New ListEditItem("Seleccione un Punto de Venta", 0))
            .SelectedIndex = 0
        End With
    End Sub

    Private Sub CargarListadoDeAsesores(Optional ByVal forzarConsulta As Boolean = False)
        cboAsesor.Items.Clear()
        Try
            Dim listaAsesor As AsesorComercialColeccion
            If Session("listaAsesores") Is Nothing OrElse forzarConsulta Then
                listaAsesor = New AsesorComercialColeccion
                With listaAsesor
                    .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                    If cboPdv.Value IsNot Nothing AndAlso CInt(cboPdv.Value) > 0 Then .IdPdv = CInt(cboPdv.Value)
                    If cbAsesorActivo.Checked Then .IdEstado = 1
                    .CargarDatos()
                End With
                Session("listaAsesores") = listaAsesor
            Else
                listaAsesor = CType(Session("listaAsesores"), AsesorComercialColeccion)
            End If
            With cboAsesor
                .DataSource = listaAsesor
                .TextField = "NombreAsesor"
                .ValueField = "IdUsuarioSistema"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Asesores. ")
        End Try
        With cboAsesor
            .Items.Insert(0, New ListEditItem("Seleccione un Asesor", 0))
            If .SelectedIndex = -1 Then .SelectedIndex = 0
        End With
    End Sub

    Private Sub CargarListadoEstrategiaComercial(Optional ByVal forzarConsulta As Boolean = False)
        cboEstrategiaComercial.Items.Clear()
        Try
            Dim listaEstrategiaComercial As EstrategiaComercialColeccion
            If Session("listaEstrategiaComercial") Is Nothing OrElse forzarConsulta Then
                listaEstrategiaComercial = New EstrategiaComercialColeccion
                With listaEstrategiaComercial
                    .CargarDatos()
                End With
                Session("listaEstrategiaComercial") = listaEstrategiaComercial
            Else
                listaEstrategiaComercial = CType(Session("listaEstrategiaComercial"), EstrategiaComercialColeccion)
            End If
            With cboEstrategiaComercial
                .DataSource = listaEstrategiaComercial
                .TextField = "Nombre"
                .ValueField = "IdEstrategia"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Asesores. ")
        End Try
        With cboEstrategiaComercial
            .Items.Insert(0, New ListEditItem("Seleccione una Estrategia", 0))
            If .SelectedIndex = -1 Then .SelectedIndex = 0
        End With
    End Sub


    Private Sub CargarListadoResultadoProceso()
        Dim listaResultado As New ResultadoProcesoVentaColeccion
        Try
            With listaResultado
                .CargarDatos()
            End With
            With cboResultadoGestion
                .DataSource = listaResultado
                .TextField = "Descripcion"
                .ValueField = "IdResultado"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Resultados de Proceso. ")
        End Try
        With cboResultadoGestion
            .Items.Insert(0, New ListEditItem("Seleccione un Item", 0))
            .SelectedIndex = 0
        End With
    End Sub

    Private Sub CargarListadoTiposDeVenta()
        Dim listaTipoVenta As New ResultadoProcesoTipoVentaColeccion
        Try
            With listaTipoVenta
                .CargarDatos()
            End With
            With cboTipoProducto
                .DataSource = listaTipoVenta
                .TextField = "TipoVenta"
                .ValueField = "IdTipoVenta"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Tipos de Venta. ")
        End Try
        With cboTipoProducto
            .Items.Insert(0, New ListEditItem("Seleccione un Item", 0))
            .SelectedIndex = 0
        End With
    End Sub

    Public Sub ObtenerDatosReporte(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable

        If Session("dtReporteGeneralVentas") Is Nothing OrElse forzarConsulta Then
            Dim infoReferido As New ReporteGeneralDeVentas
            With infoReferido
                If cboPdv.Value IsNot Nothing Then Integer.TryParse(cboPdv.Value.ToString, .IdPuntoDeVenta)
                If cboAsesor.Value IsNot Nothing Then Integer.TryParse(cboAsesor.Value.ToString, .IdAsesorComercial)
                If cboResultadoGestion.Value IsNot Nothing Then Integer.TryParse(cboResultadoGestion.Value.ToString, .IdResultadoProceso)
                If cboTipoProducto.Value IsNot Nothing Then Integer.TryParse(cboTipoProducto.Value.ToString, .IdTipoVenta)
                If cboEstrategiaComercial.Value IsNot Nothing Then Integer.TryParse(cboEstrategiaComercial.Value.ToString, .idEstrategia)
                If cmbTipoCliente.Value IsNot Nothing Then Integer.TryParse(cmbTipoCliente.Value.ToString, .idTipoCliente)
                If deFechaInicio.Date > Date.MinValue Then .FechaInicial = deFechaInicio.Date
                If deFechaFin.Date > Date.MinValue Then .FechaFinal = deFechaFin.Date

                dtDatos = .DatosReporte
            End With
        Else
            dtDatos = Session("dtReporteGeneralVentas")
        End If

        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            pnlResultado.Visible = True
            EnlazarDatos(dtDatos)
        Else
            epNotificador.showWarning("No se encontraron datos según los filtros aplicados.")
        End If
    End Sub

    Private Sub EnlazarDatos(ByVal dtDatos As DataTable)
        With gvReporte
            .DataSource = dtDatos
            .KeyFieldName = "idGestionVenta"
            .DataBind()
        End With
        Session("dtReporteVentas") = dtDatos
    End Sub

    Public Sub GuardarFiltros()
        Dim ht As New Hashtable
        With ht
            .Add("pdv", cboPdv.Value)
            .Add("asesor", cboAsesor.Value)
            .Add("fechaInicial", deFechaInicio.Date)
            .Add("fechaFinal", deFechaFin.Date)
        End With
        Session("htFiltros") = ht
    End Sub

    Public Sub InicializarTipoFiltradoEncabezado()
        For Each column As GridViewDataColumn In gvReporte.Columns
            column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList
        Next column
    End Sub

    Protected Sub cbFormatoExportar_ButtonClick(ByVal source As Object, ByVal e As DevExpress.Web.ButtonEditClickEventArgs) Handles cbFormatoExportar.ButtonClick
        Try
            ObtenerDatosReporte()
            Dim formato As String = cbFormatoExportar.Value
            If Not String.IsNullOrEmpty(formato) Then
                With gveExportador
                    .FileName = "ReporteGeneralDeVentas"
                    .ReportHeader = "Reporte General de Venta" & vbCrLf & vbCrLf
                    .ReportFooter = vbCrLf & "Logytech Mobile S.A.S"
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
            epNotificador.showError("Error al tratar de exportar datos. ")
        End Try
    End Sub

    Protected Sub cboAsesor_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cboAsesor.Callback
        If e.Parameter = "cargarLista" Then
            CargarListadoDeAsesores(True)
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End If
    End Sub

    Private Sub cpResultadoReporte_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpResultadoReporte.Callback
        Select Case e.Parameter
            Case "obtenerReporte"
                ObtenerDatosReporte(True)
            Case "limpiarFiltros"
                LimpiarFiltros()
        End Select
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub LimpiarFiltros()
        Try
            Session.Remove("dtReporteGeneralVentas")
            If cboPdv.Value IsNot Nothing AndAlso CInt(cboPdv.Value) <> 0 Then
                cboPdv.SelectedIndex = 0
                CargarListadoDeAsesores()
                CargarListadoEstrategiaComercial()
            Else
                If cboAsesor.Value IsNot Nothing AndAlso CInt(cboAsesor.Value) <> 0 Then cboAsesor.SelectedIndex = 0
            End If
            cmbTipoCliente.SelectedIndex = 0
            deFechaInicio.Date = Nothing
            deFechaFin.Date = Nothing
            If gvReporte.Settings.ShowHeaderFilterButton Then gvReporte.FilterExpression = String.Empty
            gvReporte.DataSource = Nothing
            gvReporte.DataBind()
            pnlResultado.Visible = False
        Catch ex As Exception
            epNotificador.showError("Error al tratar de limpiar filtros. ")
        End Try

    End Sub

    Private Sub cboPdv_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cboPdv.Callback
        If e.Parameter = "cargarLista" Then
            CargarListadoPdv()
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End If
    End Sub

    Private Sub gvReporte_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvReporte.CustomCallback
        Dim mensajeErr As String = "Error al tratar de procesar solicitud. "
        Try
            Select Case e.Parameters
                Case "expandir"
                    mensajeErr = "Error al tratar de Expandir Todo. "
                    gvReporte.ExpandAll()
                Case "contraer"
                    mensajeErr = "Error al tratar de Contraer Todo. "
                    gvReporte.CollapseAll()
            End Select
        Catch ex As Exception
            epNotificador.showError(mensajeErr)
        End Try
    End Sub

    Protected Sub cboEstrategiaComercial_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cboEstrategiaComercial.Callback
        If e.Parameter = "cargarLista" Then
            CargarListadoEstrategiaComercial(True)
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End If
    End Sub
End Class

