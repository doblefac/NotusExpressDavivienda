Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.WMS
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.General

Partial Public Class PoolOrdenesInventarioFisico
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private altoVentana As Integer
    Private anchoVentana As Integer

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        ObtenerTamanoVentana()
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Pool de Órdenes de Inventario")
            epNotificador.showReturnLink(Me.ResolveClientUrl("~/Administracion/Default.aspx"))
            With Request.QueryString
                If Not String.IsNullOrEmpty(.Item("acc")) AndAlso .Item("acc") = "cerrar" Then
                    epNotificador.showSuccess("La orden de inventario No. " & .Item("idOrden") & " fue Cerrada satisfactoriamente.")
                End If
            End With
            CargarListaBodegas()
            CargarListaProductos()
            CargarListaSubproductos()
            CargarListaOrdenesDeInventario()
            With dlgCrearOrden
                .Width = Unit.Pixel(Me.anchoVentana * 0.7)
                .Height = Unit.Pixel(Me.altoVentana * 0.7)
            End With
        End If
    End Sub

#Region "Métodos Privados"

    Public Sub CargarListaBodegas()
        Try
            Dim lista As BodegaColeccion = ObtenerListaBodegas()
            '*Se enlaza el listado del filtro*'
            EnlazarListaBodega(ddlBodega, lista)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de bodegas. ")
        End Try
    End Sub

    Private Function ObtenerListaBodegas() As BodegaColeccion
        Dim lista As New BodegaColeccion
        'lista.IdTipoBodega = 2
        lista.CargarDatos()
        Return lista
    End Function

    Private Sub EnlazarListaBodega(ByRef ddl As DropDownList, ByVal lista As BodegaColeccion)
        With ddl
            .DataSource = lista
            .DataTextField = "NombreBodega"
            .DataValueField = "IdBodega"
            .DataBind()
            .Items.Insert(0, New ListItem("Seleccione una Bodega", "0"))
        End With
    End Sub

    Public Sub CargarListaProductos()
        Try
            Dim lista As New ProductoColeccion
            lista.Comercializable = True
            lista.CargarDatos()
            '*Se enlaza el listado del filtro*'
            EnlazarListaProductos(lista)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Productos. ")
        End Try
    End Sub

    Private Sub EnlazarListaProductos(ByVal lista As ProductoColeccion)
        With ddlProductoPadre
            .DataSource = lista
            .DataTextField = "NombreProducto"
            .DataValueField = "IdProducto"
            .DataBind()
            .Items.Insert(0, New ListItem("Seleccione un Producto", "0"))
        End With
    End Sub

    Public Sub CargarListaSubproductos(Optional ByVal idProductoPadre As Integer = 0)
        If ddlSubproducto.Items.Count > 0 Then ddlSubproducto.Items.Clear()
        Try
            If idProductoPadre > 0 Then
                Dim lista As New SubproductoColeccion
                With lista
                    .IdEstado = 1
                    .IdProductoPadre = idProductoPadre
                    .CargarDatos()
                End With
                With ddlSubproducto
                    .DataSource = lista
                    .DataTextField = "NombreSubproducto"
                    .DataValueField = "IdSubproducto"
                    .DataBind()
                End With
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Subproductos. ")
        Finally
            ddlSubproducto.Items.Insert(0, New ListItem("Seleccione un Subproducto", "0"))
        End Try
    End Sub

    Public Sub CargarListaOrdenesDeInventario()
        Dim lista As OrdenInventarioFisicoColeccion = ObtenerOrdenesDeInventario()
        pnlResultado.Visible = False
        If lista IsNot Nothing AndAlso lista.Count > 0 Then
            EnlazarListaOrdenes(lista)
            pnlResultado.Visible = True
        Else
            epNotificador.showWarning("No se encontraron registros de acuerdo con los filtros de búsqueda especificados.")
        End If
    End Sub

    Public Function ObtenerOrdenesDeInventario() As OrdenInventarioFisicoColeccion
        Dim listaOrden As New OrdenInventarioFisicoColeccion
        With listaOrden
            If CInt(Session("idRol")) <> 12 Then .IdCreador = CInt(Session("userId"))
            If ddlBodega.SelectedValue <> "0" Then .IdBodega = CInt(ddlBodega.SelectedValue)
            If ddlProductoPadre.SelectedValue <> "0" Then .IdProducto = CInt(ddlProductoPadre.SelectedValue)
            If ddlSubproducto.SelectedValue <> "0" Then .IdSubproducto = CInt(ddlSubproducto.SelectedValue)
            .IdEstado = IIf(ddlEstado.SelectedValue <> "0", ddlEstado.SelectedValue, 1)
            .IdResponsableCierre = IIf(ddlTipoFecha.SelectedValue = "2", CInt(Session("userId")), 0)
            If Not String.IsNullOrEmpty(txtFechaInicial.Value) AndAlso Not String.IsNullOrEmpty(txtFechaFinal.Value) Then
                .FechaInicial = CDate(txtFechaInicial.Value)
                .FechaFinal = CDate(txtFechaFinal.Value)
                .IdTipoFecha = CInt(ddlTipoFecha.SelectedValue)
            End If
            .CargarDatos()
        End With

        Return listaOrden
    End Function

    Public Sub EnlazarListaOrdenes(ByVal lista As OrdenInventarioFisicoColeccion)
        If lista IsNot Nothing Then
            With gvOrdenInventario
                .DataSource = lista
                .Columns(0).FooterText = lista.Count.ToString & " Registro(s) Encontrado(s)"
                .DataBind()
            End With
            MergeGridViewFooter(gvOrdenInventario)
        End If
    End Sub

    Private Sub ObtenerTamanoVentana()
        If hfMedidasVentana.Value <> "" Then
            Dim arrAux() As String = hfMedidasVentana.Value.Split(";")
            If arrAux.Length = 2 Then
                Me.altoVentana = CInt(arrAux(0))
                Me.anchoVentana = CInt(arrAux(1))
            End If
        End If
    End Sub

