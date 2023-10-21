Imports DevExpress.Utils
Imports DevExpress.XtraPivotGrid
Imports DevExpress.Web.ASPxPivotGrid
Imports DevExpress.Web
Imports DevExpress.Data.Filtering
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General

Public Class ReporteDeCifrasCliente
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private Shared infoDenegacion As InfoDenegacionOpcionFuncionalRestringida

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Reporte de Cifras")
            Dim nombreFormulario As String = System.IO.Path.GetFileName(Me.Page.Request.Path)
            Try
                infoDenegacion = New InfoDenegacionOpcionFuncionalRestringida(nombreFormulario)
                AplicarRestricciones()
                InicializarReporte()
            Catch ex As Exception
                epNotificador.showError("Error al tratar de cargar página. Por favor intente nuevamente")
                cpGeneral.Enabled = False
            End Try
        Else
            EnlazarDatosReporte()
        End If
    End Sub

    Protected Sub btnGuardarComo_Click(sender As Object, e As EventArgs) Handles btnGuardarComo.Click
        Exportar(True)
    End Sub

    Protected Sub btnAbrir_Click(sender As Object, e As EventArgs) Handles btnAbrir.Click
        Exportar(False)
    End Sub

    'Protected Sub btnExpandir_Click(sender As Object, e As EventArgs) Handles btnExpandir.Click
    '    Try
    '        pgReporteCifras.ExpandAll()
    '    Catch ex As Exception
    '        epNotificador.showError("Error al tratar de Expandir Todo. " )
    '    End Try
    'End Sub

    'Protected Sub btnContraer_Click(sender As Object, e As EventArgs) Handles btnContraer.Click
    '    Try
    '        pgReporteCifras.CollapseAll()
    '    Catch ex As Exception
    '        epNotificador.showError("Error al tratar de Contraer Todo. " )
    '    End Try
    'End Sub

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
        ProcesarCallback(e.Parameter)
    End Sub

    Private Sub pgReporteCifras_FieldFilterChanged(sender As Object, e As DevExpress.Web.ASPxPivotGrid.PivotFieldEventArgs) Handles pgReporteCifras.FieldFilterChanged
        Dim pivot As ASPxPivotGrid = TryCast(sender, ASPxPivotGrid)
        ConstruirCriteriosDeFiltradoPivotGrid(pivot, e)
        pivot.Prefilter.Enabled = False
    End Sub


    Private Sub Exportar(ByVal guardarComo As Boolean)
        Try
            pgeExportador.OptionsPrint.PrintHeadersOnEveryPage = True
            'pgeExportador.OptionsPrint.PrintFilterHeaders = DefaultBoolean.True
            pgeExportador.OptionsPrint.PrintColumnHeaders = DefaultBoolean.True
            pgeExportador.OptionsPrint.PrintRowHeaders = DefaultBoolean.True
            pgeExportador.OptionsPrint.PrintDataHeaders = DefaultBoolean.True

            Dim nombreArchivo As String = "ReporteDeCifras"
            Select Case listExportFormat.SelectedIndex
                Case 0
                    pgeExportador.ExportPdfToResponse(nombreArchivo, guardarComo)
                Case 1
                    pgeExportador.ExportXlsToResponse(nombreArchivo, guardarComo)
                Case 2
                    pgeExportador.ExportMhtToResponse(nombreArchivo, "utf-8", "Reporte de Cifras", True, guardarComo)
                Case 3
                    pgeExportador.ExportRtfToResponse(nombreArchivo, guardarComo)
                Case 4
                    pgeExportador.ExportTextToResponse(nombreArchivo, guardarComo)
                Case 5
                    pgeExportador.ExportHtmlToResponse(nombreArchivo, "utf-8", "Reporte de Cifras", True, guardarComo)
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de exportar reporte. ")
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
            .Fields("Año").Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea
            .Fields("Año").AreaIndex = 0
            .Fields("Mes").Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea
            .Fields("Mes").AreaIndex = 1
            .Fields("ciudad").Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
            .Fields("puntoDeVenta").Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
            .Fields("nombreAsesor").Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
            .Fields("resultadoGestion").Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
            .Fields("tipoVenta").Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
            .Fields("nombreProducto").Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea
            .Fields("cupo").Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea

            Select Case codigoReporte
                Case "rpc"
                    .Fields("ciudad").Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
                    .Fields("ciudad").AreaIndex = 0
                    .Fields("puntoDeVenta").Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
                    .Fields("puntoDeVenta").AreaIndex = 1
                Case "rpp"
                    .Fields("puntoDeVenta").Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
                    '.Fields("puntoDeVenta").AreaIndex = 0
                Case "rprp"
                    .Fields("resultadoGestion").Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
                    '.Fields("resultadoGestion").AreaIndex = 0
                Case "rpa"
                    .Fields("nombreAsesor").Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
                    '.Fields("nombreAsesor").AreaIndex = 0
            End Select
            .EndUpdate()
            .ExpandAll()
        End With
    End Sub

    Private Sub EnlazarDatosReporte(Optional ByVal forzarConsulta As Boolean = False)
        Try
            Dim dtDatos As DataTable
            Dim reporteCifras As New ReporteDeCifras
            If Session("reporteCifrasCliente") Is Nothing OrElse forzarConsulta Then
                With reporteCifras
                    If deFechaInicio.Date > Date.MinValue Then .FechaInicial = deFechaInicio.Date
                    If deFechaFin.Date > Date.MinValue Then .FechaFinal = deFechaFin.Date
                    .CargarDatos()
                    dtDatos = .DatosReporte()
                End With
                Session("reporteCifrasCliente") = dtDatos
            Else
                dtDatos = CType(Session("reporteCifrasCliente"), DataTable)
            End If
            With pgReporteCifras
                .DataSource = dtDatos
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de consultar y enlazar datos. ")
        End Try
    End Sub

    'Protected Sub cbReporte_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbReporte.SelectedIndexChanged
    '    Dim codigoReporte As String = ""
    '    Try
    '        If cbReporte.Value IsNot Nothing Then codigoReporte = cbReporte.Value.ToString
    '        CambiarReporte(codigoReporte)
    '    Catch ex As Exception
    '        epNotificador.showError("Error al tratar de cambiar reporte. " )
    '    End Try
    'End Sub

    'Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
    '    EnlazarDatosReporte(True)
    'End Sub

    'Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
    '    Try
    '        cbReporte.SelectedIndex = 0
    '        CambiarReporte(cbReporte.Value)
    '        InicializarReporte()
    '    Catch ex As Exception
    '        epNotificador.showError("Error al tratar de reinicializar reporte. " )
    '    End Try
    'End Sub

    Private Sub InicializarReporte()
        With deFechaInicio
            .Date = Now.AddDays((Now.Day * -1) + 1)
            .MaxDate = Now
        End With
        With deFechaFin
            .Date = Now
            .MaxDate = Now
        End With

        EnlazarDatosReporte(True)
    End Sub

    Private Sub ProcesarCallback(parametro As String)
        Select Case parametro
            Case "consultar"
                EnlazarDatosReporte(True)
            Case "limpiar"
                Try
                    cbReporte.SelectedIndex = 0
                    LimpiarFiltrosPivotGrid(pgReporteCifras)
                    CambiarReporte(cbReporte.Value)
                    InicializarReporte()
                Catch ex As Exception
                    epNotificador.showError("Error al tratar de reinicializar reporte. ")
                End Try
        End Select
    End Sub

    Private Sub AplicarRestricciones()
        If infoDenegacion IsNot Nothing AndAlso infoDenegacion.ListaDenegaciones IsNot Nothing Then
            pnlOpcExportar.Visible = Not infoDenegacion.DenegarAcceso("pnlOpcExportar")
        End If
    End Sub



   

End Class
