<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteInventarioSerializado.aspx.vb"
    Inherits="NotusExpress.ReporteInventarioSerializado" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reporte De Inventario Serializado</title>
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

        function EnValidacionDeRango(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;

            if (fechaInicio == null || fechaInicio == false || fechaFin == null || fechaFin == false) { return; }

            if (fechaInicio > fechaFin) { e.isValid = false; }

            var diff = Math.floor((fechaFin.getTime() - fechaInicio.getTime()) / (1000 * 60 * 60 * 24));

            if (diff > 60) { e.isValid = false; }
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
                                            Width="300px" AutoPostBack="True">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td class="field">
                                        Producto:
                                    </td>
                                    <td class="field">
                                        <dx:ASPxComboBox ID="cbFiltroProducto" runat="server" ClientInstanceName="cbFiltroProducto"
                                            ValueType="System.Byte" AutoPostBack="True">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td class="field">
                                        Estado
                                    </td>
                                    <td class="field">
                                        <dx:ASPxComboBox ID="cbFiltroEstado" runat="server" ClientInstanceName="cbFiltroEstado"
                                            ValueType="System.Byte">
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
                                            ValueType="System.Byte">
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Fecha Inicial:
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio">
                                            <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                            </CalendarProperties>
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td class="field">
                                        Fecha Final:
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin">
                                            <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                            </CalendarProperties>
                                            <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha Final menor que Fecha final."
                                                ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                            </ValidationSettings>
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="padding-top: 8px">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="white-space: nowrap;" align="center">
                                                    <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline!important;"
                                                        AutoPostBack="false" ValidationGroup="Filtrado">
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
                                    <ClientSideEvents Click="function(s, e) { gvInventarioS.PerformCallback('expandir');}" />
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnContraer" runat="server" ToolTip="Contraer Todos Los Grupos"
                                    Style="vertical-align: middle;" Text="Contraer Todo" AutoPostBack="false">
                                    <ClientSideEvents Click="function(s, e) { gvInventarioS.PerformCallback('contraer');}" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <dx:ASPxGridView ID="gvInventarioS" runat="server" AutoGenerateColumns="False" Font-Size="Small"
                    ClientInstanceName="gvInventarioS">
                    <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e);}" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Ciudad" FieldName="Ciudad" VisibleIndex="0">
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Bodega" FieldName="Bodega" VisibleIndex="1">
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
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
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Serial" FieldName="Serial" VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Fecha Asignación" FieldName="FechaAsignacion" VisibleIndex="7">
                            <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Estado" FieldName="Estado" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager PageSize="50">
                    </SettingsPager>
                    <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False"
                        ShowGroupPanel="true" />
                    <SettingsText CommandEdit="Editar" Title="Reporte de Inventario Serializado" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
                    <SettingsBehavior EnableCustomizationWindow="true" AutoExpandAllGroups="true" />
                </dx:ASPxGridView>
                <dx:ASPxGridViewExporter ID="gveExportador" runat="server" GridViewID="gvInventarioS">
                </dx:ASPxGridViewExporter>
                <msgp:MensajePopUp ID="mensajero" runat="server" />
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
