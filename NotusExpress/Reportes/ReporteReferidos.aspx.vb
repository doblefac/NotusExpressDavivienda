Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Reportes
Imports NotusExpressBusinessLayer.General
Imports DevExpress.Web
Imports DevExpress.XtraPrinting

Partial Public Class ReporteReferidos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        InicializarLinksSelectorFechas()
        If Not Me.IsPostBack Then
            With epNotificador
                .setTitle("Reporte General de Referidos")
                .showReturnLink("~/Administracion/Default.aspx")
            End With
            Session.Remove("htFiltros")
            Session.Remove("dtReporteReferidos")
            InicializarTipoFiltradoEncabezado()
            CargarListadoPdv()
            pnlResultado.Visible = False
        Else
            If pnlResultado.Visible AndAlso gvReporte.IsCallback Then
                ObtenerDatosReporte()
            End If
        End If
    End Sub

    Private Sub CargarListadoPdv()
        Dim listaPdv As New PuntoDeVentaColeccion
        Try
            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                .CargarDatos()
            End With
            With ddlPdv
                .DataSource = listaPdv
                .DataTextField = "NombrePdv"
                .DataValueField = "IdPdv"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Puntos de Venta. ")
        End Try
        With ddlPdv
            .Items.Insert(0, New ListItem("Seleccione un Punto de Venta", "0"))
        End With
    End Sub

    Protected Sub lbConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbConsultar.Click
        Try
            GuardarFiltros()
            ObtenerDatosReporte()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de generar reporte general de referidos. ")
        End Try
    End Sub

    Public Sub ObtenerDatosReporte()
        Dim infoReferido As New ReporteDeReferidos
        Dim dtDatos As DataTable
        If Session("htFiltros") Is Nothing Then GuardarFiltros()
        Dim ht As Hashtable = Session("htFiltros")
        With infoReferido
            Integer.TryParse(ht("pdv").ToString, .IdPuntoDeVenta)
            Integer.TryParse(ht("asesor").ToString, .IdAsesorComercial)
            If Not String.IsNullOrEmpty(ht("fechaInicial").ToString) AndAlso _
                Not String.IsNullOrEmpty(ht("fechaFinal").ToString) Then
                .FechaInicial = CDate(ht("fechaInicial").ToString)
                .FechaFinal = CDate(ht("fechaFinal").ToString)
            End If
            dtDatos = .DatosReporte
        End With

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
            .KeyFieldName = "idInfoReferido"
            .DataBind()
        End With
        Session("dtReporteReferidos") = dtDatos
    End Sub

    Public Sub GuardarFiltros()
        Dim ht As New Hashtable
        With ht
            .Add("pdv", ddlPdv.SelectedValue)
            .Add("asesor", ddlAsesor.SelectedValue)
            .Add("fechaInicial", txtFechaInicial.Value)
            .Add("fechaFinal", txtFechaFinal.Value)
        End With
        Session("htFiltros") = ht
    End Sub

    Public Sub InicializarTipoFiltradoEncabezado()
        For Each column As GridViewDataColumn In gvReporte.Columns
            column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList
        Next column
    End Sub

    Protected Sub lbExportar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim infoReferido As New ReporteDeReferidos
        Dim resultado As New ResultadoProceso
        Try
            infoReferido.CargarDatos()
            resultado = infoReferido.GenerarReporteEnExcel()
            If resultado.Valor = 0 Then
                ForzarDescargaDeArchivo(resultado.Mensaje, "ReporteGeneralDeReferidos.xlsx")
            Else
                epNotificador.showWarning(resultado.Mensaje)
            End If
        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            epNotificador.showError("Error al tratar de exportar reporte. ")
        End Try
    End Sub

    Protected Sub cbFormatoExportar_ButtonClick(ByVal source As Object, ByVal e As DevExpress.Web.ButtonEditClickEventArgs) Handles cbFormatoExportar.ButtonClick
        Try
            ObtenerDatosReporte()
            Dim formato As String = cbFormatoExportar.Value
            If Not String.IsNullOrEmpty(formato) Then
                With gveExportador
                    .FileName = "ReporteGeneralDeReferidos"
                    .ReportHeader = "Reporte General de Referidos" & vbCrLf & vbCrLf
                    .ReportFooter = vbCrLf & "Logytech Mobile S.A.S"
                    .Landscape = False
                    With .Styles.Default.Font
                        .Name = "Arial"
                        .Size = FontUnit.Point(10)
                    End With
                End With
                Select Case formato
                    Case "xls"
                        ' No se requieren estas opciones de exportación, pero que pueden hacer la xls resultado personalizable
                        Dim ExportOptions As DevExpress.XtraPrinting.XlsExportOptions = New XlsExportOptions()
                        ExportOptions.ExportHyperlinks = False
                        ' ExportOptions.UseNativeFormat = False
                        ' Asegúrese de volver a enlazar los datos aquí si no estaba obligado al cargar la página
                        ' Aquí también se puede ocultar las columnas que no desea exportar
                        ' Decir. aspxGrid.Columns ["dontExportColumn"] Visible = false.;
                        Response.ClearContent()
                        Response.ClearHeaders()
                        Response.Buffer = True
                        Response.AppendHeader("cache-control", "no-transform")
                        gveExportador.WriteXlsToResponse("GridExport", True, ExportOptions)
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                        Response.End()

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

    Private Sub InicializarLinksSelectorFechas()
        Dim tagLinkFechaInicial As String = "<a hidefocus onclick=""if(self.gfPop)gfPop.fStartPop(document.getElementById('" & txtFechaInicial.ClientID & "')," & _
            "document.getElementById('" & txtFechaFinal.ClientID & "'));return false;"" href='javascript:void(0)'>" & _
            "<img class='PopcalTrigger' height='22' alt='Seleccione una Fecha Inicial' src='../ControlesDeUsuario/DateRange/calbtn.gif' width='34' align='middle' border='0'></a>"

        Dim tagLinkFechaFinal As String = "<a hidefocus onclick=""if(self.gfPop)gfPop.fEndPop(document.getElementById('" & txtFechaInicial.ClientID & "')," & _
            "document.getElementById('" & txtFechaFinal.ClientID & "'));return false;""href='javascript:void(0)'>" & _
            "<img class='PopcalTrigger' height='22' alt='Seleccione una Fecha Final' src='../ControlesDeUsuario/DateRange/calbtn.gif' width='34' align='middle' border='0'></a>"

        ltLinkFechaInicial.Text = tagLinkFechaInicial
        ltLinkFechaFinal.Text = tagLinkFechaFinal
    End Sub

End Class