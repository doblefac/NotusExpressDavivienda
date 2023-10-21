<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteInventarioReferencia.aspx.vb"
    Inherits="NotusExpress.ReporteInventarioReferencia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reporte De Inventario Por Referencia</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function EjecutarCallbackGeneral(s, e, parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                loadingPanel.Show();
                cpGeneral.PerformCallback(parametro);
            }
        }

        function OnExpandCollapseButtonClick(s, e) {
            var isVisible = pnlFiltros.GetVisible();
            s.SetText(isVisible ? "+" : "-");
            pnlFiltros.SetVisible(!isVisible);
        }

        function LimpiarFiltros() {
            txtFiltroCodigo.SetText('');
            txtFiltroProducto.SetText('');
            cbFiltroEstado.SetSelectedIndex(0);
            cbFiltroFabricante.SetSelectedIndex(0);
            cbFiltroGarante.SetSelectedIndex(0);
            gluAlmacen.GetGridView().UnselectAllRowsOnPage();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <vu:ValidacionURL ID="vuControlSesion" runat="server" />
    <dx:ASPxHiddenField ID="hfDimensiones" ClientInstanceName="hfDimensiones" runat="server">
    </dx:ASPxHiddenField>
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        <br />
    </div>
    <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtros de B&uacute;squeda">
        <HeaderTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="white-space: nowrap;" align="left">
                        Filtros de B&uacute;squeda
                    </td>
                    <td style="width: 1%; padding-left: 5px;">
                        <dx:ASPxButton ID="btnExpandCollapse" runat="server" Text="-" AllowFocus="False"
                            AutoPostBack="False" Width="20px">
                            <Paddings Padding="1px" />
                            <FocusRectPaddings Padding="0" />
                            <ClientSideEvents Click="OnExpandCollapseButtonClick" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxPanel ID="pnlFiltros" runat="server" Width="100%" ClientInstanceName="pnlFiltros">
                    <Paddings Padding="0px" />
                    <PanelCollection>
                        <dx:PanelContent>
                            <table>
                                <tr>
                                    <td class="field">
                                        Ciudad:
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox runat="server" ClientInstanceName="cbFiltroCiudad" ID="cbFiltroCiudad"
                                            Width="300px" AutoPostBack="true">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td class="field">
                                        Producto:
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbFiltroProducto" runat="server" ClientInstanceName="cbFiltroProducto"
                                            ValueType="System.Byte" AutoPostBack="true" Width="300px">
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Bodega:
                                    </td>
                                    <td style="vertical-align: middle !important" nowrap="nowrap">
                                        <dx:ASPxGridLookup ID="gluPdv" runat="server" KeyFieldName="IdPdv" SelectionMode="Multiple"
                                            IncrementalFilteringMode="StartsWith" TextFormatString="{0}" Width="300px" ClientInstanceName="gluPdv"
                                            MultiTextSeparator=", " AllowUserInput="false">
                                            <ClientSideEvents ButtonClick="function(s,e) {gluPdv.GetGridView().UnselectAllRowsOnPage(); gluPdv.HideDropDown(); }" />
                                            <Buttons>
                                                <dx:EditButton Text="X">
                                                </dx:EditButton>
                                            </Buttons>
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                                                <dx:GridViewDataTextColumn FieldName="NombrePdv" />
                                            </Columns>
                                            <GridViewProperties>
                                                <SettingsBehavior AllowDragDrop="False" EnableRowHotTrack="True" />
                                                <SettingsPager NumericButtonCount="5" PageSize="10" />
                                            </GridViewProperties>
                                        </dx:ASPxGridLookup>
                                    </td>
                                    <td class="field">
                                        Subproducto:
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbFiltroSubproducto" runat="server" ClientInstanceName="cbFiltroSubproducto"
                                            ValueType="System.Byte" Width="300px">
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">Num. D&iacute;as de Inventario</td>
                                    <td colspan="3">
                                        <dx:ASPxSpinEdit ID="seNumDiasInventario" runat="server" Width="80px" 
                                            Number="1" MaxLength="3" MinValue="1" NumberType="Integer" MaxValue="365">
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="padding-top: 8px">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="white-space: nowrap;" align="center">
                                                    <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline!important;"
                                                        AutoPostBack="False" ValidationGroup="Filtrado">
                                                        <Image Url="~/img/find.gif">
                                                        </Image>
                                                        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'filtrarDatos');}" />
                                                    </dx:ASPxButton>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar"
                                                        Style="display: inline!important;" AutoPostBack="false">
                                                        <Image Url="~/img/eraser.png">
                                                        </Image>
                                                        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'limpiarFiltros'); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="clear: both;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
        <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e); }" />
        <PanelCollection>
            <dx:PanelContent>
                <asp:Panel ID="pnlAuxiliar" runat="server">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxComboBox ID="cbFormatoExportar" runat="server" ShowImageInEditBox="true"
                                    SelectedIndex="-1" ValueType="System.String" EnableCallbackMode="True" AutoResizeWithContainer="true"
                                     ClientInstanceName="cbFormatoExportar" Width="210px">
                                    <Items>
                                        <dx:ListEditItem ImageUrl="~/img/excel.gif" Text="Exportar a XLS" Value="xls" Selected="true" />
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
                            </td>
                            <td style="width: 50px">
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnExpandir" runat="server" ToolTip="Expandir Todos Los Grupos"
                                    Style="vertical-align: middle;" Text="Expandir Todo" AutoPostBack="false">
                                    <ClientSideEvents Click="function(s, e) { gvInventario.PerformCallback('expandir');}" />
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnContraer" runat="server" ToolTip="Contraer Todos Los Grupos"
                                    Style="vertical-align: middle;" Text="Contraer Todo" AutoPostBack="false">
                                    <ClientSideEvents Click="function(s, e) { gvInventario.PerformCallback('contraer');}" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <dx:ASPxGridView ID="gvInventario" runat="server" AutoGenerateColumns="False" Font-Size="Small"
                    ClientInstanceName="gvInventario">
                    <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e);}" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Ciudad" FieldName="Ciudad" VisibleIndex="0">
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Bodega" FieldName="PuntoDeVenta" VisibleIndex="1">
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Código de Entrega" FieldName="CodigoEntrega"
                            VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Producto" FieldName="Producto" VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Subproducto" FieldName="Subproducto" VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Cupo" FieldName="Cupo" VisibleIndex="5">
                            <PropertiesTextEdit DisplayFormatString="c0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Total Inv." FieldName="CantidadInventario" VisibleIndex="6">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Num. Ventas Actual" FieldName="TotalVentasMesActual"
                            VisibleIndex="7">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Num. Ventas Mes -1" FieldName="TotalVentasMesAnterior"
                            VisibleIndex="8">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Num. Ventas Mes -2" FieldName="TotalVentasMesAntesAnterior"
                            VisibleIndex="9">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Prom. Diario de Ventas" FieldName="PromedioDiario" VisibleIndex="10">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Desv. Diaria de Ventas" FieldName="DesviacionDiaria" VisibleIndex="11">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Sugerido Diario" FieldName="SugeridoDiario" VisibleIndex="12">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Asignaci&oacute;n Requerida" FieldName="AsignacionRequerida" VisibleIndex="13">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager PageSize="50">
                    </SettingsPager>
                    <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False"
                        ShowGroupPanel="true" />
                    <SettingsText CommandEdit="Editar" Title="Reporte de Inventario por Referencia" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
                    <SettingsBehavior EnableCustomizationWindow="true" AutoExpandAllGroups="true" />
                </dx:ASPxGridView>
                <dx:ASPxGridViewExporter ID="gveExportador" runat="server" GridViewID="gvInventario">
                </dx:ASPxGridViewExporter>
                <%--<msgp:MensajePopUp ID="mensajero" runat="server" />--%>
            </dx:PanelContent>
        </PanelCollection>
        <LoadingDivStyle CssClass="modalBackground">
        </LoadingDivStyle>
    </dx:ASPxCallbackPanel>
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
