<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteResultadoBaseClienteInterno.aspx.vb" Inherits="NotusExpress.ReporteResultadoBaseClienteInterno" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Reporte de Resultado Base Clientes(Interno)::</title>
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

        function LimpiaFormulario() {
            if (confirm("¿Realmente desea limpiar los campos del formulario?")) {
                ASPxClientEdit.ClearEditorsInContainerById('formPrincipal');
            }
        }

        function ValidarFiltros(s, e) {
            if (cmbBase.GetValue() == null) {
                alert('Debe seleccionar la base de clientes para continuar con la consulta.');
            } else {
                EjecutarCallbackGeneral(s, e, 'filtrarDatos');
            }
        }

        function EjecutarCallbackGeneral(s, e, parametro, valor) {
            if (ASPxClientEdit.AreEditorsValid()) {
                loadingPanel.Show();
                cpGeneral.PerformCallback(parametro + ':' + valor);
            }
        }

        function toggle(control) {
            $("#" + control).slideToggle("slow");
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
        <div id="divEncabezado">
            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        </div>
        <div id="divGeneral" runat="server">
            <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" ClientInstanceName="cpGeneral" OnCallback="cpGeneral_Callback" >
                <ClientSideEvents EndCallback="function(s,e){
                    loadingPanel.Hide();
                    $('#divEncabezado').html(s.cpMensaje);
                 }" />
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtros de B&uacute;squeda" Width="50%">
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
                                                        <td class="field">Base de Datos:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cmbBase" runat="server" ClientInstanceName="cmbBase" IncrementalFilteringMode="Contains"
                                                                ValueType="System.Int32" TabIndex="4" Width="500px">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="padding-top: 8px">
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
                                        <dx:ASPxGridView ID="gvDatos" runat="server" ClientInstanceName="gvDatos" AutoGenerateColumns="false"
                                            KeyFieldName="IdCliente" Width="100%">
                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="Idcliente" Caption="IdCliente" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center" Visible="false"
                                                    VisibleIndex="0">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="nombreArchivo" Caption="Base de Datos" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="0">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="codigoEstrategia" Caption="Cod. Estrategia" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="1" Width="20px">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="nombreCampania" Caption="Campaña" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="2">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="tipoCampania" Caption="Tipo de Campaña" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="3">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ciudad" Caption="Ciudad" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="4">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="numeroIdentificacion" Caption="Cedula" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="5">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="tipoDocumento" Caption="Tipo Doc." ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="7" Width="10px">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="barrido" Caption="Ind. Barrido" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="8" Width="10px">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="estadoFinal" Caption="Estado Final" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="9">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="asesor" Caption="Asesor" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="10">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                            <SettingsPager PageSize="20">
                                                <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                            </SettingsPager>
                                            <SettingsText Title="Información" EmptyDataRow="No se encontraron datos de acuerdo a la base consultada."
                                                CommandEdit="Editar"></SettingsText>
                                        </dx:ASPxGridView>
                                        <dx:ASPxGridViewExporter ID="gveExportador" runat="server">
                                        </dx:ASPxGridViewExporter>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
        <br />
        <%--Menú Flotante--%>
        <div id="bluebar" class="menuFlotante">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4"></b></b>
            <table>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnExportar" runat="server" Text="Exportar Información" ClientInstanceName="btnExportarDetalle">
                            <Image Url="../img/Excel.gif"></Image>
                        </dx:ASPxButton>
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
