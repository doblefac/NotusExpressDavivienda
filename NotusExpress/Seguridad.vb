Imports System.Web.HttpContext
Imports EncryptionClassLibrary
Imports System.IO
Imports EncryptionClassLibrary.LMEncryption

Public Class Seguridad

    Public Sub New()
    End Sub

    Public Shared Sub verificarSession(ByVal pagina As System.Web.UI.Page)
        Try
            If pagina.Session("userId") Is Nothing Then
                If String.IsNullOrEmpty(pagina.Session("userId")) Then
                    Dim myScriptManager As ScriptManager = ScriptManager.GetCurrent(pagina)
                    If myScriptManager Is Nothing OrElse (Not myScriptManager.IsInAsyncPostBack) Then
                        With pagina.Response
                            .Write("<SCRIPT LANGUAGE='JavaScript'>")
                            .Write("alert('ERROR: Su Sesion ha vencido por superar el tiempo de inactividad en el Sistema. Por favor ingrese de nuevo.');")
                            .Write("window.top.location.href= '../login.aspx';")
                            .Write("</SCRIPT>")
                            .Redirect(pagina.ResolveUrl("~/login.aspx?errSess=true"))
                        End With
                    Else
                        Dim script As String = "alert('ERROR: Su Sesion ha vencido por superar el tiempo de inactividad en el Sistema. " & _
                            " Por favor ingrese de nuevo.');" & vbCrLf & "window.top.location.href= '../login.aspx';"
                        ScriptManager.RegisterStartupScript(pagina, pagina.GetType, "validacionSeguridad", script, True)
                    End If
                End If
            End If
            'Catch tAbEx As System.Threading.ThreadAbortException
        Catch ex As Exception
            Throw New Exception("Error al tratar de validar Sesion. " & ex.Message)
        End Try
    End Sub

End Class
