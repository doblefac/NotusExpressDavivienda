Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.Reportes

Public Class DescargueManual
    Inherits System.Web.UI.Page

#Region "Atributos Privados"

    Dim idRol As Integer

#End Region

#Region "Métodos Privados"

    Private Sub CambiarVistaTabScript(ByVal proceso As String)
        Select Case proceso
            Case "LISTADO"
                pnlInfoVenta.Visible = False
                pnlHistoricoVenta.Visible = True
                tsInfoGestion.Items(0).Disabled = False
                tsInfoGestion.Items(1).Disabled = True
                tsInfoGestion.SelectedIndex = 0
            Case "MODIFICAR"
                pnlInfoVenta.Visible = True
                tsInfoGestion.Items(0).Disabled = True
                tsInfoGestion.Items(1).Disabled = False
                tsInfoGestion.SelectedIndex = 1
        End Select
    End Sub

    Private Sub LimpiarYControlarVisualizacionDeCamposDeProducto(ByVal visible As Boolean)
        ddlProductoPadre.ClearSelection()
        ddlSubproducto.ClearSelection()
        txtNumPagare.Text = ""
        txtSerialTarjeta.Text = ""
        trInfoProducto.Visible = visible
        trInfoSerial.Visible = visible
    End Sub

    Private Sub InicializarCampos()
        For Each ctrl As Control In pnlInfoVenta.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            ElseIf TypeOf ctrl Is DropDownList Then
                CType(ctrl, DropDownList).ClearSelection()
            End If
        Next
        lblTransaccionID.Text = "0"
        txtSerialTarjeta.Text = ""
        Session.Remove("InfoCliente")
        Session.Remove("InfoPreventa")
        ActivarOInhabilitarTipoDeProducto(False)

        trInfoProducto.Visible = False
        trInfoSerial.Visible = False

    End Sub

    Private Sub CargarBodegaDestino()
        Try
            Dim lista As New BodegaDestinoColeccion
            lista.CargarDatos()
            With ddlBodegaDestino
                .DataSource = lista
                .DataTextField = "nombre"
                .DataValueField = "idBodega"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de las bodegas de destino")
        Finally
            ddlResultadoConsulta.Items.Insert(0, New ListItem("Seleccione un Resultado", "0"))
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

    Private Sub CargarTiposDeProductoConsulta(Optional ByVal idResultadoProceso As Byte = 0)
        Try
            Dim _listaTipoVenta As ResultadoProcesoTipoVentaColeccion = ObtenerListaResultadoProcesoTiposDeVenta()
            ActivarOInhabilitarTipoDeProducto(False)
            If idResultadoProceso > 0 Then
                Dim dvTipoVenta As DataView = _listaTipoVenta.GenerarDataTable.DefaultView
                dvTipoVenta.RowFilter = "idResultadoProceso=" & idResultadoProceso.ToString
                With ddlTipoProductoConsulta
                    .DataSource = dvTipoVenta
                    .DataTextField = "TipoVenta"
                    .DataValueField = "IdTipoVenta"
                    .DataBind()
                End With
                If dvTipoVenta.Count > 0 Then ActivarOInhabilitarTipoDeProducto(True)
            Else
                ddlTipoProductoConsulta.Items.Clear()
            End If
            LimpiarYControlarVisualizacionDeCamposDeProducto(False)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Tipos de Producto: ")
        Finally
            ddlTipoProductoConsulta.Items.Insert(0, New ListItem("Seleccione un Tipo de Producto", "0"))
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

    Private Sub ActivarOInhabilitarTipoDeProducto(ByVal activar As Boolean)
        ddlTipoProducto.Enabled = activar
        rfvTipoProducto.Enabled = activar
    End Sub

    Private Sub CargarPuntosDeVenta()
        Try
            Dim listaPdv As New PuntoDeVentaColeccion

            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                If chkPdvActivo.Checked Then .IdEstado = 1
                .CargarDatos()
            End With
            With ddlPuntoVentaConsulta
                .DataSource = listaPdv
                .DataTextField = "NombrePdv"
                .DataValueField = "IdPdv"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Puntos de Venta. ")
        Finally
            ddlPuntoVentaConsulta.Items.Insert(0, New ListItem("Seleccione un PDV", "0"))
        End Try
    End Sub

    
    Private Sub InicializarControlesOrigenRegistro()
        Dim activarInfoOrigenGestion As Boolean = False
        If idRol <> 3 Then
            CargarPuntosDeVenta()
            activarInfoOrigenGestion = True
        End If
        ddlPuntoVentaConsulta.Enabled = activarInfoOrigenGestion
        
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Session("idRol") IsNot Nothing Then Integer.TryParse(Session("idRol").ToString, idRol)
        If Not Me.IsPostBack Then
            With epNotificador
                .setTitle("Descargue Manual de Inventario")
            End With

            trPuntoVentaConsulta.Visible = False
            trSerial.Visible = False
            trFechaGestionConsulta.Visible = False
            trTipoProductoConsulta.Visible = False
            trConsultar.Visible = False
            pnlInfoVenta.Visible = False
            
            pnlGestion.Visible = True
            pnlHistoricoVenta.Visible = True
            CambiarVistaTabScript("LISTADO")
            ObtenerDatosSerialesNovedades()
            trBotonAccion.Visible = False
            trTraslados.Visible = False
        End If
    End Sub

    Protected Sub lbConsultar_Click(sender As Object, e As EventArgs) Handles lbConsultar.Click
        Try
            Select Case ddlConsultarPor.SelectedValue
                Case "1"
                    
                Case "2"
                    If Len(txtSerialConsultar.Text.Trim) <= 0 Then
                        epNotificador.showWarning("Por favor ingrese un número de serial.")
                        Exit Sub
                    End If
                Case "3"
                    If dpFechaGestionFinalConsulta.SelectedDate = "#12:00:00 AM#" Or dpFechaGestionInicialConsulta.SelectedDate = "#12:00:00 AM#" Then
                        epNotificador.showWarning("Por favor ingrese un rango de fechas válido.")
                        Exit Sub
                    End If
                Case "4"
                    trPuntoVentaConsulta.Visible = False
                    trSerial.Visible = False
                    trFechaGestionConsulta.Visible = False
                    trTipoProductoConsulta.Visible = True
                    trConsultar.Visible = False
                    CargarTiposDeProductoConsulta(1)
            End Select

            Dim dtDatos As DataTable

            Dim infoSeriales As New FiltroSerialesConNovedadInventario
            With infoSeriales
                If ddlPuntoVentaConsulta.SelectedValue IsNot Nothing Then Integer.TryParse(ddlPuntoVentaConsulta.SelectedValue.ToString, .IdPuntoDeVenta)
                If txtSerialConsultar.Text.Trim IsNot Nothing Then .Serial = txtSerialConsultar.Text.Trim
                If ddlTipoProductoConsulta.SelectedValue IsNot Nothing Then Integer.TryParse(ddlTipoProductoConsulta.SelectedValue.ToString, .IdTipoVenta)
                If dpFechaGestionInicialConsulta.SelectedDate > Date.MinValue Then .FechaInicial = dpFechaGestionInicialConsulta.SelectedDate
                If dpFechaGestionFinalConsulta.SelectedDate > Date.MinValue Then .FechaFinal = dpFechaGestionFinalConsulta.SelectedDate

                dtDatos = .DatosReporte
            End With

            If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
                gvHistoricoVenta.DataBind()
                EnlazarDatos(dtDatos)
            Else
                epNotificador.showWarning("No se encontraron datos según los filtros aplicados.")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ObtenerDatosSerialesNovedades(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As New DataTable

        Dim infoHistoria As New ConsultarSerialesConNovedadInventario
        With infoHistoria
            dtDatos = .DatosReporte
        End With

        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            EnlazarDatos(dtDatos)
        Else
            epNotificador.showWarning("No hay seriales con novedad de inventario.")
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
           
            chkNovedad.Enabled = False
        End If
    End Sub

    Private Sub EnlazarDatosNovedades(ByVal dtDatos As DataTable)
        With gvNovedades
            .DataSource = dtDatos
            .DataBind()
        End With

    End Sub

    Protected Sub ddlConsultarPor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlConsultarPor.SelectedIndexChanged
        Select Case ddlConsultarPor.SelectedValue
            Case "1"
                trPuntoVentaConsulta.Visible = True
                trSerial.Visible = False
                trFechaGestionConsulta.Visible = False
                trTipoProductoConsulta.Visible = False
                trConsultar.Visible = False
                CargarPuntosDeVenta()
            Case "2"
                trPuntoVentaConsulta.Visible = False
                trSerial.Visible = True
                trFechaGestionConsulta.Visible = False
                trTipoProductoConsulta.Visible = False
                trConsultar.Visible = True
            Case "3"
                trPuntoVentaConsulta.Visible = False
                trSerial.Visible = False
                trFechaGestionConsulta.Visible = True
                trTipoProductoConsulta.Visible = False
                trConsultar.Visible = True
            Case "4"
                trPuntoVentaConsulta.Visible = False
                trSerial.Visible = False
                trFechaGestionConsulta.Visible = False
                trTipoProductoConsulta.Visible = True
                trConsultar.Visible = False
                CargarTiposDeProductoConsulta(1)
        End Select
    End Sub

    Protected Sub ddlPuntoVentaConsulta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPuntoVentaConsulta.SelectedIndexChanged
        If ddlPuntoVentaConsulta.SelectedValue <> "0" Then
            trConsultar.Visible = True
        Else
            trConsultar.Visible = False
        End If
    End Sub

    
    Protected Sub ddlTipoProductoConsulta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoProductoConsulta.SelectedIndexChanged
        If ddlTipoProductoConsulta.SelectedValue <> "0" Then
            trConsultar.Visible = True
        Else
            trConsultar.Visible = False
        End If
    End Sub

    
    Private Sub gvHistoricoVenta_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvHistoricoVenta.RowCommand
        If e.CommandName = "Select" Then
            pnlHistoricoVenta.Visible = False
            CambiarVistaTabScript("MODIFICAR")
            'agregamos la información de la venta a los controles de VENTA
            CargarListaResultadoProceso()
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvHistoricoVenta.Rows(index)
            InicializarCampos()
            lblTransaccionID.Text = row.Cells(1).Text
            Dim informacionVenta = New InfoVenta(row.Cells(1).Text)
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
                If .EsNovedad = True Then
                    trDetalleNovedad.Visible = True
                    chkNovedad.Checked = True
                    ObtenerDatosNovedades(row.Cells(1).Text)
                Else
                    gvNovedades.DataBind()
                    chkNovedad.Checked = False
                    trDetalleNovedad.Visible = False
                End If
            End With
            txtAtendidoPor.Enabled = False
            ddlResultadoConsulta.Enabled = False
            ddlTipoProducto.Enabled = False
            ddlProductoPadre.Enabled = False
            txtSerialTarjeta.Enabled = False
            dpFechaVenta.Enabled = False
            txtNumPlanillaPreAnalisis.Enabled = False
            pnlInfoVenta.Visible = True
            ddlSubproducto.Enabled = False
            txtNumPagare.Enabled = False
            txtObservacionOperadorCall.Enabled = False
            chkNovedad.Enabled = False
            txtNumIdOperadorCallCenter.Enabled = False
            txtNumVentaPlanilla.Enabled = False
            pnlConsulta.Visible = False
            trBotonAccion.Visible = False
            trTraslados.Visible = False
        End If
    End Sub

    Private Sub gvHistoricoVenta_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHistoricoVenta.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
        End If
    End Sub

    Protected Sub lbCancelarVenta_Click(sender As Object, e As EventArgs) Handles lbCancelarVenta.Click
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
            pnlGestion.Visible = True

            ddlPuntoVentaConsulta.Enabled = True
            CambiarVistaTabScript("LISTADO")
            ObtenerDatosSerialesNovedades()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cancelar registro de venta. ")
        End Try
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

    Protected Sub lbDestruir_Click(sender As Object, e As EventArgs) Handles lbDestruir.Click
        Response.Redirect("RegistrarTarjetaDestruido.aspx?param=" + txtSerialTarjeta.Text.Trim + "")
    End Sub

    Protected Sub lbTrasladar_Click(sender As Object, e As EventArgs) Handles lbTrasladar.Click
        trTraslados.Visible = True
        txtSerialTrasladar.Text = txtSerialTarjeta.Text.Trim
        'consultamos la bodega origen
        Dim bodegaOrigen As New ConsultarBodegaOrigen(txtSerialTarjeta.Text.Trim)
        With bodegaOrigen
            txtBodegaOrigen.Text = .Bodega
        End With
        'consultamos las bodegas de destino
        CargarBodegaDestino()
    End Sub

    Protected Sub lbFaltante_Click(sender As Object, e As EventArgs) Handles lbFaltante.Click
        Try
            Dim faltante As New AccionesSerial
            With faltante
                .IdEstado = 11
                .IdGestionVenta = lblTransaccionID.Text.Trim
                .Serial = txtSerialTarjeta.Text.Trim
                .Actualizar()
            End With
            epNotificador.showSuccess("Serial marcado como faltante satisfactoriamente.")
            trPuntoVentaConsulta.Visible = False
            trSerial.Visible = False
            trFechaGestionConsulta.Visible = False
            trTipoProductoConsulta.Visible = False
            trConsultar.Visible = False
            pnlInfoVenta.Visible = False

            pnlGestion.Visible = True
            pnlHistoricoVenta.Visible = True
            CambiarVistaTabScript("LISTADO")
            gvHistoricoVenta.DataBind()
            ObtenerDatosSerialesNovedades()
            pnlConsulta.Visible = True
        Catch ex As Exception
            epNotificador.showError("Error al marcar como faltante el serial.")
        End Try
    End Sub

    Protected Sub lbLiberar_Click(sender As Object, e As EventArgs) Handles lbLiberar.Click
        Try
            Dim liberar As New AccionesSerial
            With liberar
                .IdEstado = 4
                .IdGestionVenta = CInt(lblTransaccionID.Text.Trim)
                .Serial = txtSerialTarjeta.Text.Trim
                .Actualizar()
            End With
            epNotificador.showSuccess("Serial Liberado satisfactoriamente.")
            trPuntoVentaConsulta.Visible = False
            trSerial.Visible = False
            trFechaGestionConsulta.Visible = False
            trTipoProductoConsulta.Visible = False
            trConsultar.Visible = False
            pnlInfoVenta.Visible = False

            pnlGestion.Visible = True
            pnlHistoricoVenta.Visible = True
            CambiarVistaTabScript("LISTADO")
            gvHistoricoVenta.DataBind()
            ObtenerDatosSerialesNovedades()
            pnlConsulta.Visible = True
        Catch ex As Exception
            epNotificador.showError("Error al liberar el serial.")
        End Try
    End Sub

    Protected Sub ddlAccion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAccion.SelectedIndexChanged
        trBotonAccion.Visible = True
    End Sub

    Protected Sub lbContinuar_Click(sender As Object, e As EventArgs) Handles lbContinuar.Click
        Select Case ddlAccion.SelectedValue
            Case 1
                lbDestruir_Click(sender, e)
            Case 2
                lbTrasladar_Click(sender, e)
            Case 3
                lbFaltante_Click(sender, e)
            Case 4
                lbLiberar_Click(sender, e)
        End Select
    End Sub

    Protected Sub lbTrasladarSerial_Click(sender As Object, e As EventArgs) Handles lbTrasladarSerial.Click
        Try
            Dim trasladar As New TrasladarBodega
            With trasladar
                .IdBodega = ddlBodegaDestino.SelectedValue
                .Serial = txtSerialTrasladar.Text.Trim
                .Actualizar()
            End With
            epNotificador.showSuccess("Serial trasladado satisfactoriamente.")
            trPuntoVentaConsulta.Visible = False
            trSerial.Visible = False
            trFechaGestionConsulta.Visible = False
            trTipoProductoConsulta.Visible = False
            trConsultar.Visible = False
            pnlInfoVenta.Visible = False

            pnlGestion.Visible = True
            pnlHistoricoVenta.Visible = True
            CambiarVistaTabScript("LISTADO")
            gvHistoricoVenta.DataBind()
            ObtenerDatosSerialesNovedades()
            pnlConsulta.Visible = True
        Catch ex As Exception
            epNotificador.showError("Error al tratar de trasladar de bodega el serial")
        End Try
    End Sub
End Class