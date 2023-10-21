Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.Reportes

Partial Public Class GestionDeVenta
    Inherits System.Web.UI.Page

#Region "Atributos Privados"

    Dim idRol As Integer

#End Region

#Region "Delegados de Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Session("idRol") IsNot Nothing Then Integer.TryParse(Session("idRol").ToString, idRol)
        If Not Me.IsPostBack Then
            Session.Remove("InfoCliente")
            Session.Remove("InfoPreventa")
            With epNotificador
                .setTitle("Gestión de Ventas")
                '.showReturnLink("~/Administracion/Default.aspx")
            End With
            dpFechaVenta.MaxValidDate = Now
            InicializarControlesOrigenRegistro()

            CargarCiudades()
            CargarListaEstatusGeneral()
            CargarListaResultadoProceso()
            CargarTiposDeProducto()
            CargarListadoDeProductosPadre()
            CargarListadoDeSubproductos()
            pnlGestion.Visible = False
            txtTransaccionConsulta.Focus()
        End If
    End Sub

    Protected Sub lbConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbConsultar.Click
        Try
            lblRegistrarPreventa.Visible = False
            lbCancelar.Visible = False
            gvHistoricoVenta.DataBind()
            If Len(txtIdentificacionConsulta.Text.Trim) >= 1 Or Len(txtTransaccionConsulta.Text.Trim) >= 1 Then

                If Len(txtTransaccionConsulta.Text.Trim) >= 1 Then
                    InicializarCampos()
                    lblTipoConsulta.Text = "1"
                    'aquí consultamos la transacción para obtener el número de cédula
                    Dim infoClienteTransaccion As New TransaccionCliente(txtTransaccionConsulta.Text.Trim)
                    With infoClienteTransaccion
                        txtIdentificacionConsulta.Text = .NumeroIdentificacion
                        lblIdentificacionAnterior.Text = .NumeroIdentificacion
                    End With

                    If txtIdentificacionConsulta.Text.Trim = txtTransaccionConsulta.Text.Trim Then
                        lblExisteTransaccion.Text = "0"
                    Else
                        lblExisteTransaccion.Text = "1"
                        lblTransaccionExistente.Text = txtTransaccionConsulta.Text.Trim
                    End If

                    Dim infoCliente As New ClienteFinal(txtIdentificacionConsulta.Text.Trim)

                    With infoCliente
                        If lblExisteTransaccion.Text = "1" Then
                            txtIdentificacion.Text = txtIdentificacionConsulta.Text.Trim
                        Else
                            txtIdentificacion.Text = ""
                        End If
                        ddlPdv.Enabled = True
                        ddlPdv.SelectedIndex = ddlPdv.Items.IndexOf(ddlPdv.Items.FindByValue(.IdPdv))
                        CargarAsesoresComerciales()
                        ddlAsesorComercial.Enabled = True
                        ddlAsesorComercial.SelectedIndex = ddlAsesorComercial.Items.IndexOf(ddlAsesorComercial.Items.FindByValue(.IdUsuarioAsesor))

                        If lblExisteTransaccion.Text = "1" Then
                            ddlPdv.Enabled = False
                            ddlAsesorComercial.Enabled = False
                            txtIdentificacion.Enabled = True
                        End If
                        'txtIdentificacion.Enabled = Not .Registrado
                        lblIdClienteOriginal.Text = .IdCliente
                        With ddlTipoIdentificacion
                            .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdTipoIdentificacion))
                        End With
                        txtNombreApellido.Text = .NombreApellido
                        ddlCiudadResidencia.SelectedIndex = ddlCiudadResidencia.Items.IndexOf(ddlCiudadResidencia.Items.FindByValue(infoCliente.IdCiudadResidencia))
                        txtDireccionResidencia.Text = .DireccionResidencia
                        txtBarrioResidencia.Text = .BarrioResidencia
                        txtTelefonoResidencia.Text = .TelefonoResidencia
                        txtCelular.Text = .Celular
                        txtDireccionOficina.Text = .DireccionOficina
                        txtTelefonoOficina.Text = .TelefonoOficina
                        txtEmail.Text = .Email
                        If .IngresoAproximado > 0 Then txtIngreso.Text = CInt(.IngresoAproximado).ToString
                        With ddlEstatusLaboral
                            .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdEstatusLaboral))
                        End With
                    End With
                    If infoCliente.Registrado Then Session("InfoCliente") = infoCliente
                    txtIdentificacionConsulta.Text = ""
                    pnlConsulta.Visible = False
                    pnlGestion.Visible = True
                    pnlDatosPersonales.Visible = True
                    pnlInfoOrigenGestion.Visible = True
                    pnlRegistroPreventa.Visible = True
                    pnlHistoricoVenta.Visible = False
                    'CambiarVistaTabScript("PREVENTA")
                    '/////////////codigo nuevo para mostrar los datos de la venta
                    'If lblEsNueva.Text = "1" Then
                    Dim informacionVenta As InfoVenta
                    If lblEsNueva.Text = "0" Then
                        informacionVenta = New InfoVenta(txtTransaccionConsulta.Text.Trim)

                    Else
                        informacionVenta = New InfoVenta(0)
                    End If
                    With informacionVenta
                        dpFechaVenta.SelectedDate = .FechaGestion
                        txtAtendidoPor.Text = .OperadorCall
                        txtNumIdOperadorCallCenter.Text = .IdentificacionOperadorCall
                        txtNumPlanillaPreAnalisis.Text = .NumPlanillaPreAnalisis
                        txtNumVentaPlanilla.Text = .NumVentaPlanilla
                        ddlResultadoConsulta.SelectedIndex = ddlResultadoConsulta.Items.IndexOf(ddlResultadoConsulta.Items.FindByValue(.IdResultadoProceso))
                        If .IdResultadoProceso = 1 Then
                            ddlTipoProducto.Enabled = True
                            CargarTiposDeProducto(.IdResultadoProceso)
                            ddlTipoProducto.SelectedIndex = ddlTipoProducto.Items.IndexOf(ddlTipoProducto.Items.FindByValue(.IdTipoVenta))
                            If .IdTipoVenta = 1 Then
                                trInfoProducto.Visible = True
                                trInfoSerial.Visible = True
                                ddlProductoPadre.Enabled = True
                                CargarListadoDeProductosPadre()
                                ddlProductoPadre.SelectedIndex = ddlProductoPadre.Items.IndexOf(ddlProductoPadre.Items.FindByValue(.IdProducto))
                                ddlSubproducto.Enabled = True
                                CargarListadoDeSubproductos(.IdProducto)
                                ddlSubproducto.SelectedIndex = ddlSubproducto.Items.IndexOf(ddlSubproducto.Items.FindByValue(.IdSubProducto))
                                txtNumPagare.Text = .NumPagare
                                txtSerialTarjeta.Text = .Serial
                            End If
                        End If
                        txtObservacionOperadorCall.Text = .ObservacionCallCenter
                        chkNovedad.Enabled = True
                        If .EsNovedad = True Then
                            trInfoNovedad.Visible = True
                            trAccionesNovedad.Visible = True
                            trDetalleNovedad.Visible = True
                            ddlTipoNovedad.Enabled = True
                            chkNovedad.Checked = True
                            CargarTipoDeNovedad()
                            ObtenerDatosNovedades(txtTransaccionConsulta.Text.Trim)
                            'ddlTipoNovedad.SelectedValue = .TipoNovedad
                            'txtObservacionesNovedad.Text = .ObservacionNovedad
                        Else
                            chkNovedad.Checked = False
                            trInfoNovedad.Visible = False
                            trAccionesNovedad.Visible = False
                            trDetalleNovedad.Visible = False
                            ddlTipoNovedad.Enabled = False
                        End If
                    End With
                    lblEsNueva.Text = "0"
                    txtAtendidoPor.Enabled = True
                    ddlResultadoConsulta.Enabled = False
                    ddlTipoProducto.Enabled = False
                    ddlProductoPadre.Enabled = False
                    txtSerialTarjeta.Enabled = False
                    pnlInfoVenta.Visible = True
                    'Else
                    '    CambiarVistaTabScript("PREVENTA")
                    '    chkNovedad.Enabled = False
                    'End If
                    '//////////////////////////////////
                Else
                    Dim identificacion As String
                    identificacion = txtIdentificacionConsulta.Text.Trim
                    InicializarCampos()
                    lblTipoConsulta.Text = "2"
                    Dim infoCliente As New ClienteFinal(txtIdentificacionConsulta.Text.Trim)

                    With infoCliente
                        txtIdentificacion.Text = txtIdentificacionConsulta.Text.Trim
                        txtIdentificacion.Enabled = Not .Registrado
                        With ddlTipoIdentificacion
                            .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdTipoIdentificacion))
                        End With
                        txtNombreApellido.Text = .NombreApellido
                        ddlCiudadResidencia.SelectedIndex = ddlCiudadResidencia.Items.IndexOf(ddlCiudadResidencia.Items.FindByValue(infoCliente.IdCiudadResidencia))
                        txtDireccionResidencia.Text = .DireccionResidencia
                        txtBarrioResidencia.Text = .BarrioResidencia
                        txtTelefonoResidencia.Text = .TelefonoResidencia
                        txtCelular.Text = .Celular
                        txtDireccionOficina.Text = .DireccionOficina
                        txtTelefonoOficina.Text = .TelefonoOficina
                        txtEmail.Text = .Email
                        If .IngresoAproximado > 0 Then txtIngreso.Text = CInt(.IngresoAproximado).ToString
                        With ddlEstatusLaboral
                            .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdEstatusLaboral))
                        End With
                    End With
                    If infoCliente.Registrado Then Session("InfoCliente") = infoCliente
                    txtIdentificacionConsulta.Text = ""
                    pnlConsulta.Visible = False
                    pnlGestion.Visible = True
                    pnlDatosPersonales.Visible = False
                    pnlInfoOrigenGestion.Visible = False
                    pnlHistoricoVenta.Visible = True
                    CambiarVistaTabScript("HISTORICO")
                    ObtenerDatosReporte(identificacion)
                End If
            ElseIf Len(txtIdentificacionConsulta.Text.Trim) <= 0 Then
                epNotificador.showError("El campo Identificación o Transacción debe contener algún valor.")
                Exit Sub
            ElseIf Len(txtTransaccionConsulta.Text.Trim) <= 0 Then
                epNotificador.showError("El campo Identificación o Transacción debe contener algún valor.")
                Exit Sub
            End If
        Catch ex As Exception
            If ex.Message = "Error al convertir el valor del parámetro de String a Int32." Then
                epNotificador.showWarning("El número de transacción " & txtTransaccionConsulta.Text.Trim & " no existe.")
            Else
                epNotificador.showError("Error al tratar de consultar información. ")
            End If

        End Try
    End Sub

    Protected Sub lblRegistrarPreventa_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lblRegistrarPreventa.Click
        If lblExisteTransaccion.Text = "0" Then
            txtAtendidoPor.Enabled = True
            ddlResultadoConsulta.Enabled = True
            ddlTipoProducto.Enabled = True
            ddlProductoPadre.Enabled = True
            txtSerialTarjeta.Enabled = True
            Dim infoPreventa As New InfoPreventa
            Dim resultado As ResultadoProceso
            Try
                If Session("InfoCliente") IsNot Nothing Then infoPreventa.InfoCliente = CType(Session("InfoCliente"), ClienteFinal)
                With infoPreventa.InfoCliente
                    lblIdClienteOriginal.Text = .IdCliente
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
                    .IdEstatusLaboral = CInt(ddlEstatusLaboral.SelectedValue)
                End With
                With infoPreventa
                    .IdUsuarioRegistra = CInt(Session("userId"))
                    If idRol <> 3 Then
                        .IdUsuarioAsesor = CInt(ddlAsesorComercial.SelectedValue)
                        .IdPdv = CInt(ddlPdv.SelectedValue)
                    Else
                        .IdUsuarioAsesor = CInt(Session("userId"))
                    End If

                    resultado = .Registrar()
                    ''AQUÍ CONSULTAMOS SI EL NUMERO DE IDENTIFICACION NUEVO EXISTE Y TIENE VENTAS
                    'Dim infoIdCliente As New ObtenerIdCliente(txtIdentificacion.Text.Trim)
                    'If infoIdCliente.IdCliente = 0 Then
                    '    'NO EXISTE Y ACTUALIZAMOS EL NUMERO DE IDENTIFICACION
                    '    Dim actualizarIdCliente As New ActualizarIdentificacionCliente()
                    '    With actualizarIdCliente
                    '        .IdCliente = lblIdClienteOriginal.Text
                    '        .NumeroIdentificacion = txtIdentificacion.Text.Trim
                    '        .Actualizar()
                    '    End With

                    'Else

                    'End If

                End With
                If resultado.Valor = 0 Then
                    If lblTipoConsulta.Text = "1" Then
                        'epNotificador.showSuccess("La preventa fue registrada satisfactoriamente. Transacci&oacute;n No. " & infoPreventa.IdPreventa)
                        'lblIdClienteOriginal.Text = "0"
                        'lblTieneVentas.Text = "0"
                        'lblIdentificacionAnterior.Text = "0"
                        'If pnlInfoOrigenGestion.Visible Then
                        '    ddlPdv.Enabled = False
                        '    ddlAsesorComercial.Enabled = False
                        'End If
                        'CambiarVistaTabScript("VENTA")

                        'txtAtendidoPor.Focus()
                        'Session("InfoPreventa") = infoPreventa

                        'Dim informacionVenta As InfoVenta
                        'If lblEsNueva.Text = "0" Then
                        '    informacionVenta = New InfoVenta(txtTransaccionConsulta.Text.Trim)

                        'Else
                        '    informacionVenta = New InfoVenta(0)
                        'End If
                        'With informacionVenta
                        '    dpFechaVenta.SelectedDate = .FechaGestion
                        '    txtAtendidoPor.Text = .OperadorCall
                        '    txtNumIdOperadorCallCenter.Text = .IdentificacionOperadorCall
                        '    txtNumPlanillaPreAnalisis.Text = .NumPlanillaPreAnalisis
                        '    txtNumVentaPlanilla.Text = .NumVentaPlanilla
                        '    ddlResultadoConsulta.SelectedValue = .IdResultadoProceso
                        '    If .IdResultadoProceso = 1 Then
                        '        ddlTipoProducto.Enabled = True
                        '        CargarTiposDeProducto(.IdResultadoProceso)
                        '        ddlTipoProducto.SelectedValue = .IdTipoVenta
                        '        If .IdTipoVenta = 1 Then
                        '            trInfoProducto.Visible = True
                        '            trInfoSerial.Visible = True
                        '            ddlProductoPadre.Enabled = True
                        '            CargarListadoDeProductosPadre()
                        '            ddlProductoPadre.SelectedValue = .IdProducto
                        '            ddlSubproducto.Enabled = True
                        '            CargarListadoDeSubproductos(.IdProducto)
                        '            ddlSubproducto.SelectedValue = .IdSubProducto
                        '            txtNumPagare.Text = .NumPagare
                        '            txtSerialTarjeta.Text = .Serial
                        '        End If
                        '    End If
                        '    txtObservacionOperadorCall.Text = .ObservacionCallCenter
                        '    If .Novedad = 1 Then
                        '        trInfoNovedad.Visible = True
                        '        ddlTipoNovedad.Enabled = True
                        '        trAccionesNovedad.Visible = True
                        '        trDetalleNovedad.Visible = True
                        '        chkNovedad.Checked = True
                        '        CargarTipoDeNovedad()
                        '        ObtenerDatosNovedades(txtTransaccionConsulta.Text.Trim)
                        '        'ddlTipoNovedad.SelectedValue = .TipoNovedad
                        '        'txtObservacionesNovedad.Text = .ObservacionNovedad
                        '    Else
                        '        chkNovedad.Checked = False
                        '        trInfoNovedad.Visible = False
                        '        trAccionesNovedad.Visible = False
                        '        trDetalleNovedad.Visible = False
                        '        ddlTipoNovedad.Enabled = False
                        '        gvNovedades.DataBind()
                        '    End If
                        'End With
                        'lblEsNueva.Text = "0"
                    Else
                        epNotificador.showSuccess("La preventa fue registrada satisfactoriamente. Transacci&oacute;n No. " & infoPreventa.IdPreventa)
                        If pnlInfoOrigenGestion.Visible Then
                            ddlPdv.Enabled = False
                            ddlAsesorComercial.Enabled = False
                        End If
                        CambiarVistaTabScript("HISTORICO")
                        'txtAtendidoPor.Focus()
                        ObtenerDatosReporte("1")

                        Session("InfoPreventa") = infoPreventa

                    End If
                    Else
                        Select Case resultado.Valor
                            Case 300
                                epNotificador.showError(resultado.Mensaje)
                            Case Else
                                epNotificador.showWarning(resultado.Mensaje)
                        End Select
                End If
                'lblIdClienteOriginal.Text = "0"
                lblTieneVentas.Text = "0"
                lblIdentificacionAnterior.Text = "0"
            Catch ex As Exception
                epNotificador.showError("Error al tratar de registrar preventa. ")
            End Try
        Else
            Dim infoPreventa As New InfoPreventa
            Dim resultado As ResultadoProceso
            Try
                If Session("InfoCliente") IsNot Nothing Then infoPreventa.InfoCliente = CType(Session("InfoCliente"), ClienteFinal)
                With infoPreventa.InfoCliente
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
                    .IdEstatusLaboral = CInt(ddlEstatusLaboral.SelectedValue)
                End With
                With infoPreventa
                    .IdUsuarioRegistra = CInt(Session("userId"))
                    If idRol <> 3 Then
                        .IdUsuarioAsesor = CInt(ddlAsesorComercial.SelectedValue)
                        .IdPdv = CInt(ddlPdv.SelectedValue)
                    Else
                        .IdUsuarioAsesor = CInt(Session("userId"))
                    End If
                    .IdPreventaA = CInt(lblTransaccionExistente.Text)

                    'AQUÍ CONSULTAMOS SI EL NUMERO DE IDENTIFICACION NUEVO EXISTE Y TIENE VENTAS
                    Dim infoIdCliente As New ObtenerIdCliente(txtIdentificacion.Text.Trim)
                    If infoIdCliente.IdCliente = 0 Then
                        'NO EXISTE POR TANTO ACTUALIZAMOS EL NUMERO DE IDENTIFICACION Y REGISTRAMOS LA PREVENTA
                        Dim actualizarIdCliente As New ActualizarIdentificacionCliente()
                        With actualizarIdCliente
                            .IdCliente = lblIdClienteOriginal.Text
                            .NumeroIdentificacion = txtIdentificacion.Text.Trim
                            .Actualizar()
                        End With
                        resultado = .Actualizar()
                    Else
                        'EXISTE. ACTUALIZAMOS EL IDCLIENTE DE ESA PREVENTA 
                        Dim actualizarIdClientePreventa As New ActualizarIdClientePreventa()
                        With actualizarIdClientePreventa
                            .IdCliente = infoIdCliente.IdCliente
                            .IdPreventa = lblTransaccionExistente.Text
                            .Actualizar()
                        End With
                        resultado = .Actualizar()
                    End If

                End With
                If resultado.Valor = 0 Then
                    If lblTipoConsulta.Text = "1" Then
                        'epNotificador.showSuccess("La preventa fue actualizada satisfactoriamente. Transacci&oacute;n No. " & lblTransaccionExistente.Text)

                        'If pnlInfoOrigenGestion.Visible Then
                        '    ddlPdv.Enabled = False
                        '    ddlAsesorComercial.Enabled = False
                        'End If
                        'If lblEsNueva.Text = "1" Then
                        'Else
                        '    CambiarVistaTabScript("VENTA")
                        'End If
                        'txtAtendidoPor.Focus()
                        'Session("InfoPreventa") = infoPreventa

                        'Dim informacionVenta As New InfoVenta(txtTransaccionConsulta.Text.Trim)
                        'With informacionVenta
                        '    dpFechaVenta.SelectedDate = .FechaGestion
                        '    txtAtendidoPor.Text = .OperadorCall
                        '    txtNumIdOperadorCallCenter.Text = .IdentificacionOperadorCall
                        '    txtNumPlanillaPreAnalisis.Text = .NumPlanillaPreAnalisis
                        '    txtNumVentaPlanilla.Text = .NumVentaPlanilla
                        '    ddlResultadoConsulta.SelectedValue = .IdResultadoProceso
                        '    If .IdResultadoProceso = 1 Then
                        '        ddlTipoProducto.Enabled = True
                        '        CargarTiposDeProducto(.IdResultadoProceso)
                        '        ddlTipoProducto.SelectedValue = .IdTipoVenta
                        '        If .IdTipoVenta = 1 Then
                        '            trInfoProducto.Visible = True
                        '            trInfoSerial.Visible = True
                        '            ddlProductoPadre.Enabled = True
                        '            CargarListadoDeProductosPadre()
                        '            ddlProductoPadre.SelectedValue = .IdProducto
                        '            ddlSubproducto.Enabled = True
                        '            CargarListadoDeSubproductos(.IdProducto)
                        '            ddlSubproducto.SelectedValue = .IdSubProducto
                        '            txtNumPagare.Text = .NumPagare
                        '            txtSerialTarjeta.Text = .Serial
                        '        End If
                        '    End If
                        '    txtObservacionOperadorCall.Text = .ObservacionCallCenter
                        '    chkNovedad.Enabled = True
                        '    If .Novedad = 1 Then
                        '        trInfoNovedad.Visible = True
                        '        trAccionesNovedad.Visible = True
                        '        trDetalleNovedad.Visible = True
                        '        ddlTipoNovedad.Enabled = True
                        '        chkNovedad.Checked = True
                        '        CargarTipoDeNovedad()
                        '        ObtenerDatosNovedades(txtTransaccionConsulta.Text.Trim)
                        '        'ddlTipoNovedad.SelectedValue = .TipoNovedad
                        '        'txtObservacionesNovedad.Text = .ObservacionNovedad
                        '    Else
                        '        chkNovedad.Checked = False
                        '        trInfoNovedad.Visible = False
                        '        trAccionesNovedad.Visible = False
                        '        trDetalleNovedad.Visible = False
                        '        ddlTipoNovedad.Enabled = False
                        '        gvNovedades.DataBind()
                        '    End If
                        'End With
                    Else
                        epNotificador.showSuccess("La preventa fue registrada satisfactoriamente. Transacci&oacute;n No. " & infoPreventa.IdPreventa)
                        If pnlInfoOrigenGestion.Visible Then
                            ddlPdv.Enabled = False
                            ddlAsesorComercial.Enabled = False
                        End If
                        CambiarVistaTabScript("HISTORICO")
                        'txtAtendidoPor.Focus()
                        ObtenerDatosReporte("1")

                        Session("InfoPreventa") = infoPreventa
                    End If
                Else
                    Select Case resultado.Valor
                        Case 300
                            epNotificador.showError(resultado.Mensaje)
                        Case Else
                            epNotificador.showWarning(resultado.Mensaje)
                    End Select
                End If
                txtAtendidoPor.Enabled = True
                ddlResultadoConsulta.Enabled = False
                ddlTipoProducto.Enabled = False
                ddlProductoPadre.Enabled = False
                txtSerialTarjeta.Enabled = False
            Catch ex As Exception
                epNotificador.showError("Error al tratar de registrar preventa. ")
            End Try
            lblIdClienteOriginal.Text = "0"
            lblTieneVentas.Text = "0"
            lblIdentificacionAnterior.Text = "0"
        End If
    End Sub

    Public Sub ObtenerDatosReporte(ByVal identificacion As String)
        Dim dtDatos As New DataTable

        Dim infoHistoria As New HistoriaTransacciones(identificacion)
        With infoHistoria
            dtDatos = .DatosReporte
        End With

        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            EnlazarDatos(dtDatos)
        Else
            epNotificador.showWarning("El cliente no tiene ventas registradas.")
        End If
    End Sub

    Private Sub EnlazarDatos(ByVal dtDatos As DataTable)
        With gvHistoricoVenta
            .DataSource = dtDatos
            .DataBind()
        End With

    End Sub

    Public Sub ObtenerDatosNovedades(ByVal idGestionVenta As Integer)
        Dim dtDatos As New DataTable

        Dim infoHistoria As New ConsultarNovedades(idGestionVenta)
        With infoHistoria
            dtDatos = .DatosReporte
        End With

        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            EnlazarDatosNovedades(dtDatos)
        Else
            gvNovedades.DataBind()
            chkNovedad.Checked = False
            trInfoNovedad.Visible = False
            trAccionesNovedad.Visible = False
            trDetalleNovedad.Visible = False
            txtObservacionesNovedad.Text = ""
            chkNovedad.Enabled = True
        End If
    End Sub

    Public Sub ObtenerDatosNovedadesTemporales(ByVal idCliente As Integer)
        Dim dtDatos As New DataTable

        Dim infoHistoria As New ConsultarNovedadesTemporales(idCliente)
        With infoHistoria
            dtDatos = .DatosReporte
        End With

        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            EnlazarDatosNovedades(dtDatos)
        Else
            gvNovedades.DataBind()
            chkNovedad.Checked = False
            trInfoNovedad.Visible = False
            trAccionesNovedad.Visible = False
            trDetalleNovedad.Visible = False
            txtObservacionesNovedad.Text = ""
            chkNovedad.Enabled = True
        End If
    End Sub

    Private Sub EnlazarDatosNovedades(ByVal dtDatos As DataTable)
        With gvNovedades
            .DataSource = dtDatos
            .DataBind()
        End With

    End Sub

    Protected Sub lbCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCancelar.Click
        InicializarControlesOrigenRegistro()
        InicializarCampos()
        pnlConsulta.Visible = True
        pnlGestion.Visible = False
        For Each ctrl As Control In pnlConsulta.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            ElseIf TypeOf ctrl Is DropDownList Then
                CType(ctrl, DropDownList).ClearSelection()
            End If
        Next
        txtTransaccionConsulta.Text = ""
        lblTransaccionExistente.Text = "0"
        lblExisteTransaccion.Text = ""
        lblTipoConsulta.Text = "0"
        lblEsNueva.Text = "0"
        lblIdClienteOriginal.Text = "0"
        lblTieneVentas.Text = "0"
        lblIdentificacionAnterior.Text = "0"
        txtTransaccionConsulta.Focus()
    End Sub

    Protected Sub lbCancelarVenta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCancelarVenta.Click
        Try
            InicializarCampos()
            For Each ctrl As Control In pnlConsulta.Controls
                If TypeOf ctrl Is TextBox Then
                    CType(ctrl, TextBox).Text = ""
                ElseIf TypeOf ctrl Is DropDownList Then
                    CType(ctrl, DropDownList).ClearSelection()
                End If
            Next
            pnlConsulta.Visible = True
            pnlGestion.Visible = False
            If Session("InfoPreventa") IsNot Nothing Then
                Dim resultado As ResultadoProceso = CType(Session("InfoPreventa"), InfoPreventa).Eliminar(CInt(Session("userId")))
                If resultado.Valor <> 0 Then
                    Select Case resultado.Valor
                        Case 300
                            epNotificador.showError(resultado.Mensaje)
                        Case Else
                            epNotificador.showWarning(resultado.Mensaje)
                    End Select
                End If
            End If
            ddlPdv.Enabled = True
            ddlAsesorComercial.Enabled = True
            txtTransaccionConsulta.Text = ""
            lblTransaccionExistente.Text = "0"
            lblExisteTransaccion.Text = ""
            lblTipoConsulta.Text = "0"
            lblEsNueva.Text = "0"
            lblIdClienteOriginal.Text = "0"
            lblTieneVentas.Text = "0"
            lblIdentificacionAnterior.Text = "0"
            txtTransaccionConsulta.Focus()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cancelar registro de venta. ")
        End Try
    End Sub

    Private Sub LimpiarYControlarVisualizacionDeCamposDeProducto(ByVal visible As Boolean)
        ddlProductoPadre.ClearSelection()
        ddlSubproducto.ClearSelection()
        txtNumPagare.Text = ""
        txtSerialTarjeta.Text = ""
        trInfoProducto.Visible = visible
        trInfoSerial.Visible = visible
    End Sub

    Private Sub lbRegistrarVenta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRegistrarVenta.Click
        If ddlPdv.SelectedIndex = 0 Then
            rfvPdv.Text = "Punto de venta requerido"
            rfvPdv.Validate()
            rfvPdv.Visible = True
        End If
        If ddlAsesorComercial.SelectedIndex = 0 Then
            rfvPdv.Text = "Asesor comercial requerido"
            rfvPdv.Validate()
            rfvPdv.Visible = True
        End If

        'AQUÍ REGISTRAMOS PREVENTA
        lblRegistrarPreventa_Click(sender, e)

        Dim diaminimo As Date
        diaminimo = DateTime.Now.AddDays(-30)
        If dpFechaVenta.SelectedDate < diaminimo Then
            epNotificador.showWarning("La fecha de venta no puede ser 30 días antes de la fecha actual.")
            Exit Sub
        End If

        If dpFechaVenta.SelectedDate > DateTime.Now.Date Then
            epNotificador.showWarning("La fecha de venta no puede ser mayor a la fecha actual.")
            Exit Sub
        End If

        If lblExisteTransaccion.Text = "0" Then
            Dim infoVenta As New InfoGestionVenta
            Dim _infoCliente As ClienteFinal
            Dim resultado As ResultadoProceso

            Try
                If Session("InfoPreventa") IsNot Nothing Then
                    _infoCliente = CType(Session("InfoPreventa"), InfoPreventa).InfoCliente
                Else
                    _infoCliente = New ClienteFinal(txtIdentificacion.Text.Trim)
                End If
                With _infoCliente

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
                    .IdEstatusLaboral = CInt(ddlEstatusLaboral.SelectedValue)
                End With
                With infoVenta
                    .OperadorCall = txtAtendidoPor.Text.Trim
                    .IdentificacionOperadorCall = txtNumIdOperadorCallCenter.Text.Trim
                    .IdResultadoProceso = CInt(ddlResultadoConsulta.SelectedValue)
                    .IdUsuarioRegistra = CInt(Session("userId"))
                    If idRol <> 3 Then
                        .IdUsuarioAsesor = CInt(ddlAsesorComercial.SelectedValue)
                        .IdPdv = CInt(ddlPdv.SelectedValue)
                    Else
                        .IdUsuarioAsesor = CInt(Session("userId"))
                    End If
                    .IdTipoVenta = CShort(ddlTipoProducto.SelectedValue)
                    .IdSubproducto = CInt(ddlSubproducto.SelectedValue)
                    .Serial = txtSerialTarjeta.Text.Trim 'meSerialTarjeta.Text.Replace("-", "").Replace(" ", "").Trim
                    .NumeroPlanillaPreAnalisis = CInt(txtNumPlanillaPreAnalisis.Text)
                    .NumeroVentaPlanilla = CInt(txtNumVentaPlanilla.Text)
                    .NumeroPagare = txtNumPagare.Text.Trim
                    .ObservacionCallCenter = txtObservacionOperadorCall.Text.Trim
                    .FechaGestion = dpFechaVenta.SelectedDate
                    If chkNovedad.Checked Then
                        .EsNovedad = True
                        
                    Else
                        .EsNovedad = False

                    End If
                    resultado = .Registrar(_infoCliente)
                End With
                If resultado.Valor = 0 Then
                    Dim idResultado As String = ddlResultadoConsulta.SelectedValue
                    Dim nombreCliente As String = txtNombreApellido.Text.Trim
                    InicializarCampos()
                    pnlConsulta.Visible = True
                    pnlGestion.Visible = False
                    lblRegistroOk.Text = "<ul><li>El registro fue realizado satisfactoriamente con. Transacci&oacute;n No. " & infoVenta.IdGestionVenta.ToString & "</li></ul>"
                    epNotificador.showSuccess("El registro fue realizado satisfactoriamente con. Transacci&oacute;n No. " & infoVenta.IdGestionVenta.ToString)
                    'AQUÍ CONFIRMAMOS LAS NOVEDADES
                    Dim confirmarnovedadesventa As New ConfirmarNovedadesTemporales
                    With confirmarnovedadesventa
                        .IdCliente = lblIdClienteOriginal.Text.Trim
                        .IdGestionVenta = infoVenta.IdGestionVenta
                        .Actualizar()
                    End With
                    '//////////////////////////////////////////////////////////////////

                    Session("referidoPor") = nombreCliente
                    If idRol = 3 Then dlgMensajeReferido.Show()
                Else
                    Select Case resultado.Valor
                        Case 300
                            epNotificador.showError(resultado.Mensaje)
                        Case Else
                            epNotificador.showWarning(resultado.Mensaje)
                    End Select
                End If
                ddlPdv.Enabled = True
                ddlAsesorComercial.Enabled = True
                txtTransaccionConsulta.Text = ""
                lblEsNueva.Text = "0"
            Catch ex As Exception
                epNotificador.showError("Error al tratar de registrar venta. ")
            End Try
        Else
            Dim infoVenta As New InfoGestionVenta
            Dim _infoCliente As ClienteFinal
            Dim resultado As ResultadoProceso

            Try
                If Session("InfoPreventa") IsNot Nothing Then
                    _infoCliente = CType(Session("InfoPreventa"), InfoPreventa).InfoCliente
                Else
                    _infoCliente = New ClienteFinal(txtIdentificacion.Text.Trim)
                End If
                With _infoCliente
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
                    .IdEstatusLaboral = CInt(ddlEstatusLaboral.SelectedValue)
                End With
                With infoVenta
                    .OperadorCall = txtAtendidoPor.Text.Trim
                    .IdentificacionOperadorCall = txtNumIdOperadorCallCenter.Text.Trim
                    .IdResultadoProceso = CInt(ddlResultadoConsulta.SelectedValue)
                    .IdUsuarioRegistra = CInt(Session("userId"))
                    If idRol <> 3 Then
                        .IdUsuarioAsesor = CInt(ddlAsesorComercial.SelectedValue)
                        .IdPdv = CInt(ddlPdv.SelectedValue)
                    Else
                        .IdUsuarioAsesor = CInt(Session("userId"))
                    End If
                    .IdTipoVenta = CShort(ddlTipoProducto.SelectedValue)
                    .IdSubproducto = CInt(ddlSubproducto.SelectedValue)
                    .Serial = txtSerialTarjeta.Text.Trim 'meSerialTarjeta.Text.Replace("-", "").Replace(" ", "").Trim
                    .NumeroPlanillaPreAnalisis = CInt(txtNumPlanillaPreAnalisis.Text)
                    .NumeroVentaPlanilla = CInt(txtNumVentaPlanilla.Text)
                    .NumeroPagare = txtNumPagare.Text.Trim
                    .ObservacionCallCenter = txtObservacionOperadorCall.Text.Trim
                    .FechaGestion = dpFechaVenta.SelectedDate
                    If chkNovedad.Checked Then
                        .EsNovedad = True
                        
                    Else
                        .EsNovedad = False

                    End If
                    .IdPreventa = CInt(lblTransaccionExistente.Text.Trim)
                    resultado = .Actualizar(_infoCliente)
                End With
                If resultado.Valor = 0 Then
                    Dim idResultado As String = ddlResultadoConsulta.SelectedValue
                    Dim nombreCliente As String = txtNombreApellido.Text.Trim
                    InicializarCampos()
                    pnlConsulta.Visible = True
                    pnlGestion.Visible = False
                    lblRegistroOk.Text = "<ul><li>El registro fue actualizado satisfactoriamente con. Transacci&oacute;n No. " & lblTransaccionExistente.Text & "</li></ul>"
                    epNotificador.showSuccess("El registro fue actualizado satisfactoriamente con. Transacci&oacute;n No. " & lblTransaccionExistente.Text)
                    Session("referidoPor") = nombreCliente
                    If idRol = 3 Then dlgMensajeReferido.Show()
                Else
                    Select Case resultado.Valor
                        Case 300
                            epNotificador.showError(resultado.Mensaje)
                        Case Else
                            epNotificador.showWarning(resultado.Mensaje)
                    End Select
                End If
                ddlPdv.Enabled = True
                ddlAsesorComercial.Enabled = True
                txtTransaccionConsulta.Text = ""
                lblTransaccionExistente.Text = "0"
                lblExisteTransaccion.Text = ""
                lblTipoConsulta.Text = "0"
                txtTransaccionConsulta.Focus()
            Catch ex As Exception
                epNotificador.showError("Error al tratar de registrar venta. ")
            End Try
        End If
    End Sub

    Private Sub btnSi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSi.Click
        Dim referidoPor As String = CStr(Session("referidoPor"))
        Session.Remove("referidoPor")
        Response.Redirect("RegistroDeReferidos.aspx?referidoPor=" & Server.HtmlEncode(referidoPor), False)
    End Sub

    Private Sub ddlTipoProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoProducto.SelectedIndexChanged
        Try
            Dim idTipoProducto As Integer
            Integer.TryParse(ddlTipoProducto.SelectedValue, idTipoProducto)
            Dim lista As ResultadoProcesoTipoVentaColeccion = CType(Session("listaResultadoProcesoTipoVenta"), ResultadoProcesoTipoVentaColeccion)
            Dim infoTipoProducto As ResultadoProcesoTipoVenta = lista.Item(lista.IndiceDe(idTipoProducto))
            Dim mostrarInfoProducto As Boolean = False

            If infoTipoProducto IsNot Nothing Then mostrarInfoProducto = infoTipoProducto.RequiereEntregarProductoProducto
            LimpiarYControlarVisualizacionDeCamposDeProducto(mostrarInfoProducto)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de validar la selección de tipo de producto. ")
        End Try
    End Sub

    Private Sub ddlResultadoConsulta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlResultadoConsulta.SelectedIndexChanged
        Dim idResultado As Integer
        Try
            Integer.TryParse(ddlResultadoConsulta.SelectedValue, idResultado)
            CargarTiposDeProducto(idResultado)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de validar la selección de resultado de consulta. ")
        End Try
    End Sub

    Private Sub ddlProductoPadre_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProductoPadre.SelectedIndexChanged
        Try
            Dim idProductoPadre As Integer
            Integer.TryParse(ddlProductoPadre.SelectedValue, idProductoPadre)
            CargarListadoDeSubproductos(idProductoPadre)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de validar la selección de tipo de tarjeta. ")
        End Try
    End Sub

    Protected Sub ddlPdv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPdv.SelectedIndexChanged
        CargarAsesoresComerciales()
    End Sub

    Private Sub cpCiudad_Execute(sender As Object, e As EO.Web.CallbackEventArgs) Handles cpCiudad.Execute
        CargarCiudades(e.Parameter)
    End Sub

