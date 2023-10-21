Imports DevExpress.Web
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.MaestroProductos

Public Class ReporteInventarioGerencia
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            epNotificador.clear()
            If Not Me.IsPostBack Then
                epNotificador.setTitle("Reporte Ventas Gerencial")
                pnlAuxiliar.Enabled = False
                Session.Remove("datosReporteInventario")
                ObtenerListaCiudad()
                ObtenerListaProducto()
                CargarListaEstadoProceso()
                CargarReporte()
            End If
            If gluPdv.IsCallback OrElse gluPdv.GridView.IsCallback OrElse Not Me.IsPostBack Then CargarListadoPdv()
            If gvInventarioG.IsCallback Then CargarReporte()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la página. ")
        End Try
    End Sub

    Private Sub CargarListaEstadoProceso()
        Try
            Dim _listaTipoVenta As New ResultadoProcesoTipoVentaColeccion
            Dim idResultadoProceso As Byte
            idResultadoProceso = 1
            If idResultadoProceso > 0 Then
                Dim dvTipoVenta As DataView = _listaTipoVenta.GenerarDataTable.DefaultView
                dvTipoVenta.RowFilter = "idResultadoProceso=" & idResultadoProceso.ToString
                With cbFiltroEstado
                    .DataSource = dvTipoVenta
                    .TextField = "TipoVenta"
                    .ValueField = "IdTipoVenta"
                    .DataBindItems()
                End With
            Else
                cbFiltroEstado.Items.Clear()
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Estatus Laboral. ")
        Finally

        End Try
    End Sub

    Private Sub ObtenerListaCiudad()
        Try
            Dim infoCiudad As New CiudadColeccion
            infoCiudad.CargarDatos()
            With cbFiltroCiudad
                .DataSource = infoCiudad
                .TextField = "ciudadDepartamento"
                .ValueField = "idCiudad"
                .DataBindItems()
            End With
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de ciudades. ")
        End Try
    End Sub

    Private Sub CargarListadoPdv(Optional ByVal idCiudad As Integer = 0)
        Try
            Dim listaPdv As New PuntoDeVentaColeccion

            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                .IdEstado = 1
                .IdCudadB = idCiudad
                .CargarDatos()
            End With
            With gluPdv
                .DataSource = listaPdv
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Puntos de Venta. ")
        End Try
    End Sub

    Private Sub ObtenerListaProducto()
        Try
            Dim infoProducto As New ProductoColeccion
            infoProducto.CargarDatos()
            With cbFiltroProducto
                .DataSource = infoProducto
                .TextField = "nombreProducto"
                .ValueField = "idTipoProducto"
                .DataBindItems()
            End With
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de ciudades. ")
        End Try
    End Sub

    Protected Sub cbFiltroCiudad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbFiltroCiudad.SelectedIndexChanged
        CargarListadoPdv(cbFiltroCiudad.Value)
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            CType(sender, ASPxCallbackPanel).JSProperties.Remove("cpLimpiarFiltros")
            Select Case e.Parameter
                Case "filtrarDatos"
                    CargarReporte(True)
                Case "limpiarFiltros"
                    LimpiarFiltros()
                    gvInventarioG.DataSource = Nothing
                    gvInventarioG.DataBind()
                    CType(sender, ASPxCallbackPanel).JSProperties("cpLimpiarFiltros") = True
            End Select
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de manejar CallBack. ", "Reporte Inventario Gerencial", ex)
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub LimpiarFiltros()
        Session.Remove("datosReporteInventario")
        pnlAuxiliar.Enabled = False
        gluPdv.GridView.Selection.UnselectAll()
        cbFiltroCiudad.SelectedIndex = -1
        cbFiltroProducto.SelectedIndex = -1
    End Sub

    Private Sub CargarReporte(Optional ByVal forzarConsulta As Boolean = False)
        Try
            Dim dtDatos As DataTable
            If Session("datosReporteInventario") Is Nothing OrElse forzarConsulta Then
                Dim reporteInventario As New WMS.ReporteInventarioGerencia

                Dim listaPdv As List(Of Object) = gluPdv.GridView().GetSelectedFieldValues("IdPdv")
                With reporteInventario

                    If cbFiltroCiudad IsNot Nothing Then .IdCiudad = CInt(cbFiltroCiudad.Value)
                    If listaPdv IsNot Nothing AndAlso listaPdv.Count > 0 Then .ListaPdv.AddRange(listaPdv)
                    If cbFiltroProducto IsNot Nothing Then .IdProducto = CInt(cbFiltroProducto.Value)
                    If deFechaInicio.Date > Date.MinValue Then .FechaInicial = deFechaInicio.Date
                    If deFechaFin.Date > Date.MinValue Then .FechaFinal = deFechaFin.Date
                    If cbFiltroEstado IsNot Nothing Then .IdEstado = CInt(cbFiltroEstado.Value)
                    .CargarDatos()
                    dtDatos = .DatosReporte()
                    Session("datosReporteInventario") = dtDatos
                End With
            Else
                dtDatos = CType(Session("datosReporteInventario"), DataTable)
            End If
            If dtDatos Is Nothing OrElse dtDatos.Rows.Count = 0 Then
                epNotificador.showWarning("No se encontraron datos acordes con los filtros de búsqueda aplicados")
                pnlAuxiliar.Enabled = False
            Else
                pnlAuxiliar.Enabled = True
            End If

            With gvInventarioG
                .SettingsBehavior.AutoExpandAllGroups = True
                .DataSource = dtDatos
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al consultar los datos.", "Reporte Inventario Gerencial", ex)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarReporte(True)
    End Sub

    Private Sub cbFormatoExportar_ButtonClick(source As Object, e As DevExpress.Web.ButtonEditClickEventArgs) Handles cbFormatoExportar.ButtonClick
        Try
            CargarReporte()
            Dim formato As String = cbFormatoExportar.Value
            If Not String.IsNullOrEmpty(formato) Then
                With gveExportador
                    .FileName = "ReporteInventarioReferencia"
                    .Landscape = False
                    With .Styles.Default.Font
                        .Name = "Arial"
                        .Size = FontUnit.Point(10)
                    End With
                    .DataBind()
                End With
                Select Case formato
                    Case "xls"
                        gveExportador.WriteXlsToResponse()
                    Case "pdf"
                        With gveExportador
                            .Landscape = True
                            .WritePdfToResponse()
                        End With
                    Case "xlsx"
                        gveExportador.WriteXlsxToResponse()
                    Case "csv"
                        gveExportador.WriteCsvToResponse()
                End Select
            End If
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de exportar datos. ", "Reporte Inventario Gerencial", ex)
        End Try
    End Sub

    Private Sub gvInventarioG_BeforeGetCallbackResult(sender As Object, e As System.EventArgs) Handles gvInventarioG.BeforeGetCallbackResult
    End Sub

    Private Sub gvInventarioG_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvInventarioG.CustomCallback
        Dim mensajeErr As String = "Error al tratar de procesar solicitud. "
        Try
            Select Case e.Parameters
                Case "expandir"
                    mensajeErr = "Error al tratar de Expandir Todo. "
                    gvInventarioG.ExpandAll()
                Case "contraer"
                    mensajeErr = "Error al tratar de Contraer Todo. "
                    gvInventarioG.CollapseAll()
            End Select
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail(mensajeErr, "Reporte Inventario Gerencial", ex)
        End Try
        CType(sender, ASPxGridView).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub
End Class