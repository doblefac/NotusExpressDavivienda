Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.ControlAcceso
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports LoginLmBusinessLayer
Imports LoginLm.Datos
Public Class EdiciondeUsuario
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private numeroIdentificacion As String

#End Region
#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            epNotificador.clear()
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Edición de Usuarios")
                    .showReturnLink("~/Usuarios/ConsultadeUsuarios.aspx")
                End With
                ObtenerListaRol()
                Cargalistadeciudades()
                ObtenerListaTipoPersona()
                ObtenerListaEmpresa()
                ObtenerListaCargos()
                RolPuntoDeVenta()
                RolRequireFechacreacion()
                ConsultaUnidadDeNegocio(1)
                ConsultaPerfil(0)
                CargarDatosUsuario()
                CargarListadoPdvUsuario()
            End If
            If cpRegistro.IsCallback Then
                cbCiudad.DataBind()
                cbUnidadNegocio.DataBind()
                cbEmpresa.DataBind()
            End If

        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la página. ")
        End Try
    End Sub
    Protected Sub cbEmpresa_DataBinding(sender As Object, e As EventArgs) Handles cbEmpresa.DataBinding
        If cbEmpresa.DataSource Is Nothing AndAlso Session("DTinfoEmpresa") IsNot Nothing Then cbEmpresa.DataSource = CType(Session("DTinfoEmpresa"), DataTable)
    End Sub
    Protected Sub cbUnidadNegocio_DataBinding(sender As Object, e As EventArgs) Handles cbUnidadNegocio.DataBinding
        If cbUnidadNegocio.DataSource Is Nothing AndAlso Session("DTinfoUnidadDeNegocio") IsNot Nothing Then cbUnidadNegocio.DataSource = CType(Session("DTinfoUnidadDeNegocio"), UnidadDeNegocioColeccion)
    End Sub
    Private Sub ActualizarUsuario()
        Dim persona1 As New CrearPersonaUsuario()

        Try
            Dim listaPdv As String = String.Empty
            Dim resultado As New NotusExpressBusinessLayer.General.ResultadoProceso()
            For Each item As ListEditItem In lbPuntodeVentaUsuario.Items
                If item.Value Then
                    listaPdv += item.Value & ","
                End If
            Next
            listaPdv = listaPdv.TrimEnd(","c)
            Dim usSietema As New UsuarioSistema
            With usSietema
                .IdUsuario = hfIdUsuario.Value
                .IdPersona = hfIdPersona.Value
                .NumeroIdentificacion = TextNumeroIdentificacion.Text.Trim()
                .NombreApellido = TextNombre.Text.Trim().ToUpper()
                .IdCreador = CInt(CInt(Session("userId")))
                .Email = TextEmail.Text
                .Usuario = TextUsuario.Text
                If Not (IsDBNull(TextTelefono.Text) Or TextTelefono.Text = "") Then
                    .Telefono = Convert.ToInt64(TextTelefono.Text)
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
                If (hfIdEstadoUsuario.Get("IdEstadoUsuario") = 1 And cbEstado.Value = 0) Then
                    If (IsDate(deFechaRetiro.Date)) Then
                        .FechaRetiro = deFechaRetiro.Date
                    End If
                End If

                If (hfIdEstadoUsuario.Get("IdEstadoUsuario") = 0 And cbEstado.Value = 1) Then
                    If (IsDate(deFechaRetiro.Date)) Then
                        .FechaRetiro = "01/01/1900"
                    End If
                End If
                .IdEstado = cbEstado.Value
                resultado = .ActualizarUsuario()

                If resultado.Valor = 0 Then
                    Dim persona As New GestionPersonaUsuario
                    Dim result As Integer
                    With persona
                        numeroIdentificacion = hfIdEstadoUsuario.Get("numeroIdentificacion")
                        .NumeroIdentificacionActual = numeroIdentificacion
                        .NumeroIdentificacion = TextNumeroIdentificacion.Text
                        .NombresApellidos = TextNombre.Text
                        .Telefono = TextTelefono.Text
                        .Email = TextEmail.Text
                        .IdCiudad = cbCiudad.Value
                        .IdEstado = cbEstado.Value
                        .Usuario = TextUsuario.Text
                        .IdCreador = CInt(Session("userId"))
                        .Usuario = TextUsuario.Text.Trim()
                        result = .ActualizarPersonaUsuario()

                        If result = 1 Then
                            epNotificador.showSuccess("El usuario se actualizo correctamente.")

                        Else
                            epNotificador.showError("Ocurrio un error con la información, por favor verifique.")
                        End If
                    End With

                         ASPxWebControl.RedirectOnCallback("~/Usuarios/ConsultadeUsuarios.aspx?idactu=1")
                Else
                    epNotificador.MostrarErrorYNotificarViaMail("Se presentó el siguiente error al actualizado el usuario: ", "Actualizado de Usurios", resultado.Mensaje)

                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Se presentó el siguiente error al actualizado el usuario." + ex.Message)
        End Try
    End Sub

    Protected Sub cpRegistro_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpRegistro.Callback
        Dim accion As String
        Dim parametro As String

        Dim cadena As String() = e.Parameter.ToString().Split(New Char() {"|"})
        accion = cadena(0)
        parametro = cadena(1)
        Try
            Select Case accion
                Case "validarRequierePDV"
                    mostrarPDV(CbRol.Value)
                    ConsultaPerfil(CbRol.Value)
                Case "CambioTipopersona"
                    cbCargo.ValidationSettings.RequiredField.IsRequired = cbTipo.Value = 1
                    cbEmpresa.ValidationSettings.RequiredField.IsRequired = cbTipo.Value = 1
                    cbUnidadNegocio.ValidationSettings.RequiredField.IsRequired = cbTipo.Value = 1
                Case "ConsultaUnidadDeNegocio"
                    ConsultaUnidadDeNegocio(parametro)
                Case "ActualizarUsuario"
                    ActualizarUsuario()
                Case "SeleccionaPDV"
                    CargarListadoPdv(cbCiudad.Value)
            End Select
        Catch ex As Exception
            epNotificador.showError("Se ejecutó el siguiente error : " + ex.Message)
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

