Imports DevExpress.Web
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.ControlAcceso

Public Class ConsultadeUsuarios
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private Shared infoPermiso As InfoPermisoOpcionFuncionalRestringida
    Private _permisos As DataTable
#End Region
#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            epNotificador.clear()
            If Not Me.IsPostBack Then
                With epNotificador
                    .setTitle("Consulta de Usuarios")
                    .showReturnLink("~/Administracion/Default.aspx")
                End With
                RolPuntoDeVenta()
                ObtenerListaCiudad()
                ObtenerListaRol()
                ObtenerListaCargos()
                If Request.QueryString("idactu") IsNot Nothing Then
                    epNotificador.showSuccess("El usuario se actualizo de forma correcta")
                End If
            End If
            If cbCiudad.IsCallback Then cbCiudad.DataBind()
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la página. ")
        End Try
    End Sub

    Protected Sub gvUsuarios_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvUsuarios.CustomCallback
        Try
            Select Case e.Parameters
                Case "limpiar"
                    Session.Remove("dtLisUsuarios")
                    gvUsuarios.DataSource = Nothing
                    gvUsuarios.DataBind()

            End Select

        Catch ex As Exception
            epNotificador.showError("Se presento un error al cargar el usuario: " & ex.Message)
        End Try
        CType(sender, ASPxGridView).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub
    Protected Sub gvUsuarios_DataBinding(sender As Object, e As EventArgs) Handles gvUsuarios.DataBinding
        If Session("dtLisUsuarios") IsNot Nothing Then gvUsuarios.DataSource = CType(Session("dtLisUsuarios"), UsuarioSistemaColeccion)
    End Sub

    Private Sub ConsultarUsuarios()
        Try
            PerilCreador()
            Dim LisUsuarios As New UsuarioSistemaColeccion
            With LisUsuarios
                .NumeroIdentificacion = TextNumeroIdentificacion.Text
                .NombreApellido = TextNombre.Text
                .IdCiudad = cbCiudad.Value
                .IdCargo = cbCargo.Value
                .IdRol = CbRol.Value
                .CargarDatos()
            End With
            Session("dtLisUsuarios") = LisUsuarios
            With gvUsuarios
                .PageIndex = 0
                .DataSource = Session("dtLisUsuarios")
                .DataBind()
            End With

        Catch ex As Exception
            epNotificador.showError("Se presento un error al realizar la consulta de usuario: " & ex.Message)
        End Try
    End Sub
    Private Sub PerilCreador()

        Dim LisUsuarios As New UsuarioSistemaColeccion
        With LisUsuarios
            .IdUsuario = CInt(CInt(Session("userId")))
            .CargarDatos()
        End With
        If LisUsuarios.Count > 0 Then
            Session("IdPerfil") = LisUsuarios(0).IdPerfil
        End If

        infoPermiso = New InfoPermisoOpcionFuncionalRestringida("AdministradorUsuariosPdv.aspx")
        Session("PermisosAdministradorPdv") = infoPermiso.ListaPermisos
        infoPermiso = New InfoPermisoOpcionFuncionalRestringida("EdiciondeUsuario.aspx")
        Session("PermisosEdiciondeUsuario") = infoPermiso.ListaPermisos

    End Sub
    Protected Sub LinkInit(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim linkVer As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkVer.NamingContainer, GridViewDataItemTemplateContainer)
            _permisos = Session("PermisosEdiciondeUsuario")
            Dim IdPerfil = Session("IdPerfil")
            'infoPermiso.ListaPermisos.
            Dim filtro As String = "idPerfil in (" & IdPerfil & ")"
            With (linkVer)
                .ClientSideEvents.Click = linkVer.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)
                If _permisos.Select(filtro).Count > 0 Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
        Catch ex As Exception
            epNotificador.showError("No fué posible establecer los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub
    Protected Sub LinkPdvInit(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim linkVer As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(linkVer.NamingContainer, GridViewDataItemTemplateContainer)
            _permisos = Session("PermisosAdministradorPdv")
            Dim IdPerfil = Session("IdPerfil")
            'infoPermiso.ListaPermisos.
            Dim filtro As String = "idPerfil in (" & IdPerfil & ")"
            Dim rolPdv As DataTable = CType(Session("rolPuntoDeVenta"), DataTable)
            Dim buscarFila() As DataRow
            Dim lisUsuarios As New UsuarioSistemaColeccion
            With lisUsuarios
                .IdUsuario = Convert.ToInt32(templateContainer.KeyValue)
                .CargarDatos()
            End With
            buscarFila = rolPdv.Select("idRol = '" & lisUsuarios(0).IdRol & "'")

            With linkVer
                .ClientSideEvents.Click = linkVer.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)
                If _permisos.Select(filtro).Count > 0 Then
                    If (buscarFila.Length > 0) Then
                        .Visible = True
                    Else
                        .Visible = False
                    End If
                Else
                    .Visible = False
                End If
            End With

        Catch ex As Exception
            epNotificador.showError("No fué posible establecer los permisos de las funcionalidades: " & ex.Message)
        End Try
    End Sub
    Protected Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Dim accion As String
        Dim parametro As String

        Dim cadena As String() = e.Parameter.ToString().Split(New Char() {"|"})

        accion = cadena(0)
        parametro = cadena(1)
        Try
            Select Case accion
                Case "Consultar"
                    ConsultarUsuarios()
                Case "EditUsuario"
                    ASPxWebControl.RedirectOnCallback("~/Usuarios/EdiciondeUsuario.aspx?idUsuario=" + parametro.ToString())
                Case "AsignarPDV"
                    ASPxWebControl.RedirectOnCallback("~/Usuarios/AdministradorUsuariosPdv.aspx?idUsuario=" + parametro.ToString())
            End Select
        Catch ex As Exception
            epNotificador.showError("Se ejecutó el siguiente error : " + ex.Message)
        End Try
    End Sub
    Protected Sub cbCiudad_OnItemRequestedByValue_SQL(source As Object, e As ListEditItemRequestedByValueEventArgs)
        If e.Value = Nothing Then
            Return
        End If
        If Session("DTCiudades") IsNot Nothing Then
            Dim data As DataTable = CType(Session("DTCiudades"), DataTable)
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
    Protected Sub cbCiudad_OnItemsRequestedByFilterCondition_SQL(source As Object, e As ListEditItemsRequestedByFilterConditionEventArgs)
        Dim infoCiudad As New CiudadColeccion
        infoCiudad.CargarDatos()
        Session("DTCiudades") = infoCiudad.ObtenerCiudadesCobox(e.Filter, e.BeginIndex + 1, e.EndIndex + 1)
        With cbCiudad
            .DataSource = CType(Session("DTCiudades"), DataTable)
            .ValueField = "idCiudad"
            .ValueType = GetType(Int32)
            .TextField = "nombre"
            .DataBind()
        End With
    End Sub
    Protected Sub cbCiudad_DataBound(sender As Object, e As EventArgs) Handles cbCiudad.DataBound
        If cbCiudad.DataSource Is Nothing AndAlso Session("DTCiudades") IsNot Nothing Then cbCiudad.DataSource = CType(Session("DTCiudades"), DataTable)

    End Sub
#End Region

#Region "Metodos"
    Private Sub RolPuntoDeVenta()
        Try
            Dim _perRolPuntoDeVenta As DataTable
            Dim infoPerfilPuntoDeVenta As New Perfil
            _perRolPuntoDeVenta = infoPerfilPuntoDeVenta.RolPuntoDeVenta()
            If _perRolPuntoDeVenta Is Nothing Then
                epNotificador.showError("No se han configurado los perfiles que requieren un punto de venta asignado")
            Else
                Session("rolPuntoDeVenta") = _perRolPuntoDeVenta
            End If


        Catch
            epNotificador.showError("Error al tratar de cargar el listado de Rol por punto de venta. ")
        End Try
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
            epNotificador.showError("Error al tratar de cargar el listado de Cargos. ")
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
            epNotificador.showError("Error al tratar de cargar el listado de Rol. ")
        End Try
    End Sub

    Private Sub ObtenerListaCiudad()
        Try
            Dim infoCiudad As New CiudadColeccion
            Session("DTCiudades") = infoCiudad.ObtenerCiudadesCobox("", 0, 0)

            With cbCiudad
                .DataSource = CType(Session("DTCiudades"), DataTable)
                .TextField = "ciudadDepartamento"
                .ValueField = "idCiudad"
                .DataBindItems()
            End With
            cbCiudad.Items.Insert(0, New ListEditItem("Seleccione una Ciudad...", -1))
            cbCiudad.SelectedIndex = 0
        Catch
            epNotificador.showError("Error al tratar de cargar el listado de ciudades. ")
        End Try
    End Sub

    Private Sub AplicarPermisos()
        If infoPermiso IsNot Nothing AndAlso infoPermiso.ListaPermisos IsNot Nothing Then
            'lnkEditarUsuario. = infoPermiso.PermitirAcceso("tmrActualizador")
        End If
    End Sub
#End Region

End Class