Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.ControlAcceso
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports LoginLmBusinessLayer
Imports LoginLm.Datos
Public Class CreacionUsuario
    Inherits System.Web.UI.Page

#Region "Atributos"

#End Region
#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            mensajero.Limpiar()
            epNotificador.clear()
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Creación de Usuarios")
                    .showReturnLink("~/Administracion/Default.aspx")
                End With
                ObtenerListaRol()
                ObtenerListaTipoPersona()
                ObtenerListaEmpresa()
                ObtenerListaCargos()
                RolPuntoDeVenta()
                RolRequireFechacreacion()
                deFechaIngreso.Date = Date.Now
            End If
            If cpRegistro.IsCallback Then
                cbCiudad.DataBind()
                'cbUnidadNegocio.DataBind()
                'cbEmpresa.DataBind()

            End If
        Catch ex As Exception
             mensajero.MostrarMensajePopUp("Error al tratar de cargar la página. ", MensajePopUp.TipoMensaje.ErrorCritico)

        End Try
    End Sub

    Protected Sub cbEmpresa_DataBinding(sender As Object, e As EventArgs) Handles cbEmpresa.DataBinding
        If cbEmpresa.DataSource Is Nothing AndAlso Session("DTinfoEmpresa") IsNot Nothing Then
            cbEmpresa.DataSource = CType(Session("DTinfoEmpresa"), DataTable)
            cbEmpresa.Items.Insert(0, New ListEditItem("Seleccione una Empresa...", -1))
            cbEmpresa.SelectedIndex = 0

        End If

    End Sub
    Protected Sub cbUnidadNegocio_DataBinding(sender As Object, e As EventArgs) Handles cbUnidadNegocio.DataBinding
        If cbUnidadNegocio.DataSource Is Nothing AndAlso Session("DTinfoUnidadDeNegocio") IsNot Nothing Then cbUnidadNegocio.DataSource = CType(Session("DTinfoUnidadDeNegocio"), UnidadDeNegocioColeccion)
    End Sub

    Private Function RegistrarUsuario() As NotusExpressBusinessLayer.General.ResultadoProceso
        Dim persona1 As New GestionPersonaUsuario
        Dim encriptar As New EncryptionLibrary
        Dim resultado As New NotusExpressBusinessLayer.General.ResultadoProceso()

        Try
            Dim listaPdv As String = String.Empty
            For Each item As ListEditItem In lbPuntodeVentaUsuario.Items
                If item.Value Then
                    listaPdv += item.Value & ","
                End If
            Next
            listaPdv = listaPdv.TrimEnd(","c)
            Dim usSietema As New UsuarioSistema
            With usSietema
                .NumeroIdentificacion = TextNumeroIdentificacion.Text.Trim()
                .NombreApellido = TextNombre.Text.Trim().ToUpper()
                .IdCreador = CInt(CInt(Session("userId")))
                .Email = TextEmail.Text
                .Usuario = TextUsuario.Text
                If Not (IsDBNull(TextTelefono.Text) Or TextTelefono.Text = "") Then
                    .Telefono = TextTelefono.Text
                End If
                .IdCiudad = cbCiudad.Value
                .IdRol = CbRol.Value
                .IdPerfil = CbPerfil.Value
                .IdTipo = cbTipo.Value
                .IdCargo = cbCargo.Value
                .IdEmpresa = cbEmpresa.Value
                .IdUnidadNegocio = cbUnidadNegocio.Value
                .ListaPdv = listaPdv
                If (IsDate(deFechaIngreso.Date)) Then
                    .FechaIngreso = deFechaIngreso.Date
                End If
                .IdEstado = cbEstado.Value
                resultado = .CrearUsuario()
                Dim resultCrearUsuario As Integer
                If resultado.Valor > 0 Then
                    With persona1
                        .NumeroIdentificacion = TextNumeroIdentificacion.Text.Trim()
                        .NombresApellidos = TextNombre.Text.Trim().ToUpper()
                        .Telefono = TextTelefono.Text
                        .Email = TextEmail.Text
                        .IdCiudad = cbCiudad.Value
                        .IdEstado = cbEstado.Value
                        .Usuario = TextUsuario.Text
                        .IdCreador = CInt(Session("userId"))
                        .IdSistema = 3
                        .Clave = "12345.LM"
                        resultCrearUsuario = .CrearPersonaUsuario()
                        If resultCrearUsuario = 1 Then
                            resultado.EstablecerMensajeYValor(1, "El usuario se creo correctamente.")
                            mensajero.MostrarMensajePopUp("El usuario se creo correctamente.", MensajePopUp.TipoMensaje.ProcesoExitoso)

                            limpiarCampor()
                        Else
                            mensajero.MostrarMensajePopUp("Ocurrio un error con la información, por favor verifique.", MensajePopUp.TipoMensaje.ErrorCritico)
                            resultado.EstablecerMensajeYValor(-1, "Ocurrio un error con la información, por favor verifique.")
                        End If
                    End With



                ElseIf (resultado.Valor = 0) Then
                    resultado.EstablecerMensajeYValor(1, "El usuario ya existe en el sistema, por favor verificar:")
                    Return resultado
                End If
            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(-1, "Se presentó el siguiente error al actualizado el usuario." + ex.Message)
            mensajero.MostrarMensajePopUp("Se presentó el siguiente error al actualizado el usuario." + ex.Message, MensajePopUp.TipoMensaje.ErrorCritico)

            Return resultado
        End Try
        Return resultado
    End Function
