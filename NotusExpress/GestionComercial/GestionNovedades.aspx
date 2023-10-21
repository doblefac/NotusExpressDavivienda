<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestionNovedades.aspx.vb" Inherits="NotusExpress.GestionNovedades" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Gestión de Novedades</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        String.prototype.trim = function () { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }

        function EjecutarCallbackGeneral(parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                cpResultadoReporte.PerformCallback(parametro);
            }
        }


        function EsRangoValido(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;

            if ((fechaInicio == null || fechaInicio == false) && (fechaFin != null && fechaFin != false)) { e.isValid = false; return; }
            if ((fechaInicio == null || fechaInicio == false) && (fechaFin != null && fechaFin != false)) { e.isValid = false; return; }

            if (fechaInicio > fechaFin) { e.isValid = false; return; }

            //            var diff = Math.floor((fechaFin.getTime() - fechaInicio.getTime()) / (1000 * 60 * 60 * 24));
            //            if (diff > 60) { e.isValid = false; }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
    </div>
    <dx:ASPxCallbackPanel ID="cpResultadoReporte" ClientInstanceName="cpResultadoReporte"
        runat="server" >
        <ClientSideEvents BeginCallback="function(s,e){ loadingPnl.Show();}" EndCallback="function(s,e){ loadingPnl.Hide(); document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
        <PanelCollection>
            <dx:PanelContent>
                <vu:ValidacionURL ID="vuControlSesion" runat="server" />
                <asp:Panel ID="pnlConsulta" runat="server">
                <table>
                    <tr>
                        <th colspan="4" class="thRojo">
                            <asp:Image ID="imgSearch" runat="server" ImageUrl="~/img/find.gif" />&nbsp;Filtros
                            de B&uacute;squeda
                        </th>
                    </tr>
                    <tr>
                        <td class="field">
                            Punto de Venta
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboPdv" runat="server" ValueType="System.Int32" EnableClientSideAPI="True"
                                Style="display: inline !important;" AutoResizeWithContainer="True" Width="300"
                                ClientInstanceName="cboPdv">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { cboAsesor.PerformCallback('cargarLista'); }"
                                    EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                            </dx:ASPxComboBox>
                            <dx:ASPxCheckBox ID="cbPdvActivo" runat="server" Checked="True" CheckState="Checked"
                                Text="Solo Activos" Style="display: inline !important;">
                                <ClientSideEvents CheckedChanged="function(s, e) { cboPdv.PerformCallback('cargarLista'); }" />
                            </dx:ASPxCheckBox>
                        </td>
                        <td class="field">
                            Asesor Comercial
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboAsesor" runat="server" ValueType="System.Int32" Style="display: inline !important;"
                                AutoResizeWithContainer="True" Width="300" ClientInstanceName="cboAsesor">
                                <ClientSideEvents EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                            </dx:ASPxComboBox>
                            <dx:ASPxCheckBox ID="cbAsesorActivo" runat="server" Checked="True" CheckState="Checked"
                                Text="Solo Activos" Style="display: inline !important;">
                                <ClientSideEvents CheckedChanged="function(s, e) { cboAsesor.PerformCallback('cargarLista'); }" />
                            </dx:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Resultado Gestión
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboResultadoGestion" runat="server" ValueType="System.Int32"
                                Style="display: inline !important;" AutoResizeWithContainer="True" Width="300"
                                ClientInstanceName="cboResultadoGestion">
                            </dx:ASPxComboBox>
                        </td>
                        <td class="field">
                            Tipo de Producto
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboTipoProducto" runat="server" ValueType="System.Int32" Style="display: inline !important;"
                                AutoResizeWithContainer="True" Width="300" ClientInstanceName="cboTipoProducto">
                                <ClientSideEvents EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Fecha de Gesti&oacute;n
                        </td>
                        <td style="vertical-align: middle !important" nowrap="nowrap" colspan="3">
                            <table style="padding: 0px !important; border: none !important">
                                <tr>
                                    <td>
                                        De:&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio">
                                            <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                            </CalendarProperties>
                                            <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final"
                                                ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                            </ValidationSettings>
                                            <ClientSideEvents Validation="EsRangoValido" />
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>
                                        &nbsp;-&nbsp;
                                    </td>
                                    <td>
                                        Hasta:&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin">
                                            <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                            </CalendarProperties>
                                            <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final"
                                                ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                            </ValidationSettings>
                                            <ClientSideEvents Validation="EsRangoValido" />
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <dx:ASPxButton ID="btnConsultar" runat="server" Text="Consultar Datos" AutoPostBack="false"
                                Style="display: inline !important;">
                                <%--<ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral('obtenerReporte');}" />--%>
                            </dx:ASPxButton>
                            &nbsp;&nbsp;&nbsp;
                            <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar" AutoPostBack="false"
                                Style="display: inline !important;">
                                <ClientSideEvents Click="function(s, e) {EjecutarCallbackGeneral('limpiarFiltros');}" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <br /><br />
                <asp:Panel ID="pnlVentasConNovedad" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="gvVentasConNovedades" runat="server" EnableModelValidation="True" 
                                            ForeColor="Black" Width="200%" CssClass="tabla" AllowPaging="True">
                                            <Columns>
                                                <asp:CommandField SelectText="Modificar" ShowSelectButton="True" />
                                            </Columns>
                                        </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    
                </asp:Panel>

                <asp:Panel ID="pnlInfoOrigenGestion" runat="server">
                        <table class="tabla">
                            <tr>
                                <th colspan="4">
                                    Informaci&oacute;n de Origen de la Gesti&oacute;n
                                </th>
                            </tr>
                            <tr>
                                <td class="field">
                                    Punto de Venta
                                </td>
                                <td>
                                    <eo:CallbackPanel ID="cpPDV" runat="server" UpdateMode="Group" GroupName="InfoOrigenGestion"
                                        ChildrenAsTriggers="true" AutoDisableContents="True">
                                        <asp:DropDownList ID="ddlPdv" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkPdvActivo" Checked="true" runat="server" Text="Solo Activos" /></eo:CallbackPanel>
                                    <asp:RequiredFieldValidator ID="rfvPdv" runat="server" ErrorMessage="Punto de Venta Requerido"
                                        Display="Dynamic" ControlToValidate="ddlPdv" ValidationGroup="registroVenta"
                                        InitialValue="0">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td class="field">
                                    Asesor Comercial
                                </td>
                                <td>
                                    <eo:CallbackPanel ID="cpAsesorComercial" runat="server" UpdateMode="Group" GroupName="InfoOrigenGestion"
                                        ChildrenAsTriggers="true" AutoDisableContents="True">
                                        <asp:DropDownList ID="ddlAsesorComercial" runat="server">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkAsesorActivo" Checked="true" runat="server" Text="Solo Activos" />
                                    </eo:CallbackPanel>
                                    <asp:RequiredFieldValidator ID="rfvAsesorComercial" runat="server" ErrorMessage="Asesor comercial requerido"
                                        Display="Dynamic" ControlToValidate="ddlAsesorComercial" ValidationGroup="registroVenta"
                                        InitialValue="0">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                <asp:Panel ID="pnlGestion" runat="server" Visible="false">
                                                <table class="tabla">
                                <tr>
                                    <th colspan="6">
                                        Datos de la Venta
                                    </th>
                                </tr>
                                <tr>
                                    <td class="field" style="width: 150px">
                                        Fecha de Venta
                                    </td>
                                    <td colspan="3">
                                        <eo:DatePicker ID="dpFechaVenta" runat="server" PickerFormat="dd/MM/yyyy" ControlSkinID="None"
                                            CssBlock="&lt;style type=&quot;text/css&quot;&gt;
                        .DatePickerStyle1 {background-color:white;border-bottom-color:Silver;border-bottom-style:solid;border-bottom-width:1px;border-left-color:Silver;border-left-style:solid;border-left-width:1px;border-right-color:Silver;border-right-style:solid;border-right-width:1px;border-top-color:Silver;border-top-style:solid;border-top-width:1px;color:#2C0B1E;padding-bottom:5px;padding-left:5px;padding-right:5px;padding-top:5px}
                        .DatePickerStyle2 {border-bottom-color:#f5f5f5;border-bottom-style:solid;border-bottom-width:1px;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle3 {font-family:Verdana;font-size:8pt}
                        .DatePickerStyle4 {background-image:url('00040402');color:#1c7cdc;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle5 {background-image:url('00040401');color:#1176db;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle6 {color:gray;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle7 {cursor:pointer;cursor:hand;margin-bottom:0px;margin-left:4px;margin-right:4px;margin-top:0px}
                        .DatePickerStyle8 {background-image:url('00040403');color:Brown;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle9 {cursor:pointer;cursor:hand}
                        .DatePickerStyle10 {font-family:Verdana;font-size:8.75pt;padding-bottom:5px;padding-left:5px;padding-right:5px;padding-top:5px}
                            &lt;/style&gt;" DayCellHeight="15" DayCellWidth="31" DayHeaderFormat="Short" DisabledDates=""
                                            OtherMonthDayVisible="True" SelectedDates="" TitleFormat="MMMM, yyyy" TitleLeftArrowImageUrl="DefaultSubMenuIconRTL"
                                            TitleRightArrowImageUrl="DefaultSubMenuIcon" AllowMultiSelect="False">
                                            <TodayStyle CssClass="DatePickerStyle5" />
                                            <SelectedDayStyle CssClass="DatePickerStyle8" />
                                            <DisabledDayStyle CssClass="DatePickerStyle6" />
                                            <FooterTemplate>
                                                <table border="0" cellpadding="0" cellspacing="5" style="font-size: 11px; font-family: Verdana">
                                                    <tr>
                                                        <td width="30">
                                                        </td>
                                                        <td valign="center">
                                                            <img src="{img:00040401}"></img>
                                                        </td>
                                                        <td valign="center">
                                                            Today: {var:today:dd/MM/yyyy}
                                                        </td>
                                                    </tr>
                                                </table>
                                            </FooterTemplate>
                                            <CalendarStyle CssClass="DatePickerStyle1" />
                                            <TitleArrowStyle CssClass="DatePickerStyle9" />
                                            <DayHoverStyle CssClass="DatePickerStyle4" />
                                            <MonthStyle CssClass="DatePickerStyle7" />
                                            <TitleStyle CssClass="DatePickerStyle10" />
                                            <DayHeaderStyle CssClass="DatePickerStyle2" />
                                            <DayStyle CssClass="DatePickerStyle3" />
                                        </eo:DatePicker>
                                        <div style="display: block;">
                                            <asp:RequiredFieldValidator ID="rfvFechaVenta" runat="server" ErrorMessage="La fecha de venta es requerida"
                                                Display="Dynamic" ControlToValidate="dpFechaVenta" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field" style="width: 150px">
                                        Nombre Asesor Call Center
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAtendidoPor" runat="server" MaxLength="100" Width="250px"></asp:TextBox>
                                        <div style="display: block;">
                                            <asp:RegularExpressionValidator ID="revAtendidoPor" runat="server" ControlToValidate="txtAtendidoPor"
                                                Display="Dynamic" ErrorMessage="El texto proporcionado contiene caracteres no válidos. Por favor verifique"
                                                ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$"
                                                ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvAtendidoPor" runat="server" ControlToValidate="txtAtendidoPor"
                                                Display="Dynamic" ErrorMessage="Nombre de la persona que atendió al asesor en Call Center querido"
                                                ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td class="field" style="width: 150px">
                                        ID Asesor Call Center
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtNumIdOperadorCallCenter" runat="server" MaxLength="15"></asp:TextBox>
                                        <div style="display: block;">
                                            <asp:RegularExpressionValidator ID="revNumIdOperadorCall" runat="server" ControlToValidate="txtNumIdOperadorCallCenter"
                                                Display="Dynamic" ErrorMessage="El dato proporionado tiene formato no válido"
                                                ValidationExpression="^[\s]{0,}[a-zA-Z0-9\-\.]+[\s]{0,}$" ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        No. Planilla Pre-An&aacute;lisis
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumPlanillaPreAnalisis" runat="server" MaxLength="12"></asp:TextBox>
                                        <div style="display: block;">
                                            <asp:RegularExpressionValidator ID="revNumPlanillaPreAnalisis" runat="server" ErrorMessage="El dato proporcionado tiene formato no válido. Se espera un valor numerico entero"
                                                Display="Dynamic" ControlToValidate="txtNumPlanillaPreAnalisis" ValidationExpression="^[0-9]+$"
                                                ValidationGroup="registroVenta"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvNumPlanillaPreAnalisis" runat="server" ErrorMessage="N&uacute;mero de planilla de Pre-An&aacute;lisis requerido"
                                                ControlToValidate="txtNumPlanillaPreAnalisis" Display="Dynamic" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td class="field">
                                        No. Venta en Planilla
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumVentaPlanilla" runat="server" MaxLength="12"></asp:TextBox>
                                        <div style="display: block;">
                                            <asp:RegularExpressionValidator ID="revNumVentaPlanilla" runat="server" ErrorMessage="El dato proporcionado tiene formato no válido. Se espera un valor numerico entero"
                                                Display="Dynamic" ControlToValidate="txtNumVentaPlanilla" ValidationExpression="^[0-9]+$"
                                                ValidationGroup="registroVenta"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvNumVentaPlanilla" runat="server" ErrorMessage="N&uacute;mero de venta seg&uacute;n planilla requerido"
                                                ControlToValidate="txtNumVentaPlanilla" Display="Dynamic" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Resultado Consulta
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlResultadoConsulta" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <div style="display: block">
                                            <asp:RequiredFieldValidator ID="rfvResultadoConsulta" runat="server" ErrorMessage="Resultado de la consulta en call center requerido"
                                                ControlToValidate="ddlResultadoConsulta" Display="Dynamic" InitialValue="0" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td class="field">
                                        Tipo de Producto
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipoProducto" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <div style="display: block">
                                            <asp:RequiredFieldValidator ID="rfvTipoProducto" runat="server" ErrorMessage="Tipo de producto requerido"
                                                ControlToValidate="ddlTipoProducto" Display="Dynamic" InitialValue="0" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trInfoProducto" runat="server">
                                    <td class="field">
                                        Tipo de Tarjeta
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProductoPadre" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <div style="display: block">
                                            <asp:RequiredFieldValidator ID="rfvTipoDeTarjetaRequerido" runat="server" ErrorMessage="Tipo de tarjeta requerido"
                                                ControlToValidate="ddlProductoPadre" Display="Dynamic" InitialValue="0" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td class="field">
                                        Cupo Asignado
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSubproducto" runat="server">
                                        </asp:DropDownList>
                                        <div style="display: block">
                                            <asp:RequiredFieldValidator ID="rfvProducto" runat="server" ErrorMessage="Cupo de credito aprobado requerido"
                                                ControlToValidate="ddlSubproducto" Display="Dynamic" InitialValue="0" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trInfoSerial" runat="server">
                                    <td class="field">
                                        No. de Pagar&eacute;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumPagare" runat="server" MaxLength="12"></asp:TextBox>
                                        <div style="display: block;">
                                            <asp:RegularExpressionValidator ID="revNumPagare" runat="server" ErrorMessage="El dato proporcionado tiene formato no válido. Se espera un valor numerico entero"
                                                Display="Dynamic" ControlToValidate="txtNumPagare" ValidationExpression="^[0-9]+$"
                                                ValidationGroup="registroVenta"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvNumPagare" runat="server" ErrorMessage="N&uacute;mero de pagar&eacute; requerido"
                                                ControlToValidate="txtNumPagare" Display="Dynamic" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td class="field">
                                        N&uacute;mero (Serial) del Pl&aacute;stico
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSerialTarjeta" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                                        <div style="display: block">
                                            <asp:RequiredFieldValidator ID="rfvSerialTarjeta" runat="server" ErrorMessage="El serial de la tarjeta es requerido"
                                                ControlToValidate="txtSerialTarjeta" Display="Dynamic" ValidationGroup="registroVenta"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revSerialTarjeta" runat="server" ErrorMessage="Por favor ingrese un serial v&aacute;lido. Valor num&eacute;rico de 14 o 16 d&iacute;gitos"
                                                Display="Dynamic" ControlToValidate="txtSerialTarjeta" ValidationExpression="^([0-9]{14}|[0-9]{16})$"
                                                ValidationGroup="registroVenta"></asp:RegularExpressionValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Observaciones
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtObservacionOperadorCall" runat="server" Rows="3" TextMode="MultiLine"
                                            Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Declinar Venta
                                    </td>
                                    <td colspan="3">
                                        <asp:CheckBox ID="chkDeclinarVenta" runat="server" Text="¿Se declina la venta?" Checked="false" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr id="trDeclinarVenta" runat="server">
                                    <td class="field">
                                        Observaciones Venta Declinada
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtObservacionesVentaDeclinada" runat="server" Rows="3" TextMode="MultiLine"
                                            Width="95%" MaxLength="500"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvObservacionVentaDeclinada" runat="server" ErrorMessage="Observaci&oacute;n requerido"
                                ControlToValidate="txtObservacionesVentaDeclinada" Display="Dynamic" 
                                ValidationGroup="consultaIdentificacion" Enabled="False"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <p>
                                            <asp:LinkButton ID="lbRegistrarVenta" runat="server" CssClass="search" ValidationGroup="registroVenta"
                                                OnClientClick="return confirm('¿Realmente desea actualizar el registro?');"><img src="../img/save_all.png" alt="" />&nbsp;Continuar</asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbCancelarVenta" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea cancelar el registro?\nSe perderá toda la información proporcionada');"><img src="../img/cancelar.png" alt="" />&nbsp;Cancelar</asp:LinkButton>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                </asp:Panel> 
            </dx:PanelContent>
        </PanelCollection>
        <LoadingPanelStyle CssClass="modalBackground">
        </LoadingPanelStyle>
    </dx:ASPxCallbackPanel>
    <dx:ASPxLoadingPanel ID="loadingPnl" runat="server" ClientInstanceName="loadingPnl"
        Modal="True" Theme="DevEx">
    </dx:ASPxLoadingPanel>
    <asp:Label ID="lblTransaccionExistente" Text="0" runat="server" Visible="false"></asp:Label>
    </form>
</body>
</html>
