<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteDeCumplimientoDeMetasPorAsesor.aspx.vb" Inherits="NotusExpress.ReporteDeCumplimientoDeMetasPorAsesor" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reporte de Cumplimiento de Metas Por Asesor</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script type="text/javascript">
    // <![CDATA[

        function EjecutarPivotCallback(parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                pgReporteCifras.PerformCallback(parametro);
            }
        }

        function EjecutarCallbackGeneral(parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                loadingPanel.Show();
                cpGeneral.PerformCallback(parametro);
            }
        }

        function CambiarVistaReporte(vistaActual, vistaNueva, valor) {
            if (vistaActual == "vpc") { hfCanal.Set("filtroCanal", valor); }
            hfInfoVista.Set("vistaAnterior", vistaActual);
            var itemVista = cbReporte.FindItemByValue(vistaNueva);
            cbReporte.SetSelectedItem(itemVista);
            EjecutarCallbackGeneral('cambioReporte');
        }

        function MostrarVistaAnterior() {
            var vistaActual = cbReporte.GetValue();
            var vistaNueva;
            switch (vistaActual) {
                case "vpe":
                    vistaNueva = "vpc";
                    break;
                case "vpci":
                    vistaNueva = "vpe";
                    break;
                case "vpa":
                    vistaNueva = "vpe";
                    break;
            }

            var itemVista = cbReporte.FindItemByValue(vistaNueva);
            cbReporte.SetSelectedItem(itemVista);
            EjecutarCallbackGeneral('cambioReporte');
        }


    // ]]> 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        <br />
    </div>
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" ClientInstanceName="cpGeneral"
        >
        <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e);}" />
        <PanelCollection>
            <dx:PanelContent ID="pnlContenidoGeneral" runat="server">
                <uc2:ValidacionURL ID="vuControlSesion" runat="server" />
                <dx:ASPxHiddenField ID="hfCanal" ClientInstanceName="hfCanal" runat="server"></dx:ASPxHiddenField>
                <dx:ASPxHiddenField ID="hfInfoVista" ClientInstanceName="hfInfoVista" runat="server"></dx:ASPxHiddenField>
                <br />
                <table>
                    <tr>
                        <th class="thRojo" colspan="2">
                            B&uacute;squeda por Fecha de Venta
                        </th>
                    </tr>
                    <tr>
                        <td class="field">
                            Fecha de Corte:
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="deFecha" runat="server" ClientInstanceName="deFecha">
                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                </CalendarProperties>
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" Display="Dynamic"
                                    ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Fecha Requerida" />
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btnConsultar" runat="server" Text="Consultar" AutoPostBack="false">
                                            <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral('consultar');}" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar" AutoPostBack="false">
                                            <ClientSideEvents Click="function(s, e) {EjecutarCallbackGeneral('limpiar');}" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <strong>
                                            <label>
                                                Vista</label></strong>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbReporte" ClientInstanceName="cbReporte" runat="server" SelectedIndex="0" ValueType="System.String"
                                            EnableClientSideAPI="True">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {hfInfoVista.Set('vistaAnterior', cbReporte.GetValue()); EjecutarCallbackGeneral('cambioReporte'); }" />
                                            <Items>
                                                <dx:ListEditItem Value="vpc" Text="Cifras Por Canal" />
                                                <dx:ListEditItem Value="vpe" Text="Cifras Por Estrategia" />
                                                <dx:ListEditItem Value="vpci" Text="Cifras Por Ciudad" />
                                                <dx:ListEditItem Value="vpa" Text="Cifras Por Asesor" />
                                                <dx:ListEditItem Value="vg" Text="General" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btnExpandir" runat="server" ToolTip="Expandir Todos Los Grupos"
                                            Style="vertical-align: middle;" Text="Expandir Todo" AutoPostBack="false">
                                            <ClientSideEvents Click="function(s, e) { pgReporteCifras.PerformCallback('expandir');}" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btnContraer" runat="server" ToolTip="Contraer Todos Los Grupos"
                                            Style="vertical-align: middle;" Text="Contraer Todo" AutoPostBack="false">
                                            <ClientSideEvents Click="function(s, e) { pgReporteCifras.PerformCallback('contraer');}" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlOpcExportar" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <strong>Exportar a:</strong>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="listExportFormat" runat="server" Style="vertical-align: middle"
                                                            SelectedIndex="0" ValueType="System.String" Width="140px">
                                                            <Items>
                                                                <dx:ListEditItem Text="Excel 97-2003 (.xls)" Value="1" />
                                                                <dx:ListEditItem Text="Excel (.xlsx)" Value="2" />
                                                            </Items>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="btnGuardarComo" runat="server" ToolTip="Exportar y Guardar" Style="vertical-align: middle;"
                                                            Text="Guardar" Width="60px" />
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="btnAbrir" runat="server" ToolTip="Exportar y Abrir" Style="vertical-align: middle;"
                                                            Text="Abrir" Width="60px" />
                                                    </td>
                                                    <td style="width: 20px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="btnGenerarGrafico" runat="server" ToolTip="Generar Gráfico" AutoPostBack="false"
                                                            Style="vertical-align: middle">
                                                            <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral('mostrarGrafico');}" />
                                                            <Image Url="~/img/reports.png">
                                                            </Image>
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="btnGenerarGrafico2" runat="server" ToolTip="Generar Gráfico" AutoPostBack="false"
                                                            Style="vertical-align: middle">
                                                            <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral('mostrarGrafico2');}" />
                                                            <Image Url="~/img/reports.png">
                                                            </Image>
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td style="width: 20px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="btnAtras" runat="server" ToolTip="Atrás" AutoPostBack="false"
                                                            Style="vertical-align: middle">
                                                            <ClientSideEvents Click="function(s, e) { MostrarVistaAnterior();}" />
                                                            <Image Url="~/img/back.png"></Image>
                                                        </dx:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </table>
                    <div id="divTitulo">
                <dx:ASPxPanel ID="pnlTitulo" runat="server">
                    <PanelCollection>
                        <dx:PanelContent>
                            <table width="300px">
                                <tr>
                                    <td class="field" align="center">
                                        <dx:ASPxLabel ID="lblTitulo" runat="server" Text="CANAL {0} - {1}">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                    </dx:ASPxPanel>
                    </div>
                    <table>
                    <tr>
                        <td>
                            <dx:ASPxPivotGrid ID="pgReporteCifras" ClientInstanceName="pgReporteCifras" runat="server"
                                EnableTheming="True" Theme="RedWine" Width="100%">
                                <Fields>
                                    <dx:PivotGridField ID="fieldCanal" Area="RowArea" Caption="Canal" FieldName="Canal"
                                        AreaIndex="0" AllowedAreas="RowArea, ColumnArea, FilterArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldEstrategia" Area="RowArea" AreaIndex="1" Caption="Estrategia"
                                        FieldName="Estrategia" AllowedAreas="RowArea, ColumnArea, FilterArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldCiudad" Area="RowArea" AreaIndex="2" Caption="Ciudad"
                                        FieldName="Ciudad" AllowedAreas="RowArea, ColumnArea, FilterArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldAsesor" Area="RowArea" AreaIndex="3" Caption="Asesor"
                                        FieldName="Asesor" AllowedAreas="RowArea, ColumnArea, FilterArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldTipoProducto" Area="RowArea" AreaIndex="4" Caption="Producto"
                                        FieldName="TipoProducto" AllowedAreas="RowArea, ColumnArea, FilterArea">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField Area="DataArea" AreaIndex="0" FieldName="MetaAnio" ID="fieldMetaAnio"
                                        Caption="Meta Año" AllowedAreas="DataArea" TotalCellFormat-FormatString="n0"
                                        TotalCellFormat-FormatType="Numeric" GrandTotalCellFormat-FormatString="n0" GrandTotalCellFormat-FormatType="Numeric"
                                        TotalValueFormat-FormatString="n0" TotalValueFormat-FormatType="Numeric" ValueFormat-FormatString="n0"
                                        ValueFormat-FormatType="Numeric" CellStyle-VerticalAlign="Middle" ValueTotalStyle-HorizontalAlign="Center"
                                        CellFormat-FormatType="Numeric" CellFormat-FormatString="n0" GroupIndex="0" InnerGroupIndex="0">
                                        <CellStyle VerticalAlign="Middle">
                                        </CellStyle>
                                        <ValueTotalStyle HorizontalAlign="Center">
                                        </ValueTotalStyle>
                                    </dx:PivotGridField>
                                    <dx:PivotGridField Area="DataArea" AreaIndex="1" FieldName="AcumuladoAnio" ID="fieldAcumuladoAnio"
                                        Caption="Cumplimiento Año" AllowedAreas="DataArea" TotalCellFormat-FormatString="n0"
                                        TotalCellFormat-FormatType="Numeric" GrandTotalCellFormat-FormatString="n0" GrandTotalCellFormat-FormatType="Numeric"
                                        TotalValueFormat-FormatString="n0" TotalValueFormat-FormatType="Numeric" ValueFormat-FormatString="n0"
                                        ValueFormat-FormatType="Numeric" CellStyle-VerticalAlign="Middle" ValueTotalStyle-HorizontalAlign="Center"
                                        GroupIndex="0" InnerGroupIndex="1">
                                        <CellStyle VerticalAlign="Middle">
                                        </CellStyle>
                                        <ValueTotalStyle HorizontalAlign="Center">
                                        </ValueTotalStyle>
                                    </dx:PivotGridField>
                                    <dx:PivotGridField Area="DataArea" AreaIndex="2" FieldName="PromedioMes" ID="fieldPromedioMes"
                                        Caption="Promedio Mes" AllowedAreas="DataArea" TotalCellFormat-FormatString="n0"
                                        TotalCellFormat-FormatType="Numeric" GrandTotalCellFormat-FormatString="n0" GrandTotalCellFormat-FormatType="Numeric"
                                        TotalValueFormat-FormatString="n0" TotalValueFormat-FormatType="Numeric" ValueFormat-FormatString="n0"
                                        ValueFormat-FormatType="Numeric" CellStyle-VerticalAlign="Middle" ValueTotalStyle-HorizontalAlign="Center"
                                        CellFormat-FormatType="Numeric" CellFormat-FormatString="n0" GroupIndex="0" InnerGroupIndex="2">
                                        <CellStyle VerticalAlign="Middle">
                                        </CellStyle>
                                        <ValueTotalStyle HorizontalAlign="Center">
                                        </ValueTotalStyle>
                                    </dx:PivotGridField>
                                    <dx:PivotGridField Area="DataArea" AreaIndex="3" FieldName="MetaMes" ID="fieldMetaMes"
                                        Caption="Meta Mes" AllowedAreas="DataArea" TotalCellFormat-FormatString="n0"
                                        TotalCellFormat-FormatType="Numeric" GrandTotalCellFormat-FormatString="n0" GrandTotalCellFormat-FormatType="Numeric"
                                        TotalValueFormat-FormatString="n0" TotalValueFormat-FormatType="Numeric" ValueFormat-FormatString="n0"
                                        ValueFormat-FormatType="Numeric" CellStyle-VerticalAlign="Middle" ValueTotalStyle-HorizontalAlign="Center"
                                        CellFormat-FormatType="Numeric" CellFormat-FormatString="n0" GroupIndex="0" InnerGroupIndex="3">
                                        <CellStyle VerticalAlign="Middle">
                                        </CellStyle>
                                        <ValueTotalStyle HorizontalAlign="Center">
                                        </ValueTotalStyle>
                                    </dx:PivotGridField>
                                    <dx:PivotGridField Area="DataArea" AreaIndex="4" FieldName="AcumuladoMes" ID="fieldAcumuladoMes"
                                        Caption="Acumulado Mes" AllowedAreas="DataArea" TotalCellFormat-FormatString="n0"
                                        TotalCellFormat-FormatType="Numeric" GrandTotalCellFormat-FormatString="n0" GrandTotalCellFormat-FormatType="Numeric"
                                        TotalValueFormat-FormatString="n0" TotalValueFormat-FormatType="Numeric" ValueFormat-FormatString="n0"
                                        ValueFormat-FormatType="Numeric" CellStyle-VerticalAlign="Middle" ValueTotalStyle-HorizontalAlign="Center"
                                        CellFormat-FormatType="Numeric" CellFormat-FormatString="n0" GroupIndex="0" InnerGroupIndex="4">
                                        <CellStyle VerticalAlign="Middle">
                                        </CellStyle>
                                        <ValueTotalStyle HorizontalAlign="Center">
                                        </ValueTotalStyle>
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldEjecucionPresupuestal" Area="DataArea" AreaIndex="5"
                                        Caption="Ejecución Presupuestal" SummaryType="Custom" CellFormat-FormatString="p0"
                                        CellFormat-FormatType="Numeric" FieldName="AcumuladoMes">
                                        <CellStyle VerticalAlign="Middle">
                                        </CellStyle>
                                        <ValueTotalStyle HorizontalAlign="Center">
                                        </ValueTotalStyle>
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldFechaGestionAnio" Area="ColumnArea" AllowedAreas="ColumnArea"
                                        AreaIndex="0" Caption="Año" FieldName="Fecha" GroupInterval="DateYear" UnboundFieldName="fieldFechaGestionAnio">
                                    </dx:PivotGridField>
                                </Fields>
                                <OptionsView ShowColumnGrandTotals="False" DataHeadersDisplayMode="Popup" />
                                <OptionsPager RowsPerPage="20">
                                </OptionsPager>
                                <OptionsLoadingPanel Text="Cargando&amp;hellip;">
                                </OptionsLoadingPanel>
                                <OptionsFilter NativeCheckBoxes="False" />
                                <OptionsChartDataSource DataProvideMode="UseCustomSettings" ProvideColumnGrandTotals="false"
                                    ProvideRowGrandTotals="false" ProvideDataByColumns="false" />
                                <Prefilter Enabled="False" />
                                <Groups>
                                    <dx:PivotGridWebGroup ShowNewValues="True" />
                                </Groups>
                            </dx:ASPxPivotGrid>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="pucGrafico" runat="server" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" Modal="true" HeaderText="Gráfico" ClientInstanceName="pucGrafico"
                    Width="850px" Height="300px" ScrollBars="Auto" ShowMaximizeButton="True" ShowPageScrollbarWhenModal="True"
                    CloseAction="CloseButton">
                    <ModalBackgroundStyle CssClass="modalBackground" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <dxchartsui:WebChartControl ID="wcGrafico" runat="server" DataSourceID="pgReporteCifras"
                                Width="800px" Height="250px" SeriesDataMember="Series">
                                <legend maxhorizontalpercentage="30" />
                                <borderoptions visible="False" />
                                <borderoptions visible="False"></borderoptions>
                                <diagramserializable>
                        <dxcharts:XYDiagram>
                            <axisx visibleinpanesserializable="-1" 
                                title-text="Canal Estrategia Ciudad Asesor Producto">
                                <label staggered="True" />
                                <range sidemarginsenabled="True" />
