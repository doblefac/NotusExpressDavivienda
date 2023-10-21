Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        DevExpress.XtraPrinting.Native.BrickFactory.RegisterFactory(DevExpress.XtraPrinting.Native.BrickTypes.RichText, New DevExpress.XtraPrinting.Native.DefaultBrickFactory(Of DevExpress.XtraPrinting.RichTextBrick))
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        HttpContext.Current.Response.AddHeader("X-Frame-Options", "SAMEORIGIN")
        HttpContext.Current.Response.AddHeader("X-Content-Type-Options", "NOSNIFF")
        DevExpress.Web.ASPxWebControl.SetIECompatibilityMode(9)
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class