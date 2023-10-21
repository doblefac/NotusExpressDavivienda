Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports DevExpress.Web
Imports System.Collections.Generic

Public Class EditarCampaniaFinanciero
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private _idCampania As Integer

#End Region

#Region "Eventos"

    Private Sub EditarCampaniaFinanciero_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Session("dtServicios") IsNot Nothing Then
            lbServicios.DataSource = Session("dtServicios")
            lbServicios.DataBind()
        End If
        If Session("dtBodegas") IsNot Nothing Then
            lbBodegas.DataSource = Session("dtBodegas")
            lbBodegas.DataBind()
        End If
        If Session("dtProductos") IsNot Nothing Then
            lbProductoExt.DataSource = Session("dtProductos")
            lbProductoExt.DataBind()
        End If
        If Session("dtDocumentos") IsNot Nothing Then
            lbDocumentos.DataSource = Session("dtDocumentos")
            lbDocumentos.DataBind()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        miEncabezado.clear()
        If Not IsPostBack Then
            If Request.QueryString("idCampania") IsNot Nothing Then Integer.TryParse(Request.QueryString("idCampania").ToString, _idCampania)
            If _idCampania > 0 Then
                With miEncabezado
                    .setTitle("Modificar Campaña Servicio Financiero")
                End With
                Session("idCampania") = _idCampania
                CargaInicial()
                CargarDetalleCampaña()
            Else
                miEncabezado.showWarning("Imposible recuperar el identificador del servicio. Por favor retorne a la página anterior.")
            End If
        Else
            If Session("idCampania") IsNot Nothing Then _idCampania = Session("idCampania")
        End If
    End Sub

    Private Sub cpRegistro_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpRegistro.Callback
        Dim resultado As New ResultadoProceso
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Actualizar"
                    resultado = ActualizarCampania()
            End Select
        Catch ex As Exception
            miEncabezado.showError("Se presento un error al procesar el Callback: " & ex.Message)
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargaInicial()
        Try

            Dim objCampania As New Campania(idCampania:=_idCampania)
            With objCampania
                txtNombreCampania.Text = .Nombre
                dateFechaInicio.Value = .FechaInicio
                dateFechaInicio.MinDate = .FechaInicio
                If .FechaFin <> Date.MinValue Then dateFechaFin.Value = .FechaFin
                cbActivo.Checked = .Activo
                cmbCl.Value = .IdClienteExterno
                txtMetaCliente.Text = .MetaCliente
                txtMetaCall.Text = .MetaCallcenter
                chkCargueUsuariosFueraBase.Checked = .CrearClienteFueraBase
                cmbTipoCampania.Value = .IdTipoCampania
                If .FechaLlegada.Trim <> "" Then
                    rblPersonalizacion.Items.Item(0).Selected = True
                Else
                    rblPersonalizacion.Items.Item(0).Selected = False
                End If
                dateFechaLlegada.Value = .FechaLlegada
                chkCargueUsuariosFueraBase.Checked = .CrearClienteFueraBase
            End With

            'Se cargan los tipos de servicio
            With objCampania
                .EsFinanciero = True
                Session("dtServicios") = .ObtenerServicios()
            End With
            With lbServicios
                .DataSource = Session("dtServicios")
                .DataBind()
            End With

            With objCampania
                Session("dtTipoCampania") = .ObtenerTipoCampania
            End With
            CargarComboDX(cmbTipoCampania, CType(Session("dtTipoCampania"), DataTable), "idTipoCampania", "tipoCampania")

            'Se cargan las bodegas
            With objCampania
                Session("dtBodegas") = .ObtenerBodegas()
            End With
            With lbBodegas
                .DataSource = Session("dtBodegas")
                .DataBind()
            End With

            ' Se cargan los productos
            With objCampania
                Session("dtProductos") = .ObtenerProductosComerciales({ .IdClienteExterno})
            End With
            With lbProductoExt
                .DataSource = Session("dtProductos")
                .DataBind()
            End With

            'Se cargan los documentos (Productos Financieros)
            With objCampania
                Session("dtDocumentos") = .ObtenerDocumentos()
            End With
            With lbDocumentos
                .DataSource = Session("dtDocumentos")
                .DataBind()
            End With

            With objCampania
                .EsFinanciero = True
                Session("dtCliente") = .ObtenerClienteExterno()
            End With
            CargarComboDX(cmbCl, CType(Session("dtCliente"), DataTable), "idClienteExterno", "nombre")

        Catch ex As Exception
            miEncabezado.showError("Se generó un error al intentar realizar la carga inicial: " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDetalleCampaña()
        Try
            Dim dt As DataTable
            '---------------------------------------------------------
            'Servicios
            dt = Nothing
            Dim objServicioCampania As New ServicioCampaniaColeccion
            With objServicioCampania
                .IdCampania = Session("idCampania")
                dt = .GenerarDataTable

            End With
            For i As Integer = 0 To dt.Rows.Count - 1
                For a As Integer = 0 To lbServicios.Items.Count - 1
                    If lbServicios.Items(a).Value = dt.Rows(i).Item("IdTipoServicio") Then
                        lbServicios.Items(a).Selected = True
                        Exit For
                    End If
                Next
            Next

            '---------------------------------------------------------
            'Bodegas
            dt = Nothing
            Dim objBodegaCampania As New BodegaCampaniaColeccion
            With objBodegaCampania
                .IdCampania = Session("idCampania")
                dt = .CargarDatos
            End With
            For i As Integer = 0 To dt.Rows.Count - 1
                For a As Integer = 0 To lbBodegas.Items.Count - 1
                    If lbBodegas.Items(a).Value = dt.Rows(i).Item("IdBodega") Then
                        lbBodegas.Items(a).Selected = True
                        Exit For
                    End If
                Next
            Next
            '---------------------------------------------------------
            'Productos
            dt = Nothing
            Dim objProductoCampania As New ProductoCampaniaColeccion
            With objProductoCampania
                .IdCampania = Session("idCampania")
                dt = .CargarDatos
            End With
            For i As Integer = 0 To dt.Rows.Count - 1
                For a As Integer = 0 To lbProductoExt.Items.Count - 1
                    If lbProductoExt.Items(a).Value = dt.Rows(i).Item("IdProductoComercial") Then
                        lbProductoExt.Items(a).Selected = True
                        Exit For
                    End If
                Next
            Next
            '---------------------------------------------------------
            'Documentos
            dt = Nothing
            Dim objDocumentoCampania As New DocumentoCampaniaColeccion
            With objDocumentoCampania
                .IdCampania = Session("idCampania")
                dt = .CargarDatos
            End With
            For i As Integer = 0 To dt.Rows.Count - 1
                For a As Integer = 0 To lbDocumentos.Items.Count - 1
                    If lbDocumentos.Items(a).Value = dt.Rows(i).Item("IdProducto") Then
                        lbDocumentos.Items(a).Selected = True
                        Exit For
                    End If
                Next
            Next
        Catch ex As Exception
            miEncabezado.showError("Se generó un error al intentar realizar la carga inicial: " & ex.Message)
        End Try
    End Sub

    Private Function ActualizarCampania() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miCampania As New Campania
        With miCampania
            .IdCampania = _idCampania
            .Nombre = txtNombreCampania.Text.Trim()
            .FechaInicio = dateFechaInicio.Date
            If dateFechaFin.Date <> Date.MinValue Then .FechaFin = dateFechaFin.Date
            .Activo = cbActivo.Checked
            If txtMetaCliente.Text.Trim = "" Or txtMetaCliente.Text = "0" Then
                .MetaCliente = 0
            Else
                .MetaCliente = CInt(txtMetaCliente.Text)
            End If
            If txtMetaCall.Text.Trim = "" Or txtMetaCall.Text = "0" Then
                .MetaCallcenter = 0
            Else
                .MetaCallcenter = CInt(txtMetaCall.Text)
            End If
            .IdTipoCampania = cmbTipoCampania.Value
            If rblPersonalizacion.Value = 1 Then
                .FechaLlegada = dateFechaLlegada.Value
            Else
                .FechaLlegada = ""
            End If
            If lbServicios.SelectedValues.Count > 0 Then
                .ListTiposDeServicio = New List(Of Integer)
                For servicio As Integer = 0 To lbServicios.SelectedValues.Count - 1
                    .ListTiposDeServicio.Add(lbServicios.SelectedValues(servicio))
                Next
            End If
            If lbBodegas.SelectedValues.Count > 0 Then
                .ListBodegas = New List(Of Integer)
                For bodega As Integer = 0 To lbBodegas.Items.Count - 1
                    If lbBodegas.Items(bodega).Selected Then
                        .ListBodegas.Add(lbBodegas.Items(bodega).Value)
                    End If
                Next
            End If
            If lbProductoExt.SelectedValues.Count > 0 Then
                .ListProductoExterno = New List(Of Integer)
                For prod As Integer = 0 To lbProductoExt.SelectedValues.Count - 1
                    .ListProductoExterno.Add(lbProductoExt.SelectedValues(prod))
                Next
            End If
            If lbDocumentos.SelectedValues.Count > 0 Then
                .ListDocumentoFinanciero = New List(Of Integer)
                For doc As Integer = 0 To lbDocumentos.SelectedValues.Count - 1
                    .ListDocumentoFinanciero.Add(lbDocumentos.SelectedValues(doc))
                Next
            End If
            .CrearClienteFueraBase = chkCargueUsuariosFueraBase.Checked
            .IdSistema = 2
            resultado = .ActualizarFinanciero()
            If resultado.Valor = 0 Then
                resultado = .SincronizarActualizacion()
                If resultado.Valor = 0 Then
                    miEncabezado.showSuccess(resultado.Mensaje)
                    lblMensajeGenerico.Text = resultado.Mensaje
                    lblMensajeGenerico.Attributes.Add("style", "Color:darkGreen;font-size:160%")

                Else
                    miEncabezado.showWarning("Se Actualizo la campaña en NotusExpress exitosamente pero no se pudo sincronizar con NotusIls")
                    lblMensajeGenerico.Text = "Se Actualizó la campaña en NotusExpress exitosamente pero no se pudo sincronizar con NotusIls"
                    lblMensajeGenerico.Attributes.Add("style", "Color:darkGreen;font-size:160%")
                End If
            Else
                miEncabezado.showWarning(resultado.Mensaje)
                lblMensajeGenerico.Text = resultado.Mensaje
                lblMensajeGenerico.Attributes.Add("style", "Color:darkRed;font-size:160%")
            End If
            Dim objCampaña As New Campania()
            With objCampaña
                Session("dtBodegas") = .ObtenerBodegas()
            End With
            With lbBodegas
                .DataSource = Session("dtBodegas")
                .DataBind()
            End With
            Dim dt As DataTable
            Dim objBodegaCampania As New BodegaCampaniaColeccion
            With objBodegaCampania
                .IdCampania = Session("idCampania")
                dt = .GenerarDataTable
            End With
            For i As Integer = 0 To dt.Rows.Count - 1
                For a As Integer = 0 To lbBodegas.Items.Count - 1
                    If lbBodegas.Items(a).Value = dt.Rows(i).Item("IdBodega") Then
                        lbBodegas.Items(a).Selected = True
                        Exit For
                    End If
                Next
            Next

        End With
        Return resultado
    End Function

#End Region

End Class