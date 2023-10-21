Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer

PUblic Class ReporteRealceClienteExterno
    Inherits System.Web.UI.Page

#Region "Eventos"

    Private Sub ReporteRealceClienteExterno_Init(sender As Object, e As EventArgs) Handles Me.Init
        Herramientas.CargarLicenciaGembox()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Reporte Solicitud de Realce Cliente")
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
            Dim objDatos As New Reportes.ReporteRealceClienteExterno
            With objDatos
                .DatosReporte = CType(Session("listaBusqueda"), DataTable)
                resultado = .GenerarReporteExcel()
                Dim id As New Guid
                Dim fecha As Date = Date.Now
                If resultado.Valor = 0 Then
                    Session("rutaArchivo") = .RutaArchivo
                    Herramientas.ForzarDescargaDeArchivo(HttpContext.Current, .RutaArchivo, "ReporteSolicitudRealce_" + CStr(fecha) + ".xlsx")
                Else
                    epNotificador.showWarning(resultado.Mensaje)
                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Se presento un error al generar el reporte del detalle de la informacion: " & ex.Message)
        End Try
    End Sub

    Protected Sub btnExportarReportar_Click(sender As Object, e As EventArgs) Handles btnExportarReportar.Click
        Try
            ReportarObtenerClientesRegistrados(True)
            Dim resultado As New ResultadoProceso
            Dim dt As DataTable = CType(Session("listaBusqueda"), DataTable)
            Dim objDatos As New Reportes.ReporteRealceClienteExterno
            With objDatos
                .DatosReporte = CType(Session("listaBusqueda"), DataTable)
                resultado = .GenerarReporteExcel()
                Dim id As New Guid
                Dim fecha As Date = Date.Now
                If resultado.Valor = 0 Then
                    Session("rutaArchivo") = .RutaArchivo
                    Herramientas.ForzarDescargaDeArchivo(HttpContext.Current, .RutaArchivo, "ReporteSolicitudRealce_" + CStr(fecha) + ".xlsx")
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

        'Cargar Campañas registradas
        Dim objCampania As New CampaniaColeccion
        objCampania.IdUsuarioConsulta = Integer.Parse(Session("userId").ToString())
        Session("dtcampania") = objCampania.GenerarDataTable
        CargarComboDX(cmbCampania, CType(Session("dtcampania"), DataTable), "IdCampania", "Nombre")

    End Sub

    Private Sub CargarListadoDeClientesRegistrados(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable = Nothing
        Try
            If Session("listaBusqueda") Is Nothing OrElse forzarConsulta Then

                Dim infoCliente As New Reportes.ReporteRealceClienteExterno
                With infoCliente
                    .IdBase = cmbBase.Value
                    .IdCampania = cmbCampania.Value
                    .FechaInicial = deFechaInicio.Value
                    .FechaFinal = deFechaFin.Value
                    If cmbReportado.SelectedIndex = -1 Then
                        .Reportado = 0
                    Else
                        .Reportado = cmbReportado.Value
                    End If
                    .IdUsuarioConsulta = Integer.Parse(Session("userId").ToString())
                    dtDatos = .DatosReporte
                End With

                Session("listaBusqueda") = dtDatos
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

    Private Sub ReportarObtenerClientesRegistrados(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable = Nothing
        Try
            Dim infoCliente As New Reportes.ReporteRealceClienteExterno
            With infoCliente
                .IdBase = cmbBase.Value
                .IdCampania = cmbCampania.Value
                .FechaInicial = deFechaInicio.Value
                .FechaFinal = deFechaFin.Value
                .Reportado = 0
                .IdUsuarioConsulta = Integer.Parse(Session("userId").ToString())
                dtDatos = .DatosReportarVentas()
            End With

            Session("listaBusqueda") = dtDatos

            'With gvDatos
            '    .DataSource = dtDatos
            '    .DataBind()
            'End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la información del reporte.")
        End Try
    End Sub

#End Region


End Class