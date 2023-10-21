Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.ControlAcceso
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion

Public Class AdministradorUsuariosPdv
    Inherits System.Web.UI.Page

#Region "Atributos"

#End Region
#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            epNotificador.clear()
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Administración de Usuarios Por Punto de Venta")
                    .showReturnLink("~/Usuarios/ConsultadeUsuarios.aspx")
                End With
                ObtenerListaCiudad()
                ObtenerListaRol()
                ObtenerListaTipoPersona()
                ObtenerListaCargos()
                RolPuntoDeVenta()
                RolRequireFechacreacion()
                CargarDatosUsuario()
                CargarListadoPdv()
                CargarListadoPdvUsuario()
            End If

        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la página. ")
        End Try
    End Sub

    Protected Sub BtnActualizarUsuarioClick(sender As Object, e As EventArgs) Handles btnActualizarUsuario.Click
        Try
            Dim listaPdv As String = String.Empty
            Dim resultado As New ResultadoProceso()
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
                .ListaPdv = listaPdv
                resultado = .ActualizarPDVUsuario()

                If resultado.Valor = 0 Then
                    epNotificador.showSuccess("El usuario fue actualizado correctamente")

                Else
                    'epNotificador.showError("Se presentó el siguiente error al crear el usuario: ")
                    epNotificador.MostrarErrorYNotificarViaMail("Se presentó el siguiente error al actualizado el usuario: ", "Actualizado de Usurios", resultado.Mensaje)

                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Se presentó el siguiente error al actualizado el usuario." + ex.Message)
        End Try

    End Sub


    Protected Sub CbPerfilCallback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles CbPerfil.Callback
        If (e.Parameter <> "") Then
            ConsultaPerfil(e.Parameter)

        End If
    End Sub
 

    Protected Sub CbCargoCallback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cbCargo.Callback
        Dim rolPdv As DataTable = CType(Session("rolPuntoDeVenta"), DataTable)
        Dim buscarFila() As DataRow
        buscarFila = rolPdv.Select("idRol = '" & CbRol.Value & "'")

        If (buscarFila.Length > 0) Then
            CType(sender, ASPxComboBox).JSProperties("cpRequierePdv") = 1
        Else
            CType(sender, ASPxComboBox).JSProperties("cpRequierePdv") = 0
        End If



    End Sub

#End Region

#Region "Metodos"


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
    Private Sub ObtenerListaCiudad()
        Try
            Dim infoCiudad As New CiudadColeccion
            infoCiudad.CargarDatos()
            With cbCiudad
                .DataSource = infoCiudad
                .TextField = "ciudadDepartamento"
                .ValueField = "idCiudad"
                .DataBindItems()
            End With
            cbCiudad.Items.Insert(0, New ListEditItem("Seleccione una Ciudad...", -1))
            cbCiudad.Value = -1
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de ciudades. ")
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
                .IdUsuario = hfIdUsuario.Value
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

    Private Sub CargarListadoPdvUsuario(Optional ByVal idCiudad As Integer = 0)
        Try
            Dim listaPdv As New PuntoDeVentaColeccion

            With listaPdv
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                .IdEstado = 1
                .IdCudadB = idCiudad
                .IdUsuario = hfIdUsuario.Value
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
        Dim lisUsuarios As New UsuarioSistemaColeccion
        With lisUsuarios
            .IdUsuario = Convert.ToInt32(Request.QueryString("idUsuario"))
            .CargarDatos()
        End With
        hfIdUsuario.Value = lisUsuarios(0).IdUsuario
        hfIdPersona.Value = lisUsuarios(0).IdPersona
        TextNumeroIdentificacion.Text = lisUsuarios(0).NumeroIdentificacion
        TextNombre.Text = lisUsuarios(0).NombreApellido
        TextEmail.Text = lisUsuarios(0).Email
        TextUsuario.Text = lisUsuarios(0).Usuario
        TextTelefono.Text = lisUsuarios(0).Telefono
        cbCiudad.Value = lisUsuarios(0).IdCiudad
        CbRol.Value = lisUsuarios(0).IdRol
        cbTipo.Value = Convert.ToInt32(lisUsuarios(0).IdTipo)
        cbCargo.Value = lisUsuarios(0).IdCargo
        ConsultaPerfil(lisUsuarios(0).IdRol)
        CbPerfil.Value = lisUsuarios(0).IdPerfil
        cbCargo.ValidationSettings.RequiredField.IsRequired = cbTipo.Value = 1
    End Sub
#End Region

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        Response.Redirect("~/Usuarios/ConsultadeUsuarios.aspx")

    End Sub
End Class