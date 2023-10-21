Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.Comunes
Imports DevExpress.XtraScheduler
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.GestionComercial
Imports System.Net

Public Class GestionAgendamiento
    Inherits System.Web.UI.Page
    Dim controlDia As Control

#Region "Atributos"
    Private diasBogota As Integer
    Private diasCiudades As Integer
#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim resultado As New ResultadoProceso
            If Not Me.IsPostBack Then
                Dim fecha As Date
                Dim bogota As New ConfigValues("MAX_DIAS_AGENDAMIENTO_BTA")
                Dim ciudades As New ConfigValues("MAX_DIAS_AGENDAMIENTO_CIUDAD")
                Dim diaBog As New System.Text.RegularExpressions.Regex(bogota.ConfigKeyValue)
                Dim diaCiudad As New System.Text.RegularExpressions.Regex(ciudades.ConfigKeyValue)
                diasBogota = CInt(diaBog.ToString)
                diasCiudades = CInt(diaCiudad.ToString)
                Session.Remove("dtDisponibilidadEntrega")
                'fecha = DateAdd(DateInterval.Day, +9, DateTime.Now.Date)
                With epNotificador
                    .setTitle("Asignación Agenda Venta - Call Center")
                End With
                If Request.QueryString("id") IsNot Nothing Then
                    Session("IdGestionVenta") = CLng(Request.QueryString("id"))
                    Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=CInt(Session("IdGestionVenta")))
                    ' Se valida la disponibilidad de documentos
                    If miGestion.IdEstadoServicioMensajeria <> Enumerados.EstadosServicioMensajeria.Confirmado AndAlso miGestion.IdEstadoServicioMensajeria <> Enumerados.EstadosServicioMensajeria.Devolución Then
                        Dim miCampania As New Campania(miGestion.IdCampaniaNotus)
                        If miCampania.FechaLlegada IsNot Nothing Then
                            If miCampania.FechaLlegada.ToString.Trim = "" Then
                                resultado = VerificarDisponibilidadInventario(CLng(Session("IdGestionVenta")))
                            Else
                                resultado.Valor = 0
                            End If
                        Else
                            resultado.Valor = 0
                        End If
                    End If

                    If resultado.Valor = 0 Then
                        ' Si existe inventario de documentos, se extrae la disponibilidad de agendamiento.

                        Dim dt As DataTable
                        Dim _fechaIni As Date

                        With miGestion
                            .IdGestionVenta = Session("IdGestionVenta")
                            dt = .ObtenerFechaInicialAgenda
                            _fechaIni = dt.Rows(0).Item("dia")
                        End With

                        resultado = VerificarDisponibilidadEntrega(CLng(Session("IdGestionVenta")))
                        If resultado.Valor = 0 Then
                            If miGestion.IdCiudadCliente = 150 Then
                                fecha = DateAdd(DateInterval.Day, +CInt(diaBog.ToString), DateTime.Now.Date)
                            Else
                                fecha = DateAdd(DateInterval.Day, +CInt(diaCiudad.ToString), DateTime.Now.Date)
                            End If
                            schAgenda.LimitInterval.Start = _fechaIni 'DateTime.Now
                            schAgenda.LimitInterval.End = fecha
                            schAgenda.Start = _fechaIni 'DateTime.Now
                            Session("fechaIni") = _fechaIni
                        Else
                            epNotificador.showWarning(resultado.Mensaje)
                            imgAgenda.Enabled = False
                        End If
                    ElseIf resultado.Valor = 10 Then
                        ''mostrar gv con no disponibilidad
                        imgAgenda.Enabled = False
                        epNotificador.showWarning(resultado.Mensaje)
                    Else
                        epNotificador.showWarning(resultado.Mensaje)
                        imgAgenda.Enabled = False
                    End If
                Else
                    epNotificador.showWarning("No se pudo recuperar el identificador de la gestión de venta, por favor regrese a la pagina anterior.")
                    schAgenda.Visible = False
                    imgAgenda.Enabled = False
                End If
            End If
        Catch ex As Exception
            epNotificador.showError("Se generó un error al cargar la página: " & ex.Message)
        End Try
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Dim resultado As New ResultadoProceso
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Agendar"
                    resultado = EstablecerAgenda()
                    If resultado.Valor = 0 Then
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ProcesoExitoso, "Éxito")
                        imgAgenda.Enabled = False
                    Else
                        mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
                    End If
            End Select

        Catch ex As Exception
            mensajero.MostrarMensajePopUp("Ocurrio un error al generar el registro: " & ex.Message, MensajePopUp.TipoMensaje.ErrorCritico, "Error")
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje
        CType(sender, ASPxCallbackPanel).JSProperties("cpResultado") = resultado.Valor
    End Sub

    Private Sub schAgenda_BeforeExecuteCallbackCommand(sender As Object, e As DevExpress.Web.ASPxScheduler.SchedulerCallbackCommandEventArgs) Handles schAgenda.BeforeExecuteCallbackCommand
        If e.CommandId = SchedulerCallbackCommandId.MenuView Then
            e.Command = New CustomMenuViewCallbackCommand(sender, Session("currentUser") = "Admin")
        End If
    End Sub

    Private Sub schAgenda_InitNewAppointment(sender As Object, e As DevExpress.XtraScheduler.AppointmentEventArgs) Handles schAgenda.InitNewAppointment
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=CLng(Session("IdGestionVenta")))
        Dim miCliente As New ClienteFinal(miGestion.IdCliente)
        Dim cantidadAM As String, cantidadPM As String
        If Session("dtDisponibilidadEntrega") IsNot Nothing Then
            Dim fecha As Date = CDate(e.Appointment.Start)
            Dim fechaF As Date = DateAdd(DateInterval.Day, -1, fecha)
            Dim dtDisponibilidad As DataTable = CType(Session("dtDisponibilidadEntrega"), DataTable)
            Dim Fila As DataRow() = dtDisponibilidad.Select("Fecha <= '" & fecha & "' AND Fecha >= '" & fechaF & "' AND IdJornada =1")
            Dim Fila2 As DataRow() = dtDisponibilidad.Select("Fecha <= '" & fecha & "' AND Fecha >= '" & fechaF & "' AND IdJornada =2")
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

            e.Appointment.Subject = "Agendamiento Banca Seguros: " & miGestion.Cliente
            e.Appointment.Location = "Dirección: " & miCliente.DireccionResidencia
            e.Appointment.Description = "Cantidad cupos AM: " & cantidadAM & ", cantidad cupos PM: " & cantidadPM

        End If
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function VerificarDisponibilidadInventario(ByVal IdGestionVenta As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=IdGestionVenta)
        Dim lisIdProducto As New List(Of Integer)
        Dim objProducto As New GestionComercial.GestionDeVentaDetalleColeccion
        Dim dtProducto As New DataTable
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        Dim objService As New NotusIlsService.NotusIlsService
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim infoWs As New InfoUrlService(objService, True)
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsInfoDisponibilidad
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With objProducto
            .listGestionVenta.Add(CLng(Session("IdGestionVenta")))
            dtProducto = .GenerarDataTable
        End With

        For Each drRow As DataRow In dtProducto.Rows
            lisIdProducto.Add((drRow.Item("IdProducto").ToString))
        Next

        WSInfoFiltros.IdCampania = miGestion.IdCampaniaNotus
        WSInfoFiltros.IdCiudad = miGestion.IdCiudadCliente
        WSInfoFiltros.ListProductos = lisIdProducto.ToArray()
        Wsresultado = objService.ConsultarDisponibilidadDocumentos(WSInfoFiltros, dsDatos)
        dtDatos = dsDatos.Tables(0)

        If dtDatos.Rows.Count <> 0 Then
            Dim dvDatos As DataView = dtDatos.DefaultView
            dvDatos.RowFilter = "cantidadDisponible < cantidadSolicitada "
            Dim dtAux As DataTable = dvDatos.ToTable()
            If dtAux.Rows.Count <> 0 Then
                Session("dtDisponibilidad") = dtAux
                resultado.EstablecerMensajeYValor(10, "No existe disponibilidad de documentos para realizar el agendamiento.")
            Else
                resultado.Valor = 0
            End If
        Else
            resultado.EstablecerMensajeYValor(1, "No se logró establecer la configuración de productos, por favor verifique la campaña asociada.")
        End If
        Return resultado
    End Function

    Private Function VerificarDisponibilidadEntrega(ByVal IdGestionVenta As Long) As ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=IdGestionVenta)
        Dim resultado As New ResultadoProceso
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        Dim WSInfoFiltros As New NotusExpressBusinessLayer.NotusIlsService.WsInfoCapacidadEntrega
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso

        With WSInfoFiltros
            .FechaInicial = CDate(DateAdd(DateInterval.Day, +1, DateTime.Now.Date))
            '.FechaFinal = CDate(DateAdd(DateInterval.Day, +9, DateTime.Now.Date))
            If miGestion.IdCiudadCliente = 150 Then
                .FechaFinal = DateAdd(DateInterval.Day, +CInt(diasBogota.ToString), DateTime.Now.Date)
            Else
                .FechaFinal = DateAdd(DateInterval.Day, +CInt(diasCiudades.ToString), DateTime.Now.Date)
            End If
            .IdCiudad = miGestion.IdCiudadCliente
            .IdEmpresa = New ConfigValues("ID_EMPRESA").ConfigKeyValue
        End With
        Wsresultado = objService.ConsultarCapacidadEntrega(WSInfoFiltros, dsDatos)
        dtDatos = dsDatos.Tables(0)
        If dtDatos.Rows.Count <> 0 Then
            Session("dtDisponibilidadEntrega") = dtDatos
        Else
            resultado.EstablecerMensajeYValor(1, "No se logró establecer la disponibilidad de entregas para realizar el agendamiento.")
        End If
        Return resultado
    End Function

    Private Function EstablecerAgenda() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If Session("customEvent") IsNot Nothing Then
            Dim oAgenda = DirectCast(Session("customEvent"), CustomEvent)
            Dim fecha As Date = CDate(oAgenda.StartTime.ToShortDateString())
            Dim hora As String = oAgenda.StartTime.ToShortTimeString()
            If oAgenda.StartTime > DateAdd(DateInterval.Day, -1, DateTime.Now) Then
                Dim idJornada As Integer
                If (hora.IndexOf("a.m")) <> -1 Or (hora.IndexOf("a. m")) <> -1 Then
                    idJornada = 1
                Else
                    idJornada = 2
                End If
                resultado = RegistrarServicio(idJornada, fecha)
            Else
                resultado.EstablecerMensajeYValor(1, "La fecha de agendamiento debe ser superior a la fecha actual.")
                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
            End If
        Else
            resultado.EstablecerMensajeYValor(2, "Por favor verifique que se encuentre asignada una fecha.")
            mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico, "Alerta")
        End If
        Return resultado
    End Function

    Public Function RegistrarServicio(ByVal idJornada As Integer, ByVal fecha As Date) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miGestion As New GestionComercial.GestionDeVenta(idGestionVenta:=CLng(Session("IdGestionVenta")))
        Dim lisIdProducto As New List(Of Integer)
        Dim lisTipoProducto As New List(Of String)
        Dim dtProducto As New DataTable
        Dim objProducto As New GestionComercial.GestionDeVentaDetalleColeccion
        Dim miCliente As New ClienteFinal(miGestion.IdCliente)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objService As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objService, True)
        Dim WSInfoServicio As New NotusExpressBusinessLayer.NotusIlsService.WsRegistroServicioMensajeria
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Dim idServicio As Long

        With objProducto
            .listGestionVenta.Add(CLng(Session("IdGestionVenta")))
            dtProducto = .GenerarDataTable
        End With
        For Each drRow As DataRow In dtProducto.Rows
            lisIdProducto.Add((drRow.Item("IdProducto").ToString))
            lisTipoProducto.Add((drRow.Item("ProductoTipoServicio").ToString))
        Next
        With WSInfoServicio
            .FechaAgenda = fecha
            .IdJornada = idJornada
            .IdEmpresa = CInt(New ConfigValues("ID_EMPRESA").ConfigKeyValue)
            .IdEstado = NotusIlsService.EstadoServicio.PendienteAprobacionCalidad
            .Nombre = miCliente.NombreApellido
            .Identicacion = miCliente.NumeroIdentificacion
            .IdCiudad = miCliente.IdCiudadResidencia
            .Direccion = miCliente.DireccionResidencia
            .Telefono = miCliente.Celular
            .IdCampania = miGestion.IdCampaniaNotus
            .ListProductos = lisIdProducto.ToArray()
            .ListTipoServicio = lisTipoProducto.ToArray()
            .ActividadLaboral = miGestion.ActividadLaboral
            .CodOficinaCliente = miCliente.CodOficina
            .Observacion = miGestion.ObservacionCallCenter
            If miGestion.IdServicioNotus = 0 Then
                Wsresultado = objService.RegistrarServicioWS(WSInfoServicio, idServicio)
            Else
                If miGestion.IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.Confirmado Or _
                  miGestion.IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.Devolución Or _
                  miGestion.IdEstadoServicioMensajeria = Enumerados.EstadosServicioMensajeria.PendienteAprobacionCalidad Then
                    .IdServicioMensajeria = miGestion.IdServicioNotus
                    Wsresultado = objService.ActualizarServicioWS(WSInfoServicio)
                Else
                    Wsresultado.Valor = 2
                    Wsresultado.Mensaje = "El servicio se encuentra en un estado diferente a confirmado, no se puede realizar la modificación."
                End If
            End If

        End With
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje

        If Wsresultado.Valor = 0 Then
            resultado = ActualizarGestionVenta(idServicio, miGestion.IdGestionVenta, fecha)
            If resultado.Valor = 0 Then
                resultado.Mensaje = "Servicio agendado satisfactoriamente."
            End If
        End If

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

