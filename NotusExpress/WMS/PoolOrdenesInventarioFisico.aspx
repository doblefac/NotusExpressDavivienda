<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PoolOrdenesInventarioFisico.aspx.vb"
    Inherits="NotusExpress.PoolOrdenesInventarioFisico" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pool de Ordenes de Inventario Físico</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
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

        function FiltrarSubproductosCrear() {
            try {
                MostrarOcultarDivFloater(true);
                eo_Callback("cpSubproductoCrear", "");
            } catch (e) {
                alert("Error al tratar de filtrar Datos.\n" + e.description);
            }
        }

        function MostrarVentanaCreacionOrden() {
            eo_Callback("cpCreacionOrden", "limpiarDatos");
            return (false);
        }

        function AsignarTamanoVentana() {
            document.getElementById('hfMedidasVentana').value = GetWindowSize();
        }

        function ActualizarPoolDeOrdenes(dlg, args) {
            if (args != null) {
                eo_Callback("cpResultado", "actualizar");
            }
        }
    </script>

</head>
<body onload="AsignarTamanoVentana();">
    <form id="form1" runat="server">
    <eo:CallbackPanel ID="cpNotificacion" runat="server" Width="100%" UpdateMode="Always">
        <uc3:ValidacionURL ID="vuControladorSesion" runat="server" />
        <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
        <asp:HiddenField ID="hfMedidasVentana" runat="server" />
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
        <table class="tabla">
            <tr>
                <th colspan="6">
                    Filtros de Búsqueda
                </th>
            </tr>
            <tr>
                <td class="field">
                    Bodega
                </td>
                <td>
                    <asp:DropDownList ID="ddlBodega" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="field">
                    Producto Padre
                </td>
                <td>
                    <asp:DropDownList ID="ddlProductoPadre" runat="server" onchange="FiltrarSubproductos();">
                    </asp:DropDownList>
                </td>
                <td class="field">
                    Subproducto
                </td>
                <td>
                    <eo:CallbackPanel ID="cpSubproducto" runat="server" UpdateMode="Self" ClientSideAfterUpdate="CallbackAfterUpdateHandler">
                        <asp:DropDownList ID="ddlSubproducto" runat="server">
                        </asp:DropDownList>
                    </eo:CallbackPanel>
                </td>
            </tr>
            <tr>
                <td class="field">
                    Estado
                </td>
                <td>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Text="Seleccione un Estado" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Activo" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Cerrado" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Anulado" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="field">
                    Tipo de Fecha
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipoFecha" runat="server">
                        <asp:ListItem Text="Seleccione un Tipo" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Fecha de Creaci&oacute;n" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Fecha de Cierre" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="field">
                    Fecha
                </td>
                <td style="vertical-align: middle" nowrap="nowrap">
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
                                        width="34" align="middle" border="0"></a>
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
                                        width="34" align="middle" border="0"></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <br />
                    <asp:LinkButton ID="lbConsultar" runat="server" CssClass="search"><img src="../img/find.gif" alt="" />&nbsp;Consultar</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lbQuitarFiltros" runat="server" CssClass="search"><img src="../img/unfunnel.png" alt="" />&nbsp;Quitar Filtros</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <div style="margin-bottom: 15px;">
            <asp:LinkButton ID="lbCrearNuevaOrden" runat="server" ToolTip="Crear Nueva Orden"
                OnClientClick="return MostrarVentanaCreacionOrden();" CssClass="search"><img src="../img/new.png" alt="" />&nbsp;Crear Nueva Orden</asp:LinkButton><br />
        </div>
        <eo:CallbackPanel ID="cpResultado" runat="server" GroupName="resultado"
            ChildrenAsTriggers="True" LoadingDialogID="ldrWait_dlgWait" 
            Triggers="{ControlID:lbConsultar;Parameter:},{ControlID:lbQuitarFiltros;Parameter:}">
            <asp:Panel ID="pnlResultado" runat="server">
                <asp:GridView ID="gvOrdenInventario" runat="server" CellPadding="4" CellSpacing="2"
                    ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" ShowFooter="true">
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="IdOrden" HeaderText="No. Orden">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Bodega" HeaderText="Bodega" />
                        <asp:BoundField DataField="ProductoPadre" HeaderText="Producto Padre" />
                        <asp:BoundField DataField="Subproducto" HeaderText="Subproducto" />
                        <asp:BoundField DataField="CantidadEnInventario" HeaderText="Cantidad Le&iacute;da">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaCreacion" HeaderText="FechaCreacion" DataFormatString="{0:dd/MM/yyyy}">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Creador" HeaderText="Creador" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaCierre" HeaderText="Fecha de Cierre" DataFormatString="{0:dd/MM/yyyy}">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ResponsableCierre" HeaderText="Responsable de Cierre" />
                        <asp:TemplateField HeaderText="Opc.">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibAdicionarDetalle" runat="server" ImageUrl="~/img/package.png"
                                    ToolTip="Adicionar Detalle" CommandName="leerSeriales" CommandArgument='<%# Bind("IdOrden") %>' />
                                <asp:ImageButton ID="ibCerrarOrden" runat="server" ImageUrl="~/img/door-open-out.png"
                                    CommandName="cerrarOrden" CommandArgument='<%# Bind("IdOrden") %>'
                                    ToolTip="Cerrar Orden" OnClientClick="return confirm('¿Realmente desea cerrar la orden?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </eo:CallbackPanel>
        <eo:CallbackPanel ID="cpCreacionOrden" runat="server" Width="100%" UpdateMode="Group"
            GroupName="resultado" ChildrenAsTriggers="true" LoadingDialogID="ldrWait_dlgWait">
            <eo:Dialog runat="server" ID="dlgCrearOrden" ControlSkinID="None" HeaderHtml="Crear Orden de Inventario Físico"
                CloseButtonUrl="00020312" BackColor="White" BackShadeColor="Gray" BackShadeOpacity="60"
                ClientSideOnEnd="ActualizarPoolDeOrdenes" EnableKeyboardNavigation="True" MinHeight="350"
                MinWidth="350">
                <ContentTemplate>
                </ContentTemplate>
                <FooterStyleActive CssText="padding-right: 4px !important; padding-left: 4px !important; font-size: 8pt !important; padding-bottom: 4px !important; padding-top: 4px !important; font-family: tahoma">
                </FooterStyleActive>
                <HeaderStyleActive CssText="background-image:url('00020311') !important !important;color:black !important;font-family:'trebuchet ms' !important;font-size:10pt !important;font-weight:bold !important;padding-bottom:5px !important;padding-left:8px !important;padding-right:3px !important;padding-top:0px !important;">
                </HeaderStyleActive>
                <ContentStyleActive CssText="padding-right: 4px !important; padding-left: 4px !important; font-size: 8pt !important; padding-bottom: 4px !important; padding-top: 4px !important; font-family: tahoma">
                </ContentStyleActive>
            </eo:Dialog>
        </eo:CallbackPanel>
    </div>
    <uc2:Loader ID="ldrWait" runat="server" />
    <iframe id="gToday:contrast:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
        position: absolute; top: -500px" name="gToday:contrast:agenda.js" src="../ControlesDeUsuario/DateRange/ipopeng.htm"
        frameborder="0" width="132" scrolling="no" height="142"></iframe>
    </form>
</body>
</html>
