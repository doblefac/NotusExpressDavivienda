Imports System.IO
Imports GemBox.Spreadsheet
Imports NotusExpressBusinessLayer
Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.Comunes
Imports System.Net

Public Class AdministracionClientes
    Inherits System.Web.UI.Page

#Region "Atributos."

    Private oExcel As ExcelFile
    Private objCliente As CargueClientes
    Private _mensajeGenerico As String = String.Empty

#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Administración Clientes")
                End With
                CargarInicial()
                cmbCampania.SelectedIndex = -1
                cmbCampania.Focus()
            Else
                If Session("objCliente") IsNot Nothing Then objCliente = Session("objCliente")
            End If
            CargarLicenciaGembox()
        Catch ex As Exception
            epNotificador.showError("Se presento un error al cargar la página: " & ex.Message)
        End Try
    End Sub

    Private Sub lbVerArchivo_Click(sender As Object, e As System.EventArgs) Handles lbVerArchivo.Click
        Dim ruta As String
        Try
            ruta = Server.MapPath("~/GestionComercial/Archivos/EjemploMaestroCliente.xlsx")
            If ruta <> "" Then Herramientas.ForzarDescargaDeArchivo(HttpContext.Current, ruta)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de abrir archivo de ejemplo. " & ex.Message)
        End Try
    End Sub

    Private Sub ProcesarArchivo()
        Try
            Session("idCampania") = cmbCampania.Value
            Dim fec As String = DateTime.Now.ToString("HH:mm:ss:fff").Replace(":", "_")
            Dim ruta As String
            Dim resultado As New ResultadoProceso
            Dim idCampania As Integer = CInt(Session("idCampania"))
            Session("dtErrores") = Nothing
            Dim numeroColumnasErrores As Integer
            'Dim dtEstrategia As DataTable
            Dim codEstrategia As String = String.Empty

            If ValidarCargue() Then
                Dim nombreArchivo As String = "CargueMaestroCliente_" & fec & "-" & Session("usxp001") & Path.GetExtension(fuArchivo.PostedFile.FileName)
                If Not (fuArchivo.PostedFile.FileName Is Nothing) Then
                    Dim fileExtension As String = Path.GetExtension(fuArchivo.PostedFile.FileName)
                    ruta = Server.MapPath("~/ArchivosCargados/" & nombreArchivo)
                    Try
                        fuArchivo.SaveAs(ruta)
                    Catch ex As Exception
                        epNotificador.showError("Se genero un error al guardar el archivo: " & ex.Message)
                        Exit Sub
                    End Try
                    oExcel = New ExcelFile
                    If fileExtension.ToUpper = ".XLS" Then
                        oExcel.LoadXls(ruta)
                    ElseIf fileExtension.ToUpper = ".XLSX" Then
                        oExcel.LoadXlsx(ruta, XlsxOptions.None)
                    End If

                    Try
                        objCliente = New CargueClientes(oExcel)

                        ' dtEstrategia = objCliente.ConsultaCodEstrategia(idCampania)
                        'If (dtEstrategia.Rows.Count > 0) Then
                        '    codEstrategia = dtEstrategia.Rows(0)(0).ToString()
                        'End If
                        If objCliente.ValidarEstructura(codEstrategia) Then
                            callback.JSProperties("cpResultadoProceso") = 0
                            If objCliente.ValidarInformacion(idCampania) Then
                                Dim identificador As Guid = Guid.NewGuid()
                                'Dim uploader As UploadedFile = fuArchivo.
                                Dim rutaAlmacenamiento As String = "\GestionComercial\ClientesCargados\"
                                resultado = objCliente.CrearClientes(CInt(idCampania), fuArchivo.FileName, fuArchivo.FileBytes.Length, rutaAlmacenamiento)
                                If resultado.Valor = 0 Then
                                    'Guardar Archivo
                                    fuArchivo.SaveAs(Server.MapPath("~") & rutaAlmacenamiento & identificador.ToString())
                                End If
                            Else
                                resultado.EstablecerMensajeYValor(20, "Datos Inválidos en el archivo proporcionado")
                                MostrarErrores(objCliente.EstructuraTablaErrores)
                                callback.JSProperties("cpResultadoProceso") = 1
                            End If
                        Else
                            MostrarErrores(objCliente.EstructuraTablaErrores)
                            numeroColumnasErrores = objCliente.EstructuraTablaErrores.Rows.Count
                            If (numeroColumnasErrores > 0) Then
                                resultado.EstablecerMensajeYValor(10, "Se encontraron errores en la validación. " & objCliente.EstructuraTablaErrores.Rows(numeroColumnasErrores - 1)(2).ToString())
                            Else
                                resultado.EstablecerMensajeYValor(10, "Se encontraron errores en la validación.")
                            End If
                            callback.JSProperties("cpResultadoProceso") = 1
                        End If
                        Session("objCliente") = objCliente
                    Catch ex As Exception
                        Throw ex
                    End Try
                    'epNotificador.clear()
                    With epNotificador
                        .setTitle("Administración Clientes")
                    End With
                    If resultado.Valor = 0 Then
                        epNotificador.showSuccess("Se realizó la carga de clientes, satisfactoriamente.")
                    ElseIf resultado.Valor = 10 Or resultado.Valor = 20 Or resultado.Valor = 30 Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "MyScript", "  setTimeout ('dialogoErrores.ShowWindow();', 100);", True)
                        epNotificador.showError(resultado.Mensaje)
                    ElseIf resultado.Valor = 40 Then
                        epNotificador.showError(resultado.Mensaje)
                    Else
                        epNotificador.showError("Se presentaron errores en la carga del archivo, verifique el log de errores.")
                    End If
                    callback.JSProperties("cpResultadoProceso") = resultado.Valor
                Else
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "MyScript", "setTimeout ('LoadingPanel.Hide();', 100);", True)
                    epNotificador.showError("Seleccione los valores requeridos")
                End If

            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "MyScript", "setTimeout ('LoadingPanel.Hide();', 100);", True)
                epNotificador.showError(_mensajeGenerico)
            End If

        Catch ex As Exception
            epNotificador.showError("Se generó un error al intentar procesar el archivo: " & ex.Message)
        End Try
        callback.JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Function ValidarCargue() As Boolean
        If (cmbCampania.Value) Is Nothing Then
            _mensajeGenerico = "Seleccione una campaña para procesar el archivo."
            Return False
        ElseIf (fuArchivo.PostedFile.ContentLength = 0) Then
            _mensajeGenerico = "Seleccione archivo a procesar."
            Return False
        ElseIf (Path.GetExtension(fuArchivo.PostedFile.FileName).ToUpper() <> ".XLS") AndAlso (Path.GetExtension(fuArchivo.PostedFile.FileName).ToUpper() <> ".XLSX") Then
            _mensajeGenerico = "Seleccione un archivo de Excel válido."
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub gvErrores_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvErrores.CustomCallback
        Try
            If objCliente Is Nothing Then
                objCliente = Session("objCliente")
            End If
            With gvErrores
                .DataSource = objCliente.EstructuraTablaErrores()
                Session("dtErrores") = .DataSource
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Se generó un error al intentar visualizar el log: " & ex.Message)
            CType(sender, ASPxGridView).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End Try
    End Sub

    Private Sub gvErrores_DataBinding(sender As Object, e As System.EventArgs) Handles gvErrores.DataBinding
        If Session("dtErrores") IsNot Nothing Then gvErrores.DataSource = Session("dtErrores")
    End Sub

    Private Sub cbFormatoExportar_ButtonClick(source As Object, e As DevExpress.Web.ButtonEditClickEventArgs) Handles cbFormatoExportar.ButtonClick
        Try
            Dim formato As String = cbFormatoExportar.Value
            If Not String.IsNullOrEmpty(formato) Then
                With gveErrores
                    .FileName = "ReporteNovedades"
                    .ReportHeader = "Reporte Novedades" & vbCrLf & vbCrLf
                    .ReportFooter = vbCrLf & "Logytech Mobile S.A.S"
                    .Landscape = False
                    With .Styles.Default.Font
                        .Name = "Arial"
                        .Size = FontUnit.Point(10)
                    End With
                    .DataBind()
                End With
                gvErrores.SettingsDetail.ExportMode = GridViewDetailExportMode.Expanded
                Select Case formato
                    Case "xls"
                        gveErrores.WriteXlsToResponse()
                    Case "pdf"
                        With gveErrores
                            .Landscape = True
                            .WritePdfToResponse()
                        End With
                    Case "xlsx"
                        gveErrores.WriteXlsxToResponse()
                    Case "csv"
                        gveErrores.WriteCsvToResponse()
                End Select
            Else
                epNotificador.showWarning("No se pudo recuperar la información del reporte, por favor intente nuevamente.")
            End If
        Catch ex As Exception
            epNotificador.showError("Se presento un error al exportar los datos: " & ex.Message)
        End Try
    End Sub

    'Protected Sub btnXlsxExport_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    gvErrores.DataSource = Session("dtErrores")
    '    gveErrores.WriteXlsxToResponse()
    'End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarInicial()
        '** Cargar Campañas desde NotusOP mediante un WebService **
        Dim dtCampania As New DataTable
        dtCampania = ObtenerCampanias()
        CargarComboDX(cmbCampania, dtCampania, "idCampania", "campania")

        If Session("idCampania") IsNot Nothing Then
            cmbCampania.Value = CInt(Session("idCampania"))
        End If

    End Sub

    Private Function ObtenerCampanias() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsFiltroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Dim resultado As New ResultadoProceso

        With WSInfoFiltros
            .IdEmpresa = CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue)
            Wsresultado = objCampania.ConsultarCampaniasCEM(WSInfoFiltros, dsDatos)
        End With
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Private Sub MostrarErrores(ByVal dtErrores As DataTable)
        gvErrores.DataSource = dtErrores
        Session("dtErrores") = dtErrores
        gvErrores.DataBind()
    End Sub

#End Region

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        ProcesarArchivo()
        CargarInicial()
        fuArchivo.Focus()
    End Sub
End Class