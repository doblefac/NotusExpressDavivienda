<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegistroDePresupuestoDiarioDeGestiones.aspx.vb"
    Inherits="NotusExpress.RegistroDePresupuestoDiarioDeGestiones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registro de Presupuesto Diario de Gestiones</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        String.prototype.trim = function () { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }

        function EjecutarCallbackGeneral(parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                cpGeneral.PerformCallback(parametro);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
    </div>
    <dx:ASPxCallbackPanel ID="cpGeneral" ClientInstanceName="cpGeneral" runat="server"
        >
        <ClientSideEvents BeginCallback="function(s,e){ loadingPnl.Show();}" EndCallback="function(s,e){ loadingPnl.Hide(); document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
        <PanelCollection>
            <dx:PanelContent>
                <vu:ValidacionURL ID="vuControlSesion" runat="server" />
                <table>
                    <tr>
                        <th colspan="4" class="thRojo">
                            <asp:Image ID="imgSearch" runat="server" ImageUrl="~/img/find.gif" />&nbsp;Filtros
                            de B&uacute;squeda
                        </th>
                    </tr>
                    <tr>
                        <td class="field">
                            Fecha
                        </td>
                        <td colspan="3">
                            <dx:ASPxDateEdit ID="deFecha" runat="server" ClientInstanceName="deFecha" AllowNull="False"
                                AllowUserInput="False" EnableClientSideAPI="true">
                                <ClientSideEvents ValueChanged="function(s,e) { cboCorte.PerformCallback('cargarLista'); }" />
                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy"></CalendarProperties>
                                <ValidationSettings SetFocusOnError="True">
                                    <RequiredField ErrorText="Fecha Requerida" IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Asesor Comercial
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboAsesor" runat="server" ValueType="System.Int32" Style="display: inline !important;"
                                AutoResizeWithContainer="True" Width="300" ClientInstanceName="cboAsesor" IncrementalFilteringMode="StartsWith"
                                EnableClientSideAPI="True">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { cboPdv.PerformCallback('cargarLista'); }" />
                                <ValidationSettings Display="Dynamic" SetFocusOnError="true" CausesValidation="false">
                                    <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td class="field">
                            Punto de Venta
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboPdv" runat="server" ValueType="System.Int32" EnableClientSideAPI="True"
                                Style="display: inline !important;" AutoResizeWithContainer="True" Width="300"
                                ClientInstanceName="cboPdv" IncrementalFilteringMode="StartsWith">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { cboSupervisor.PerformCallback('cargarLista'); }"
                                    EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                                <ValidationSettings Display="Dynamic" SetFocusOnError="true" CausesValidation="false">
                                    <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Supervisor
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboSupervisor" runat="server" ValueType="System.Int32" Style="display: inline !important;"
                                AutoResizeWithContainer="True" Width="300" ClientInstanceName="cboSupervisor"
                                EnableClientSideAPI="True">
                                <ClientSideEvents EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                                <ValidationSettings Display="Dynamic" SetFocusOnError="true">
                                    <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td class="field">
                            Corte
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboCorte" runat="server" ValueType="System.Int32" Style="display: inline !important;"
                                AutoResizeWithContainer="True" Width="300" ClientInstanceName="cboCorte">
                                <ClientSideEvents EndCallback="function(s, e) {document.getElementById('divEncabezado').innerHTML = s.cpMensaje;}" />
                                <ValidationSettings Display="Dynamic" SetFocusOnError="true">
                                    <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Gestiones en Punto
                        </td>
                        <td colspan="3">
                            <table>
                                <tr>
                                    <td class="field">
                                        Total Rechazadas
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTotRechazadasPdv" runat="server" Width="80px" HorizontalAlign="Right"
                                            MaxLength="4">
                                            <ClientSideEvents GotFocus="function(s, e) {s.SelectAll();}" />
                                            <ValidationSettings Display="Dynamic" ErrorText="Valor no válido">
                                                <RegularExpression ErrorText="Valor no válido" ValidationExpression="[0-9]+" />
                                                <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Total Express
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTotExpressPdv" runat="server" Width="80px" HorizontalAlign="Right"
                                            MaxLength="4">
                                            <ClientSideEvents GotFocus="function(s, e) {s.SelectAll();}" />
                                            <ValidationSettings Display="Dynamic" ErrorText="Valor no válido">
                                                <RegularExpression ErrorText="Valor no válido" ValidationExpression="[0-9]+" />
                                                <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Total Normales
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTotNormalesPdv" runat="server" Width="80px" HorizontalAlign="Right"
                                            MaxLength="4">
                                            <ClientSideEvents GotFocus="function(s, e) {s.SelectAll();}" />
                                            <ValidationSettings Display="Dynamic" ErrorText="Valor no válido">
                                                <RegularExpression ErrorText="Valor no válido" ValidationExpression="[0-9]+" />
                                                <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Gestiones Empresariales
                        </td>
                        <td colspan="3">
                            <table>
                                <tr>
                                    <td class="field">
                                        Total Rechazadas
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTotRechazadasEmp" runat="server" Width="80px" HorizontalAlign="Right"
                                            MaxLength="4">
                                            <ClientSideEvents GotFocus="function(s, e) {s.SelectAll();}" />
                                            <ValidationSettings Display="Dynamic" ErrorText="Valor no válido">
                                                <RegularExpression ErrorText="Valor no válido" ValidationExpression="[0-9]+" />
                                                <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Total Express
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTotExpressEmp" runat="server" Width="80px" HorizontalAlign="Right"
                                            MaxLength="4">
                                            <ClientSideEvents GotFocus="function(s, e) {s.SelectAll();}" />
                                            <ValidationSettings Display="Dynamic" ErrorText="Valor no válido">
                                                <RegularExpression ErrorText="Valor no válido" ValidationExpression="[0-9]+" />
                                                <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Total Normales
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTotNormalesEmp" runat="server" Width="80px" HorizontalAlign="Right"
                                            MaxLength="4">
                                            <ClientSideEvents GotFocus="function(s, e) {s.SelectAll();}" />
                                            <ValidationSettings Display="Dynamic" ErrorText="Valor no válido">
                                                <RegularExpression ErrorText="Valor no válido" ValidationExpression="[0-9]+" />
                                                <RequiredField ErrorText="Dato requerido" IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Novedad
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboNovedad" runat="server" ValueType="System.Int32" Style="display: inline !important;"
                                AutoResizeWithContainer="True" Width="300" ClientInstanceName="cboNovedad" IncrementalFilteringMode="StartsWith">
                            </dx:ASPxComboBox>
                        </td>
                        <td class="field">
                            Observaciones
                        </td>
                        <td>
                            <dx:ASPxMemo ID="txtObservacion" runat="server" Rows="4" Width="300px">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <dx:ASPxButton ID="btnRegistrar" runat="server" Text="Registrar Datos" AutoPostBack="false"
                                Style="display: inline !important;">
                                <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral('registrarDatos');}" />
                            </dx:ASPxButton>
                            &nbsp;&nbsp;&nbsp;
                            <dx:ASPxButton ID="btnCancelar" runat="server" Text="Cancelar" AutoPostBack="false"
                                Style="display: inline !important;">
                                <ClientSideEvents Click="function(s, e) {cpGeneral.PerformCallback('limpiarCampos');}" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
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
