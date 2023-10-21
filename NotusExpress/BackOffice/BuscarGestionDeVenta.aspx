<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BuscarGestionDeVenta.aspx.vb"
    Inherits="NotusExpress.BuscarGestionDeVenta" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <eo:CallbackPanel ID="cpEncabezado" runat="server" Width="100%" UpdateMode="Always">
        <uc2:ValidacionURL ID="vuControlSesion" runat="server" />
        <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
    </eo:CallbackPanel>
    <eo:CallbackPanel ID="cpFiltrosBusqueda" runat="server" Width="100%" UpdateMode="Conditional"
        LoadingDialogID="ldrWait_dlgWait">
        <table class="tabla">
            <tr>
                <th colspan="4">
                    <asp:Image ID="imgSearch" runat="server" ImageUrl="~/img/find.gif" />&nbsp;Filtros
                    de B&uacute;squeda
                </th>
            </tr>
            <tr>
                <td class="field">
                    Punto de Venta
                </td>
                <td>
                    <asp:DropDownList ID="ddlPdv" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="field">
                    Asesor Comercial
                </td>
                <td>
                    <asp:DropDownList ID="ddlAsesor" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">
                    ID Cliente
                </td>
                <td>
                    <asp:TextBox ID="txtNumIdCliente" runat="server" MaxLength="20"></asp:TextBox>
                </td>
                <td class="field">
                    Resultado Proceso
                </td>
                <td>
                    <asp:DropDownList ID="ddlResultadoProceso" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Fecha
                </td>
                <td style="vertical-align: middle !important" nowrap="nowrap">
                    <table style="padding: 0px !important">
                        <tr>
                            <td>
                                De:&nbsp;&nbsp;
                            </td>
                            <td valign="middle">
                                <input class="textbox" id="txtFechaInicial" readonly="readonly" size="11" name="txtFechaInicial"
                                    runat="server" />
                            </td>
                            <td valign="middle">
                                <a hidefocus onclick="if(self.gfPop)gfPop.fStartPop(document.form1.txtFechaInicial,document.form1.txtFechaFinal);return false;"
                                    href="javascript:void(0)">
                                    <img class="PopcalTrigger" height="22" alt="Seleccione una Fecha Inicial" src="../ControlesDeUsuario/DateRange/calbtn.gif"
                                        width="34" align="middle" border="0" /></a>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                Hasta:&nbsp;&nbsp;
                            </td>
                            <td>
                                <input class="textbox" id="txtFechaFinal" readonly="readonly" size="11" name="txtFechaFinal"
                                    runat="server" />
                            </td>
                            <td>
                                <a hidefocus onclick="if(self.gfPop)gfPop.fEndPop(document.form1.txtFechaInicial,document.form1.txtFechaFinal);return false;"
                                    href="javascript:void(0)">
                                    <img class="PopcalTrigger" height="22" alt="Seleccione una Fecha Final" src="../ControlesDeUsuario/DateRange/calbtn.gif"
                                        width="34" align="middle" border="0" /></a>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="field">
                    Tipo de Fecha
                </td>
                <td>
                    <asp:RadioButtonList ID="rblTipoFecha" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Fecha de Venta" Value="fv" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Fecha de Registro" Value="fr"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:CustomValidator ID="cusSeleccionFiltro" runat="server" ErrorMessage="Debe seleccionar por lo menos un filtro de búsqueda"></asp:CustomValidator>
                    <br />
                    <br />
                    <asp:LinkButton ID="lbConsultar" runat="server" CssClass="search"><img src="../img/funnel.png" alt="" />&nbsp;Consultar Datos</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lbLimpiarFiltros" runat="server" CssClass="search"><img src="../img/unfunnel.png" alt="" />&nbsp;Limpiar Filtros</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
    </eo:CallbackPanel>
    <eo:CallbackPanel ID="cpResultado" runat="server" Width="100%" Triggers="{ControlID:lbConsultar;Parameter:},{ControlID:lbLimpiarFiltros;Parameter:}"
        ChildrenAsTriggers="true" LoadingDialogID="ldrWait_dlgWait">
        <asp:GridView ID="gvResultado" runat="server" AllowPaging="True" PageSize="30" ShowFooter="True"
            AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="puntoDeVenta" HeaderText="Punto de Venta" />
                <asp:BoundField DataField="numeroIdentificacion" HeaderText="ID Cliente" />
                <asp:BoundField DataField="nombreApellido" HeaderText="Nombre Cliente" />
                <asp:BoundField DataField="fechaVenta" HeaderText="Fecha de Venta" DataFormatString="{0:dd/MM/yyyy}">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha de Registro" DataFormatString="{0:dd/MM/yyyy}">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="resultadoVenta" HeaderText="Resultado Proceso" />
                <asp:BoundField DataField="tipoVenta" HeaderText="Tipo de Producto" />
                <asp:BoundField DataField="tipoProducto" HeaderText="Producto" />
                <asp:TemplateField HeaderText="Opc.">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibVer" runat="server" ImageUrl="~/img/ver.png" ToolTip="Ver Informaci&oacute;n"
                            CssClass="separadorControl" />
                        <asp:ImageButton ID="ibEditar" runat="server" ImageUrl="~/img/edit.gif" ToolTip="Editar Informaci&oacute;n"
                            CssClass="separadorControl" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="gvAlternating" />
            <FooterStyle CssClass="gvFooter" />
            <HeaderStyle CssClass="thRojo" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </eo:CallbackPanel>
    <!-- iframe para uso de selector de fechas -->
    <iframe id="gToday:contrast:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
        position: absolute; top: -500px" name="gToday:contrast:agenda.js" src="../ControlesDeUsuario/DateRange/ipopeng.htm"
        frameborder="0" width="132" scrolling="no" height="142"></iframe>
    <uc3:Loader ID="ldrWait" runat="server" />
    </form>
</body>
</html>
