Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.Comunes
Imports System.Collections.Generic
Imports DevExpress.Web
Imports System.Web.Services
Imports System.Web.Script.Serialization

Public Class AdministracionCampaniaFinanciero
    Inherits System.Web.UI.Page

#Region "Eventos"

    Private Sub AdministracionCampaniaFinanciero_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        If Session("dtServicios") IsNot Nothing Then
            lbServicios.DataSource = Session("dtServicios")
            lbServicios.DataBind()
            CargarComboDX(cmbTipoServicio, CType(Session("dtServicios"), DataTable), "idTipoServicio", "nombre")
        End If
        If Session("dtBodegas") IsNot Nothing Then
            lbBodegas.DataSource = Session("dtBodegas")
            lbBodegas.DataBind()
            CargarComboDX(cmbCiudad, CType(Session("dtCiudades"), DataTable), "idCiudad", "Ciudad")
        End If
        If Session("dtProductos") IsNot Nothing Then
            lbProductoExt.DataSource = Session("dtProductos")
            lbProductoExt.DataBind()
        End If
        If Session("dtDocumentos") IsNot Nothing Then
            lbDocumentos.DataSource = Session("dtDocumentos")
            lbDocumentos.DataBind()
        End If
        If Session("dtCliente") IsNot Nothing Then
            CargarComboDX(cmbCliente, CType(Session("dtCliente"), DataTable), "idClienteExterno", "nombre")
            CargarComboDX(cmbCl, CType(Session("dtCliente"), DataTable), "idClienteExterno", "nombre")
            cmbCliente.Value = CInt(Session("idClienteExterno"))
            cmbCl.Value = CInt(Session("idClienteExterno"))

        End If
        dateFechaInicio.MinDate = Now.AddDays(-1)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        miEncabezado.clear()
        Try
            If Not IsPostBack Then
                With miEncabezado
                    .setTitle("Administración de Campañas Financiero")
                End With
                CargaInicial()
            End If
        Catch ex As Exception
            miEncabezado.showError("Se generó un error al cargar la página: " & ex.Message)
        End Try
    End Sub

    Private Sub cpRegistro_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpRegistro.Callback
        Dim resultado As New ResultadoProceso
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Registrar"
                    resultado = RegistrarCampania()
                    CargarComboDX(cmbTipoCampania, CType(Session("dtTipoCampania"), DataTable), "idTipoCampania", "tipoCampania")
            End Select
        Catch ex As Exception
            miEncabezado.showError("Se presento un error al procesar el Callback: " & ex.Message)
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = resultado.Mensaje
        CType(sender, ASPxCallbackPanel).JSProperties("cpValor") = resultado.Valor

    End Sub

    <WebMethod>
    Public Shared Function ConsultaAutocomplete(ByVal operacion, ByVal filtroBusqueda) As String
        Dim miCampania As New Campania
        Dim dsDataCodigo As New DataSet
        Dim dtDatos As New DataTable

        dsDataCodigo = miCampania.ConsultaAutocomplete(operacion, filtroBusqueda)
        dtDatos = dsDataCodigo.Tables(0)



        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))()
        Dim row As Dictionary(Of String, Object)
        For Each dr As DataRow In dtDatos.Rows
            row = New Dictionary(Of String, Object)()
            For Each col As DataColumn In dtDatos.Columns
                row.Add(col.ColumnName, dr(col))
            Next
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)

    End Function

    <WebMethod>
    Public Shared Function GuardarNuevoCodEstrategia(ByVal nuevoCodigo) As String
        Dim miCampania As New Campania
        Dim resultado As New ResultadoProceso
        resultado = miCampania.GuardarNuevoCodigoEstrategia(nuevoCodigo)

        Return resultado.Mensaje
    End Function

    <WebMethod>
    Public Shared Function ActualizarCodigoEstrategia(ByVal codigoAnterior, ByVal codigoNuevo) As String
        Dim miCampania As New Campania
        Dim resultado As New ResultadoProceso
        resultado = miCampania.ActualizarCodigoEstrategia(codigoAnterior, codigoNuevo)

        Return resultado.Mensaje
    End Function

    <WebMethod>
    Public Shared Function EliminarCodigoEstrategia(ByVal codigoEliminar) As String
        Dim miCampania As New Campania
        Dim resultado As New ResultadoProceso
        resultado = miCampania.EliminarCodigoEstrategia(codigoEliminar)

        Return resultado.Mensaje
    End Function

    Protected Sub Link_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim idSistemaOrigen As Integer
        Try
            Dim linkVer As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkVer.NamingContainer, GridViewDataItemTemplateContainer)

            linkVer.ClientSideEvents.Click = linkVer.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)
            idSistemaOrigen = CInt(gvCampanias.GetRowValuesByKeyValue(templateContainer.KeyValue, "IdSistema"))
            Dim lnkMdifica As ASPxHyperLink = templateContainer.FindControl("lnkEditar")

            If idSistemaOrigen = Enumerados.Sistema.NotusExpress Then
                lnkMdifica.Visible = True
            Else
                lnkMdifica.Visible = False
            End If
        Catch ex As Exception
            miEncabezado.showError("No fué posible establecer los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub

    Private Sub gvCampanias_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvCampanias.CustomCallback
        BuscarCampanias()
        CType(sender, ASPxGridView).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
    End Sub

    Private Sub gvCampanias_DataBinding(sender As Object, e As System.EventArgs) Handles gvCampanias.DataBinding
        gvCampanias.DataSource = Session("dtDatosCampanias")
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargaInicial()
        Try
            Dim clienteExterno As New ConfigValues("IDCLIENTEEXTERNO")
            If (clienteExterno Is Nothing) Then
                miEncabezado.showError("No fué posible Encontra comfiguracion de empresa en ConfigValues IDCLIENTEEXTERNO Notificar a IT ")
                Exit Sub
            Else
                Session("idClienteExterno") = clienteExterno.ConfigKeyValue
            End If

            Dim objCampaña As New Campania()
            With objCampaña
                .EsFinanciero = True
                .IdClienteExterno = CInt(Session("idClienteExterno"))
                Session("dtCliente") = .ObtenerClienteExterno()
            End With
            CargarComboDX(cmbCl, CType(Session("dtCliente"), DataTable), "idClienteExterno", "nombre")
            cmbCl.Value = CInt(Session("idClienteExterno"))
            With objCampaña
                Session("dtTipoCampania") = .ObtenerTipoCampania
            End With
            CargarComboDX(cmbTipoCampania, CType(Session("dtTipoCampania"), DataTable), "idTipoCampania", "tipoCampania")

            'Se cargan los tipos de servicio
            With objCampaña
                .EsFinanciero = True
                Session("dtServicios") = .ObtenerServicios()
            End With
            With lbServicios
                .DataSource = Session("dtServicios")
                .DataBind()
            End With

            'Se cargan las bodegas
            With objCampaña
                Session("dtBodegas") = .ObtenerBodegas()
            End With
            With lbBodegas
                .DataSource = Session("dtBodegas")
                .DataBind()
            End With

            ' Se cargan los productos
            If (clienteExterno Is Nothing) Then
                miEncabezado.showError("No fué posible Encontra comfiguracion de empresa en ConfigValues IDCLIENTEEXTERNO Notificar a IT ")
                Exit Sub
            End If
            With objCampaña
                Dim idCliente As Integer() = {clienteExterno.ConfigKeyValue}
                Session("dtProductos") = .ObtenerProductosComerciales(idCliente)
            End With
            With lbProductoExt
                .DataSource = Session("dtProductos")
                .DataBind()
            End With

            'Se cargan los documentos (Productos Financieros)
            With objCampaña
                Session("dtDocumentos") = .ObtenerDocumentos()
            End With
            With lbDocumentos
                .DataSource = Session("dtDocumentos")
                .DataBind()
            End With
            '---------------------------------------------------------------------------------------------------------------
            ' Se cargarn las ciudaddes
            Dim objCiudad As New Localizacion.CiudadColeccion
            Session("dtCiudades") = objCiudad.GenerarDataTable
            CargarComboDX(cmbCiudad, CType(Session("dtCiudades"), DataTable), "idCiudad", "ciudadDepartamento")
            'Carga los tipos de servicio
            CargarComboDX(cmbTipoServicio, CType(Session("dtServicios"), DataTable), "idTipoServicio", "nombre")
            ''Carga los tipos de cliente
            CargarComboDX(cmbCliente, CType(Session("dtCliente"), DataTable), "idClienteExterno", "nombre")
            '---------------------------------------------------------------------------------------------------------------
            cmbCliente.Value = CInt(Session("idClienteExterno"))
        Catch ex As Exception
            miEncabezado.showError("No fué posible realizar la carga inicial: " & ex.Message)
        End Try
    End Sub

    Private Function RegistrarCampania() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miCampania As New Campania

        With miCampania
            .Nombre = txtNombreCampania.Text.Trim()
            .FechaInicio = dateFechaInicio.Date
            If dateFechaFin.Date <> Date.MinValue Then .FechaFin = dateFechaFin.Date
            .Activo = True
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
            End If
            If lbServicios.SelectedValues.Count > 0 Then
                .ListTiposDeServicio = New List(Of Integer)
                For servicio As Integer = 0 To lbServicios.SelectedValues.Count - 1
                    .ListTiposDeServicio.Add(lbServicios.SelectedValues(servicio))
                Next
            End If
            If lbBodegas.SelectedValues.Count > 0 Then
                .ListBodegas = New List(Of Integer)
                For bodega As Integer = 0 To lbBodegas.SelectedValues.Count - 1
                    .ListBodegas.Add(lbBodegas.SelectedValues(bodega))
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
            .IdClienteExterno = CInt(cmbCl.Value)
            .IdSistema = 2
            resultado = .Sincronizar()
            If resultado.Valor > 0 Then
                .IdCampania = resultado.Valor
                resultado = .RegistrarFinanciero()
                If resultado.Valor = 0 Then

                    Dim operacion As New ResultadoProceso
                    Dim codigosConjunto As String = hdfCodigosEstrategia.Value
                    Dim codigoSplit As String() = codigosConjunto.Split("|")
                    Dim palabra As String
                    Dim idCampania As Integer = Integer.Parse(resultado.Mensaje.Split(".")(1).Replace(" ", ""))

                    For Each palabra In codigoSplit
                        Dim codEstrategia = palabra
                        operacion = miCampania.AsociarEstrategiaCampania(idCampania, codEstrategia)
                    Next
                    miEncabezado.showSuccess(resultado.Mensaje)
                Else
                    miEncabezado.showWarning("Se creó la campaña en NotusIls exitosamente pero no se pudo sincronizar con NotusExpress")
                End If

                LimpiarCampos()

            Else
                miEncabezado.showWarning(resultado.Mensaje)
            End If
        End With
        Return resultado
    End Function

    Private Sub BuscarCampanias()
        Try
            Dim objCampanias As New CampaniaColeccion()
            With objCampanias
                If Not String.IsNullOrEmpty(txtFiltroCampania.Text.Trim) Then .NombreCampania = txtFiltroCampania.Text.Trim
                If rblEstado.Value = 1 Then
                    .Activo = True
                Else
                    .Activo = False
                End If
                If cmbCliente.Value > 0 Then .IdClienteExterno = cmbCliente.Value
                If cmbTipoServicio.Value > 0 Then .ListaTipoServicio.Add(cmbTipoServicio.Value)
                If cmbCiudad.Value > 0 Then .ListaIdCiudad.Add(cmbCiudad.Value)
            End With

            With gvCampanias
                .DataSource = objCampanias.GenerarDataTable()
                Session("dtDatosCampanias") = .DataSource
                .DataBind()
            End With
        Catch ex As Exception
            miEncabezado.showError("Se generó un error al intentar realizar la búsqueda: " & ex.Message)
        End Try
    End Sub

    Private Sub LimpiarCampos()
        txtNombreCampania.Text = String.Empty
        cmbCl.Value = CInt(Session("idClienteExterno"))
        dateFechaInicio.Value = String.Empty
        dateFechaFin.Value = String.Empty
        txtMetaCliente.Text = String.Empty
        txtMetaCall.Text = String.Empty
        cmbTipoCampania.SelectedIndex = -1
        chkCargueUsuariosFueraBase.Checked = False
        lbServicios.Value = String.Empty
        lbBodegas.Value = String.Empty
        lbProductoExt.Value = String.Empty
        lbDocumentos.Value = String.Empty
    End Sub

#End Region

End Class