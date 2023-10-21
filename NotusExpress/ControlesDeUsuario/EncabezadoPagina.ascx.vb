Imports NotificacionEventos
Imports System.IO

Partial Public Class EncabezadoPagina
    Inherits System.Web.UI.UserControl

    Public Sub clear()
        lblError.Text = ""
        lblSuccess.Text = ""
        lblWarning.Text = ""
        lblError.Visible = False
        lblSuccess.Visible = False
        lblWarning.Visible = False
    End Sub

    Public Sub showError(ByVal message As String)
        If Page.IsCallback Then
            Page.ClientScript.RegisterClientScriptInclude("SymbolError", "alert('" & message.Trim & "', 'rojo');")
            'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "SymbolError", "alert('" & message.Trim & "', 'rojo');", True)
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "error", "alert('" & message.Trim & "', 'rojo');", True)

        End If
      
        'lblError.Visible = True
        'lblError.CssClass = "error"
        'lblError.ForeColor = Drawing.Color.Red
        'lblError.Text = "<ul><li>&nbsp;" & message.Trim & "</li></ul>"
    End Sub

    Public Sub showWarning(ByVal message As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "advertencia", "alert('" & message.Trim & "', 'amarillo');", True)
        'lblWarning.Visible = True
        'lblWarning.CssClass = "warning"
        'lblWarning.Text = "<ul><li>&nbsp;" & message.Trim & "</li></ul>"
    End Sub

    Public Sub showSuccess(ByVal message As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "exito", "alert('" & message.Trim & "', 'verde');", True)
        'lblSuccess.Visible = True
        'lblSuccess.Text = "<ul><li>&nbsp;" & message.Trim & "</li></ul>"
    End Sub

    Public Sub showInfo(ByVal message As String)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "informativo", "alert('" & message.Trim & "', 'azul');", True)
    End Sub

    Public Sub setTitle(ByVal title As String)
        lblTitle.Visible = True
        lblTitle.Text = title.Trim
        ltDivision.Visible = True
    End Sub

    Public Function getTitle() As String
        Return lblTitle.Text.Trim
    End Function

    Public Sub showReturnLink(ByVal url As String)
        hlRegresar.NavigateUrl = Me.Page.ResolveUrl(url)
        pnlRegresar.Visible = True
    End Sub

    Public Function GetReturnUrl() As String
        Return hlRegresar.NavigateUrl
    End Function

    Public Sub hideReturnLink()
        pnlRegresar.Visible = False
    End Sub

    Public Function RenderHtml() As String
        Dim sb As New System.Text.StringBuilder
        Dim sw As New System.IO.StringWriter(sb)
        Dim hw As New System.Web.UI.HtmlTextWriter(sw)
        Me.RenderControl(hw)

        Return sb.ToString
    End Function

    Public Sub MostrarErrorYNotificarViaMail(mensaje As String, proceso As String, Optional ByVal excepcion As Exception = Nothing)
        Dim notificador As NotificacionEventos.NotificadorEvento
        Try
            With lblError
                .Visible = True
                .CssClass = "error"
                .Text = "<ul><li>&nbsp;" & mensaje.Trim & "</li></ul>"
            End With

            notificador = New NotificacionEventos.NotificadorEvento
            If Me.Page IsNot Nothing Then proceso += ". " & Path.GetFileName(Me.Page.Request.Path)
            If excepcion IsNot Nothing Then mensaje += excepcion.Message & ". " & excepcion.StackTrace

            notificador.NotificarViaMail(proceso, mensaje, NotificacionEventos.NotificadorEvento.TipoEvento.Error, True)
        Catch ex As Exception
        Finally
            notificador = Nothing
        End Try
    End Sub

    Sub MostrarErrorYNotificarViaMail(p1 As String, p2 As String, p3 As String)
        Throw New NotImplementedException
    End Sub

End Class