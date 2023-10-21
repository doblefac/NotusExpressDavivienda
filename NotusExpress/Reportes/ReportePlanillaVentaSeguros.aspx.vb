Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.RecursoHumano

Public Class ReportePlanillaVentaSeguros
    Inherits System.Web.UI.Page

#Region "Eventos"

    Private Sub ReporteRealceClienteExterno_Init(sender As Object, e As EventArgs) Handles Me.Init
        Herramientas.CargarLicenciaGembox()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Reporte de Planilla Venta de Seguros")
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
            If Session("listaBusqueda") Is Nothing Then
                CargarListadoDeClientesRegistrados(True)
            End If
            Dim resultado As New ResultadoProceso
            Dim dt As DataTable = CType(Session("listaBusqueda"), DataTable)
            Dim objDatos As New Reportes.ReportePlanillaVentaSeguros
            With objDatos
                .DatosReporte = CType(Session("listaBusqueda"), DataTable)
                .DatosSeguros = CType(Session("listaSeguros"), DataTable)
                resultado = .GenerarReporteExcel()
                If resultado.Valor = 0 Then
                    Session("rutaArchivo") = .RutaArchivo
                    Herramientas.ForzarDescargaDeArchivo(HttpContext.Current, .RutaArchivo, "Reporte Planilla Venta Seguros.xlsx")
                Else
                    epNotificador.showWarning(resultado.Mensaje)
                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Se presento un error al generar el reporte de la informacion: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarFiltros()
        'Se cargan los asesores comerciales
        Dim listaAsesor As New AsesorComercialColeccion
        With listaAsesor
            .IdEstado = Enumerados.EstadoBinario.Activo
        End With
        CargarComboDX(cmbAsesor, CType(listaAsesor.GenerarDataTable, DataTable), "IdUsuarioSistema", "NombreAsesor")
        cmbAsesor.SelectedIndex = -1
        cmbAsesor.Enabled = True
    End Sub

    Private Sub CargarListadoDeClientesRegistrados(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable = Nothing
        Dim dsDatos As DataSet = Nothing
        Try
            If Session("listaBusqueda") Is Nothing OrElse forzarConsulta Then
                Dim infoReporte As New Reportes.ReportePlanillaVentaSeguros
                With infoReporte
                    If txtFiltroIdentificacion.Text.Trim <> "" Then .NumIdentificacion = txtFiltroIdentificacion.Text.Trim
                    If cmbAsesor.Value <> Nothing Then .IdAsesor = cmbAsesor.Value
                    If deFechaInicio.Value <> Nothing Then .FechaInicial = deFechaInicio.Value
                    If deFechaFin.Value <> Nothing Then .FechaFinal = deFechaFin.Value
                    dsDatos = .DsDatosReporte
                    .DatosReporte = dsDatos.Tables(0)
                    .DatosSeguros = dsDatos.Tables(1)
                    Session("listaSeguros") = .DatosSeguros
                    dtDatos = .DatosReporte
                    Session("listaBusqueda") = dtDatos
                End With
            Else
                dtDatos = CType(Session("listaBusqueda"), DataTable)
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