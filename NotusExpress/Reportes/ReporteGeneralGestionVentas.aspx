<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteGeneralGestionVentas.aspx.vb"
    Inherits="NotusExpress.ReporteGeneralGestionVentas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Reporte de Referidos</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        String.prototype.trim = function () { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }

        function EjecutarCallbackGeneral(parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                cpResultadoReporte.PerformCallback(parametro);
            }
        }

        function EsRangoValido(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;

            if ((fechaInicio == null || fechaInicio == false) && (fechaFin != null && fechaFin != false)) { e.isValid = false; return; }
            if ((fechaInicio == null || fechaInicio == false) && (fechaFin != null && fechaFin != false)) { e.isValid = false; return; }

            if (fechaInicio > fechaFin) { e.isValid = false; return; }

            //            var diff = Math.floor((fechaFin.getTime() - fechaInicio.getTime()) / (1000 * 60 * 60 * 24));
            //            if (diff > 60) { e.isValid = false; }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divEncabezado">
            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        </div>
        <dx:ASPxCallbackPanel ID="cpResultadoReporte" ClientInstanceName="cpResultadoReporte"
            runat="server" >
            <ClientSideEvents BeginCallback="function(s,e){ loadingPnl.Show();}" EndCallback="function(s,e){ loadingPnl.Hide(); document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
            <PanelCollection>
                <dx:PanelContent>
                    <vu:ValidacionURL ID="vuControlSesion" runat="server" />
                    <table width="85%">
                        <tr>
                            <th colspan="4" class="thRojo">
                                <asp:Image ID="imgSearch" runat="server" ImageUrl="~/img/find.gif" />&nbsp;Filtros
                            de B&uacute;squeda
                            </th>
                        </tr>
                        <tr>
                            <td class="field">Punto de Venta
                            </td>
                            <td>
                                <div style="display: inline; float: left; width: 200px">
                                    <dx:ASPxComboBox ID="cboPdv" runat="server" ValueType="System.Int32" EnableClientSideAPI="True"
                                        AutoResizeWithContainer="false" Width="200px"
                                        ClientInstanceName="cboPdv">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { cboAsesor.PerformCallback('cargarLista'); }"
                                            EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                                    </dx:ASPxComboBox>
                                </div>
                                <div style="display: inline; float: left; width: 100px">
                                    <dx:ASPxCheckBox ID="cbPdvActivo" runat="server" Checked="True" CheckState="Checked"
                                        Text="Solo Activos">
                                        <ClientSideEvents CheckedChanged="function(s, e) { cboPdv.PerformCallback('cargarLista'); }" />
                                    </dx:ASPxCheckBox>
                                </div>
                            </td>
                            <td class="field">Asesor Comercial
                            </td>
                            <td>
                                <div style="display: inline; float: left; width: 200px">
                                    <dx:ASPxComboBox ID="cboAsesor" runat="server" ValueType="System.Int32"
                                        AutoResizeWithContainer="false" Width="200px" ClientInstanceName="cboAsesor">
                                        <ClientSideEvents EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                                    </dx:ASPxComboBox>
                                </div>
                                <div style="display: inline; float: left; width: 100px">
                                    <dx:ASPxCheckBox ID="cbAsesorActivo" runat="server" Checked="True" CheckState="Checked"
                                        Text="Solo Activos" Style="display: inline !important;">
                                        <ClientSideEvents CheckedChanged="function(s, e) { cboAsesor.PerformCallback('cargarLista'); }" />
                                    </dx:ASPxCheckBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="field">Resultado Gestión
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="cboResultadoGestion" runat="server" ValueType="System.Int32"
                                    AutoResizeWithContainer="true" Width="200px" ClientInstanceName="cboResultadoGestion">
                                </dx:ASPxComboBox>
                            </td>
                            <td class="field">Tipo de Producto
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="cboTipoProducto" runat="server" ValueType="System.Int32"
                                    AutoResizeWithContainer="True" Width="200px" ClientInstanceName="cboTipoProducto">
                                    <ClientSideEvents EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="field">Estrategia Comercial
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="cboEstrategiaComercial" runat="server" ValueType="System.Int32"
                                    AutoResizeWithContainer="True" Width="200px" ClientInstanceName="cboEstrategiaComercial">
                                    <ClientSideEvents EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                                </dx:ASPxComboBox>
                            </td>
                            <td class="field">Tipo de Cliente
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="cmbTipoCliente" runat="server" ValueType="System.Int32"
                                    AutoResizeWithContainer="True" Width="200px" ClientInstanceName="cmbTipoCliente">
                                    <ClientSideEvents EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                                    <Items>
                                        <dx:ListEditItem Text="Todos" Value="0" Selected="true" />
                                        <dx:ListEditItem Text="Clientes Potenciales" Value="1" />
                                        <dx:ListEditItem Text="Clientes con Gestiones" Value="2" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="field">Fecha de Gesti&oacute;n
                            </td>
                            <td style="vertical-align: middle !important" nowrap="nowrap" colspan="3">
                                <table style="padding: 0px !important; border: none !important">
                                    <tr>
                                        <td>De:&nbsp;&nbsp;
                                        </td>
                                        <td valign="middle">
                                            <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio">
                                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                </CalendarProperties>
                                                <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final"
                                                    ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                </ValidationSettings>
                                                <ClientSideEvents Validation="EsRangoValido" />
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>&nbsp;-&nbsp;
                                        </td>
                                        <td>Hasta:&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin">
                                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                </CalendarProperties>
                                                <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final"
                                                    ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                </ValidationSettings>
                                                <ClientSideEvents Validation="EsRangoValido" />
                                            </dx:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                                <dx:ASPxButton ID="btnConsultar" runat="server" Text="Consultar Datos" AutoPostBack="false"
                                    Style="display: inline !important;">
                                    <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral('obtenerReporte');}" />
                                </dx:ASPxButton>
                                &nbsp;&nbsp;&nbsp;
                            <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar" AutoPostBack="false"
                                Style="display: inline !important;">
                                <ClientSideEvents Click="function(s, e) {EjecutarCallbackGeneral('limpiarFiltros');}" />
                            </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <asp:Panel ID="pnlResultado" runat="server">
                        <table>
                            <tr>
                                <td>
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
                                            <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular"></RegularExpression>
                                            <RequiredField IsRequired="true" ErrorText="Formato a exportar requerido" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td>&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btnExpandir" runat="server" ToolTip="Expandir Todos Los Grupos"
                                        Style="vertical-align: middle;" Text="Expandir Todo" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s, e) { gvReporte.PerformCallback('expandir');}" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btnContraer" runat="server" ToolTip="Contraer Todos Los Grupos"
                                        Style="vertical-align: middle;" Text="Contraer Todo" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s, e) { gvReporte.PerformCallback('contraer');}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                        &nbsp;
                    <dx:ASPxGridView ID="gvReporte" ClientInstanceName="gvReporte" runat="server" Width="98%"
                        AutoGenerateColumns="False" Theme="RedWine" EnableRowsCache="False">
                        <Columns>
                            <dx:GridViewDataColumn FieldName="idGestionVenta" Caption="idGestionVenta" VisibleIndex="0" CellStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="numeroIdentificacion" Caption="C&eacute;dula" VisibleIndex="1" />
                            <dx:GridViewDataColumn FieldName="nombreApellido" Caption="Nombre del Cliente" VisibleIndex="2" />
                            <dx:GridViewDataColumn FieldName="CiudadCliente" Caption="Ciudad Cliente" VisibleIndex="3" />
                            <dx:GridViewDataColumn FieldName="ingresoAproximado" Caption="Ingreso Aproximado" VisibleIndex="4" />
                            <dx:GridViewDataColumn FieldName="NombreEmpresa" Caption="Nombre Empresa" VisibleIndex="5" />
                            <dx:GridViewDataColumn FieldName="email" Caption="Email" VisibleIndex="6" />
                            <dx:GridViewDataColumn FieldName="resultadoUbica" Caption="Resultado Ubica" VisibleIndex="7" />
                            <dx:GridViewDataColumn FieldName="numConsultaUbica" Caption="Num Consulta Ubica" VisibleIndex="8" />
                            <dx:GridViewDataColumn FieldName="resultadoEvidente" Caption="Resultado Evidente" VisibleIndex="9" />
                            <dx:GridViewDataColumn FieldName="numConsultaEvidente" Caption="Num Consulta Evidente" VisibleIndex="10" />
                            <dx:GridViewDataColumn FieldName="resultadoDataCredito" Caption="Resultado DataCredito" VisibleIndex="11" />
                            <dx:GridViewDataColumn FieldName="numConsultaDataCredito" Caption="Num Consulta DataCredito" VisibleIndex="12" />
                            <dx:GridViewDataColumn FieldName="nombreTipoDeProducto" Caption="Tipo de Producto" VisibleIndex="13" />
                            <dx:GridViewDataColumn FieldName="Producto" Caption="Producto" VisibleIndex="14" />
                            <dx:GridViewDataColumn FieldName="estatusLaboral" Caption="Estatus Laboral " VisibleIndex="15" />
                            <dx:GridViewDataColumn FieldName="telefonoFijo" Caption="Tel&eacute;fono Fijo" VisibleIndex="16" />
                            <dx:GridViewDataColumn FieldName="celular" Caption="Tel&eacute;fono Celular" VisibleIndex="17" />
                            <dx:GridViewDataColumn FieldName="direccion" Caption="Direcci&oacute;n" VisibleIndex="18" />
                            <dx:GridViewDataColumn FieldName="fechaGestion" Caption="Fecha de Gesti&oacute;n"
                                VisibleIndex="19">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="fechaRegistro" Caption="Fecha de Registro" VisibleIndex="20">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="numeroIdentificacionAsesor" Caption="C&eacute;dula Asesor"
                                VisibleIndex="21" />
                            <dx:GridViewDataColumn FieldName="nombreApellidoAsesor" Caption="Nombres y Apellidos Asesor"
                                VisibleIndex="22" />
                            <dx:GridViewDataColumn FieldName="puntoDeVenta" Caption="Punto De Venta" VisibleIndex="23" />
                            <dx:GridViewDataColumn FieldName="codigoPdv" Caption="C&oacute;digo PDV" VisibleIndex="24" />
                            <dx:GridViewDataColumn FieldName="codigoSucursal" Caption="C&oacute;digo Sucursal"
                                VisibleIndex="25" />
                            <dx:GridViewDataColumn FieldName="ciudad" Caption="Ciudad" VisibleIndex="26" />
                            <dx:GridViewDataColumn FieldName="resultadoVenta" Caption="Resultado Proceso" VisibleIndex="27" />
                            
                            <dx:GridViewDataTextColumn FieldName="cupoPreaprobadoDataCredito" Caption="Cupo" VisibleIndex="28">
                                <PropertiesTextEdit DisplayFormatString="c" NullDisplayText="">
                                    <ValidationSettings ErrorText="Valor inv&#225;lido">
                                        <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular"></RegularExpression>
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="serial" Caption="No. de Tarjeta" VisibleIndex="29">
                                <PropertiesTextEdit NullDisplayText="">
                                    <ValidationSettings ErrorText="Valor inv&#225;lido">
                                        <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular"></RegularExpression>
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataColumn FieldName="identificacionOperadorCall" Caption="C&eacute;dula Asesor Call"
                                VisibleIndex="30" />
                            <dx:GridViewDataColumn FieldName="atendidoEnCallCenterPor" Caption="Nombre Asesor Call"
                                VisibleIndex="31" />
                            <dx:GridViewDataColumn FieldName="observacionCallCenter" Caption="Observaciones"
                                VisibleIndex="32" />
                             <dx:GridViewDataColumn FieldName="EstadoAnimo" Caption="Estado Animo"
                                VisibleIndex="33" />

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
                            <dx:ASPxSummaryItem FieldName="puntoDeVenta" SummaryType="Count" Tag="Num. Ventas Por Pdv" />
                        </GroupSummary>
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="idGestionVenta" SummaryType="Count" Tag="Total Ventas" />
                        </TotalSummary>
                    </dx:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="gveExportador" runat="server" GridViewID="gvReporte">
                        </dx:ASPxGridViewExporter>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
            <LoadingPanelStyle CssClass="modalBackground">
            </LoadingPanelStyle>
        </dx:ASPxCallbackPanel>
        <dx:ASPxLoadingPanel ID="loadingPnl" runat="server" ClientInstanceName="loadingPnl"
            Modal="True" Theme="DevEx">
        </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
