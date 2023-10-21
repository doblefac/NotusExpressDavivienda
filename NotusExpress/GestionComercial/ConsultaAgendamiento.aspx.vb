Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Public Class ConsultaAgendamiento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Inicio()
        End If
    End Sub

#Region "Métodos Privados"
    Private Sub Inicio()
        Dim estrategiaComercial As New EstrategiaComercialColeccion()
        estrategiaComercial.CargarDatos()
        ddlEstrategiaComercialFiltro.DataSource = estrategiaComercial
        ddlEstrategiaComercialFiltro.DataBind()

        Dim ciudad As New CiudadColeccion()
        ciudad.CargarDatos()
        ddlCiudadFiltro.DataSource = ciudad
        ddlCiudadFiltro.DataBind()

        Dim empresa As New EmpresaColeccion()
        empresa.CargarDatos()
        ddlEmpresaFiltro.DataSource = empresa
        ddlEmpresaFiltro.DataBind()

        Dim estado As New ListadoEstadosColeccion()
        estado.CargarDatos()
        ddlEstadoFiltro.DataSource = estado
        ddlEstadoFiltro.DataBind()
    End Sub
#End Region

End Class