#End Region

#Region "Métodos Privados"


    Private Sub InicializarCampos()
        For Each ctrl As Control In pnlDatosPersonales.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            ElseIf TypeOf ctrl Is DropDownList Then
                CType(ctrl, DropDownList).ClearSelection()
            End If
        Next
        For Each ctrl As Control In pnlInfoVenta.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            ElseIf TypeOf ctrl Is DropDownList Then
                CType(ctrl, DropDownList).ClearSelection()
            End If
        Next

        ddlPdv.ClearSelection()
        CargarAsesoresComerciales()
        txtSerialTarjeta.Text = ""
        Session.Remove("InfoCliente")
        Session.Remove("InfoPreventa")
        ActivarOInhabilitarTipoDeProducto(False)

        trInfoProducto.Visible = False
        trInfoSerial.Visible = False
        trInfoNovedad.Visible = False
        trAccionesNovedad.Visible = False
        trDetalleNovedad.Visible = False
    End Sub

    Private Sub CargarCiudades(Optional ByVal filtro As String = "")
        Try
            If Not String.IsNullOrEmpty(filtro) Then
                Dim dvCiudad As DataView = ObtenerListaCiudad().DefaultView
                dvCiudad.RowFilter = "ciudadDepartamento like '" & filtro & "%'"
                With ddlCiudadResidencia
                    .DataSource = dvCiudad
                    .DataTextField = "ciudadDepartamento"
                    .DataValueField = "idCiudad"
                    .DataBind()
                End With
            Else
                ddlCiudadResidencia.Items.Clear()
            End If
            lblNumCiudades.Text = ddlCiudadResidencia.Items.Count.ToString & " registro(s) cargado(s)"
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de ciudades. ")
        Finally
            If ddlCiudadResidencia.Items.Count <> 1 Then ddlCiudadResidencia.Items.Insert(0, New ListItem("Seleccione una Ciudad", "0"))
        End Try
    End Sub

    Private Sub CargarListaEstatusGeneral()
        Dim listaEstatusLaboral As New EstatusLaboralColeccion
        Try
            listaEstatusLaboral.CargarDatos()
            With ddlEstatusLaboral
                .DataSource = listaEstatusLaboral
                .DataTextField = "descripcion"
                .DataValueField = "idEstatusLaboral"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Estatus Laboral. ")
        Finally
            ddlEstatusLaboral.Items.Insert(0, New ListItem("Seleccione un Estatus", "0"))
        End Try
    End Sub

    Private Sub CargarListaResultadoProceso()
        Try
            Dim lista As New ResultadoProcesoVentaColeccion
            lista.CargarDatos()
            With ddlResultadoConsulta
                .DataSource = lista
                .DataTextField = "Descripcion"
                .DataValueField = "IdResultado"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Resultados de Consulta: ")
        Finally
            ddlResultadoConsulta.Items.Insert(0, New ListItem("Seleccione un Resultado", "0"))
        End Try
    End Sub

    Private Sub CargarTiposDeProducto(Optional ByVal idResultadoProceso As Byte = 0)
        Try
            Dim _listaTipoVenta As ResultadoProcesoTipoVentaColeccion = ObtenerListaResultadoProcesoTiposDeVenta()
            ActivarOInhabilitarTipoDeProducto(False)
            If idResultadoProceso > 0 Then
                Dim dvTipoVenta As DataView = _listaTipoVenta.GenerarDataTable.DefaultView
                dvTipoVenta.RowFilter = "idResultadoProceso=" & idResultadoProceso.ToString
                With ddlTipoProducto
                    .DataSource = dvTipoVenta
                    .DataTextField = "TipoVenta"
                    .DataValueField = "IdTipoVenta"
                    .DataBind()
                End With
                If dvTipoVenta.Count > 0 Then ActivarOInhabilitarTipoDeProducto(True)
            Else
                ddlTipoProducto.Items.Clear()
            End If
            LimpiarYControlarVisualizacionDeCamposDeProducto(False)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Tipos de Producto: ")
        Finally
            ddlTipoProducto.Items.Insert(0, New ListItem("Seleccione un Tipo de Producto", "0"))
        End Try
    End Sub

    Private Function ObtenerListaResultadoProcesoTiposDeVenta() As ResultadoProcesoTipoVentaColeccion
        Dim lista As ResultadoProcesoTipoVentaColeccion

        If Session("listaResultadoProcesoTipoVenta") Is Nothing Then
            lista = New ResultadoProcesoTipoVentaColeccion
            lista.CargarDatos()
            Session("listaResultadoProcesoTipoVenta") = lista
        Else
            lista = CType(Session("listaResultadoProcesoTipoVenta"), ResultadoProcesoTipoVentaColeccion)
        End If
        Return lista
    End Function

    Private Function ObtenerListaSubproductos() As SubproductoColeccion
        Dim lista As SubproductoColeccion

        If Session("listaSubproductos") Is Nothing Then
            lista = New SubproductoColeccion
            lista.Comercializable = Enumerados.EstadoBinario.Activo
            lista.CargarDatos()
            Session("listaSubproductos") = lista
        Else
            lista = CType(Session("listaSubproductos"), SubproductoColeccion)
        End If
        Return lista
    End Function

    Private Sub CargarListadoDeProductosPadre()
        Try
            Dim lista As New ProductoColeccion
            With lista
                .Comercializable = Enumerados.EstadoBinario.Activo
                .CargarDatos()
            End With

            With ddlProductoPadre
                .DataSource = lista
                .DataTextField = "NombreProducto"
                .DataValueField = "IdProducto"
                .DataBind()
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el listado de Tipos de Tarjeta: " & vbCrLf)
        Finally
            ddlProductoPadre.Items.Insert(0, New ListItem("Seleccione un Tipo", "0"))
        End Try
    End Sub

    Private Sub CargarListadoDeSubproductos(Optional ByVal idProductoPadre As Integer = 0)
        Try
            Dim lista As SubproductoColeccion = ObtenerListaSubproductos()
            If idProductoPadre > 0 Then
                Dim dvSubproducto As DataView = lista.GenerarDataTable.DefaultView
                dvSubproducto.RowFilter = "IdProducto=" & idProductoPadre.ToString
                With ddlSubproducto
                    .DataSource = dvSubproducto
                    .DataTextField = "NombreSubproducto"
                    .DataValueField = "IdSubproducto"
                    .DataBind()
                End With
            Else
                ddlSubproducto.Items.Clear()
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Subproductos (Cupos): ")
        Finally
            ddlSubproducto.Items.Insert(0, New ListItem("Seleccione un Cupo", "0"))
        End Try
    End Sub

    Private Sub CambiarVistaTabScript(ByVal proceso As String)
        Select Case proceso
            Case "PREVENTA"
                pnlInfoVenta.Visible = False
                trInfoPreventa.Visible = True
                tsInfoGestion.Items(0).Disabled = False
                tsInfoGestion.Items(1).Disabled = True
                tsInfoGestion.Items(2).Disabled = True
                tsInfoGestion.SelectedIndex = 0
            Case "VENTA"
                pnlInfoVenta.Visible = True
                trInfoPreventa.Visible = False
                tsInfoGestion.Items(0).Disabled = True
                tsInfoGestion.Items(1).Disabled = False
                tsInfoGestion.Items(2).Disabled = True
                tsInfoGestion.SelectedIndex = 1
            Case "HISTORICO"
                pnlInfoVenta.Visible = False
                pnlHistoricoVenta.Visible = True
                trInfoPreventa.Visible = False
                tsInfoGestion.Items(0).Disabled = True
                tsInfoGestion.Items(1).Disabled = True
                tsInfoGestion.Items(2).Disabled = False
                tsInfoGestion.SelectedIndex = 2
        End Select
    End Sub

    Private Sub ActivarOInhabilitarTipoDeProducto(ByVal activar As Boolean)
        ddlTipoProducto.Enabled = activar
        rfvTipoProducto.Enabled = activar
    End Sub

    Private Sub AsignarAtributosDeControlDeEnter()
        txtIdentificacionConsulta.Attributes.Add("onkeydown", "ProcesarEnterGeneral(this,'" + lbConsultar.ClientID + "')")

        For Each ctrl As Control In pnlDatosPersonales.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Attributes.Add("onkeydown", "ProcesarEnterGeneral(this,'" + lblRegistrarPreventa.ClientID + "')")
            End If
        Next

        For Each ctrl As Control In pnlInfoVenta.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Attributes.Add("onkeydown", "ProcesarEnterGeneral(this,'" + lbRegistrarVenta.ClientID + "')")
            End If
        Next
    End Sub

    Private Sub CargarPuntosDeVenta()
        Try
            Dim listaPdv As New PuntoDeVentaColeccion

            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                If chkPdvActivo.Checked Then .IdEstado = 1
                .CargarDatos()
            End With
            With ddlPdv
                .DataSource = listaPdv
                .DataTextField = "NombrePdv"
                .DataValueField = "IdPdv"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Puntos de Venta. ")
        Finally
            ddlPdv.Items.Insert(0, New ListItem("Seleccione un PDV", "0"))
        End Try
    End Sub

    Private Sub CargarAsesoresComerciales(Optional ByVal usuario As Integer = False)
        Try
            If String.IsNullOrEmpty(ddlPdv.SelectedValue) OrElse ddlPdv.SelectedValue = "0" Then
                ddlAsesorComercial.Items.Clear()
            Else
                Dim listaAsesor As New AsesorComercialColeccion
                With listaAsesor
                    .IdPdv = CInt(ddlPdv.SelectedValue)
                    .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                    .Usuario = CInt(usuario)
                    If chkPdvActivo.Checked Then .IdEstado = 1
                    .CargarDatos()
                End With
                With ddlAsesorComercial
                    .DataSource = listaAsesor
                    .DataTextField = "NombreApellido"
                    .DataValueField = "IdUsuarioSistema"
                    .DataBind()
                End With
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Asesores Comerciales. ")
        Finally
            ddlAsesorComercial.Items.Insert(0, New ListItem("Seleccione un Asesor", "0"))
        End Try
    End Sub

    Private Sub InicializarControlesOrigenRegistro()
        Dim activarInfoOrigenGestion As Boolean = False
        If idRol <> 3 Then
            CargarPuntosDeVenta()
            CargarAsesoresComerciales()
            activarInfoOrigenGestion = True
        End If
        pnlInfoOrigenGestion.Visible = activarInfoOrigenGestion
        ddlPdv.Enabled = activarInfoOrigenGestion
        ddlAsesorComercial.Enabled = activarInfoOrigenGestion
        With rfvAsesorComercial
            .ValidationGroup = "datosPersonales"
            .Enabled = activarInfoOrigenGestion
        End With
        With rfvPdv
            .ValidationGroup = "datosPersonales"
            .Enabled = activarInfoOrigenGestion
        End With
    End Sub

    Private Function ObtenerListaCiudad() As DataTable
        Dim dtCiudad As DataTable
        If Session("listaCiudad") Is Nothing Then
            Dim infoCiudad As New Localizacion.CiudadColeccion
            infoCiudad.CargarDatos()
            dtCiudad = infoCiudad.GenerarDataTable()
        Else
            dtCiudad = CType(Session("listaCiudad"), DataTable)
        End If
        Return dtCiudad
    End Function

    Private Sub CargarTipoDeNovedad()
        Try
            Dim lista As New TipoDeNovedadColeccion
            lista.CargarDatos()
            With ddlTipoNovedad
                .DataSource = lista
                .DataTextField = "descripcion"
                .DataValueField = "idTipoNovedad"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Tipos de Novedades: ")
        Finally
            ddlResultadoConsulta.Items.Insert(0, New ListItem("Seleccione un Resultado", "0"))
        End Try
    End Sub

