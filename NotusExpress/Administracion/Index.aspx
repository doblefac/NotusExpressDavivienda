<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Index.aspx.vb" Inherits="NotusExpress.Index" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../ControlesDeUsuario/ModalProgress.ascx" TagName="ModalProgress"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: Logytech Mobile - NotusExpress:: </title>
    <link href="../Estilos/estiloMaster.css" rel="stylesheet" type="text/css" />
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/UIlayout/jquery.js"></script>
    <script type="text/javascript" src="../Scripts/UIlayout/jquery.ui.all.js"></script>
    <script type="text/javascript" src="../Scripts/UIlayout/jquery.layout.js"></script>
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        var myLayout; // a var is required because this page utilizes: myLayout.allowOverflow() method
        $(document).ready(function () {
            myLayout = $('body').layout({
                north: { minSize: 85, closable: false, resizable: false },
                south: { size: 20, closable: false, resizable: false, slidable: false, fxName: "none" },
                west: { maxSize: 400, minSize: 100, size: 200, spacing_closed: 20, togglerLength_closed: 100,
                    togglerAlign_closed: "top", togglerContent_closed: "M<BR>E<BR>N<BR>U",
                    togglerTip_closed: "Anclar de Menú", sliderTip: "Deslizar Menu", slideTrigger_open: "mouseover",
                    initClosed: true
                }

            });

            var mainFrame = document.getElementById('mainFrame');
            if (mainFrame.onload == null) {
                mainFrame.onload = function () {BloquearDesbloquearVentana(false);};
                if (window.attachEvent) {mainFrame.attachEvent('onload', mainFrame.onload);}
            }

        });

        function MostrarVentanaCambioPwd() {
            var lbl = document.getElementById("<%=lblErrores.ClientID %>");
            var pwdAntiguo = document.getElementById("<%=txtAntigua.ClientID %>");
            var pwdNuevo = document.getElementById("<%=txtNueva.ClientID %>");
            var pwdNuevoConfirma = document.getElementById("<%=txtNuevaConfirm.ClientID %>");

            lbl.innerHTML = "";
            pwdAntiguo.value = "";
            pwdNuevo.value = "";
            pwdNuevoConfirma.value = "";

            var dlg = eo_GetObject("<%=dlgCambiarContrasena.ClientID %>");
            dlg.show(true);
            return (false);
        }

        function LimpiarVentanaCambioPwd(dlg, args) {
            if (args != null) {
                var lbl = document.getElementById("<%=lblErrores.ClientID %>");
                var pwdAntiguo = document.getElementById("<%=txtAntigua.ClientID %>");
                var pwdNuevo = document.getElementById("<%=txtNueva.ClientID %>");
                var pwdNuevoConfirma = document.getElementById("<%=txtNuevaConfirm.ClientID %>");

                lbl.innerHTML = "";
                pwdAntiguo.value = "";
                pwdNuevo.value = "";
                pwdNuevoConfirma.value = "";

            }
        }

        function BloquearDesbloquearVentana(bloquear) {
            var dlg = eo_GetObject("ldrWait_dlgWait");
            if (bloquear == true) {dlg.show(true); }
            else {var resultado; dlg.close(resultado); }
        }

        
        function CargarPagina(url) {
            BloquearDesbloquearVentana(true);
            document.getElementsByName("mainFrame")[0].contentWindow.location.href = url;
        }

    </script>
    <style type="text/css">
        .user
        {
            width: 100%;
            display: block;
            height: 45px;
            color: #666666;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            padding-left: 15px;
            padding-top: 10px;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.50;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <uc2:ValidacionURL ID="vuControladorSesion" runat="server" />
    <div class="ui-layout-north" onmouseover="myLayout.allowOverflow('north')" onmouseout="myLayout.resetOverflow(this)">
        <div id="Header" style="width: 100%">
            <table width="100%">
                <tr>
                    <td>
                        <img alt="Logytech" src="../img/LogoHome.png" style="height: 78px; width: 175px" />
                    </td>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="60%">
                                    <table class="tabla">
                                        <tr>
                                            <td class="field">
                                                <asp:Label ID="Label1" runat="server" Text="Usuario:" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblUsuario" runat="server"></asp:Label>
                                            </td>
                                            <td class="field">
                                                <asp:Label ID="Label3" runat="server" Text="Cargo:" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCargo" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">
                                                <asp:Label ID="Label2" runat="server" Text="Perfil:" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblPerfil" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <br />
                                    <eo:CallbackPanel ID="cpNotificacion" runat="server" Width="100%" UpdateMode="Group"
                                        GroupName="general">
                                        <asp:Label ID="lblNotificacionOK" runat="server" CssClass="comment" Visible="False"
                                            Font-Bold="True" ForeColor="Blue"></asp:Label>
                                    </eo:CallbackPanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <ul class="options">
                                        <li>
                                            <table style="display: inline;">
                                                <tr>
                                                    <td>
                                                        <asp:LoginStatus ID="LoginStatus1" runat="server" LogoutText="&lt;img src=&quot;../img/door-open-out.png&quot; /&gt; Cerrar Sesión" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </li>
                                        <li>
                                            <table style="display: inline;">
                                                <tr>
                                                    <td>
                                                        <asp:HyperLink ID="lnkMenu" runat="server" NavigateUrl="javascript:myLayout.open('west')">
                                            <img alt="Expandir Menu Principal" border="0" src="../img/view_left_right.png"/>&nbsp;Ver Menú</asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </table>
                                        </li>
                                        <li>
                                            <eo:CallbackPanel ID="cpContrasena" runat="server" UpdateMode="Group" GroupName="general"
                                                ChildrenAsTriggers="true" Style="display: inline;" AutoDisableContents="true">
                                                <asp:LinkButton runat="server" ID="lbContrasena" OnClientClick="return MostrarVentanaCambioPwd();"> <img alt="Cambiar Contraseña" height = "15px" border="0" src="../img/contrasena.png"/>&nbsp;Cambiar Contraseña</asp:LinkButton>
                                            </eo:CallbackPanel>
                                        </li>
                                    </ul>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- allowOverflow auto-attached by option: west__showOverflowOnHover = true -->
    <div class="ui-layout-west">
        <asp:TreeView ID="treevMenu" runat="server" Target="mainFrame" NodeIndent="5" Font-Size="8pt">
            <LevelStyles>
                <asp:TreeNodeStyle Font-Underline="False" ImageUrl="~/img/home_red.png" />
                <asp:TreeNodeStyle Font-Underline="False" ImageUrl="~/img/module_file_format.png" />
                <asp:TreeNodeStyle Font-Underline="False" ImageUrl="~/img/subfolder_small.png" />
            </LevelStyles>
            <RootNodeStyle HorizontalPadding="10px" />
            <NodeStyle HorizontalPadding="5px" />
        </asp:TreeView>
    </div>
    <div class="ui-layout-south">
        <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
        <div id="footer" style="text-align: center; font-size: xx-small">
            <span>All content © copyright 2012 [Logytech Mobile S.A.S]. All rights reserved.</span></div>
    </div>
        <iframe id="mainFrame" runat="server" name="mainFrame" width="100%" class="ui-layout-center"
        height="600" frameborder="0" scrolling="auto" src="Default.aspx">
        </iframe>
   <%-- <HtmlIframe id="mainFrame" runat="server" name="mainFrame" class="ui-layout-center" width="100%"
        height="600" frameborder="0" scrolling="auto" src="Default.aspx"></HtmlIframe>--%>
    <eo:CallbackPanel ID="cpCambioContrasena" runat="server" Width="100%" UpdateMode="Group"
        GroupName="general" ChildrenAsTriggers="true" AutoDisableContents="true" LoadingDialogID="ldrWait_dlgWait">
        <eo:Dialog runat="server" ID="dlgCambiarContrasena" ControlSkinID="None" Height="350px"
            HeaderHtml="Cambio de contrase&ntilde;a" CloseButtonUrl="00020312" BackColor="White"
            CancelButton="btnCancelar" BackShadeColor="Gray" BackShadeOpacity="50" ShowButton="lbContrasena"
            ClientSideOnEnd="LimpiarVentanaCambioPwd" ClientSideOnCancel="DialogCancelHandler"
            EnableKeyboardNavigation="True">
            <ContentTemplate>
                <div style="text-align: center; padding: 5px; width: 470px;">
                    <asp:Label ID="lblErrores" runat="server" Font-Italic="True" ForeColor="Red"></asp:Label></div>
                <table class="tabla" width="480px">
                    <tr>
                        <th colspan="2">
                            <center>
                                Cambiar Contrase&ntilde;a de Usuario</center>
                        </th>
                    </tr>
                    <tr style="height: auto;">
                        <td align="center" colspan="2" style="width: 470px; height: auto;" nowrap="nowrap">
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            Contraseña Actual:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAntigua" runat="server" Width="220px" TextMode="Password" MaxLength="15"></asp:TextBox>
                            <div style="display: block">
                                <asp:RequiredFieldValidator ID="rfAntigua" runat="server" ControlToValidate="txtAntigua"
                                    ErrorMessage="El campo de contraseña actual no puede estar vacío" Display="Dynamic"
                                    Font-Italic="True"></asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            Nueva Contraseña:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNueva" runat="server" Width="220px" TextMode="Password" MaxLength="15"></asp:TextBox>
                            <div style="display: block">
                                <asp:RequiredFieldValidator ID="rfvNuevaContrasena" runat="server" ControlToValidate="txtNueva"
                                    ErrorMessage="El campo de nueva contraseña no puede estar vacío" Display="Dynamic"
                                    Font-Italic="True"></asp:RequiredFieldValidator>
                            </div>
                            <div style="display: block">
                                <asp:RegularExpressionValidator ID="reLongitud" runat="server" ControlToValidate="txtNueva"
                                    Display="Dynamic" ValidationExpression="^(?!.*\').{6,15}$" ErrorMessage="La contraseña debe tener mínimo 6 caracteres y ser alfanumérica"
                                    Font-Italic="True"></asp:RegularExpressionValidator>
                            </div>
                            <div style="display: block">
                                <asp:CompareValidator ID="cvIguales" runat="server" ControlToCompare="txtAntigua"
                                    ControlToValidate="txtNueva" Display="Dynamic" ErrorMessage="La contraseña actual y la nueva no pueden ser iguales"
                                    Font-Italic="True" Operator="NotEqual"></asp:CompareValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            Confirmar Contraseña:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNuevaConfirm" runat="server" Width="220px" TextMode="Password"
                                MaxLength="15"></asp:TextBox>
                            <div style="display: block">
                                <asp:RequiredFieldValidator ID="rfvConfirmacion" runat="server" ControlToValidate="txtNuevaConfirm"
                                    ErrorMessage="Debe confirmar la nueva contraseña" Display="Dynamic" Font-Italic="True"></asp:RequiredFieldValidator>
                            </div>
                            <div style="display: block">
                                <asp:CompareValidator ID="cvNueva" runat="server" ControlToCompare="txtNueva" ControlToValidate="txtNuevaConfirm"
                                    Display="Dynamic" ErrorMessage="La  contraseña y su confirmación no coinciden"
                                    Font-Italic="True"></asp:CompareValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <center>
                                &nbsp;<asp:Button ID="btnCambiar" runat="server" Text="Cambiar Contraseña" CssClass="search" />
                                &nbsp;<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                                    CssClass="search" />
                            </center>
                        </td>
                    </tr>
                </table>
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
    </form>
</body>
</html>
