<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenInventarioFisico.aspx.vb"
    Inherits="NotusExpress.CrearOrdenInventarioFisico" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crear Nueva Orden de Inventario Físico</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function FiltrarSubproductos() {
            try {
                MostrarOcultarDivFloater(true);
                eo_Callback("cpSubproducto", "");
            } catch (e) {
                alert("Error al tratar de filtrar Datos.\n" + e.description);
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
        <eo:CallbackPanel ID="cpNuevaOrden" runat="server" Width="100%" UpdateMode="Self"
            ChildrenAsTriggers="true">
            <table class="tabla" width="60%">
                <tr>
                    <th colspan="2">
                        Datos de la Orden
                    </th>
                </tr>
                <tr>
                    <td class="field">
                        Bodega
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBodega" runat="server">
                        </asp:DropDownList>
                        <div style="display: block">
                            <asp:RequiredFieldValidator ID="rfvBodega" runat="server" ErrorMessage="Campo bodega requerido"
                                Display="Dynamic" ControlToValidate="ddlBodega" InitialValue="0" ValidationGroup="crearOrden"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Producto Padre
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProductoPadre" runat="server" onchange="FiltrarSubproductos();">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Subproducto
                    </td>
                    <td>
                        <eo:CallbackPanel ID="cpSubproducto" runat="server" UpdateMode="Self" ClientSideAfterUpdate="CallbackAfterUpdateHandler">
                            <asp:DropDownList ID="ddlSubproducto" runat="server">
                            </asp:DropDownList>
                        </eo:CallbackPanel>
                        <div style="display: block">
                            <asp:RequiredFieldValidator ID="rfvSubproducto" runat="server" ErrorMessage="Campo subproducto requerido"
                                Display="Dynamic" ControlToValidate="ddlSubproducto" InitialValue="0" ValidationGroup="crearOrden"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                        <asp:LinkButton ID="lbGuardar" runat="server" CssClass="search" ValidationGroup="crearOrden"><img src="../img/save_all.png" alt="" />&nbsp;Crear</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </eo:CallbackPanel>
    </div>
    <uc2:Loader ID="ldrWait" runat="server" />
    </form>
</body>
</html>
