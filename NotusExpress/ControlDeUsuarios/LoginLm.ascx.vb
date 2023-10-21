Imports LoginLmBusinessLayer
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Configuration
Imports LoginLm.Datos
Imports System.Net
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports NotusExpressBusinessLayer
Imports EncryptionClassLibrary


Public Class LoginLm
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load, CambioContrasena.DataBinding
        Select Case Session("recuperacionContrasena")
            Case 1
                lblCambioConfirmacion.Text = "Recuperacion Contraseña: El correo para la recuperación de contraseña ha sido enviado a la direccion de correo registrada"
            Case 2
                lblCambioConfirmacion.ForeColor = Color.Red
                lblCambioConfirmacion.Text = "Recuperacion Contraseña: El número de identificación que se encuentra ingresando no está registrada en el sistema"
            Case 3
                lblCambioConfirmacion.ForeColor = Color.Red
                lblCambioConfirmacion.Text = "Recuperacion Contraseña: Se espera un valor numerico con cédula"
            Case 4
                lblCambioConfirmacion.ForeColor = Color.Green
                lblCambioConfirmacion.Text = "Cambio de contraseña: Cambio de contraseña exitoso"
            Case Else
                Session("recuperacionContrasena") = 5
        End Select
    End Sub

    Protected Sub lgAutenticador_Authenticate(sender As Object, e As AuthenticateEventArgs) Handles lgAutenticador.Authenticate
        Dim infoAutenticacion As New ValidadorAccesoLoginLm
        Dim encriptarContrasna As New EncryptionLibrary
        Dim numeroIngresos As Integer
        Dim NotificacionBloqueo As New NotificacionBloqueoUsuario
        Dim identificacion As String = 0
        Try
            identificacion = infoAutenticacion.EsUsuarioValido(encriptarContrasna.CrearHash(encriptarContrasna.Encriptar(lgAutenticador.Password)), lgAutenticador.UserName)
            Session("idUsuario") = infoAutenticacion.IdUsuario
            If identificacion = 1 Then
                PopCambioDeContrasenaIngresoPrimeraVez.ShowOnPageLoad = True
            Else
                numeroIngresos = infoAutenticacion.ValidarcantidadIngresos(lgAutenticador.UserName)
                lblCambioConfirmacion.Visible = False
                Select Case numeroIngresos
                    Case 1
                        lgAutenticador.FailureText = "El usuario y la contraseña no coinciden, por favor intentar nuevamente"
                    Case 2
                        pcCaptcha.ShowOnPageLoad = True
                    Case 3
                        lgAutenticador.FailureText = "Si vuelve a tener un ingreso fallido su usuario será bloqueado"
                    Case 4
                        If infoAutenticacion.BolqueoDeUsuarioPorIntentosFallidos(lgAutenticador.UserName) Then
                            NotificacionBloqueo.NotificacionUsuarioBloqueoPorIntentosFallidos(lgAutenticador.UserName)
                            lgAutenticador.FailureText = "Se ha detectado un movimiento extraño con su usuario, por lo que debe realizar la recuperación de la contraseña"
                        End If
                    Case 5
                        'En esta parte implemente lo que se encontraba antes en el código
                        Dim autentucarBancolombia As New NotusExpressBusinessLayer.ControlAcceso.ValidadorAcceso
                        With autentucarBancolombia
                            .Usuario = lgAutenticador.UserName.Trim.ToLower
                            .Password = lgAutenticador.Password
                            e.Authenticated = .EsUsuarioValido(identificacion)
                        End With

                        If e.Authenticated Then
                            Session("userId") = autentucarBancolombia.IdUsuario
                            Session("idPerfil") = autentucarBancolombia.IdPerfil
                            Session("idCiudad") = autentucarBancolombia.IdCiudad
                            lgAutenticador.DestinationPageUrl = "~/Administracion/index.aspx"
                        End If
                        'Se finaliza la parte de acceso con Bancolombia
                    Case Else
                        lgAutenticador.FailureText = "El usuario y la contraseña no coinciden, por favor intentar nuevamente"
                End Select
            End If

            Session("Autenticacion") = e.Authenticated
            Session("idUsuario") = infoAutenticacion.IdUsuario
        Catch ex As Exception
            lgAutenticador.FailureText = "Imposible autenticar usuario : " & ex.Message
            e.Authenticated = False
        End Try
    End Sub

    Protected Sub OnChangingPassword(sender As Object, e As LoginCancelEventArgs)
        Dim validarContrasena As New ValidacionContrasena
        Dim modificarContrasena As New CambioContrasena
        Dim resultadoValidacion As String
        Dim InfoCambioPassword As New ValidadorAccesoLoginLm
        Dim encriptarContrasena As New EncryptionLibrary
        Dim regex As Regex = New Regex("\d+")
        Dim contrasenaEncriptadaNueva As String = encriptarContrasena.CrearHash(encriptarContrasena.Encriptar(CambioContrasena.NewPassword.Trim))
        Dim contrasenaEncriptadaVieja As String = encriptarContrasena.CrearHash(encriptarContrasena.Encriptar(CambioContrasena.CurrentPassword.Trim))
        Dim identificacion As String
        With InfoCambioPassword
            .PasswordAntiguo = CambioContrasena.CurrentPassword.Trim
            .PasswordNuevo = CambioContrasena.NewPassword.Trim
            .ConfirmarPasswordNuevo = CambioContrasena.ConfirmNewPassword.Trim
        End With
        If CambioContrasena.NewPassword.Trim = CambioContrasena.ConfirmNewPassword.Trim Then
            If modificarContrasena.ValidarUsuarioCambiar(Session("idUsuario"), contrasenaEncriptadaVieja) Then
                resultadoValidacion = validarContrasena.validacionContrasena(CambioContrasena.NewPassword.Trim.ToString(), Session("idUsuario"))
                If resultadoValidacion <> "" Then
                    lblMessage.ForeColor = Color.Red
                    lblMessage.Text = resultadoValidacion
                Else
                    identificacion = modificarContrasena.CambioContrasena(Session("idUsuario"), contrasenaEncriptadaNueva)
                    Session("CambioContrasena") = 1
                    PopCambioDeContrasenaIngresoPrimeraVez.ShowOnPageLoad = False
                    lblCambioConfirmacion.Text = "Cambio de contraseña exitoso"
                    Session("recuperacionContrasena") = 4
                    Response.Redirect(Request.RawUrl)
                End If
            Else
                lblMessage.ForeColor = Color.Red
                lblMessage.Text = "La contraseña que desea cambiar no coincide con la que se tenía almacenada"
            End If
        Else
            lblMessage.ForeColor = Color.Red
            lblMessage.Text = "La contraseña nueva y la confirmación de la contraseña deben coincidir"
        End If
        e.Cancel = True
    End Sub

    Protected Sub lbRecuperarContrasena_Click(sender As Object, e As EventArgs) Handles lbRecuperarContrasena.Click
        PopRecuperarContrasena.ShowOnPageLoad = True
    End Sub

    Protected Sub recuperarContrasena_VerifyingUser(sender As Object, e As EventArgs)
        Dim recuperacion As New RecuperarContrasena
        Dim variable As Int64
        If (Int64.TryParse(recuperarContrasena.UserName, variable)) Then
            If recuperacion.recuperacionContrasena(recuperarContrasena.UserName) Then
                PopRecuperarContrasena.ShowOnPageLoad = False
                Session("recuperacionContrasena") = 1
                Response.Redirect(Request.RawUrl)
            Else
                Session("recuperacionContrasena") = 2
                Response.Redirect(Request.RawUrl)
            End If
        Else
            Session("recuperacionContrasena") = 3
            Response.Redirect(Request.RawUrl)
        End If
    End Sub
    Protected Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        If captcha.IsValid Then
            pcCaptcha.ShowOnPageLoad = False
            Response.Redirect(Request.RawUrl)
        End If
    End Sub

    Protected Sub CambioContrasena_CancelButtonClick(sender As Object, e As EventArgs) Handles CambioContrasena.CancelButtonClick
        PopCambioDeContrasenaIngresoPrimeraVez.ShowOnPageLoad = False
    End Sub
End Class