<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteGestionVentasCallCenter.aspx.vb" Inherits="NotusExpress.ReproteGestionVentasCallCenter" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Reporte Solicitud de Realce Cliente::</title>
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

        function ValidarFiltros(s, e)
        {
            if (deFechaInicio.GetDate() != null || deFechaFin.GetDate() != null)
            {
                if (deFechaInicio.GetDate() == null || deFechaFin.GetDate() == null)
                {
                    alert('Debe seleccionar los dos rangos de fecha.', 'rojo');
                }
                else {
                    loadingPanel.Show();
                    EjecutarCallbackGeneral(s, e, 'filtrarDatos');
                }
            }
            else
            {
                loadingPanel.Show();
                EjecutarCallbackGeneral(s, e, 'filtrarDatos');
            }
        }

        function ValidacionDeFecha(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;
            if (fechaInicio == null || fechaInicio == false || fechaFin == null || fechaFin == false) { return; }
            if (fechaInicio > fechaFin) { e.isValid = false; }
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
                        <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtros de B&uacute;squeda" Width="75%">
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
                                                                ValueType="System.Int32" TabIndex="0" Width="300px">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                        <td class="field">Campaña:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cmbCampania" runat="server" ClientInstanceName="cmbCampania" IncrementalFilteringMode="Contains"
                                                                ValueType="System.Int32" TabIndex="1" Width="300px">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="field">Fecha Gestión Inicial:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio"
                                                                TabIndex="5" Width="300px">
                                                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                                </CalendarProperties>
                                                                <ClientSideEvents Validation="ValidacionDeFecha"></ClientSideEvents>
                                                                <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inválido. Fecha inicial menor que Fecha final."
                                                                    ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                                </ValidationSettings>
                                                                <ClientSideEvents Validation="ValidacionDeFecha" />
                                                            </dx:ASPxDateEdit>
                                                        </td>
                                                        <td class="field">Fecha Gestión Final:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin" TabIndex="6"
                                                                Width="300px">
                                                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                                </CalendarProperties>
                                                                <ClientSideEvents Validation="ValidacionDeFecha"></ClientSideEvents>
                                                                <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inválido. Fecha inicial menor que Fecha final."
                                                                    ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                                </ValidationSettings>
                                                                <ClientSideEvents Validation="ValidacionDeFecha" />
                                                            </dx:ASPxDateEdit>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="field">Estado Reporte:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cmbEstadoNotus" runat="server" ClientInstanceName="cmbEstadoNotus"
                                                                TabIndex="6" Width="300px" IncrementalFilteringMode="Contains">
                                                            </dx:ASPxComboBox>
                                                            <%--           <dx:ASPxComboBox ID="cmbReportado" runat="server" ClientInstanceName="cmbReportado" 
                                                                ValueType="System.Int32" TabIndex="0" Width="300px">
                                                                <Items>
                                                                    <dx:ListEditItem Text="No Reportado" Value="0" />
                                                                    <dx:ListEditItem Text="Reportado" Value="1" />
                                                                </Items>
                                                            </dx:ASPxComboBox>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right">
                                                            <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline!important;"
                                                                AutoPostBack="false" ValidationGroup="Filtrado" TabIndex="7" ClientInstanceName="btnBuscar" HorizontalAlign="Justify">
                                                                <ClientSideEvents Click="function(s, e) { 
                                                                    ValidarFiltros(s, e); 
                                                                }"></ClientSideEvents>
                                                                <Image Url="~/img/find.gif">
                                                                </Image>
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td colspan="2">
                                                            <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar"
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
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="btnExportar" runat="server" Text="Exportar" Image-Url="~/img/Excel.gif">
                                                        <Image Url="~/img/Excel.gif"></Image>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <br />
                                                </td>
                                                <%--        <td>
                                                    <dx:ASPxButton ID="btnExportarReportar" runat="server" Text="Reportar y Exportar" Image-Url="~/img/Excel.gif">
                                                        <Image Url="~/img/Attach.gif"></Image>
                                                    </dx:ASPxButton>
                                                </td>--%>
                                            </tr>
                                        </table>
                                        <br />
                                        <dx:ASPxGridView ID="gvDatos" runat="server" ClientInstanceName="gvDatos" AutoGenerateColumns="false"
                                            KeyFieldName="id" Width="100%">
                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="canal" Caption="Canal" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center" Visible="true"
                                                    VisibleIndex="0">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="proceso_producto" Caption="Proceso/Producto" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="1">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="FechaGestion" Caption="Fecha Gestión" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="2">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="fechaAgendamiento" Caption="Fecha Agendamiento" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="3">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="tipoDocumento" Caption="Tipo documento persona" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="4">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="numeroDocumento" Caption="Número documento persona" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="5">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="nombreApellido" Caption="Cliente" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="6">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="nombreEstado" Caption="Estado Venta" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="7">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                            <SettingsPager PageSize="20">
                                                <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                            </SettingsPager>
                                            <SettingsText Title="Información" EmptyDataRow="No se encontraron datos de acuerdo a la filtros consultados."
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
        <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
            Modal="True">
        </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
