<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EdiciondeUsuario.aspx.vb"
    Inherits="NotusExpress.EdiciondeUsuario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <title></title>
    <style type="text/css">
        div#tablecontainer
        {
            width: 1032px;
            border-top: 0px solid black;
        }

        div.tablecontainerrow
        {
            clear: both;
            overflow: hidden;
            border: 0px solid black;
            border-top: none;
        }

        div#tablecontainer div div.column1
        {
            width: 150px;
            float: left;
        }

        div#tablecontainer div div.column2
        {
            width: 200px;
            float: left;
        }

        div#tablecontainer div div.column3
        {
            width: 150px;
            float: left;
        }

        div#tablecontainer div div.column4
        {
            width: 200px;
            float: left;
        }
    </style>
    <script language="jscript" type="text/javascript">
        // <![CDATA[


        function SeleccionaPDV(s, key) {
            cpRegistro.PerformCallback(key);

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

        function ValidaRequiereCargoUNEmpresa(s, e, parametro) {
            cpRegistro.PerformCallback(parametro);
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
            if (hdMostarPDV.Get('MostarPDV') == "1") {
                pnlpdv.SetVisible(true);
            }
            else {
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
        function ValidaEstado(s, e) {
            var estadoActual = hfIdEstadoUsuario.Get('IdEstadoUsuario');
            var NuevoEstado = cbEstado.GetValue();
            if (estadoActual == 1 && NuevoEstado == 0) {
                deFechaRetiro.SetVisible(true);
            }
            else {
                deFechaRetiro.SetVisible(false);
            }
        }
        function isNumberKey(s, e) {
            var tel = TextTelefono.GetValue();
            if (tel != null) {
                if (tel.length == 7 || tel.length == 10) {
                    e.isValid = true;
                    return true;
                } else {
                    e.isValid = false;
                    return false;
                }
            }
        }

        // ]]> 
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <vu:ValidacionURL ID="vuControlSesion" runat="server" />
        <div id="divEncabezado">
            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
            <br />
        </div>
        <div>
            <dx:ASPxCallbackPanel ID="cpRegistro" runat="server"  ClientInstanceName="cpRegistro">
                <ClientSideEvents Init="function(s, e) { mostarPDV(s,e); }" EndCallback="function(s, e) {mostarPDV(s,e); ActualizarEncabezado(s,e);}" />
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxRoundPanel ID="rpPrincipal" runat="server" HeaderText="&nbsp;" Width="80%">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <asp:HiddenField ID="hfIdUsuario" runat="server" />
                                    <asp:HiddenField ID="hfIdPersona" runat="server" />
                                    <dx:ASPxHiddenField ID="hfIdEstadoUsuario" runat="server" ClientInstanceName="hfIdEstadoUsuario"></dx:ASPxHiddenField>
                                    <dx:ASPxHiddenField ID="hfDimensiones" ClientInstanceName="hfDimensiones" runat="server">
                                    </dx:ASPxHiddenField>
                                    <dx:ASPxHiddenField ID="hdMostarPDV" runat="server" ClientInstanceName="hdMostarPDV"></dx:ASPxHiddenField>
                                    <div id="tablecontainer">
                                        <div class="tablecontainerrow">
                                            <div class="column1">Numero Identificación: </div>
                                            <div class="column2">
                                                <dx:ASPxTextBox ID="TextNumeroIdentificacion" runat="server" Width="170px" MaxLength="15"
                                                    NullText="Ingrese el número de identificación">
                                                    <ValidationSettings ErrorTextPosition="Bottom" ErrorDisplayMode="Text" Display="Dynamic"
                                                        SetFocusOnError="true" ValidationGroup="Guardar">
                                                        <RequiredField IsRequired="True" ErrorText="El documento es requerido"></RequiredField>
                                                        <RegularExpression ErrorText="El documento no es valida" ValidationExpression="[0-9]*"></RegularExpression>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                            <div class="column3">Nombre y Apelido: </div>
                                            <div class="column4">
                                                <dx:ASPxTextBox ID="TextNombre" runat="server" Width="170px" MaxLength="100" NullText="Ingrese nombre y apellido ">
                                                    <ValidationSettings ErrorTextPosition="Bottom" ErrorDisplayMode="Text" Display="Dynamic"
                                                        SetFocusOnError="true" ValidationGroup="Guardar">
                                                        <RequiredField IsRequired="True" ErrorText="El nombre es requerido"></RequiredField>
                                                        <RegularExpression ErrorText="Telefono no valido" ValidationExpression="^[a-z A-Z]+$"></RegularExpression>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">E-mail: </div>
                                            <div class="column2">
                                                <dx:ASPxTextBox runat="server" ID="TextEmail" Width="170" MaxLength="100" NullText="alguien@ejemplo.com">
                                                    <ValidationSettings ErrorDisplayMode="Text" Display="Dynamic" ErrorTextPosition="Bottom"
                                                        SetFocusOnError="true">
                                                        <RegularExpression ErrorText="E-mail invalido" ValidationExpression="\w+([-+.&#39;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                            <div class="column3">Usuario: </div>
                                            <div class="column4">
                                                <dx:ASPxTextBox ID="TextUsuario" runat="server" Width="170px" MaxLength="50" NullText="Ingrese un usuario">
                                                    <ValidationSettings ErrorTextPosition="Bottom" ErrorDisplayMode="Text" Display="Dynamic"
                                                        SetFocusOnError="true" ValidationGroup="Guardar">
                                                        <RequiredField IsRequired="True" ErrorText="El usuario es requerido"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Telefono: </div>
                                            <div class="column2">
                                                <dx:ASPxTextBox ID="TextTelefono" ClientInstanceName="TextTelefono" runat="server"
                                                    MaxLength="10" Width="170px">
                                                    <ValidationSettings ErrorTextPosition="Bottom" ErrorText="Telefono no valido" ErrorDisplayMode="Text"
                                                        Display="Dynamic" SetFocusOnError="true" ValidationGroup="Guardar">
                                                        <RegularExpression ErrorText="Telefono no valido" ValidationExpression="[0-9]*"></RegularExpression>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="isNumberKey"></ClientSideEvents>
                                                </dx:ASPxTextBox>
                                            </div>
                                            <div class="column3">Ciudad: </div>
                                            <div class="column4">
                                                <dx:ASPxComboBox ID="cbCiudad" runat="server" ClientInstanceName="cbCiudad" ValueType="System.Int32" FilterMinLength="3" NullText="Seleccione una ciudad" IncrementalFilteringMode="Contains"
                                                    TextFormatString="{0} ({1})" TextField="nombre" ValueField="idCiudad" CallbackPageSize="15" EnableCallbackMode="true" IncrementalFilteringDelay="0" 
                                                    OnItemRequestedByValue="cbCiudad_OnItemRequestedByValue_SQL" OnItemsRequestedByFilterCondition="cbCiudad_OnItemsRequestedByFilterCondition_SQL">
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="nombre" Caption="Ciudad" Width="100px" />
                                                        <dx:ListBoxColumn FieldName="departamento" Caption="Departamento" Width="300px" />
                                                    </Columns>
                                                    <ValidationSettings EnableCustomValidation="true" ValidationGroup="Guardar" SetFocusOnError="True"
                                                        ErrorText="Seleccione una ciudad">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione una ciudad"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { SeleccionaPDV(s,'SeleccionaPDV|');}" />
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Rol: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="CbRol" runat="server" NullText="Seleccione un Rol..." ClientInstanceName="CbRol"
                                                    DropDownStyle="DropDown" ValueType="System.Int32">
                                                    <ValidationSettings ValidationGroup="Guardar" ErrorText="Seleccione un Rol" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Rol"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation" />
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { SeleccionaPDV(s,'validarRequierePDV|');}" />
                                                </dx:ASPxComboBox>
                                            </div>
                                            <div class="column3">Perfil: </div>
                                            <div class="column4">
                                                <dx:ASPxComboBox ID="CbPerfil" runat="server" TextField="Nombre" ValueField="IdPerfil"
                                                    NullText="Seleccione un Perfil..." ClientInstanceName="CbPerfil" DropDownStyle="DropDown"
                                                    ValueType="System.Int32">
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Perfil"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Tipo: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="cbTipo" runat="server" InitialValue="-1" NullText="Seleccione un Tipo de persona..."
                                                    ClientInstanceName="cbTipo" DropDownStyle="DropDown" ValueType="System.Int32">
                                                    <ValidationSettings ValidationGroup="Guardar" ErrorText="Seleccione un Tipo de usuario"
                                                        SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Tipo de usuario"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation" />
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { ValidaRequiereCargoUNEmpresa(s,e,'CambioTipopersona|');}" />
                                                </dx:ASPxComboBox>
                                            </div>
                                            <div class="column3">Cargo: </div>
                                            <div class="column4">
                                                <dx:ASPxComboBox ID="cbCargo" runat="server" NullText="Seleccione un cargo..." ClientInstanceName="cbCargo"
                                                    DropDownStyle="DropDown" ValueType="System.Int32">
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Cargo"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents EndCallback="function(s, e) { RequiereFechaCrea(s, e);}"></ClientSideEvents>
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Empresa: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="cbEmpresa" runat="server"  NullText="Seleccione una Empresa..." IncrementalFilteringMode="Contains" FilterMinLength="3"
                                                    Width="170px" ClientInstanceName="cbEmpresa" ValueType="System.Int32" DropDownStyle="DropDown" CallbackPageSize="15" EnableCallbackMode="True" EnableViewState="true">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { cpRegistro.PerformCallback('ConsultaUnidadDeNegocio|'+ cbEmpresa.GetValue()); }" ></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" ErrorText="Seleccione una Empresa"
                                                        SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione una Empresa"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </div>
                                            <div class="column3">Unidad de Negocio: </div>
                                            <div class="column4">
                                                <dx:ASPxComboBox ID="cbUnidadNegocio" runat="server" ClientInstanceName="cbUnidadNegocio"
                                                    DropDownStyle="DropDown" ValueType="System.Int32">
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione una unidad de negocio"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Fecha de ingreso: </div>
                                            <div class="column2">
                                                <dx:ASPxDateEdit ID="deFechaIngreso" runat="server" ClientEnabled="False" ClientInstanceName="deFechaIngreso">
                                                    <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                    </CalendarProperties>
                                                    <ClientSideEvents Validation="EsRangoValido"></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Registre una Fecha de ingreso"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </div>
                                            <div class="column3">Estado: </div>
                                            <div class="column4">
                                                <dx:ASPxComboBox ID="cbEstado" runat="server" AutoPostBack="False" SelectedIndex="0"
                                                    Width="170px" ClientInstanceName="cbEstado" IncrementalFilteringMode="StartsWith"
                                                    Enabled="True" ValueType="System.Int32">
                                                    <Items>
                                                        <dx:ListEditItem Selected="True" Text="ACTIVO" Value="1" />
                                                        <dx:ListEditItem Text="INACTIVO" Value="0" />
                                                    </Items>
                                                    <ValidationSettings EnableCustomValidation="true" ValidationGroup="Guardar" SetFocusOnError="True"
                                                        ErrorText="Seleccione un Estado">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Estado"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { ValidaEstado(s,e); }" />
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Fecha de Retiro: </div>
                                            <div class="column2">
                                                <dx:ASPxDateEdit ID="deFechaRetiro" runat="server" ClientVisible="False" ClientInstanceName="deFechaRetiro">
                                                    <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                    </CalendarProperties>
                                                    <ClientSideEvents Validation="EsRangoValido"></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Registre una Fecha de ingreso"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </div>
                                            <div class="column3"></div>
                                            <div class="column4">
                                            </div>
                                        </div>
                                        <div>
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
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }"></ClientSideEvents>
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
                                                                        </ValidationSettings>
                                                                        <ClientSideEvents Validation="function(s, e) { ValidarselecciondePDV(s, e,'1'); }" />
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }"></ClientSideEvents>
                                                                    </dx:ASPxListBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxCallbackPanel>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1"></div>
                                            <div class="column2">
                                                <dx:ASPxButton ID="btnActualizarUsuario" runat="server" Text="Actualizar Usuario"
                                                    Style="display: inline!important;" AutoPostBack="False" ValidationGroup="Guardar">
                                                    <ClientSideEvents Click="function(s, e) {  if(ASPxClientEdit.ValidateGroup('Guardar')){
                                                                                            if (confirm('Esta seguro que desea guardar la información?')) {
                                                                                               cpRegistro.PerformCallback('ActualizarUsuario|0');
                                                                                            }
                                                                                        }}" />
                                                    <Image Url="../img/save_all.png">
                                                    </Image>
                                                </dx:ASPxButton>
                                            </div>
                                            <div class="column3"></div>
                                            <div class="column4">
                                                <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Cancelar" Style="display: inline!important;"
                                                    ClientInstanceName="btnLimpiar" AutoPostBack="False" UseSubmitBehavior="False">
                                                    <Image Url="../img/continue.png">
                                                    </Image>
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
        <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
            Modal="True">
        </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