#End Region

#Region "Establecer Agenda"

    Private objectInstance As CustomEventDataSource

    Protected Sub appointmentsDataSource_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
        Me.objectInstance = New CustomEventDataSource(GetCustomEvents())
        e.ObjectInstance = Me.objectInstance
    End Sub

    Private Function GetCustomEvents() As CustomEventList
        Dim events As CustomEventList = TryCast(Session("CustomEventListData"), CustomEventList)
        If events Is Nothing Then
            events = New CustomEventList()
            Session("CustomEventListData") = events
        End If
        Return events
    End Function

#End Region

#Region "Clases Auxiliares"

    Public Class CustomMenuViewCallbackCommand
        Inherits MenuViewCallbackCommand

        Dim allowAppointmentCreate As Boolean
        Dim currentCommandProhibited As Boolean

        Public Sub New(control As ASPxScheduler, allowAppointmentCreate As Boolean)
            MyBase.New(control)
            Me.allowAppointmentCreate = allowAppointmentCreate
        End Sub

        Protected Overrides Sub ParseParameters(parameters As String)
            Dim isNewAppointmentCommand As Boolean = (parameters = "NewAllDayEvent" OrElse parameters = "NewRecurringAppointment" OrElse parameters = "NewRecurringEvent")
            currentCommandProhibited = isNewAppointmentCommand AndAlso Not allowAppointmentCreate
            MyBase.ParseParameters(parameters)

        End Sub

        Protected Overrides Sub ExecuteCore()
            AddHandler Me.Control.CustomJSProperties, AddressOf schAgenda_CustomJSProperties
            'Me.Control = New CustomJSPropertiesEventHandler(schAgenda_CustomJSProperties)
            If Not currentCommandProhibited Then
                MyBase.ExecuteCore()
            End If
        End Sub

        Sub schAgenda_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
            e.Properties.Add("cpShowWarning", currentCommandProhibited)
        End Sub

    End Class

#End Region

End Class