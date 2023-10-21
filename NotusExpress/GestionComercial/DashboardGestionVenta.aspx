<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DashboardGestionVenta.aspx.vb" Inherits="NotusExpress.Dashboard_Gestion_de_Venta" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pool de Liquidacion</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--    <script type="text/javascript">
        window.setTimeout(function () { document.forms(0).submit(); }, 10000);
    </script>--%>
    <meta http-equiv="refresh" content="10" />
    <script lang="javascript" type="text/javascript">
        function toggle(control) {
            $("#" + control).slideToggle("slow");
        }

        function ActualizarEncabezado(s, e) {

            if (loadingPanel) { loadingPanel.Hide(); }
            if (s.cpMensaje) {
                if (document.getElementById('divEncabezado')) {
                    document.getElementById('divEncabezado').innerHTML = s.cpMensaje;

                }
            }
        }

        function EjecutarCallbackGeneral(s, e, parametro, valor) {
            loadingPanel.Show();
            cpGeneral.PerformCallback(parametro + ':' + valor);
        }

        function TamanioVentana() {
            if (typeof (window.innerWidth) == 'number') {
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }
        }

        function Actualizar() {
            TamanioVentana();
            dialogoVer.SetSize(myWidth * 0.8, myHeight * 0.8);
            dialogoVer.ShowWindow();
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
        <div id="divEncabezado">
            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        </div>

        <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" ClientInstanceName="cpGeneral" EnableAnimation="true" 
            >
            <ClientSideEvents EndCallback="function (s, e){
                ActualizarEncabezado(s,e); 
            }" />
            <PanelCollection>
                <dx:PanelContent>

                    <dx:ASPxComboBox ID="cmbCampania" runat="server" ValueType="System.Int32" AutoPostBack="true"
                        ClientInstanceName="cmbCampania" TabIndex="1" Width="74%" ValueField="IdCampania">
                        <Columns>
                            <dx:ListBoxColumn FieldName="Nombre" Width="300px" Caption="Descripción" />
                        </Columns>
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroCliente">
                            <RequiredField ErrorText="Información requerida" IsRequired="true" />
                        </ValidationSettings>
                    </dx:ASPxComboBox>

                    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server">
                        <Items>
                            <dx:LayoutGroup Caption="Gestion de Ventas" ColCount="2">
                                <Items>
                                    <dx:LayoutItem Caption="Layout Item" ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dxchartsui:WebChartControl ID="wcSupervisores" runat="server" Height="320px" SideBySideEqualBarWidth="True" Width="520px"
                                                     ShowLoadingPanelImage="False" EnableCallBacks="false" SelectionMode="Multiple" SeriesSelectionMode="Point">
                                                    <borderoptions visible="False" />
                                                    <diagramserializable>
                                                        <dxcharts:SimpleDiagram EqualPieSize="False">
                                                        </dxcharts:SimpleDiagram>
                                                    </diagramserializable>
                                                    <fillstyle>
                                                            <optionsserializable>
                                                                <dxcharts:SolidFillOptions />
                                                            </optionsserializable>
                                                        </fillstyle>
                                                    <seriesserializable>
                                                        <dxcharts:Series ArgumentScaleType="Qualitative" Name="Supervisor">
                                                            <viewserializable>
                                                                <dxcharts:PieSeriesView RuntimeExploding="False">
                                                                </dxcharts:PieSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <dxcharts:PieSeriesLabel LineVisible="True">
                                                                    <fillstyle>
                                                                        <optionsserializable>
                                                                            <dxcharts:SolidFillOptions />
                                                                        </optionsserializable>
                                                                    </fillstyle>
                                                                    <pointoptionsserializable>
                                                                        <dxcharts:PiePointOptions>
                                                                            <valuenumericoptions format="Percent" />
                                                                        </dxcharts:PiePointOptions>
                                                                    </pointoptionsserializable>
                                                                </dxcharts:PieSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <dxcharts:PiePointOptions>
                                                                    <valuenumericoptions format="Percent" />
                                                                </dxcharts:PiePointOptions>
                                                            </legendpointoptionsserializable>
                                                        </dxcharts:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                            <viewserializable>
                                                                <dxcharts:PieSeriesView RuntimeExploding="False">
                                                                </dxcharts:PieSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <dxcharts:PieSeriesLabel LineVisible="True">
                                                                    <fillstyle>
                                                                        <optionsserializable>
                                                                            <dxcharts:SolidFillOptions />
                                                                        </optionsserializable>
                                                                    </fillstyle>
                                                                    <pointoptionsserializable>
                                                                        <dxcharts:PiePointOptions>
                                                                            <valuenumericoptions format="Percent" />
                                                                        </dxcharts:PiePointOptions>
                                                                    </pointoptionsserializable>
                                                                </dxcharts:PieSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <dxcharts:PiePointOptions>
                                                                    <valuenumericoptions format="Percent" />
                                                                </dxcharts:PiePointOptions>
                                                            </legendpointoptionsserializable>
                                                        </seriestemplate>
                                                    <clientsideevents
                                                        objectselected="function (s, e){
                                                                      Actualizar();
                                                                      }" />
                                                    <crosshairoptions>
                                                            <commonlabelpositionserializable>
                                                                <dxcharts:CrosshairMousePosition />
                                                            </commonlabelpositionserializable>
                                                        </crosshairoptions>
                                                    <tooltipoptions>
                                                            <tooltippositionserializable>
                                                                <dxcharts:ToolTipMousePosition />
                                                            </tooltippositionserializable>
                                                        </tooltipoptions>
                                                </dxchartsui:WebChartControl>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Layout Item" ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxGridView ID="gvAsesorVentas" runat="server" AutoGenerateColumns="False" Width="500px" KeyFieldName="Asesor" EnableCallBacks="False">
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Nombre Asesor" ShowInCustomizationForm="True" VisibleIndex="0" FieldName="Asesor">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Campaña" ShowInCustomizationForm="True" VisibleIndex="1" FieldName="Campana">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Cantidad Actual" ShowInCustomizationForm="True" VisibleIndex="2" FieldName="Ventas" Width="10px">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="idEstado" ShowInCustomizationForm="true" Caption="Estado" VisibleIndex="5" Width="100px">
                                                            <DataItemTemplate>
                                                                <asp:Image ID="imgNovedad" ImageUrl="~/img/BallGreen.png" runat="server" OnInit="Link_Init_imagen" /><br />
                                                                <%--  <dx:ASPxLabel ID="lblEstado" runat="server" Text=" " OnInit="Link_Init_label"
                                                                    CssClass="comentario" Font-Size="XX-Small" Font-Bold="False" Font-Italic="True"
                                                                    Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                                                                </dx:ASPxLabel>--%>
                                                            </DataItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>

                                                </dx:ASPxGridView>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Layout Item" ColSpan="2" ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dxchartsui:WebChartControl ID="wcVentasHora" runat="server" Height="240px" SideBySideEqualBarWidth="True" Width="1000px">
                                                    <borderoptions visible="False" />
                                                    <diagramserializable>
                                                        <dxcharts:XYDiagram>
                                                            <axisx visibleinpanesserializable="-1">
                                                                <range sidemarginsenabled="True" />
                                                            </axisx>
                                                            <axisy visibleinpanesserializable="-1">
                                                                <range sidemarginsenabled="True" />
                                                            </axisy>
                                                        </dxcharts:XYDiagram>
                                                    </diagramserializable>
                                                    <fillstyle>
                                                        <optionsserializable>
                                                            <dxcharts:SolidFillOptions />
                                                        </optionsserializable>
                                                    </fillstyle>
                                                    <seriesserializable>
                                                        <dxcharts:Series ArgumentScaleType="Qualitative" Name="Ventas por Hora">
                                                            <viewserializable>
                                                                <dxcharts:LineSeriesView>
                                                                </dxcharts:LineSeriesView>
                                                            </viewserializable>
                                                            <labelserializable>
                                                                <dxcharts:PointSeriesLabel LineVisible="True">
                                                                    <fillstyle>
                                                                        <optionsserializable>
                                                                            <dxcharts:SolidFillOptions />
                                                                        </optionsserializable>
                                                                    </fillstyle>
                                                                    <pointoptionsserializable>
                                                                        <dxcharts:PointOptions>
                                                                        </dxcharts:PointOptions>
                                                                    </pointoptionsserializable>
                                                                </dxcharts:PointSeriesLabel>
                                                            </labelserializable>
                                                            <legendpointoptionsserializable>
                                                                <dxcharts:PointOptions>
                                                                </dxcharts:PointOptions>
                                                            </legendpointoptionsserializable>
                                                        </dxcharts:Series>
                                                    </seriesserializable>
                                                    <seriestemplate>
                                                        <viewserializable>
                                                            <dxcharts:LineSeriesView>
                                                            </dxcharts:LineSeriesView>
                                                        </viewserializable>
                                                        <labelserializable>
                                                            <dxcharts:PointSeriesLabel LineVisible="True">
                                                                <fillstyle>
                                                                    <optionsserializable>
                                                                        <dxcharts:SolidFillOptions />
                                                                    </optionsserializable>
                                                                </fillstyle>
                                                                <pointoptionsserializable>
                                                                    <dxcharts:PointOptions>
                                                                    </dxcharts:PointOptions>
                                                                </pointoptionsserializable>
                                                            </dxcharts:PointSeriesLabel>
                                                        </labelserializable>
                                                        <legendpointoptionsserializable>
                                                            <dxcharts:PointOptions>
                                                            </dxcharts:PointOptions>
                                                        </legendpointoptionsserializable>
                                                    </seriestemplate>
                                                    <crosshairoptions>
                                                        <commonlabelpositionserializable>
                                                            <dxcharts:CrosshairMousePosition />
                                                        </commonlabelpositionserializable>
                                                    </crosshairoptions>
                                                    <tooltipoptions>
                                                        <tooltippositionserializable>
                                                            <dxcharts:ToolTipMousePosition />
                                                        </tooltippositionserializable>
                                                    </tooltipoptions>
                                                </dxchartsui:WebChartControl>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                        </Items>
                    </dx:ASPxFormLayout>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
        <br />
        <dx:ASPxPopupControl ID="pcVer" runat="server" ClientInstanceName="dialogoVer"
            Modal="True" HeaderText="Detalle de la Informacion"
            PopupVerticalAlign="WindowCenter" ScrollBars="Auto" AllowDragging="True" CloseAction="CloseButton" Font-Bold="True" ForeColor="Gray" PopupHorizontalAlign="WindowCenter" Width="400px" Style="margin-right: 0px">
            <HeaderImage Url="~/img/Truck.png">
            </HeaderImage>
            <HeaderStyle VerticalAlign="Middle" />
            <ModalBackgroundStyle CssClass="modalBackground" />
            <ContentCollection>
                <dx:PopupControlContentControl>

                    <dxchartsui:WebChartControl ID="wcAsesor" runat="server" Height="350px" EnableCallBacks="false" SideBySideEqualBarWidth="True" Width="980px">
                        <borderoptions visible="False" />
                        <diagramserializable>
                            <dxcharts:XYDiagram>
                                <axisx visibleinpanesserializable="-1">
                                    <range sidemarginsenabled="True" />
                                </axisx>
                                <axisy visibleinpanesserializable="-1">
                                    <range sidemarginsenabled="True" />
                                </axisy>
                            </dxcharts:XYDiagram>
                        </diagramserializable>
                        <fillstyle>
                            <optionsserializable>
                                <dxcharts:SolidFillOptions />
                            </optionsserializable>
                        </fillstyle>
                        
                        <seriesserializable>
                            <dxcharts:Series Name="Asesor" ArgumentScaleType="Qualitative">
                                <viewserializable>
                                    <dxcharts:SideBySideBarSeriesView>
                                    </dxcharts:SideBySideBarSeriesView>
                                </viewserializable>
                                <labelserializable>
                                    <dxcharts:SideBySideBarSeriesLabel LineVisible="True">
                                        <fillstyle>
                                            <optionsserializable>
                                                <dxcharts:SolidFillOptions />
                                            </optionsserializable>
                                        </fillstyle>
                                        <pointoptionsserializable>
                                            <dxcharts:PointOptions>
                                            </dxcharts:PointOptions>
                                        </pointoptionsserializable>
                                    </dxcharts:SideBySideBarSeriesLabel>
                                </labelserializable>
                                <legendpointoptionsserializable>
                                    <dxcharts:PointOptions>
                                    </dxcharts:PointOptions>
                                </legendpointoptionsserializable>
                            </dxcharts:Series>
                        </seriesserializable>
                        <seriestemplate>
                            <viewserializable>
                                <dxcharts:SideBySideBarSeriesView>
                                </dxcharts:SideBySideBarSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <dxcharts:SideBySideBarSeriesLabel LineVisible="True">
                                    <fillstyle>
                                        <optionsserializable>
                                            <dxcharts:SolidFillOptions />
                                        </optionsserializable>
                                    </fillstyle>
                                    <pointoptionsserializable>
                                        <dxcharts:PointOptions>
                                        </dxcharts:PointOptions>
                                    </pointoptionsserializable>
                                </dxcharts:SideBySideBarSeriesLabel>
                            </labelserializable>
                            <legendpointoptionsserializable>
                                <dxcharts:PointOptions>
                                </dxcharts:PointOptions>
                            </legendpointoptionsserializable>
                        </seriestemplate>
                        <crosshairoptions>
                            <commonlabelpositionserializable>
                                <dxcharts:CrosshairMousePosition />
                            </commonlabelpositionserializable>
                        </crosshairoptions>
                        <tooltipoptions>
                            <tooltippositionserializable>
                                <dxcharts:ToolTipMousePosition />
                            </tooltippositionserializable>
                        </tooltipoptions>
                    </dxchartsui:WebChartControl>
                    <br />
                    <%--<dx:ASPxButton ID="btn" runat="server">
                    </dx:ASPxButton>--%>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <br />
        <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
            Modal="True">
        </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
