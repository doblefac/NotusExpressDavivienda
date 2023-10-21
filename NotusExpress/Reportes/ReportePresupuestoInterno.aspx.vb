Imports DevExpress.Web.ASPxPivotGrid
Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer
Imports System.Collections.Generic
Imports pdd = DevExpress.XtraPivotGrid
Imports DevExpress.XtraPivotGrid

Public Class ReportePresupuestoInterno
    Inherits System.Web.UI.Page

#Region "Atributos"

#End Region

#Region "Eventos"

    Private Sub ReportePresupuestoInterno_Init(sender As Object, e As EventArgs) Handles Me.Init
        Herramientas.CargarLicenciaGembox()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim columnIndexValue As String = ColumnIndex.Value, rowIndexValue As String = RowIndex.Value
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Reporte de Presupuesto (Interno)")
                    CargarFiltros()
                End With
            Else
                If Session("dtDatos") IsNot Nothing Then
                    With pivotGrid
                        .DataSource = Session("dtDatos")
                        .DataBind()
                    End With
                End If
            End If
            pivotGrid.ClientSideEvents.CellClick = GetJSCellClickHandler()
        Catch ex As Exception
            epNotificador.showError("Se presento un error al cargar la página: " & ex.Message)
        End Try
    End Sub

    Private Sub pivotGrid_CustomCallback(sender As Object, e As DevExpress.Web.ASPxPivotGrid.PivotGridCustomCallbackEventArgs) Handles pivotGrid.CustomCallback
        Dim parametros As String = e.Parameters.ToString
        Select Case parametros
            Case "limpiar"
                Session.Remove("dtDatos")
                With pivotGrid
                    .DataSource = Session("dtDatos")
                    .DataBind()
                End With
            Case "expandir"
                pivotGrid.ExpandAll()
            Case "contraer"
                pivotGrid.CollapseAll()
            Case Else
                CargarDatos()
        End Select
        CType(sender, ASPxPivotGrid).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub pivotGrid_CustomSummary(sender As Object, e As DevExpress.Web.ASPxPivotGrid.PivotGridCustomSummaryEventArgs) Handles pivotGrid.CustomSummary

        Dim ds As pdd.PivotDrillDownDataSource = e.CreateDrillDownDataSource()
        Dim dtDatos As DataTable = Session("dtDatos")
        If e.RowFieldValue = "Gestionado" Then
            Dim returnValue As Double = 0
            Dim cantidadBase As Integer
            For i As Integer = 0 To ds.RowCount - 1
                Dim row As pdd.PivotDrillDownDataRow = ds(i)
                If row(fieldindicador) IsNot Nothing Then
                    cantidadBase = row(fieldCantidadBase)
                    returnValue += 1
                End If
            Next
            e.CustomValue = returnValue / cantidadBase
        ElseIf e.RowFieldValue = "Contactado" Then
            Dim cntContactados As Integer = 0
            Dim cntContactadosEfectivos As Integer = 0
            For i As Integer = 0 To ds.RowCount - 1
                Dim row As pdd.PivotDrillDownDataRow = ds(i)
                If row(fieldindicador) IsNot Nothing And e.RowFieldValue = "Contactado" Then
                    cntContactados = row(fieldCantidadBase)
                    cntContactadosEfectivos += 1
                End If
            Next
            e.CustomValue = cntContactadosEfectivos / cntContactados
        ElseIf e.RowFieldValue = "Conversion" Then
            Dim cntGestionados As Integer = 0
            Dim cntContactadosEfectivos As Integer = 0
            For i As Integer = 0 To ds.RowCount - 1
                Dim row As pdd.PivotDrillDownDataRow = ds(i)
                If row(fieldindicador) IsNot Nothing And e.RowFieldValue = "Conversion" Then
                    cntGestionados = row(fieldCantidadBase)
                    cntContactadosEfectivos += 1
                End If
            Next
            e.CustomValue = cntContactadosEfectivos / cntGestionados
        ElseIf e.RowFieldValue = "Efectividad" Then
            Dim cntBase As Integer = 0
            Dim cntEfectivos As Integer = 0
            For i As Integer = 0 To ds.RowCount - 1
                Dim row As pdd.PivotDrillDownDataRow = ds(i)
                If row(fieldindicador) IsNot Nothing And e.RowFieldValue = "Efectividad" Then
                    cntBase = row(fieldCantidadBase)
                    cntEfectivos += 1
                End If
            Next
            e.CustomValue = cntEfectivos / cntBase
        End If

    End Sub

    Private Sub pivotGrid_DataBinding(sender As Object, e As System.EventArgs) Handles pivotGrid.DataBinding
        If Session("dtDatos") IsNot Nothing Then pivotGrid.DataSource = Session("dtDatos")
    End Sub

    Protected Sub cbFormatoExportar_ButtonClick(source As Object, e As DevExpress.Web.ButtonEditClickEventArgs) Handles cbFormatoExportar.ButtonClick
        Dim nombreArchivo As String = "Reporte Presupuesto (Interno)"
        Dim guardarComo As Boolean = True
        Select Case cbFormatoExportar.Value
            Case "pdf"
                pgeExportador.ExportPdfToResponse(nombreArchivo, guardarComo)
            Case "xls"
                pgeExportador.ExportXlsToResponse(nombreArchivo, guardarComo)
            Case "xlsx"
                pgeExportador.ExportToXlsx(nombreArchivo, guardarComo)
            Case "cvs"
                pgeExportador.ExportCsvToResponse(nombreArchivo, guardarComo)
        End Select
    End Sub

    Protected Sub gvDatos_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvDatos.CustomCallback
        Dim param() As String = e.Parameters.Split("|"c)
        If param(0) <> "D" Then
            Return
        End If
        BindGridViewDatos(ColumnIndex.Value, RowIndex.Value)
        gvDatos.PageIndex = 0
    End Sub

    Private Sub gvDatos_DataBinding(sender As Object, e As System.EventArgs) Handles gvDatos.DataBinding
        If Session("dtGestiones") IsNot Nothing Then gvDatos.DataSource = Session("dtGestiones")
    End Sub

    Protected Sub gvMetas_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvMetas.CustomCallback
        BindGridViewMetas(ColumnIndex.Value, RowIndex.Value)
        gvMetas.PageIndex = 0
    End Sub

    Private Sub gvMetas_DataBinding(sender As Object, e As System.EventArgs) Handles gvMetas.DataBinding
        If Session("listaMetas") IsNot Nothing Then gvMetas.DataSource = Session("listaMetas")
    End Sub

    Private Sub btnExportarDetalle_Click(sender As Object, e As EventArgs) Handles btnExportarDetalle.Click
        Try
            Dim resultado As New ResultadoProceso
            Dim dt As DataTable = CType(Session("dtDatos"), DataTable)
            Dim objDatos As New Reportes.ReportePresupuestoInterno
            With objDatos
                .DatosReporte = CType(Session("dtDatos"), DataTable)
                resultado = .GenerarReporteExcel()
                If resultado.Valor = 0 Then
                    Session("rutaArchivo") = .RutaArchivo
                    Herramientas.ForzarDescargaDeArchivo(HttpContext.Current, .RutaArchivo, "Reporte Detalle Presupuesto (Interno).xlsx")
                Else
                    epNotificador.showWarning(resultado.Mensaje)
                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Se presento un error al generar el reporte del detalle de la informacion: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarFiltros()
        ' Se cargan los productos
        Dim objProducto As New Campania()
        Dim idCliente As Integer() = {3}
        Session("dtProductos") = objProducto.ObtenerProductosComerciales(idCliente)
        CargarComboDX(cmbProducto, CType(Session("dtProductos"), DataTable), "idProductoComercial", "productoExterno")

        ' Se cargan las bases cargadas
        Dim objBase As New PlanoClienteFinalColeccion
        Session("dtBase") = objBase.GenerarDataTable
        CargarComboDX(cmbBase, CType(Session("dtBase"), DataTable), "idPlano", "nombreBase")

    End Sub

    Private Sub CargarDatos()
        Try
            Dim objReporte As New Reportes.ReportePresupuestoInterno
            With objReporte
                If cmbProducto.Value IsNot Nothing Then .IdProducto = cmbProducto.Value
                If cmbBase.Value IsNot Nothing Then .IdBase = cmbBase.Value
                .FechaInicial = deFechaInicio.Date
                .FechaFinal = deFechaFin.Date
                .CargarDatos()
            End With

            Session("dtDatos") = objReporte.DatosReporte
            If objReporte.DatosReporte().Rows.Count = 0 Then
                epNotificador.showWarning("<i> No se encontraron resultados, según los filtros establecidos. </i>")
            Else
                epNotificador.clear()
            End If
            With pivotGrid
                .DataSource = Session("dtDatos")
                .DataBind()
            End With

        Catch ex As Exception
            epNotificador.showError("Se genero un error al tratar de procesar datos: " & ex.Message)
        End Try
    End Sub

    Protected Sub BindGridViewDatos(ByVal columnIndex As String, ByVal rowIndex As String)
        Dim ds As PivotDrillDownDataSource = pivotGrid.CreateDrillDownDataSource(Int32.Parse(columnIndex), Int32.Parse(rowIndex))
        Dim rowIndex1 As Integer = 0
        Dim fieldAsesor As String = "idAsesor"
        Dim valueAsesor As Object = ds(rowIndex1)(fieldAsesor)
        Dim fieldBase As String = "idPlano"
        Dim valueBase As Object = ds(rowIndex1)(fieldBase)
        ConsultaGestiones(valueAsesor, valueBase)
    End Sub

    Private Sub ConsultaGestiones(ByVal _idAsesor As Integer, ByVal _idBase As Integer)
        If _idAsesor > 0 And _idBase > 0 Then
            Dim dtGestiones As DataTable
            Dim objGestiones As New GestionDeVentaColeccion
            With objGestiones
                .IdAsesor.Add(_idAsesor)
                .idBase.add(_idBase)
                dtGestiones = .GenerarDataTable
            End With
            Session("dtGestiones") = dtGestiones
            With gvDatos
                .PageIndex = 0
                .DataSource = dtGestiones
                .DataBind()
            End With
        End If
    End Sub

    Protected Sub BindGridViewMetas(ByVal columnIndex As String, ByVal rowIndex As String)
        Dim ds As PivotDrillDownDataSource = pivotGrid.CreateDrillDownDataSource(Int32.Parse(columnIndex), Int32.Parse(rowIndex))
        Dim rowIndex1 As Integer = 0
        Dim fieldAsesor As String = "idAsesor"
        Dim valueAsesor As Object = ds(rowIndex1)(fieldAsesor)
        ConsultaMetas(valueAsesor)
    End Sub

    Private Sub ConsultaMetas(ByVal _idAsesor As Integer)
        If _idAsesor > 0 Then
            Dim listaMetas = New ConfiguracionComercial.MetaComercialAsesorColeccion
            With listaMetas
                .IdPersonaAsesor = _idAsesor
                .CargarDatos()
            End With
            Session("listaMetas") = listaMetas
            With gvMetas
                .PageIndex = 0
                .DataSource = listaMetas
                .DataBind()
            End With
        End If
    End Sub

    Protected Function GetJSCellClickHandler() As String
        Return String.Format("function (s, e) {{" & ControlChars.CrLf & " var columnIndex = document.getElementById('{0}')," & ControlChars.CrLf & " rowIndex = document.getElementById('{1}');" & ControlChars.CrLf & " columnIndex.value = e.ColumnIndex;" & ControlChars.CrLf & " rowIndex.value = e.RowIndex;" & ControlChars.CrLf & " GridView.PerformCallback('D'); " & ControlChars.CrLf & " ShowDrillDown();" & ControlChars.CrLf & "}}", ColumnIndex.ClientID, RowIndex.ClientID)
    End Function

    Protected Function GetJSPopupClosingHandler() As String
        Return String.Format("function (s, e) {{" & ControlChars.CrLf & " var columnIndex = document.getElementById('{0}')," & ControlChars.CrLf & " rowIndex = document.getElementById('{1}');" & ControlChars.CrLf & " columnIndex.value = '';" & ControlChars.CrLf & " rowIndex.value = ''; " & ControlChars.CrLf & "}}", ColumnIndex.ClientID, RowIndex.ClientID)
    End Function

#End Region

End Class