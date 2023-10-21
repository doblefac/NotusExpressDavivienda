<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestionAgendamiento.aspx.vb" Inherits="NotusExpress.GestionAgendamiento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Notus Express - Gestión De Agendamiento::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.12.4.js"></script>

    <script language="javascript" type="text/javascript">
        function TamanioVentana() {
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible 
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }
        }

        function mostrar() {
            alert("nada"); 
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
        <ClientSideEvents EndCallback="function(s,e){ 
              if(s.cpResultado!=0){
                ActualizarEncabezado(s,e);}
            }" />
        <PanelCollection>
            <dx:PanelContent>
                <div id="divEncabezado">
                    <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
                    <br />
                </div>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <div>
                                <dx:ASPxImage ID="imgAgenda" runat="server" ImageUrl="../img/CalendarDB.png" ToolTip="Registrar Agendamiento"
                                    Cursor="pointer">
                                    <ClientSideEvents Click ="function(s, e){
                                        if(confirm('Esta seguro que desea realizar el agendamiento?')){
                                         cpGeneral.PerformCallback('Agendar');   
                                        }
                                    }" />
                                </dx:ASPxImage> 
                                <div>
                                    <dx:ASPxLabel ID="lblComentario" runat="server" Text="Generar Agendamiento."
                                        CssClass="comentario" Width="270px" Font-Size="Small" Font-Bold="False" Font-Italic="True"
                                        Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <%--fecha agendamiento a cambiar--%>
                        <td valign="top">
                            <dx:ASPxScheduler ID="schAgenda" runat="server" Start="2014-03-03" ActiveViewType="WorkWeek"
                                ClientIDMode="AutoID" ClientInstanceName="schAgenda" GroupType="Date" AppointmentDataSourceID="appointmentDataSource">
                                    <%--OnAppointmentRowInserted="schAgenda_AppointmentRowInserted"> --%>
                                <ClientSideEvents EndCallback ="function(s, e){
                                    if (s.cpShowWarning)
                                     alert('El usuario que ha iniciado sesión no puede crear citas.');
                                }" />
                                <Views>
                                    <%--<DayView ShowWorkTimeOnly="True">
                                        <TimeRulers>
                                            <dx:TimeRuler AlwaysShowTimeDesignator="true">
                                                <timezone id="Colombia"></timezone>
                                            </dx:TimeRuler>
                                            <dx:TimeRuler></dx:TimeRuler>
                                            <dx:TimeRuler></dx:TimeRuler>
                                        </TimeRulers>
                                        <WorkTime End="18:00:00" Start="07:00:00" />
                                    </DayView>--%>
                                    <DayView><TimeRulers>
                                    <dx:TimeRuler></dx:TimeRuler>
                                    </TimeRulers>

<AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4"></AppointmentDisplayOptions>
                                    </DayView>
                                    <WorkWeekView ShowWorkTimeOnly="true" TimeScale="01:00:00" ShowMoreButtonsOnEachColumn="true" ShowFullWeek ="true">
                                        <TimeRulers>
                                            
<dx:TimeRuler></dx:TimeRuler>
                                            
                                        </TimeRulers>
                                        <WorkTime End="18:00:00" Start="07:00:00" />

<AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4"></AppointmentDisplayOptions>
                                    </WorkWeekView>

<WeekView ViewSelectorItemAdaptivePriority="4"></WeekView>

<MonthView ViewSelectorItemAdaptivePriority="5"></MonthView>

                                    <TimelineView IntervalCount="2" NavigationButtonVisibility="Always" 
                                        ShortDisplayName="">
                                    </TimelineView>

<FullWeekView ViewSelectorItemAdaptivePriority="7">
<TimeRulers>
<dx:TimeRuler></dx:TimeRuler>
</TimeRulers>

<AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4"></AppointmentDisplayOptions>
</FullWeekView>

<AgendaView ViewSelectorItemAdaptivePriority="1"></AgendaView>
                                </Views>
                                <OptionsCustomization AllowAppointmentConflicts="Forbidden" AllowAppointmentDrag="None"
                                        AllowAppointmentDragBetweenResources="None" AllowAppointmentResize="None" AllowAppointmentCopy="None"
                                        AllowAppointmentDelete="None" />
                                    <OptionsBehavior ShowViewNavigator="False" 
                                    ShowViewNavigatorGotoDateButton="False" ShowViewSelector="False" 
                                    ShowViewVisibleInterval="False" />
                                    <OptionsView FirstDayOfWeek="Monday"/>
                                    <Storage>
                                        <Appointments>
                                            <Mappings AppointmentId="Id" Start="StartTime" End="EndTime" Subject="Subject" AllDay="AllDay"
                                                Description="Description" Label="Label" Location="Location" RecurrenceInfo="RecurrenceInfo"
                                                ReminderInfo="ReminderInfo" Status="Status" Type="EventType" />
                                        </Appointments>
                                    </Storage>
                            </dx:ASPxScheduler>
                            
                        </td> 
                        <td valign="top">
                            <dx:ASPxDateNavigator ID="dnAgenda" runat="server" MasterControlID="schAgenda">
                                <Properties Rows="2">
                                </Properties>
                            </dx:ASPxDateNavigator>
                        </td> 
                    </tr>
                </table>
                <asp:ObjectDataSource ID="appointmentDataSource" runat="server" DataObjectTypeName="NotusExpressBusinessLayer.GestionComercial.CustomEvent"
                    SelectMethod="SelectMethodHandler" TypeName="NotusExpressBusinessLayer.GestionComercial.CustomEventDataSource"
                    InsertMethod="InsertMethodHandler" OnObjectCreated="appointmentsDataSource_ObjectCreated"
                    OldValuesParameterFormatString="original_{0}">
                </asp:ObjectDataSource>
                <msgp:MensajePopUp ID="mensajero" runat="server" />
            </dx:PanelContent>
        </PanelCollection> 
    </dx:ASPxCallbackPanel>
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
