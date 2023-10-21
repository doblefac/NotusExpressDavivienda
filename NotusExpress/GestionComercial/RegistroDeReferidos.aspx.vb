Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General

Partial Public Class RegistroDeReferidos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            AsignarAtributosDeControlDeEnter()
            Session.Remove("InfoCliente")
            If Request.QueryString("referidoPor") IsNot Nothing Then
                txtReferidoPor.Text = Server.HtmlDecode(Request.QueryString("referidoPor").ToString)
            End If
            With epNotificador
                .setTitle("Gestión de Referidos")
                .showReturnLink("~/Administracion/Default.aspx")
            End With
            CargarCiudades()
        End If
    End Sub

    Protected Sub lbConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbConsultar.Click
        Try
            Dim numeroIdentificacion As String = txtIdentificacion.Text.ToString
            Session.Remove("InfoCliente")
            Dim infoCliente As ClienteFinal
            InicializarCampos()
            If Not String.IsNullOrEmpty(numeroIdentificacion) Then
                infoCliente = New ClienteFinal(numeroIdentificacion)
                With infoCliente
                    txtIdentificacion.Text = numeroIdentificacion
                    txtIdentificacion.Enabled = Not .Registrado
                    With ddlTipoIdentificacion
                        .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdTipoIdentificacion))
                    End With
                    txtNombreApellido.Text = .NombreApellido
                    With ddlCiudadResidencia
                        .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdCiudadResidencia))
                    End With
                    txtDireccionResidencia.Text = .DireccionResidencia
                    txtBarrioResidencia.Text = .BarrioResidencia
                    txtTelefonoResidencia.Text = .TelefonoResidencia
                    txtCelular.Text = .Celular
                    txtDireccionOficina.Text = .DireccionOficina
                    txtTelefonoOficina.Text = .TelefonoOficina
                    txtEmail.Text = .Email
                    If .IngresoAproximado > 0 Then txtIngreso.Text = CInt(.IngresoAproximado).ToString
                End With
                If infoCliente.Registrado Then Session("InfoCliente") = infoCliente
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de consultar información. ")
        End Try
    End Sub

    Private Sub InicializarCampos()
        txtIdentificacion.Text = ""
        ddlTipoIdentificacion.ClearSelection()
        txtNombreApellido.Text = ""
        ddlCiudadResidencia.ClearSelection()
        txtDireccionResidencia.Text = ""
        txtBarrioResidencia.Text = ""
        txtTelefonoResidencia.Text = ""
        txtCelular.Text = ""
        txtDireccionOficina.Text = ""
        txtTelefonoOficina.Text = ""
        txtEmail.Text = ""
        txtIngreso.Text = ""
        txtReferidoPor.Text = ""
        txtIdentificacion.Enabled = True
    End Sub

    Protected Sub lbRegistrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbRegistrar.Click
        Dim infoReferido As InformacionReferido
        Dim resultado As ResultadoProceso
        Try
            infoReferido = New InformacionReferido
            If Session("InfoCliente") IsNot Nothing Then infoReferido.InfoCliente = CType(Session("InfoCliente"), ClienteFinal)
            With infoReferido.InfoCliente
                .NumeroIdentificacion = txtIdentificacion.Text.Trim
                .IdTipoIdentificacion = CShort(ddlTipoIdentificacion.SelectedValue)
                .NombreApellido = txtNombreApellido.Text.Trim
                .IdCiudadResidencia = CInt(ddlCiudadResidencia.SelectedValue)
                .DireccionResidencia = txtDireccionResidencia.Text.Trim
                .BarrioResidencia = txtBarrioResidencia.Text.Trim
                .TelefonoResidencia = txtTelefonoResidencia.Text.Trim
                .Celular = txtCelular.Text.Trim
                .DireccionOficina = txtDireccionOficina.Text.Trim
                .TelefonoOficina = txtTelefonoOficina.Text.Trim
                .Email = txtEmail.Text.Trim
                Double.TryParse(txtIngreso.Text.Trim, .IngresoAproximado)
            End With
            With infoReferido
                .ReferidoPor = txtReferidoPor.Text.Trim
                .IdAsesor = CInt(Session("userId"))
                resultado = .Registrar()
            End With
            If resultado.Valor = 0 Then
                epNotificador.showSuccess("La información fue registrada correctamente con Id Referido No. " & infoReferido.IdInfoReferido.ToString)
                InicializarCampos()
            Else
                Select Case resultado.Valor
                    Case 300
                        epNotificador.showError(resultado.Mensaje)
                    Case Else
                        epNotificador.showWarning(resultado.Mensaje)
                End Select
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de registrar información del referido. ")
        Finally
            infoReferido = Nothing
        End Try

    End Sub

    Private Sub CargarCiudades()
        Dim infoCiudad As New Localizacion.CiudadColeccion
        Try
            infoCiudad.CargarDatos()
            With ddlCiudadResidencia
                .DataSource = infoCiudad
                .DataTextField = "ciudadDepartamento"
                .DataValueField = "idCiudad"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de ciudades. ")
        Finally
            ddlCiudadResidencia.Items.Insert(0, New ListItem("Seleccione una Ciudad", "0"))
        End Try
    End Sub

    Protected Sub lbNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbNuevo.Click
        InicializarCampos()
    End Sub

    Private Sub AsignarAtributosDeControlDeEnter()
        For Each ctrl As Control In pnlInformacionPersonal.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Attributes.Add("onkeydown", "ProcesarEnterGeneral(this,'" + lbRegistrar.ClientID + "')")
            End If
        Next
    End Sub
End Class