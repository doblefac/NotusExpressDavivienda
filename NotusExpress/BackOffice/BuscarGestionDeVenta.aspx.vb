Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Reportes
Imports NotusExpressBusinessLayer.General
Imports System.Globalization

Public Class BuscarGestionDeVenta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Buscar Información de Gestiones de Venta")
            txtFechaInicial.Value = Now.AddDays((Now.Day * -1) + 1).ToString("dd/MM/yyyy")
            txtFechaFinal.Value = Now.ToString("dd/MM/yyyy")
        End If
    End Sub

    Private Function ObtenerDatos() As DataTable
        Dim dtDatos As DataTable = Nothing
        Dim reporte As New ReporteGeneralDeVentas
        RecargarFiltros()

        With reporte
            If Not String.IsNullOrEmpty(txtFechaInicial.Value) AndAlso Not String.IsNullOrEmpty(txtFechaFinal.Value) Then
                Date.TryParseExact(txtFechaInicial.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, .FechaInicial)
                Date.TryParseExact(txtFechaFinal.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, .FechaFinal)
                .TipoFechas = rblTipoFecha.SelectedValue
            End If
            If Not String.IsNullOrEmpty(ddlPdv.SelectedValue) AndAlso ddlPdv.SelectedValue <> "0" Then .IdPuntoDeVenta = CInt(ddlPdv.SelectedValue)
            If Not String.IsNullOrEmpty(ddlAsesor.SelectedValue) AndAlso ddlAsesor.SelectedValue <> "0" Then .IdAsesorComercial = CInt(ddlAsesor.SelectedValue)
            If Not String.IsNullOrEmpty(txtNumIdCliente.Text) Then .NumIDCliente = txtNumIdCliente.Text.Trim
            If Not String.IsNullOrEmpty(ddlResultadoProceso.SelectedValue) AndAlso ddlResultadoProceso.SelectedValue <> "0" Then .IdResultadoProceso = CInt(ddlResultadoProceso.SelectedValue)
            .CargarDatos()
            dtDatos = .DatosReporte()
        End With
        GuardarFiltrosAplicados()
        Return dtDatos
    End Function

    Private Sub RecargarFiltros()
        If Not String.IsNullOrEmpty(Request.QueryString("recargar")) AndAlso Session("filtrosBusquedaVenta") IsNot Nothing Then
            Dim htFiltros As Hashtable = CType(Session("filtrosBusquedaVenta"), Hashtable)
            With ddlPdv
                .SelectedIndex = .Items.IndexOf(.Items.FindByValue(htFiltros(.ID)))
            End With
            With ddlAsesor
                .SelectedIndex = .Items.IndexOf(.Items.FindByValue(htFiltros(.ID)))
            End With
            txtNumIdCliente.Text = htFiltros(txtNumIdCliente.ID)
            With ddlResultadoProceso
                .SelectedIndex = .Items.IndexOf(.Items.FindByValue(htFiltros(.ID)))
            End With

            txtFechaInicial.Value = htFiltros(txtFechaInicial.ID)
            txtFechaFinal.Value = htFiltros(txtFechaFinal.ID)

            With rblTipoFecha
                .SelectedIndex = .Items.IndexOf(.Items.FindByValue(htFiltros(.ID)))
            End With
        End If
    End Sub

    Private Sub EnlazarDatos(dtDatos As DataTable)
        If dtDatos Is Nothing Then dtDatos = New DataTable
        With gvResultado
            .DataSource = dtDatos
            .Columns(0).FooterText = dtDatos.Rows.Count.ToString & " Registro(s) Encontrado(s)"
            .DataBind()
        End With
        MergeGridViewFooter(gvResultado)
        If dtDatos.Rows.Count = 0 Then epNotificador.showWarning("<i>No se obtuvieron resultados de acuerdo con los filtros aplicados</i>")
    End Sub

    Protected Sub lbConsultar_Click(sender As Object, e As EventArgs) Handles lbConsultar.Click
        Try
            Dim dtDatos As DataTable = ObtenerDatos()
            EnlazarDatos(dtDatos)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener datos. ")
        End Try
    End Sub

    Private Sub GuardarFiltrosAplicados()
        Dim htFiltros As New Hashtable
        With htFiltros
            .Add(ddlPdv.ID, ddlPdv.SelectedValue)
            .Add(ddlAsesor.ID, ddlAsesor.SelectedValue)
            .Add(txtNumIdCliente.ID, txtNumIdCliente.Text)
            .Add(ddlResultadoProceso.ID, ddlResultadoProceso.SelectedValue)
            .Add(txtFechaInicial.ID, txtFechaInicial.Value)
            .Add(txtFechaFinal.ID, txtFechaFinal.Value)
            .Add(rblTipoFecha.ID, rblTipoFecha.SelectedValue)
        End With
        Session("filtrosBusquedaVenta") = htFiltros
    End Sub

End Class