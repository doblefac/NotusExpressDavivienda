<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministradorUsuariosPdv.aspx.vb"
    Inherits="NotusExpress.AdministradorUsuariosPdv" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script type="text/javascript">
    // <![CDATA[
        function EjecutarCallbackGeneral(s, e, parametro) {

            if (ASPxClientEdit.AreEditorsValid()) {

                loadingPanel.Show();

                cpGeneral.PerformCallback(parametro);

            }

        }

        function OnPassValidation(s, e) {
            var errorText = GetErrorText(s);
            if (errorText) {
                e.isValid = false;
                e.errorText = errorText;
            }
        }
        function AddSelectedItems() {
            MoveSelectedItems(lbPuntodeVenta, lbPuntodeVentaUsuario);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(lbPuntodeVenta, lbPuntodeVentaUsuario);
            UpdateButtonState();
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbPuntodeVentaUsuario, lbPuntodeVenta);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(lbPuntodeVentaUsuario, lbPuntodeVenta);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            var items = srcListBox.GetSelectedItems();
            for (var i = items.length - 1; i >= 0; i = i - 1) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }
        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }
        function UpdateButtonState() {
            btnMoveAllItemsToRight.SetEnabled(lbPuntodeVenta.GetItemCount() > 0);
            btnMoveAllItemsToLeft.SetEnabled(lbPuntodeVentaUsuario.GetItemCount() > 0);
            btnMoveSelectedItemsToRight.SetEnabled(lbPuntodeVenta.GetSelectedItems().length > 0);
            btnMoveSelectedItemsToLeft.SetEnabled(lbPuntodeVentaUsuario.GetSelectedItems().length > 0);
        }

        function ActualizarEncabezado(s, e) {
            if (loadingPanel) { loadingPanel.Hide(); }
            if (s.cpMensaje) {
                if (document.getElementById('divEncabezado')) {
                    document.getElementById('divEncabezado').innerHTML = s.cpMensaje;
                }
            }
            if (s.cpMensajePopUp && mensajePopUp) {
                if (s.cpTituloPopUp) { mensajePopUp.SetHeaderText(s.cpTituloPopUp); }
                if (document.getElementById(textoMensajePopUp.name)) {
                    document.getElementById(textoMensajePopUp.name).innerHTML = s.cpMensajePopUp;
                    mensajePopUp.Show();
                    s.cpMensajePopUp = null;
                    s.cpTituloPopUp = null;
                }
            }
            if (s.cpLimpiarFiltros) { LimpiarFiltros(); }
        }
        function CambioRol(CbRol) {
            CbPerfil.PerformCallback(CbRol.GetValue());

        }

        function CambioCiudad(cbEmpresa) {
            cbUnidadNegocio.PerformCallback(cbEmpresa.GetValue());
        }

        function CambiocpGeneral(s, e, parametro) {
            cbEmpresa.PerformCallback(parametro);

        }
        function SeleccionaPDV(s, e) {
            cbCargo.PerformCallback();

        }
        function OnASPxComboBoxValidation(s, e) {
            if (e.value == -1)
                e.isValid = false;
        }
        function ValidarselecciondePDV(s, e, parametro) {
            var arr = s.GetItemCount();
            e.isValid = arr >= 1;
        }
        function mostarPDV(s, e) {
            //            alert(s.cpRequierePdv);
            if (s.cpRequierePdv == 1) {

                pnlpdv.SetVisible(true);
            } else {
                pnlpdv.SetVisible(false);
            }
        }
        function RequiereFechaCrea(s, e) {
            if (s.cpRequiereFechaCrea == 1) {

                deFechaIngreso.SetEnabled(true);
            } else {
                deFechaIngreso.SetEnabled(false);
            }
        }
        function EsRangoValido(s, e) {

            var fechaInicio = deFechaIngreso.date;
            var estado = deFechaIngreso.enabled;

            if (estado == true) {
                if (fechaInicio == null) {
                    e.isValid = false;
                    return;
                } else {
                    e.isValid = true;
                    return;
                }
            } else {
                e.isValid = true;
                return;
            }
        }
       
        
    // ]]> 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- <vu:ValidacionURL ID="vuControlSesion" runat="server" />--%>
    <asp:HiddenField ID="hfIdUsuario" runat="server" />
    <asp:HiddenField ID="hfIdPersona" runat="server" />
    <vu:ValidacionURL ID="vuControlSesion" runat="server" />
    <dx:ASPxHiddenField ID="hfDimensiones" ClientInstanceName="hfDimensiones" runat="server">
    </dx:ASPxHiddenField>
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        <br />
    </div>
    <div>
        <dx:ASPxRoundPanel ID="rpCierreCiclo" runat="server" HeaderText="&nbsp;" Width="70%">
            <PanelCollection>
                <dx:PanelContent>
                     <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
                        <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e)}" />
                        <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e)}"></ClientSideEvents>
                        <PanelCollection>
                            <dx:PanelContent>
                    <dx:ASPxFormLayout ID="Cplayout" runat="server" RequiredMarkDisplayMode="Auto" Styles-LayoutGroupBox-Caption-CssClass="layoutGroupBoxCaption"
                        AlignItemCaptionsInAllGroups="true">
                        <Items>
                            <dx:LayoutGroup ColCount="2" GroupBoxDecoration="None" Caption="&nbsp;" SettingsItemCaptions-HorizontalAlign="Right">
                                <Items>
                                    <dx:LayoutItem Caption="Numero Identificación">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxLabel ID="TextNumeroIdentificacion" runat="server" Width="170px">
                                                </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Nombre y Apellido">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxLabel ID="TextNombre" runat="server" Width="170px" >
                                                </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="E-mail" RequiredMarkDisplayMode="Hidden">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxLabel runat="server" ID="TextEmail" Width="170">
                                                </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Usuario">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxLabel ID="TextUsuario" runat="server" Width="170px">
                                               </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Telefono" RequiredMarkDisplayMode="Hidden">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxLabel ID="TextTelefono" ClientInstanceName="TextTelefono" runat="server" Width="170px">
                                              
                                                </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Ciudad">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="cbCiudad" runat="server" AutoPostBack="False" Enabled="False" SelectedIndex="0"
                                                    ValueType="System.Int32" ClientInstanceName="cbCiudad" IncrementalFilteringMode="StartsWith">
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation"></ClientSideEvents>
                                                    <ValidationSettings EnableCustomValidation="true" ValidationGroup="Guardar" SetFocusOnError="True"
                                                        ErrorText="Seleccione una ciudad">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione una ciudad"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation" />
                                                </dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Rol">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="CbRol" runat="server"  Enabled="False" NullText="Seleccione un Rol..." ClientInstanceName="CbRol"
                                                    DropDownStyle="DropDown" ValueType="System.Int32">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { CambioRol(s); SeleccionaPDV(s,e); }"
                                                        Validation="OnASPxComboBoxValidation"></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" ErrorText="Seleccione un Rol" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Rol"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation" />
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { CambioRol(s); SeleccionaPDV(s,e); }" />
                                                </dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Perfil">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="CbPerfil" runat="server"  Enabled="False" TextField="Nombre" ValueField="IdPerfil"
                                                    NullText="Seleccione un Perfil..." ClientInstanceName="CbPerfil" DropDownStyle="DropDown"
                                                    ValueType="System.Int32">
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Perfil"></RequiredField>
                                                    </ValidationSettings>
                                                    <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { SeleccionaPDV(s,e);}" />--%>
                                                </dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Tipo">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="cbTipo" runat="server" InitialValue="-1" NullText="Seleccione un Tipo de persona..."
                                                    ClientInstanceName="cbTipo" DropDownStyle="DropDown" ValueType="System.Int32"
                                                    Enabled="False">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { CambiocpGeneral(s,e,&#39;CambioTipopersona&#39;);}"
                                                        Validation="OnASPxComboBoxValidation"></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" ErrorText="Seleccione un Tipo de usuario"
                                                        SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Tipo de usuario"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation" />
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { CambiocpGeneral(s,e,'CambioTipopersona');}" />
                                                </dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Cargo" RequiredMarkDisplayMode="Hidden">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="cbCargo" runat="server" NullText="Seleccione un cargo..."  Enabled="False" ClientInstanceName="cbCargo"
                                                    DropDownStyle="DropDown" ValueType="System.Int32">
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Cargo"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents EndCallback="function(s, e) { mostarPDV(s, e); RequiereFechaCrea(s, e);}"
                                                        Validation="function(s, e) { mostarPDV(s, e); }"></ClientSideEvents>
                                                </dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="&nbsp;" ColSpan="2" RequiredMarkDisplayMode="Hidden">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer ID="liccPdv" Visible="True">
                                                <dx:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                                                    <ClientSideEvents ControlsInitialized="function(s, e) { UpdateButtonState(); }">
                                                    </ClientSideEvents>
                                                </dx:ASPxGlobalEvents>
                                                <dx:ASPxCallbackPanel ID="pnlpdv" runat="server" Width="100%" ClientInstanceName="pnlpdv">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <table style="width: 90%">
                                                                <tr>
                                                                    <td style="width: 45%;">
                                                                        <div>
                                                                            <dx:ASPxLabel ID="lblAvailable" runat="server" Text="Puntos de Venta:" />
                                                                        </div>
                                                                        <dx:ASPxListBox ID="lbPuntodeVenta" runat="server" ClientInstanceName="lbPuntodeVenta"
                                                                            Width="340px" Height="190px" SelectionMode="CheckColumn" TextField="nombrePdv"
                                                                            ValueField="IdPdv">
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }">
                                                                            </ClientSideEvents>
                                                                        </dx:ASPxListBox>
                                                                    </td>
                                                                    <td align="center">
                                                                        <div>
                                                                            <dx:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight"
                                                                                AutoPostBack="False" Text="Add &gt;" Width="130px" Height="23px" ClientEnabled="False"
                                                                                ToolTip="Add selected items" Style="margin-bottom: 3px">
                                                                                <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }"></ClientSideEvents>
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                        <div>
                                                                            <dx:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight"
                                                                                AutoPostBack="False" Text="Add All &gt;&gt;" Width="130px" Height="23px" ToolTip="Add all items">
                                                                                <ClientSideEvents Click="function(s, e) { AddAllItems(); }"></ClientSideEvents>
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                        <div style="height: 32px">
                                                                        </div>
                                                                        <div>
                                                                            <dx:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft"
                                                                                AutoPostBack="False" Text="&lt; Remove" Width="130px" Height="23px" ClientEnabled="False"
                                                                                ToolTip="Remove selected items" Style="margin-bottom: 3px">
                                                                                <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }"></ClientSideEvents>
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                        <div>
                                                                            <dx:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft"
                                                                                AutoPostBack="False" Text="&lt;&lt; Remove All" Width="130px" Height="23px" ClientEnabled="False"
                                                                                ToolTip="Remove all items">
                                                                                <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }"></ClientSideEvents>
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 45%">
                                                                        <div>
                                                                            <dx:ASPxLabel ID="lblChosen" runat="server" Text="Punto de Venta del Usuario:" />
                                                                        </div>
                                                                        <dx:ASPxListBox ID="lbPuntodeVentaUsuario" runat="server" ClientInstanceName="lbPuntodeVentaUsuario"
                                                                            Width="340px" Height="190px" SelectionMode="CheckColumn" TextField="nombrePdv"
                                                                            ValueField="IdPdv">
                                                                            <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True" ErrorText="Seleccione los puntos de venta">
                                                                                <%-- <RequiredField IsRequired="True" ErrorText="Seleccione los puntos de venta"></RequiredField>--%>
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents Validation="function(s, e) { ValidarselecciondePDV(s, e,'1'); }" />
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }">
                                                                            </ClientSideEvents>
                                                                        </dx:ASPxListBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxCallbackPanel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="&nbsp;" HorizontalAlign="Center" ColSpan="2" HelpTextSettings-Position="Top"
                                        RequiredMarkDisplayMode="Hidden">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <table cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="btnActualizarUsuario" runat="server" Text="Actualizar PDV Usuario"
                                                                Style="display: inline!important;" AutoPostBack="False" ValidationGroup="Guardar">
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td class="field">
                                                            &nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Cancelar" Style="display: inline!important;"
                                                                ClientInstanceName="btnLimpiar" AutoPostBack="False" UseSubmitBehavior="False">
                                                                <%--<Image Url="~/images/edit-clear.png">
                                                                </Image>--%>
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                        <HelpTextSettings Position="Top"></HelpTextSettings>
                                    </dx:LayoutItem>
                                </Items>
                                <SettingsItemCaptions HorizontalAlign="Right"></SettingsItemCaptions>
                            </dx:LayoutGroup>
                        </Items>
                        <Styles>
                            <LayoutGroupBox>
                                <Caption CssClass="layoutGroupBoxCaption">
                                </Caption>
                            </LayoutGroupBox>
                        </Styles>
                    </dx:ASPxFormLayout>
                     </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </div>
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
