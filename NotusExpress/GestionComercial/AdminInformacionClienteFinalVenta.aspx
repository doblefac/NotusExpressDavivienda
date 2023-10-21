<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminInformacionClienteFinalVenta.aspx.vb"
    Inherits="NotusExpress.AdminInformacionClienteFinalVenta" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Notus Express - Informacion del Cliente::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <eo:callbackpanel id="cpValidacionYNotificacion" runat="server" width="100%" updatemode="Always">
        <uc1:ValidacionURL ID="vuControlSesion" runat="server" />
        <uc2:EncabezadoPagina ID="epNotificador" runat="server" />
    </eo:callbackpanel>
    <eo:callbackpanel id="cpGeneral" runat="server" width="100%" updatemode="Group" groupname="General"
        loadingdialogid="ldrWait_dlgWait" triggers="{ControlID:lbConsultar;Parameter:},{ControlID:lblRegistrarPreventa;Parameter:},{ControlID:lbCancelar;Parameter:},{ControlID:lbRegistrarVenta;Parameter:},{ControlID:lbCancelarVenta;Parameter:}">
        <asp:Panel ID="pnlConsulta" runat="server">
            <table class="tabla" style="width: 70%">
                <tr>
                    <th colspan="2">
                        Consulta del Cliente
                    </th>
                </tr>
                <tr>
                    <td class="field" style="width: 160px">
                        No. de Identificaci&oacute;n
                    </td>
                    <td>
                        <asp:TextBox ID="txtIdentificacionConsulta" runat="server" MaxLength="15"></asp:TextBox>
                        <div style="display: block">
                            <asp:RequiredFieldValidator ID="rfvIdentificacionConsulta" runat="server" ErrorMessage="No. de Identificaci&oacute;n requerido"
                                ControlToValidate="txtIdentificacionConsulta" Display="Dynamic" ValidationGroup="consultaIdentificacion"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revIdentificacionConsulta" runat="server" ErrorMessage="El dato proporionado tiene formato no v&aacute;lido"
                                ControlToValidate="txtIdentificacionConsulta" Display="Dynamic" ValidationGroup="consultaIdentificacion"
                                ValidationExpression="^[\s]{0,}[a-zA-Z0-9\-\.]+[\s]{0,}$"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                        <p>
                            <asp:LinkButton ID="lbConsultar" runat="server" CssClass="search" ValidationGroup="consultaIdentificacion"><img src="../img/find.gif" alt="" />&nbsp;Consultar</asp:LinkButton></p>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlGestion" runat="server">
            <eo:TabStrip runat="server" ID="tsInfoGestion" ControlSkinID="None" MultiPageID="mpInfoGestion">
                <TopGroup>
                    <Items>
                        <eo:TabItem Text-Html="Datos Personales del Cliente ">
                        </eo:TabItem>
                        <eo:TabItem Text-Html="Información de la Venta">
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
                    <asp:Panel ID="pnlDatosPersonales" runat="server">
                        <table class="tabla">
                            <tr>
                                <th colspan="4">
                                    Datos Personales del Cliente
                                </th>
                            </tr>
                            <tr>
                                <td class="field">
                                    Tipo de Identificación
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoIdentificacion" runat="server">
                                        <asp:ListItem Text="Seleccione un Tipo" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Cédula de Ciudadanía" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NUIP" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Cédula de Extranjería" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="NIT" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div style="display: block">
                                        <asp:RequiredFieldValidator ID="rfvTipoIdentificacion" runat="server" ErrorMessage="Tipo de Identificaci&oacute;n requerido"
                                            ControlToValidate="ddlTipoIdentificacion" Display="Dynamic" InitialValue="0"
                                            ValidationGroup="datosPersonales"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td class="field" style="width: 150px">
                                    No. de Identificación
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIdentificacion" runat="server" MaxLength="15"></asp:TextBox>
                                    <div style="display: block">
                                        <asp:RegularExpressionValidator ID="revIdentificacion" runat="server" ErrorMessage="El dato proporionado tiene formato no v&aacute;lido"
                                            ControlToValidate="txtIdentificacion" Display="Dynamic" ValidationGroup="datosPersonales"
                                            ValidationExpression="^[\s]{0,}[a-zA-Z0-9\-\.]+[\s]{0,}$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvIdentificacion" runat="server" ErrorMessage="No. de Identificaci&oacute;n requerido"
                                            ControlToValidate="txtIdentificacion" Display="Dynamic" ValidationGroup="datosPersonales"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="field">
                                    Nombres y Apellidos
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombreApellido" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revNombreApellido" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                            Display="Dynamic" ControlToValidate="txtNombreApellido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#]+\s*$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvNombreApellido" runat="server" ErrorMessage="Nombres y Apellidos del cliente requeridos"
                                            ControlToValidate="txtNombreApellido" Display="Dynamic" ValidationGroup="datosPersonales"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td class="field">
                                    Ciudad de Residencia
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCiudadResidencia" runat="server">
                                    </asp:DropDownList>
                                    <div style="display: block">
                                        <asp:RequiredFieldValidator ID="rfvCiudadResidencia" runat="server" ErrorMessage="Ciudad de residencia requerida"
                                            ControlToValidate="ddlCiudadResidencia" Display="Dynamic" ValidationGroup="datosPersonales"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="field">
                                    Direcci&oacute;n de Residencia
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDireccionResidencia" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revDireccionResidencia" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                            Display="Dynamic" ControlToValidate="txtdireccionResidencia" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvDireccionResidencia" runat="server" ErrorMessage="Direcci&oacute;n de residencia requerida"
                                            ControlToValidate="txtDireccionResidencia" Display="Dynamic" ValidationGroup="datosPersonales"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td class="field">
                                    Barrio Residencia
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBarrioResidencia" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revBarrioResidencia" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                            Display="Dynamic" ControlToValidate="txtBarrioResidencia" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvBarrio" runat="server" ErrorMessage="Barrio de residencia requerida"
                                            ControlToValidate="txtBarrioResidencia" Display="Dynamic" ValidationGroup="datosPersonales"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="field">
                                    Tel&eacute;fono de Residencia
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelefonoResidencia" runat="server" MaxLength="20"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revtTelefonoResidencia" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                            Display="Dynamic" ControlToValidate="txtTelefonoResidencia" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                                <td class="field">
                                    Celular
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCelular" runat="server" MaxLength="20"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revCelular" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                            Display="Dynamic" ControlToValidate="txtCelular" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="field">
                                    Direccion Oficina
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDireccionOficina" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revDireccionOficina" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                            Display="Dynamic" ControlToValidate="txtDireccionOficina" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                                <td class="field">
                                    Tel&eacute;fono Oficina
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelefonoOficina" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revTelefonoOficina" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                            Display="Dynamic" ControlToValidate="txtTelefonoOficina" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="field">
                                    Ingreso Aproximado
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIngreso" runat="server" MaxLength="15"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revIngreso" runat="server" ErrorMessage="El dato proporcionado tiene formato no válido. Se espera un valor numerico entero"
                                            Display="Dynamic" ControlToValidate="txtIngreso" ValidationExpression="^[0-9]+$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvIngreso" runat="server" ErrorMessage="Ingreso aproximado requerido"
                                            ControlToValidate="txtIngreso" Display="Dynamic" ValidationGroup="datosPersonales"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td class="field">
                                    Estatus Laboral
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEstatusLaboral" runat="server">
                                    </asp:DropDownList>
                                    <div style="display: block">
                                        <asp:RequiredFieldValidator ID="rfvEstatusLaboral" runat="server" ErrorMessage="Estatus laboral requerido"
                                            ControlToValidate="ddlEstatusLaboral" Display="Dynamic" InitialValue="0" ValidationGroup="datosPersonales"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="field">
                                    E-Mail
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                    <div style="display: block;">
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                            Display="Dynamic" ControlToValidate="txtEmail" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\@\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$"
                                            ValidationGroup="datosPersonales"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trInfoPreventa" runat="server">
                                <td colspan="4">
                                    <asp:Panel ID="pnlRegistroPreventa" runat="server">
                                        <asp:CustomValidator ID="cusDatosMinimos" runat="server" ErrorMessage="Debe proporcionar por lo menos un tel&eacute;fono de contacto"
                                            ValidationGroup="datosPersonales" Display="Dynamic" ClientValidationFunction="ValidarDatosMinimos"></asp:CustomValidator>
                                        <br />
                                        <p>
                                            <asp:LinkButton ID="lblRegistrarPreventa" runat="server" CssClass="search" ValidationGroup="datosPersonales"
                                                OnClientClick="return ValidarDatosYMostrarConfirmacion('datosPersonales', '¿Realmente desea continuar con el registro proceso?');"><img src="../img/save_all.png" alt="" />&nbsp;&nbsp;Registrar Datos</asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbCancelar" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea cancelar el proceso?');"><img src="../img/cancelar.png" alt="" />&nbsp;Cancelar</asp:LinkButton>
                                        </p>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlInfoVenta" runat="server">
                        <eo:CallbackPanel ID="cpInfoVenta" runat="server" LoadingDialogID="ldrWait_dlgWait"
                            UpdateMode="Self" Triggers="{ControlID:ddlResultadoConsulta;Parameter:},{ControlID:ddlTipoProducto;Parameter:},{ControlID:ddlProductoPadre;Parameter:}">
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
                                    <td colspan="4">
                                        <br />
                                        <p>
                                            <asp:LinkButton ID="lbRegistrarVenta" runat="server" CssClass="search" ValidationGroup="registroVenta"
                                                OnClientClick="return ValidarDatosYMostrarConfirmacion('registroVenta', '¿Realmente desea continuar con el registro de datos?');"><img src="../img/save_all.png" alt="" />&nbsp;Registrar Datos</asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbCancelarVenta" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea cancelar el registro?\nSe perderá toda la información proporcionada');"><img src="../img/cancelar.png" alt="" />&nbsp;Cancelar</asp:LinkButton>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </eo:CallbackPanel>
                    </asp:Panel>
                </eo:PageView>
            </eo:MultiPage>
        </asp:Panel>
    </eo:callbackpanel>
    <eo:callbackpanel id="cpMensajeConfirmacion" runat="server" width="100%" updatemode="Group"
        groupname="General" loadingdialogid="ldrWait_dlgWait" childrenastriggers="true">
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
    </eo:callbackpanel>
    <uc3:loader id="ldrWait" runat="server" />
    </form>
</body>
</html>
