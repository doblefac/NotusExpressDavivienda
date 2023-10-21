<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DescargueManual.aspx.vb" Inherits="NotusExpress.DescargueManual" %>


<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Notus Express - Gestión de Novedades::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
     <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        String.prototype.trim = function () { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }
        function ValidarDatosMinimos(source, args) {
            try {
                var telefonoResidencia = document.getElementById("txtTelefonoResidencia").value.trim();
                var celular = document.getElementById("txtCelular").value.trim();
                var telefonoOficina = document.getElementById("txtTelefonoOficina").value.trim();
                if (telefonoResidencia.length > 0 || celular.length > 0 || telefonoOficina.length > 0) {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            } catch (e) {
                args.IsValid = false;
                alert("Imposible evaluar si se ha proporcionado un teléfono de contacto.\n" + e.description);
            }
        }

        function CallbackAfterUpdateHandler(callback, extraData) {
            try {
                MostrarOcultarDivFloater(false);
            } catch (e) {
                //alert("Error al tratar de evaluar respuesta del servidor.\n" + e.description);
            }

        }

        function MostrarOcultarDivFloater(mostrar) {
            var valorDisplay = mostrar ? "block" : "none";
            var elDiv = document.getElementById("divFloater");
            elDiv.style.display = valorDisplay;
        }

        function FiltrarDatos(source, callbackPanel, filtro, idControladorFiltro) {
            var controladorFiltro = document.getElementById(idControladorFiltro);
            try {
                if (filtro.length >= 4 || (filtro.length < 4 && controladorFiltro.value == "1")) {
                    MostrarOcultarDivFloater(true);
                    if (filtro.length < 4) { filtro = ""; }
                    eo_Callback(callbackPanel, filtro);
                    if (filtro.length >= 4) {
                        controladorFiltro.value = "1";
                    } else {
                        controladorFiltro.value = "0";
                    }
                }
                source.focus();
            } catch (e) {
                MostrarOcultarDivFloater(false);
                //alert("Error al tratar de filtrar Datos.\n" + e.description);
            }
        }
       
    </script>

    

 
    <style type="text/css">
        .style1
        {
            width: 1010px;
        }
        .style2
        {
            font-weight: bold;
            background: #A32035;
            color: White;
            width: 1010px;
        }
    </style>
   
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfControlFiltroCiudad" runat="server" />
    <eo:CallbackPanel ID="cpValidacionYNotificacion" runat="server" Width="100%" UpdateMode="Always">
        <uc1:ValidacionURL ID="vuControlSesion" runat="server" />
        <uc2:EncabezadoPagina ID="epNotificador" runat="server" />
    </eo:CallbackPanel>
    <eo:CallbackPanel ID="cpGeneral" runat="server" Width="100%" UpdateMode="Group" GroupName="General"
        LoadingDialogID="ldrWait_dlgWait" Triggers="{ControlID:lbConsultar;Parameter:}">
        <asp:Panel ID="pnlConsulta" runat="server">
            <table class="tabla" style="width: 70%">
                <tr>
                    <th colspan="2">
                        Consulta de Novedades
                    </th>
                </tr>
                <tr>
                    <td class="field" style="width: 160px">
                        Consultar por
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlConsultarPor" runat="server" AutoPostBack="True">
                        <asp:ListItem Text="Seleccione una opción" Selected="True" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Punto de Venta" Selected="False" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Número de Serial" Selected="False" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Fecha de Gestión" Selected="False" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Tipo de Producto" Selected="False" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trPuntoVentaConsulta" runat="server">
                    <td class="field" style="width: 160px">
                        Punto de Venta
                    </td>
                    <td>
                                        <asp:DropDownList ID="ddlPuntoVentaConsulta" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkPdvActivo" Checked="true" runat="server" Text="Solo Activos" AutoPostBack="true"  />
                    </td>
                </tr>
                <tr id="trSerial" runat="server">
                    <td class="field" style="width: 160px">
                        Número de Serial
                    </td>
                    <td>
                        <asp:TextBox ID="txtSerialConsultar" runat="server" tet=""></asp:TextBox>
                        <%--<div style="display: block">
                            <asp:RequiredFieldValidator ID="rfvTransaccionConsulta" runat="server" ErrorMessage="No. de Transacci&oacute;n requerido"
                                ControlToValidate="txtTransaccionConsulta" Display="Dynamic" 
                                ValidationGroup="consultaIdentificacion" Enabled="False"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revTransaccionConsulta" runat="server" ErrorMessage="El dato proporionado tiene formato no v&aacute;lido"
                                ControlToValidate="txtTransaccionConsulta" Display="Dynamic" ValidationGroup="consultaIdentificacion"
                                ValidationExpression="^[\s]{0,}[a-zA-Z0-9\-\.]+[\s]{0,}$"></asp:RegularExpressionValidator>
                        </div>--%>
                    </td>
                </tr>
                <tr id="trFechaGestionConsulta" runat="server">
                    <td class="field" style="width: 160px">
                        Fecha de Gestión
                    </td>
                    <td>
                    <table width="50%">
                    <tr>
                    <td>
                        Fecha Inicial: 
                        <eo:DatePicker ID="dpFechaGestionInicialConsulta" runat="server" PickerFormat="dd/MM/yyyy" ControlSkinID="None"
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
                        <%--<div style="display: block">
                            <asp:RequiredFieldValidator ID="rfvTransaccionConsulta" runat="server" ErrorMessage="No. de Transacci&oacute;n requerido"
                                ControlToValidate="txtTransaccionConsulta" Display="Dynamic" 
                                ValidationGroup="consultaIdentificacion" Enabled="False"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revTransaccionConsulta" runat="server" ErrorMessage="El dato proporionado tiene formato no v&aacute;lido"
                                ControlToValidate="txtTransaccionConsulta" Display="Dynamic" ValidationGroup="consultaIdentificacion"
                                ValidationExpression="^[\s]{0,}[a-zA-Z0-9\-\.]+[\s]{0,}$"></asp:RegularExpressionValidator>
                        </div>--%>
                    </td>
                    <td>
                        Fecha Final: 
                        <eo:DatePicker ID="dpFechaGestionFinalConsulta" runat="server" PickerFormat="dd/MM/yyyy" ControlSkinID="None"
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
                        <%--<div style="display: block">
                            <asp:RequiredFieldValidator ID="rfvTransaccionConsulta" runat="server" ErrorMessage="No. de Transacci&oacute;n requerido"
                                ControlToValidate="txtTransaccionConsulta" Display="Dynamic" 
                                ValidationGroup="consultaIdentificacion" Enabled="False"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revTransaccionConsulta" runat="server" ErrorMessage="El dato proporionado tiene formato no v&aacute;lido"
                                ControlToValidate="txtTransaccionConsulta" Display="Dynamic" ValidationGroup="consultaIdentificacion"
                                ValidationExpression="^[\s]{0,}[a-zA-Z0-9\-\.]+[\s]{0,}$"></asp:RegularExpressionValidator>
                        </div>--%>
                    </td>
                    </tr>
                    </table>
                    </td>
                </tr>
                <tr id="trTipoProductoConsulta" runat="server">
                    <td class="field" style="width: 160px">
                        Tipo de Producto
                    </td>
                    <td>
                    <asp:DropDownList ID="ddlTipoProductoConsulta" runat="server" 
                            ValidationGroup="consultaNovedad" AutoPostBack="True">
                        <asp:ListItem Text="Seleccione una opción" Selected="True" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Tipo Producto 1" Selected="False" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Tipo Producto 2" Selected="False" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trConsultar" runat="server">
                    <td colspan="2">
                        <br />
                        <p>
                            <asp:LinkButton ID="lbConsultar" runat="server" CssClass="search" ValidationGroup="consultaNovedad"><img src="../img/find.gif" alt="" />&nbsp;Consultar</asp:LinkButton></p>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div id="divFloater" style="display: none; position: static; height: 35px; width: 200px;">
            <table align="center">
                <tr>
                    <td align="center" valign="middle" style="height: 100%">
                        <asp:Image ID="imgLoading" runat="server" ImageUrl="~/img/loader_dots.gif" ImageAlign="Middle" />
                        <b>Procesando...</b>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Panel ID="pnlGestion" runat="server">
            <eo:TabStrip runat="server" ID="tsInfoGestion" ControlSkinID="None" MultiPageID="mpInfoGestion">
                <TopGroup>
                    <Items>
                        <eo:TabItem Text-Html="Listado de Seriales ">
                        </eo:TabItem>
                        <eo:TabItem Text-Html="Transacciones Seriales">
                        </eo:TabItem>
                    </Items>
                </TopGroup>
                <LookItems>
                    <eo:TabItem ItemID="_Default" RightIcon-Url="00010223" RightIcon-SelectedUrl="00010226"
                        NormalStyle-CssText="color: #606060" SelectedStyle-CssText="color: #2f4761; font-weight: bold;"
                        LeftIcon-Url="00010221" LeftIcon-SelectedUrl="00010224" Image-Url="00010222"
                        Image-SelectedUrl="00010225" Image-Mode="TextBackground" Image-BackgroundRepeat="RepeatX">
                        <SubGroup Style-CssText="font-family: tahoma; font-size: 8pt; background-image: url(00010220); background-repeat: repeat-x; cursor: hand;"
                            OverlapDepth="8">
                        </SubGroup>
                    </eo:TabItem>
                </LookItems>
            </eo:TabStrip>
            <eo:MultiPage runat="server" ID="mpInfoGestion">
                <eo:PageView ID="pvDatosCliente" runat="server">
                    <br />
                    <asp:Panel ID="pnlInfoVenta" runat="server">
                        <eo:CallbackPanel ID="cpInfoVenta" runat="server" LoadingDialogID="ldrWait_dlgWait"
                            UpdateMode="Self" Triggers="{ControlID:ddlResultadoConsulta;Parameter:},{ControlID:ddlTipoProducto;Parameter:},{ControlID:ddlProductoPadre;Parameter:},{ControlID:ddlAccion;Parameter:}">
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
                                        Novedad
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkNovedad" runat="server" Text="¿Sí/No?" Checked="false" 
                                            AutoPostBack="true" />
                                    </td>
                                    
                                </tr>
                                
                                <tr id="trDetalleNovedad" runat="server">
                                    <td class="field">
                                        Detalle Novedad
                                    </td>
                                    <td  colspan="3">
                                        <asp:GridView ID="gvNovedades" runat="server" Width="100%" 
                                            EnableModelValidation="True">
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr id="trAcciones" runat="server">
                                    <td class="field">
                                        Acción a realizar
                                    </td>
                                    <td  colspan="3">
                                        <asp:DropDownList ID="ddlAccion" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="Seleccione una acción" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Destruír" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Trasladar" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Faltante" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Liberar" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trBotonAccion" runat="server">
                                    <td class="field">
                                        
                                    </td>
                                    <td  colspan="3">
                                        <asp:LinkButton ID="lbContinuar" runat="server" CssClass="search"><img src="../img/save_all.png" alt="" />&nbsp;Continuar</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trTraslados" runat="server">
                                    <td class="field">
                                        Traslado
                                    </td>
                                    <td  colspan="3">
                                        Serial: <asp:TextBox ID="txtSerialTrasladar" runat="server" Enabled="False"></asp:TextBox>
                                        Bodega Orígen: <asp:TextBox ID="txtBodegaOrigen" runat="server" Enabled="False"></asp:TextBox>
                                        Bodega Destino: <asp:DropDownList ID="ddlBodegaDestino" runat="server" AutoPostBack="true"></asp:DropDownList> 
                                        <asp:LinkButton ID="lbTrasladarSerial" runat="server" CssClass="search"><img src="../img/save_all.png" alt="" />&nbsp;Continuar</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <p>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbCancelarVenta" visible="false" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea cancelar el registro?\nSe perderá toda la información proporcionada');"><img src="../img/cancelar.png" alt="" />&nbsp;Cancelar</asp:LinkButton>
                                            <asp:LinkButton ID="lbDestruir" visible="false" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea destruir el número de serial?');"><img src="../img/save_all.png" alt="" />&nbsp;Destruir</asp:LinkButton>
                                            <asp:LinkButton ID="lbTrasladar" visible="false" runat="server" CssClass="search"><img src="../img/save_all.png" alt="" />&nbsp;Trasladar</asp:LinkButton>
                                            <asp:LinkButton ID="lbFaltante" visible="false" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea marcar como faltante para éste punto de venta el número de serial?');"><img src="../img/save_all.png" alt="" />&nbsp;Faltante</asp:LinkButton>
                                            <asp:LinkButton ID="lbLiberar" visible="false" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea liberar el número de serial?');"><img src="../img/save_all.png" alt="" />&nbsp;Liberar</asp:LinkButton>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </eo:CallbackPanel>
                    </asp:Panel>
                    <br />
                    <%--Nuevo panel de historico de ventas--%>
                    <asp:Panel ID="pnlHistoricoVenta" runat="server" width="100%">
                        <eo:CallbackPanel ID="cpHistoricoVenta" runat="server" LoadingDialogID="ldrWait_dlgWait"
                            UpdateMode="Self" 
                            
                            Triggers="{ControlID:ddlResultadoConsulta;Parameter:},{ControlID:ddlTipoProducto;Parameter:},{ControlID:ddlProductoPadre;Parameter:}" 
                            Width="100%">
                            <table class="tabla">
                                <tr>
                                    <th colspan="6" class="style1">
                                        Información de Seriales
                                    </th>
                                </tr>
                                <tr>
                                    <td class="style2" colspan="6">
                                        

                                        <asp:GridView ID="gvHistoricoVenta" runat="server" EnableModelValidation="True" 
                                            ForeColor="Black" Width="100%">
                                            <Columns>
                                                <asp:CommandField SelectText="Ver" ShowSelectButton="True" />
                                            </Columns>
                                        </asp:GridView>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="style1">
                                        <br />
                                        <p>
                                            &nbsp;</p>
                                    </td>
                                </tr>
                            </table>
                        </eo:CallbackPanel>
                    </asp:Panel>
                </eo:PageView>
            </eo:MultiPage>
        </asp:Panel>
    </eo:CallbackPanel>
    <eo:CallbackPanel ID="cpMensajeConfirmacion" runat="server" Width="100%" UpdateMode="Group"
        GroupName="General" LoadingDialogID="ldrWait_dlgWait" ChildrenAsTriggers="true">
        <eo:Dialog runat="server" ID="dlgMensajeReferido" ControlSkinID="None" Height="150px"
            Width="500px" HeaderHtml="Solicitud de Referido" BackColor="White" CancelButton="btnNo"
            BackShadeColor="Gray" BackShadeOpacity="50">
            <ContentTemplate>
                <div style="text-align: center; padding: 5px; width: 100%;">
                    <asp:Label ID="lblRegistroOk" CssClass="ok" runat="server"></asp:Label><br />
                    <asp:Label ID="lblSolicitudReferido" ForeColor="Gray" Font-Bold="true" runat="server"
                        Text="¿El Cliente desea proporcionar información de Referido?"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btnSi" runat="server" Text="Si" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnNo" runat="server" Text="No" />
                </div>
            </ContentTemplate>
            <FooterStyleActive CssText="padding-right: 4px; padding-left: 4px; font-size: 8pt; padding-bottom: 4px; padding-top: 4px; font-family: tahoma">
            </FooterStyleActive>
            <HeaderStyleActive CssText="background-image:url('00020311');color:black;font-family:'trebuchet ms';font-size:10pt;font-weight:bold;padding-bottom:5px;padding-left:8px;padding-right:3px;padding-top:0px;">
            </HeaderStyleActive>
            <ContentStyleActive CssText="padding-right: 4px; padding-left: 4px; font-size: 8pt; padding-bottom: 4px; padding-top: 4px; font-family: tahoma">
            </ContentStyleActive>
        </eo:Dialog>
    </eo:CallbackPanel>
    <uc3:Loader ID="ldrWait" runat="server" />
    <asp:Label ID="lblTipoConsulta" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lblTransaccionID" runat="server" Text="0" Visible="false"></asp:Label>
    </form>
</body>
</html>

