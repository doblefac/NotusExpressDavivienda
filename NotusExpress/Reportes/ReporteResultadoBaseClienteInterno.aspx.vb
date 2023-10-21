Imports NotusExpressBusinessLayer
Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.Comunes
Imports System.Net

Public Class ReporteResultadoBaseClienteInterno
    Inherits System.Web.UI.Page

#Region "Eventos"

    Private Sub ReportePresupuestoInterno_Init(sender As Object, e As EventArgs) Handles Me.Init
        Herramientas.CargarLicenciaGembox()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Reporte Resultado Base Datos Clientes (Interno)")
                    CargarFiltros()
                End With
            End If
        Catch ex As Exception
            epNotificador.showError("Se presento un error al cargar la página: " & ex.Message)
        End Try
    End Sub

    Protected Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "filtrarDatos"
                    CargarListadoDeClientesRegistrados(True)
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub gvDatos_DataBinding(sender As Object, e As System.EventArgs) Handles gvDatos.DataBinding
        If Session("listaBusqueda") IsNot Nothing Then gvDatos.DataSource = Session("listaBusqueda")
    End Sub

    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Dim resultado As New ResultadoProceso
            Dim dt As DataTable

            If Session("listaBusqueda") Is Nothing Then
                Dim infoCliente As New Reportes.ReporteResultadoBaseClienteInterno(cmbBase.Value)
                dt = infoCliente.DatosReporte
                Session("listaBusqueda") = dt
            Else
                dt = CType(Session("listaBusqueda"), DataTable)
            End If

            Dim objDatos As New Reportes.ReporteResultadoBaseClienteInterno
            With objDatos
                .DatosReporte = CType(Session("listaBusqueda"), DataTable)
                resultado = .GenerarReporteExcel()
                If resultado.Valor = 0 Then
                    Session("rutaArchivo") = .RutaArchivo
                    Herramientas.ForzarDescargaDeArchivo(HttpContext.Current, .RutaArchivo, "Reporte Resultado Base de Clientes (Interno).xlsx")
                Else
                    epNotificador.showWarning(resultado.Mensaje)
                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Se presento un error al generar el reporte del detalle de la informacion: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarFiltros()
        'Se cargan las bases cargadas
        Dim objBase As New PlanoClienteFinalColeccion
        Session("dtBase") = objBase.GenerarDataTable
        CargarComboDX(cmbBase, CType(Session("dtBase"), DataTable), "idPlano", "nombreBase")
    End Sub

    Private Function ObtenerCampanias() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsFiltroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Dim resultado As New ResultadoProceso

        With WSInfoFiltros
            .ListaTipoServicio = New Object() {NotusIlsService.TipoServicio.ServiciosFinancieros}
            Wsresultado = objCampania.ConsultarCampaniasCEM(WSInfoFiltros, dsDatos)
        End With
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Private Sub CargarListadoDeClientesRegistrados(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable = Nothing
        Try
            If Session("listaBusqueda") Is Nothing OrElse forzarConsulta Then
                Dim infoCliente As New Reportes.ReporteResultadoBaseClienteInterno(cmbBase.Value)
                dtDatos = infoCliente.DatosReporte
                Session("listaBusqueda") = dtDatos
            Else
                dtDatos = CType(Session("listaBusqueda"), DataTable)
            End If

            Dim _idCampania As Integer = dtDatos.Rows(0).Item("idCampania")
            Dim dtCampania As DataTable = ObtenerCampanias()

            If dtCampania IsNot Nothing AndAlso dtCampania.Rows.Count > 0 Then
                Dim dr As DataRow() = dtCampania.Select("idCampania=" & _idCampania)
                If dr.Length > 0 Then
                    For i As Integer = 0 To dtDatos.Rows.Count - 1
                        dtDatos.Rows(i).Item("nombreCampania") = dr(0).Item("Nombre")
                    Next
                End If
            End If

            With gvDatos
                .DataSource = dtDatos
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la información del reporte.")
        End Try
    End Sub

#End Region


End Class