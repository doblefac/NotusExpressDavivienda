Imports NotusExpressBusinessLayer
Imports DevExpress.Web

Public Class AdministracionTipificacionNovedades
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Administración Tipificación de Novedades")
            Session.Remove("listaNovedades")
            ObtenerListaCausalGenerica()
        End If
        If Not Me.IsPostBack OrElse gvListaNovedades.IsCallback Then CargarListadoDeNovedadesRegistradas()
    End Sub

    Private Sub ObtenerListaCausalGenerica()
        Try
            Dim infoCausal As New CausalGenericaColeccion
            infoCausal.CargarDatos()
            With cbCausalGenerica
                .DataSource = infoCausal
                .TextField = "descripcion"
                .ValueField = "idCausal"
                .DataBindItems()
            End With
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de causales genéricas. ")
        End Try
    End Sub

    Private Sub CargarListadoDeNovedadesRegistradas(Optional ByVal forzarConsulta As Boolean = False)
        Dim listaNovedad As NovedadServicioColeccion = Nothing

        Try
            If Session("listaNovedades") Is Nothing OrElse forzarConsulta Then
                listaNovedad = New NovedadServicioColeccion
                With listaNovedad
                    If Not String.IsNullOrEmpty(txtFiltroNombre.Text) Then .Nombre = txtFiltroNombre.Text
                    If cbFiltroEstado.Value IsNot Nothing Then .IdEstado = CByte(cbFiltroEstado.Value)
                    .CargarDatos()
                End With
                Session("listaNovedades") = listaNovedad
            Else
                listaNovedad = CType(Session("listaNovedades"), NovedadServicioColeccion)
            End If

            With gvListaNovedades
                .DataSource = listaNovedad
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Novedades.")
        End Try
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            Select Case e.Parameter
                Case "filtrarDatos"
                    CargarListadoDeNovedadesRegistradas(True)
                Case "limpiarFiltros"
                    txtFiltroNombre.Text = ""
                    cbFiltroEstado.SelectedIndex = -1
                    CargarListadoDeNovedadesRegistradas(True)
                    CType(sender, ASPxCallbackPanel).JSProperties("cpLimpiar") = True
                Case "registrarDatos"
                    RegistrarNuevaNovedad()

            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        rpFiltros.Enabled = Not pucNuevoCanal.ShowOnPageLoad
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub RegistrarNuevaNovedad()
        Try
            Dim obj As New NovedadServicio
            Dim resultado As General.ResultadoProceso
            pucNuevoCanal.ShowOnPageLoad = True
            With obj
                .Nombre = txtNombreNovedad.Text.Trim
                .IdCausal = cbCausalGenerica.Value
                resultado = .Registrar()
                Select Case resultado.Valor
                    Case 0
                        epNotificador.showSuccess("La Novedad: " & txtNombreNovedad.Text.Trim & " fue registrada satisfactoriamente.")
                        CargarListadoDeNovedadesRegistradas(True)
                        pucNuevoCanal.ShowOnPageLoad = False
                    Case Else
                        epNotificador.showError(resultado.Mensaje)
                End Select
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de registrar nueva novedad.")
        End Try
    End Sub

    Private Sub gvListaNovedades_DataBinding(sender As Object, e As System.EventArgs) Handles gvListaNovedades.DataBinding
        If Session("listaNovedades") Is Nothing Then
        Else
            gvListaNovedades.DataSource = Session("listaNovedades")
        End If
    End Sub

    Private Sub gvListaNovedades_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gvListaNovedades.RowUpdating
        Try
            epNotificador.clear()
            CType(sender, ASPxGridView).JSProperties.Clear()
            Dim obj As New NovedadServicio(CInt(e.Keys("IdNovedad")))
            With obj
                Dim resultado As General.ResultadoProceso
                If obj.Registrado Then
                    If ObtenerTexto(e.OldValues("Nombre")) <> ObtenerTexto(e.NewValues("Nombre")) Then _
                        .Nombre = ObtenerTexto(e.NewValues("Nombre"))
                    If ObtenerTexto(e.OldValues("Causal")) <> ObtenerTexto(e.NewValues("Causal")) Then _
                        .IdCausal = ObtenerTexto(e.NewValues("Causal"))
                    If ObtenerTexto(e.OldValues("Estado")) <> ObtenerTexto(e.NewValues("Estado")) Then _
                        .IdEstado = CByte(e.NewValues("Estado"))
                    .IdCreador = Session("userId")
                    resultado = .Actualizar()
                    Select Case resultado.Valor
                        Case 0
                            epNotificador.showSuccess("La novedad fue actualizada satisfactoriamente.")
                            CargarListadoDeNovedadesRegistradas(True)
                        Case 100
                            epNotificador.showError("Error al Actualizar Datos")
                        Case Else
                            epNotificador.showError(resultado.Mensaje)
                    End Select
                Else
                    epNotificador.showWarning("No existe un registro en la base de datos con el identificador seleccionado. Por favor intente nuevamente")
                End If
            End With
            gvListaNovedades.CancelEdit()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de actualizar datos.")
        Finally
            e.Cancel = True
            CType(sender, ASPxGridView).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End Try
    End Sub

    Private Sub gvListaNovedades_StartRowEditing(sender As Object, e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles gvListaNovedades.StartRowEditing
        epNotificador.clear()
        Dim combo As GridViewDataComboBoxColumn = CType(gvListaNovedades.Columns("Estado"), GridViewDataComboBoxColumn)
        Dim dt As New DataTable

        With dt.Columns
            .Add("IdEstado", GetType(Byte))
            .Add("Estado", GetType(String))
        End With
        dt.Rows.Add("1", "Activo")
        dt.Rows.Add("0", "Inactivo")

        combo.PropertiesComboBox.DataSource = dt

        Dim comboc As GridViewDataComboBoxColumn = CType(gvListaNovedades.Columns("Causal"), GridViewDataComboBoxColumn)
        Dim infoCausal As New CausalGenericaColeccion
        infoCausal.CargarDatos()
        comboc.PropertiesComboBox.DataSource = infoCausal
        
    End Sub
End Class