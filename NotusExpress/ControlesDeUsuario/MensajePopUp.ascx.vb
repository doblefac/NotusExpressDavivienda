Imports System.IO

Public Class MensajePopUp
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property Titulo As String
        Get
            Return pucMensaje.HeaderText
        End Get
        Set(value As String)
            pucMensaje.HeaderText = value
        End Set
    End Property

    Public ReadOnly Property Mensaje As String
        Get
            Return lblMensaje.Text.Trim
        End Get
    End Property

    Public Sub MostrarMensajePopUp(mensaje As String, Optional ByVal tipo As TipoMensaje = TipoMensaje.ProcesoExitoso,
                                   Optional ByVal titulo As String = "")
        lblMensaje.EncodeHtml = False
        Select Case tipo
            Case TipoMensaje.ErrorCritico
                lblMensaje.CssClass = "mensajeError"
                If String.IsNullOrEmpty(titulo) Then pucMensaje.HeaderText = "Error"
            Case TipoMensaje.Alerta
                lblMensaje.CssClass = "mensajeWarning"
                If String.IsNullOrEmpty(titulo) Then pucMensaje.HeaderText = "Alerta"
            Case Else
                lblMensaje.CssClass = "mensajeOk"
                If String.IsNullOrEmpty(titulo) Then pucMensaje.HeaderText = "Proceso Exitoso"
        End Select
        lblMensaje.Text = "<ul><li>" & mensaje.Trim & "</li></ul>"
        If Not String.IsNullOrEmpty(titulo) Then pucMensaje.HeaderText = titulo
        pucMensaje.ShowOnPageLoad = True
    End Sub

    Public Sub MostrarErrorYNotificarViaMail(mensaje As String, proceso As String, Optional ByVal excepcion As Exception = Nothing)
        Dim notificador As NotificacionEventos.NotificadorEvento
        Try
            With lblMensaje
                .EncodeHtml = False
                .CssClass = "mensajeError"
                .Text = "<ul><li>" & mensaje.Trim & "</li></ul>"
            End With
            pucMensaje.HeaderText = "Error Crítico"
            pucMensaje.ShowOnPageLoad = True

            notificador = New NotificacionEventos.NotificadorEvento
            If Me.Page IsNot Nothing Then proceso += ". " & Path.GetFileName(Me.Page.Request.Path)
            If excepcion IsNot Nothing Then mensaje += excepcion.Message & ". " & excepcion.StackTrace

            notificador.NotificarViaMail(proceso, mensaje, NotificacionEventos.NotificadorEvento.TipoEvento.Error, True)
        Catch ex As Exception
        Finally
            notificador = Nothing
        End Try
    End Sub

    Public Sub Limpiar()
        lblMensaje.Text = ""
    End Sub

    Public Function RenderHtmlDeMensaje() As String
        Dim sb As New System.Text.StringBuilder
        Dim sw As New System.IO.StringWriter(sb)
        Dim hw As New System.Web.UI.HtmlTextWriter(sw)
        lblMensaje.RenderControl(hw)

        Return sb.ToString
    End Function

    Enum TipoMensaje
        ProcesoExitoso = 1
        Alerta = 2
        ErrorCritico = 3
    End Enum

End Class