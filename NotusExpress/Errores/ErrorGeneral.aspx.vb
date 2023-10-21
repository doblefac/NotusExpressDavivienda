Public Class ErrorGeneral
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ex As Exception = Server.GetLastError()
        Dim errorInfo As String
        If Request.UrlReferrer IsNot Nothing Then
            errorInfo = "URL: " + Request.UrlReferrer.ToString()
        Else
            errorInfo = "URL: " + Request.Url.ToString()
        End If

        If ex IsNot Nothing Then errorInfo += "<br />Recurso: " + ex.Source & "<br />Mensaje: " + ex.Message

        lblDescripcion.Text = errorInfo
        Server.ClearError()
    End Sub

End Class