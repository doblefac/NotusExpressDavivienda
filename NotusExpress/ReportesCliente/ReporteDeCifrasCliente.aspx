<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="ReporteDeCifrasCliente.aspx.vb"
    Inherits="NotusExpress.ReporteDeCifrasCliente" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporte de Cifras - Cliente</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script type="text/javascript">
    // <![CDATA[

        function EjecutarPivotCallback(parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                pgReporteCifras.PerformCallback(parametro);
            }
        }

        function EjecutarCallbackGeneral(parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                cpGeneral.PerformCallback(parametro);
            }
        }

        function EnValidacionDeRango(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;

            if (fechaInicio == null || fechaInicio == false || fechaFin == null || fechaFin == false) { return; }

            if (fechaInicio > fechaFin) { e.isValid = false; }

            var diff = Math.floor((fechaFin.getTime() - fechaInicio.getTime()) / (1000 * 60 * 60 * 24));

            if (diff > 60) { e.isValid = false; }
        }
    // ]]> 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" ClientInstanceName="cpGeneral"
        EnableAnimation="True" >
        <PanelCollection>
            <dx:PanelContent ID="pnlContenidoGeneral" runat="server">
                <uc2:ValidacionURL ID="vuControlSesion" runat="server" />
                <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
                <asp:HiddenField ID="hfInfoProcesoBoton" runat="server" />
                <br />
                <table>
                    <tr>
                        <th class="thRojo" colspan="5">
                            B&uacute;squeda por Fecha de Venta
                        </th>
                    </tr>
                    <tr>
                        <td class="field">
                            Fecha Inicio:
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio">
                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                </CalendarProperties>
                                <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final. Rango menor que 60 d&iacute;as"
                                    ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Fecha de Inicio Requerida" />
                                </ValidationSettings>
                                <ClientSideEvents Validation="EnValidacionDeRango" />
                            </dx:ASPxDateEdit>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="field">
                            Fecha Fin
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin">
                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                </CalendarProperties>
                                <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final. Rango menor que 60 d&iacute;as"
                                    ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Fecha Final Requerida" />
                                </ValidationSettings>
                                <ClientSideEvents Validation="EnValidacionDeRango" />
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                                            <ClientSideEvents Click="function(s, e) { cpGeneral.PerformCallback('limpiar');}" />
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
                                                Reporte</label></strong>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbReporte" runat="server" SelectedIndex="0" ValueType="System.String"
                                            EnableClientSideAPI="True">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { EjecutarPivotCallback('cambioReporte'); }" />
                                            <Items>
                                                <dx:ListEditItem Value="rpc" Text="Registros Por Ciudad" />
                                                <dx:ListEditItem Value="rpp" Text="Registros Por Punto" />
                                                <dx:ListEditItem Value="rprp" Text="Registros Por Resultado Proceso" />
                                                <dx:ListEditItem Value="rpa" Text="Registros Por Asesor" />
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
                                                            SelectedIndex="0" ValueType="System.String" Width="61px">
                                                            <Items>
                                                                <dx:ListEditItem Text="Pdf" Value="0" />
                                                                <dx:ListEditItem Text="Excel" Value="1" />
                                                                <dx:ListEditItem Text="Mht" Value="2" />
                                                                <dx:ListEditItem Text="Rtf" Value="3" />
                                                                <dx:ListEditItem Text="Text" Value="4" />
                                                                <dx:ListEditItem Text="Html" Value="5" />
                                                            </Items>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="btnGuardarComo" runat="server" ToolTip="Exportar y Guardar" Style="vertical-align: middle;"
                                                            Text="Guardar" Width="60px" />
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="btnAbrir" runat="server" ToolTip="Exportar y Abrir" Style="vertical-align: middle"
                                                            Text="Abrir" Width="60px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxPivotGrid ID="pgReporteCifras" ClientInstanceName="pgReporteCifras" runat="server"
                                EnableTheming="True" Theme="RedWine" Width="100%">
                                <Fields>
                                    <dx:PivotGridField ID="fieldciudad" Area="RowArea" Caption="Ciudad" FieldName="ciudad"
                                        AreaIndex="0">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldpuntoDeVenta" Area="RowArea" AreaIndex="1" Caption="Punto de Venta"
                                        FieldName="puntoDeVenta">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldfechaGestionAnio" Area="ColumnArea" AreaIndex="0" Caption="Año"
                                        FieldName="fechaGestion" GroupInterval="DateYear" UnboundFieldName="fieldfechaGestionAnio">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldfechaGestionMes" Area="ColumnArea" AreaIndex="1" Caption="Mes"
                                        FieldName="fechaGestion" GroupInterval="DateMonth" UnboundFieldName="fieldfechaGestionMes">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField Area="DataArea" AreaIndex="0" FieldName="numVentas" ID="fieldnumVEntas"
                                        Caption="Numero de Gestiones" AllowedAreas="DataArea" TotalCellFormat-FormatString="n0"
                                        TotalCellFormat-FormatType="Numeric" GrandTotalCellFormat-FormatString="n0" GrandTotalCellFormat-FormatType="Numeric"
                                        TotalValueFormat-FormatString="n0" TotalValueFormat-FormatType="Numeric" ValueFormat-FormatString="n0"
                                        ValueFormat-FormatType="Numeric" CellStyle-VerticalAlign="Middle" ValueTotalStyle-HorizontalAlign="Center">
                                        <CellStyle VerticalAlign="Middle">
                                        </CellStyle>
                                        <ValueTotalStyle HorizontalAlign="Center">
                                        </ValueTotalStyle>
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldnombreAsesor" AreaIndex="0" Caption="Asesor Comercial"
                                        FieldName="nombreAsesor">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldresultadoGestion" AreaIndex="2" Caption="Resultado Gesti&oacute;n"
                                        FieldName="resultadoGestion">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldtipoVenta" AreaIndex="1" Caption="Tipo de Producto" FieldName="tipoVenta">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldnombreProducto" AreaIndex="3" Caption="Producto" FieldName="nombreProducto">
                                    </dx:PivotGridField>
                                    <dx:PivotGridField ID="fieldCupo" AreaIndex="4" Caption="Cupo de Cr&eacute;dito"
                                        FieldName="cupo" ValueFormat-FormatType="Custom" ValueFormat-FormatString="c0">
                                    </dx:PivotGridField>
                                </Fields>
                                <OptionsPager RowsPerPage="20">
                                </OptionsPager>
                                <OptionsLoadingPanel Text="Cargando&amp;hellip;">
                                </OptionsLoadingPanel>
                                <OptionsFilter NativeCheckBoxes="False" />
                                <Prefilter Enabled="False" />
                                <StylesPrint Cell-BackColor2="" Cell-GradientMode="Horizontal" FieldHeader-BackColor2=""
                                    FieldHeader-GradientMode="Horizontal" TotalCell-BackColor2="" TotalCell-GradientMode="Horizontal"
                                    GrandTotalCell-BackColor2="" GrandTotalCell-GradientMode="Horizontal" CustomTotalCell-BackColor2=""
                                    CustomTotalCell-GradientMode="Horizontal" FieldValue-BackColor2="" FieldValue-GradientMode="Horizontal"
                                    FieldValueTotal-BackColor2="" FieldValueTotal-GradientMode="Horizontal" FieldValueGrandTotal-BackColor2=""
                                    FieldValueGrandTotal-GradientMode="Horizontal" Lines-BackColor2="" Lines-GradientMode="Horizontal">
                                </StylesPrint>
                            </dx:ASPxPivotGrid>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:ASPxPivotGridExporter ID="pgeExportador" runat="server" ASPxPivotGridID="pgReporteCifras"
        Visible="False">
    </dx:ASPxPivotGridExporter>
    <dx:ASPxLoadingPanel ID="lpWait" runat="server" ClientInstanceName="lpWait" Modal="true"
        Text="Cargando&amp;hellip;">
    </dx:ASPxLoadingPanel>
    <asp:SqlDataSource ID="sdsDatos" runat="server" ConnectionString="Server=COS9;DataBase=VentaExpressData;User Id=AppNotusExpress;Password=7_Gmrzku"
        SelectCommand="ReporteDeCifrasCliente" SelectCommandType="StoredProcedure" DataSourceMode="DataReader">
    </asp:SqlDataSource>
    </form>
</body>
</html>