#End Region




    Protected Sub lbCancelarHistorico_Click(sender As Object, e As EventArgs) Handles lbCancelarHistorico.Click
        Try
            InicializarCampos()
            For Each ctrl As Control In pnlConsulta.Controls
                If TypeOf ctrl Is TextBox Then
                    CType(ctrl, TextBox).Text = ""
                ElseIf TypeOf ctrl Is DropDownList Then
                    CType(ctrl, DropDownList).ClearSelection()
                End If
            Next
            pnlConsulta.Visible = True
            pnlGestion.Visible = False
            If Session("InfoPreventa") IsNot Nothing Then
                Dim resultado As ResultadoProceso = CType(Session("InfoPreventa"), InfoPreventa).Eliminar(CInt(Session("userId")))
                If resultado.Valor <> 0 Then
                    Select Case resultado.Valor
                        Case 300
                            epNotificador.showError(resultado.Mensaje)
                        Case Else
                            epNotificador.showWarning(resultado.Mensaje)
                    End Select
                End If
            End If
            ddlPdv.Enabled = True
            ddlAsesorComercial.Enabled = True
            txtTransaccionConsulta.Text = ""
            lblTransaccionExistente.Text = "0"
            lblExisteTransaccion.Text = ""
            lblTipoConsulta.Text = "0"
            lblEsNueva.Text = "0"
            lblIdClienteOriginal.Text = "0"
            lblTieneVentas.Text = "0"
            lblIdentificacionAnterior.Text = "0"
            txtTransaccionConsulta.Focus()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cancelar registro de venta. ")
        End Try
    End Sub

    Private Sub gvHistoricoVenta_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvHistoricoVenta.RowCommand
        If e.CommandName = "Select" Then
            lblRegistrarPreventa.Visible = False
            lbCancelar.Visible = False
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvHistoricoVenta.Rows(index)
            txtTransaccionConsulta.Text = row.Cells(1).Text
            InicializarCampos()
            lblTipoConsulta.Text = "1"
            'aquí consultamos la transacción para obtener el número de cédula
            Dim infoClienteTransaccion As New TransaccionCliente(txtTransaccionConsulta.Text.Trim)
            With infoClienteTransaccion
                txtIdentificacionConsulta.Text = .NumeroIdentificacion
            End With
            If txtIdentificacionConsulta.Text.Trim = txtTransaccionConsulta.Text.Trim Then
                lblExisteTransaccion.Text = "0"
            Else
                lblExisteTransaccion.Text = "1"
                lblTransaccionExistente.Text = txtTransaccionConsulta.Text.Trim
            End If
            'If txtTransaccionConsulta.Text.Trim = "1" Then
            '    txtIdentificacionConsulta.Text = "10171711288"
            'Else
            '    txtIdentificacionConsulta.Text = "0"
            'End If

            Dim infoCliente As New ClienteFinal(txtIdentificacionConsulta.Text.Trim)

            With infoCliente
                If lblExisteTransaccion.Text = "1" Then
                    txtIdentificacion.Text = txtIdentificacionConsulta.Text.Trim
                Else
                    txtIdentificacion.Text = ""
                End If

                lblIdClienteOriginal.Text = .IdCliente
                
                With ddlTipoIdentificacion
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdTipoIdentificacion))
                End With
                txtNombreApellido.Text = .NombreApellido
                ddlCiudadResidencia.SelectedIndex = ddlCiudadResidencia.Items.IndexOf(ddlCiudadResidencia.Items.FindByValue(infoCliente.IdCiudadResidencia))
                txtDireccionResidencia.Text = .DireccionResidencia
                txtBarrioResidencia.Text = .BarrioResidencia
                txtTelefonoResidencia.Text = .TelefonoResidencia
                txtCelular.Text = .Celular
                txtDireccionOficina.Text = .DireccionOficina
                txtTelefonoOficina.Text = .TelefonoOficina
                txtEmail.Text = .Email
                If .IngresoAproximado > 0 Then txtIngreso.Text = CInt(.IngresoAproximado).ToString
                With ddlEstatusLaboral
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdEstatusLaboral))
                End With
            End With
            If infoCliente.Registrado Then Session("InfoCliente") = infoCliente
            txtIdentificacionConsulta.Text = ""
            pnlConsulta.Visible = False
            pnlGestion.Visible = True
            pnlDatosPersonales.Visible = True
            pnlInfoOrigenGestion.Visible = True
            pnlRegistroPreventa.Visible = True
            pnlHistoricoVenta.Visible = False
            CambiarVistaTabScript("PREVENTA")
            '/////////////codigo nuevo para mostrar los datos de la venta
            Dim informacionVenta = New InfoVenta(txtTransaccionConsulta.Text.Trim)
            With informacionVenta
                ddlPdv.Enabled = True
                ddlPdv.SelectedIndex = ddlPdv.Items.IndexOf(ddlPdv.Items.FindByValue(.IdPdv))
                CargarAsesoresComerciales(.IdUsuarioAsesor)
                ddlAsesorComercial.Enabled = True
                ddlAsesorComercial.SelectedIndex = ddlAsesorComercial.Items.IndexOf(ddlAsesorComercial.Items.FindByValue(.UsuarioAsesor))
                dpFechaVenta.SelectedDate = .FechaGestion
                txtAtendidoPor.Text = .OperadorCall
                txtNumIdOperadorCallCenter.Text = .IdentificacionOperadorCall
                txtNumPlanillaPreAnalisis.Text = .NumPlanillaPreAnalisis
                txtNumVentaPlanilla.Text = .NumVentaPlanilla
                ddlResultadoConsulta.SelectedIndex = ddlResultadoConsulta.Items.IndexOf(ddlResultadoConsulta.Items.FindByValue(.IdResultadoProceso))
                If .IdResultadoProceso = 1 Then
                    ddlTipoProducto.Enabled = True
                    CargarTiposDeProducto(.IdResultadoProceso)
                    ddlTipoProducto.SelectedIndex = ddlTipoProducto.Items.IndexOf(ddlTipoProducto.Items.FindByValue(.IdTipoVenta))
                    If .IdTipoVenta = 1 Then
                        trInfoProducto.Visible = True
                        trInfoSerial.Visible = True
                        ddlProductoPadre.Enabled = True
                        CargarListadoDeProductosPadre()
                        ddlProductoPadre.SelectedIndex = ddlProductoPadre.Items.IndexOf(ddlProductoPadre.Items.FindByValue(.IdProducto))
                        ddlSubproducto.Enabled = True
                        CargarListadoDeSubproductos(.IdProducto)
                        ddlSubproducto.SelectedIndex = ddlSubproducto.Items.IndexOf(ddlSubproducto.Items.FindByValue(.IdSubProducto))
                        txtNumPagare.Text = .NumPagare
                        txtSerialTarjeta.Text = .Serial
                    End If
                End If
                txtObservacionOperadorCall.Text = .ObservacionCallCenter
                If .EsNovedad = True Then
                    trInfoNovedad.Visible = True
                    trAccionesNovedad.Visible = True
                    trDetalleNovedad.Visible = True
                    ddlTipoNovedad.Enabled = True
                    chkNovedad.Checked = True
                    CargarTipoDeNovedad()
                    ObtenerDatosNovedades(txtTransaccionConsulta.Text.Trim)
                    'ddlTipoNovedad.SelectedValue = .TipoNovedad
                    'txtObservacionesNovedad.Text = .ObservacionNovedad
                Else
                    gvNovedades.DataBind()
                    chkNovedad.Checked = False
                    trInfoNovedad.Visible = False
                    trAccionesNovedad.Visible = False
                    trDetalleNovedad.Visible = False
                    ddlTipoNovedad.Enabled = False
                End If
            End With
            lblEsNueva.Text = "0"
            txtAtendidoPor.Enabled = True
            ddlResultadoConsulta.Enabled = False
            ddlTipoProducto.Enabled = False
            ddlProductoPadre.Enabled = False
            txtSerialTarjeta.Enabled = False
            txtIdentificacion.Enabled = True
            pnlInfoVenta.Visible = True
            ddlPdv.Enabled = False
            ddlAsesorComercial.Enabled = False
            '//////////////////////////////////

        End If
    End Sub

    Private Sub gvHistoricoVenta_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHistoricoVenta.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor = '#A32035';")
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = 'white';")
            'e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvHistoricoVenta, "Select$" + e.Row.RowIndex.ToString)

        End If

    End Sub

    Protected Sub lbNuevaVenta_Click(sender As Object, e As EventArgs) Handles lbNuevaVenta.Click
        lblIdClienteOriginal.Text = "0"
        lblEsNueva.Text = "1"
        lblExisteTransaccion.Text = "0"
        Dim numIdentificacion As String = txtIdentificacion.Text.Trim

        InicializarCampos()
        'InicializarControlesOrigenRegistro()e
        ddlPdv.Enabled = True
        ddlAsesorComercial.Enabled = True
        lblTipoConsulta.Text = "1"
        Dim infoCliente As New ClienteFinal(numIdentificacion)

        With infoCliente
            lblIdClienteOriginal.Text = .IdCliente
            txtIdentificacion.Text = numIdentificacion
            With ddlTipoIdentificacion
                .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdTipoIdentificacion))
            End With
            txtNombreApellido.Text = .NombreApellido
            ddlCiudadResidencia.SelectedIndex = ddlCiudadResidencia.Items.IndexOf(ddlCiudadResidencia.Items.FindByValue(infoCliente.IdCiudadResidencia))
            txtDireccionResidencia.Text = .DireccionResidencia
            txtBarrioResidencia.Text = .BarrioResidencia
            txtTelefonoResidencia.Text = .TelefonoResidencia
            txtCelular.Text = .Celular
            txtDireccionOficina.Text = .DireccionOficina
            txtTelefonoOficina.Text = .TelefonoOficina
            txtEmail.Text = .Email
            If .IngresoAproximado > 0 Then txtIngreso.Text = CInt(.IngresoAproximado).ToString
            With ddlEstatusLaboral
                .SelectedIndex = .Items.IndexOf(.Items.FindByValue(infoCliente.IdEstatusLaboral))
            End With
        End With
        If infoCliente.Registrado Then Session("InfoCliente") = infoCliente
        txtIdentificacionConsulta.Text = ""
        pnlConsulta.Visible = False
        pnlGestion.Visible = True
        pnlHistoricoVenta.Visible = False
        pnlInfoOrigenGestion.Visible = True
        pnlDatosPersonales.Visible = True
        CambiarVistaTabScript("PREVENTA")
        txtIdentificacion.Enabled = True
        chkNovedad.Checked = False
        chkNovedad.Enabled = True
        pnlInfoVenta.Visible = True
    End Sub

    Protected Sub chkNovedad_CheckedChanged(sender As Object, e As EventArgs) Handles chkNovedad.CheckedChanged
        If chkNovedad.Checked Then
            ddlTipoNovedad.Enabled = True
            trInfoNovedad.Visible = True
            trAccionesNovedad.Visible = True
            trDetalleNovedad.Visible = True
            CargarTipoDeNovedad()
        Else
            ddlTipoNovedad.Enabled = False
            trInfoNovedad.Visible = False
            trAccionesNovedad.Visible = False
            trDetalleNovedad.Visible = False
            txtObservacionesNovedad.Text = ""
        End If
    End Sub

    Private Sub chkNovedad_DataBinding(sender As Object, e As System.EventArgs) Handles chkNovedad.DataBinding
        If chkNovedad.Checked Then
            ddlTipoNovedad.Enabled = True
            trInfoNovedad.Visible = True
            trAccionesNovedad.Visible = True
            trDetalleNovedad.Visible = True
        Else
            ddlTipoNovedad.Enabled = False
            trInfoNovedad.Visible = False
            trAccionesNovedad.Visible = False
            trDetalleNovedad.Visible = False
        End If
    End Sub

    Private Sub gvNovedades_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvNovedades.RowCommand
        If e.CommandName = "Select" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvNovedades.Rows(index)
            Dim eliminarNovedad As New EliminarNovedad(row.Cells(1).Text)
            ObtenerDatosNovedades(txtTransaccionConsulta.Text.Trim)
        End If
    End Sub

    Private Sub lbRegistrarNovedad_Click(sender As Object, e As System.EventArgs) Handles lbRegistrarNovedad.Click
        Try
            If chkNovedad.Checked Then
                If Len(txtObservacionesNovedad.Text.Trim) < 25 Then
                    epNotificador.showWarning("Las observaciones de la novedad deben contener al menos 25 caracteres.")
                    txtObservacionesNovedad.Focus()
                    Exit Sub
                End If
            End If
            If lblEsNueva.Text = "0" Then
                Dim agregarNovedad As New InsertarTemporalNovedades()
                With agregarNovedad
                    .IdGestionVenta = txtTransaccionConsulta.Text.Trim
                    .ObservacionNovedad = txtObservacionesNovedad.Text
                    .IdTipoNovedad = ddlTipoNovedad.SelectedValue
                    .Registrar(txtTransaccionConsulta.Text.Trim)
                End With
                ddlTipoNovedad.SelectedIndex = 0
                txtObservacionesNovedad.Text = ""
                ddlTipoNovedad.Focus()
                gvNovedades.DataBind()
                ObtenerDatosNovedades(txtTransaccionConsulta.Text.Trim)
                epNotificador.showSuccess("Novedad registrada satisfactoriamente.")
            Else
                Dim agregarNovedad As New InsertarNovedadesVentaNueva()
                With agregarNovedad
                    .IdCliente = lblIdClienteOriginal.Text.Trim
                    .IdTipoNovedad = ddlTipoNovedad.SelectedValue
                    .ObservacionNovedad = txtObservacionesNovedad.Text
                    .Registrar(lblIdClienteOriginal.Text.Trim)
                End With
                ddlTipoNovedad.SelectedIndex = 0
                txtObservacionesNovedad.Text = ""
                ddlTipoNovedad.Focus()
                gvNovedades.DataBind()
                ObtenerDatosNovedadesTemporales(lblIdClienteOriginal.Text.Trim)
                epNotificador.showSuccess("Novedad registrada satisfactoriamente.")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al adicionar una novedad a la venta")
        End Try
    End Sub
End Class