#End Region

#Region "Metodos"
    Private Sub limpiarCampor()

        TextNumeroIdentificacion.Text = ""
        TextNombre.Text = ""
        TextEmail.Text = ""
        TextUsuario.Text = ""
        TextTelefono.Text = ""
        cbCiudad.SelectedIndex = 0
        CbRol.SelectedIndex = 0
        cbTipo.SelectedIndex = 0
        cbCargo.SelectedIndex = 0
        cbEmpresa.SelectedIndex = 0
        cbEstado.SelectedIndex = 0
        CbPerfil.SelectedIndex = 0
        cbUnidadNegocio.SelectedIndex = 0
        lbPuntodeVentaUsuario.Items.Clear()
    End Sub
    Private Sub ObtenerListaCargos()
        Try
            Dim infoCargos As New CargosColeccion
            infoCargos.CargarDatos()
            With cbCargo
                .DataSource = infoCargos
                .TextField = "nombre"
                .ValueField = "idCargo"
                .DataBindItems()
            End With
            cbCargo.Items.Insert(0, New ListEditItem("Seleccione un Cargo...", -1))
            cbCargo.SelectedIndex = 0
        Catch
            mensajero.MostrarMensajePopUp("Error al tratar de cargar el listado de Cargos. ", MensajePopUp.TipoMensaje.ErrorCritico)
        End Try
    End Sub
    Private Sub ObtenerListaTipoPersona()
        Try
            Dim infoTipoPersona As New TipoPersonaColeccion
            infoTipoPersona.CargarDatos()
            With cbTipo
                .DataSource = infoTipoPersona
                .TextField = "descripcion"
                .ValueField = "idTipoPersona"
                .DataBindItems()
            End With
            cbTipo.Items.Insert(0, New ListEditItem("Seleccione un tipo de persona...", -1))
            cbTipo.SelectedIndex = 0
        Catch
            mensajero.MostrarMensajePopUp("Error al tratar de cargar el listado de Tipo Persona.  ", MensajePopUp.TipoMensaje.ErrorCritico)
        End Try
    End Sub
    Private Sub ObtenerListaRol()
        Try
            Dim infoRol As New RolSistema
            infoRol.CargarDatos()
            With CbRol
                .DataSource = infoRol
                .TextField = "nombre"
                .ValueField = "idRol"
                .DataBindItems()
            End With
            CbRol.Items.Insert(0, New ListEditItem("Seleccione un Rol...", -1))
            CbRol.SelectedIndex = 0
        Catch
            mensajero.MostrarMensajePopUp("Error al tratar de cargar el listado de Rol. ", MensajePopUp.TipoMensaje.ErrorCritico)
        End Try
    End Sub
    Private Sub RolPuntoDeVenta()
        Try
            Dim _perRolPuntoDeVenta As DataTable
            Dim infoPerfilPuntoDeVenta As New Perfil
            _perRolPuntoDeVenta = infoPerfilPuntoDeVenta.RolPuntoDeVenta()
            If _perRolPuntoDeVenta Is Nothing Then
                mensajero.MostrarMensajePopUp("No se han configurado los perfiles que requieren un punto de venta asignado", MensajePopUp.TipoMensaje.ErrorCritico)
            Else
                Session("rolPuntoDeVenta") = _perRolPuntoDeVenta
            End If
        Catch
            mensajero.MostrarMensajePopUp("Error al tratar de cargar el listado de Rol por punto de venta. ", MensajePopUp.TipoMensaje.ErrorCritico)
        End Try
    End Sub
    Private Sub RolRequireFechacreacion()
        Try
            Dim _rolPuntoDeVenta As DataTable
            Dim infoPerfilPuntoDeVenta As New Perfil
            _rolPuntoDeVenta = infoPerfilPuntoDeVenta.RolRequireFechacreacion()
            If _rolPuntoDeVenta Is Nothing Then
                mensajero.MostrarMensajePopUp("No se han configurado los perfiles que requieren Fecha de creación", MensajePopUp.TipoMensaje.ErrorCritico)
            Else
                Session("RolRequireFechacreacion") = _rolPuntoDeVenta
            End If
        Catch
            mensajero.MostrarMensajePopUp("Error al tratar de cargar el listado de Rol que requieren Fecha de creación ", MensajePopUp.TipoMensaje.ErrorCritico)
         End Try
    End Sub
    Private Sub CargarListadoPdv(Optional ByVal idCiudad As Integer = 0)
        Try
            Dim listaPdv As New PuntoDeVentaColeccion
            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                .IdEstado = 1
                .IdCudadB = idCiudad
                .CargarDatos()
            End With
            With lbPuntodeVenta
                .DataSource = listaPdv
                .TextField = "nombrePdv"
                .ValueField = "idPdv"
                .DataBindItems()
            End With
        Catch ex As Exception
            mensajero.MostrarMensajePopUp("Error al tratar de obtener el listado de Puntos de Venta. ", MensajePopUp.TipoMensaje.ErrorCritico)
        End Try
    End Sub

    Private Sub ConsultaPerfil(rol As Int32)
        Dim infoPerfil As New PerfilColeccion
        With infoPerfil
            .IdRol = rol
            .IdEstado = 1
            .CargarDatos()
        End With
        With CbPerfil
            .DataSource = infoPerfil
            .TextField = "Nombre"
            .ValueField = "IdPerfil"
            .DataBindItems()
        End With
        CbPerfil.Items.Insert(0, New ListEditItem("Seleccione un perfil...", -1))
        CbPerfil.SelectedIndex = 0
    End Sub
    Private Sub CargarUnidadDeNegocio(ByVal IdEmpresa As String)
        Try
            If (IdEmpresa <> "") Then
                Dim infoUnidadDeNegocio As New UnidadDeNegocioColeccion
                With infoUnidadDeNegocio
                    .IdEstado = 1
                    .IdEmpresa = IdEmpresa
                    .CargarDatos()
                End With
                Session("DTinfoUnidadDeNegocio") = infoUnidadDeNegocio
                With cbUnidadNegocio
                    .DataSource = CType(Session("DTinfoUnidadDeNegocio"), UnidadDeNegocioColeccion)
                    .TextField = "nombre"
                    .ValueField = "idUnidadNegocio"
                    .DataBindItems()
                End With
                cbUnidadNegocio.Items.Insert(0, New ListEditItem("Seleccione una unidad de negocio...", -1))
                cbUnidadNegocio.SelectedIndex = 0
            End If
        Catch ex As Exception
            mensajero.MostrarErrorYNotificarViaMail("Error al tratar de cargar el listado de Unidad De Negocio.", "Creación de Usuario - Cargar Unidad de Negocio", ex)

        End Try
    End Sub
