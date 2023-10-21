Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer.RecursoHumano
Imports DevExpress.Web

Public Class BusquedaGestionesNoEfectivasCallCenter
    Inherits System.Web.UI.Page

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Pool de busqueda de ventas no efectivas")
                End With
                CargarPermisosOpcionesRestringidas()
                CargaInicial()
            End If
        Catch ex As Exception
            epNotificador.showError("Se presento un error al cargar la página: " & ex.Message)
        End Try
    End Sub

    Protected Sub Link_Init(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim linkVer As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkVer.NamingContainer, GridViewDataItemTemplateContainer)
            Dim NumeroDocumento As String = CInt(gvDatos.GetRowValuesByKeyValue(templateContainer.KeyValue, "NumeroIdentificacion"))
            Dim arrControles() As String = {"lnkGestionar"}
            For indice As Integer = 0 To arrControles.Length - 1
                Dim ctrl As ASPxHyperLink = templateContainer.FindControl(arrControles(indice))
                ctrl.Visible = True
                If ctrl IsNot Nothing Then
                    ctrl.ClientSideEvents.Click = ctrl.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)
                    ctrl.ClientSideEvents.Click = ctrl.ClientSideEvents.Click.Replace("{1}", NumeroDocumento)
                End If
            Next
        Catch ex As Exception
            epNotificador.showError("No fué posible establecer los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub

    Protected Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "filtrarDatos"
                    CargarListadoDeClientesRegistrados(True)
                Case "LimpiarConsulta"
                    CType(Session("listaBusqueda"), DataTable).Clear()
                    Dim dt As New DataTable
                    With gvDatos
                        .DataSource = dt
                        .DataBind()
                    End With
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub gvDatos_DataBinding(sender As Object, e As System.EventArgs) Handles gvDatos.DataBinding
        If Session("listaBusqueda") Is Nothing Then
        Else
            gvDatos.DataSource = Session("listaBusqueda")
        End If
    End Sub

    Private Sub cbFormatoExportar_ButtonClick(source As Object, e As DevExpress.Web.ButtonEditClickEventArgs) Handles cbFormatoExportar.ButtonClick
        Try
            'CargarListadoDeServiciosRegistrados(True)
            Dim formato As String = cbFormatoExportar.Value
            If Not String.IsNullOrEmpty(formato) Then
                With gveExportador
                    .FileName = "ReporteGeneralVentas"
                    .Landscape = False
                    With .Styles.Default.Font
                        .Name = "Arial"
                        .Size = FontUnit.Point(10)
                    End With
                    .DataBind()
                End With
                Dim valor As Integer = CInt(cmbDetalle.Value)
                Select Case formato
                    Case "xls"
                        gvDatos.SettingsDetail.ExportMode = CType(System.Enum.Parse(GetType(GridViewDetailExportMode), valor), GridViewDetailExportMode)
                        gveExportador.WriteXlsToResponse()
                    Case "pdf"
                        gvDatos.SettingsDetail.ExportMode = CType(System.Enum.Parse(GetType(GridViewDetailExportMode), valor), GridViewDetailExportMode)
                        With gveExportador
                            .Landscape = True
                            .WritePdfToResponse()
                        End With
                    Case "xlsx"
                        gvDatos.SettingsDetail.ExportMode = CType(System.Enum.Parse(GetType(GridViewDetailExportMode), valor), GridViewDetailExportMode)
                        gveExportador.WriteXlsxToResponse()
                    Case "csv"
                        gvDatos.SettingsDetail.ExportMode = CType(System.Enum.Parse(GetType(GridViewDetailExportMode), valor), GridViewDetailExportMode)
                        gveExportador.WriteCsvToResponse()
                End Select
            End If
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de exportar datos. ", "Reporte Inventario Gerencial", ex)
        End Try
    End Sub

    Protected Sub gvDetalle_DataSelect(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Session("IdCliente") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
            CargarDetalle(TryCast(sender, ASPxGridView))
        Catch ex As Exception
            epNotificador.showError("Se presento un error al consultar el detalle de la venta: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarPermisosOpcionesRestringidas()
        Try
            Dim dtPermisos As DataTable = HerramientasGenerales.ObtenerInfoPermisosOpcionesRestringidas()
            Session("dtInfoPermisosOpcRestringidas") = dtPermisos
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar información de permisos sobre opciones restringidas. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarListadoDeClientesRegistrados(Optional ByVal forzarConsulta As Boolean = False)
        Dim dtDatos As DataTable = Nothing
        Try
            If Session("listaBusqueda") Is Nothing OrElse forzarConsulta Then
                Dim infoCliente As New ClienteFinalColeccion
                With infoCliente
                    If cmbCiudad.Value > 0 Then .IdCiudad = cmbCiudad.Value
                    If Not String.IsNullOrEmpty(txtIdentificacion.Text) Then .NumeroIdentificacion = txtIdentificacion.Text.Trim
                    If cmbEstrategia.Value > 0 Then .IdEstrategia = cmbEstrategia.Value
                    If deFechaInicio.Date > Date.MinValue Then .FechaInicio = deFechaInicio.Date
                    If deFechaFin.Date > Date.MinValue Then .FechaFin = deFechaFin.Date
                    dtDatos = .GenerarDataTable()
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

    Public Function EsVisibleOpcionRestringida(ByVal nombreControl As String) As Boolean
        Dim dtPermisos As DataTable
        dtPermisos = HerramientasGenerales.ObtenerInfoPermisosOpcionesRestringidas()
        If dtPermisos IsNot Nothing Then
            Dim idPerfil As Integer = Session("idPerfil")
            Dim idCiudadUsuario As Integer = 0

            Dim dvPermiso As DataView = dtPermisos.Copy().DefaultView
            dvPermiso.RowFilter = "nombreControl = '" & nombreControl & "' And idPerfil = " & idPerfil.ToString

            If dvPermiso.Count > 0 Then
                Return True
            Else
                Return False
            End If

        Else
            Return False
        End If
    End Function

    Private Sub CargaInicial()
        Try
            ' *** Se cargan las ciudades
            Dim infoCiudad As New CiudadColeccion
            With infoCiudad
                .IdPais = Enumerados.CodigoPais.Colombia
            End With
            CargarComboDX(cmbCiudad, CType(infoCiudad.GenerarDataTable, DataTable), "IdCiudad", "CiudadDepartamento")

            'Se carga el listado de Asesores Comerciales
            'Dim listaAsesor As New AsesorComercialColeccion
            'With listaAsesor
            '    .IdEstado = Enumerados.EstadoBinario.Activo
            'End With
            'CargarComboDX(cmbAsesor, CType(listaAsesor.GenerarDataTable, DataTable), "IdUsuarioSistema", "NombreAsesor")
            'cmbAsesor.SelectedIndex = -1
            'cmbAsesor.Enabled = True
            'CargarComboDX(CmbAsesorEdicion, CType(listaAsesor.GenerarDataTable, DataTable), "IdUsuarioSistema", "NombreAsesor")
            'CmbAsesorEdicion.SelectedIndex = -1

            'Se Carga la lista de clientes registrados sin gestio efectiva
            'CargarListadoDeClientesRegistrados(True)

            ' Se carga el listado de estrategias
            Dim listaEstrategia As New EstrategiaComercialColeccion
            With listaEstrategia
                .IdEstado = Enumerados.EstadoBinario.Activo
            End With
            CargarComboDX(cmbEstrategia, CType(listaEstrategia.GenerarDataTable, DataTable), "IdEstrategia", "Nombre")

            'Dim listaCausal As New ResultadoProcesoTipoVentaColeccion
            'CargarComboDX(cmbCausal, CType(listaCausal.GenerarDataTable, DataTable), "IdTipoVenta", "TipoVenta")

        Catch ex As Exception
            epNotificador.showError("Se presento un error en la carga inicial: " & ex.Message)
        End Try
    End Sub

    Private Function ValidaPerfil() As Boolean
        Dim resultado As Boolean = False

        Dim LisUsuarios As New UsuarioSistemaColeccion
        With LisUsuarios
            .IdUsuario = CInt(Session("userId"))
            .CargarDatos()
        End With
        Session("idPerfil") = LisUsuarios(0).IdPerfil

        Dim _callcenter As New ConfigValues("PERFILES_SUPERIORES_CALLCENTER")
        Dim strCallcenter() As String
        strCallcenter = _callcenter.ConfigKeyValue.Split(",")
        For i As Integer = 0 To strCallcenter.Count - 1
            If strCallcenter(i) = Session("idPerfil") Then
                resultado = True
                Exit For
            End If
        Next
        Return resultado
    End Function

    Private Sub CargarDetalle(gv As ASPxGridView)
        If Session("IdCliente") IsNot Nothing Then
            Dim dtDetalle As New DataTable
            Dim idCliente As Long = CLng(Session("IdCliente"))
            dtDetalle = ObtenerDetalle(idCliente)
            Session("dtDetalle") = dtDetalle
            With gv
                .DataSource = Session("dtDetalle")
            End With
        Else
            epNotificador.showWarning("No se pudo establecer el identificador del cliente, por favor intente nuevamente.")
        End If
    End Sub

    Private Function ObtenerDetalle(ByVal idCliente As Long) As DataTable
        Dim dtResultado As New DataTable
        Dim objGestiones As New GestionDeVentaColeccion
        Try
            With objGestiones
                .ListIdCliente.Add(idCliente)
                dtResultado = .GenerarDataTable()
            End With
        Catch ex As Exception
            epNotificador.showError("Se presento un error al cargar los servicios: " & ex.Message)
        End Try
        Return dtResultado
    End Function

#End Region

End Class