#End Region

    Private Sub cpSubproducto_Execute(ByVal sender As Object, ByVal e As EO.Web.CallbackEventArgs) Handles cpSubproducto.Execute
        Dim idProducto As Integer
        Integer.TryParse(ddlProductoPadre.SelectedValue, idProducto)
        CargarListaSubproductos(idProducto)
    End Sub

    'Private Sub cpSubproductoCrear_Execute(ByVal sender As Object, ByVal e As EO.Web.CallbackEventArgs) Handles cpSubproductoCrear.Execute
    '    Dim idProducto As Integer
    '    Integer.TryParse(ddlProductoCrear.SelectedValue, idProducto)
    '    CargarListaSubproductos(ddlSubproductoCrear, idProducto)
    'End Sub

    Private Sub cpCreacionOrden_Execute(ByVal sender As Object, ByVal e As EO.Web.CallbackEventArgs) Handles cpCreacionOrden.Execute
        If e.Parameter = "limpiarDatos" Then
            With dlgCrearOrden
                .ContentUrl = "CrearOrdenInventarioFisico.aspx"
                .Width = Unit.Pixel(Me.anchoVentana * 0.7)
                .Height = Unit.Pixel(Me.altoVentana * 0.7)
                .Show()
            End With
        End If
    End Sub

    Private Sub cpResultado_Execute(ByVal sender As Object, ByVal e As EO.Web.CallbackEventArgs) Handles cpResultado.Execute
        If e.Parameter = "actualizar" Then
            CargarListaOrdenesDeInventario()
        End If
    End Sub

    Private Sub gvOrdenInventario_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvOrdenInventario.RowCommand
        If e.CommandName = "leerSeriales" Then
            Dim idOrden As String = e.CommandArgument.ToString
            With dlgCrearOrden
                .ContentUrl = "RegistrarDetalleDeInventarioFisico.aspx?idOrden=" & idOrden
                .Width = Unit.Pixel(Me.anchoVentana * 0.9)
                .Height = Unit.Pixel(Me.altoVentana * 0.7)
                .Show()
            End With
        ElseIf e.CommandName = "cerrarOrden" Then
            Try
                Dim infoOrden As New OrdenInventarioFisico(CInt(e.CommandArgument))
                Dim resultado As ResultadoProceso
                With infoOrden
                    .IdResponsableCierre = CInt(Session("userId"))
                    resultado = .CerrarOrden()
                End With
                If resultado.Valor = 0 Then
                    epNotificador.showSuccess("La orden de inventario No. " & e.CommandArgument.ToString & " fue Cerrada satisfactoriamente.")
                    CargarListaOrdenesDeInventario()
                Else
                    epNotificador.showError(resultado.Mensaje)
                End If
            Catch ex As Exception
                epNotificador.showError("Error al tratar de crerrar la orden. ")
            End Try
        End If
    End Sub

    Private Sub gvOrdenInventario_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOrdenInventario.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim fechaAux As Date = CType(e.Row.DataItem, OrdenInventarioFisico).FechaCierre
            If fechaAux > Date.MinValue Then
                CType(e.Row.FindControl("ibAdicionarDetalle"), ImageButton).Visible = False
                CType(e.Row.FindControl("ibCerrarOrden"), ImageButton).Visible = False
            Else
                e.Row.Cells(8).Text = ""
            End If
        End If
    End Sub

    Protected Sub lbConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbConsultar.Click
        Try
            CargarListaOrdenesDeInventario()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de consultar ordenes de inventario. ")
        End Try
    End Sub

    Protected Sub lbQuitarFiltros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbQuitarFiltros.Click
        Try
            ddlBodega.ClearSelection()
            ddlProductoPadre.ClearSelection()
            CargarListaSubproductos()
            ddlEstado.ClearSelection()
            ddlTipoFecha.ClearSelection()
            txtFechaInicial.Value = ""
            txtFechaFinal.Value = ""
            CargarListaOrdenesDeInventario()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de quitar filtros. ")
        End Try
    End Sub

End Class