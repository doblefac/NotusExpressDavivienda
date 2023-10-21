Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports GemBox.Spreadsheet
Imports System.Drawing
Imports System.Web

Namespace Reportes

    Public Class ReporteDeReferidos

#Region "Atributos (Campos)"

        Private _idPuntoDeVenta As Integer
        Private _idAsesorComercial As Integer
        Private _fechaInicial As Date
        Private _fechaFinal As Date
        Private _datosReporte As DataTable

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal fechaInicial As Date, ByVal fechaFinal As Date)
            MyBase.New()
            _fechaInicial = fechaInicial
            _fechaFinal = fechaFinal
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdPuntoDeVenta() As Integer
            Get
                Return _idPuntoDeVenta
            End Get
            Set(ByVal value As Integer)
                _idPuntoDeVenta = value
            End Set
        End Property

        Public Property IdAsesorComercial() As Integer
            Get
                Return _idAsesorComercial
            End Get
            Set(ByVal value As Integer)
                _idAsesorComercial = value
            End Set
        End Property

        Public Property FechaInicial() As Date
            Get
                Return _fechaInicial
            End Get
            Set(ByVal value As Date)
                _fechaInicial = value
            End Set
        End Property

        Public Property FechaFinal() As Date
            Get
                Return _fechaFinal
            End Get
            Set(ByVal value As Date)
                _fechaFinal = value
            End Set
        End Property

        Public ReadOnly Property DatosReporte() As DataTable
            Get
                If _datosReporte Is Nothing Then CargarDatos()
                Return _datosReporte
            End Get
        End Property

#End Region

#Region "Métodos Privados"

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idPuntoDeVenta > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPuntoDeVenta
                    If _idAsesorComercial > 0 Then .SqlParametros.Add("@idAsesor", SqlDbType.Int).Value = _idAsesorComercial
                    If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    End If
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ReporteDeReferidos", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Function GenerarReporteEnExcel() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            CargarLicenciaGembox()
            Dim oExcel As New ExcelFile
            Dim oWs As ExcelWorksheet
            Dim idUsuario As Integer

            Dim rutaPlantilla As String = HttpContext.Current.Server.MapPath("~/Reportes/Plantillas/PlanillaReferidos.xlsx")
            Dim nombreArchivo As String = ""

            resultado.EstablecerMensajeYValor(1, "Imposible Generar el archivo. Por favor intente nuevamente.")
            If System.IO.File.Exists(rutaPlantilla) Then
                oExcel.LoadXlsx(rutaPlantilla, XlsxOptions.PreserveMakeCopy)
                oWs = oExcel.Worksheets.ActiveWorksheet
                oWs.InsertDataTable(_datosReporte, 4, 0, False)
            Else
                oWs = oExcel.Worksheets.Add("Reporte")
                'With oWs.Cells("A1")
                '    .Value = "Reporte de Seriales Enviados Pendientes por Nacionalización"
                '    .Style.Font.Weight = ExcelFont.BoldWeight
                '    .Style.Font.Size = 20 * 16
                'End With

                'oWs.InsertDataTable(_datosReporte, 2, 0, True)
                'Dim myStyle As New CellStyle
                'With myStyle
                '    .Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin)
                '    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                '    .Font.Weight = ExcelFont.BoldWeight
                '    .FillPattern.SetSolid(ColorTranslator.FromHtml("gray"))
                '    .Font.Color = Color.White
                'End With
                'oWs.Cells.GetSubrange("A3", CellRange.RowColumnToPosition(2, _datosReporte.Columns.Count - 1)).Style = myStyle
                'oWs.Cells.GetSubrange("A1", CellRange.RowColumnToPosition(0, _datosReporte.Columns.Count - 1)).Merged = True
            End If
            For index As Integer = 0 To _datosReporte.Columns.Count - 1
                oWs.Columns(index).AutoFitAdvanced(1.1)
            Next

            With HttpContext.Current
                If .Session("userId") IsNot Nothing Then Integer.TryParse(.Session("userId").ToString, idUsuario)
                nombreArchivo = .Server.MapPath("~/Reportes/ReporteGeneralReferidos_" & idUsuario.ToString & ".xlsx")
                oExcel.SaveXlsx(nombreArchivo)
            End With

            If System.IO.File.Exists(nombreArchivo) Then
                resultado.EstablecerMensajeYValor(0, nombreArchivo)
            Else
                resultado.EstablecerMensajeYValor(2, "No fue posible almacenar el archivo en el servidor. Por favor intente nuevamente.")
            End If
            Return resultado
        End Function

#End Region
    End Class

End Namespace