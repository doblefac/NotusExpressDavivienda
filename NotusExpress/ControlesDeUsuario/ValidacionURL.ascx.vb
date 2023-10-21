Public Partial Class ValidacionURL
    Inherits System.Web.UI.UserControl
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
#If Not Debug Then
        If Session("userId") Is Nothing Then Response.Redirect("~/Login.aspx?err=true&codErr=5", False)
        If Request.UrlReferrer Is Nothing Then Response.Redirect("~/Administracion/index.aspx", False)
        If Request.UrlReferrer IsNot Nothing AndAlso Request.UrlReferrer.ToString.ToLower.Contains("login.aspx") AndAlso _
            Not Request.Url.AbsolutePath.ToLower.Contains("index.aspx") Then Response.Redirect("~/Login.aspx?err=true&codErr=4", False)
#End If
    End Sub

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        Dim strScript As String = "document.getElementById('" & hfControlOrigen.ClientID & "').value = top != self;"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "OnLoadScript_" + Me.ClientID, strScript, True)
    End Sub
End Class