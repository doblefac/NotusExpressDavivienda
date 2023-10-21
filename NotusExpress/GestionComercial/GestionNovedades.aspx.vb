Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.Reportes
Imports DevExpress.Web

Public Class GestionNovedades
    Inherits System.Web.UI.Page

#Region "Atributos Privados"

    Dim idRol As Integer

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Session("idRol") IsNot Nothing Then Integer.TryParse(Session("idRol").ToString, idRol)
        If Not Me.IsPostBack Then
            With epNotificador
                .setTitle("Gestión de Novedades")
            End With
            Session.Remove("htFiltros")
            Session.Remove("dtReporteVentas")
            CargarListadoPdv()
            CargarListadoDeAsesores(True)
            CargarListadoResultadoProceso()
            CargarListadoTiposDeVenta()
            ObtenerDatosNovedades()
            pnlInfoOrigenGestion.Visible = False
        End If
    End Sub

    Public Sub GuardarFiltros()
        Dim ht As New Hashtable
        With ht
            .Add("pdv", cboPdv.Value)
            .Add("asesor", cboAsesor.Value)
            .Add("fechaInicial", deFechaInicio.Date)
            .Add("fechaFinal", deFechaFin.Date)
        End With
        Session("htFiltros") = ht
    End Sub

    Private Sub CargarListadoPdv()
        Dim listaPdv As New PuntoDeVentaColeccion
        Try
            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                If cbPdvActivo.Checked Then .IdEstado = 1
                .CargarDatos()
            End With
            With cboPdv
                .DataSource = listaPdv
                .TextField = "NombrePdv"
                .ValueField = "IdPdv"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Puntos de Venta. ")
        End Try
        With cboPdv
            .Items.Insert(0, New ListEditItem("Seleccione un Punto de Venta", 0))
            .SelectedIndex = 0
        End With
    End Sub

    Private Sub CargarListadoDeAsesores(Optional ByVal forzarConsulta As Boolean = False)
        cboAsesor.Items.Clear()
        Try
            Dim listaAsesor As AsesorComercialColeccion
            If Session("listaAsesores") Is Nothing OrElse forzarConsulta Then
                listaAsesor = New AsesorComercialColeccion
                With listaAsesor
                    .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                    If cboPdv.Value IsNot Nothing AndAlso CInt(cboPdv.Value) > 0 Then .IdPdv = CInt(cboPdv.Value)
                    If cbAsesorActivo.Checked Then .IdEstado = 1
                    .CargarDatos()
                End With
                Session("listaAsesores") = listaAsesor
            Else
                listaAsesor = CType(Session("listaAsesores"), AsesorComercialColeccion)
            End If
            With cboAsesor
                .DataSource = listaAsesor
                .TextField = "NombreApellido"
                .ValueField = "IdUsuarioSistema"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Asesores. ")
        End Try
        With cboAsesor
            .Items.Insert(0, New ListEditItem("Seleccione un Asesor", 0))
            If .SelectedIndex = -1 Then .SelectedIndex = 0
        End With
    End Sub

    Private Sub CargarListadoResultadoProceso()
        Dim listaResultado As New ResultadoProcesoVentaColeccion
        Try
            With listaResultado
                .CargarDatos()
            End With
            With cboResultadoGestion
                .DataSource = listaResultado
                .TextField = "Descripcion"
                .ValueField = "IdResultado"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Resultados de Proceso. ")
        End Try
        With cboResultadoGestion
            .Items.Insert(0, New ListEditItem("Seleccione un Item", 0))
            .SelectedIndex = 0
        End With
    End Sub

    Private Sub CargarAsesoresComerciales()
        Try
            If String.IsNullOrEmpty(ddlPdv.SelectedValue) OrElse ddlPdv.SelectedValue = "0" Then
                ddlAsesorComercial.Items.Clear()
            Else
                Dim listaAsesor As New AsesorComercialColeccion
                With listaAsesor
                    .IdPdv = CInt(ddlPdv.SelectedValue)
                    .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
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

    Private Sub CargarListadoTiposDeVenta()
        Dim listaTipoVenta As New ResultadoProcesoTipoVentaColeccion
        Try
            With listaTipoVenta
                .CargarDatos()
            End With
            With cboTipoProducto
                .DataSource = listaTipoVenta
                .TextField = "TipoVenta"
                .ValueField = "IdTipoVenta"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Tipos de Venta. ")
        End Try
        With cboTipoProducto
            .Items.Insert(0, New ListEditItem("Seleccione un Item", 0))
            .SelectedIndex = 0
        End With
    End Sub

    Public Sub ObtenerDatosNovedades()
        Dim dtDatos As New DataTable

        Dim infoHistoria As New VentasConNovedadGeneral
        With infoHistoria
            dtDatos = .DatosNovedades
        End With

        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            EnlazarDatos(dtDatos)
        Else
            epNotificador.showWarning("No hay ventas registradas con novedades.")
        End If
    End Sub

    Private Sub EnlazarDatos(ByVal dtDatos As DataTable)
        With gvVentasConNovedades
            .DataBind()
            .DataSource = dtDatos
            .DataBind()
        End With

    End Sub

    Protected Sub cboAsesor_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cboAsesor.Callback
        If e.Parameter = "cargarLista" Then
            CargarListadoDeAsesores(True)
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End If
    End Sub

    Private Sub cpResultadoReporte_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpResultadoReporte.Callback
        Select Case e.Parameter
            Case "obtenerReporte"
                ObtenerDatosReporte(True)
            Case "limpiarFiltros"
                LimpiarFiltros()
        End Select
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Public Sub ObtenerDatosReporte(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable
        Dim infoReferido As New FiltroVentasConNovedades
        With infoReferido
            If cboPdv.Value IsNot Nothing Then Integer.TryParse(cboPdv.Value.ToString, .IdPuntoDeVenta)
            If cboAsesor.Value IsNot Nothing Then Integer.TryParse(cboAsesor.Value.ToString, .IdAsesorComercial)
            If cboResultadoGestion.Value IsNot Nothing Then Integer.TryParse(cboResultadoGestion.Value.ToString, .IdResultadoProceso)
            If cboTipoProducto.Value IsNot Nothing Then Integer.TryParse(cboTipoProducto.Value.ToString, .IdTipoVenta)
            If deFechaInicio.Date > Date.MinValue Then .FechaInicial = deFechaInicio.Date
            If deFechaFin.Date > Date.MinValue Then .FechaFinal = deFechaFin.Date

            dtDatos = .DatosReporte
        End With
        
        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            gvVentasConNovedades.DataBind()
            EnlazarDatos(dtDatos)
        Else
            epNotificador.showWarning("No se encontraron datos según los filtros aplicados.")
        End If
    End Sub

    Private Sub LimpiarFiltros()
        Try
            Session.Remove("dtReporteGeneralVentas")
            If cboPdv.Value IsNot Nothing AndAlso CInt(cboPdv.Value) <> 0 Then
                cboPdv.SelectedIndex = 0
                CargarListadoDeAsesores()
            Else
                If cboAsesor.Value IsNot Nothing AndAlso CInt(cboAsesor.Value) <> 0 Then cboAsesor.SelectedIndex = 0
            End If
            deFechaInicio.Date = Nothing
            deFechaFin.Date = Nothing
            'gvHistoricoVenta.DataBind()

        Catch ex As Exception
            epNotificador.showError("Error al tratar de limpiar filtros. ")
        End Try

    End Sub

    Private Sub cboPdv_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cboPdv.Callback
        If e.Parameter = "cargarLista" Then
            CargarListadoPdv()
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End If
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

    Private Sub gvVentasConNovedades_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvVentasConNovedades.PageIndexChanging
        gvVentasConNovedades.PageIndex = e.NewPageIndex
        ObtenerDatosNovedades()
    End Sub

    Private Sub gvVentasConNovedades_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVentasConNovedades.RowCommand
        If e.CommandName = "Select" Then
            pnlConsulta.Visible = False
            pnlVentasConNovedad.Visible = False
            pnlGestion.Visible = True
            pnlInfoOrigenGestion.Visible = True
            CargarListaResultadoProceso()
            CargarTiposDeProducto()
            CargarListadoDeProductosPadre()
            CargarListadoDeSubproductos()

            'agregamos la información de la venta a los controles de VENTA
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvVentasConNovedades.Rows(index)
            lblTransaccionExistente.Text = row.Cells(1).Text

            Dim infoClienteTransaccion As New TransaccionCliente(lblTransaccionExistente.Text.Trim)
            CargarPuntosDeVenta()
            Dim infoCliente As New ClienteFinal(infoClienteTransaccion.NumeroIdentificacion)
            ddlPdv.SelectedIndex = ddlPdv.Items.IndexOf(ddlPdv.Items.FindByValue(infoCliente.IdPdv))
            CargarAsesoresComerciales()
            ddlAsesorComercial.Enabled = True
            ddlAsesorComercial.SelectedIndex = ddlAsesorComercial.Items.IndexOf(ddlAsesorComercial.Items.FindByValue(infoCliente.IdUsuarioAsesor))


            Dim informacionVenta = New InfoVentaDeclinada(row.Cells(1).Text)
            With informacionVenta
                
                dpFechaVenta.SelectedDate = .FechaGestion
                txtAtendidoPor.Text = .OperadorCall
                txtNumIdOperadorCallCenter.Text = .IdentificacionOperadorCall
                txtNumPlanillaPreAnalisis.Text = .NumPlanillaPreAnalisis
                txtNumVentaPlanilla.Text = .NumVentaPlanilla
                ddlResultadoConsulta.SelectedValue = .IdResultadoProceso
                If .IdEstado = 10 Then
                    chkDeclinarVenta.Checked = True
                    trDeclinarVenta.Visible = True
                    txtObservacionesVentaDeclinada.Text = .ObservacionDeclinar
                Else
                    chkDeclinarVenta.Checked = False
                    trDeclinarVenta.Visible = False
                End If
                If .IdResultadoProceso = 1 Then
                    ddlTipoProducto.Enabled = True
                    CargarTiposDeProducto(.IdResultadoProceso)
                    ddlTipoProducto.SelectedValue = .IdTipoVenta
                    If .IdTipoVenta = 1 Then
                        trInfoProducto.Visible = True
                        trInfoSerial.Visible = True
                        ddlProductoPadre.Enabled = True
                        CargarListadoDeProductosPadre()
                        ddlProductoPadre.SelectedValue = .IdProducto
                        ddlSubproducto.Enabled = True
                        CargarListadoDeSubproductos(.IdProducto)
                        ddlSubproducto.SelectedValue = .IdSubProducto
                        txtNumPagare.Text = .NumPagare
                        txtSerialTarjeta.Text = .Serial
                    End If
                Else
                    ddlTipoProducto.Enabled = False
                    chkDeclinarVenta.Enabled = False
                End If
                txtObservacionOperadorCall.Text = .ObservacionCallCenter

            End With
            txtAtendidoPor.Enabled = True
            ddlResultadoConsulta.Enabled = True

        End If
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

    Private Sub LimpiarYControlarVisualizacionDeCamposDeProducto(ByVal visible As Boolean)
        ddlProductoPadre.ClearSelection()
        ddlSubproducto.ClearSelection()
        txtNumPagare.Text = ""
        txtSerialTarjeta.Text = ""
        trInfoProducto.Visible = visible
        trInfoSerial.Visible = visible
    End Sub

    Private Sub ActivarOInhabilitarTipoDeProducto(ByVal activar As Boolean)
        ddlTipoProducto.Enabled = activar
        rfvTipoProducto.Enabled = activar
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

    Protected Sub lbCancelarVenta_Click(sender As Object, e As EventArgs) Handles lbCancelarVenta.Click
        pnlGestion.Visible = False
        pnlConsulta.Visible = True
        pnlVentasConNovedad.Visible = True
        pnlInfoOrigenGestion.Visible = False
        LimpiarFiltros()
        txtObservacionesVentaDeclinada.Text = ""
    End Sub

    Protected Sub ddlResultadoConsulta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlResultadoConsulta.SelectedIndexChanged
        Dim idResultado As Integer
        Try
            Integer.TryParse(ddlResultadoConsulta.SelectedValue, idResultado)
            CargarTiposDeProducto(idResultado)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de validar la selección de resultado de consulta. ")
        End Try
    End Sub

    Protected Sub ddlTipoProducto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoProducto.SelectedIndexChanged
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

    Protected Sub ddlProductoPadre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProductoPadre.SelectedIndexChanged
        Try
            Dim idProductoPadre As Integer
            Integer.TryParse(ddlProductoPadre.SelectedValue, idProductoPadre)
            CargarListadoDeSubproductos(idProductoPadre)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de validar la selección de tipo de tarjeta. ")
        End Try
    End Sub

    Protected Sub lbRegistrarVenta_Click(sender As Object, e As EventArgs) Handles lbRegistrarVenta.Click
        If Len(txtObservacionOperadorCall.Text.Trim) <= 0 Then
            epNotificador.showWarning("La observación de la venta es obligatoria")
            Exit Sub
        End If

        If Len(txtObservacionesVentaDeclinada.Text.Trim) < 25 And chkDeclinarVenta.Checked Then
            epNotificador.showWarning("La observación para declinar una venta debe contener al menos 25 caracteres.")
            Exit Sub
        End If

        Dim infoVenta As New InfoGestionVentaDeclinada
        Dim resultado As ResultadoProceso

        Try
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
                .Serial = txtSerialTarjeta.Text.Trim
                .NumeroPlanillaPreAnalisis = CInt(txtNumPlanillaPreAnalisis.Text)
                .NumeroVentaPlanilla = CInt(txtNumVentaPlanilla.Text)
                .NumeroPagare = txtNumPagare.Text.Trim
                .ObservacionCallCenter = txtObservacionOperadorCall.Text.Trim
                .FechaGestion = dpFechaVenta.SelectedDate
                .IdPreventa = CInt(lblTransaccionExistente.Text.Trim)
                If chkDeclinarVenta.Checked = False Then
                    .IdEstado = 8
                    .ObservacionDeclinar = ""
                Else
                    .IdEstado = 10
                    .ObservacionDeclinar = txtObservacionesVentaDeclinada.Text.Trim
                End If
                resultado = .Actualizar()
            End With
            If resultado.Valor = 0 Then
                epNotificador.showSuccess("El registro fue actualizado satisfactoriamente. Transacci&oacute;n No. " & lblTransaccionExistente.Text)
                pnlConsulta.Visible = True
                pnlGestion.Visible = False
                pnlVentasConNovedad.Visible = True
                pnlInfoOrigenGestion.Visible = False
                LimpiarFiltros()
            Else
                Select Case resultado.Valor
                    Case 300
                        epNotificador.showError(resultado.Mensaje)
                    Case Else
                        epNotificador.showWarning(resultado.Mensaje)
                End Select
            End If
            txtObservacionesVentaDeclinada.Text = ""
        Catch ex As Exception
            epNotificador.showError("Error al tratar de registrar venta. ")
        End Try
    End Sub

    Private Sub chkDeclinarVenta_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkDeclinarVenta.CheckedChanged
        If chkDeclinarVenta.Checked Then
            trDeclinarVenta.Visible = True
        Else
            trDeclinarVenta.Visible = False
        End If
        txtObservacionesVentaDeclinada.Text = ""
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        ObtenerDatosReporte()
    End Sub
End Class