<Label Staggered="True"></Label>

<Range SideMarginsEnabled="True"></Range>
                            </axisx>
                            <axisy visibleinpanesserializable="-1" 
                                title-text="Meta Año Cumplimiento Año Promedio Mes Meta Mes Acumulado Mes Ejecución Presupuestal">
                                <range sidemarginsenabled="True" />
<Range SideMarginsEnabled="True"></Range>
                            </axisy>
                        </dxcharts:XYDiagram>
                    </diagramserializable>
                                <fillstyle>
                        <OptionsSerializable>
                            <dxcharts:SolidFillOptions />
                        </OptionsSerializable>
                    </fillstyle>
                                <legend AlignmentHorizontal="Center" AlignmentVertical="BottomOutside" 
                                    MaxVerticalPercentage="30"></legend>
                                <seriestemplate argumentdatamember="Arguments" valuedatamembersserializable="Values"
                                    argumentscaletype="Qualitative">
                        <ViewSerializable>
                            <dxcharts:SideBySideBarSeriesView />
                        </ViewSerializable>
                        <LabelSerializable>
                            <dxcharts:SideBySideBarSeriesLabel LineVisible="True">
                                <FillStyle>
                                    <OptionsSerializable>
                                        <dxcharts:SolidFillOptions />
                                    </OptionsSerializable>
                                </FillStyle>
                                <PointOptionsSerializable>
                                    <dxcharts:PointOptions />
                                </PointOptionsSerializable>
                            </dxcharts:SideBySideBarSeriesLabel>
                        </LabelSerializable>
                        <LegendPointOptionsSerializable>
                            <dxcharts:PointOptions />
                        </LegendPointOptionsSerializable>
                    </seriestemplate>
                                <crosshairoptions><CommonLabelPositionSerializable>
