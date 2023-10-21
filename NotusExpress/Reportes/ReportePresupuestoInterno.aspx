<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportePresupuestoInterno.aspx.vb" Inherits="NotusExpress.ReportePresupuestoInterno" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Reporte de Presupuesto (Interno)::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function TamanioVentana() {
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }
        }

        function OnExpandCollapseButtonClick(s, e) {
            var isVisible = pnlFiltros.GetVisible();
            s.SetText(isVisible ? "+" : "-");
            pnlFiltros.SetVisible(!isVisible);
        }

        function ValidacionDeFecha(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;
            if (fechaInicio == null || fechaInicio == false || fechaFin == null || fechaFin == false) { return; }
            if (fechaInicio > fechaFin) { e.isValid = false; }
            var diff = Math.floor((fechaFin.getTime() - fechaInicio.getTime()) / (1000 * 60 * 60 * 24));
            if (diff > 60) { e.isValid = false; }
        }

        function LimpiaFormulario() {
            if (confirm("¿Realmente desea limpiar los campos del formulario?")) {
                ASPxClientEdit.ClearEditorsInContainerById('formPrincipal');
            }
        }

        function ValidarFiltros() {
            if (deFechaInicio.GetDate() == null && deFechaFin.GetDate() == null && cmbProducto.GetValue() == null && cmbBase.GetValue() == null) {
                alert('Debe seleccionar por lo menos un filtro de búsqueda.');
            } else {
                if (deFechaInicio.GetDate() == null || deFechaFin.GetDate() == null) {
                    if (cmbProducto.GetValue() == null && cmbBase.GetValue() == null) {
                        alert('Debe seleccionar los dos rangos de fecha.');
                    } else {
                        loadingPanel.Show();
                        pivotGrid.PerformCallback();
                    }
                } else {
                    loadingPanel.Show();
                    pivotGrid.PerformCallback();
                }
            }
        }

        function toggle(control) {
            $("#" + control).slideToggle("slow");
        }

        function ShowDrillDown() {
            TamanioVentana()
            var mainElement = pivotGrid.GetMainElement();
            var pcWidth = myWidth;
            var pxHieght = myWidth;
            var width = _aspxGetDocumentClientWidth()
            var height = _aspxGetDocumentClientHeight();
            DrillDownWindow.ShowAtPos(width / 4, height / 6);
            DrillDownWindow.ShowWindow();
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
        <div id="divEncabezado">
            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        </div>
        <div id="divGeneral" runat="server">
            <input runat="server" id="ColumnIndex" type="hidden" enableviewstate="true" />
            <input runat="server" id="RowIndex" type="hidden" enableviewstate="true" />
            <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtros de B&uacute;squeda" Width="85%">
                <HeaderTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="white-space: nowrap;" align="left">Filtros de B&uacute;squeda
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
                            <Paddings Padding="0px"></Paddings>
                            <PanelCollection>
                                <dx:PanelContent>
                                    <table width="95%">
                                        <tr>
                                            <td class="field">Producto:
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbProducto" runat="server" ClientInstanceName="cmbProducto" IncrementalFilteringMode="Contains"
                                                    ValueType="System.Int32" TabIndex="1" Width="300px">
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td class="field">Base de Datos:
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbBase" runat="server" ClientInstanceName="cmbBase" IncrementalFilteringMode="Contains"
                                                    ValueType="System.Int32" TabIndex="4" Width="300px">
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">Fecha Inicial:
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio"
                                                    TabIndex="5" Width="300px">
                                                    <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                    </CalendarProperties>
                                                    <ClientSideEvents Validation="ValidacionDeFecha"></ClientSideEvents>
                                                    <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inválido. Fecha inicial menor que Fecha final. Rango menor que 60 días"
                                                        ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="ValidacionDeFecha" />
                                                </dx:ASPxDateEdit>
                                            </td>
                                            <td class="field">Fecha Final:
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin" TabIndex="6"
                                                    Width="300px">
                                                    <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                    </CalendarProperties>
                                                    <ClientSideEvents Validation="ValidacionDeFecha"></ClientSideEvents>
                                                    <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inválido. Fecha inicial menor que Fecha final. Rango menor que 60 días"
                                                        ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="ValidacionDeFecha" />
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="padding-top: 8px">
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="white-space: nowrap;" align="center">
                                                            <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline!important;"
                                                                AutoPostBack="false" ValidationGroup="Filtrado" TabIndex="7" ClientInstanceName="btnBuscar" HorizontalAlign="Justify">
                                                                <ClientSideEvents Click="function(s, e) { 
                                                                    ValidarFiltros(s, e); 
                                                                 }"></ClientSideEvents>
                                                                <Image Url="~/img/find.gif">
                                                                </Image>
                                                            </dx:ASPxButton>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;<dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar"
                                                                Style="display: inline!important;" AutoPostBack="false" TabIndex="8" HorizontalAlign="Justify">
                                                                <ClientSideEvents Click="function(s, e) { LimpiaFormulario(); }"></ClientSideEvents>
                                                                <Image Url="~/img/eraser.png">
                                                                </Image>
                                                                <ClientSideEvents Click="function(s, e) { 
                                                                                LimpiaFormulario(); 
                                                                            }" />
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
            <div id="divDatos" style="float: left; margin-top: 5px; width: 100%; visibility: visible">
                <dx:ASPxRoundPanel ID="rpEntregas" runat="server" HeaderText="Detalle"
                    Width="100%">
                    <PanelCollection>
                        <dx:PanelContent>
                            <dx:ASPxPivotGrid ID="pivotGrid" runat="server" ClientInstanceName="pivotGrid" Width="100%" OptionsLoadingPanel-Enabled="false">
                                <ClientSideEvents EndCallback="function (s, e){
                                    loadingPanel.Hide();
                                    $('#divEncabezado').html(s.cpMensaje);
                                }" />
                                <Styles>
                                    <CellStyle Cursor="pointer" />
                                    <CellStyle Cursor="pointer"></CellStyle>
                                </Styles>
                                <Fields>
                                    <dx:PivotGridField Area="RowArea" AreaIndex="0" FieldName="nombreArchivo" Caption="Base"
                                        ID="fieldnombreArchivo" TotalsVisibility="None" SummaryType="Count">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField Area="RowArea" AreaIndex="1" FieldName="asesor" Caption="Asesor"
                                        ID="fieldasesor" TotalsVisibility="None" SummaryType="Count">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField Area="RowArea" AreaIndex="2" FieldName="indicador" Caption="Indicador"
                                        ID="fieldindicador" TotalsVisibility="None" SummaryType="Count">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldidCliente" Area="DataArea" AreaIndex="3" Caption="Cantidad"
                                        FieldName="nombreArchivo" SummaryType="Count" AllowedAreas="DataArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldClientePor" Area="DataArea" AreaIndex="4" CellFormat-FormatString="P1"
                                        CellFormat-FormatType="Numeric" Caption="%" FieldName="indicador" SummaryType="Custom"
                                        AllowedAreas="DataArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldCantidadBase" Area="DataArea" AreaIndex="5" Caption="Cantidad Base" Visible="False"
                                        FieldName="cantidadBase" SummaryType="Max" AllowedAreas="DataArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldIdAsesor" Area="DataArea" AreaIndex="6" Caption="idAsesor" Visible="false"
                                        FieldName="idAsesor" SummaryType="Max" AllowedAreas="DataArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldPlano" Area="DataArea" AreaIndex="7" Caption="idPlano" Visible="false"
                                        FieldName="idPlano" SummaryType="Max" AllowedAreas="DataArea">
                                    </dx:PivotGridField>
                                </Fields>
                                <OptionsView ShowRowTotals="False" ShowRowGrandTotals="False" />
                                <OptionsView ShowHorizontalScrollBar="false"></OptionsView>
                                <OptionsPager RowsPerPage="20"></OptionsPager>
                                <OptionsLoadingPanel Enabled="False"></OptionsLoadingPanel>
                                <OptionsFilter NativeCheckBoxes="False"></OptionsFilter>
                            </dx:ASPxPivotGrid>
                            <dx:ASPxPivotGridExporter ID="pgeExportador" runat="server" ASPxPivotGridID="pivotGrid"
                                Visible="False">
                            </dx:ASPxPivotGridExporter>
                            <dx:ASPxPopupControl ID="pcModal" Modal="true" runat="server" Height="1px" AllowDragging="True"
                                ClientInstanceName="DrillDownWindow" Left="200" CloseAction="CloseButton"
                                Width="750px" HeaderText="Detalle Gestiones">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="pccModal" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <dx:ASPxGridView ID="gvDatos" runat="server" ClientInstanceName="GridView" AutoGenerateColumns="false"
                                                        KeyFieldName="idGestion" Width="100%">
                                                        <ClientSideEvents EndCallback="function(s,e){
                                                            gvMetas.PerformCallback();
                                                        }" />
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="IdentificacionCliente" Caption="Identificacion Cliente" ShowInCustomizationForm="True"
                                                                HeaderStyle-HorizontalAlign="Center" VisibleIndex="0" Width="10%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ResultadoProceso" Caption="Resultado de Proceso" ShowInCustomizationForm="True"
                                                                HeaderStyle-HorizontalAlign="Center" VisibleIndex="0" Width="10%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="TipoVenta" Caption="Causal Generica" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="1" Width="15%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ObservacionCallCenter" Caption="Observacion" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="2" Width="40%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="FechaGestion" Caption="Fecha Gestion" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="3" Width="17%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="UsuarioRegistra" Caption="Registrada por" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="4" Width="20%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                                        <SettingsPager PageSize="5">
                                                            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                                        </SettingsPager>
                                                        <SettingsText Title="Gestiones" EmptyDataRow="No se encontraron gestiones realizadas"
                                                            CommandEdit="Editar"></SettingsText>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <dx:ASPxGridView ID="gvMetas" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvMetas"
                                                        KeyFieldName="idCampania">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Campaña" FieldName="Campania" VisibleIndex="0" />
                                                            <dx:GridViewDataTextColumn Caption="Meta Configurada" FieldName="Meta" VisibleIndex="1" />
                                                        </Columns>
                                                        <SettingsPager PageSize="5"></SettingsPager>
                                                        <SettingsLoadingPanel Mode="ShowOnStatusBar" />
                                                        <SettingsLoadingPanel Mode="ShowOnStatusBar"></SettingsLoadingPanel>
                                                        <Styles>
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px" />
                                                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                                                        </Styles>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
        </div>
        <br />
        <%--Menú Flotante--%>
        <div id="bluebar" class="menuFlotante">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4"></b></b>
            <table>
                <tr>
                    <td>
                        <dx:ASPxComboBox ID="cbFormatoExportar" runat="server" ShowImageInEditBox="true"
                            SelectedIndex="-1" ValueType="System.String" EnableCallbackMode="true" AutoResizeWithContainer="true"
                            AutoPostBack="false"  ClientInstanceName="cbFormatoExportar"
                            Width="400px">
                            <Items>
                                <dx:ListEditItem ImageUrl="../img/xlsx_win.png" Text="Exportar a XLSX" Value="xlsx" />
                                <dx:ListEditItem ImageUrl="../img/excel.gif" Text="Exportar a XLS" Value="xls"
                                    Selected="true" />
                                <dx:ListEditItem ImageUrl="../img/pdf.png" Text="Exportar a PDF" Value="pdf" />
                                <dx:ListEditItem ImageUrl="../img/csv.png" Text="Exportar a CSV" Value="csv" />
                            </Items>
                            <Buttons>
                                <dx:EditButton Text="Exportar Información de Pantalla" ToolTip="Exportar Reporte al formato seleccionado">
                                    <Image Url="../img/upload.png">
                                    </Image>
                                </dx:EditButton>
                            </Buttons>
                            <ValidationSettings ErrorText="Formato a exportar requerido" RequiredField-ErrorText="Formato a exportar requerido"
                                Display="Dynamic" CausesValidation="true" ValidateOnLeave="true" ValidationGroup="exportar">
                                <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular"></RegularExpression>
                                <RequiredField IsRequired="True" ErrorText="Formato a exportar requerido"></RequiredField>
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnExportarDetalle" runat="server" Text="Exportar Detalle" ClientInstanceName="btnExportarDetalle">
                            <Image Url="../img/Excel.gif"></Image>
                        </dx:ASPxButton>
                    </td>
                    <td align="right">
                        <dx:ASPxImage ID="imgExpandir" runat="server" ClientInstanceName="imgExpandir" ToolTip="Expandir Todos Los Grupos"
                            ImageUrl="../img/expandir.png">
                            <ClientSideEvents Click="function (s, e){
                                pivotGrid.PerformCallback('expandir');
                            }" />
                        </dx:ASPxImage>
                    </td>
                    <td align="right">
                        <dx:ASPxImage ID="imgContraer" runat="server" ClientInstanceName="imgContraer" ToolTip="Contraer Todos Los Grupos"
                            ImageUrl="../img/contraer.png">
                            <ClientSideEvents Click="function (s, e){
                                pivotGrid.PerformCallback('contraer');
                            }" />
                        </dx:ASPxImage>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div1" style="float: right; margin-right: 5px; margin-bottom: 5px; margin-top: 5px; width: 2%; position: fixed; overflow: hidden; display: block; bottom: 0px">
            <table>
                <tr>
                    <td align="right">
                        <a style="color: Black; font-size: 15px; cursor: hand; cursor: pointer;" id="a1"
                            onclick="toggle('bluebar');">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/structure.png" ToolTip="Ocultar/Mostrar, Menú "
                                Width="16px" /></a>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
            Modal="True">
        </dx:ASPxLoadingPanel>
    </form>
</body>
</html>

