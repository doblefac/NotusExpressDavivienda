Imports NotusExpressBusinessLayer
Imports DevExpress.Web

Public Class AdministrarCanales
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Administración de Canales")
            Session.Remove("listaMarcas")
        End If
        If Not Me.IsPostBack OrElse gvListaCanales.IsCallback Then CargarListadoDeCanalesRegistrados()
    End Sub

    Private Sub CargarListadoDeCanalesRegistrados(Optional ByVal forzarConsulta As Boolean = False)
        Dim listaCanal As CanalColeccion = Nothing

        Try
            If Session("listaMarcas") Is Nothing OrElse forzarConsulta Then
                listaCanal = New CanalColeccion
                With listaCanal
                    If Not String.IsNullOrEmpty(txtFiltroNombre.Text) Then .Nombre = txtFiltroNombre.Text
                    If cbFiltroEstado.Value IsNot Nothing Then .IdEstado = CByte(cbFiltroEstado.Value)
                    .CargarDatos()
                End With
                Session("listaMarcas") = listaCanal
            Else
                listaCanal = CType(Session("listaMarcas"), CanalColeccion)
            End If

            With gvListaCanales
                .DataSource = listaCanal
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Canales.")
        End Try
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            Select Case e.Parameter
                Case "filtrarDatos"
                    CargarListadoDeCanalesRegistrados(True)
                Case "limpiarFiltros"
                    txtFiltroNombre.Text = ""
                    cbFiltroEstado.SelectedIndex = -1
                    CargarListadoDeCanalesRegistrados(True)
                    CType(sender, ASPxCallbackPanel).JSProperties("cpLimpiar") = True
                Case "registrarDatos"
                    RegistrarNuevoCanal()

            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        rpFiltros.Enabled = Not pucNuevoCanal.ShowOnPageLoad
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub RegistrarNuevoCanal()
        Try
            Dim obj As New Canal
            Dim resultado As General.ResultadoProceso
            pucNuevoCanal.ShowOnPageLoad = True
            With obj
                .Nombre = txtNombreCanal.Text.Trim
                resultado = .Registrar()
                Select Case resultado.Valor
                    Case 0
                        epNotificador.showSuccess("El Canal: " & txtNombreCanal.Text.Trim & " fue registrada satisfactoriamente.")
                        CargarListadoDeCanalesRegistrados(True)
                        pucNuevoCanal.ShowOnPageLoad = False
                    Case Else
                        epNotificador.showError(resultado.Mensaje)
                End Select
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de registrar nueva marca.")
        End Try
    End Sub

    Private Sub gvListaCanales_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gvListaCanales.RowUpdating
        Try
            epNotificador.clear()
            CType(sender, ASPxGridView).JSProperties.Clear()
            Dim obj As New Canal(CInt(e.Keys("IdCanal")))
            With obj
                Dim resultado As General.ResultadoProceso
                If obj.Registrado Then
                    If ObtenerTexto(e.OldValues("Nombre")) <> ObtenerTexto(e.NewValues("Nombre")) Then _
                        .Nombre = ObtenerTexto(e.NewValues("Nombre"))
                    If ObtenerTexto(e.OldValues("Estado")) <> ObtenerTexto(e.NewValues("Estado")) Then _
                        .IdEstado = CByte(e.NewValues("Estado"))
                    .IdCreador = Session("userId")
                    resultado = .Actualizar()
                    Select Case resultado.Valor
                        Case 0
                            epNotificador.showSuccess("El Canal " & .Nombre.Trim & " fue actualizada satisfactoriamente.")
                            CargarListadoDeCanalesRegistrados(True)
                        Case 100
                            epNotificador.showError("Error al Actualizar Datos")
                        Case Else
                            epNotificador.showError(resultado.Mensaje)
                    End Select
                Else
                    epNotificador.showWarning("No existe un registro en la base de datos con el identificador seleccionado. Por favor intente nuevamente")
                End If
            End With
            gvListaCanales.CancelEdit()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de actualizar datos.")
        Finally
            e.Cancel = True
        End Try
    End Sub

    Private Sub gvListaCanales_StartRowEditing(sender As Object, e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles gvListaCanales.StartRowEditing
        Dim combo As GridViewDataComboBoxColumn = CType(gvListaCanales.Columns("Estado"), GridViewDataComboBoxColumn)
        Dim dt As New DataTable

        With dt.Columns
            .Add("IdEstado", GetType(Byte))
            .Add("Estado", GetType(String))
        End With
        dt.Rows.Add("1", "Activo")
        dt.Rows.Add("0", "Inactivo")

        combo.PropertiesComboBox.DataSource = dt
    End Sub
End Class