#End Region

#Region "Metodos"

    Private Sub ConsultaUnidadDeNegocio(idEmpresa As Int32)
        Try
            Dim infoUnidadDeNegocio As New UnidadDeNegocioColeccion
            With infoUnidadDeNegocio
                .IdEstado = 1
                .IdEmpresa = idEmpresa
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
            cbUnidadNegocio.Value = -1
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de Unidad De Negocio.")
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
            .DataBind()
            .TextField = "Nombre"
            .ValueField = "IdPerfil"
        End With
        CbPerfil.Items.Insert(0, New ListEditItem("Seleccione un perfil...", -1))
        CbPerfil.Value = -1
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
            cbCargo.Items.Insert(0, New ListEditItem("Seleccione un cargo...", -1))
            cbCargo.Value = -1
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de Cargos. ")
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
            cbTipo.Value = -1
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de Tipo Persona. ")
        End Try
    End Sub

    Private Sub ObtenerListaEmpresa()
        Try
            Dim infoEmpreas As New EmpresaColeccion
            Session("DTinfoEmpresa") = infoEmpreas.CargarComboEmpresas()
            With cbEmpresa
                .DataSource = CType(Session("DTinfoEmpresa"), DataTable)
                .TextField = "nombre"
                .ValueField = "idEmpresa"
                .Items.Insert(0, New ListEditItem("Seleccione una Empresa...", -1))
                .DataBind()
            End With
            cbEmpresa.SelectedIndex = 0
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de Empresas. ")
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
            CbRol.Value = -1
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de Rol. ")
        End Try
    End Sub
    Private Sub RolPuntoDeVenta()
        Try
            Dim perRolPuntoDeVenta As DataTable
            Dim infoPerfilPuntoDeVenta As New Perfil
            perRolPuntoDeVenta = infoPerfilPuntoDeVenta.RolPuntoDeVenta()
            If perRolPuntoDeVenta Is Nothing Then
                epNotificador.showError("No se han configurado los perfiles que requieren un punto de venta asignado")
            Else
                Session("rolPuntoDeVenta") = perRolPuntoDeVenta
            End If
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de Rol por punto de venta. ")
        End Try
    End Sub
    Private Sub RolRequireFechacreacion()
        Try
            Dim _rolPuntoDeVenta As DataTable
            Dim infoPerfilPuntoDeVenta As New Perfil
            _rolPuntoDeVenta = infoPerfilPuntoDeVenta.RolRequireFechacreacion()
            If _rolPuntoDeVenta Is Nothing Then
                epNotificador.showError("No se han configurado los perfiles que requieren Fecha de creación")
            Else
                Session("RolRequireFechacreacion") = _rolPuntoDeVenta
            End If

        Catch
            epNotificador.showError("Error al tratar de cargar el listado de Rol que requieren Fecha de creación ")
        End Try
    End Sub
    Private Sub CargarListadoPdv(Optional ByVal idCiudad As Integer = 0)
        Try
            Dim listaPdv As New PuntoDeVentaColeccion

            With listaPdv
                .IdEstado = 1
                .IdCudadB = idCiudad
                .IdUsuario = IIf(hfIdUsuario.Value = "", 0, hfIdUsuario.Value)
                .LitaPdvNoasignados()
            End With
            With lbPuntodeVenta
                .DataSource = listaPdv
                .DataBind()
                .TextField = "nombrePdv"
                .ValueField = "idPdv"
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Puntos de Venta. ")
        End Try
    End Sub
    Private Sub Cargalistadeciudades()
        Dim infoCiudad As New CiudadColeccion
        Session("DTCiudades") = infoCiudad.ObtenerCiudadesCobox("", 0, 0)
        With cbCiudad
            .DataSource = Session("DTCiudades")
            .ValueField = "idCiudad"
            .TextField = "nombre"
            .DataBind()
        End With
        cbCiudad.Items.Insert(0, New ListEditItem("Seleccione una Ciudad...", -1))
        cbCiudad.SelectedIndex = 0
    End Sub
    Private Sub CargarListadoPdvUsuario(Optional ByVal idCiudad As Integer = 0)
        Try
            Dim listaPdv As New PuntoDeVentaColeccion

            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                .IdEstado = 1
                .IdCudadB = idCiudad
                .IdUsuario = IIf(hfIdUsuario.Value = "", 0, hfIdUsuario.Value)
                .CargarDatos()
            End With
            With lbPuntodeVentaUsuario
                .DataSource = listaPdv
                .DataBind()
                .TextField = "nombrePdv"
                .ValueField = "idPdv"
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Puntos de Venta. ")
        End Try
    End Sub

    Private Sub CargarDatosUsuario()
        If (Convert.ToInt32(Request.QueryString("idUsuario")) <> 0) Then

            Dim lisUsuarios As New UsuarioSistemaColeccion
            With lisUsuarios
                .IdUsuario = Convert.ToInt32(Request.QueryString("idUsuario"))

                .CargarDatos()
            End With
            hfIdUsuario.Value = lisUsuarios(0).IdUsuario
            hfIdPersona.Value = lisUsuarios(0).IdPersona
            hfIdEstadoUsuario.Set("IdEstadoUsuario", IIf(lisUsuarios(0).IdEstado = 1, 1, 0))
            hfIdEstadoUsuario.Set("numeroIdentificacion", lisUsuarios(0).NumeroIdentificacion)
            TextNumeroIdentificacion.Text = lisUsuarios(0).NumeroIdentificacion
            numeroIdentificacion = lisUsuarios(0).NumeroIdentificacion
            TextNombre.Text = lisUsuarios(0).NombreApellido
            TextEmail.Text = lisUsuarios(0).Email
            TextUsuario.Text = lisUsuarios(0).Usuario
            TextTelefono.Text = lisUsuarios(0).Telefono
            cbCiudad.Value = CType(lisUsuarios(0).IdCiudad, Integer)
            CargarListadoPdv(lisUsuarios(0).IdCiudad)
            CbRol.Value = lisUsuarios(0).IdRol
            mostrarPDV(lisUsuarios(0).IdRol)
            cbCargo.ValidationSettings.RequiredField.IsRequired = lisUsuarios(0).IdTipo = 1
            cbEmpresa.ValidationSettings.RequiredField.IsRequired = lisUsuarios(0).IdTipo = 1
            cbUnidadNegocio.ValidationSettings.RequiredField.IsRequired = lisUsuarios(0).IdTipo = 1
            cbTipo.Value = Convert.ToInt32(lisUsuarios(0).IdTipo)
            deFechaIngreso.Date = lisUsuarios(0).FechaIngreso
            cbCargo.Value = CType(lisUsuarios(0).IdCargo, Integer)
            cbEmpresa.Value = CType(lisUsuarios(0).IdEmpresa, Integer)
            cbEstado.Value = lisUsuarios(0).IdEstado
            ConsultaPerfil(lisUsuarios(0).IdRol)
            CbPerfil.Value = lisUsuarios(0).IdPerfil
            ConsultaUnidadDeNegocio(lisUsuarios(0).IdEmpresa)
            cbUnidadNegocio.Value = Convert.ToInt32(lisUsuarios(0).IdUnidadNegocio)
            Session("idUnidadNegocio") = Convert.ToInt32(lisUsuarios(0).IdUnidadNegocio)

        End If
    End Sub
