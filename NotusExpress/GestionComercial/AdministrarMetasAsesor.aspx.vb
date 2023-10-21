Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.ConfiguracionComercial
Imports NotusExpressBusinessLayer.RecursoHumano
Imports DevExpress.Web
Imports System.IO
Imports GemBox
Imports GemBox.Spreadsheet

Public Class AdministrarMetasAsesor
    Inherits System.Web.UI.Page


#Region "Atributos"

    Private oExcel As ExcelFile
    Private Shared listaCampania As CampaniaColeccion
    Private Shared listaAsesores As AsesorComercialColeccion

    Private Const UploadDirectory As String = "~\ArchivosCargados\"
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        miEncabezado.clear()
        If Not Me.IsPostBack Then
            miEncabezado.setTitle("Administración de Metas Comerciales - Por Asesor")
            Session.Remove("listaMetas")
            Session.Remove("tablaErrores")
            Session.Remove("listaAsesores")
            CargarLicenciaGembox()
            CargarListadoDeAsesores(cbFiltroAsesor)
            CargarListadoDeAsesores(cbAsesor)
            CargarListadoCampanias(cbFiltroCampania)
            CargarListadoCampanias(cbCampania)
            CargarListadoDeMeses(cbMes)
        End If
        If cpGeneral.IsCallback Then
            CargarListadoDeAsesores(cbAsesor)
            CargarListadoDeMeses(cbMes)
        End If

        If Not Me.IsPostBack OrElse gvListaMetas.IsCallback Then CargarListadoDeMetasComercialesRegistradas()

    End Sub

    Private Sub CargarListadoCampanias(ByRef combo As ASPxComboBox)
        Try
            If listaCampania Is Nothing Then
                listaCampania = New CampaniaColeccion
                With listaCampania
                    .Activo = Enumerados.EstadoBinario.Activo
                    .CargarDatos()
                End With
            End If
            With combo
                .DataSource = listaCampania
                .TextField = "Nombre"
                .ValueField = "IdCampania"
                .SelectedIndex = -1
                .DataBind()
            End With
        Catch ex As Exception
            miEncabezado.showError("Error al tratar de cargar el listado de Campañas")
        End Try
    End Sub

    Private Sub CargarListadoDeAsesores(ByRef combo As ASPxComboBox, Optional ByVal forzarConsulta As Boolean = False)
        Try
            If listaAsesores Is Nothing OrElse forzarConsulta Then
                listaAsesores = New AsesorComercialColeccion
                With listaAsesores
                    .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                    .IdEstado = 1
                    .CargarDatos()
                End With
            End If

            With combo
                .DataSource = listaAsesores
                .TextField = "NombreAsesor"
                .ValueField = "IdPersona"
                .SelectedIndex = -1
                .DataBind()
            End With
        Catch ex As Exception
            miEncabezado.showError("Error al tratar de cargar el listado de Asesores")
        End Try
    End Sub

    Private Sub CargarListadoDeMeses(ByRef combo As ASPxComboBox)
        Try
            Dim dt As New DataTable

            With dt.Columns
                .Add("IdMes", GetType(Integer))
                .Add("Mes", GetType(String))
            End With
            With dt.Rows
                .Add("1", "Enero")
                .Add("2", "Febrero")
                .Add("3", "Marzo")
                .Add("4", "Abril")
                .Add("5", "Mayo")
                .Add("6", "Junio")
                .Add("7", "Julio")
                .Add("8", "Agosto")
                .Add("9", "Septiembre")
                .Add("10", "Octubre")
                .Add("11", "Noviembre")
                .Add("12", "Diciembre")
            End With

            With combo
                .DataSource = dt
                .TextField = "Mes"
                .ValueField = "IdMes"
                .SelectedIndex = -1
                .DataBind()
            End With
        Catch ex As Exception
            miEncabezado.showError("Error al tratar de cargar el listado de meses")
        End Try
    End Sub

    Private Sub CargarListadoDeMetasComercialesRegistradas(Optional ByVal forzarConsulta As Boolean = False)
        Dim listaMetas As ConfiguracionComercial.MetaComercialAsesorColeccion = Nothing

        Try
            listaMetas = New ConfiguracionComercial.MetaComercialAsesorColeccion
            With listaMetas
                If cbFiltroAsesor.Value IsNot Nothing Then .IdPersonaAsesor = cbFiltroAsesor.Value
                If cbFiltroCampania.Value IsNot Nothing Then .IdCampania = cbFiltroCampania.Value
                .CargarDatos()
            End With
            Session("listaMetas") = listaMetas

            With gvListaMetas
                .DataSource = listaMetas
                .DataBind()
            End With
        Catch ex As Exception
            miEncabezado.showError("Error al tratar de cargar el listado de Metas Comerciales.")
        End Try
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            miEncabezado.clear()
            Select Case e.Parameter
                Case "filtrarDatos"
                    CargarListadoDeMetasComercialesRegistradas(True)
                Case "limpiarFiltros"
                    cbFiltroAsesor.SelectedIndex = -1
                    cbFiltroCampania.SelectedIndex = -1
                    CargarListadoDeMetasComercialesRegistradas(True)
                    CType(sender, ASPxCallbackPanel).JSProperties("cpLimpiar") = True
                Case "registrarDatos"
                    RegistrarNuevaMeta()
            End Select
        Catch ex As Exception
            miEncabezado.showError("Error al tratar de manejar CallBack.")
        End Try
        CargarListadoDeAsesores(cbFiltroAsesor)
        CargarListadoDeAsesores(cbAsesor)
        CargarListadoCampanias(cbFiltroCampania)
        CargarListadoCampanias(cbCampania)
        CargarListadoDeMeses(cbMes)
        rpFiltros.Enabled = Not pucNuevaMeta.ShowOnPageLoad
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
    End Sub

    Private Sub gvListaMetas_DataBinding(sender As Object, e As System.EventArgs) Handles gvListaMetas.DataBinding
        gvListaMetas.DataSource = Session("listaMetas")
    End Sub

    Private Sub cbFormatoExportar_ButtonClick(source As Object, e As DevExpress.Web.ButtonEditClickEventArgs) Handles cbFormatoExportar.ButtonClick
        Try
            Dim formato As String = cbFormatoExportar.Value
            If Not String.IsNullOrEmpty(formato) Then
                With gveExportador
                    .FileName = "ReporteMetasConfiguradas"
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
            miEncabezado.showError("Error al tratar de exportar datos. " & ex.ToString)
        End Try
    End Sub

    Private Sub RegistrarNuevaMeta()
        Try
            Dim obj As New ConfiguracionComercial.MetaComercialAsesor
            Dim resultado As General.ResultadoProceso
            pucNuevaMeta.ShowOnPageLoad = True
            With obj
                .Meta = CInt(txtNombreMeta.Text.Trim)
                .Anio = CInt(txtAnio.Text.Trim)
                .Mes = CInt(cbMes.Value)
                .IdCampania = CInt(cbCampania.Value)
                .IdAsesor = CInt(cbAsesor.Value)
                .IdUsuarioRegistra = CInt(Session("userId"))
                resultado = .Registrar()
                Select Case resultado.Valor
                    Case 0
                        miEncabezado.showSuccess("La meta fue registrada satisfactoriamente.")
                        pucNuevaMeta.ShowOnPageLoad = False
                        CargarListadoDeMetasComercialesRegistradas(True)
                    Case Else
                        Select Case resultado.Valor
                            Case 1, 200, 300
                                miEncabezado.showWarning(resultado.Mensaje)
                            Case Else
                                miEncabezado.showError(resultado.Mensaje)
                        End Select
                End Select
            End With
        Catch ex As Exception
            miEncabezado.showError("Error al tratar de registrar nueva meta comercial. " & ex.ToString)
        End Try
    End Sub

    Private Sub gvListaMetas_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gvListaMetas.RowUpdating
        Try
            miEncabezado.clear()
            CType(sender, ASPxGridView).JSProperties.Clear()
            Dim obj As New ConfiguracionComercial.MetaComercialAsesor(CInt(e.Keys("IdMeta")))
            With obj
                Dim resultado As General.ResultadoProceso
                If obj.Registrado Then
                    If (ObtenerTexto(e.OldValues("Meta")) <> ObtenerTexto(e.NewValues("Meta"))) Or (e.OldValues("Estado") <> e.NewValues("Estado")) Then
                        .Meta = ObtenerTexto(e.NewValues("Meta"))
                        .Estado = e.NewValues("Estado")
                        .IdMeta = e.Keys("IdMeta")
                        resultado = .Actualizar()
                        CType(sender, ASPxGridView).JSProperties("cpResultado") = 0
                        Select Case resultado.Valor
                            Case 0
                                miEncabezado.showSuccess("La Meta fue actualizada satisfactoriamente.")
                                CargarListadoDeMetasComercialesRegistradas(True)
                            Case 200, 300
                                miEncabezado.showWarning(resultado.Mensaje)
                            Case Else
                                miEncabezado.showError(resultado.Mensaje)
                        End Select
                    Else
                        miEncabezado.showWarning("No se realiza la actualización, puesto que no se cambiaron datos")
                    End If
                Else
                    miEncabezado.showWarning("No existe un registro en la base de datos con el identificador seleccionado. Por favor intente nuevamente")
                End If
            End With
            gvListaMetas.CancelEdit()
        Catch ex As Exception
            miEncabezado.showError("Error al tratar de actualizar datos. " & ex.Message)
        Finally
            CType(sender, ASPxGridView).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
            e.Cancel = True
        End Try
    End Sub

End Class