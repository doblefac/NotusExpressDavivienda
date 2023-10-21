Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Reportes
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.PresupuestoGestiones
Imports DevExpress.Web

Public Class RegistroDePresupuestoDiarioDeGestiones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            With epNotificador
                .setTitle("Registro de Presupuesto Diario de Gestiones")
                .showReturnLink("~/Administracion/Default.aspx")
            End With
            deFecha.Date = CDate(Now.ToShortDateString)
            deFecha.MaxDate = CDate(Now.ToShortDateString)
            If Now.DayOfWeek = DayOfWeek.Monday Then
                deFecha.MinDate = CDate(Now.AddDays(-2).ToShortDateString)
            Else
                deFecha.MinDate = CDate(Now.AddDays(-1).ToShortDateString)
            End If
            CargarDatosIniciales()
            CargarListadoDeAsesores()
            CargarListadoPdv()
            CargarListadoDeCortes(True)
            CargarListadoDeNovedades()
        Else
            If cpGeneral.IsCallback Then
                CargarListadoPdv()
                CargarListadoDeSupervisores()
                CargarListadoDeCortes()
            End If
        End If
    End Sub

    Private Sub CargarListadoDeAsesores()
        cboAsesor.Items.Clear()
        Try
            Dim listaAsesor As AsesorComercialColeccion
            listaAsesor = New AsesorComercialColeccion
            With listaAsesor
                .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                .IdEstado = 1
                .IncluirSupervisorComoAsesor = 1
                .CargarDatos()
            End With
            With cboAsesor
                .DataSource = listaAsesor
                .TextField = "NombreApellido"
                .ValueField = "IdUsuarioSistema"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Asesores. ")
        End Try
        With cboAsesor
            .Items.Insert(0, New ListEditItem("Seleccione un Asesor", Nothing))
            If .SelectedIndex = -1 Then .SelectedIndex = 0
        End With
    End Sub

    Private Sub CargarListadoPdv()
        Try
            cboPdv.Items.Clear()
            If cboAsesor.Value IsNot Nothing AndAlso CInt(cboAsesor.Value) > 0 Then
                Dim listaPdv As New PuntoDeVentaColeccion
                With listaPdv
                    .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                    .IdEstado = 1
                    .IdUsuario = CInt(cboAsesor.Value)
                    .CargarDatos()
                End With
                With cboPdv
                    .DataSource = listaPdv
                    .TextField = "NombrePdv"
                    .ValueField = "IdPdv"
                    .AutoResizeWithContainer = True
                    .DataBindItems()
                End With
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Puntos de Venta. ")
        End Try
        With cboPdv
            .Items.Insert(0, New ListEditItem("Seleccione un Punto de Venta", Nothing))
            If .SelectedIndex = -1 Then .SelectedIndex = 0
        End With
    End Sub

    Private Sub CargarListadoDeSupervisores()
        cboSupervisor.Items.Clear()
        Try
            If cboPdv.Value IsNot Nothing AndAlso CInt(cboPdv.Value) > 0 Then
                Dim listaSupervisor As SupervisorComercialColeccion
                listaSupervisor = New SupervisorComercialColeccion
                With listaSupervisor
                    .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                    .IdEstado = 1
                    .IdPdv = CInt(cboPdv.Value)
                    .CargarDatos()
                End With
                With cboSupervisor
                    .DataSource = listaSupervisor
                    .TextField = "NombreApellido"
                    .ValueField = "IdUsuarioSistema"
                    .AutoResizeWithContainer = True
                    .DataBindItems()
                End With
            Else
                cboSupervisor.SelectedIndex = -1
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Supervisores. ")
        End Try
    End Sub

    Private Sub CargarListadoDeCortes(Optional ByVal forzarConsulta As Boolean = False)
        cboCorte.Items.Clear()
        Try
            Dim listaCorte As CorteReportePresupuestoGestionesColeccion
            If Session("listaCortePresupuestoGestiones") Is Nothing OrElse forzarConsulta Then
                listaCorte = New CorteReportePresupuestoGestionesColeccion

                With listaCorte
                    .Fecha = deFecha.Date
                    .CargarDatos()
                End With
            Else
                listaCorte = CType(Session("listaCortePresupuestoGestiones"), CorteReportePresupuestoGestionesColeccion)
            End If

            With cboCorte
                .DataSource = listaCorte
                .TextField = "Descripcion"
                .ValueField = "IdCorte"
                .AutoResizeWithContainer = True
                .DataBindItems()
                If .Items.Count <> 1 Then .Items.Insert(0, New ListEditItem("Seleccione un Corte", Nothing))
                .SelectedIndex = 0
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Cortes. ")
        End Try
    End Sub

    Private Sub CargarListadoDeNovedades()
        cboNovedad.Items.Clear()
        Try
            Dim listaNovedad As New NovedadReportePresupuestoGestionesVentaColeccion

            With listaNovedad
                .IdEstado = 1
                .CargarDatos()
            End With
            With cboNovedad
                .DataSource = listaNovedad
                .TextField = "Descripcion"
                .ValueField = "IdNovedad"
                .AutoResizeWithContainer = True
                .DataBindItems()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Novedades. ")
        Finally
            With cboNovedad
                .Items.Insert(0, New ListEditItem("Seleccione una Novedad", Nothing))
                .SelectedIndex = 0
            End With
        End Try
    End Sub

    Private Sub cboPdv_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cboPdv.Callback
        If e.Parameter = "cargarLista" Then
            CargarListadoPdv()
            cboSupervisor.Items.Clear()
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End If
    End Sub

    Private Sub cboSupervisor_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cboSupervisor.Callback
        If e.Parameter = "cargarLista" Then
            CargarListadoDeSupervisores()
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End If
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        If e.Parameter = "registrarDatos" Then
            RegistrarPresupuesto()
        ElseIf e.Parameter = "limpiarCampos" Then
            LimpiarCampos()
        End If
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub CargarDatosIniciales()
        txtTotRechazadasPdv.Text = 0
        txtTotExpressPdv.Text = 0
        txtTotNormalesPdv.Text = 0

        txtTotRechazadasEmp.Text = 0
        txtTotExpressEmp.Text = 0
        txtTotNormalesEmp.Text = 0
    End Sub

    Private Sub cboCorte_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cboCorte.Callback
        If e.Parameter = "cargarLista" Then
            CargarListadoDeCortes(True)
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End If
    End Sub

    Private Sub RegistrarPresupuesto()
        Dim resultado As ResultadoProceso
        Try
            Dim presupuesto As New PresupuestoDeGestionesDeVenta

            With presupuesto
                '***Datos de Cabecera***'
                .IdCorte = CByte(cboCorte.Value)
                .IdPdv = CInt(cboPdv.Value)
                .IdUsuarioSupervisor = CInt(cboSupervisor.Value)
                .IdUsuarioAsesor = CInt(cboAsesor.Value)
                .IdUsuarioRegistra = CInt(Session("userId"))
                .FechaRegistro = deFecha.Date
                If cboNovedad IsNot Nothing Then .IdNovedad = CShort(cboNovedad.Value)
                If Not String.IsNullOrEmpty(txtObservacion.Text) Then .Observaciones = txtObservacion.Text.Trim

                '***Datos de Detalle***'
                '***Gestiones en Punto de Venta***'
                .Detalle.Adicionar(New DetallePresupuestoDeGestionesDeVenta(1, txtTotExpressPdv.Text, 1, 1))
                .Detalle.Adicionar(New DetallePresupuestoDeGestionesDeVenta(1, txtTotNormalesPdv.Text, 1, 2))
                .Detalle.Adicionar(New DetallePresupuestoDeGestionesDeVenta(2, txtTotRechazadasPdv.Text, 1))

                '***Gestiones en Empresariales***'
                .Detalle.Adicionar(New DetallePresupuestoDeGestionesDeVenta(1, txtTotExpressEmp.Text, 2, 1))
                .Detalle.Adicionar(New DetallePresupuestoDeGestionesDeVenta(1, txtTotNormalesEmp.Text, 2, 2))
                .Detalle.Adicionar(New DetallePresupuestoDeGestionesDeVenta(2, txtTotRechazadasEmp.Text, 2))

                resultado = .Registrar()

                If resultado.Valor = 0 Then
                    epNotificador.showSuccess("Registro exitoso con transacción No. " & .IdPresupuesto.ToString)
                    LimpiarCampos()
                Else
                    epNotificador.showError(resultado.Mensaje)
                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de registrar datos.")
        End Try
        
    End Sub

    Public Sub LimpiarCampos()
        deFecha.Date = CDate(Now.ToShortDateString)
        CargarDatosIniciales()
        cboAsesor.SelectedIndex = -1
        CargarListadoPdv()
        CargarListadoDeSupervisores()
        CargarListadoDeCortes()
        cboNovedad.SelectedIndex = -1
        txtObservacion.Text = ""
    End Sub

End Class