Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports System.Web
Imports System.IO
Imports GemBox.Spreadsheet
Imports System.Drawing

Namespace Reportes

    Public Class ReportePresupuestoInterno

#Region "Atributos (Campos)"

        Private _idProducto As Integer
        Private _idBase As Integer
        Private _fechaInicial As Date
        Private _fechaFinal As Date
        Private _datosReporte As DataTable
        Private _rutaArchivo As String

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdProducto As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property IdBase As Integer
            Get
                Return _idBase
            End Get
            Set(ByVal value As Integer)
                _idBase = value
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

        Public Property DatosReporte() As DataTable
            Get
                If _datosReporte Is Nothing Then CargarDatos()
                Return _datosReporte
            End Get
            Set(ByVal value As DataTable)
                _datosReporte = value
            End Set
        End Property

        Public Property RutaArchivo As String
            Get
                Return _rutaArchivo
            End Get
            Set(value As String)
                _rutaArchivo = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    If _idBase > 0 Then .SqlParametros.Add("@idBase", SqlDbType.Int).Value = _idBase
                    If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    End If
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ReporteDePresupuestoInterno", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Function GenerarInformeExcel(ByVal ruta As String, ByVal rutaImagen As String, ByVal RutaArchivo As String) As ResultadoProceso
            Dim miWs As ExcelWorksheet
            Dim miExcel As New ExcelFile
            Dim colInicial As Integer = 1
            Dim filaInicial As Integer = 2
            Dim colFinal As Integer = 0
            Dim resultado As New ResultadoProceso
            Dim dvDatos As New DataView
            'Encabezado 
            miWs = miExcel.Worksheets.Add("Detalle")
            miWs.Cells("A1").Value = "Reporte Detallado Presupuesto (Interno)"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                    .Font.Color = Color.Red
                End With
            End With
            Me.PintarTitulosCeldas(0, 0, 0, 3, Color.White, miWs, True)
            colInicial = 0
            filaInicial = 2
            miWs.Cells(filaInicial, colInicial).Value = "Base de Datos"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Asesor"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "idCliente"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Indicador"
            colFinal = colInicial
            Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, colFinal, Color.Gainsboro, miWs)
            miWs.Panes = New WorksheetPanes(PanesState.Frozen, 0, 3, "A4", PanePosition.BottomLeft)
            If _datosReporte.Rows.Count > 0 Then
                'Cuerpo del Reporte
                filaInicial = 3
                For i As Integer = 0 To _datosReporte.Rows.Count - 1
                    colInicial = 0
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("nombreArchivo")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("asesor")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("idCliente")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("indicador")
                    Dim fila As Integer = filaInicial + 1
                    filaInicial = fila
                Next
            End If
            For i As Integer = 0 To colFinal
                miWs.Columns(i).AutoFit()
            Next
            _rutaArchivo = RutaArchivo & "Reporte Detalle Presupuesto (Interno)" & ".xlsx"
            miExcel.SaveXlsx(_rutaArchivo)
            resultado = New ResultadoProceso
            resultado.Valor = 0
            resultado.Mensaje = "Se ha generado el archivo correctamente"
            Return resultado
        End Function

        Private Sub PintarTitulosCeldas(ByVal filaInicial As Integer, ByVal columnaInicial As Integer, ByVal filaFinal As Integer, ByVal columnaFinal As Integer, ByVal colorFondo As Color, ByVal miWS As ExcelWorksheet, Optional ByVal merge As Boolean = False, Optional ByVal alineacion As HorizontalAlignmentStyle = HorizontalAlignmentStyle.Center)
            Dim cr As CellRange = miWS.Cells.GetSubrangeAbsolute(filaInicial, columnaInicial, filaFinal, columnaFinal)
            cr.Merged = merge
            For Each cel As ExcelCell In cr
                With cel.Style
                    .FillPattern.SetPattern(FillPatternStyle.Solid, colorFondo, colorFondo)
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                    .Borders.SetBorders(MultipleBorders.Top, Color.Gray, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Right, Color.Gray, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Left, Color.Gray, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.Gray, LineStyle.Thin)
                    .HorizontalAlignment = alineacion
                End With
            Next
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function GenerarReporteExcel() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim FolderTempImage As String
            If _datosReporte.Rows.Count > 0 Then
                Dim contexto As HttpContext = HttpContext.Current
                Dim fullPath As String
                FolderTempImage = Guid.NewGuid().ToString()
                fullPath = contexto.Server.MapPath("~/Reportes/Archivos/")
                System.IO.Directory.CreateDirectory(contexto.Server.MapPath("~/Reportes/Archivos/") & FolderTempImage)
                Dim ruta As String = fullPath & FolderTempImage
                Dim resul As ResultadoProceso
                resul = GenerarInformeExcel(fullPath, ruta & "\", fullPath)
                System.IO.Directory.Delete(ruta, True)
                If resul.Valor <> 0 Then
                    resultado.EstablecerMensajeYValor(1, "No se pudo generar el reporte, por favor intentelo nuevamente.")
                End If
            Else
                resultado.EstablecerMensajeYValor(1, "No existen registros para exportar")
            End If
            Return resultado
        End Function

#End Region

    End Class

End Namespace
