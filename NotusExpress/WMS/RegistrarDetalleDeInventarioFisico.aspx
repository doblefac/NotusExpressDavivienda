<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegistrarDetalleDeInventarioFisico.aspx.vb"
    Inherits="NotusExpress.RegistrarDetalleDeInventarioFisico" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registrar Detalle de Inventario</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        function RegistrarSerial() {
            try {
                if (Page_ClientValidate("registrarSerial")) {
                    MostrarOcultarDivFloater(true);
                    eo_Callback("cpSerial", "registrar");
                    //document.getElementById("txtSerial").focus();
                }
            } catch (e) {
                alert("Error al tratar de registrar serial.\n" + e.description);
            }
            return false;
        }

        function AfterUpdateHandler(callback, extraData) {
            try {
                MostrarOcultarDivFloater(false);
                document.getElementById("txtSerial").select();
            } catch (e) {
                alert("Error al tratar de evaluar respuesta del servidor.\n" + e.description);
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <eo:CallbackPanel ID="cpNotificacion" runat="server" Width="100%" UpdateMode="Always">
        <uc3:ValidacionURL ID="vuControladorSesion" runat="server" />
        <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
    </eo:CallbackPanel>
    <div>
        <asp:Panel ID="pnlGeneral" runat="server">
            <table class="tabla">
                <tr>
                    <th colspan="6">
                        Datos de la Orden
                    </th>
                </tr>
                <tr>
                    <td class="field">
                        Bodega
                    </td>
                    <td>
                        <asp:Label ID="lblBodega" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                    <td class="field">
                        Producto Padre
                    </td>
                    <td>
                        <asp:Label ID="lblProductoPadre" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                    <td class="field">
                        Subproducto
                    </td>
                    <td>
                        <asp:Label ID="lblSubproducto" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        No. Orden
                    </td>
                    <td>
                        <asp:Label ID="lblIdOrden" runat="server" Font-Bold="true" Font-Size="Large" Text=""></asp:Label>
                    </td>
                    <td class="field">
                        Cantidad Leida
                    </td>
                    <td colspan="3">
                        <eo:CallbackPanel ID="cpCantidad" runat="server" UpdateMode="Group" GroupName="registro">
                            <asp:Label ID="lblCantidadLeida" runat="server" Font-Bold="true" Font-Size="Large"
                                Text=""></asp:Label>
                        </eo:CallbackPanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                        <eo:CallbackPanel ID="cpCerrar" runat="server" UpdateMode="Group" GroupName="registro" LoadingDialogID="ldrWait_dlgWait"
                        ChildrenAsTriggers="true">
                            <asp:LinkButton ID="lbCerrarOrden" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea cerrar la orden?');">
                                <img src="../img/door-open-out.png" alt="" />&nbsp;Cerrar Orden</asp:LinkButton>
                        </eo:CallbackPanel>
                    </td>
                </tr>
            </table>
            <br />
            <div id="divFloater" style="display: none;">
                <table width="98%" align="center">
                    <tr>
                        <td style="width: 40px">
                            <asp:Image ID="imgLoading" runat="server" ImageUrl="~/img/loader_dots.gif" />
                        </td>
                        <td valign="middle">
                            <b>Procesando...</b>
                        </td>
                    </tr>
                </table>
            </div>
            <table class="tabla">
                <tr>
                    <th colspan="2">
                        Información del Serial
                    </th>
                </tr>
                <tr>
                    <td class="field">
                        Serial
                    </td>
                    <td>
                        <eo:CallbackPanel ID="cpSerial" runat="server" UpdateMode="Group" GroupName="registro"
                            AutoDisableContents="true" ClientSideAfterUpdate="AfterUpdateHandler">
                            <asp:TextBox ID="txtSerial" Width="200px" MaxLength="30" runat="server" onkeydown="ProcesarEnterGeneral(this,'lbGuardar');">
                            </asp:TextBox>
                        </eo:CallbackPanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="display: block">
                            <asp:RequiredFieldValidator ID="rfvSerial" runat="server" ErrorMessage="Serial requerido"
                                Display="Dynamic" ControlToValidate="txtSerial" ValidationGroup="registrarSerial"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revSerialTarjeta" runat="server" ErrorMessage="Por favor ingrese un serial v&aacute;lido. Valor n&uacute;merico de 14, 16 o 22 d&iacute;gitos"
                                Display="Dynamic" ControlToValidate="txtSerial" ValidationExpression="^([0-9]{14}|[0-9]{16}|[0-9]{22})$"
                                ValidationGroup="registrarSerial"></asp:RegularExpressionValidator>
                        </div>
                        <br />
                        <asp:LinkButton ID="lbGuardar" runat="server" CssClass="search" OnClientClick="return RegistrarSerial();"
                            ValidationGroup="registrarSerial"><img src="../img/save_all.png" alt="" />&nbsp;Registrar Serial</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <uc2:Loader ID="ldrWait" runat="server" />
    </form>
</body>
</html>
