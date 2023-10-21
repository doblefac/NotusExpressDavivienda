Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.MaestroProductos
Imports System.String

Public Class ConsultaVentaPresencial
    Inherits System.Web.UI.Page

#Region "Atributos"
    Private _continuar As Boolean = False
#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                miEncabezado.setTitle("Consulta Ventas Presenciales")
                CargarTipoDocumento()
                CargarEmpresas()
                CargarCiudades()
                CargarDatosCategoria()
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "toggle", "IniciarToggle();", True)
            If gluDocumentos.IsCallback OrElse gluDocumentos.GridView.IsCallback OrElse Not Me.IsPostBack Then CargarDocumentos()
        Catch ex As Exception
            miEncabezado.showError("Se presento un error al cargar la pagina: " & ex.Message)
        End Try

    End Sub

    Protected Sub gvDetalle_DataSelect(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Session("idDetalle") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
            CargarDetalle(TryCast(sender, ASPxGridView))
        Catch ex As Exception
            miEncabezado.showError("Se presento un error al consultar el detalle de la venta: " & ex.Message)
        End Try
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Dim resultado As New ResultadoProceso
        Try
            CType(sender, ASPxCallbackPanel).JSProperties.Remove("cpLimpiarFiltros")
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "ConsultarCliente"
                    CargarDatosCliente()
                Case "Eliminar"
                    resultado = EliminarServicios(arrayAccion(1))
                    If resultado.Valor <> 0 Then
                        ValidarControlesPerfil()
                        miEncabezado.showError(resultado.Mensaje)
                    Else
                        miEncabezado.showSuccess(resultado.Mensaje)
                    End If
                    CType(sender, ASPxCallbackPanel).JSProperties("cpRespuesta") = 0
                Case "CerrarProductos"
                    resultado = RegistrarServicios()
                    If resultado.Valor = 0 Then
                        miEncabezado.showSuccess(resultado.Mensaje)
                    ElseIf resultado.Valor = 99 Then
                        miEncabezado.clear()
                    Else
                        miEncabezado.showError(resultado.Mensaje)
                    End If
                    CType(sender, ASPxCallbackPanel).JSProperties("cpRespuesta") = 0
                    CType(sender, ASPxCallbackPanel).JSProperties("cpRespuestaProducto") = 0
                Case "Actualizar"
                    resultado = ActualizarInformacionCliente()
                    If resultado.Valor <> 0 Then
                        miEncabezado.showError(resultado.Mensaje)
                    Else
                        miEncabezado.showSuccess(resultado.Mensaje)
                    End If
                    CType(sender, ASPxCallbackPanel).JSProperties("cpRespuesta") = 0
            End Select
        Catch ex As Exception
            miEncabezado.showError("Ocurrio un error al generar el registro: " & ex.Message)
            resultado.Valor = 1
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
    End Sub

    Protected Sub Link_Init(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim lnkEliminar As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(lnkEliminar.NamingContainer, GridViewDataItemTemplateContainer)
            lnkEliminar.ClientSideEvents.Click = lnkEliminar.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)
            If Not CType(Session("perfilBackOffice"), Boolean) Then
                lnkEliminar.ClientVisible = False
            Else
                lnkEliminar.ClientVisible = True
            End If
        Catch ex As Exception
            miEncabezado.showError("No fué posible establecer los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub

    Private Sub pcServicio_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles pcServicio.WindowCallback
        Dim resultado As New ResultadoProceso
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Servicios"
                    CargarDatosServicio()
                    cmbProductos.Items.Clear()
                Case "Productos"
                    CargarDatosServicio()
                    If arrayAccion(1) = "1" Then
                        txtCupo2.Text = txtCupo1.Text
                    End If
                    CargarDatosProducto(arrayAccion(1))
                    CargarDocumentos()
                    For i As Integer = 0 To gluDocumentos.GridView.VisibleRowCount - 1
                        If gluDocumentos.GridView.Selection.IsRowSelected(i) Then
                            gluDocumentos.GridView.Selection.SetSelection(i, False)
                        End If
                    Next
                Case "Primas"
                    CargarDatosServicio()
                    CargarDatosProducto(cmbTipoServicio.Value)
                    CargarDocumentos()
                    cargarPrimas()
                Case "RegistroServicioTemporal"
                    resultado = RegistrarServiciosTemporales()
                    CargarDatosServicio()
                    CargarDatosProducto(cmbTipoServicio.Value)
                    If resultado.Valor <> 0 Then
                        miEncabezado.showError(resultado.Mensaje)
                    Else
                        miEncabezado.showSuccess(resultado.Mensaje)
                    End If
            End Select
        Catch ex As Exception
            miEncabezado.showError("Ocurrio un error al generar el registro: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Metodos privados"

    Private Sub CargarTipoDocumento()
        Dim dtTipo As New DataTable
        dtTipo = HerramientasGenerales.ConsultarTipoIdentificacion
        Session("tipoDocumento") = dtTipo
        With cbTipoId
            .DataSource = dtTipo
            .ValueField = "idTipo"
            .TextField = "descripcion"
            .DataBind()
        End With
    End Sub

    Private Sub CargarEmpresas()
        Dim listaEmpresa As New EmpresasColeccion
        If Session("listaEmpresa") Is Nothing Then
            listaEmpresa = New EmpresasColeccion
            With listaEmpresa
                .IdTipoEmpresa = 4
                .CargarDatos()
            End With
            Session("listaEmpresa") = listaEmpresa
        Else
            listaEmpresa = CType(Session("listaEmpresa"), EmpresasColeccion)
        End If
        With cmbEmpresa
            .DataSource = listaEmpresa
            .ValueField = "IdEmpresa"
            .TextField = "Nombre"
            .DataBind()
        End With
        Session("dtEmpresas") = listaEmpresa.GenerarDataTable
    End Sub

    Private Sub CargarCiudades()
        Dim listaCiudad As CiudadColeccion
        If Session("listaCiudad") Is Nothing Then
            listaCiudad = New CiudadColeccion
            With listaCiudad
                .CargarDatos()
            End With
            Session("listaCiudad") = listaCiudad
        Else
            listaCiudad = CType(Session("listaCiudad"), CiudadColeccion)
        End If
        With cmbCiudad1
            .DataSource = listaCiudad
            .ValueField = "IdCiudad"
            .TextField = "CiudadDepartamento"
            .DataBind()
        End With
    End Sub

    Private Sub CargarDatosCliente()
        ConsultarCliente(txtNumeroDocumento.Text.Trim)
        If _continuar Then
            MostrarDatos(txtNumIdentificacion.Text.Trim, CInt(Session("userId").ToString.Trim))
            txtNombres.Focus()
        End If
    End Sub

    Private Sub CargarDatosCategoria()
        Dim listaCategoria As CategoriaCreditoColeccion
        If Session("listaCategoria") Is Nothing Then
            listaCategoria = New CategoriaCreditoColeccion
            With listaCategoria
                .IdEstado = True
                .CargarDatos()
            End With
            Session("listaCategoria") = listaCategoria
        Else
            listaCategoria = CType(Session("listaCategoria"), CategoriaCreditoColeccion)
        End If
        With cmbCategoria
            .DataSource = listaCategoria
            .ValueField = "IdCategoria"
            .TextField = "Categoria"
            .DataBind()
        End With
        cmbProductos.Items.Clear()
        Session("listaServicio") = Nothing
    End Sub

    Private Sub ConsultarCliente(ByVal numeroidentificacion As String)
        'se consulta la info del cliente
        Dim infoClienteFinal As New ClienteFinal(numeroidentificacion)
        'limpiarControles()
        With infoClienteFinal
            _continuar = True
            If .Nombres IsNot Nothing Then
                If (.Nombres.Split(" ").Length) > 1 Then
                    'se separa el nombre
                    Dim nombres As String
                    nombres = .NombreApellido
                    Dim words As String() = nombres.Split(New Char() {" "c})
                    Dim i As Integer
                    For i = 0 To words.Length - 1
                        If (words.Length - 1) - i = 0 Then
                            txtSegundoApellido.Text = words(i)
                        ElseIf (words.Length - 1) - i = 1 Then
                            txtPrimerApellido.Text = words(i)
                        Else
                            If txtNombres.Text = "" Then
                                txtNombres.Text = words(i)
                            Else
                                txtNombres.Text = txtNombres.Text & " " & words(i)
                            End If

                        End If
                    Next
                Else
                    txtNombres.Text = .Nombres
                    txtPrimerApellido.Text = .PrimerApellido
                    txtSegundoApellido.Text = .SegundoApellido
                End If
            Else
                txtNombres.Text = ""
                txtPrimerApellido.Text = ""
                txtSegundoApellido.Text = ""
            End If
            hflidcliente.Value = .IdCliente
            txtNumIdentificacion.Text = .NumeroIdentificacion.ToString
            If .IdTipoIdentificacion > 0 Then
                cbTipoId.Value = .IdTipoIdentificacion
            Else
                cbTipoId.SelectedIndex = -1
            End If
            If .DireccionResidencia Is System.DBNull.Value Or .DireccionResidencia = "" Then
                txtDireccion.Text = ""
            Else
                txtDireccion.Text = .DireccionResidencia
            End If
            If .IdCiudadResidencia > 0 Then
                cmbCiudad1.Value = .IdCiudadResidencia
            Else
                cmbCiudad1.SelectedIndex = -1
            End If
            txtTelFijo.Text = .TelefonoResidencia
            txtTelefono2.Text = .TelefonoAdicional
            txtCelular.Text = .Celular
            If .Sexo IsNot Nothing Then
                cmbSexo.Value = .Sexo
            Else
                cmbSexo.SelectedIndex = -1
            End If
            txtEmail.Text = .Email
            txtIngresos.Text = CInt(.IngresoAproximado)
            Dim dr() As DataRow
            dr = CType(Session("dtEmpresas"), DataTable).Select("idEmpresa=" & .IdEmpresa)
            If dr.Length > 0 Then
                cmbEmpresa.Value = .IdEmpresa
            Else
                cmbEmpresa.SelectedIndex = -1
            End If
            If .IdCliente > 0 Then
                If .IdTipoIdentificacion > 0 Then
                    cbTipoId.Value = .IdTipoIdentificacion
                End If
            End If
            If .ResultadoUbica <> "" Then
                cmbUbica.Text = .ResultadoUbica
            Else
                cmbUbica.SelectedIndex = -1
            End If

            If .ResultadoEvidente <> "" Then
                cmbEvidente.Text = .ResultadoEvidente
            Else
                cmbEvidente.SelectedIndex = -1
            End If

            If .ResultadoDataCredito <> "" Then
                cmbDataCredito.Text = .ResultadoDataCredito
            Else
                cmbDataCredito.SelectedIndex = -1
            End If

            txtUbica.Text = .NumConsultaUbica
            txtEvidente.Text = .NumConsultaEvidente
            txtDataCredito.Text = .NumConsultaDataCredito
            txtCupo1.Text = .ValorPreaprobadoDataCredito
            txtObservacionOperadorCall.Text = .ObservacionesCallCenter
            Session("idCliente") = .IdCliente
            Session("idGestionVenta") = .IdGestionVenta
            If .ResultadoDataCredito = "Positivo" And .ResultadoUbica = "Positivo" And .ResultadoEvidente = "Positivo" Then
                imgAgrega.ClientVisible = True
                cmbResultadoProceso.Text = "Aprobado"
            Else
                imgAgrega.ClientVisible = False
                cmbResultadoProceso.Text = "Negado"
            End If
            Dim _perfilBackOffice As Boolean = ValidaPerfil()
            Session("perfilBackOffice") = _perfilBackOffice
            ValidarControlesPerfil()
        End With
    End Sub

    Public Sub MostrarDatos(ByVal identificacionCliente As String, ByVal idUsuario As Integer)
        Dim miRegistro As New GestionVentaCallCenter
        Dim dtResultado As New DataTable
        If CType(Session("idGestionVenta"), Integer) > 0 Then
            With miRegistro
                .IdGestionVenta = Session("idGestionVenta")
                dtResultado = .ConsultarServiciosGestionVenta
            End With

            With gvDatosIniciales
                .PageIndex = 0
                .DataSource = dtResultado
                .DataBind()
            End With
        End If
        ValidarControlesPerfil()
    End Sub

    Private Sub CargarDetalle(gv As ASPxGridView)
        If Session("idDetalle") IsNot Nothing Then
            Dim dtDetalle As New DataTable
            Dim idDetalle As Long = CLng(Session("idDetalle"))
            dtDetalle = ObtenerDetalle(idDetalle)
            Session("dtDetalle") = dtDetalle
            With gv
                .DataSource = Session("dtDetalle")
            End With
        Else
            miEncabezado.showWarning("No se pudo establecer el identificador del producto, por favor intente nuevamente.")
        End If
    End Sub

    Private Function ObtenerDetalle(ByVal idRegistro As Long) As DataTable
        Dim dtResultado As New DataTable
        Dim objGestiones As New Documento
        Try
            With objGestiones
                .IdRegistro = idRegistro
                dtResultado = .obtenerDocumentosProductosGestionVenta
            End With
        Catch ex As Exception
            miEncabezado.showError("Se presento un error al cargar los servicios: " & ex.Message)
        End Try
        Return dtResultado
    End Function

    Private Function ValidaPerfil() As Boolean
        Dim resultado As Boolean = False

        Dim LisUsuarios As New UsuarioSistemaColeccion
        With LisUsuarios
            .IdUsuario = CInt(Session("userId"))
            .CargarDatos()
        End With
        Session("idPerfil") = LisUsuarios(0).IdPerfil

        Dim _BackOffice As New ConfigValues("PERFILBACKOFFICE")
        Dim strBackOffice() As String
        strBackOffice = _BackOffice.ConfigKeyValue.Split(",")
        For i As Integer = 0 To strBackOffice.Count - 1
            If strBackOffice(i) = Session("idPerfil") Then
                resultado = True
                Exit For
            End If
        Next
        Return resultado
    End Function

    Private Function EliminarServicios(ByVal idDetalle As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miRegistro As New GestionVentaCallCenter
        With miRegistro
            .IdUsuarioAsesor = Session("userId")
            resultado = .EliminarServiciosGestionVenta(idDetalle)
        End With
        If resultado.Valor = 0 Then
            MostrarDatos(txtNumIdentificacion.Text.Trim, CInt(Session("userId").ToString.Trim))
        End If
        Return resultado
    End Function

    Private Sub CargarDatosServicio()
        Dim listaServicio As TipoProductoColeccion
        listaServicio = New TipoProductoColeccion
        With listaServicio
            .VentaPresencial = Enumerados.EstadoBinario.Activo
            .IdCategoria = cmbCategoria.Value
            Session("listaServicio") = listaServicio.GenerarDataTableServicioCategoria
        End With
        With cmbTipoServicio
            .DataSource = CType(Session("listaServicio"), DataTable)
            .ValueField = "IdTipoProducto"
            .TextField = "Nombre"
            .DataBind()
        End With
    End Sub

    Private Sub CargarDatosProducto(ByVal idTipoProducto As Integer)
        '**Carga Productos
        Dim listaProductos As ProductoColeccion
        listaProductos = New ProductoColeccion
        With listaProductos
            .VentaPresencial = Enumerados.EstadoBinario.Activo
            .IdTipoProducto = cmbTipoServicio.Value
            .IdCategoria = cmbCategoria.Value
            .CargarDatos()
        End With
        Session("listaProductos") = listaProductos
        cmbProductos.Items.Clear()

        With cmbProductos
            .DataSource = listaProductos
            .ValueField = "IdProducto"
            .TextField = "NombreProducto"
            .SelectedIndex = -1
            .DataBind()
        End With
        If listaProductos.Count = 0 Then
            cmbProductos.Items.Insert(0, New ListEditItem("No tiene Productos", Nothing))
            If cmbProductos.SelectedIndex = -1 Then cmbProductos.SelectedIndex = 0
            cmbProductos.Enabled = False
        Else
            cmbProductos.Enabled = True
        End If
    End Sub

    Private Sub cargarPrimas()
        '**Cargar Primas
        Dim miValorPrima As New ValorPrimaServicioColeccion
        Dim dtPrima As New DataTable
        With miValorPrima
            .IdTipoProducto = cmbTipoServicio.Value
            .IdProducto = cmbProductos.Value
            dtPrima = .GenerarDataTable
        End With
        If dtPrima.Rows.Count = 0 Then
            cmbPrima.Enabled = False
            cmbPrima.Items.Insert(0, New ListEditItem("No existen valores", Nothing))
        Else
            cmbPrima.Enabled = True
            CargarComboDX(cmbPrima, dtPrima, "IdValorPrimaServicio", "ValorPrimaServicio")
        End If
        cmbPrima.SelectedIndex = -1
    End Sub

    Private Sub CargarDocumentos()
        '**Carga Documentos
        Dim infoDocumentos As New DocumentoColeccion
        Dim dtDocumentos As DataTable
        With infoDocumentos
            .IdCategoria = cmbCategoria.Value
            .IdTipoProducto = cmbTipoServicio.Value
            .VentaPresencial = Enumerados.EstadoBinario.Activo
            .IdEstado = Enumerados.EstadoBinario.Activo
            dtDocumentos = .GenerarDataTablePresencial
            Session("dtDocumentos") = dtDocumentos
        End With
        'lbDocumentos.Items.Clear()
        'With lbDocumentos
        '    .DataSource = Session("dtDocumentos")
        '    .DataBind()
        'End With


        With gluDocumentos
            .DataSource = Session("dtDocumentos")
            .DataBind()
        End With
    End Sub

    Private Function RegistrarServiciosTemporales() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miRegistro As New GestionVentaCallCenter
        Dim ListDocumento As New List(Of Integer)

        Dim listaDoc As List(Of Object) = gluDocumentos.GridView().GetSelectedFieldValues("IdDocumento")

        If listaDoc.Count > 0 Then
            For doc As Integer = 0 To listaDoc.Count - 1
                ListDocumento.Add(listaDoc(doc))
            Next
        End If

        With miRegistro
            .IdGestionVenta = Session("idGestionVenta")
            .IdProducto = cmbProductos.Value
            .IdUsuarioAsesor = CInt(Session("userId").ToString.Trim)
            .IdentificacionCliente = txtNumIdentificacion.Text.Trim
            .ListDocumentoPresencialList = ListDocumento
            .Observaciones = mmObservaciones.Text.Trim
            If cmbPrima.Value > 0 Then
                .IdValorPrima = cmbPrima.Value
            Else
                .IdValorPrima = 0
            End If
            If CInt(txtCupo2.Text) > 0 Then
                .ValorPreaprobadocliente = CInt(txtCupo2.Text)
            Else
                .ValorPreaprobadocliente = 0
            End If
            resultado = .RegistrarServiciosTransitorios
        End With
        If resultado.Valor = 0 Then
            MostrarDatos(txtNumIdentificacion.Text.Trim, CInt(Session("userId").ToString.Trim))
            limpiarControlesServicio()
        Else
            miEncabezado.showError(resultado.Mensaje)
        End If
        Return resultado
    End Function

    Private Sub limpiarControlesServicio()
        cmbCategoria.SelectedIndex = -1
        cmbTipoServicio.SelectedIndex = -1
        cmbProductos.SelectedIndex = -1
        cmbPrima.SelectedIndex = -1
        mmObservaciones.Text = ""
        Session("dtDocumentos") = Nothing
        With gluDocumentos
            .DataSource = Session("dtDocumentos")
            .DataBind()
        End With
        For i As Integer = 0 To gluDocumentos.GridView.VisibleRowCount - 1
            If gluDocumentos.GridView.Selection.IsRowSelected(i) Then
                gluDocumentos.GridView.Selection.SetSelection(i, False)
            End If
        Next
        txtCupo2.Text = "0"
    End Sub

    Private Function RegistrarServicios() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miRegistro As New GestionVentaCallCenter
        With miRegistro
            .IdGestionVenta = Session("idGestionVenta")
            .IdUsuarioAsesor = CInt(Session("userId").ToString.Trim)
            .IdentificacionCliente = txtNumIdentificacion.Text.Trim
            resultado = .RegistrarServiciosGestionVenta
        End With
        If resultado.Valor = 0 Then
            MostrarDatos(txtNumIdentificacion.Text.Trim, CInt(Session("userId").ToString.Trim))
        ElseIf resultado.Valor = 99 Then
            miEncabezado.clear()
        Else
            miEncabezado.showError(resultado.Mensaje)
        End If
        Session("dtServiciosTemporales") = Nothing
        Return resultado
    End Function

    Private Sub ValidarControlesPerfil()
        If Not CType(Session("perfilBackOffice"), Boolean) Then
            rpDatosCliente.Enabled = False
            imgAgrega.ClientVisible = False
            imgActualiza.ClientVisible = False
        Else
            rpDatosCliente.Enabled = True
            imgAgrega.ClientVisible = True
            imgActualiza.ClientVisible = True
        End If
    End Sub

    Private Function ActualizarInformacionCliente() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim infoClienteFinal As New ClienteFinal(txtNumIdentificacion.Text.Trim)
        Dim miRegistro As New GestionVentaCallCenter

        With infoClienteFinal
            If cbTipoId.Value <> .IdTipoIdentificacion Or txtNumIdentificacion.Text <> .NumeroIdentificacion _
                Or txtNombres.Text.Trim <> .Nombres Or txtPrimerApellido.Text.Trim <> .PrimerApellido Or txtSegundoApellido.Text.Trim <> .SegundoApellido _
                Or txtDireccion.Text.Trim <> .DireccionResidencia Or cmbCiudad1.Value <> .IdCiudadResidencia Or txtTelFijo.Text <> .TelefonoResidencia _
                Or txtTelefono2.Text <> .TelefonoAdicional Or txtCelular.Text <> .Celular Or txtEmail.Text.Trim <> .Email Or cmbSexo.Value <> .Sexo _
                Or Int(txtIngresos.Text) <> .IngresoAproximado Or cmbEmpresa.Value <> .IdEmpresa Or cmbUbica.Text.Trim <> .ResultadoUbica _
                Or txtUbica.Text <> .NumConsultaUbica Or cmbEvidente.Text.Trim <> .ResultadoEvidente Or txtEvidente.Text <> .NumConsultaEvidente _
                Or cmbDataCredito.Text.Trim <> .ResultadoDataCredito Or txtDataCredito.Text <> .NumConsultaDataCredito _
                Or Int(txtCupo1.Text) <> .ValorPreaprobadoDataCredito Or txtObservacionOperadorCall.Text <> .ObservacionesCallCenter Then

                With miRegistro
                    .IdCliente = Integer.Parse(hflidcliente.Value)
                    .IdTipoIdentificacion = cbTipoId.Value
                    .IdentificacionCliente = txtNumIdentificacion.Text
                    .NombresCliente = txtNombres.Text.Trim
                    .PrimerApellido = txtPrimerApellido.Text.Trim
                    .SegundoApellido = txtSegundoApellido.Text.Trim
                    .DireccionResidencia = txtDireccion.Text.Trim
                    .IdCiudad = cmbCiudad1.Value
                    .TelefonoResidencia = txtTelFijo.Text
                    .TelefonoAdicional = txtTelefono2.Text
                    .Celular = txtCelular.Text
                    .Email = txtEmail.Text.Trim
                    .Sexo = cmbSexo.Value
                    .IngresosAproximados = Int(txtIngresos.Text)
                    .IdEmpresa = cmbEmpresa.Value
                    .ResultadoUbica = cmbUbica.Text.Trim
                    .NumConsultaUbica = txtUbica.Text
                    .ResultadoEvidente = cmbEvidente.Text.Trim
                    .NumConsultaEvidente = txtEvidente.Text
                    .ResultadoDataCredito = cmbDataCredito.Text.Trim
                    .NumConsultaDataCredito = txtDataCredito.Text
                    .ValorPreaprobadoDataCredito = Int(txtCupo1.Text)
                    .Observaciones = txtObservacionOperadorCall.Text
                    .IdUsuarioAsesor = CInt(Session("userId").ToString.Trim)
                    .IdGestion = Session("idGestionVenta")
                    resultado = .ActualizarInformacionClienteVentaPresencial
                    If resultado.Valor <> 0 Then
                        miEncabezado.showError(resultado.Mensaje)
                    End If
                End With
            Else
                resultado.Mensaje = "No existen cambios de informacion del cliente para registrar"
                resultado.Valor = 1
            End If
        End With
        ValidarControlesPerfil()
        Return resultado
    End Function

#End Region

End Class