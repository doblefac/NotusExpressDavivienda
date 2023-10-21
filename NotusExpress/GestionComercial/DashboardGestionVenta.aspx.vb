Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer
Imports DevExpress.XtraCharts
Imports DevExpress.Web
Imports NotusExpressBusinessLayer.Comunes
Imports System.Net

Public Class Dashboard_Gestion_de_Venta
    Inherits System.Web.UI.Page

    Private VentasHora As New Series("Ventas por Hora", ViewType.Line)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Not IsPostBack Then
            LlenarComboCampania()
            If Session("idCampania") IsNot Nothing Then
                cmbCampania.Value = Session("idCampania")
            End If
            CargaInicial()
            'End If

        Catch ex As Exception
            epNotificador.showError("Se presento un error al cargar la página: " & ex.Message)
        End Try

    End Sub

    Private Sub CargaInicial()
        Dim resultado As New ResultadoProceso
        Dim infoClienteFinal As New ClienteFinal
        Try
            Dim dsDatos As DataSet

            With infoClienteFinal
                If Not String.IsNullOrEmpty(cmbCampania.Value) Then
                    .IdCampania = Integer.Parse(cmbCampania.Value)
                    Session("idCampania") = cmbCampania.Value
                End If
                dsDatos = .ObtenerDatosDashboard()
            End With

            If dsDatos.Tables(0).Rows.Count > 0 Or dsDatos.Tables(1).Rows.Count > 0 Then



                Session("dtVentasHora") = dsDatos.Tables(0)

                For Each row In dsDatos.Tables(0).Rows
                    With wcVentasHora
                        .Series("Ventas por Hora").Points.Add(New SeriesPoint(row("Hora"), row("Cantidad")))
                    End With
                Next

                wcSupervisores.Series.Clear()

                Session("dtSupervisores") = dsDatos.Tables(1)

                Dim Supervisor As New Series("Supervisor", ViewType.Pie)
                wcSupervisores.Series.Add(Supervisor)

                Supervisor.ArgumentScaleType = ScaleType.Qualitative
                Supervisor.ValueScaleType = ScaleType.Numerical

                With wcSupervisores

                    For Each row In dsDatos.Tables(1).Rows
                        .Series("Supervisor").Points.Add(New SeriesPoint(row("Supervisor"), row("Porcentaje")))
                    Next

                    Dim filter As New _
                        SeriesPointFilter(SeriesPointKey.Value_1, DataFilterCondition.GreaterThanOrEqual, 10)
                    CType(.Series("Supervisor").View, PieSeriesView).ExplodedPointsFilters.Add(filter)
                    CType(.Series("Supervisor").View, PieSeriesView).ExplodeMode = PieExplodeMode.UseFilters

                    .Series("Supervisor").SeriesPointsSorting = SortingMode.Ascending
                    .Series("Supervisor").SeriesPointsSortingKey = SeriesPointKey.Value_1
                    CType(.Series("Supervisor").View, PieSeriesView).Rotation = 90

                    CType(.Series("Supervisor").Label.PointOptions, PiePointOptions).PointView = PointView.ArgumentAndValues
                    wcSupervisores.Legend.Visible = False

                End With

                Session("dtTotalVentas") = dsDatos.Tables(2)

                gvAsesorVentas.DataSource = dsDatos.Tables(2)
                gvAsesorVentas.DataBind()

                CambiarTab()
            End If

        Catch ex As Exception
            epNotificador.showWarning("No se encontraron datos para la generacion del reporte.")
            'epNotificador.showError(ex.Message)
        End Try

    End Sub

    Protected Sub wcSupervisores_ObjectSelected(sender As Object, e As HotTrackEventArgs) Handles wcSupervisores.ObjectSelected
        Dim series As Series = TryCast(e.Object, Series)
        If Not (TypeOf e.Object Is Series) Then
            e.Cancel = True
            wcSupervisores.ClearSelection()
        Else
            Dim explodedPoints As ExplodedSeriesPointCollection = (CType(series.View, PieSeriesViewBase)).ExplodedPoints
            Dim point As SeriesPoint = CType(e.AdditionalObject, SeriesPoint)
            CargarAsesores(point.Argument)
        End If
    End Sub

    Private Sub LlenarComboCampania()
        '** Cargar Campañas desde NotusOP mediante un WebService **
        Dim dtCampania As New DataTable
        dtCampania = ObtenerCampanias()
        CargarComboDX(cmbCampania, dtCampania, "idCampania", "campania")
    End Sub

    Private Function ObtenerCampanias() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsFiltroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim objetoCampania As New Campania

        ' Se hace la validción del perfil para saber si el personal es de Atento. El perfil 23 es del agente de Contact de Atento
        With WSInfoFiltros
            .IdClienteExterno = Session("userId")
            .ListaTipoServicio = New Object() {NotusIlsService.TipoServicio.ServiciosFinancieros, "20,21"}
            Wsresultado = objCampania.ConsultarCampaniasCEM(WSInfoFiltros, dsDatos)
            'dtDatos = objetoCampania.ConsultarCampaniaGestionCallCenterComboBox(NotusIlsService.TipoServicio.ServiciosFinancieros, "10,20")
        End With
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    'Protected Sub pcVer_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles pcVer.WindowCallback
    '    Try
    '        Dim arrayAccion As String()
    '        arrayAccion = e.Parameter.Split(":")
    '        Select Case arrayAccion(0)
    '            Case "Actualizar"
    '                CargarAsesores()
    '        End Select
    '    Catch ex As Exception
    '        epNotificador.showError("Ocurrio un error al generar el registro: " & ex.Message)
    '    End Try
    'End Sub

    Private Sub CargarAsesores(ByVal Supervisor As String)
        Dim infoClienteFinal As New ClienteFinal
        Try
            wcAsesor.Series.Clear()

            Dim Asesor As New Series("Asesor", ViewType.Bar)
            wcAsesor.Series.Add(Asesor)

            Asesor.ArgumentScaleType = ScaleType.Qualitative
            Asesor.ValueScaleType = ScaleType.Numerical

            Dim dtDatos As New DataTable
            With infoClienteFinal
                .Supervisor = Supervisor
                dtDatos = .ObtenerDatosAsesores()
            End With

            For Each row In dtDatos.Rows
                Asesor.Points.Add(New SeriesPoint(row("Asesor"), row("Ventas")))
            Next

            With wcAsesor
                Dim v = New SideBySideBarSeriesView()
                v.BarWidth = 0.30000000000000004
                wcAsesor.SeriesTemplate.View = v
                .DataBind()
            End With

        Catch ex As Exception
            epNotificador.showError("Ocurrio un error al generar el registro: " & ex.Message)
        End Try
    End Sub

    Protected Sub gvAsesorVentas_DataBinding(sender As Object, e As EventArgs) Handles gvAsesorVentas.DataBinding
        If Session("dtTotalVentas") IsNot Nothing Then gvAsesorVentas.DataSource = CType(Session("dtTotalVentas"), DataTable)
    End Sub

    Protected Sub Link_Init_imagen(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim ctrl As Image = CType(sender, Image)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            Dim _estado As Integer = gvAsesorVentas.GetRowValuesByKeyValue(templateContainer.KeyValue, "idEstado")

            If _estado = 1 Then
                ctrl.ImageUrl = "~/img/BallGreen.png"
            ElseIf _estado = 2 Then
                ctrl.ImageUrl = "~/img/BallOrange.png"
            ElseIf _estado = 3 Then
                ctrl.ImageUrl = "~/img/BallRed.png"
            ElseIf _estado = 4 Then
                ctrl.ImageUrl = "~/img/BallGray.png"
            End If

        Catch ex As Exception
            epNotificador.showError("No fué posible establecer todos los parametros: " & ex.Message)
        End Try
    End Sub


    Private Sub CambiarTab()

        Dim Total As Integer

        Total = gvAsesorVentas.PageCount - 1
        'Session("index") = gvAsesorVentas.PageIndex

        If CType(Session("Page"), Integer) <= 0 Then
            Session("Page") = 0
        End If

        gvAsesorVentas.PageIndex = Session("Page")

        If Session("Page") = Total Then
            Session("Page") = -1
        Else
            Session("Page") += 1
        End If

        gvAsesorVentas.DataBind()

    End Sub

    'Protected Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
    '    pcVer.ShowShadow = False
    'End Sub
End Class