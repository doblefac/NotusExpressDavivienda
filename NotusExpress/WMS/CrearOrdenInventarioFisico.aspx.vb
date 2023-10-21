Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.WMS
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.General

Partial Public Class CrearOrdenInventarioFisico
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            CargarListaBodegas()
            CargarListaProductos()
            CargarListaSubproductos(ddlSubproducto)
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
            EnlazarListaProductos(ddlProductoPadre, lista)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Productos. ")
        End Try
    End Sub

    Private Sub EnlazarListaProductos(ByRef ddl As DropDownList, ByVal lista As ProductoColeccion)
        With ddl
            .DataSource = lista
            .DataTextField = "NombreProducto"
            .DataValueField = "IdProducto"
            .DataBind()
            .Items.Insert(0, New ListItem("Seleccione un Producto", "0"))
        End With
    End Sub

    Public Sub CargarListaSubproductos(ByRef ddl As DropDownList, Optional ByVal idProductoPadre As Integer = 0)
        If ddl.Items.Count > 0 Then ddl.Items.Clear()
        Try
            If idProductoPadre > 0 Then
                Dim lista As New SubproductoColeccion
                With lista
                    .IdEstado = 1
                    .IdProductoPadre = idProductoPadre
                    .CargarDatos()
                End With
                With ddl
                    .DataSource = lista
                    .DataTextField = "NombreSubproducto"
                    .DataValueField = "IdSubproducto"
                    .DataBind()
                End With
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Subproductos. ")
        Finally
            ddl.Items.Insert(0, New ListItem("Seleccione un Subproducto", "0"))
        End Try
    End Sub

#End Region

    Private Sub cpSubproducto_Execute(ByVal sender As Object, ByVal e As EO.Web.CallbackEventArgs) Handles cpSubproducto.Execute
        Dim idProducto As Integer
        Integer.TryParse(ddlProductoPadre.SelectedValue, idProducto)
        CargarListaSubproductos(ddlSubproducto, idProducto)
    End Sub

    Private Sub lbGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbGuardar.Click
        Try
            Dim resultado As New ResultadoProceso
            Dim orden As New OrdenInventarioFisico
            With orden
                .IdBodega = CInt(ddlBodega.SelectedValue)
                .IdProducto = CInt(ddlProductoPadre.SelectedValue)
                .IdSubproducto = CInt(ddlSubproducto.SelectedValue)
                .IdCreador = CInt(Session("userId"))
                resultado = .Registrar()
            End With
            If resultado.Valor = 0 Then
                epNotificador.showSuccess(resultado.Mensaje)
                ddlBodega.ClearSelection()
                ddlProductoPadre.ClearSelection()
                CargarListaSubproductos(ddlSubproducto)
            Else
                epNotificador.showError(resultado.Mensaje)
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de crear orden. ")
        End Try
    End Sub
End Class