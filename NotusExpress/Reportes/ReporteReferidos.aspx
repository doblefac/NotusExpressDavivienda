<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteReferidos.aspx.vb"
    Inherits="NotusExpress.ReporteReferidos"%>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Reporte de Referidos</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        String.prototype.trim = function() { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }

        function OnMakeCallback(s, e) {
            var resultado = ValidarSeleccionRango();
            if (resultado) {
                cpResultadoReporte.PerformCallback();
                loadingPnl.Show();
            }
            return resultado;
        }

        function OnEndCallback(s, e) {
            cpResultadoReporte.PerformCallback();
            loadingPnl.Hide();
        }

        function ValidarSeleccionRango(source, args) {
            try {
                var fechaInicial = document.getElementById('<%=txtFechaInicial.ClientID %>').value;
                var fechaFinal = document.getElementById('<%=txtFechaFinal.ClientID %>').value;
                var lbl = document.getElementById('<%=lblMensajeValidador.ClientID %>');

                if ((fechaInicial.length > 0 && fechaFinal.length == 0) || (fechaInicial.length == 0 && fechaFinal.length > 0)) {
                    //args.IsValid = false;
                    lbl.innerHTML = "Debe seleccionar los dos valores del rango. ";
                    return false;
                } else {
                    //args.IsValid = true;
                    lbl.innerHTML = "";
                }
                return true;
            } catch (e) {
                //args.IsValid = false;
                alert("Imposible evaluar si se ha seleccionado un rango de fechas válido.\n" + e.description);
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <uc2:ValidacionURL ID="vuControlSesion" runat="server" />
    <dx:ASPxCallbackPanel ID="cpResultadoReporte" ClientInstanceName="cpResultadoReporte"
        runat="server" >
        <ClientSideEvents EndCallback="OnEndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
                <table class="tabla">
                    <tr>
                        <th colspan="2">
                            
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
                                <asp:ListItem Text="Seleccione un Punto de Venta" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Asesor Comercial
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAsesor" runat="server">
                                <asp:ListItem Text="Seleccione un Asesor" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Fecha de Registro
                        </td>
                        <td style="vertical-align: middle !important" nowrap="nowrap">
                            <table style="padding: 0px !important; border: none !important">
                                <tr>
                                    <td>
                                        De:&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <input class="textbox" id="txtFechaInicial" readonly="readonly" size="11" name="txtFechaInicial"
                                            runat="server" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Literal ID="ltLinkFechaInicial" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        &nbsp;-&nbsp;
                                    </td>
                                    <td>
                                        Hasta:&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <input class="textbox" id="txtFechaFinal" readonly="readonly" size="11" name="txtFechaFinal"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltLinkFechaFinal" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                            <div style="display: block">
                                <asp:Label ID="lblMensajeValidador" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            <asp:LinkButton ID="lbConsultar" runat="server" CssClass="search" OnClientClick="return OnMakeCallback();"><img src="../img/funnel.png" alt="" />&nbsp;Consultar Datos</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lbLimpiarFiltros" runat="server" CssClass="search" OnClientClick="OnMakeCallback();"><img src="../img/unfunnel.png" alt="" />&nbsp;Limpiar Filtros</asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlResultado" runat="server">
                    <dx:ASPxComboBox ID="cbFormatoExportar" runat="server" ShowImageInEditBox="true"
                        SelectedIndex="-1" ValueType="System.String" EnableCallbackMode="True" AutoResizeWithContainer="true"
                        Theme="RedWine"  ClientInstanceName="cbFormatoExportar">
                        <Items>
                            <dx:ListEditItem ImageUrl="~/img/excel.gif" Text="Exportar a XLS" Value="xls" Selected="true" />
                            <dx:ListEditItem ImageUrl="~/img/pdf.png" Text="Exportar a PDF" Value="pdf" />
                            <dx:ListEditItem ImageUrl="~/img/xlsx_win.png" Text="Exportar a XLSX" Value="xlsx" />
                            <dx:ListEditItem ImageUrl="~/img/csv.png" Text="Exportar a CSV" Value="csv" />
                        </Items>
                        <Buttons>
                            <dx:EditButton Text="Exportar" ToolTip="Exportar Reporte al formato seleccionado">
                                <Image Url="~/img/Download.png">
                                </Image>
                            </dx:EditButton>
                        </Buttons>
                        <ValidationSettings ErrorText="Formato a exportar requerido" RequiredField-ErrorText="Formato a exportar requerido"
                            Display="Dynamic" CausesValidation="true" ValidateOnLeave="true" ValidationGroup="exportar">
                            <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular">
                            </RegularExpression>
                            <RequiredField IsRequired="true" ErrorText="Formato a exportar requerido" />
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                    &nbsp;
                    <dx:ASPxGridView ID="gvReporte" ClientInstanceName="gvReporte" runat="server" Width="98%"
                        AutoGenerateColumns="False" Theme="RedWine" EnableRowsCache="False">
                        <Columns>
                            <dx:GridViewDataColumn FieldName="idInfoReferido" Caption="ID" VisibleIndex="0" CellStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="fechaRegistro" Caption="Fecha de Registro" VisibleIndex="1"
                                CellStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="nombreApellido" Caption="Nombre del Cliente" VisibleIndex="2" />
                            <dx:GridViewDataColumn FieldName="numeroIdentificacion" Caption="C&eacute;dula" VisibleIndex="3" />
                            <dx:GridViewDataColumn FieldName="telefonoFijo" Caption="Tel&eacute;fono Fijo" VisibleIndex="4" />
                            <dx:GridViewDataColumn FieldName="celular" Caption="Tel&eacute;fono Celular" VisibleIndex="5" />
                            <dx:GridViewDataColumn FieldName="direccion" Caption="Direcci&oacute;n" VisibleIndex="6" />
                            <dx:GridViewDataColumn FieldName="email" Caption="Correo Electr&oacute;nico" VisibleIndex="7" />
                            <dx:GridViewDataColumn FieldName="puntoDeVenta" Caption="Punto De Venta" VisibleIndex="8" />
                            <dx:GridViewDataColumn FieldName="proveedor" Caption="Proveedor" VisibleIndex="9" />
                            <dx:GridViewDataColumn FieldName="ciudad" Caption="Ciudad" VisibleIndex="10" />
                        </Columns>
                        <Settings ShowGroupPanel="True" ShowFooter="True" ShowHeaderFilterButton="true" />
                        <SettingsPopup>
                            <HeaderFilter Height="200" />
                        </SettingsPopup>
                        <SettingsBehavior AllowFocusedRow="true" AutoExpandAllGroups="True" />
                        <SettingsLoadingPanel Text="Cargando&amp;hellip;"></SettingsLoadingPanel>
                        <SettingsPager PageSize="100">
                        </SettingsPager>
                        <GroupSummary>
                            <dx:ASPxSummaryItem FieldName="puntoDeVenta" SummaryType="Count" Tag="Num. Ventas" />
                        </GroupSummary>
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="puntoDeVenta" SummaryType="Count" Tag="Total Ventas" />
                        </TotalSummary>
                    </dx:ASPxGridView>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:ASPxGridViewExporter ID="gveExportador" runat="server" GridViewID="gvReporte">
    </dx:ASPxGridViewExporter>
    <dx:ASPxLoadingPanel ID="loadingPnl" runat="server" ClientInstanceName="loadingPnl"
        Modal="True" Theme="DevEx">
    </dx:ASPxLoadingPanel>
    <iframe id="gToday:contrast:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
        position: absolute; top: -500px" name="gToday:contrast:agenda.js" src="../ControlesDeUsuario/DateRange/ipopeng.htm"
        frameborder="0" width="132" scrolling="no" height="142"></iframe>
    </form>
</body>
</html>
