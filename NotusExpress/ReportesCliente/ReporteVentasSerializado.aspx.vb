﻿Imports DevExpress.Web
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.MaestroProductos

Public Class ReporteInventarioGerencial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            epNotificador.clear()
            If Not Me.IsPostBack Then
                epNotificador.setTitle("Reporte Ventas Serializado")
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
        Dim lista As New ResultadoProcesoVentaColeccion
        Try
            lista.CargarDatos()
            With cbFiltroEstado
                .DataSource = lista
                .TextField = "Descripcion"
                .ValueField = "IdResultado"
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Estatus Laboral. ")
        Finally
            'cbFiltroEstado.Items.Insert(0, New ListItem("Seleccione un Estatus", "0"))
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

    'Private Sub ObtenerListaSubproducto(Optional ByVal idTipoProducto As Integer = 0)
    '    Try
    '        Dim infoSubproducto As New SubproductoColeccion
    '        infoSubproducto.CargarDatos()
    '        infoSubproducto.IdProductoPadre = idTipoProducto
    '        With cbFiltroSubproducto
    '            .DataSource = infoSubproducto
    '            .TextField = "nombreSubproducto"
    '            .ValueField = "idProducto"
    '            .DataBindItems()
    '        End With
    '    Catch
    '        epNotificador.showError("Error al tratar de cargar el listado de ciudades. ")
    '    End Try
    'End Sub

    Protected Sub cbFiltroCiudad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbFiltroCiudad.SelectedIndexChanged
        CargarListadoPdv(cbFiltroCiudad.Value)
    End Sub

    'Protected Sub cbFiltroProducto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbFiltroProducto.SelectedIndexChanged
    '    ObtenerListaSubproducto(cbFiltroProducto.Value)
    'End Sub

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
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de manejar CallBack. ", "Reporte Inventario Serializado", ex)
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        'If mensajero.Mensaje.Length > 0 Then
        'CType(sender, ASPxCallbackPanel).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje()
        'CType(sender, ASPxCallbackPanel).JSProperties("cpTituloPopUp") = mensajero.Titulo
        'End If
    End Sub

    Private Sub LimpiarFiltros()
        Session.Remove("datosReporteInventario")
        pnlAuxiliar.Enabled = False
        gluPdv.GridView.Selection.UnselectAll()
        cbFiltroCiudad.SelectedIndex = -1
        cbFiltroProducto.SelectedIndex = -1
        'cbFiltroSubproducto.SelectedIndex = -1
    End Sub

    Private Sub CargarReporte(Optional ByVal forzarConsulta As Boolean = False)
        Try
            Dim dtDatos As DataTable
            If Session("datosReporteInventario") Is Nothing OrElse forzarConsulta Then
                Dim reporteInventario As New WMS.ReporteVentasSerializado

                Dim listaPdv As List(Of Object) = gluPdv.GridView().GetSelectedFieldValues("IdPdv")
                With reporteInventario

                    If cbFiltroCiudad IsNot Nothing Then .IdCiudad = CInt(cbFiltroCiudad.Value)
                    If listaPdv IsNot Nothing AndAlso listaPdv.Count > 0 Then .ListaPdv.AddRange(listaPdv)
                    If cbFiltroProducto IsNot Nothing Then .IdProducto = CInt(cbFiltroProducto.Value)
                    'If cbFiltroSubproducto IsNot Nothing Then .IdSubproducto = CShort(cbFiltroSubproducto.Value)
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
                'CType(.Columns("Almacen"), GridViewDataColumn).GroupBy()
                .SettingsBehavior.AutoExpandAllGroups = True
                .DataSource = dtDatos
                .DataBind()
            End With
        Catch ex As Exception
            'mensajero.MostrarErrorYNotificarViaMail("Error al tratar de consultar datos. ", "Reporte General de Inventario", ex)
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
        'CType(gvInventario.Columns("Almacen"), GridViewDataColumn).GroupBy()
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