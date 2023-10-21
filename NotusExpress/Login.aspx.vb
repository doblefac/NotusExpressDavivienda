Imports System.Web.Security
Imports EncryptionClassLibrary.LMEncryption
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.ControlAcceso


Partial Public Class Login
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            ValidarUsoDeDominio()
            EO.Web.Runtime.AddLicense( _
          "jvPMiLnN3fCtfMbRs8uud4SOscufWZekscu7mtvosR/4qdzBs/7rotvp3hDt" + _
          "rpmkBxDxrODz/+ihcqW0s8vnqOr4zs3LiL7d5fDCgcTT0/TLfqXH4PihWabC" + _
          "nrWfWZekscufWbPl9Q+frfD09uihjdjm5B/xouemsSHkq+rtABm8W7Cywc2f" + _
          "oeb3BeihhcbL6v/EfL/R4O3Ihbyy1PrMW5ezz7iJWZekscufWZfA8g/jWev9" + _
          "ARC8W8v29hDVotz7s8v1nun3+hrtdpm9v9uhWd/zBB+8W8PT2ATTfrrM3vrB" + _
          "gsPJv+7OhpmkwOmMQ5ekscufWZekzQzjnZf4ChvkdpnRBhfzosfl+BChWe3p" + _
          "Ax7oqOXBs+StaZmk+RryrbSm3frGksvJ1PPMiLnN3fCtfMbRs8uud4SOscuf" + _
          "WZekscu7mtvosR/4qdzBs+7gpdzy9QzxW5f69h3youbyzs24Z6emsRPurOvB" + _
          "s/fOgNDY1u7HhsbG2vfEZ7rT3s2faLWRm8ufWZekscufddjo9cvzsufpzs3C" + _
          "muPw8wzipJmkBxDxrODz/+ihcqW0s8vnqOr4zs3LiL7d5fDCgcTT0/TLfqXH" + _
          "4PihWabCnrWfWZekscufWbPl9Q+frfD09uihesHF6QDvpebl9RDxW5f69h3y" + _
          "oubyzs24Z6emsRPurOvBs/fOgNDY1u7HhsbG2vfEZ7rT3s2faLWRm8ufWZek" + _
          "scufddjo9cvzsufpzs3DotjwABKhWe3pAx7oqOXBs+StaZmk+RryrbSm3frG" + _
          "ksvJ1PPMiLnN3fCtfMbRs8uud4SOscufWZekscu7mtvosR/4qdzBs/7vpeD4" + _
          "BRDxW5f69h3youbyzs24Z6emsRPurOvBs/fOgNDY1u7HhsbG2vfEZ7rT3s2f" + _
          "aLWRm8ufWZekscufddjo9cvzsufpzs3Mmurv9g/EneD4s8v1nun3+hrtdpm9" + _
          "v9uhWd/zBB+8W8PT2ATTfrrM3vrBgsPJv+7OhpmkwOmMQ5ekscufWZekzQzj" + _
          "nZf4ChvkdpnLAxTjW5f69h3youbyzs24Z6emsRPurOvBs/fOgNDY1u7HhsbG" + _
          "2vfEZ7rT3s2faLWRm8ufWZekscufddjo9cvzsufpzs3CqOPzA/vonOLpA82f" + _
          "r9z2BBTup7SmytmvW5fsAB7zdpnQ4PLYjbzH2fjOe8DQ1tnCiMSmsdq9RoGk" + _
          "scufWZeksefgndukBSTvnrSm5BvkpePH+RDipNz2s8v1nun3+hrtdpm9v9uh" + _
          "Wd/zBB+8W8PT2ATTfrrM3vrBgsPJv+7OhpmkwOmMQ5ekscufWZekzQzjnZf4" + _
          "ChvkdpnJ9RTzqOmmsSHkq+rtABm8W7Cywc2foeb3BeihhcbL6v/EfL/R4O3I" + _
          "hbyy1PrMW5ezz7iJWZekscufWZfA8g/jWev9ARC8W8Dx8hLkk+bz/s2fr9z2" + _
          "BBTup7SmytmvW5fsAB7zdpnQ4PLYjbzH2fjOe8DQ1tnCiMSmsdq9RoGkscuf" + _
          "WZeksefgndukBSTvnrSm1Rr2p+Pz8g/kq5mkBxDxrODz/+ihcqW0s8vnqOr4" + _
          "zs3LiL7d5fDCgcTT0/TLfqXH4PihWabCnrWfWZekscufWbPl9Q+frfD09uih" + _
          "f+Pz8h/kq5mkBxDxrODz/+ihcqW0s8vnqOr4zs3LiL7d5fDCgcTT0/TLfqXH" + _
          "4PihWabCnrWfWZekscufWbPl9Q+frfD09uihjOPt9RChWe3pAx7oqOXBs+St" + _
          "aZmk+RryrbSm3frGksvJ1PPMiLnN3fCtfMbRs8uud4SOscufWZekscu7mtvo" + _
          "sR/4qdzBs/Hrsub5Bc2fr9z2BBTup7SmytmvW5fsAB7zdpnQ4PLYjbzH2fjO" + _
          "e8DQ1tnCiMSmsdq9RoGkscufWZeksefgndukBSTvnrSm1g/ordjm/RDLmtnp" + _
          "/c2fr9z2BBTup7SmytmvW5fsAB7zdpnQ4PLYjbzH2fjOe8DQ1tnCiMSmsdq9" + _
          "RoGkscufdabl/RfusLWRm8ufWZfAAB3jnunN/xHuWdvlBRC8W6mzwtqxaai1" + _
          "s8v1nun3+hrtdpm8s8uud4SOscufWbP3+hLtmuv5AxC9gN242hvSr9zd+xrW" + _
          "mrzK2fbPa7/w0ui8dab3+hLtmuv5AxC9RoHAwBfonNzyBBC9RoF14+30EO2s" + _
          "3MKetZ9Zl6TNF+ic3PIEEMidtbfF3rFuqLbI4bN1pvD6DuSn6unaD71GgaSx" + _
          "y5914+30EO2s3OnP566l4Of2GfKe3MKetZ9Zl6TNDOul5vvPuIlZl6Sxy59Z" + _
          "l8DyD+NZ6/0BELxbxOn/IKFZ7ekDHuio5cGz5K1pmaT5GvKttKbd+saSy8nU")
            If Request.QueryString("err") IsNot Nothing Then
                If Request.QueryString("codErr") IsNot Nothing Then
                    Select Case Request.QueryString("codErr")
                        Case "1"
                            lblError.Text = "Acceso denegado. Usuario no tiene menú de opciones disponible."
                        Case "2"
                            lblError.Text = "Acceso denegado. Imposible obtener información del Usuario."
                        Case "3"
                            lblError.Text = "Acceso denegado. Usuario no tiene perfiles asignados."
                        Case "4"
                            lblError.Text = "Acceso denegado. Está tratando de acceder directamente a recurso protegido."
                        Case "5"
                            lblError.Text = "Su sesión ha expirado."
                    End Select
                Else
                    lblError.Text = "Ocurrió un error al tratar de cargar la página de Inicio por favor vuelva a ingresar."
                End If
            End If
        End If
    End Sub

    Private Sub ValidarUsoDeDominio()
        Dim dominio As String
        Try
            dominio = ConfigurationManager.AppSettings("nombreDominio")
            If dominio IsNot Nothing Then
                Dim url As String = Request.Url.ToString
                If Not url.ToLower.Contains("localhost") Then
                    Dim ipGateway As String = ConfigurationManager.AppSettings("IP_GATEWAY")
                    Dim ipRemota As String = Request.ServerVariables("REMOTE_ADDR")
                    Dim oReg As New System.Text.RegularExpressions.Regex("\b(?:\d{1,3}\.){3}\d{1,3}\b")

                    If ipGateway Is Nothing Then ipGateway = ""
                    If (Not url.ToLower.Contains(dominio)) AndAlso (ipRemota <> ipGateway) AndAlso Not oReg.IsMatch(Request.ServerVariables("SERVER_NAME")) Then
                        url = url.Replace(Request.ServerVariables("SERVER_NAME"), Request.ServerVariables("SERVER_NAME") & "." & dominio)
                        Response.Redirect(url)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de validar uso de dominio. "
        End Try
    End Sub


End Class