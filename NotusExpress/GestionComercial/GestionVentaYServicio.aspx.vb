Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.General
Imports DevExpress.Web
Imports NotusExpressBusinessLayer.GestionComercial
Imports System.Net

Public Class GestionVentaYServicio
    Inherits System.Web.UI.Page

#Region "Atributos"
    Private _continuar As Boolean = False
    Private _visibleAgregarCliente As Boolean = False
    Private _mensajeGenerico As String = String.Empty
    Private _idCampaniaCliente As Integer = 0
    Private IntervaloDias As Integer
    Private diasBogota As Integer
    Private diasCiudades As Integer
#End Region
#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.IsPostBack Then
                Dim fecha As Date
                fecha = DateAdd(DateInterval.Year, -18, DateTime.Now.Date)

                Session.Remove("dtDisponibilidadEntrega")
                Session.Remove("IdProducto")
                Session.Remove("sinRealce")
                Session.Remove("IdGestionVenta")
                Session.Remove("clienteFuera")

                epNotificador.setTitle("Gestión de Venta - Call Center")
                'limpiarControles(String.Empty)
                CargarInicial()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alerta", "LimpiaFormularioInicial();", True)
                If Request.QueryString("id") IsNot Nothing Then
                    btnCancelar.ClientVisible = False
                    btnCancelarPopUp.ClientVisible = True
                    cmbCampania.ClientEnabled = False
                    Session("IdGestionVenta") = CLng(Request.QueryString("id"))
                    Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=CLng(Session("IdGestionVenta")))
                    txtNumIdentificacion.Text = miGestion.NumeroIdentificacion
                    RpFechaAgendamiento.ClientVisible = True
                    CargarDatosCliente(False, True)
                End If

            End If
            If cmbCiudad1.IsCallback Then
                CargarInicial()
            End If
            If cpGeneral.IsCallback Then
                cmbCausal.DataBind()
                If cmbCausal.Value = 3 Then
                    RpFechaAgendamiento.ClientVisible = True
                    btnAgregar.ClientVisible = True
                Else
                    RpFechaAgendamiento.ClientVisible = False
                    btnAgregar.ClientVisible = False
                End If
                FechasAgendamiento()
            End If
        Catch ex As Exception
            _mensajeGenerico = "Se presentó un error al cargar la página: " & ex.Message & "|rojo"
            'mensajero.MostrarMensajePopUp("Se presento un error al cargar la página: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Error")
        End Try

    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Dim resultado As New ResultadoProceso
        Dim idGestion As Long
        Dim operacion As Boolean = False    'variable que impide que se cargue el agendamiento luego de guardar el nuevo cliente fuera de la base de cargue
        Try
            CType(sender, ASPxCallbackPanel).JSProperties.Remove("cpLimpiarFiltros")
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")

            Select Case arrayAccion(0)

                Case "ConsultarCliente"
                    LimpiarGrids()
                    ReiniciarCalendario()
                    If (arrayAccion(1) <> "undefined") Then
                        CargarDatosCliente(True)

                    Else
                        'cmbCampania.SelectedIndex = -1
                        CargarDatosCliente(False)
                    End If
                    resultado.Valor = 1
                Case "Registrar"
                    CargarInicial()
                    resultado = ValidarCiudad()
                    If resultado.Valor = 0 Then
                        resultado = Registrar()
                    End If
                    If resultado.Valor = 0 Then
                        epNotificador.showInfo(resultado.Mensaje)
                        idGestion = CLng((Session("IdGestionVenta")))
                        ReiniciarCalendario()
                        limpiarControles("NuevoCliente")
                    ElseIf resultado.Valor = 3 Then
                        txtObservacionOperadorCall.Text = String.Empty
                        epNotificador.showInfo(resultado.Mensaje)
                    ElseIf resultado.Valor = 10 Then
                        txtObservacionOperadorCall.Text = String.Empty
                        epNotificador.showInfo(resultado.Mensaje)
                        resultado.Valor = 1
                        limpiarControles("NuevoCliente")
                    Else
                        CargarInicial()
                        MostrarDatos(txtNumIdentificacion.Text.Trim, CInt(Session("userId").ToString.Trim))
                        _mensajeGenerico = resultado.Mensaje & "|rojo"
                    End If
                    cbTipoId.Focus()
                    ConsultaGestiones(txtNumIdentificacion.Text.Trim)
                Case "Mostrar"
                    MostrarDatos(txtNumIdentificacion.Text.Trim, CInt(Session("userId").ToString.Trim))
                    CargarInicial()
                    resultado.Valor = 1
                    cmbCampania.ClientEnabled = False
                    imgAgregar.ClientVisible = True
                    ValidarEdicionservicio()
                Case "Causal"

                    CargarCausales(arrayAccion(1))
                    CargarInicial()
                    cmbCausal.Focus()
                    cmbCausal.SelectedIndex = -1
                    cmbCampania.Enabled = False
                    resultado.Valor = 1
                    ValidarEdicionservicio()
                    'ConsultarCliente(txtNumIdentificacion.Value, True)
                    If _visibleAgregarCliente Then
                        imgAgregarCliente.ClientVisible = True
                        btnRegistra.ClientVisible = False
                        imgAgregar.ClientVisible = False
                    End If
                Case "RequiereProducto"
                    RequiereProducto(arrayAccion(1))
                    CargarInicial()
                    txtObservacionOperadorCall.Focus()
                    resultado.Valor = 1
                    If CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 1 Then
                        cbTipoId.Enabled = False
                        txtNombres.Enabled = False
                        txtPrimerApellido.Enabled = False
                        txtSegundoApellido.Enabled = False
                        txtDireccionResidencia.ClientEnabled = False
                        cmbCiudad1.ClientEnabled = False
                        cmbCampania.ClientEnabled = False
                        txtTelFijo.Enabled = False
                        txtCelular.Enabled = False
                        txtTelFijo.Enabled = False
                        txtEmail.Enabled = False
                        txtOficinaCliente.Enabled = False
                    Else
                        cbTipoId.Enabled = False
                        txtNombres.Enabled = False
                        txtPrimerApellido.Enabled = False
                        txtSegundoApellido.Enabled = False
                        cmbCampania.ClientEnabled = False
                    End If
                    'ConsultarCliente(txtNumIdentificacion.Value, True)
                    If _visibleAgregarCliente Then
                        imgAgregarCliente.ClientVisible = True
                        btnRegistra.ClientVisible = False
                        imgAgregar.ClientVisible = False
                    End If

                    FechasAgendamiento()
                    CargarInicial()

                Case "AgregarClienteFueraBase"
                    resultado = AgregarCLienteFueraBase()
                    CargarInicial()
                    operacion = True
                    _mensajeGenerico = resultado.Mensaje
                    If resultado.Valor = 2 Then
                        imgAgregarCliente.ClientVisible = True
                        btnRegistra.ClientVisible = False
                        cmbResultadoProceso.Enabled = False
                        cmbCausal.Enabled = False
                        Session.Remove("clienteFuera")
                    End If
                Case "ObtenerCapacidadFechaAgenda"
                    resultado = ObtenerCapacidadFechaAgenda()
                    CargarInicial()
                    cmbCampania.ClientEnabled = False
                Case "Ciudad"
                    ReiniciarCalendario()
                    FechasAgendamiento()
                    CargarInicial()
                    If Session("clienteFuera") = True Then
                        imgAgregarCliente.ClientVisible = True
                        _visibleAgregarCliente = True
                        btnRegistra.ClientVisible = False
                        imgAgregar.ClientVisible = False
                        cmbResultadoProceso.Enabled = False
                        cmbCausal.Enabled = False
                    End If
            End Select

        Catch ex As Exception
            _mensajeGenerico = "Ocurrió un error al generar el registro: " & ex.Message & "|rojo"
            'mensajero.MostrarMensajePopUp("Ocurrio un error al generar el registro: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Error")
            resultado.Valor = 1
        End Try
        If operacion = False Then
            CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
            CType(sender, ASPxCallbackPanel).JSProperties("cpResultado") = resultado.Valor
            CType(sender, ASPxCallbackPanel).JSProperties("cpGestion") = idGestion
        End If
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensajeGeneral") = _mensajeGenerico
    End Sub

    Private Sub cbCallBackId_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cbCallBackId.Callback
        CType(sender, ASPxCallbackPanel).JSProperties.Remove("cpLimpiarFiltros")

        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub pcServicio_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles pcServicio.WindowCallback
        Dim resultado As New ResultadoProceso
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Inicial"
                    CargarDatosServicio()
                    limpiarControlesServicio()
                    MostrarDatos(arrayAccion(1), CInt(Session("userId").ToString.Trim))
                    Session("IdentificacionUsuaio") = arrayAccion(1)
                    ValidarEdicionservicio()
                Case "Productos"
                    CargarDatosProducto(arrayAccion(1))
                    CargarDatosServicio()
                    Session("arrayaccion1") = arrayAccion(1)
                Case "Registro"
                    If arrayAccion(2).ToString() = "null" Then
                        arrayAccion(2) = Integer.Parse(cmbCampania.Value)
                    End If
                    resultado = ValidarMateriales(arrayAccion(2))
                    If resultado.Valor = 0 Then
                        resultado = RegistrarServiciosTransitorios(arrayAccion(1))
                    Else
                        CargarDatosProducto(CInt(Session("arrayaccion1")))
                    End If
                    CargarDatosServicio()
                    If resultado.Valor <> 0 Then
                        '_mensajeGenerico = resultado.Mensaje & "|amarillo"
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                    End If
                Case "Eliminar"
                    resultado = EliminarServiciosTransitorios(arrayAccion(1))
                    CargarDatosServicio()
                    If resultado.Valor <> 0 Then
                        '_mensajeGenerico = resultado.Mensaje & "|amarillo"
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                    End If

            End Select
            cmbTipoServicio.Focus()
        Catch ex As Exception
            '_mensajeGenerico = "Ocurrio un error al generar el servicio: " & ex.Message & "|rojo"
            mensajero.MostrarMensajePopUp("Ocurrio un error al generar el servicio: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Error")
        End Try
        CType(source, ASPxPopupControl).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje
        CType(source, ASPxPopupControl).JSProperties("cpResultado") = resultado.Valor
    End Sub

    Protected Sub Link_Init(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim linkVer As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkVer.NamingContainer, GridViewDataItemTemplateContainer)

            linkVer.ClientSideEvents.Click = linkVer.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)

        Catch ex As Exception
            _mensajeGenerico = "No fué posible establecer los permisos de las funcionalidades: " & ex.Message & "|rojo"
            'mensajero.MostrarMensajePopUp("No fué posible establecer los permisos de las funcionalidades: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Error")
        End Try
    End Sub

    Private Sub gvInfoDevolucionCallCenter_DataBinding(sender As Object, e As System.EventArgs) Handles gvInfoDevolucionCallCenter.DataBinding
        If Session("dtDevolucionesCall") Is Nothing Then
        Else
            gvInfoDevolucionCallCenter.DataSource = Session("dtDevolucionesCall")
        End If
    End Sub

    Private Sub gvDetalle_DataBinding(sender As Object, e As System.EventArgs) Handles gvDetalle.DataBinding
        If Session("dtGestiones") Is Nothing Then
        Else
            gvDetalle.DataSource = Session("dtGestiones")
        End If
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarInicial()

        '** Carga el listado de ciudades por WebService** 
        Dim infoCiudad As New CiudadColeccion
        Dim dtCiudad As New DataTable
        Dim idCampania As Integer
        With infoCiudad
            If cmbCampania.Value Is Nothing And _idCampaniaCliente > 0 Then
                idCampania = _idCampaniaCliente
            ElseIf cmbCampania.Value IsNot Nothing Then
                idCampania = cmbCampania.Value
            End If
            '.IdPais = Enumerados.CodigoPais.Colombia
            dtCiudad = ObtenerCiudadCampania(idCampania)
        End With
        CargarComboDX(cmbCiudad1, dtCiudad, "IdCiudad", "CiudadDepartamento")

        ' ** Carga tipo de documentos** 
        Dim dtTipo As New DataTable
        dtTipo = HerramientasGenerales.ConsultarTipoIdentificacion
        CargarComboDX(cbTipoId, dtTipo, "idTipo", "descripcion")

        ' ** Carga Estados Animo** 
        Dim dtEstado As New DataTable
        dtEstado = HerramientasGenerales.ConsultarEstadosAnimo
        CargarComboDX(cbEstadoId, dtEstado, "idEstadoAnimo", "nombre")

        Dim dtActividad As New DataTable
        dtActividad = HerramientasGenerales.ConsultarActividadLaboral
        CargarComboDX(cbActividadLaboral, dtActividad, "ALCid", "ALCnombre")

        '** Cargar Campañas desde NotusOP mediante un WebService **
        Dim dtCampania As New DataTable
        dtCampania = ObtenerCampanias()
        CargarComboDX(cmbCampania, dtCampania, "IdCampania", "Nombre")

        '** Cargar Resultado Proceso Venta
        Dim infoProceso As New ResultadoProcesoVentaColeccion
        Dim dtProceso As New DataTable
        With infoProceso
            .IdEstado = Enumerados.EstadoBinario.Activo
            .GestionCallCenter = True
            dtProceso = .GenerarDataTable()
        End With
        CargarComboDX(cmbResultadoProceso, dtProceso, "IdResultado", "Descripcion")

        If Session("clienteFuera") = True Then
            imgAgregarCliente.ClientVisible = True
            _visibleAgregarCliente = True
        Else
            ValidarEdicionservicio()
        End If



    End Sub

    Private Sub CargarDatosCliente(ByVal nuevaVentaCliente As Boolean, Optional ByVal esActualizacionCliente As Boolean = False)

        CargarInicial()
        ConsultarCliente(txtNumIdentificacion.Text.Trim, esActualizacionCliente)
        If _continuar Then

            ConsultaGestiones(txtNumIdentificacion.Text.Trim)
            ConsultaDevolucionesCall()
            MostrarDatos(txtNumIdentificacion.Text.Trim, CInt(Session("userId").ToString.Trim))
            txtNombres.Focus()
            If Session("dtGestiones").Rows.Count > 0 Then
                Dim dtEstadoVenta As New DataTable
                Dim indice As Integer

                dtEstadoVenta = Session("dtGestiones")
                indice = 0

                If Integer.Parse(dtEstadoVenta(indice)(45)) = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad OrElse
                   Integer.Parse(dtEstadoVenta(indice)(45)) = Enumerados.EstadosServicioMensajeria.Confirmado OrElse Integer.Parse(dtEstadoVenta(indice)(45)) = Enumerados.EstadosServicioMensajeria.DevoluciónCallCenter Then
                    _mensajeGenerico = "Seleccione una campaña diferente para gestionar nueva venta.|azul"

                    If CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 1 Then
                        BloquearControles(False, esActualizacionCliente)
                        btnRegistra.ClientVisible = False
                        'cmbCiudad1.Enabled = False
                        txtTelFijo.Enabled = False
                        txtCelular.Enabled = False
                        txtEmail.Enabled = False
                        cmbCampania.Enabled = False
                    Else
                        cmbCiudad1.Enabled = True
                        txtTelFijo.Enabled = True
                        txtCelular.Enabled = True
                        txtEmail.Enabled = True
                        txtIngresos.Enabled = True
                        cmbResultadoProceso.Enabled = False
                        cmbCausal.Enabled = False
                    End If

                ElseIf Integer.Parse(dtEstadoVenta(indice)(45)) = 0 Then
                    _mensajeGenerico = "El cliente tiene una venta pendiente por agendamiento. Es necesario agendar o seleccionar una nueva campaña.|azul"
                    BloquearControles(False, esActualizacionCliente)
                Else
                    _mensajeGenerico = "Cliente habilitado para nuevas gestiones.|verde"
                    BloquearControles(True, esActualizacionCliente)
                End If

                If dtEstadoVenta IsNot Nothing And esActualizacionCliente Then

                    CargarCausales(dtEstadoVenta(0).Item("IdResultadoProceso"))

                    cmbResultadoProceso.Value = dtEstadoVenta(0).Item("IdResultadoProceso").ToString()
                    cmbCausal.Value = dtEstadoVenta(0).Item("IdTipoVenta").ToString()
                    txtObservacionOperadorCall.Text = dtEstadoVenta(0).Item("ObservacionCallCenter")
                    If CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 1 Then
                        BloquearControles(True, esActualizacionCliente)
                    End If
                End If

            Else
                _mensajeGenerico = "Puede gestionar nuevas ventas para el cliente.|verde"
                cmbCampania.Enabled = False
                If CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 1 Then
                    BloquearControles(True, esActualizacionCliente)
                End If
                If Session("CampanaActiva") = False Then
                    _mensajeGenerico = "El cliente esta asociado a una Campaña Vencida o desactivada"
                    Session("dtGestiones") = Nothing
                    gvDetalle.DataBind()
                    Session("dtDevolucionesCall") = Nothing
                    gvInfoDevolucionCallCenter.DataBind()
                    gvServiciosAux.DataSource = Nothing
                    gvServiciosAux.DataBind()
                    btnRegistra.ClientVisible = False
                    cmbResultadoProceso.SelectedIndex = -1
                    txtOficinaCliente.Enabled = False
                    txtTelFijo.Enabled = False
                    txtCelular.Enabled = False
                    txtEmail.Enabled = False
                    txtNombres.Enabled = False
                    txtSegundoApellido.Enabled = False
                    txtDireccionResidencia.ClientEnabled = False
                    txtPrimerApellido.Enabled = False
                    cmbCiudad1.Enabled = False
                    cbEstadoId.Enabled = False
                    cbActividadLaboral.Enabled = False
                    txtDireccionResidencia.Enabled = False
                    cmbResultadoProceso.Enabled = False
                    cmbCausal.SelectedIndex = -1
                    cmbCausal.Enabled = False
                End If
            End If
        Else
            limpiarControles("NuevoCliente")
            LimpiarGrids()
        End If
    End Sub

    Private Sub BloquearControles(ByVal HabilitarResultadoP As Boolean, Optional ByVal esActualizacionClienteFinal As Boolean = False)
        cbTipoId.Enabled = False

        If esActualizacionClienteFinal Then
            txtNumIdentificacion.Enabled = False
            btnRegistra.ClientVisible = True
            txtIngresos.Enabled = True
            cmbResultadoProceso.Enabled = False
            cmbCausal.Enabled = False
        Else

            'btnRegistra.ClientVisible = False
            'cmbCiudad1.Enabled = False
            'txtTelFijo.Enabled = False
            'txtCelular.Enabled = False
            'txtEmail.Enabled = False
            txtNumIdentificacion.Enabled = True
            txtIngresos.Enabled = HabilitarResultadoP
            cmbCampania.Enabled = False
            cmbResultadoProceso.Enabled = HabilitarResultadoP
            cmbResultadoProceso.SelectedIndex = -1
            cmbCausal.Enabled = HabilitarResultadoP
            cmbCausal.SelectedIndex = -1
        End If
        If CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue) = 1 Then
            txtOficinaCliente.Enabled = False
            txtTelFijo.Enabled = False
            txtCelular.Enabled = False
            txtEmail.Enabled = False
            txtNombres.Enabled = False
            txtSegundoApellido.Enabled = False
            txtDireccionResidencia.ClientEnabled = False
            txtPrimerApellido.Enabled = False
            cmbCiudad1.ClientEnabled = False
            cbModificarDireccion.ClientVisible = True

        End If

        cbActividadLaboral.Enabled = HabilitarResultadoP
        'txtActividadLaboral.Enabled = False
        txtObservacionOperadorCall.Enabled = HabilitarResultadoP
        cbEstadoId.Enabled = HabilitarResultadoP
    End Sub

    Private Sub CargarDatosServicio()
        'Dim tipoProducto As New TipoProductoColeccion
        'Dim dtDatos As New DataTable
        'dtDatos = tipoProducto.GenerarDataTable

        Dim miProducto As New ProductoCampaniaColeccion
        Dim dtDatos As New DataTable
        With miProducto
            .IdCampania = cmbCampania.Value
            .TipoProducto = True
            dtDatos = .CargarDatos
        End With
        CargarComboDX(cmbTipoServicio, dtDatos, "idProductoComercial", "productoExterno")
    End Sub

    Private Sub CargarDatosProducto(ByVal idTipoProducto As Integer)

        Dim miProducto As New ProductoCampaniaColeccion
        Dim dtDatos As New DataTable
        With miProducto
            .IdCampania = cmbCampania.Value
            dtDatos = .CargarDatos
        End With
        CargarComboDX(cmbProducto, dtDatos, "idProductoExterno", "productoExterno")

        'Dim miProducto As New ProductoColeccion
        'Dim dtDatos As New DataTable
        'With miProducto
        '    .IdTipoProducto = idTipoProducto
        '    dtDatos = .GenerarDataTable
        'End With
        'CargarComboDX(cmbProducto, dtDatos, "IdProducto", "NombreProducto")
        cmbProducto.SelectedIndex = -1

        Dim miValorPrima As New ValorPrimaServicioColeccion
        Dim dtPrima As New DataTable
        With miValorPrima
            .IdTipoProducto = idTipoProducto
            dtPrima = .GenerarDataTable
        End With
        'If dtPrima.Rows.Count = 0 Then
        '    cmbPrima.Items.Insert(0, New ListEditItem("No existen valores", Nothing))
        'Else
        '    CargarComboDX(cmbPrima, dtPrima, "IdValorPrimaServicio", "ValorPrimaServicio")
        'End If
        'cmbPrima.SelectedIndex = -1

    End Sub

    Private Sub ConsultarCliente(ByVal numeroidentificacion As String, ByVal esActualizacionClienteFinal As Boolean)
        Session.Remove("sinRealce")
        Session.Remove("CampanaActiva")
        'cmbCampania.SelectedIndex = -1
        'se consulta la info del cliente
        Dim infoCliente As New Object
        If esActualizacionClienteFinal Then
            infoCliente = New ClienteFinal(CInt(Session("IdGestionVenta")), numeroidentificacion, Integer.Parse(CInt(Session("userId"))))
        Else
            infoCliente = New ClienteFinal(numeroidentificacion, Integer.Parse(CInt(Session("userId"))))
        End If

        Dim objetoCampania As New Campania
        Dim dtDatosCampania As New DataTable
        Dim dtGvDetalle As New DataTable
        Dim permiteCrearCliente As Boolean = False

        With infoCliente
            If .Registrado = False Then

                If txtNumIdentificacion.Value <> String.Empty Then
                    If cmbCampania.Value <> Nothing Then
                        dtDatosCampania = objetoCampania.ConsultarCampaniaPermiteCrearClienteFueraBase(cmbCampania.Value)
                        If dtDatosCampania.Rows.Count > 0 Then
                            permiteCrearCliente = dtDatosCampania(0)(12)
                            If permiteCrearCliente = False Then
                                _mensajeGenerico = "La campaña no permite crear clientes fuera de la base de cargue|rojo"
                                'mensajero.MostrarMensajePopUp("La campaña no permite crear clientes fuera de la base de cargue", MensajePopUp.TipoMensaje.ErrorCritico, "Error")
                            Else
                                imgAgregarCliente.ClientVisible = True
                                _visibleAgregarCliente = True
                                btnRegistra.ClientVisible = False
                                imgAgregar.ClientVisible = False
                                _mensajeGenerico = "El cliente no existe pero puede crearlo en la campaña: " & cmbCampania.Text & "|azul"
                                cmbResultadoProceso.Enabled = False
                                cmbCausal.Enabled = False
                                Session("clienteFuera") = True
                                txtNombres.ClientEnabled = True
                                txtPrimerApellido.ClientEnabled = True
                                txtSegundoApellido.ClientEnabled = True
                            End If
                            If (Session("dtGestiones")) IsNot Nothing Then
                                limpiarControles("NuevoCliente")
                                LimpiarGrids()
                            End If

                        End If
                    Else
                        _mensajeGenerico = "Por favor seleccione una campaña que permita crear al cliente fuera de la base de cargue|rojo"
                        btnRegistra.ClientVisible = False
                        cmbCampania.Enabled = True
                        'mensajero.MostrarMensajePopUp("El cliente no existe. Por favor seleccione una campaña para crearlo", MensajePopUp.TipoMensaje.ErrorCritico, "Error")
                    End If
                End If
                CargarInicial()
            Else
                If (cmbCampania.Value IsNot Nothing) Then
                    If cmbCampania.Value <> .IdCampania Then
                        dtDatosCampania = objetoCampania.ConsultarCampaniaPermiteCrearClienteFueraBase(cmbCampania.Value)
                        If dtDatosCampania.Rows.Count > 0 Then
                            permiteCrearCliente = dtDatosCampania(0)(12)
                        End If
                    Else
                        permiteCrearCliente = True
                    End If
                Else
                    permiteCrearCliente = True
                End If
                If permiteCrearCliente Then
                    _continuar = True
                    If Len(.Nombres) <= 0 Then
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
                                txtNombres.Text = txtNombres.Text & words(i)
                            End If

                        Next
                    Else
                        txtNombres.Text = .Nombres
                        txtPrimerApellido.Text = .PrimerApellido
                        txtSegundoApellido.Text = .SegundoApellido
                    End If
                    txtCelular.Text = .Celular
                    If .IdCiudadResidencia > 0 Then
                        cmbCiudad1.Value = .IdCiudadResidencia
                        Session("idCiudad") = cmbCiudad1.Value
                    End If
                    _idCampaniaCliente = .IdCampania

                    If .IdCampania > 0 And cmbCampania.Value Is Nothing Then
                        cmbCampania.Value = .IdCampania
                        cmbCampania.Enabled = False
                    Else
                        cmbCampania.Enabled = True
                    End If

                    If cmbCampania.Value IsNot Nothing Then
                        dtDatosCampania = objetoCampania.ConsultarCampaniaPermiteCrearClienteFueraBase(cmbCampania.Value)
                        If dtDatosCampania.Rows.Count > 0 Then
                            If dtDatosCampania(0).Item("fechaLlegadaproducto") = "" Then
                                Session("sinRealce") = True
                            Else
                                Session("sinRealce") = False
                            End If
                            If dtDatosCampania(0).Item("activo") = "False" Then
                                Session("CampanaActiva") = False
                            Else
                                Session("CampanaActiva") = True
                            End If
                        End If
                    End If

                    If .IdJornada > 0 And .IdJornada IsNot Nothing Then
                        cbJornada.Value = .IdJornada
                    End If

                    If .FechaAgenda IsNot Nothing AndAlso .FechaAgenda <> "" Then
                        calFechaInicio.Date = .FechaAgenda.ToString
                        ObtenerCapacidadFechaAgenda()
                    End If

                    txtTelFijo.Text = .TelefonoResidencia
                    txtIngresos.Text = .IngresoAproximado
                    txtEmail.Text = .Email
                    If .IdTipoIdentificacion > 0 Then
                        cbTipoId.Value = .IdTipoIdentificacion
                    End If
                    Session("idCliente") = .IdCliente
                    If .DireccionResidencia Is System.DBNull.Value Then
                    Else
                        txtDireccionResidencia.Text = .DireccionResidencia
                        Session("direccionBase") = .DireccionResidencia
                    End If
                    cbActividadLaboral.Text = .ActividadLaboral
                    'txtActividadLaboral.Text = .ActividadLaboral
                    txtOficinaCliente.Text = .CodOficina
                    If .IdEstadoAnimo > 0 Then
                        cbEstadoId.Value = .IdEstadoAnimo
                    Else
                        cbEstadoId.SelectedIndex = -1
                    End If
                    btnRegistra.ClientVisible = True

                    If esActualizacionClienteFinal = False Then
                        txtObservacionOperadorCall.Text = ""
                        cmbCausal.SelectedIndex = -1
                        cmbResultadoProceso.SelectedIndex = -1
                    End If

                Else
                    Session("dtGestiones") = Nothing
                    gvDetalle.DataBind()
                    Session("dtDevolucionesCall") = Nothing
                    gvInfoDevolucionCallCenter.DataBind()
                    gvServiciosAux.DataSource = Nothing
                    gvServiciosAux.DataBind()
                    _mensajeGenerico = "Por favor seleccione una campaña que permita crear al cliente fuera de la base de cargue|rojo"
                    btnRegistra.ClientVisible = False
                    CargarInicial()
                    cmbResultadoProceso.SelectedIndex = -1
                    cmbResultadoProceso.Enabled = False
                    cmbCausal.SelectedIndex = -1
                    cmbCausal.Enabled = False
                End If

            End If

        End With
    End Sub

    Private Sub LimpiarGrids()
        If (Session("dtGestiones")) IsNot Nothing Then
            Session("dtGestiones") = Nothing
            gvDetalle.DataBind()
        End If

        If (Session("dtDevolucionesCall")) IsNot Nothing Then
            Session("dtDevolucionesCall") = Nothing
            gvInfoDevolucionCallCenter.DataBind()
        End If

        If Session("IdProducto") IsNot Nothing Then
            Session.Remove("IdProducto")
            Session("IdProducto") = Nothing
            gvServiciosAux.DataSource = Session("IdProducto")
            gvServiciosAux.DataBind()
        End If
        'cmbCampania.SelectedIndex = -1
    End Sub

    Private Sub ConsultaGestiones(ByVal _numeroidentificacion As String)
        If _numeroidentificacion.Trim.Length > 0 Then
            Dim dtGestiones As DataTable
            Dim objGestiones As New GestionDeVentaColeccion
            With objGestiones
                .IdCampania = cmbCampania.Value
                .IdentificacionCliente.Add(_numeroidentificacion.Trim)
                dtGestiones = .GenerarDataTable
            End With
            Session("dtGestiones") = dtGestiones
            With gvDetalle
                .PageIndex = 0
                .DataSource = dtGestiones
                .DataBind()
            End With
        End If
    End Sub

    Private Sub ConsultaDevolucionesCall()
        If Request.QueryString("id") IsNot Nothing Then
            rpInfoDevolucionCallCenter.Visible = True
            Dim idGestionVenta As Long = CLng(Session("IdGestionVenta"))

            Dim dtDevolucionesCall As DataTable

            Dim objGestiones As New GestionDeVentaDevolucionesCall
            With objGestiones
                .IdGestionVenta = idGestionVenta
                dtDevolucionesCall = .ConsultaDevolucionesCall()
            End With

            Session("dtDevolucionesCall") = dtDevolucionesCall
            With gvInfoDevolucionCallCenter
                .PageIndex = 0
                .DataSource = dtDevolucionesCall
                .DataBind()
            End With
        End If

    End Sub

    Private Sub limpiarControles(origen As String)
        Session.Remove("IdProducto")
        Session.Remove("dtDisponibilidadEntrega")
        Session.Remove("IdProducto")
        Session.Remove("sinRealce")
        Session.Remove("IdGestionVenta")
        'cmbCampania.SelectedIndex = -1
        If String.IsNullOrEmpty(origen) Then
            txtNumIdentificacion.Text = String.Empty
            txtNumIdentificacion.Focus()
            gvServiciosAux.DataSource = Nothing
            gvServiciosAux.DataBind()
            Session("dtGestiones") = Nothing
            gvDetalle.DataSource = Nothing
            gvDetalle.DataBind()
            Session("dtDevolucionesCall") = Nothing
            gvInfoDevolucionCallCenter.DataSource = Nothing
            gvInfoDevolucionCallCenter.DataBind()
        End If
        cbTipoId.ClientEnabled = True
        txtNombres.Text = String.Empty
        txtPrimerApellido.Text = String.Empty
        txtSegundoApellido.Text = String.Empty
        txtCelular.Text = String.Empty
        txtDireccionResidencia.Text = String.Empty
        txtEmail.Text = String.Empty
        txtIngresos.Text = String.Empty
        txtTelFijo.Text = String.Empty
        txtObservacionOperadorCall.Text = String.Empty
        cbTipoId.SelectedIndex = -1
        cmbCiudad1.SelectedIndex = -1
        cmbResultadoProceso.SelectedIndex = -1
        'cmbCampania.SelectedIndex = -1
        cmbCausal.SelectedIndex = -1
        cbActividadLaboral.SelectedIndex = -1
        cbEstadoId.SelectedIndex = -1
        txtOficinaCliente.Text = String.Empty
        CargarInicial()
    End Sub

    Private Sub limpiarControlesServicio()
        cmbTipoServicio.SelectedIndex = -1
        cmbProducto.SelectedIndex = -1
        memoObservacionServicio.Text = ""
        txtCupo.Text = String.Empty
        ValidarEdicionservicio()
        ' cmbPrima.SelectedIndex = -1
    End Sub

    Private Sub ValidarEdicionservicio()
        If CInt(New ConfigValues("PERMITEEDITARGESTION").ConfigKeyValue) = 1 Then
            txtDireccionResidencia.ClientEnabled = True
            cmbCiudad1.ClientEnabled = True
            cmbCampania.ClientEnabled = True
            cbTipoId.ClientEnabled = False
            txtNombres.ClientEnabled = False
            txtPrimerApellido.ClientEnabled = False
            txtSegundoApellido.ClientEnabled = False
            txtCelular.ClientEnabled = True
            txtTelFijo.ClientEnabled = True
            txtEmail.ClientEnabled = True
            txtOficinaCliente.ClientEnabled = True
            cbModificarDireccion.ClientVisible = False
        Else
            cbTipoId.ClientEnabled = False
            txtNombres.ClientEnabled = False
            txtPrimerApellido.ClientEnabled = False
            txtSegundoApellido.ClientEnabled = False
            txtDireccionResidencia.ClientEnabled = False
            cmbCiudad1.ClientEnabled = False
            cbModificarDireccion.ClientVisible = True
            txtTelFijo.ClientEnabled = False
            txtCelular.ClientEnabled = False
            txtTelFijo.ClientEnabled = False
            txtEmail.ClientEnabled = False
            txtOficinaCliente.ClientEnabled = False
        End If


    End Sub

    Private Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim WSresultado As New ResultadoProceso
        Dim actualizacionWS As Boolean = False
        Dim regVenta As New GestionVentaCallCenter
        Dim nombreCampania As String = cmbCampania.Text
        Dim campania As String
        Dim arrayAccion As String()

        resultado.Valor = 0
        WSresultado.Valor = 0

        arrayAccion = nombreCampania.Split(";")
        If arrayAccion.Count > 1 Then
            campania = arrayAccion(1).Trim
        Else
            campania = ""
        End If

        If validarCampos() Then

            If Session("IdProducto") IsNot Nothing And Session("IdGestionVenta") Is Nothing Then
                If Session("IdProducto").ToString().Length > 0 And cmbCausal.Value = 3 Then
                    RegistrarServiciosTransitorios(txtNumIdentificacion.Text)
                End If
            End If

            With regVenta

                If cmbCausal.Value = 3 Then
                    WSresultado = RegistrarServicio()
                    .IdServicioNotus = WSresultado.Retorno
                    .FechaAgenda = Session("calFechaInicio")
                    .IdJornada = cbJornada.Value
                End If

                .IdTipoIdentificacion = cbTipoId.Value
                .IdEstadoAnimo = cbEstadoId.Value
                .IdentificacionCliente = txtNumIdentificacion.Text.Trim.ToUpper
                .IdEstado = Enumerados.EstadosGestionDeVenta.Registrada
                .IdEstrategiaComercial = Enumerados.EstrategiasComerciales.CentrosComerciales
                .IdPdv = 63
                .IdUsuarioAsesor = CInt(Session("userId").ToString.Trim)
                .IngresosAproximados = txtIngresos.Text.Trim.ToUpper
                .NombresCliente = txtNombres.Text.Trim.ToUpper
                .PrimerApellido = txtPrimerApellido.Text.Trim.ToUpper
                If (txtSegundoApellido.Text.Length > 0) Then
                    .SegundoApellido = txtSegundoApellido.Text.Trim.ToUpper
                End If
                .TelefonoResidencia = txtTelFijo.Text.Trim.ToUpper
                .Celular = txtCelular.Text.Trim.ToUpper

                If cbModificarDireccion.Checked Then
                    .DireccionResidencia = txtDireccionResidencia.Text
                    .IdCiudad = cmbCiudad1.Value
                ElseIf .IdEmpresa <> 1 Then
                    .DireccionResidencia = txtDireccionResidencia.Text
                    .IdCiudad = cmbCiudad1.Value
                Else
                    .DireccionResidencia = Session("direccionBase")
                    .IdCiudad = Session("idCiudad")
                End If

                .IdCampania = cmbCampania.Value
                .Campania = campania.Trim
                .IdResultado = cmbResultadoProceso.Value
                .IdCausal = cmbCausal.Value
                .Email = txtEmail.Text.Trim.ToUpper
                .ActividadLaboralCliente = cbActividadLaboral.Text
                .CodOficinaCliente = txtOficinaCliente.Text
                .Observaciones = txtObservacionOperadorCall.Text.Trim
                If cmbCausal.Value = 3 Then
                    .IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad
                Else
                    .IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.Rechazado
                End If

                If .IdServicioNotus > 0 Or cmbCausal.Value <> 3 Then
                    resultado = .Registrar()
                    If Session("IdGestionVenta") Is Nothing Then
                        Session("IdGestionVenta") = .IdGestionVenta
                    End If
                Else
                    resultado = WSresultado
                    actualizacionWS = True
                End If

            End With
            If regVenta.IdServicioNotus = 0 And cmbCausal.Value = 3 And actualizacionWS = False Then
                mensajero.MostrarMensajePopUp("Servicio notus no registrado en base local", MensajePopUp.TipoMensaje.ErrorCritico, "Error")
            ElseIf resultado.Valor = 0 And regVenta.IdServicioNotus > 0 Then
                mensajero.MostrarMensajePopUp(resultado.Mensaje + " idServicioNotus: " + CStr(regVenta.IdServicioNotus), MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                limpiarControles(String.Empty)
            ElseIf actualizacionWS = True And resultado.Valor = 0 Then
                resultado = regVenta.Registrar()
                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
            ElseIf cmbCausal.Value <> 3 And resultado.Valor = 0 Then
                mensajero.MostrarMensajePopUp(resultado.Mensaje + "Gestion Registrada con Exito")
                limpiarControles(String.Empty)
            ElseIf resultado.Valor = 3 And regVenta.IdServicioNotus > 0 Then
                mensajero.MostrarMensajePopUp(resultado.Mensaje + " idServicioNotus: " + CStr(regVenta.IdServicioNotus), MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
            ElseIf resultado.Valor = 10 And regVenta.IdServicioNotus > 0 Then
                mensajero.MostrarMensajePopUp(resultado.Mensaje + " idServicioNotus: " + CStr(regVenta.IdServicioNotus), MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
            Else
                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Error")
            End If
        Else
            resultado.Mensaje = _mensajeGenerico
            resultado.Valor = 2
        End If

        Return resultado
    End Function

    Private Function AgregarCLienteFueraBase() As ResultadoProceso
        Dim objetoCliente As New ClienteFinal
        Dim resultado As New ResultadoProceso
        Dim objCliente As New ClienteFinal

        Try
            If validarCampos() Then
                With objCliente
                    .IdTipoIdentificacion = cbTipoId.Value
                    .NumeroIdentificacion = txtNumIdentificacion.Value
                    .Nombres = txtNombres.Value
                    .NombreApellido = txtNombres.Value & " " & txtPrimerApellido.Value
                    .PrimerApellido = txtPrimerApellido.Value
                    .SegundoApellido = txtSegundoApellido.Value
                    .IdCiudadResidencia = cmbCiudad1.Value
                    .DireccionResidencia = txtDireccionResidencia.Value
                    .TelefonoResidencia = txtTelFijo.Value
                    .Celular = txtCelular.Value
                    .Email = txtEmail.Value
                    .IngresoAproximado = txtIngresos.Value
                    .IdCreador = Integer.Parse(CInt(Session("userId")))
                    .IdModificador = Integer.Parse(CInt(Session("userId")))
                    .IdCampania = Integer.Parse(cmbCampania.Value)
                    .ActividadLaboral = cbActividadLaboral.Text
                    .CodOficina = txtOficinaCliente.Value
                    .IdEstadoAnimo = cbEstadoId.Value
                    .CrearFueraBaseCargue = True
                    resultado = objCliente.AgregarClienteFueraBaseCargue()
                End With
            Else
                resultado.Mensaje = _mensajeGenerico
                resultado.Valor = 2
            End If
        Catch ex As Exception
            resultado.Mensaje = ex.ToString()
        End Try
        Return resultado
    End Function


    Private Function validarCampos() As Boolean
        If String.IsNullOrEmpty(cbTipoId.Value) Then
            _mensajeGenerico = "Se requiere el tipo de identificación|rojo"
            cbTipoId.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(txtNumIdentificacion.Value) Then
            _mensajeGenerico = "Se requiere el número de identificación|rojo"
            Return False
        ElseIf String.IsNullOrEmpty(txtNombres.Value) Then
            _mensajeGenerico = "Se requiere el o los nombres del cliente|rojo"
            txtNombres.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(txtPrimerApellido.Value) Then
            _mensajeGenerico = "Se requiere primer apellido del cliente|rojo"
            txtPrimerApellido.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(cmbCiudad1.Value) Then
            _mensajeGenerico = "Se requiere ciudad de residencia del cliente|rojo"
            cmbCiudad1.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(txtDireccionResidencia.Value) Then
            _mensajeGenerico = "Se requiere dirección de residencia del cliente|rojo"
            txtDireccionResidencia.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(txtTelFijo.Value) Then
            _mensajeGenerico = "Se requiere teléfono fijo del cliente|rojo"
            txtTelFijo.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(txtCelular.Value) Then
            _mensajeGenerico = "Se requiere celular del cliente|rojo"
            txtCelular.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(txtEmail.Value) Then
            _mensajeGenerico = "Se requiere E-mail del cliente|rojo"
            txtEmail.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(txtIngresos.Value) Then
            _mensajeGenerico = "Se requieren ingresos del cliente|rojo"
            txtIngresos.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(cbActividadLaboral.Value) Then
            _mensajeGenerico = "Se requiere actividad laboral del cliente|rojo"
            cbActividadLaboral.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(txtOficinaCliente.Value) Then
            _mensajeGenerico = "Se requiere la oficina del cliente|rojo"
            txtOficinaCliente.Focus()
            Return False
        ElseIf String.IsNullOrEmpty(cbEstadoId.Value) Then
            _mensajeGenerico = "Se requiere estado de ánimo del cliente|rojo"
            cbEstadoId.Focus()
            Return False
        ElseIf gvServiciosAux.VisibleRowCount = 0 And cmbCausal.Value = 3 Then
            _mensajeGenerico = "Se requiere ingresar productos |rojo"
            Return False
        ElseIf RpFechaAgendamiento.ClientVisible And calFechaInicio.Date < Date.Today.AddDays(1) Then
            _mensajeGenerico = "Se requiere ingresar Fecha agenda superior al dia de hoy|rojo"
            Return False
        ElseIf cbJornada.Value < 1 And RpFechaAgendamiento.ClientVisible Then
            _mensajeGenerico = "Se requiere ingresar jornada para la fecha agenda|rojo"
            Return False
        Else
            Return True
        End If
    End Function


    Private Function ValidarCiudad() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        Dim WSInfoServicio As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With WSInfoServicio
            .IdCampania = CInt(cmbCampania.Value)
            .IdCiudad = CInt(cmbCiudad1.Value)
            Wsresultado = objService.ValidaRegistrosServicioWS(WSInfoServicio)
        End With
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje
        Return resultado
    End Function

    Private Function RegistrarServiciosTransitorios(ByVal identificacionCliente As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miRegistro As New GestionVentaCallCenter
        With miRegistro
            If cmbProducto.Value <> Nothing Then
                .IdProducto = cmbProducto.Value
            ElseIf Session("IdProducto").ToString().Length > 0 Then
                .IdProducto = Session("IdProducto")
            End If
            If (Session("dtGestiones") IsNot Nothing) Then
                If (Session("dtGestiones").Rows.Count > 0) Then
                    .IdGestionVenta = Session("dtGestiones").Rows(0)(0).ToString()
                End If
            End If
            .IdUsuarioAsesor = CInt(Session("userId").ToString.Trim)
            .IdentificacionCliente = identificacionCliente.Trim
            If Not String.IsNullOrEmpty(txtCupo.Text) Then
                .ValorCupo = txtCupo.Text
            End If
            If Not String.IsNullOrEmpty(memoObservacionServicio.Text) Then
                .Observaciones = memoObservacionServicio.Text.Trim.ToUpper
            End If
            resultado = .RegistrarServiciosTransitorios()
        End With
        If resultado.Valor = 0 Then
            MostrarDatos(identificacionCliente, CInt(Session("userId").ToString.Trim))
            limpiarControlesServicio()
        End If
        Return resultado
    End Function

    Private Function ValidarMateriales(ByVal idCampania As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim lisIdProducto As New List(Of Integer)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        Dim WSInfoServicio As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        lisIdProducto.Add(cmbProducto.Value)

        With WSInfoServicio
            .IdCampania = idCampania
            .ListProductos = lisIdProducto.ToArray()
            Wsresultado = objService.ValidaRegistrosServicioWS(WSInfoServicio)
        End With
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje
        Return resultado
    End Function

    Private Function EliminarServiciosTransitorios(ByVal idRegistro As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miRegistro As New GestionVentaCallCenter
        With miRegistro
            resultado = .EliminarServiciosTransitorios(idRegistro)
        End With
        If resultado.Valor = 0 Then
            MostrarDatos(Session("IdentificacionUsuaio"), CInt(Session("userId").ToString.Trim))
            limpiarControlesServicio()
        End If
        Return resultado
    End Function

    Public Sub MostrarDatos(ByVal identificacionCliente As String, ByVal idUsuario As Integer)
        Dim miRegistro As New GestionVentaCallCenter
        Dim dtResultado As New DataTable
        With miRegistro
            .IdUsuarioAsesor = CInt(Session("userId").ToString.Trim)
            .IdentificacionCliente = identificacionCliente.Trim
            .IdCampania = cmbCampania.Value
            .IdGestionVenta = Session("IdGestionVenta")
            dtResultado = .ConsultarServiciosTransitorios()
        End With
        If dtResultado.Rows.Count > 0 Then
            Session("IdProducto") = dtResultado.Rows(0)(7)
        End If
        With gvServicios
            .PageIndex = 0
            .DataSource = dtResultado
            .DataBind()
        End With

        With gvServiciosAux
            .PageIndex = 0
            .DataSource = dtResultado
            .DataBind()
        End With
    End Sub

    Private Function ObtenerCampanias() As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsFiltroCampania
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Dim resultado As New ResultadoProceso
        With WSInfoFiltros
            .IdEmpresa = CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue)
            Wsresultado = objCampania.ConsultarCampaniasCEM(WSInfoFiltros, dsDatos)
        End With
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Private Function ObtenerCiudadCampania(ByVal idCampania As Integer) As DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        Wsresultado = objCampania.ConsultarCiudadesCampania(dsDatos, idCampania)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

    Private Function CargarCausales(ByVal idResultado As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dtCausal As New DataTable
        Dim miCausal As New ResultadoProcesoTipoVentaColeccion
        With miCausal
            .IdResultadoProceso = idResultado
            dtCausal = .GenerarDataTable()
        End With
        Session("dtCausal") = dtCausal
        CargarComboDX(cmbCausal, dtCausal, "IdTipoVenta", "TipoVenta")

        Return resultado
    End Function

    Private Function RequiereProducto(ByVal idResultado As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miCausal As New ResultadoProcesoTipoVenta(idResultado)
        Dim dtGestiones As New DataTable
        Dim indice As Integer

        dtGestiones = Session("dtGestiones")

        If miCausal.RequiereEntregarProductoProducto Then
            If (dtGestiones IsNot Nothing) Then
                If (dtGestiones.Rows.Count > 0) Then
                    indice = dtGestiones.Rows.Count - 1
                    If (dtGestiones.Rows(indice)(45) = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad Or dtGestiones.Rows(indice)(45) = Enumerados.EstadosServicioMensajeria.Confirmado) Then
                        imgAgregar.ClientVisible = False
                    Else
                        imgAgregar.ClientVisible = True
                    End If
                Else
                    imgAgregar.ClientVisible = True
                End If
            Else
                imgAgregar.ClientVisible = True
            End If
        Else
            imgAgregar.ClientVisible = False
        End If

        Return resultado
    End Function

    Private Function ObtenerCapacidadFechaAgenda() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim cantidadAM As String, cantidadPM As String
        Dim dtJornadas As New DataTable
        Dim PoseeCuposDia As Boolean = False
        epNotificador.clear()

        FechasAgendamiento()

        dtJornadas.Columns.Add("idJornada")
        dtJornadas.Columns.Add("nombre")

        Dim dtrJornada As DataRow = dtJornadas.NewRow()
        'dtJ = jornadas.GenerarDataTable

        Dim fechaI As Date = calFechaInicio.Date.ToString

        Session("calFechaInicio") = fechaI

        resultado = VerificarDisponibilidadEntrega(fechaI)
        Dim dtDisponibilidad As DataTable = CType(Session("dtDisponibilidadEntrega"), DataTable)

        If dtDisponibilidad IsNot Nothing Then

            Dim Fila As DataRow() = dtDisponibilidad.Select("Fecha = '" & fechaI.ToShortDateString() & "' AND IdJornada =1")
            Dim Fila2 As DataRow() = dtDisponibilidad.Select("Fecha = '" & fechaI.ToShortDateString() & "' AND IdJornada =2")

            If Fila.Count <> 0 Then
                cantidadAM = CStr(Fila(0).Item("CantidadDisponible"))
            Else
                cantidadAM = "0"
            End If
            If Fila2.Count <> 0 Then
                cantidadPM = CStr(Fila2(0).Item("CantidadDisponible"))
            Else
                cantidadPM = "0"
            End If

            Dim cantidadDisponibleAM1 As Integer = CInt(cantidadAM)
            Dim cantidadDisponiblePM1 As Integer = CInt(cantidadPM)

            Session("cantidadAM") = cantidadAM
            Session("cantidadPM") = cantidadPM

            Dim dt As New DataTable
            dt.Columns.Add("Horario")
            dt.Columns.Add("Cupo")
            Dim dtr As DataRow = dt.NewRow()
            If (cantidadDisponibleAM1 > 0) Then
                PoseeCuposDia = True
                dtr("Horario") = "AM"
                dtr("Cupo") = cantidadDisponibleAM1.ToString
                dt.Rows.Add(dtr)


                dtrJornada("idJornada") = "1"
                dtrJornada("nombre") = "AM"
                dtJornadas.Rows.Add(dtrJornada)

            Else
                If (dtJornadas.Rows.Count > 0) Then
                    dtJornadas.Rows(0).BeginEdit()
                    dtJornadas.Rows(0)("idJornada") = "0"
                    dtJornadas.Rows(0).EndEdit()
                End If
            End If

            If (cantidadDisponiblePM1 > 0) Then
                PoseeCuposDia = True
                dtr = dt.NewRow()
                dtr("Horario") = "PM"
                dtr("Cupo") = cantidadDisponiblePM1.ToString
                dt.Rows.Add(dtr)

                dtrJornada = dtJornadas.NewRow()
                dtrJornada("idJornada") = "2"
                dtrJornada("nombre") = "PM"
                dtJornadas.Rows.Add(dtrJornada)
            Else
                If (dtJornadas.Rows.Count > 2) Then
                    dtJornadas.Rows(2).BeginEdit()
                    dtJornadas.Rows(2)("idJornada") = "0"
                    dtJornadas.Rows(2).EndEdit()
                End If
            End If

            If dtJornadas.Select("idJornada<>0").Length > 0 Then
                Dim dtTemp As DataTable = dtJornadas.Select("idJornada<>0").CopyToDataTable
                dtJornadas.Clear()
                dtJornadas.Merge(dtTemp)
                dtTemp.Dispose()
            End If

            gvJornadas.DataSource = dt
            gvJornadas.DataBind()
        Else
            epNotificador.showWarning("Ha seleccionado un día no hábil. Por favor seleccione otro día.")
        End If

        If PoseeCuposDia Then
            epNotificador.clear()
            epNotificador.showInfo("Existe disponibilidad")
            Session("dtJornadas") = dtJornadas
            CargarComboDX(cbJornada, dtJornadas, "idJornada", "nombre")
        Else
            cbJornada.Items.Clear()
            gvJornadas.DataSource = Nothing
            gvJornadas.DataBind()
        End If

        Return resultado
    End Function

    Private Function VerificarDisponibilidadEntrega(ByVal fechaInicial As Date) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsInfoCapacidadEntrega
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With WSInfoFiltros
            .FechaInicial = fechaInicial
            .FechaFinal = fechaInicial
            .IdCiudad = cmbCiudad1.Value
            .IdEmpresa = New ConfigValues("ID_EMPRESA").ConfigKeyValue
        End With
        Wsresultado = objService.ConsultarCapacidadEntrega(WSInfoFiltros, dsDatos)
        dtDatos = dsDatos.Tables(0)
        If dtDatos.Rows.Count <> 0 Then

            Session("dtDisponibilidadEntrega") = dtDatos
        Else
            resultado.EstablecerMensajeYValor(1, "No existe disponibilidad para realizar el agendamiento de entregas en la fecha seleccionada.")
        End If
        Return resultado
    End Function

    Private Sub ReiniciarCalendario()
        Session.Remove("dtDisponibilidadEntrega")
        gvJornadas.DataSource = Nothing
        gvJornadas.DataBind()
        calFechaInicio.ReadOnly = True
        lblCapacidadEntrega.Text = String.Empty
        calFechaInicio.Dispose()
        calFechaInicio.Date = Date.Now
        cbJornada.DataSource = String.Empty
        cbJornada.DataBind()
        cbJornada.Text = String.Empty
    End Sub

    Public Function RegistrarServicio() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim lisIdProducto As New List(Of Integer)
        Dim lisTipoProducto As New List(Of String)
        Dim lisCupoProducto As New List(Of String)
        Dim dtProducto As New DataTable
        'Dim miCliente As New ClienteFinal(miGestion.IdCliente)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        Dim objClienteFinal As New ClienteFinal(txtNumIdentificacion.Text, CInt(Session("userId").ToString.Trim))
        Dim WSInfoServicio As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Dim idServicio As Long

        If Request.QueryString("id") Is Nothing Then
            For I As Integer = 0 To gvServiciosAux.VisibleRowCount - 1
                lisIdProducto.Add(gvServiciosAux.GetRowValues(I, New String() {"idProducto"}))
                lisTipoProducto.Add(gvServiciosAux.GetRowValues(I, New String() {"producto"}))
                lisCupoProducto.Add(gvServiciosAux.GetRowValues(I, New String() {"valorPrimaServicio"}))
            Next
        End If

        With WSInfoServicio
            .FechaAgenda = Session("calFechaInicio")
            .IdJornada = cbJornada.Value
            .IdEmpresa = CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue)
            .IdEstado = NotusIlsService.EstadoServicio.PendienteAprobacionCalidad
            .Nombre = txtNombres.Text & " " & txtPrimerApellido.Text & " " & txtSegundoApellido.Text

            .NombresCompleto = txtNombres.Text
            .PrimerApellido = txtPrimerApellido.Text
            .SegundoApellido = txtSegundoApellido.Text
            .Identicacion = txtNumIdentificacion.Text
            .CodigoEstrategiaComercial = objClienteFinal.CodigoEstrategia
            .Sexo = objClienteFinal.Sexo
            .Telefono = objClienteFinal.TelefonoResidencia
            .Celular = txtCelular.Text
            .TelefonoAdicional = objClienteFinal.TelefonoAdicional
            .Correo = objClienteFinal.Email
            .CodigoAgenteVendedor = objClienteFinal.Agente


            If cbModificarDireccion.Checked Then
                .Direccion = txtDireccionResidencia.Text
                .IdCiudad = cmbCiudad1.Value
            ElseIf .IdEmpresa <> 1 Then
                .Direccion = txtDireccionResidencia.Text
                .IdCiudad = cmbCiudad1.Value
            Else
                .Direccion = Session("direccionBase")
                .IdCiudad = Session("idCiudad")
            End If

            .IdCampania = cmbCampania.Value
            .ListProductos = lisIdProducto.ToArray()
            .ListTipoServicio = lisTipoProducto.ToArray()
            .ListCupoProducto = lisCupoProducto.ToArray()
            .ActividadLaboral = cbActividadLaboral.Text
            .CodOficinaCliente = txtOficinaCliente.Text
            .Observacion = txtObservacionOperadorCall.Text

            If Request.QueryString("id") IsNot Nothing Then
                Dim idGestionVenta As Long = CLng(Session("IdGestionVenta"))
                Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta)

                If miGestion.IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad Or miGestion.IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.DevoluciónCallCenter Then
                    .IdServicioMensajeria = miGestion.IdServicioNotus
                    Wsresultado = objService.ActualizarServicioWS(WSInfoServicio)
                Else
                    Wsresultado.Valor = 2
                    Wsresultado.Mensaje = "El servicio se encuentra en un estado diferente a confirmado, no se puede realizar la modificación."
                End If
            Else
                Wsresultado = objService.RegistrarServicioWS(WSInfoServicio, idServicio)
            End If

        End With

        resultado.Mensaje = Wsresultado.Mensaje
        resultado.Valor = Wsresultado.Valor
        resultado.Retorno = idServicio

        Return resultado
    End Function

    Private Function ActualizarGestionVenta(ByVal idServicio As Long, idGestionVenta As Integer, ByVal fechaAgenda As Date) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim objGestion As New GestionComercial.GestionDeVenta
        With objGestion
            .IdGestionVenta = idGestionVenta
            .IdModificador = Session("userId")
            .IdServicioNotus = idServicio
            .FechaAgenda = fechaAgenda
            .IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad
            resultado = .Actualizar()
        End With
        Return resultado
    End Function


    Private Function FechasAgendamiento() As ResultadoProceso
        '** Agendamiento
        Dim resultado As New ResultadoProceso
        Dim fecha As Date
        fecha = Date.Now
        Dim intervalos As New ConfigValues("INTERVALO_DIAS_HABILES_AGENDAMIENTO")
        Dim diaIntervaloDiasHabilesAgenda As New System.Text.RegularExpressions.Regex(intervalos.ConfigKeyValue)
        IntervaloDias = CInt(diaIntervaloDiasHabilesAgenda.ToString)
        Session("IntervaloDias") = IntervaloDias
        calFechaInicio.ReadOnly = True

        'Valida que la cuidad se encuentre seleccionada y sea una venta efectiva
        If Not String.IsNullOrEmpty(cmbCiudad1.Value) And RpFechaAgendamiento.ClientVisible Then
            calFechaInicio.ReadOnly = False

            Dim bogota As New ConfigValues("MAX_DIAS_AGENDAMIENTO_BTA")
            Dim ciudades As New ConfigValues("MAX_DIAS_AGENDAMIENTO_CIUDAD")
            Dim diaBog As New System.Text.RegularExpressions.Regex(bogota.ConfigKeyValue)
            Dim diaCiudad As New System.Text.RegularExpressions.Regex(ciudades.ConfigKeyValue)
            diasBogota = CInt(diaBog.ToString)

            If Session("sinRealce") IsNot Nothing And Session("sinRealce") Then
                diasCiudades = 0
            Else
                diasCiudades = CInt(diaCiudad.ToString)
            End If

            Session("diasCiudades") = diasCiudades
            Dim diasInicio As Integer = diasCiudades
            Dim diasIntervalo As Integer = diasCiudades + CInt(diaIntervaloDiasHabilesAgenda.ToString)
            If Request.QueryString("Id") IsNot Nothing Then
                diasInicio = 0
                calFechaInicio.MinDate = fecha.AddDays(diasInicio)
                calFechaInicio.MaxDate = fecha.AddDays(diasIntervalo)
            Else
                calFechaInicio.MinDate = fecha.AddDays(diasInicio)
                calFechaInicio.MaxDate = fecha.AddDays(diasIntervalo)
            End If
            Dim dias As Int16 = 0
            Dim fechaTmp As Date = fecha.AddDays(diasInicio)
            While fechaTmp <= fecha.AddDays(diasIntervalo)
                Select Case fechaTmp.DayOfWeek
                    Case 0
                        dias = dias + 1
                    Case 6
                        dias = dias + 1
                End Select

                fechaTmp = fechaTmp.AddDays(1)

            End While
            calFechaInicio.MaxDate = fecha.AddDays(diasIntervalo + dias)
        Else
            'calFechaInicio.ReadOnly = True
        End If
        Return resultado
    End Function

    Protected Sub cmbCausal_DataBinding(sender As Object, e As EventArgs) Handles cmbCausal.DataBinding
        If Session("dtCausal") IsNot Nothing Then
            cmbCausal.DataSource = Session("dtCausal")
        End If
    End Sub

    Protected Sub cmbCausal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCausal.SelectedIndexChanged
        If cmbCausal.Value = 3 Then
            RpFechaAgendamiento.ClientVisible = True
            btnAgregar.ClientVisible = True
        Else
            RpFechaAgendamiento.ClientVisible = False
            btnAgregar.ClientVisible = False
        End If

    End Sub

#End Region

End Class