#End Region

    Private Sub cpRegistro_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpRegistro.Callback
        mensajero.Limpiar()

        Dim resultado As New NotusExpressBusinessLayer.General.ResultadoProceso()
        Try
            Dim accion As String
            Dim parametro As String
            Dim arrParametros() As String = e.Parameter.Split("|")
            accion = arrParametros(0)
            parametro = arrParametros(1)
            Select Case accion
                Case "validarRequierePDV"
                    mostrarPDV(CbRol.Value)
                    ConsultaPerfil(CbRol.Value)
                Case "LimpiarPanel"
                    limpiarCampor()
                Case "CargarUnidadDeNegocio"
                    CargarUnidadDeNegocio(cbEmpresa.Value)
                Case "SeleccionaPDV"
                    CargarListadoPdv(cbCiudad.Value)
                Case "RegistrarUsuario"
                    resultado = RegistrarUsuario()
                    mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.Alerta)
            End Select
        Catch ex As Exception
            cpRegistro.JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje()
            cpRegistro.JSProperties("cpTituloPopUp") = mensajero.Titulo

            'mensajero.showError("Se generó un error inesperado al intentar registrar el servicio: " + ex.Message)
        End Try
        'cpRegistro.JSProperties("cpMensaje") = resultado.Mensaje
    End Sub
    Protected Sub cbCiudad_DataBound(sender As Object, e As EventArgs) Handles cbCiudad.DataBound
        If cbCiudad.DataSource Is Nothing AndAlso Session("DTCiudades") IsNot Nothing Then cbCiudad.DataSource = CType(Session("DTCiudades"), DataTable)

    End Sub
    Private Sub mostrarPDV(ByVal IdRol As Integer)
        If (Session("rolPuntoDeVenta") IsNot Nothing) Then
            Dim rolPdv As DataTable = CType(Session("rolPuntoDeVenta"), DataTable)
            Dim buscarFila() As DataRow
            buscarFila = rolPdv.Select("idRol = '" & IdRol & "'")
            If (buscarFila.Length > 0) Then
                hdMostarPDV.Set("MostarPDV", 1)
                CargarListadoPdv(cbCiudad.Value)
            Else
                hdMostarPDV.Set("MostarPDV", 0)
            End If
        End If
        If (Session("RolRequireFechacreacion") IsNot Nothing) Then
            Dim rolfecharequiere As DataTable = CType(Session("RolRequireFechacreacion"), DataTable)
            Dim Buscar_Filarequierefecha() As DataRow
            Buscar_Filarequierefecha = rolfecharequiere.Select("idRol = '" & IdRol & "'")
            If (Buscar_Filarequierefecha.Length > 0) Then
                deFechaIngreso.Enabled = True
                hdMostarPDV.Set("cpRequiereFechaCrea", 1)
            Else
                deFechaIngreso.Enabled = False
                hdMostarPDV.Set("cpRequiereFechaCrea", 1)
            End If
        End If

    End Sub
    Private Sub ObtenerListaEmpresa()
        Try
            Dim infoEmpreas As New EmpresaColeccion
            Session("DTinfoEmpresa") = infoEmpreas.CargarComboEmpresas()
            With cbEmpresa
                .DataSource = CType(Session("DTinfoEmpresa"), DataTable)
                .TextField = "nombre"
                .ValueField = "idEmpresa"
                .DataBind()
            End With
            cbEmpresa.Items.Insert(0, New ListEditItem("Seleccione una Empresa...", -1))
            cbEmpresa.SelectedIndex = 0

        Catch
            mensajero.MostrarMensajePopUp("Error al tratar de cargar el listado de Empresas. ", MensajePopUp.TipoMensaje.ErrorCritico)
         End Try
    End Sub
    Protected Sub cbCiudad_OnItemsRequestedByFilterCondition_SQL(source As Object, e As ListEditItemsRequestedByFilterConditionEventArgs)

        Dim infoCiudad As New CiudadColeccion
        Session("DTCiudades") = infoCiudad.ObtenerCiudadesCobox(e.Filter, e.BeginIndex + 1, e.EndIndex + 1)
        With cbCiudad
            .DataSource = CType(Session("DTCiudades"), DataTable)
            .ValueField = "idCiudad"
            .ValueType = GetType(Int32)
            .TextField = "nombre"
            .DataBind()
        End With
        cbCiudad.Items.Insert(0, New ListEditItem("Seleccione una Ciudad...", -1))
        cbCiudad.SelectedIndex = 0

    End Sub
    Protected Sub cbCiudad_OnItemRequestedByValue_SQL(source As Object, e As ListEditItemRequestedByValueEventArgs)
        If e.Value = Nothing Or e.Value = -1 Then
            Return
        End If
        If Session("DTCiudades") IsNot Nothing Then
            Dim data As DataTable = Session("DTCiudades")
            Dim query = From r In data Where r.Field(Of Decimal)("idCiudad") = e.Value Select r
            If query.Count = 0 Then
                Return
            ElseIf query.Count > 1 Then
                Return
            Else
                With cbCiudad
                    .DataSource = query.CopyToDataTable
                    .ValueField = "idCiudad"
                    .ValueType = GetType(Int32)
                    .TextField = "nombre"
                    .DataBind()
                End With

            End If

        End If
    End Sub

End Class