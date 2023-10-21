Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.RecursoHumano
Imports DevExpress.Web
Imports GemBox
Imports GemBox.Spreadsheet
Imports System.IO

Public Class CambioMasivoAsesor
    Inherits System.Web.UI.Page

#Region "Atributos"
    Private objAsesores As New AsesorComercial
#End Region

#Region "Eventos"

    Private Sub CambioMasivoAsesor_Init(sender As Object, e As EventArgs) Handles Me.Init
        Herramientas.CargarLicenciaGembox()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Actualización masiva de asesores")
        End If
    End Sub

    Private Sub lbVerArchivo_Click(sender As Object, e As System.EventArgs) Handles lbVerArchivo.Click
        Dim ruta As String
        Try
            ruta = Server.MapPath("~/GestionComercial/Archivos/EjemploActualizacionMasivaAsesor.xlsx")
            If ruta <> "" Then Herramientas.ForzarDescargaDeArchivo(HttpContext.Current, ruta)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de abrir archivo de ejemplo. " & ex.Message)
        End Try
    End Sub

    Private Sub ucArchivo_FileUploadComplete(sender As Object, e As DevExpress.Web.FileUploadCompleteEventArgs) Handles ucArchivo.FileUploadComplete
        Try
            Dim rutaArchivo As String = ""
            rutaArchivo = Server.MapPath("~/GestionComercial/Archivos/" & ucArchivo.UploadedFiles(0).FileName.Split(".").GetValue(0) & " - " & Session("usxp001") & "." & ucArchivo.UploadedFiles(0).FileName.Split(".").GetValue(1))
            CargarArchivo()
            If objAsesores.TablaErrores.Rows.Count > 0 Then
                CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 1
            Else
                CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 0
            End If
            e.CallbackData = "Archivo Cargado: " & e.UploadedFile.FileName
            File.Delete(rutaArchivo)
            Session("dtErrores") = objAsesores.TablaErrores
            Session("dtDatos") = objAsesores.TablaDatos
            If objAsesores.TablaErrores.Rows.Count = 0 OrElse objAsesores.TablaErrores Is Nothing Then
                If objAsesores.TablaDatos.Rows.Count > 0 Then
                    RegistrarDatos()
                End If
            End If
            epNotificador.setTitle("Actualización masiva de asesores")
        Catch ex As Exception
            epNotificador.showError("Imposible subir el archivo al Servidor: " & ex.Message)
        End Try
        CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub gvLog_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvLog.CustomCallback
        Try
            With gvLog
                .DataSource = Session("dtErrores")
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Se generó un error al intentar visualizar el log: " & ex.Message)
        End Try
    End Sub

    Private Sub gvLog_DataBinding(sender As Object, e As System.EventArgs) Handles gvLog.DataBinding
        If Session("dtErrores") Is Nothing Then
        Else
            gvLog.DataSource = Session("dtErrores")
        End If
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarArchivo()
        Dim oExcel As New ExcelFile
        Dim nombreArchivoProceso As String = ""
        Dim NombreValido As Boolean = False
        Dim nombreArchivo As String
        Try
            If ucArchivo.FileInputCount > 0 Then
                nombreArchivo = Path.GetFileNameWithoutExtension(ucArchivo.UploadedFiles(0).FileName) & Path.GetExtension(ucArchivo.UploadedFiles(0).FileName)
                Session("nombreArchivo") = nombreArchivo
                Dim ExpReg As New System.Text.RegularExpressions.Regex("^[a-zA-Z0-9-_ ]+$")
                NombreValido = (ExpReg.IsMatch(nombreArchivo.Split(".").GetValue(0)))
                If NombreValido Then
                    nombreArchivoProceso = Server.MapPath("~/GestionComercial/Archivos/" & ucArchivo.UploadedFiles(0).FileName.Split(".").GetValue(0) & " - " & Session("usxp001") & "." & ucArchivo.UploadedFiles(0).FileName.Split(".").GetValue(1))
                    System.IO.File.Delete(nombreArchivoProceso)
                    ucArchivo.UploadedFiles(0).SaveAs(nombreArchivoProceso)
                    Dim fileExtension As String = Path.GetExtension(ucArchivo.UploadedFiles(0).FileName)
                    oExcel = New ExcelFile()
                    If fileExtension.ToUpper = ".XLS" Then
                        oExcel.LoadXls(New MemoryStream(ucArchivo.UploadedFiles(0).FileBytes))
                    ElseIf fileExtension.ToUpper = ".XLSX" Then
                        oExcel.LoadXlsx(New MemoryStream(ucArchivo.UploadedFiles(0).FileBytes), XlsxOptions.None)
                    End If
                    objAsesores = New AsesorComercial(oExcel)
                    With objAsesores
                        .IdUsuarioSistema = Session("userId")
                        If .ValidarArchivo() Then
                            .ValidarInformacion(objAsesores.TablaDatos)
                        End If
                    End With
                Else
                    epNotificador.showError("Nombre de archivo no valido para procesar")
                    Exit Sub
                End If
            Else
                epNotificador.showError("Seleccione el archivo a procesar")
            End If
        Catch ex As Exception
            epNotificador.showError("Se generó un error procesando el archivo: " + ex.Message)
        Finally
            If File.Exists(nombreArchivoProceso) Then File.Delete(nombreArchivoProceso)
        End Try
    End Sub

    Private Sub RegistrarDatos()
        Dim resultRegistro As resultadoproceso
        Try
            objAsesores = New AsesorComercial
            With objAsesores
                .TablaDatos = Session("dtDatos")
                .IdModificador = Session("userId")
                resultRegistro = .ActualizarAsesorClienteArchivo
                If resultRegistro.Valor = 0 Then
                    epNotificador.showSuccess("Información actualizada exitosamente.")
                Else
                    epNotificador.showError("Error al Actualizar la información: " & resultRegistro.Mensaje)
                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Imposible subir el archivo al Servidor: " & ex.Message)
        End Try
    End Sub

#End Region

End Class