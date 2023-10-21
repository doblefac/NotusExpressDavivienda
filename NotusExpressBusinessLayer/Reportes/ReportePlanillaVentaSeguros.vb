Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports System.Web
Imports System.IO
Imports GemBox.Spreadsheet
Imports System.Drawing

Namespace Reportes
    Public Class ReportePlanillaVentaSeguros

#Region "Atributos (Campos)"

        Private _numIdentificacion As String
        Private _idAsesor As Integer
        Private _fechaInicial As Date
        Private _fechaFinal As Date
        Private _datosReporte As DataTable
        Private _datosSeguros As DataTable
        Private _rutaArchivo As String
        Private _dsDatos As DataSet

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Public Property NumIdentificacion As String
            Get
                Return _numIdentificacion
            End Get
            Set(ByVal value As String)
                _numIdentificacion = value
            End Set
        End Property

        Public Property IdAsesor As Integer
            Get
                Return _idAsesor
            End Get
            Set(value As Integer)
                _idAsesor = value
            End Set
        End Property

        Public Property FechaInicial As Date
            Get
                Return _fechaInicial
            End Get
            Set(value As Date)
                _fechaInicial = value
            End Set
        End Property

        Public Property FechaFinal As Date
            Get
                Return _fechaFinal
            End Get
            Set(value As Date)
                _fechaFinal = value
            End Set
        End Property

        Public Property DatosReporte() As DataTable
            Get
                Return _datosReporte
            End Get
            Set(ByVal value As DataTable)
                _datosReporte = value
            End Set
        End Property

        Public Property DatosSeguros() As DataTable
            Get
                Return _datosSeguros
            End Get
            Set(value As DataTable)
                _datosSeguros = value
            End Set
        End Property

        Public Property DsDatosReporte() As DataSet
            Get
                If _dsDatos Is Nothing Then CargarDatos()
                Return _dsDatos
            End Get
            Set(value As DataSet)
                _dsDatos = value
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
                    If _numIdentificacion IsNot Nothing Then .SqlParametros.Add("@numIdentificacion", SqlDbType.VarChar).Value = _numIdentificacion
                    If _idAsesor > 0 Then .SqlParametros.Add("@idAsesor", SqlDbType.Int).Value = _idAsesor
                    If _fechaInicial <> Date.MinValue And _fechaFinal <> Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    End If
                    .TiempoEsperaComando = 600
                    _dsDatos = .EjecutarDataSet("ObtenerReportePlanillaVentaSeguros", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Function GenerarInformeExcel(ByVal ruta As String, ByVal rutaImagen As String, ByVal RutaArchivo As String) As ResultadoProceso
            Dim miWs As ExcelWorksheet
            Dim miExcel As New ExcelFile
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim resultado As New ResultadoProceso
            Dim dvDatos As New DataView

            For a As Integer = 0 To _datosSeguros.Rows.Count - 1
                miWs = miExcel.Worksheets.Add(_datosSeguros.Rows(a).Item(0))
                colInicial = 1
                filaInicial = 1
                miWs.Cells(filaInicial, colInicial).Value = "Fecha Venta"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Tipo ID Cliente"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "ID Cliente"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Nombres Cliente"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Primer Apellidos Cliente"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Segundo Apellidos Cliente"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Fecha Nacimiento"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Sexo"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "TDC Ó Cuenta"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Franquicia"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Valor Prima"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Dirección Envio Poliza"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Ciudad/Municipio"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Departamento"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Teléfono Fijo"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Teléfono Celular"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "E-Mail"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "id Asesor"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Nombre Completo Asesor"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Observación"
                colFinal = colInicial
                For i As Integer = 1 To colFinal
                    With miWs.Cells(filaInicial, i)
                        With .Style
                            .Font.Weight = ExcelFont.BoldWeight
                            .HorizontalAlignment = HorizontalAlignmentStyle.Center
                            .Font.Color = Color.White
                        End With
                    End With
                    Me.PintarTitulosCeldas(filaInicial, i, filaInicial, i, Color.DarkBlue, miWs, False)
                Next
                Dim dr() As DataRow = _datosReporte.Select("Producto='" & _datosSeguros.Rows(a).Item(0) & "'")
                If dr.Count > 0 Then
                    'Cuerpo del Reporte
                    filaInicial = 2
                    For i As Integer = 0 To dr.Count - 1
                        colInicial = 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("fechaVenta")
                        miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("tipoIdentificacion")
                        miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("numeroIdentificacion")
                        miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("nombres")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("primerApellido")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("segundoApellido")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("fechaNacimiento")
                        miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("sexo")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("cuenta")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("franquicia")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("valorPrimaServicio")
                        miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("direccionResidencia")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("ciudad")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("departamento")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("telefonoResidencia")
                        miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("celular")
                        miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("email")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("numeroIdentificacionAsesor")
                        miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("nombreApellidoAsesor")
                        colInicial += 1
                        miWs.Cells(filaInicial, colInicial).Value = dr(i).Item("observacion")
                        Dim fila As Integer = filaInicial + 1
                        filaInicial = fila
                    Next
                Else
                    filaInicial = filaInicial + 1
                End If
                miWs.Cells(filaInicial, 1).Value = "Cantidad de registros: " & dr.Count
                Me.PintarTitulosCeldas(filaInicial, 1, filaInicial, 2, Color.DarkBlue, miWs, True)
                miWs.Cells(filaInicial, 1).Style.Font.Color = Color.White
                With miWs.Cells(filaInicial, 0)
                    With .Style
                        .Font.Weight = ExcelFont.BoldWeight
                        .HorizontalAlignment = HorizontalAlignmentStyle.Left
                        .Font.Color = Color.White
                    End With
                End With
                For i As Integer = 0 To colFinal
                    miWs.Columns(i).AutoFit()
                Next
            Next
            _rutaArchivo = RutaArchivo & "Reporte Planilla Venta Seguros" & ".xlsx"
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