<dxcharts:CrosshairMousePosition></dxcharts:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</crosshairoptions>
                                <tooltipoptions><ToolTipPositionSerializable>
<dxcharts:ToolTipMousePosition></dxcharts:ToolTipMousePosition>
</ToolTipPositionSerializable>
</tooltipoptions>
                            </dxchartsui:WebChartControl>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pucGrafico2" runat="server" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" Modal="true" HeaderText="Gráfico" ClientInstanceName="pucGrafico"
                    Width="750px" Height="350px" ScrollBars="Auto" ShowMaximizeButton="True" ShowPageScrollbarWhenModal="True"
                    CloseAction="CloseButton">
                    <ModalBackgroundStyle CssClass="modalBackground" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <dxchartsui:WebChartControl ID="wcGrafico2" runat="server" Height="300px" Width="700px"
                                ClientInstanceName="chart">
                                <seriesserializable>
<dxcharts:Series Name="Ventas" SummaryFunction="SUM([AcumuladoMes])">
<ViewSerializable>
<dxcharts:SideBySideBarSeriesView></dxcharts:SideBySideBarSeriesView>
</ViewSerializable>
<LabelSerializable>
<dxcharts:SideBySideBarSeriesLabel LineVisible="True">
<FillStyle><OptionsSerializable>
<dxcharts:SolidFillOptions></dxcharts:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
<PointOptionsSerializable>
<dxcharts:PointOptions>
<ValueNumericOptions Format="FixedPoint"></ValueNumericOptions>
</dxcharts:PointOptions>
</PointOptionsSerializable>
</dxcharts:SideBySideBarSeriesLabel>
</LabelSerializable>
<LegendPointOptionsSerializable>
<dxcharts:PointOptions>
<ValueNumericOptions Format="FixedPoint"></ValueNumericOptions>
</dxcharts:PointOptions>
</LegendPointOptionsSerializable>
</dxcharts:Series>
<dxcharts:Series Name="Meta" SummaryFunction="SUM([MetaMes])">
<ViewSerializable>
<dxcharts:LineSeriesView MarkerVisibility="True">
<LineMarkerOptions Size="20"></LineMarkerOptions>
</dxcharts:LineSeriesView>
</ViewSerializable>
<LabelSerializable>
<dxcharts:PointSeriesLabel Angle="70" LineLength="30" LineVisible="True">
<FillStyle><OptionsSerializable>
<dxcharts:SolidFillOptions></dxcharts:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
<PointOptionsSerializable>
<dxcharts:PointOptions>
<ValueNumericOptions Format="Number"></ValueNumericOptions>
</dxcharts:PointOptions>
</PointOptionsSerializable>
</dxcharts:PointSeriesLabel>
</LabelSerializable>
<LegendPointOptionsSerializable>
<dxcharts:PointOptions>
<ValueNumericOptions Format="Number"></ValueNumericOptions>
</dxcharts:PointOptions>
</LegendPointOptionsSerializable>
</dxcharts:Series>
</seriesserializable>
                                <seriestemplate><ViewSerializable>
