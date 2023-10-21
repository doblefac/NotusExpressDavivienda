Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Text
Imports System.Web

Namespace General

    Public Class CargadorArchivoMetasComercialesPorAsesor
#Region "Atributos"

        Private _nombreArchivo As String
        Private _dtDatosArchivo As DataTable
        Private _dtError As DataTable
        Private _hayError As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            CargarLicenciaGembox()
        End Sub

        Public Sub New(nombreArchivo As String)
            CargarLicenciaGembox()
            _nombreArchivo = nombreArchivo
        End Sub

#End Region

#Region "Propiedades"

        Public Property NombreArchivo As String
            Get
                Return _nombreArchivo
            End Get
            Set(value As String)
                _nombreArchivo = value
            End Set
        End Property

        Public ReadOnly Property TablaErrores As DataTable
            Get
                If _dtError Is Nothing Then InicializarTablaDeErrores()
                Return _dtError
            End Get
        End Property

        Public ReadOnly Property HayErrores As Boolean
            Get
                If (_dtError Is Nothing OrElse _dtError.Rows.Count = 0) AndAlso Not _hayError Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub InicializarTablaDatosArchivo()
            _dtDatosArchivo = New DataTable

            With _dtDatosArchivo
                .Columns.Add("lineaArchivo", GetType(Integer))
                .Columns.Add("estrategia", GetType(String))
                .Columns.Add("cedulaAsesor", GetType(String))
                .Columns.Add("nombreAsesor", GetType(String))
                .Columns.Add("tipoProducto", GetType(String))
                .Columns.Add("anio", GetType(Short))
                .Columns.Add("mes", GetType(Short))
                .Columns.Add("meta", GetType(Integer))
            End With
        End Sub

        Private Sub InicializarTablaDeErrores()
            _dtError = New DataTable

            With _dtError
                .Columns.Add("linea", GetType(Integer))
                .Columns.Add("dato", GetType(String))
                .Columns.Add("descripcionError", GetType(String))
            End With
        End Sub

        Private Function ExtraerDatos() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            InicializarTablaDeErrores()
            InicializarTablaDatosArchivo()
            _hayError = True
            If File.Exists(_nombreArchivo) Then
                Dim oExcel As New ExcelFile
                Dim oWs As ExcelWorksheet
                Select Case Path.GetExtension(_nombreArchivo)
                    Case ".csv"
                        oExcel.LoadCsv(_nombreArchivo, CsvType.SemicolonDelimited)
                    Case ".xlsx"
                        oExcel.LoadXlsx(_nombreArchivo, XlsxOptions.PreserveMakeCopy)
                    Case Else
                        oExcel.LoadXls(_nombreArchivo)
                End Select

                oWs = oExcel.Worksheets.ActiveWorksheet
                If oWs.Rows.Count > 0 Then
                    Dim numFilas As Integer = oWs.Rows.Count
                    Dim indicePrimeraFila As Integer = 0
                    Dim indicePrimeraColumna As Integer = oWs.Cells.FirstColumnIndex
                    For Each eRow As ExcelRow In oWs.Rows
                        indicePrimeraFila = eRow.Index + 1
                        If eRow.AllocatedCells IsNot Nothing AndAlso eRow.AllocatedCells.Count > 0 AndAlso EsTitulo(eRow) Then Exit For
                    Next

                    If EsTituloValido(oWs.Rows(indicePrimeraFila - 1)) Then
                        If indicePrimeraFila < oWs.Rows.Count Then
                            For index As Integer = indicePrimeraFila To numFilas - 1
                                If Not EsFilaVacia(oWs.Rows(index)) Then
                                    If FormatoDeFilaEsCorrecto(oWs.Rows(index)) Then
                                        AdicionarDato(oWs.Rows(index))
                                    End If
                                End If
                            Next
                            If _dtError.Rows.Count = 0 Then
                                If _dtDatosArchivo.Rows.Count <= 20000 Then
                                    _hayError = False
                                    resultado.EstablecerMensajeYValor(0, "El formato del archivo es correcto y los tipos de datos son válidos")
                                Else
                                    AdicionarError(oWs.Rows.Count, "El archivo contiene más registros de los permitidos.")
                                End If
                            Else
                                resultado.EstablecerMensajeYValor(1, "El archivo contiene errores a nivel de formato o tipos de datos")
                            End If
                        Else
                            resultado.EstablecerMensajeYValor(506, "No se pudo determinar el inicio de los datos en el archivo, puesto que no se encontró la fila de títulos.")
                        End If
                    Else
                        resultado.EstablecerMensajeYValor(505, "El formato del archivo no es correcto. Los titulos del archivo (o el orden de los mismos) no coinciden con los esperados.")
                    End If
                Else
                    resultado.EstablecerMensajeYValor(504, "El archivo proporcionado no contiene registros. Por favor verificar")
                End If
            Else
                resultado.EstablecerMensajeYValor(503, "No se logro encontrar el archivo cargado en su ruta de almacenamiento en el servidor. Por favor intente nuevamente")
            End If

            Return resultado
        End Function

        Private Function EsTitulo(ByVal eRow As ExcelRow) As Boolean
            Dim resultado As Boolean = False
            Dim arrTitulo As New ArrayList("ESTRATEGIA,CEDULA ASESOR,NOMBRE ASESOR,PRODUCTO,AÑO,MES,META".Split(","))

            For index As Integer = 0 To eRow.AllocatedCells.Count
                If Not EsNuloOVacio(eRow.Cells(index).Value) AndAlso arrTitulo.Contains(eRow.Cells(index).Value.ToString.ToUpper) Then
                    resultado = True
                    Exit For
                End If
            Next
            Return resultado
        End Function

        Private Function EsTituloValido(ByVal eRow As ExcelRow) As Boolean
            Dim resultado As Boolean = True
            Dim arrTitulo As New ArrayList("ESTRATEGIA,CEDULA ASESOR,NOMBRE ASESOR,PRODUCTO,AÑO,MES,META".Split(","))
            For index As Integer = 0 To arrTitulo.Count - 1
                If EsNuloOVacio(eRow.Cells(index).Value) OrElse eRow.Cells(index).Value.ToString.ToUpper <> arrTitulo(index).ToString.ToUpper Then
                    resultado = False
                    Exit For
                End If
            Next

            Return resultado
        End Function

        Private Function FormatoDeFilaEsCorrecto(ByVal eRow As ExcelRow) As Boolean
            Dim resultado As Boolean = True
            With eRow
                If Not EsNuloOVacio(.Cells("A").Value) AndAlso Not EsNuloOVacio(.Cells("B").Value) _
                    AndAlso Not EsNuloOVacio(.Cells("D").Value) AndAlso Not EsNuloOVacio(.Cells("E").Value) _
                    AndAlso Not EsNuloOVacio(.Cells("F").Value) AndAlso Not EsNuloOVacio(.Cells("G").Value) Then

                    Dim oRegExp As New System.Text.RegularExpressions.Regex("^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$")
                    If Not oRegExp.IsMatch(.Cells("A").Value.ToString.Trim) Then
                        AdicionarError(eRow.Index + 1, "El campo estrategia contiene caracteres no válidos.", "ESTRATEGIA")
                        resultado = False
                    End If

                    If Not oRegExp.IsMatch(.Cells("B").Value.ToString.Trim) Then
                        AdicionarError(eRow.Index + 1, "El campo cédula asesor contiene caracteres no válidos.", "CEDULA ASESOR")
                        resultado = False
                    End If

                    If Not oRegExp.IsMatch(.Cells("C").Value.ToString.Trim) Then
                        AdicionarError(eRow.Index + 1, "El campo nombre asesor contiene caracteres no válidos.", "NOMBRE ASESOR")
                        resultado = False
                    End If

                    If Not oRegExp.IsMatch(.Cells("D").Value.ToString.Trim) Then
                        AdicionarError(eRow.Index + 1, "El campo producto contiene caracteres no válidos.", "PRODUCTO")
                        resultado = False
                    End If

                    If Not RegularExpressions.Regex.IsMatch(.Cells("E").Value.ToString.Trim, "^[0-9]{4,4}$") Then
                        AdicionarError(eRow.Index + 1, "El año proporcionado no es válido. Se espera un valor numérico entero de 4 dígitos", "AÑO")
                        resultado = False
                    End If

                    If Not RegularExpressions.Regex.IsMatch(.Cells("F").Value.ToString.Trim, "^[0-9]{1,2}$") _
                        OrElse CInt(.Cells("F").Value) < 1 OrElse CInt(.Cells("F").Value) > 12 Then
                        AdicionarError(eRow.Index + 1, "El mes proporcionado no es válido. Se espera un valor numérico entre 1 y 12", "MES")
                        resultado = False
                    End If

                    If Not RegularExpressions.Regex.IsMatch(.Cells("G").Value.ToString.Trim, "^[0-9]{1,6}$") Then
                        AdicionarError(eRow.Index + 1, "La meta proporcionada no es válida. Se espera un valor numérico entero de máximo 6 dígitos", "META")
                        resultado = False
                    End If
                Else
                    If EsNuloOVacio(.Cells("A").Value) Then _
                        AdicionarError(eRow.Index + 1, "El valor del campo es requerido y no fue proporcionado", "ESTRATEGIA")
                    If EsNuloOVacio(.Cells("B").Value) Then _
                        AdicionarError(eRow.Index + 1, "El valor del campo es requerido y no fue proporcionado", "PUNTO DE VENTA")
                    If EsNuloOVacio(.Cells("D").Value) Then _
                        AdicionarError(eRow.Index + 1, "El valor del campo es requerido y no fue proporcionado", "PRODUCTO")
                    If EsNuloOVacio(.Cells("E").Value) Then _
                        AdicionarError(eRow.Index + 1, "El valor del campo es requerido y no fue proporcionado", "AÑO")
                    If EsNuloOVacio(.Cells("F").Value) Then _
                        AdicionarError(eRow.Index + 1, "El valor del campo es requerido y no fue proporcionado", "MES")
                    If EsNuloOVacio(.Cells("G").Value) Then _
                        AdicionarError(eRow.Index + 1, "El valor del campo es requerido y no fue proporcionado", "META")
                    resultado = False
                End If
            End With
            Return resultado
        End Function

        Private Function EsFilaVacia(ByVal eRow As ExcelRow) As Boolean
            Dim resultado As Boolean = True
            For index As Integer = 0 To eRow.AllocatedCells.Count - 1
                If Not EsNuloOVacio(eRow.Cells(index).Value) Then
                    resultado = False
                    Exit For
                End If
            Next

            Return resultado
        End Function

        Private Sub AdicionarError(numLinea As Integer, mensaje As String, Optional ByVal datoAfectado As String = "")
            If _dtError Is Nothing Then InicializarTablaDeErrores()
            Dim drRow As DataRow = _dtError.NewRow
            drRow("linea") = numLinea
            drRow("dato") = datoAfectado
            drRow("descripcionError") = mensaje
            _dtError.Rows.Add(drRow)
        End Sub

        Private Sub AdicionarDato(eRow As ExcelRow)
            If _dtDatosArchivo Is Nothing Then InicializarTablaDatosArchivo()
            Dim drRow As DataRow = _dtDatosArchivo.NewRow
            With eRow
                drRow("lineaArchivo") = eRow.Index + 1
                drRow("estrategia") = .Cells("A").Value.ToString
                drRow("cedulaAsesor") = .Cells("B").Value.ToString
                drRow("nombreAsesor") = .Cells("C").Value.ToString
                drRow("tipoProducto") = .Cells("D").Value.ToString
                drRow("anio") = .Cells("E").Value.ToString
                drRow("mes") = .Cells("F").Value.ToString
                drRow("meta") = .Cells("G").Value.ToString
            End With
            _dtDatosArchivo.Rows.Add(drRow)
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function ProcesarYRegistrarDatos() As ResultadoProceso
            Dim resultado As ResultadoProceso
            resultado = ExtraerDatos()
            If resultado.Valor = 0 And Not _hayError Then
                Dim identificadorProceso As System.Guid = System.Guid.NewGuid()
                Dim idUsuario As Integer = 1
                Dim dcAux As DataColumn = Nothing
                If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId"), idUsuario)
                If Not _dtDatosArchivo.Columns.Contains("idUsuario") Then
                    dcAux = New DataColumn("idUsuario", GetType(Integer))
                    dcAux.DefaultValue = idUsuario
                    _dtDatosArchivo.Columns.Add(dcAux)
                End If
                If Not _dtDatosArchivo.Columns.Contains("identificadorProceso") Then
                    dcAux = New DataColumn("identificadorProceso", GetType(System.Guid))
                    dcAux.DefaultValue = identificadorProceso
                    _dtDatosArchivo.Columns.Add(dcAux)
                End If

                Dim dbManager As LMDataAccess = Nothing

                Try
                    dbManager = New LMDataAccess
                    With dbManager
                        .iniciarTransaccion()
                        .inicilizarBulkCopy()
                        .TiempoEsperaComando = 300
                        With .BulkCopy
                            .DestinationTableName = "TransitoriaRegistroMetasComercialesAsesor"
                            .ColumnMappings.Add("lineaArchivo", "lineaArchivo")
                            .ColumnMappings.Add("estrategia", "estrategia")
                            .ColumnMappings.Add("cedulaAsesor", "cedulaAsesor")
                            .ColumnMappings.Add("nombreAsesor", "nombreAsesor")
                            .ColumnMappings.Add("tipoProducto", "tipoProducto")
                            .ColumnMappings.Add("anio", "anio")
                            .ColumnMappings.Add("mes", "mes")
                            .ColumnMappings.Add("meta", "meta")
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .ColumnMappings.Add("identificadorProceso", "identificadorProceso")
                            .WriteToServer(_dtDatosArchivo)
                        End With

                        '.SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@identificadorProceso", SqlDbType.UniqueIdentifier).Value = identificadorProceso

                        .llenarDataTable(_dtError, "ValidarDatosRegistroMetasAsesorPorArchivo", CommandType.StoredProcedure)

                        If _dtError Is Nothing OrElse _dtError.Rows.Count = 0 Then
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("RegistrarMetasComercialesDeAsesoresPorCargueDeArchivo", CommandType.StoredProcedure)
                            If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                If resultado.Valor = 0 Then
                                    .SqlParametros.Clear()
                                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = CInt(HttpContext.Current.Session("userId"))
                                    .ejecutarNonQuery("LiberarDatosRegistroTransitorioMetasAsesor", CommandType.StoredProcedure)
                                    If .estadoTransaccional Then .confirmarTransaccion()

                                    If _dtError Is Nothing OrElse _dtError.Rows.Count = 0 Then
                                        resultado.EstablecerMensajeYValor(0, "El registro de metas comerciales fue realizado satisfactoriamente")
                                    End If
                                Else
                                    If .estadoTransaccional Then .abortarTransaccion()
                                End If
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If

            Return resultado
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Shared Sub GenerarYDescargarArchivoDeDatosGuia()
            CargarLicenciaGembox()
            Dim contexto As HttpContext = HttpContext.Current
            Dim oExcel As New ExcelFile()
            Dim rutaPlantilla As String = contexto.Server.MapPath("~/Reportes/Plantillas/DatosGuiaCargarMetasComercialesAsesor.xlsx")
            If File.Exists(rutaPlantilla) Then
                Dim oWs As ExcelWorksheet
                oExcel.LoadXlsx(rutaPlantilla, XlsxOptions.PreserveMakeCopy)

                Dim maestroTipoProducto As New TipoProductoColeccion
                With maestroTipoProducto
                    .IdEstado = 1
                    .CargarDatos()
                End With

                oWs = oExcel.Worksheets("ListaTipoProducto")
                Dim index As Integer = 1
                For Each tipo As TipoProducto In maestroTipoProducto
                    With oWs.Rows(index)
                        .Cells("A").Value = tipo.IdTipoProducto
                        .Cells("B").Value = tipo.Nombre
                    End With
                    index += 1
                Next

                For Each eCol As ExcelColumn In oWs.Columns
                    eCol.AutoFitAdvanced(1.1000000000000001)
                Next

                Dim maestroEstrategias As New EstrategiaComercialColeccion
                With maestroEstrategias
                    .IdEstado = 1
                    .CargarDatos()
                End With

                oWs = oExcel.Worksheets("ListaEstrategias")
                index = 1
                For Each estrategia As EstrategiaComercial In maestroEstrategias
                    With oWs.Rows(index)
                        .Cells("A").Value = estrategia.IdEstrategia
                        .Cells("B").Value = estrategia.Nombre
                    End With
                    index += 1
                Next

                For Each eCol As ExcelColumn In oWs.Columns
                    eCol.AutoFitAdvanced(1.1000000000000001)
                Next

                Dim listaAsesores As New AsesorComercialColeccion
                With listaAsesores
                    .IdEstado = 1
                    .CargarDatos()
                End With

                oWs = oExcel.Worksheets("ListaAsesores")
                index = 1
                For Each asesor As AsesorComercial In listaAsesores
                    With oWs.Rows(index)
                        .Cells("A").Value = asesor.DocIdentificacion
                        .Cells("B").Value = asesor.NombreAsesor
                    End With
                    index += 1
                Next

                For Each eCol As ExcelColumn In oWs.Columns
                    eCol.AutoFitAdvanced(1.1000000000000001)
                Next

                Dim rutaArchivo As String = contexto.Server.MapPath("~/ArchivosCargados/DatosGuiaCargarMetasComercialesAsesor_" & contexto.Session("userId") & ".xlsx")

                oExcel.SaveXlsx(rutaArchivo)
                ForzarDescargaDeArchivo(rutaArchivo, Path.GetFileName(rutaPlantilla))
            Else
                Throw New Exception("No se encontró la plantilla de información guía para cargue de metas comerciales por Asesor. Por favor contracte a IT")
            End If
        End Sub

#End Region

    End Class

End Namespace