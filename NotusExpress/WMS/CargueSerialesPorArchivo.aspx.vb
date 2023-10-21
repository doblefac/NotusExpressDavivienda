Imports DevExpress.Web
Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Data.OleDb
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General

Public Class CargueSerialesPorArchivo
    Inherits System.Web.UI.Page



#Region "Atributos"

    Dim idRol As Integer
    Private oExcel As ExcelFile
    Private Const UploadDirectory As String = "~\archivos_planos\"

#End Region

#Region "Propiedades"

    Public ReadOnly Property ContieneErrores() As Boolean
        Get
            If Session("dtError") Is Nothing Then
                Return False
            Else
                Return (CType(Session("dtError"), DataTable).Rows.Count > 0)
            End If
        End Get
    End Property


#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("idRol") IsNot Nothing Then Integer.TryParse(Session("idRol").ToString, idRol)
            miEncabezado.clear()

            If Not IsPostBack Then
                With miEncabezado
                    .showReturnLink("~/Administracion/Default.aspx")
                    .setTitle("Cargue de Seriales a Inventario - Por Archivo")
                End With
                Limpiar()
                setGemBoxLicense()
            End If

        Catch ex As Exception
            miEncabezado.showError("Error al tratar de cargar página. " & ex.Message & "<br><br>")
        End Try
    End Sub

    Protected Sub CargarInformacion(ByVal sender As Object, ByVal e As FileUploadCompleteEventArgs)
        Try
            Limpiar()
            SavePostedFile(e.UploadedFile)
            e.CallbackData = e.UploadedFile.FileName
            Dim sb As New System.Text.StringBuilder
            Dim hw As New System.Web.UI.HtmlTextWriter(New System.IO.StringWriter(sb))
            rpLogErrores.RenderControl(hw)
            CType(sender, ASPxUploadControl).JSProperties("cprpLogErrores") = sb.ToString()
        Catch ex As Exception
            miEncabezado.showError("Error al Cargar el archivo. " & ex.Message & "<br><br>")
        End Try
    End Sub

    Protected Sub GvErroresDataBinding(sender As Object, e As EventArgs) Handles gvErrores.DataBinding
        gvErrores.DataSource = Session("dtError")
    End Sub

    Protected Sub GvRadidadoscargadoDataBinding(sender As Object, e As EventArgs) Handles gvSerialcargado.DataBinding
        gvSerialcargado.DataSource = Session("dtError")
    End Sub

    Protected Sub BtnLimpiarClick(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        Limpiar()
    End Sub

    Public Shared Sub setGemBoxLicense()
        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
    End Sub

    Protected Sub BtProcesarClick(sender As Object, e As EventArgs) Handles btProcesar.Click
        Try
            rpLogErrores.Visible = False
            rpResultado.Visible = False
            Dim retorno As Boolean
            retorno = ExtraeDatosX(Session("FilePath"), Session("Extension"), "Yes")
            If (retorno = False) Then
                rpResultado.Visible = False
                rpLogErrores.Visible = True
            Else
                rpLogErrores.Visible = False
                rpResultado.Visible = True

            End If
            btProcesar.ClientEnabled = False
        Catch ex As Exception
            miEncabezado.showError("Error Al Procesar la informacion . " & ex.Message & "<br><br>")
        End Try
    End Sub

    Protected Sub btnExportarResul_Click(sender As Object, e As EventArgs) Handles btnExportarResul.Click
        gveExportadorResultado.WriteXlsxToResponse("ResultadoCargueSeriales")
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        gveExportador.WriteXlsxToResponse("ErroresCargueSeriales")
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function SavePostedFile(ByVal uploadedFile As UploadedFile) As String
        Dim retorno As String = ""
        Try
            If (Not uploadedFile.IsValid) Then Return String.Empty

            Session.Remove("dtError")
            Session.Remove("Extension")
            Session.Remove("FilePath")
            If ucCargueArchivoRadicados.HasFile Then
                Dim fileName As String = Path.Combine(MapPath(UploadDirectory), ucCargueArchivoRadicados.PostedFile.FileName)
                Session("Extension") = Path.GetExtension(ucCargueArchivoRadicados.PostedFile.FileName)
                Session("FilePath") = Server.MapPath(UploadDirectory) & ucCargueArchivoRadicados.FileName
                ucCargueArchivoRadicados.SaveAs(Server.MapPath(UploadDirectory) & ucCargueArchivoRadicados.FileName)
                btProcesar.ClientEnabled = True
            End If
        Catch ex As Exception
            miEncabezado.showError("Error al Cargar el archivo. " & ex.Message & "<br><br>")
        End Try

        Return retorno
    End Function

    Private Function ExtraeDatosX(ByVal filePath As String, ByVal extension As String, ByVal isHDR As String) As Boolean
        Try
            Dim oExcel As New ExcelFile
            Dim dt_informacionSeriales As New DataTable()
            Session.Remove("dtError")
            Dim retorno As Boolean
            Dim resultado As Integer = -1
            Dim idUsuario As Integer
            idUsuario = CInt(CInt(Session("userId")))
            Dim conStr As String = ""
            Select Case extension
                Case ".xls"
                    'Cargar el archivo de Excel.
                    oExcel.LoadXls(filePath)
                    Exit Select
                Case ".xlsx"
                    'Cargar el archivo de Excel.
                    oExcel.LoadXlsx(filePath, XlsxOptions.None)
                    Exit Select
            End Select
            ''Seleccione la primera hoja del expediente.
            'Dim ws As ExcelWorksheet = oExcel.Worksheets(0)

            'If oExcel.Worksheets.Count = 1 Then
            Dim oWs As ExcelWorksheet = oExcel.Worksheets.ActiveWorksheet
            Dim numColumnas As Integer
            numColumnas = oWs.CalculateMaxUsedColumns()

            If numColumnas = 4 Then
                dt_informacionSeriales = CrearEstructuraDeTabla()
                Dim filaInicial As Integer = oWs.Cells.FirstRowIndex
                Dim columnaInicial As Integer = oWs.Cells.FirstColumnIndex

                'Manage ExtractDataError.WrongType error
                AddHandler oWs.ExtractDataEvent, AddressOf ExtractDataErrorHandler

                oWs.ExtractToDataTable(dt_informacionSeriales, oWs.Rows.Count, ExtractDataOptions.SkipEmptyRows, oWs.Rows(filaInicial + 1), oWs.Columns(columnaInicial))


            ElseIf numColumnas > 4 Then
                RegError(0, "No se puede procesar el archivo, ya que contiene más columnas que las esperadas. Por favor verifique", "")
            Else
                RegError(0, "No se puede procesar el archivo, ya que contiene menos columnas que las esperadas. Por favor verifique", "")
            End If

            'Else
            'RegError(0, "No se puede procesar el archivo, ya que contiene más hojas que las esperdas. Por favor verifique", "")
            'End If

            If Session("dtError") Is Nothing Then ValidarInformacionArchivo(dt_informacionSeriales)

            If Not Session("dtError") Is Nothing Then
                rpLogErrores.Visible = True
                With gvErrores
                    .DataSource = CType(Session("dtError"), DataTable)
                    .DataBind()
                End With
                rpLogErrores.Visible = True
                btProcesar.ClientEnabled = True
                retorno = "False"
            Else
                Dim cargueSeriales As New CargueMasivodeSeriales()
                Session("dtError") = cargueSeriales.CargarSeriales(dt_informacionSeriales, idUsuario, resultado)
                If (resultado = 1) Then
                    lbInfogeneral.Text = dt_informacionSeriales.Rows.Count
                    With gvSerialcargado
                        .DataSource = CType(Session("dtError"), DataTable)
                        .DataBind()
                    End With
                    retorno = "True"
                    btProcesar.ClientEnabled = False
                    rpLogErrores.Visible = False
                    rpResultado.Visible = True
                Else
                    rpLogErrores.Visible = True
                    With gvErrores
                        .DataSource = CType(Session("dtError"), DataTable)
                        .DataBind()
                    End With
                    rpResultado.Visible = False
                    rpLogErrores.Visible = True
                    btProcesar.ClientEnabled = True

                    retorno = "False"
                End If
            End If

            Return retorno
        Catch ex As Exception
            miEncabezado.showError("Error al procesar el archivo. " & ex.Message & "<br><br>")
        End Try
    End Function

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        Return
    End Sub

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        With dtAux.Columns
            dtAux.Columns.Add("NOMBRE BODEGA-PDV", GetType(String))
            dtAux.Columns.Add("PRODUCTO PADRE", GetType(String))
            dtAux.Columns.Add("CUPO", GetType(String))
            dtAux.Columns.Add("CODIGO BARRAS", GetType(String))

        End With
        Return dtAux
    End Function

    Private Sub ValidarInformacionArchivo(ByVal informacionSeriales As DataTable)
        Try
            If (informacionSeriales.Columns.Count <> 4) Then
                RegError(0, "La hoja (Hoja1) debe tener 4 columnas por favor verificar el archivo", "")
            End If
            Dim fila As Integer = 1
            For Each row As DataRow In informacionSeriales.Rows
                If (IsDBNull(row("NOMBRE BODEGA-PDV"))) Then
                    RegError(fila, "El campo (NOMBRE BODEGA-PDV) tiene valores no válidos por favor verificar", row("CODIGO BARRAS").ToString())
                ElseIf (row("NOMBRE BODEGA-PDV").ToString().Length > 150) Then
                    RegError(fila, "El valor del campo (NOMBRE BODEGA-PDV) supera el máximo número de caracteres permitidos (150)", row("CODIGO BARRAS").ToString())
                End If

                If (IsDBNull(row("PRODUCTO PADRE"))) Then
                    RegError(fila, "El campo (PRODUCTO PADRE) tiene valores no válidos por favor verificar", row("CODIGO BARRAS").ToString())
                ElseIf (row("PRODUCTO PADRE").ToString().Length > 100) Then
                    RegError(fila, "El valor del campo (PRODUCTO PADRE) supera el máximo número de caracteres permitidos (100)", row("CODIGO BARRAS").ToString())
                End If

                If Not (IsNumeric(row("CUPO"))) Then
                    RegError(fila, "El valor del campo (CUPO) no es válido. Por favor verificar que sea un número entero sin formato", row("CODIGO BARRAS").ToString())
                End If

                If Not (IsNumeric(row("CODIGO BARRAS"))) Then
                    RegError(fila, "El valor del campo (CODIGO BARRAS) no es válido. Por favor verificar que sea un número entero sin formato", row("CODIGO BARRAS").ToString())
                ElseIf (row("CODIGO BARRAS").ToString().Length <> 14 And row("CODIGO BARRAS").ToString().Length <> 16 And row("CODIGO BARRAS").ToString().Length <> 22) Then
                    RegError(fila, "La longitud del campo (CODIGO BARRAS) no es válida. Las longitudes permitidas son: 14, 16 y 22 dígitos", row("CODIGO BARRAS").ToString())
                End If

                fila = fila + 1
            Next
        Catch ex As Exception
            miEncabezado.showError("Error al validar la Información del archivo. " & ex.Message & "<br><br>")
        End Try
    End Sub

    Private Sub RegError(ByVal linea As Integer, ByVal descripcion As String, Optional ByVal serial As String = "")
        Try
            Dim dtError As New DataTable
            If Session("dtError") Is Nothing Then
                dtError.Columns.Add(New DataColumn("lineaArchivo"))
                dtError.Columns.Add(New DataColumn("descripcion"))
                dtError.Columns.Add(New DataColumn("serial", GetType(String)))
                Session("dtError") = dtError
            Else
                dtError = Session("dtError")
            End If
            Dim dr As DataRow = dtError.NewRow()
            dr("lineaArchivo") = linea
            dr("serial") = serial
            dr("descripcion") = descripcion
            dtError.Rows.Add(dr)
            Session("dtError") = dtError
        Catch ex As Exception
            miEncabezado.showError("Error al registra errores . " & ex.Message & "<br><br>")
        End Try
    End Sub

    Private Sub Limpiar()
        Try
            Session.Remove("dtError")
            Session.Remove("Extension")
            Session.Remove("FilePath")
            lbInfogeneral.Text = ""
            With gvErrores
                .DataSource = CType(Session("dtError"), DataTable)
                .DataBind()
            End With
            With gvSerialcargado
                .DataSource = CType(Session("dtError"), DataTable)
                .DataBind()
            End With
            rpLogErrores.Visible = False
            rpResultado.Visible = False
        Catch ex As Exception
            miEncabezado.showError("Error Al limpiar los campos . " & ex.Message & "<br><br>")
        End Try
    End Sub

    Private Sub ExtractDataErrorHandler(ByVal sender As Object, ByVal e As ExtractDataDelegateEventArgs)
        If e.ErrorID = ExtractDataError.WrongType Then
            If e.ExcelValue Is Nothing Then
                e.DataTableValue = DBNull.Value
            Else
                e.DataTableValue = e.ExcelValue.ToString()
            End If
            e.Action = ExtractDataEventAction.Continue
        End If
    End Sub
#End Region

    Protected Sub BtnExportarProductoBodega_Click(sender As Object, e As EventArgs) Handles BtnExportarProductoBodega.Click
        Dim cargueSeriales As New CargueMasivodeSeriales()
        Dim dataSet = cargueSeriales.ProductosBodegas()
        Dim ef As New ExcelFile
        Dim ws As ExcelWorksheet
        For Each dataTable In dataSet.Tables
            ' Añadir nueva hoja de cálculo en el archivo.
            If (dataTable.TableName = "Table") Then
                ws = ef.Worksheets.Add("Maestro de Productos")
            Else
                ws = ef.Worksheets.Add("Maestro de Bodegas")
            End If
            ' Introduzca los datos de DataTable a la hoja a partir de la celda "A1".
            ws.InsertDataTable(dataTable, "A1", True)
        Next
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=ProductosBodegas.xls")
        ef.SaveXls(Response.OutputStream)
        Response.End()

    End Sub

    
End Class