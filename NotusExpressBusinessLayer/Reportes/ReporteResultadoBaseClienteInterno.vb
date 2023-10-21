Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports System.Web
Imports System.IO
Imports GemBox.Spreadsheet
Imports System.Drawing

Namespace Reportes

    Public Class ReporteResultadoBaseClienteInterno

#Region "Atributos (Campos)"

        Private _idBase As Integer
        Private _datosReporte As DataTable
        Private _rutaArchivo As String

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idbase As Integer)
            _idBase = idbase
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdBase As Integer
            Get
                Return _idBase
            End Get
            Set(ByVal value As Integer)
                _idBase = value
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
                    If _idBase > 0 Then .SqlParametros.Add("@idBase", SqlDbType.Int).Value = _idBase
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ObtenerReporteResultadoBaseClienteInterno", CommandType.StoredProcedure)
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
            miWs.Cells("A1").Value = "Reporte Resultado Base de Clientes (Interno)"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                    .Font.Color = Color.Black
                End With
            End With
            Me.PintarTitulosCeldas(0, 0, 0, 2, Color.White, miWs, True)
            colInicial = 0
            filaInicial = 2
            miWs.Cells(filaInicial, colInicial).Value = "Nombre del Archivo"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Codigo Estrategia"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Campaña"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Tipo de Campaña"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Ciudad"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Cedula"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Tipo Documento Cliente"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Producto1"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Cupo o Prima ACEPTADO POR EL CLIENTE 1"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Producto2"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Cupo o Prima ACEPTADO POR EL CLIENTE 2"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Producto3"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Cupo o Prima ACEPTADO POR EL CLIENTE 3"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Producto4"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Cupo o Prima ACEPTADO POR EL CLIENTE 4"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Indicador de Barrido"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha Gestión Telefonica"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha Contacto"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha Cita"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha Radicación"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Resultado de la Gestión"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Estado de la Gestión"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Estado Final"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Observaciones"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Nombre Outsourcing"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Nombre Asesor"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "idAsesor"
            colFinal = colInicial
            Me.PintarTitulosCeldas(2, 0, 2, 0, Color.Chocolate, miWs)
            Me.PintarTitulosCeldas(2, 1, 2, 1, Color.DarkSalmon, miWs)
            Me.PintarTitulosCeldas(2, 2, 2, 6, Color.Chocolate, miWs)
            Me.PintarTitulosCeldas(2, 7, 2, 14, Color.Green, miWs)
            Me.PintarTitulosCeldas(2, 15, 2, 15, Color.DarkSalmon, miWs)
            Me.PintarTitulosCeldas(2, 16, 2, 19, Color.Orange, miWs)
            Me.PintarTitulosCeldas(2, 20, 2, 22, Color.Chocolate, miWs)
            Me.PintarTitulosCeldas(2, 23, 2, 23, Color.Yellow, miWs)
            Me.PintarTitulosCeldas(2, 24, 2, 24, Color.Red, miWs)
            Me.PintarTitulosCeldas(2, 25, 2, 26, Color.Green, miWs)
            miWs.Panes = New WorksheetPanes(PanesState.Frozen, 0, 3, "A4", PanePosition.BottomLeft)
            If _datosReporte.Rows.Count > 0 Then
                'Cuerpo del Reporte
                filaInicial = 3
                For i As Integer = 0 To _datosReporte.Rows.Count - 1
                    colInicial = 0
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("nombreArchivo")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("codigoEstrategia")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("nombreCampania")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("tipoCampania")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("ciudad")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("numeroIdentificacion")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("tipoDocumento")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("producto1")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("cupo1")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("producto2")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("cupo2")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("producto3")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("cupo3")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("producto4")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("cupo4")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("barrido")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("fechaGestion")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("fechaContacto")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("fechaAgenda")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("fechaRadicacion")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("resultadoGestion")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("estadoGestion")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("estadoFinal")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("observaciones")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("nombreOutsourcing")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("asesor")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("idAsesor")
                    Dim fila As Integer = filaInicial + 1
                    filaInicial = fila
                Next
            End If
            For i As Integer = 0 To colFinal
                miWs.Columns(i).AutoFit()
            Next

            miWs.Cells(filaInicial, 0).Value = "Cantidad de registros: " & _datosReporte.Rows.Count
            With miWs.Cells(filaInicial, 0)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                    .Font.Color = Color.Black
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, 2, Color.White, miWs, True)
            _rutaArchivo = RutaArchivo & "Reporte Resultado Base clientes (Interno)" & ".xlsx"
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
