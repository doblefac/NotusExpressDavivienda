Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports System.Web
Imports System.IO
Imports GemBox.Spreadsheet
Imports System.Drawing

Namespace Reportes
    Public Class ReporteRealceClienteExterno

#Region "Atributos (Campos)"

        Private _idBase As Integer
        Private _idCampania As Integer
        Private _fechaInicial As Date
        Private _fechaFinal As Date
        Private _reportado As Boolean
        Private _datosReporte As DataTable
        Private _dsReporteGestion As DataSet
        Private _datosReportarVentas As DataTable
        Private _rutaArchivo As String
        Private _idUsuarioConsulta As Integer
        Private _estadoCalidad As Integer

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
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

        Public Property IdCampania As Integer
            Get
                Return _idCampania
            End Get
            Set(value As Integer)
                _idCampania = value
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

        Public Property Reportado As Boolean
            Get
                Return _reportado
            End Get
            Set(value As Boolean)
                _reportado = value
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

        Public Property DatosReporteVentasCall() As DataSet
            Get
                If _dsReporteGestion Is Nothing Then CargarDatosVentasCall()
                Return _dsReporteGestion
            End Get
            Set(ByVal value As DataSet)
                _dsReporteGestion = value
            End Set
        End Property

        Public Property DatosReportarVentas() As DataTable
            Get
                If _datosReportarVentas Is Nothing Then CargarReportarDatos()
                Return _datosReportarVentas
            End Get
            Set(ByVal value As DataTable)
                _datosReportarVentas = value
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

        Public Property IdUsuarioConsulta As Integer
            Get
                Return _idUsuarioConsulta
            End Get
            Set(value As Integer)
                _idUsuarioConsulta = value
            End Set
        End Property

        Public Property EstadoCalidad As Integer
            Get
                Return _estadoCalidad
            End Get
            Set(value As Integer)
                _estadoCalidad = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idBase > 0 Then .SqlParametros.Add("@idBase", SqlDbType.Int).Value = _idBase
                    If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If _fechaInicial <> Date.MinValue And _fechaFinal <> Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    End If
                    If _idUsuarioConsulta > 0 Then
                        .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuarioConsulta
                    End If
                    .SqlParametros.Add("@reportado", SqlDbType.Bit).Value = _reportado
                    .TiempoEsperaComando = 600
                    _datosReporte = .EjecutarDataTable("ObtenerReporteRealceClienteExterno", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub CargarDatosVentasCall()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _estadoCalidad > 0 Then .SqlParametros.Add("@estadoCalidad", SqlDbType.Int).Value = _estadoCalidad
                    If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If _fechaInicial <> Date.MinValue And _fechaFinal <> Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    End If
                    If _idUsuarioConsulta > 0 Then
                        .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuarioConsulta
                    End If
                    .TiempoEsperaComando = 600
                    _dsReporteGestion = .EjecutarDataSet("ReporteVentasGestionComercial", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub CargarReportarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idBase > 0 Then .SqlParametros.Add("@idBase", SqlDbType.Int).Value = _idBase
                    If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If _fechaInicial <> Date.MinValue And _fechaFinal <> Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    End If
                    If _idUsuarioConsulta > 0 Then
                        .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuarioConsulta
                    End If
                    .TiempoEsperaComando = 600
                    _datosReportarVentas = .EjecutarDataTable("ReportarObtenerReporteRealceClienteExterno", CommandType.StoredProcedure)
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
            miWs = miExcel.Worksheets.Add("Planilla")
            miWs.Cells("A1").Value = "Reporte Gestión de Ventas"
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
            miWs.Cells(filaInicial, colInicial).Value = "Canal"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha Venta"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Número de Cedula"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Tipo ID Cliente"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Apellido 1"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Apellido 2"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Nombre 1"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Nombre 2"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Dirección"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Teléfono"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Celular"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Ciudad"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Producto Aceptado"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Actividad Laboral"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Cupo"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Codigo Oficina"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Codigo Estrategia"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Codigo Agente Vendedor"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha de Envío"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha Agendamiento"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Estado Venta"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "ID Asesor Venta"
            colFinal = colInicial
            Me.PintarTitulosCeldas(2, 0, 2, 22, Color.Red, miWs)
            For i As Integer = 0 To 22
                With miWs.Cells(2, i)
                    With .Style
                        .HorizontalAlignment = HorizontalAlignmentStyle.Center
                        .Font.Color = Color.White
                    End With
                End With
            Next
            miWs.Panes = New WorksheetPanes(PanesState.Frozen, 0, 3, "A4", PanePosition.BottomLeft)
            If _datosReporte.Rows.Count > 0 Then
                'Cuerpo del Reporte
                filaInicial = 3
                For i As Integer = 0 To _datosReporte.Rows.Count - 1
                    colInicial = 0
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("canal")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("FechaGestion")
                    miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("numeroDocumento")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("tipoDocumento")
                    miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("primerApellido")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("segundoApellido")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("nombres").ToString.Split(" ").GetValue(0)
                    colInicial += 1
                    If _datosReporte.Rows(i).Item("nombres").ToString.Split(" ").Count > 1 Then
                        miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("nombres").ToString.Split(" ").GetValue(1)
                    Else
                        miWs.Cells(filaInicial, colInicial).Value = ""
                    End If
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("direccionResidencia")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("telefonoResidencia")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("celular")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("ciudad")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("proceso_producto")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("actividadLaboral")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("cupo")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("codOficina1")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("codigoEstrategia")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("codigoAgente")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("fechaEnvio")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("fechaAgendamiento")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("nombreEstado")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _datosReporte.Rows(i).Item("idAgente")
                    colInicial += 1
                    Dim fila As Integer = filaInicial + 1
                    filaInicial = fila
                Next
            End If

            miWs.Cells(filaInicial, 0).Value = "Cantidad de registros: " & _datosReporte.Rows.Count
            Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, 1, Color.Red, miWs, True)
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

            _rutaArchivo = RutaArchivo & "Reporte Gestion Ventas Call" & ".xlsx"
            miExcel.SaveXlsx(_rutaArchivo)
            resultado = New ResultadoProceso
            resultado.Valor = 0
            resultado.Mensaje = "Se ha generado el archivo correctamente"
            Return resultado
        End Function

        Private Function GenerarInformeExcelGestionVentas(ByVal ruta As String, ByVal rutaImagen As String, ByVal RutaArchivo As String) As ResultadoProceso
            Dim miWs As ExcelWorksheet
            Dim miExcel As New ExcelFile
            Dim colInicial As Integer = 1
            Dim filaInicial As Integer = 2
            Dim colFinal As Integer = 0
            Dim resultado As New ResultadoProceso
            Dim dvDatos As New DataView
            Dim indice As Integer = 0
            'Encabezado 
            miWs = miExcel.Worksheets.Add("Planilla")
            miWs.Cells("A1").Value = "Reporte Gestión de Ventas"
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
            miWs.Cells(filaInicial, colInicial).Value = "Canal"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha Venta"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Número de Cedula"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Tipo ID Cliente"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Apellido 1"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Apellido 2"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Nombre 1"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Nombre 2"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Dirección"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Teléfono"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Celular"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Ciudad"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Producto Aceptado"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Actividad Laboral"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Cupo"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Codigo Oficina"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Codigo Estrategia"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Codigo Agente Vendedor"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha de Envío"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Fecha Agendamiento"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Estado Venta"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "Observaciones"
            colInicial = colInicial + 1
            miWs.Cells(filaInicial, colInicial).Value = "ID Asesor Venta"
            colFinal = colInicial
            Me.PintarTitulosCeldas(2, 0, 2, 22, Color.Red, miWs)
            For i As Integer = 0 To 22
                With miWs.Cells(2, i)
                    With .Style
                        .HorizontalAlignment = HorizontalAlignmentStyle.Center
                        .Font.Color = Color.White
                    End With
                End With
            Next
            miWs.Panes = New WorksheetPanes(PanesState.Frozen, 0, 3, "A4", PanePosition.BottomLeft)
            If _dsReporteGestion.Tables(0).Rows.Count > 0 Then
                'Cuerpo del Reporte
                filaInicial = 3
                For i As Integer = 0 To _dsReporteGestion.Tables(0).Rows.Count - 1
                    colInicial = 0
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("canal")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("FechaGestion")
                    miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("numeroDocumento")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("tipoDocumento")
                    miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("primerApellido")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("segundoApellido")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("nombres").ToString.Split(" ").GetValue(0)
                    colInicial += 1
                    If _dsReporteGestion.Tables(0).Rows(i).Item("nombres").ToString.Split(" ").Count > 1 Then
                        miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("nombres").ToString.Split(" ").GetValue(1)
                    Else
                        miWs.Cells(filaInicial, colInicial).Value = ""
                    End If
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("direccionResidencia")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("telefonoResidencia")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("celular")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("ciudad")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("proceso_producto")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("actividadLaboral")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("cupo")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("codOficina1")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("codigoEstrategia")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("codigoAgente")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("fechaEnvio")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("fechaAgendamiento")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("nombreEstado")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("observacionCallCenter")
                    colInicial += 1
                    miWs.Cells(filaInicial, colInicial).Value = _dsReporteGestion.Tables(0).Rows(i).Item("idAgente")
                    colInicial += 1
                    Dim fila As Integer = filaInicial + 1
                    filaInicial = fila
                Next
            End If

            miWs.Cells(filaInicial, 0).Value = "Cantidad de registros: " & _dsReporteGestion.Tables(0).Rows.Count
            Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, 1, Color.Red, miWs, True)
            With miWs.Cells(filaInicial, 0)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                    .Font.Color = Color.White
                End With
            End With

            filaInicial += 1

            For Each dr As DataRow In _dsReporteGestion.Tables(1).Rows
                miWs.Cells(filaInicial, 0).Value = _dsReporteGestion.Tables(1).Rows(indice).Item("nombreEstado") & ": " & _dsReporteGestion.Tables(1).Rows(indice).Item("Cantidad")
                Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, 1, Color.Red, miWs, True)
                With miWs.Cells(filaInicial, 0)
                    With .Style
                        .Font.Weight = ExcelFont.BoldWeight
                        .HorizontalAlignment = HorizontalAlignmentStyle.Left
                        .Font.Color = Color.White
                    End With
                End With
                filaInicial += 1
                indice += 1
            Next

            For i As Integer = 0 To colFinal
                miWs.Columns(i).AutoFit()
            Next

            _rutaArchivo = RutaArchivo & "Reporte Gestion Ventas Call" & ".xlsx"
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

        Public Function GenerarReporteExcelGestionVentas() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim FolderTempImage As String
            If _dsReporteGestion.Tables(0).Rows.Count > 0 Then
                Dim contexto As HttpContext = HttpContext.Current
                Dim fullPath As String
                FolderTempImage = Guid.NewGuid().ToString()
                fullPath = contexto.Server.MapPath("~/Reportes/Archivos/")
                System.IO.Directory.CreateDirectory(contexto.Server.MapPath("~/Reportes/Archivos/") & FolderTempImage)
                Dim ruta As String = fullPath & FolderTempImage
                Dim resul As ResultadoProceso
                resul = GenerarInformeExcelGestionVentas(fullPath, ruta & "\", fullPath)
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