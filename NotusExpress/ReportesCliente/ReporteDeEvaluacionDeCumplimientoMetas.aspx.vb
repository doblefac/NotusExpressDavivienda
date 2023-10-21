Imports DevExpress.Utils
Imports DevExpress.XtraPivotGrid
Imports DevExpress.Web
Imports DevExpress.Web.ASPxPivotGrid
Imports DevExpress.Data.Filtering
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.ConfiguracionComercial

Public Class ReporteDeEvaluacionDeCumplimientoMetas
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private Shared infoDenegacion As InfoDenegacionOpcionFuncionalRestringida
    Private Shared infoPermiso As InfoPermisoOpcionFuncionalRestringida

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Reporte de Cumplimiento de Metas Comerciales")
            Dim nombreFormulario As String = System.IO.Path.GetFileName(Me.Page.Request.Path)
            Try
                infoDenegacion = New InfoDenegacionOpcionFuncionalRestringida(nombreFormulario)
                infoPermiso = New InfoPermisoOpcionFuncionalRestringida(nombreFormulario)
                pnlTitulo.Visible = False
                hfCanal.Clear()
                hfInfoVista.Add("vistaAnterior", "")
                AplicarRestricciones()
                AplicarPermisos()
                InicializarReporte()
            Catch ex As Exception
                epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de cargar página. Por favor intente nuevamente", "Reporte de Cumplimiento de Metas", ex)
                cpGeneral.Enabled = False
            End Try
        Else
            EnlazarDatosReporte()
        End If
    End Sub

    Protected Sub btnGuardarComo_Click(sender As Object, e As EventArgs) Handles btnGuardarComo.Click
        Exportar()
    End Sub

    Private Sub pgReporteCifras_CustomCallback(sender As Object, e As DevExpress.Web.ASPxPivotGrid.PivotGridCustomCallbackEventArgs) Handles pgReporteCifras.CustomCallback
        Dim mensajeErr As String = "Error al tratar de procesar solicitud. "
        Try
            Select Case e.Parameters
                Case "cambioReporte"
                    Dim codigoReporte As String = ""
                    mensajeErr = "Error al tratar de cambiar reporte. "
                    If cbReporte.Value IsNot Nothing Then codigoReporte = cbReporte.Value.ToString
                    CambiarReporte(codigoReporte)
                Case "expandir"
                    mensajeErr = "Error al tratar de Expandir Todo. "
                    pgReporteCifras.ExpandAll()
                Case "contraer"
                    mensajeErr = "Error al tratar de Contraer Todo. "
                    pgReporteCifras.CollapseAll()
            End Select
            pgReporteCifras.DataBind()
        Catch ex As Exception
            epNotificador.showError(mensajeErr)
        End Try

    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Dim mensajeError As String = ""
        Try
            Dim codigoReporte As String = ""
            If cbReporte.Value IsNot Nothing Then codigoReporte = cbReporte.Value.ToString
            Select Case e.Parameter
                Case "consultar"
                    mensajeError = "Error al tratar de consultar y enlazar datos."
                    EnlazarDatosReporte(True)
                Case "limpiar"
                    mensajeError = "Error al tratar de reinicializar reporte. "
                    cbReporte.SelectedIndex = 0
                    LimpiarFiltrosPivotGrid(pgReporteCifras)
                    InicializarReporte()
                Case "mostrarGrafico"
                    mensajeError = "Error al tratar de generar gráfico"
                    CargarGrafico(cbReporte.Value.ToString)
                Case "mostrarGrafico2"
                    wcGrafico.DataBind()
                    pucGrafico.ShowOnPageLoad = True
                Case "cambioReporte"
                    mensajeError = "Error al tratar de cambiar reporte. "
                    CambiarReporte(codigoReporte)
            End Select
            btnAtras.Visible = IIf(Not String.IsNullOrEmpty(codigoReporte) AndAlso codigoReporte <> "vpc" AndAlso codigoReporte <> "vg", True, False)
            If codigoReporte <> "vpc" Then
                MostrarTitulo()
            Else
                hfCanal.Clear()
                pnlTitulo.Visible = False
            End If
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail(mensajeError, "Reporte de Cumplimiento de Metas", ex)
        Finally
            CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End Try
    End Sub

    Private Sub pgReporteCifras_FieldFilterChanged(sender As Object, e As DevExpress.Web.ASPxPivotGrid.PivotFieldEventArgs) Handles pgReporteCifras.FieldFilterChanged
        Dim pivot As ASPxPivotGrid = TryCast(sender, ASPxPivotGrid)
        ConstruirCriteriosDeFiltradoPivotGrid(pivot, e)
        pivot.Prefilter.Enabled = False
    End Sub

    Private Sub Exportar(Optional ByVal guardarComo As Boolean = True)
        Try
            pgeExportador.OptionsPrint.PrintHeadersOnEveryPage = True
            pgeExportador.OptionsPrint.PrintFilterHeaders = DefaultBoolean.False
            pgeExportador.OptionsPrint.PrintColumnHeaders = DefaultBoolean.True
            pgeExportador.OptionsPrint.PrintRowHeaders = DefaultBoolean.True
            pgeExportador.OptionsPrint.PrintDataHeaders = DefaultBoolean.False

            Dim nombreArchivo As String = "ReporteDeCumplimientoDeMetasComerciales"
            Select Case CInt(listExportFormat.Value)
                Case 0
                    pgeExportador.ExportPdfToResponse(nombreArchivo, guardarComo)
                Case 1
                    pgeExportador.ExportXlsToResponse(nombreArchivo, guardarComo)
                Case 2
                    pgeExportador.ExportXlsxToResponse(nombreArchivo, guardarComo)
            End Select
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de exportar datos. Por favor intente nuevamente", "Reporte de Cumplimiento de Metas", ex)
        End Try
    End Sub

    Private Sub ExpandirContraerGrupos(ByVal expandir As Boolean)
        Try
            For Each grupo As PivotGridGroup In pgReporteCifras.Groups
                For Each campo As PivotGridFieldBase In grupo
                    campo.ExpandedInFieldsGroup = expandir
                Next campo
            Next grupo
        Catch ex As Exception
            epNotificador.showError("Error al tratar de Expandir/Contraer Grupos. ")
        End Try
    End Sub

    Private Sub CambiarReporte(ByVal codigoReporte As String)
        With pgReporteCifras
            .BeginUpdate()
            'btnAtras.Visible = True
            Select Case codigoReporte
                Case "vpc"
                    If hfCanal.Count > 0 Then hfCanal("filtroCanal") = Nothing
                    'btnAtras.Visible = False
                    With .Fields("fieldCanal")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 0
                    End With
                    .Fields("fieldEstrategia").Visible = False
                    .Fields("fieldCiudad").Visible = False
                    .Fields("fieldPdv").Visible = False
                    With .Fields("fieldTipoProducto")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 1
                    End With
                Case "vpe"
                    .Fields("fieldCanal").Visible = False
                    With .Fields("fieldEstrategia")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 0
                    End With
                    With .Fields("fieldCiudad")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 1
                    End With
                    With .Fields("fieldPdv")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 2
                    End With
                    .Fields("fieldTipoProducto").Visible = False
                Case "vpci"
                    .Fields("fieldCanal").Visible = False
                    .Fields("fieldEstrategia").Visible = False
                    .Fields("fieldCiudad").Visible = True
                    .Fields("fieldPdv").Visible = False
                    .Fields("fieldTipoProducto").Visible = False
                Case "vpp"
                    .Fields("fieldCanal").Visible = False
                    .Fields("fieldEstrategia").Visible = False
                    .Fields("fieldPdv").Visible = True
                    .Fields("fieldPdv").Index = 0
                    With .Fields("fieldCiudad")
                        .Visible = True
                        .Index = 1
                        .Area = PivotArea.FilterArea
                    End With
                    .Fields("fieldTipoProducto").Visible = False
                Case Else
                    'btnAtras.Visible = False
                    With .Fields("fieldCanal")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 0
                    End With
                    With .Fields("fieldEstrategia")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 1
                    End With
                    With .Fields("fieldCiudad")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 2
                    End With
                    With .Fields("fieldPdv")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 3
                    End With
                    With .Fields("fieldTipoProducto")
                        .Visible = True
                        .Area = PivotArea.RowArea
                        .AreaIndex = 4
                    End With
            End Select
            .EndUpdate()
            .ExpandAll()
        End With
    End Sub

    Private Sub EnlazarDatosReporte(Optional ByVal forzarConsulta As Boolean = False)
        Try
            Dim dtDatos As DataTable
            Dim codigoReporte As String = ""
            If cbReporte.Value IsNot Nothing Then codigoReporte = cbReporte.Value.ToString
            If codigoReporte = "vpc" AndAlso hfCanal.Count > 0 Then hfCanal("filtroCanal") = Nothing
            If Session("reporteCumplimientoMetas") Is Nothing OrElse forzarConsulta Then
                Dim reporte As New ReporteCumplimientoMetas
                With reporte
                    If deFecha.Date > Date.MinValue Then .Fecha = deFecha.Date
                    .CargarDatos()
                    dtDatos = .DatosReporte()
                End With
                Session("reporteCumplimientoMetas") = dtDatos
            Else
                dtDatos = CType(Session("reporteCumplimientoMetas"), DataTable)
            End If
            Dim dvDatos As DataView = dtDatos.DefaultView
            If hfCanal.Count > 0 AndAlso hfCanal("filtroCanal") IsNot Nothing Then
                dvDatos.RowFilter = "Canal='" & hfCanal("filtroCanal").ToString & "'"
            Else
                dvDatos.RowFilter = ""
            End If
            With pgReporteCifras
                .CellTemplate = New TemplateCelda(fieldEjecucionPresupuestal)
                .FieldValueTemplate = New TemplateCampo(cbReporte.Value.ToString)
                .DataSource = dvDatos
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de enlazar datos. Por favor intente nuevamente", "Reporte de Cumplimiento de Metas", ex)
        End Try
    End Sub

    Private Sub InicializarReporte()
        With deFecha
            .Date = Now
            .MaxDate = Now
        End With
        btnAtras.Visible = False
        If hfCanal.Count > 0 Then hfCanal("filtroCanal") = Nothing
        CambiarReporte(cbReporte.Value)
        EnlazarDatosReporte(True)
    End Sub

    Private Sub AplicarRestricciones()
        If infoDenegacion IsNot Nothing AndAlso infoDenegacion.ListaDenegaciones IsNot Nothing Then
            pnlOpcExportar.Visible = Not infoDenegacion.DenegarAcceso("pnlOpcExportar")
        End If
    End Sub

    Private Sub AplicarPermisos()
        If infoPermiso IsNot Nothing AndAlso infoPermiso.ListaPermisos IsNot Nothing Then
            tmrActualizador.Enabled = infoPermiso.PermitirAcceso("tmrActualizador")
        End If
    End Sub

    Private Sub pgReporteCifras_CustomCellValue(sender As Object, e As DevExpress.Web.ASPxPivotGrid.PivotCellValueEventArgs) Handles pgReporteCifras.CustomCellValue
        If e.DataField IsNot fieldEjecucionPresupuestal Then Return

        Dim ds As PivotSummaryDataSource = e.CreateSummaryDataSource()
        Dim acumuladoMes As Decimal = 0
        Dim metaMes As Decimal = 0
        For Index As Integer = 0 To ds.RowCount - 1
            Dim row As PivotSummaryDataRow = ds(Index)
            acumuladoMes += CDec(row(fieldAcumuladoMes))
            metaMes += CDec(row(fieldMetaMes))
        Next
        If metaMes = 0 Then
            e.Value = IIf(acumuladoMes <> 0, 1, 0)
        Else
            e.Value = acumuladoMes / metaMes
        End If
    End Sub

    Class TemplateCelda
        Implements ITemplate

        Private _campoKpi As DevExpress.Web.ASPxPivotGrid.PivotGridField

        Public Sub New(campoKpi As DevExpress.Web.ASPxPivotGrid.PivotGridField)
            Me._campoKpi = campoKpi
        End Sub

        Public Sub InstantiateIn(container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim contenedorTemplate As PivotGridCellTemplateContainer = CType(container, PivotGridCellTemplateContainer)
            If Object.ReferenceEquals(contenedorTemplate.DataField, _campoKpi) Then
                Dim tabla As New Table
                contenedorTemplate.Controls.Add(tabla)

                Dim fila As New TableRow
                tabla.Controls.Add(fila)

                Dim celda As New TableCell
                celda.Controls.Add(ObtenerImagen(contenedorTemplate.Value))
                fila.Controls.Add(celda)

                celda = New TableCell
                celda.Text = contenedorTemplate.Item.Text
                celda.Wrap = False
                fila.Controls.Add(celda)
            Else
                contenedorTemplate.Controls.Add(New LiteralControl(contenedorTemplate.Text))
            End If
        End Sub

        Private Sub CargarIndicadoresDeCumplimiento()
                Dim lista As New IndicadorCumplimientoMetaColeccion
                lista.CargarDatos()
                HttpContext.Current.Session("listaIndicadoresCumplimiento") = lista.GenerarDataTable()
        End Sub

        Private Function ObtenerImagen(valor As Object) As Image
            Dim dtDatos As DataTable
            Dim imagen As New Image
            If HttpContext.Current.Session("listaIndicadoresCumplimiento") Is Nothing Then CargarIndicadoresDeCumplimiento()
            dtDatos = CType(HttpContext.Current.Session("listaIndicadoresCumplimiento"), DataTable)
            Dim filtro As String = "valorInicial<= " & Math.Min(CDec(valor), 1).ToString.Replace(",", ".") & " and valorFinal>=" & Math.Min(CDec(valor), 1).ToString.Replace(",", ".")
            Dim drAux() As DataRow = dtDatos.Select(filtro)
            If drAux IsNot Nothing AndAlso drAux.Count > 0 Then
                imagen.ImageUrl = drAux(0).Item("rutaImagen").ToString
            End If

            Return imagen
        End Function
    End Class

    Class TemplateCampo
        Implements ITemplate

        Private _vistaActual As String

        Public Sub New(ByVal vistaActual As String)
            _vistaActual = vistaActual
        End Sub

        Public Sub InstantiateIn(container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim contenedorTemplate As PivotGridFieldValueTemplateContainer = CType(container, PivotGridFieldValueTemplateContainer)
            If contenedorTemplate.Field IsNot Nothing Then
                If contenedorTemplate.Field.ID = "fieldCanal" AndAlso _vistaActual = "vpc" Then
                    contenedorTemplate.Controls.Add(CrearLink("vpe", contenedorTemplate.Text, contenedorTemplate.Value))
                ElseIf contenedorTemplate.Field.ID = "fieldCiudad" AndAlso _vistaActual = "vpe" Then
                    contenedorTemplate.Controls.Add(CrearLink("vpci", contenedorTemplate.Text, contenedorTemplate.Value))
                ElseIf contenedorTemplate.Field.ID = "fieldPdv" AndAlso _vistaActual = "vpe" Then
                    contenedorTemplate.Controls.Add(CrearLink("vpp", contenedorTemplate.Text, contenedorTemplate.Value))
                Else
                    contenedorTemplate.Controls.Add(New LiteralControl(contenedorTemplate.Text))
                End If
            End If
        End Sub

        Private Function CrearLink(ByVal vistaNueva As String, ByVal texto As String, ByVal valor As String) As MyLink
            'Dim contenedor As New HtmlGenericControl
            'contenedor.TagName = "div"
            'contenedor.Attributes("class") = "linkPivot"

            'contenedor.Controls.Add(New MyLink(_vistaActual, vistaNueva, texto, valor))

            Return New MyLink(_vistaActual, vistaNueva, texto, valor)
        End Function
    End Class

    Public Class MyLink
        Inherits HyperLink

        Public Sub New(ByVal vistaActual As String, ByVal vistaNueva As String, ByVal texto As String, ByVal valor As String)
            MyBase.New()
            Me.Text = texto
            Me.NavigateUrl = "#"
            Me.CssClass = "linkPivot"
            Me.Attributes("onclick") = "CambiarVistaReporte('" & vistaActual & "','" & vistaNueva & "','" & valor & "');"
            Me.Font.Underline = True
        End Sub
    End Class

    Protected Sub btnAbrir_Click(sender As Object, e As EventArgs) Handles btnAbrir.Click
        Exportar(False)
    End Sub

    Private Sub CargarGrafico(ByVal vista As String)
        Dim dtDatos As DataTable = CType(Session("reporteCumplimientoMetas"), DataTable).Copy
        Dim dvDatos As DataView = dtDatos.DefaultView
        dvDatos.RowFilter = "Anio='" & deFecha.Date.AddYears(-1).Year.ToString & "'"
        Select Case vista
            Case "vpc"
                With wcGrafico2
                    .Titles(0).Text = "Cumplimiento Por Canal"
                    .Series(0).ArgumentDataMember = "Canal"
                    .Series(1).ArgumentDataMember = "Canal"
                End With
            Case "vpe"
                With wcGrafico2
                    .Titles(0).Text = "Cumplimiento Por Estrategia"
                    .Series(0).ArgumentDataMember = "Estrategia"
                    .Series(1).ArgumentDataMember = "Estrategia"
                End With
            Case "vpci"
                With wcGrafico2
                    .Titles(0).Text = "Cumplimiento Por Ciudad"
                    .Series(0).ArgumentDataMember = "Ciudad"
                    .Series(1).ArgumentDataMember = "Ciudad"
                End With
            Case "vpp"
                With wcGrafico2
                    .Titles(0).Text = "Cumplimiento Por Punto de Venta"
                    .Series(0).ArgumentDataMember = "PuntoDeVenta"
                    .Series(1).ArgumentDataMember = "PuntoDeVenta"
                End With
            Case Else
                With wcGrafico2
                    .Titles(0).Text = "Cumplimiento Por Tipo de Producto"
                    .Series(0).ArgumentDataMember = "TipoProducto"
                    .Series(1).ArgumentDataMember = "TipoProducto"
                End With
        End Select
        With wcGrafico2
            .DataSource = dvDatos
            .DataBind()
        End With
        pucGrafico2.ShowOnPageLoad = True
    End Sub

    Public Sub MostrarTitulo()
        Dim canal As String = "(TODOS)"
        If hfCanal.Count > 0 AndAlso hfCanal("filtroCanal") IsNot Nothing Then canal = hfCanal("filtroCanal").ToString
        lblTitulo.Text = String.Format("CANAL {0} - {1}", canal, cbReporte.Text.ToUpper)
        pnlTitulo.Visible = True
    End Sub

End Class