<dxcharts:SideBySideBarSeriesView></dxcharts:SideBySideBarSeriesView>
</ViewSerializable>
<LabelSerializable>
<dxcharts:SideBySideBarSeriesLabel LineVisible="True">
<FillStyle><OptionsSerializable>
<dxcharts:SolidFillOptions></dxcharts:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
<PointOptionsSerializable>
<dxcharts:PointOptions></dxcharts:PointOptions>
</PointOptionsSerializable>
</dxcharts:SideBySideBarSeriesLabel>
</LabelSerializable>
<LegendPointOptionsSerializable>
<dxcharts:PointOptions></dxcharts:PointOptions>
</LegendPointOptionsSerializable>
</seriestemplate>
                                <fillstyle><OptionsSerializable>
<dxcharts:SolidFillOptions></dxcharts:SolidFillOptions>
</OptionsSerializable>
</fillstyle>
                                <borderoptions visible="False" />
                                <titles>
<dxcharts:ChartTitle Text=""></dxcharts:ChartTitle>
<dxcharts:ChartTitle Dock="Bottom" Alignment="Far" Text="Por Logytech Mobile SAS" Font="Tahoma, 8pt" TextColor="Gray"></dxcharts:ChartTitle>
</titles>
<BorderOptions Visible="False"></BorderOptions>
                                <diagramserializable>
<dxcharts:XYDiagram>
<AxisX Title-Text="Years" VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
<GridLines Visible="True"></GridLines>
</AxisX>
<AxisY Title-Text="Número de Ventas" Title-Visible="True" VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>
<GridLines MinorVisible="True"></GridLines>
</AxisY>
</dxcharts:XYDiagram>
</diagramserializable>

<CrosshairOptions><CommonLabelPositionSerializable>
<dxcharts:CrosshairMousePosition></dxcharts:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>

<ToolTipOptions><ToolTipPositionSerializable>
<dxcharts:ToolTipMousePosition></dxcharts:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
                            </dxchartsui:WebChartControl>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:ASPxPivotGridExporter ID="pgeExportador" runat="server" ASPxPivotGridID="pgReporteCifras"
        Visible="False">
    </dx:ASPxPivotGridExporter>
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    <dx:ASPxTimer ID="tmrActualizador" runat="server" Interval="600000" ClientInstanceName="tmrActualizador" Enabled="false">
        <ClientSideEvents Tick="function(s, e) { EjecutarCallbackGeneral('consultar');}" />
    </dx:ASPxTimer>
    </form>
</body>
</html>