#End Region

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        Response.Redirect("~/Usuarios/ConsultadeUsuarios.aspx")

    End Sub

    Protected Sub cbEstado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbEstado.SelectedIndexChanged

        If (hfIdEstadoUsuario.Get("IdEstadoUsuario") = 1 And cbEstado.Value = 0) Then
            deFechaRetiro.Visible = True
        End If
    End Sub
    Private Sub mostrarPDV(ByVal IdRol As Integer)
        If (Session("rolPuntoDeVenta") IsNot Nothing) Then
            Dim rolPdv As DataTable = CType(Session("rolPuntoDeVenta"), DataTable)
            Dim buscarFila() As DataRow
            buscarFila = rolPdv.Select("idRol = '" & IdRol & "'")
            If (buscarFila.Length > 0) Then
                hdMostarPDV.Set("MostarPDV", "1")
                CargarListadoPdv(cbCiudad.Value)
            Else
                hdMostarPDV.Set("MostarPDV", "0")
            End If
        End If

    End Sub


    Protected Sub cbCiudad_DataBound(sender As Object, e As EventArgs) Handles cbCiudad.DataBound
        If cbCiudad.DataSource Is Nothing AndAlso Session("DTCiudades") IsNot Nothing Then cbCiudad.DataSource = CType(Session("DTCiudades"), DataTable)

    End Sub
    Protected Sub cbCiudad_OnItemsRequestedByFilterCondition_SQL(source As Object, e As ListEditItemsRequestedByFilterConditionEventArgs)

        Dim infoCiudad As New CiudadColeccion
        Session("DTCiudades") = infoCiudad.ObtenerCiudadesCobox(e.Filter, e.BeginIndex + 1, e.EndIndex + 1)
        With cbCiudad
            .DataSource = Session("DTCiudades")
            .ValueField = "idCiudad"
            .ValueType = GetType(Int32)
            .TextField = "nombre"
            .DataBind()
        End With
        cbCiudad.Items.Insert(0, New ListEditItem("Seleccione una Ciudad...", -1))
        cbCiudad.SelectedIndex = 0
    End Sub
    Protected Sub cbCiudad_OnItemRequestedByValue_SQL(source As Object, e As ListEditItemRequestedByValueEventArgs)
        If e.Value = Nothing Then
            Return
        End If
        If Session("DTCiudades") IsNot Nothing Then
            Dim data As DataTable = Session("DTCiudades")
            Dim query = From r In data Where r.Field(Of Int32)("idCiudad") = e.Value Select r
            If query.Count = 0 Then
                Return
            Else
                With cbCiudad
                    .DataSource = query.CopyToDataTable
                    .ValueField = "idCiudad"
                    .ValueType = GetType(Int32)
                    .TextField = "nombre"
                    .DataBind()
                End With
                cbCiudad.Items.Insert(0, New ListEditItem("Seleccione una Ciudad...", -1))
            End If

        End If
    End Sub
End Class