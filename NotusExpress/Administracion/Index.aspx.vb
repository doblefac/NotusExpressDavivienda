Imports System.Web.Security
Imports EncryptionClassLibrary.LMEncryption
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.ControlAcceso

Partial Public Class index
    Inherits System.Web.UI.Page

    Private dvMenu As DataView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblError.Text = ""
        txtAntigua.Attributes.Add("onkeydown", "ProcesarEnterGeneral(this,'" + btnCambiar.ClientID + "')")
        txtNueva.Attributes.Add("onkeydown", "ProcesarEnterGeneral(this,'" + btnCambiar.ClientID + "')")
        txtNuevaConfirm.Attributes.Add("onkeydown", "ProcesarEnterGeneral(this,'" + btnCambiar.ClientID + "')")
        If Not IsPostBack Then
            Dim idUsr As String = Session("userId")
            Dim infoUsuario As UsuarioSistema
            Try
                infoUsuario = New UsuarioSistema(idUsr)
                If infoUsuario.Registrado Then
                    With infoUsuario
                        lblUsuario.Text = .NombreApellido
                        lblCargo.Text = .Cargo
                        Session("idUnidadNegocio") = .IdUnidadNegocio
                        Session("idRol") = .IdRol
                        Session("idPersona") = .IdPersona
                        Session("idPerfil") = .IdPerfil
                    End With

                    Dim infoPerfil As New PerfilColeccion(infoUsuario.IdUsuario)
                    If infoPerfil.Count > 0 Then
                        Dim arrPerfil As New ArrayList
                        Dim arrIdPerfil As New ArrayList
                        For Each perAux As Perfil In infoPerfil
                            arrPerfil.Add(perAux.Nombre)
                            arrIdPerfil.Add(perAux.IdPerfil)
                        Next
                        Session("arrIdPerfiles") = arrIdPerfil
                        lblPerfil.Text = Join(arrPerfil.ToArray, ", ")
                        Dim miColeccionMenu As New MenuColeccion(idUsr)
                        Try
                            dvMenu = miColeccionMenu.GenerarDataTable.DefaultView
                            dvMenu.Sort = "nivel, posicionOrdinal"
                            If dvMenu.Count > 0 Then
                                LlenarMenu(dvMenu(0), Nothing)
                                If treevMenu.Nodes.Count > 0 Then treevMenu.Nodes(0).Expand()
                                'Dim urlInicial As String = infoUsuario.ObtenerPaginaPorDefecto
                                'If urlInicial.Trim.Length > 0 Then
                                '    mainFrame.Attributes.Add("src", Me.Page.ResolveUrl(urlInicial))
                                '    With treevMenu
                                '        If .Nodes(0) IsNot Nothing Then
                                '            .Nodes(0).SelectAction = TreeNodeSelectAction.Select
                                '            .Nodes(0).NavigateUrl = Me.Page.ResolveUrl(urlInicial)
                                '        End If
                                '    End With
                                'End If
                            Else
                                Response.Redirect("~/Login.aspx?err=true&codErr=1")
                            End If
                        Finally
                            dvMenu.Dispose()
                            miColeccionMenu = Nothing
                        End Try
                    Else
                        Response.Redirect("~/Login.aspx?err=true&codErr=3")
                    End If
                Else
                    Response.Redirect("~/Login.aspx?err=true&codErr=2")
                End If
            Catch tAbEx As Threading.ThreadAbortException
            Catch ex As Exception
                Response.Redirect("~/Login.aspx?err=true")
            End Try
        End If
    End Sub

    Private Sub LlenarMenu(ByVal elMenu As DataRowView, ByRef nodoActual As TreeNode)
        Dim nodo As New TreeNode

        With nodo
            .Text = elMenu("Nombre")
            .ToolTip = elMenu("Nombre")
            .Value = elMenu("IdMenu")
            If elMenu("UrlFormulario").ToString.Trim.Length > 0 Then
                .Target = ""
                .NavigateUrl = "javascript: CargarPagina('" & Page.ResolveClientUrl(elMenu("UrlFormulario")) & "');"
                .ImageUrl = "~/img/application_form.png"
            Else
                .SelectAction = TreeNodeSelectAction.Expand
            End If
        End With
        If nodoActual Is Nothing Then
            nodoActual = nodo
            treevMenu.Nodes.Add(nodoActual)
        Else
            nodoActual.ChildNodes.Add(nodo)
            nodoActual.CollapseAll()
        End If
        dvMenu.RowFilter = "IdPadre=" & elMenu("idMenu") & " and idEstado=1"
        If dvMenu.Count > 0 Then
            For Each drvMenu As DataRowView In dvMenu
                LlenarMenu(drvMenu, nodo)
            Next
        End If
    End Sub

    Protected Sub btnCambiar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCambiar.Click
        Dim idUsuario As Integer
        Dim pwdActual As String = ""
        Dim pwdNuevo As String = ""

        Try
            If txtNueva.Text.Equals(txtNuevaConfirm.Text) Then
                Dim resultado As General.ResultadoProceso
                pwdActual = EncryptionData.getMD5Hash(txtAntigua.Text)
                pwdNuevo = EncryptionData.getMD5Hash(txtNueva.Text)
                Integer.TryParse(Session("userId"), idUsuario)
                resultado = UsuarioSistema.CambiarPassword(idUsuario, pwdActual, pwdNuevo)
                Select Case resultado.Valor
                    Case 0
                        With lblNotificacionOK
                            .Visible = True
                            .Text = "La contraseña fue actualizada correctamente."
                        End With
                    Case Else
                        lblNotificacionOK.Visible = False
                        lblErrores.Text = "Imposible cambiar contraseña. " & resultado.Mensaje
                        dlgCambiarContrasena.Show()
                End Select
            Else
                lblErrores.Text = "Las contraseñas escritas no coinciden. "
                dlgCambiarContrasena.Show()
            End If
        Catch ex As Exception
            lblErrores.Text = "Error al tratar de cambiar clave. Por favor contactar a IT"
            Dim notificador As New NotificacionEventos.NotificadorEvento
            notificador.NotificarViaMail("Cambio de Contraseña Notus Express Web", "Error al tratar de cambiar clave. ", NotificacionEventos.NotificadorEvento.TipoEvento.Error, False)
            lblNotificacionOK.Visible = False
            dlgCambiarContrasena.Show()
        End Try
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        txtAntigua.Text = ""
        txtNueva.Text = ""
        txtNuevaConfirm.Text = ""
        lblNotificacionOK.Visible = False
    End Sub

    Protected Sub LoginStatus1_LoggingOut(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.LoginCancelEventArgs) Handles LoginStatus1.LoggingOut
        Session.RemoveAll()
    End Sub

    Protected Sub lbContrasena_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbContrasena.Click
        lblErrores.Text = ""
        dlgCambiarContrasena.Show()
    End Sub
End Class