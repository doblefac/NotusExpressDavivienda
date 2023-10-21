Imports System.IO
Imports GemBox.Spreadsheet
Imports NotusExpressBusinessLayer.Localizacion

Public Class DescargaDocumento
    Inherits System.Web.UI.Page

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("id") IsNot Nothing Then
            Dim id As Long = CLng(Request.QueryString("id"))
            CargarLicenciaGembox()
            Select Case id
                Case "1"
                    ConsutaMaestroCiudades()
                    If Session("dtCiudad") IsNot Nothing Then
                        Dim dtReporte As DataTable = CType(Session("dtCiudad"), DataTable)
                        DescargarReporte(dtReporte)
                    End If
                Case "2"
                    If Session("dtReporteExtendido") IsNot Nothing Then
                        Dim dtReporte As DataTable = CType(Session("dtReporteExtendido"), DataTable)
                        DescargarReporteExtendidoLegalizacion(dtReporte)
                    End If
            End Select
        End If
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub ConsutaMaestroCiudades()
        Dim infoCiudad As New CiudadColeccion
        Dim dtCiudad As New DataTable
        With infoCiudad
            .IdPais = 170
            dtCiudad = .GenerarDataTable
        End With
        Session("dtCiudad") = dtCiudad
    End Sub

    Private Sub DescargarReporte(ByVal dtReporte As DataTable)
        Dim nombreArchivo As String = GenerarArchivoExcelDiferencias(dtReporte)
        If System.IO.File.Exists(nombreArchivo) Then
            ForzarDescargaDeArchivo(nombreArchivo)
        End If
    End Sub

    Private Function GenerarArchivoExcelDiferencias(ByVal dtReporte As DataTable)
        Dim rutaPlantilla As String = Server.MapPath("~/GestionComercial/Archivos/PlantillaCiudades.xlsx")
        Dim nombreArchivo As String = Server.MapPath("~/ArchivosCargados/Ciudades" & Session("userId") & ".xls")
        Dim miExcel As New ExcelFile
        Dim miHoja As ExcelWorksheet
        Dim numRegistros As Integer = 0
        If File.Exists(rutaPlantilla) Then
            miExcel.LoadXlsx(rutaPlantilla, XlsxOptions.PreserveMakeCopy)
            miHoja = miExcel.Worksheets.ActiveWorksheet
            numRegistros = miHoja.InsertDataTable(dtReporte, "A5", False)
            'If numRegistros = 0 Then miEncabezado.showWarning("No fue posible adicionar datos al archivo. Por favor intente nuevamente.")
        Else
            miHoja = miExcel.Worksheets.Add("Maestro de ciudades")
            With miHoja
                numRegistros = miHoja.InsertDataTable(dtReporte, "A5", True)
            End With
        End If
        For index As Integer = 0 To dtReporte.Columns.Count - 1
            miHoja.Columns(index).AutoFitAdvanced(1.1)
        Next
        miExcel.SaveXls(nombreArchivo)
        Return nombreArchivo
    End Function

    Private Sub DescargarReporteExtendidoLegalizacion(ByVal dtReporte As DataTable)
        Dim nombreArchivo As String = GenerarArchivoExcelExtendidoLegalizacion(dtReporte)
        If System.IO.File.Exists(nombreArchivo) Then
            ForzarDescargaDeArchivo(nombreArchivo)
        End If
    End Sub

    Private Function GenerarArchivoExcelExtendidoLegalizacion(ByVal dtReporte As DataTable)
        Dim nombreArchivo As String = Server.MapPath("~/ArchivosCargados/Extendido" & Session("userId") & ".xls")
        Dim miExcel As New ExcelFile
        Dim miHoja As ExcelWorksheet
        Dim numRegistros As Integer = 0

        miHoja = miExcel.Worksheets.Add("Reporte Extendido")
        With miHoja
            numRegistros = miHoja.InsertDataTable(dtReporte, "A1", True)
        End With

        For index As Integer = 0 To dtReporte.Columns.Count - 1
            miHoja.Columns(index).AutoFitAdvanced(1.1)
        Next
        miExcel.SaveXls(nombreArchivo)
        Return nombreArchivo
    End Function

#End Region

End Class