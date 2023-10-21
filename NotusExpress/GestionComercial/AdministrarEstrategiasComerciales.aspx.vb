Imports NotusExpressBusinessLayer
Imports DevExpress.Web

Public Class AdministrarEstrategiasComerciales
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Administración de Estrategias Comerciales")
            Session.Remove("listaMarcas")
            CargarCanales()
        End If
        If Not Me.IsPostBack OrElse gvListaEstrategias.IsCallback Then CargarListadoDeEstrategiasComercialesRegistradas()
    End Sub

    Private Sub CargarCanales()
        Try
            Dim lista As New FiltroCanalColeccion
            lista.CargarDatos()
            With cbFiltroCanal
                .DataSource = lista
                .TextField = "nombre"
                .ValueField = "idCanal"
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de canales")
        Finally
            'cbFiltroCanal.Items.Insert(0, New DevExpress.Web.ListEditItem("Seleccione un Canal", "0"))
        End Try
    End Sub

    Private Sub CargarCanalesRegistro()
        Try
            Dim lista As New FiltroCanalColeccion
            lista.CargarDatos()
            With cbCanal
                .DataSource = lista
                .TextField = "nombre"
                .ValueField = "idCanal"
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de canales")
        Finally
            'cbFiltroCanal.Items.Insert(0, New DevExpress.Web.ListEditItem("Seleccione un Canal", "0"))
        End Try
    End Sub

    Private Sub CargarListadoDeEstrategiasComercialesRegistradas(Optional ByVal forzarConsulta As Boolean = False)
        Dim listaEstrategia As EstrategiaComercialColeccion = Nothing

        Try
            If Session("listaMarcas") Is Nothing OrElse forzarConsulta Then
                listaEstrategia = New EstrategiaComercialColeccion
                With listaEstrategia
                    If Not String.IsNullOrEmpty(txtFiltroNombre.Text) Then .Nombre = txtFiltroNombre.Text
                    If cbFiltroEstado.Value IsNot Nothing Then .IdEstado = CByte(cbFiltroEstado.Value)
                    If cbFiltroCanal.Value IsNot Nothing Then .IdCanal = CByte(cbFiltroCanal.Value)
                    .CargarDatos()
                End With
                Session("listaMarcas") = listaEstrategia
            Else
                listaEstrategia = CType(Session("listaMarcas"), EstrategiaComercialColeccion)
            End If

            With gvListaEstrategias
                .DataSource = listaEstrategia
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Estrategias Comerciales.")
        End Try
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            Select Case e.Parameter
                Case "filtrarDatos"
                    CargarListadoDeEstrategiasComercialesRegistradas(True)
                Case "limpiarFiltros"

                    cbCanal.SelectedIndex = -1
                    CargarListadoDeEstrategiasComercialesRegistradas(False)
                    CType(sender, ASPxCallbackPanel).JSProperties("cpLimpiar") = True
                Case "registrarDatos"
                    RegistrarNuevaEstrategia()
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        rpFiltros.Enabled = Not pucNuevaEstrategia.ShowOnPageLoad
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub RegistrarNuevaEstrategia()
        Try
            Dim obj As New EstrategiaComercial
            Dim resultado As General.ResultadoProceso
            pucNuevaEstrategia.ShowOnPageLoad = True
            With obj
                .Nombre = txtNombreEstrategia.Text.Trim
                .IdCanal = cbCanal.Value
                resultado = .Registrar()
                Select Case resultado.Valor
                    Case 0
                        epNotificador.showSuccess("La estrategia: " & txtNombreEstrategia.Text.Trim & " fue registrada satisfactoriamente.")
                        'Aquí se guarda la relación entre pdv y estrategia
                        Dim idestrategia As Integer
                        Dim estrategia As New ObtenerIdEstrategia(0)
                        With estrategia
                            idestrategia = .IdEstrategiaCliente
                        End With
                        Dim seleccionado As Integer
                        For i = 0 To gvPdv.VisibleRowCount
                            'seleccionado = CInt(gvPdv.GetDataRow(i).Item(2).ToString())

                            If gvPdv.Selection.IsRowSelected(i) Then
                                seleccionado = CInt(gvPdv.GetRowValues(i, "IdPdv").ToString)
                                Dim registraEstrategia As New RegistrarEstrategiaPorPdv
                                With registraEstrategia
                                    .IdEstrategia = idestrategia
                                    .IdPdv = seleccionado
                                    .IdUsuarioRegistra = Session("userId")
                                    .Registrar()
                                End With
                            End If
                        Next
                        '////////////////////////////////////
                        CargarListadoDeEstrategiasComercialesRegistradas(True)
                        pucNuevaEstrategia.ShowOnPageLoad = False
                    Case Else
                        epNotificador.showError(resultado.Mensaje)
                End Select
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de registrar nueva estrategia comercial.")
        End Try
    End Sub

    Private Sub gvListaEstrategias_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gvListaEstrategias.RowUpdating
        Try
            epNotificador.clear()
            CType(sender, ASPxGridView).JSProperties.Clear()
            Dim obj As New Canal(CInt(e.Keys("IdEstrategia")))
            With obj
                Dim resultado As General.ResultadoProceso
                If obj.Registrado Then
                    If ObtenerTexto(e.OldValues("Nombre")) <> ObtenerTexto(e.NewValues("Nombre")) Then _
                        .Nombre = ObtenerTexto(e.NewValues("Nombre"))
                    If ObtenerTexto(e.OldValues("Estado")) <> ObtenerTexto(e.NewValues("Estado")) Then _
                        .IdEstado = CByte(e.NewValues("Estado"))
                    resultado = .Actualizar()
                    Select Case resultado.Valor
                        Case 0
                            epNotificador.showSuccess("La marca " & .Nombre.Trim & " fue actualizada satisfactoriamente.")
                            CargarListadoDeEstrategiasComercialesRegistradas(True)
                        Case 100
                            epNotificador.showError("Error al Actualizar Datos")
                        Case Else
                            epNotificador.showError(resultado.Mensaje)
                    End Select
                Else
                    epNotificador.showWarning("No existe un registro en la base de datos con el identificador seleccionado. Por favor intente nuevamente")
                End If
            End With
            gvListaEstrategias.CancelEdit()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de actualizar datos.")
        Finally
            e.Cancel = True
        End Try
    End Sub

    Private Sub gvListaEstrategias_StartRowEditing(sender As Object, e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles gvListaEstrategias.StartRowEditing
        Dim combo As GridViewDataComboBoxColumn = CType(gvListaEstrategias.Columns("Estado"), GridViewDataComboBoxColumn)
        Dim dt As New DataTable

        With dt.Columns
            .Add("IdEstado", GetType(Byte))
            .Add("Estado", GetType(String))
        End With
        dt.Rows.Add("1", "Activo")
        dt.Rows.Add("0", "Inactivo")

        combo.PropertiesComboBox.DataSource = dt
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        CargarCanalesRegistro()
        CargarListadoDePdv()
    End Sub

    Private Sub CargarListadoDePdv(Optional ByVal forzarConsulta As Boolean = False)
        Dim listaPdv As PuntosDeVentaECColeccion = Nothing

        Try
            listaPdv = New PuntosDeVentaECColeccion
            With listaPdv
                .CargarDatos()
            End With
            Session("listaMarcas") = listaPdv
            listaPdv = CType(Session("listaMarcas"), PuntosDeVentaECColeccion)

            With gvPdv
                .DataSource = listaPdv
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Puntos de Venta.")
        End Try
    End Sub

    Private Sub gvPdv_DataBinding(sender As Object, e As System.EventArgs) Handles gvPdv.DataBinding
        gvPdv.DataSource = Session("listaMarcas")
    End Sub

    Protected Sub gvDetalle_DataSelect(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim idEstrategia As Long = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()

            Dim objDetalleEstrategia As New PuntosDeVentasPorEstrategiaColeccion()
            With objDetalleEstrategia
                .IdEstrategia = idEstrategia
                .CargarDatos()
                TryCast(sender, ASPxGridView).DataSource = objDetalleEstrategia.GenerarDataTable()
            End With
        Catch ex As Exception
            miEncabezado.showError("Se presento un error al cargar el detalle de la Distribución. " & ex.Message)
        End Try
    End Sub
End Class