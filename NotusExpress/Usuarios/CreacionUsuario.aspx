    <%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreacionUsuario.aspx.vb"
    Inherits="NotusExpress.CreacionUsuario" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<%@ Register Src="../ControlesDeUsuario/MensajePopUp.ascx" TagName="MensajePopUp"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
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
            width: 250px;
            float: left;
        }

        div#tablecontainer div div.column2
        {
            width: 250px;
            float: left;
        }

        div#tablecontainer div div.column3
        {
            width: 250px;
            float: left;
        }

        div#tablecontainer div div.column4
        {
            width: 250px;
            float: left;
        }
    </style>

    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        // <![CDATA[
        
        function SeleccionaPDV(s, key) {
            cpRegistro.PerformCallback(key);

        }
        /*Valida que si se confirma password*/
        function ValidarConfirmaPass(s, e) {
            if (TextUsuario.GetValue() != null) {
                /*Se trata de un usuario que debe tener Password*/
                if (passwordTextBox.GetValue() == null) {
                    e.isValid = false;
                }
                else {
                    if (confirmPasswordTextBox.GetValue() == null) {
                        e.isValid = false;
                    }
                    else {
                        e.isValid = true;
                    }
                }
            }
        }
        /*Valida que si se escribe en Usuario y Clave ,se debe solicitar password*/
        function ValidarPass(s, e) {
            if (TextUsuario.GetValue() != null) {
                /*Se trata de un usuario que debe tener Password*/
                if (passwordTextBox.GetValue() == null) {
                    e.isValid = false;
                }
                else {
                    if (passwordTextBox.GetValue() == null) {
                        e.isValid = false;
                    }
                    else {
                        e.isValid = true;
                    }
                }
            }
        }
        var passwordMinLength = 6;
        function GetPasswordRating(password) {
            var result = 0;
            if (password) {
                result++;
                if (password.length >= passwordMinLength) {
                    if (/[a-z]/.test(password))
                        result++;
                    if (/[A-Z]/.test(password))
                        result++;
                    if (/\d/.test(password))
                        result++;
                    if (!(/^[a-z0-9]+$/i.test(password)))
                        result++;
                }
            }
            return result;
        }
        function OnPasswordTextBoxInit(s, e) {
            ApplyCurrentPasswordRating();
        }
        function OnPassChanged(s, e) {
            ApplyCurrentPasswordRating();
        }
        function ApplyCurrentPasswordRating() {
            var password = passwordTextBox.GetText();
            var passwordRating = GetPasswordRating(password);
            ApplyPasswordRating(passwordRating);
        }
        function ApplyPasswordRating(value) {
            ratingControl.SetValue(value);
            switch (value) {
                case 0:
                    ratingLabel.SetValue("Nivel de Seguridad");
                    break;
                case 1:
                    ratingLabel.SetValue("Demasiado simple");
                    break;
                case 2:
                    ratingLabel.SetValue("No es seguro");
                    break;
                case 3:
                    ratingLabel.SetValue("Normal");
                    break;
                case 4:
                    ratingLabel.SetValue("Seguro");
                    break;
                case 5:
                    ratingLabel.SetValue("Muy seguro");
                    break;
                default:
                    ratingLabel.SetValue("Nivel de Seguridad");
            }
        }
        function GetErrorText(editor) {
            if (editor === passwordTextBox) {
                if (ratingControl.GetValue() === 1)
                    return "El password es demasiado simple";
            } else if (editor === confirmPasswordTextBox) {
                if (passwordTextBox.GetText() !== confirmPasswordTextBox.GetText())
                    return "El password que introducido no coinciden";
            }
            return "";
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
        function ValidaTipoPersona(s, e)
        {
            cbCargo.SetIsValid( cbTipo.GetValue() == 1);
            //
            e.ValidationSettings.is = cbTipo.GetValue() == 1;
            cbUnidadNegocio.SetIsValid( cbTipo.GetValue() == 1);
            //cpRegistro.PerformCallback('CambioTipopersona|0')

        }
        function RequiereFechaCrea(s, e) {
            if (hdMostarPDV.Get('cpRequiereFechaCrea') == 1) {

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
                        <dx:ASPxRoundPanel ID="rpPrincipal" runat="server" HeaderText="&nbsp;" Width="70%">                            
                            <PanelCollection>
                                <dx:PanelContent>
                                    <div id="tablecontainer">
                                        <dx:ASPxHiddenField ID="hfDimensiones" ClientInstanceName="hfDimensiones" runat="server"></dx:ASPxHiddenField>
                                        <dx:ASPxHiddenField ID="hdMostarPDV" runat="server" ClientInstanceName="hdMostarPDV"></dx:ASPxHiddenField>
                                        <dx:ASPxHiddenField ID="cpRequiereFechaCrea" runat="server" ClientInstanceName="cpRequiereFechaCrea"></dx:ASPxHiddenField>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Numero Identificación: </div>
                                            <div class="column2">
                                                <dx:ASPxTextBox ID="TextNumeroIdentificacion" runat="server" Width="250px" MaxLength="15"
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
                                                        <RegularExpression ErrorText="El nombre no valido" ValidationExpression="^[a-zA-Z''-' Ñ ñ\s]{1,40}$"></RegularExpression>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">E-mail: </div>
                                            <div class="column2">
                                                <dx:ASPxTextBox runat="server" ID="TextEmail" Width="250" MaxLength="100" NullText="alguien@ejemplo.com">
                                                    <ValidationSettings ErrorDisplayMode="Text" Display="Dynamic" ErrorTextPosition="Bottom"
                                                        SetFocusOnError="true" ValidationGroup="Guardar">
                                                        <RequiredField IsRequired="True" ErrorText="El email es requerido"></RequiredField>
                                                        <RegularExpression ErrorText="E-mail invalido" ValidationExpression="\w+([-+.&#39;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>                                                        
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>

                                            <div class="column3">Usuario: </div>
                                            <div class="column4">
                                                <dx:ASPxTextBox ID="TextUsuario" runat="server" Width="170px" ClientInstanceName="TextUsuario" MaxLength="50" NullText="Ingrese un usuario">
                                                    <ValidationSettings ErrorTextPosition="Bottom" ErrorDisplayMode="Text" Display="Dynamic"
                                                        SetFocusOnError="true" ValidationGroup="Guardar">
                                                        <RequiredField IsRequired="True" ErrorText="El usuario es requerido"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                       <div class="tablecontainerrow">
                                            <div class="column3">Telefono: </div>
                                            <div class="column4">
                                                <dx:ASPxTextBox ID="TextTelefono" ClientInstanceName="TextTelefono" runat="server"
                                                    MaxLength="10" Width="250px">
                                                    <ValidationSettings ErrorTextPosition="Bottom" ErrorText="Telefono no valido" ErrorDisplayMode="Text"
                                                        Display="Dynamic" SetFocusOnError="true" ValidationGroup="Guardar">
                                                        <RegularExpression ErrorText="Telefono no valido" ValidationExpression="[0-9]*"></RegularExpression>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="isNumberKey"></ClientSideEvents>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Ciudad: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="cbCiudad" runat="server" ClientInstanceName="cbCiudad" ValueType="System.Int32" FilterMinLength="4" NullText="Seleccione una ciudad" IncrementalFilteringMode="Contains"
                                                    TextFormatString="{0} ({1})" Width="250px" TextField="nombre" ValueField="idCiudad" CallbackPageSize="15" EnableCallbackMode="true" IncrementalFilteringDelay="0"
                                                    OnItemRequestedByValue="cbCiudad_OnItemRequestedByValue_SQL" OnItemsRequestedByFilterCondition="cbCiudad_OnItemsRequestedByFilterCondition_SQL">
                                                       <Columns>
                                                        <dx:ListBoxColumn FieldName="nombre" Caption="Ciudad" Width="100px" />
                                                        <dx:ListBoxColumn FieldName="departamento" Caption="Departamento" Width="300px" />
                                                    </Columns>
                                                    <ValidationSettings EnableCustomValidation="true" ValidationGroup="Guardar" SetFocusOnError="True"
                                                        ErrorText="Seleccione una ciudad">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione una ciudad"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { SeleccionaPDV(s,'SeleccionaPDV|0');}" />
                                                </dx:ASPxComboBox>
                                            </div>
                                            <div class="column3">Rol: </div>
                                            <div class="column4">
                                                <dx:ASPxComboBox ID="CbRol" runat="server" NullText="Seleccione un Rol..." Width="170px"
                                                    ClientInstanceName="CbRol" DropDownStyle="DropDown">
                                                    <ValidationSettings ValidationGroup="Guardar" ErrorText="Seleccione un Rol" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Rol"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation" />
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { SeleccionaPDV(s,'validarRequierePDV|0'); mostarPDV(s,e)}" />
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>


                                        <div class="tablecontainerrow">
                                            <div class="column1">Perfil: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="CbPerfil" runat="server" TextField="Nombre" ValueField="IdPerfil"
                                                    NullText="Seleccione un Perfil..." Width="250px" ClientInstanceName="CbPerfil"
                                                    DropDownStyle="DropDown" Height="16px">
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Perfil"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { mostarPDV(s,e)}" />
                                                </dx:ASPxComboBox>
                                            </div>
                                                <div class="column3">Cargo: </div>
                                            <div class="column4">
                                               <dx:ASPxComboBox ID="cbCargo" runat="server" NullText="Seleccione un cargo..." Width="170px"
                                                    ClientInstanceName="cbCargo" DropDownStyle="DropDown">
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True" ErrorText="Seleccione un Cargo">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Cargo"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents EndCallback="function(s, e) { RequiereFechaCrea(s, e);}"></ClientSideEvents>
                                                </dx:ASPxComboBox>
                                            </div>
                                        
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Empresa: </div>
                                            <div class="column2">
                                                 <dx:ASPxComboBox ID="cbEmpresa" runat="server" NullText="Seleccione una Empresa..." TextField="nombre" ValueType="System.String" ValueField="idEmpresa" IncrementalFilteringMode="Contains"
                                                    Width="250px" ClientInstanceName="cbEmpresa" DropDownStyle="DropDown" CallbackPageSize="15" >
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { cpRegistro.PerformCallback('CargarUnidadDeNegocio|'+ cbEmpresa.GetValue()); }" ></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" ErrorText="Seleccione una Empresa"
                                                        SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione una Empresa"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                                 
                                            </div>

                                            <div class="column3">Tipo: </div>
                                            <div class="column4">
                                               <dx:ASPxComboBox ID="cbTipo" runat="server" InitialValue="-1" NullText="Seleccione un Tipo de persona..."
                                                    Width="170px" ClientInstanceName="cbTipo" DropDownStyle="DropDown">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { ValidaTipoPersona(s,e);}"
                                                        Validation="OnASPxComboBoxValidation"></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" ErrorText="Seleccione un Tipo de usuario"
                                                        SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Tipo de usuario"></RequiredField>
                                                    </ValidationSettings>                                                    
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Unidad de Negocio: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="cbUnidadNegocio" runat="server" Width="250px" NullText="Seleccione una unidad de negocio..." TextField="nombre" ValueField="idUnidadNegocio" ClientInstanceName="cbUnidadNegocio"
                                                    DropDownStyle="DropDown">
                                                    <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e)}"></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True" ErrorText="Seleccione una unidad de negocio">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione una unidad de negocio"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </div>

                                            <div class="column3">Fecha de ingreso: </div>
                                            <div class="column4">
                                                <dx:ASPxDateEdit ID="deFechaIngreso" runat="server" ClientEnabled="False" ClientInstanceName="deFechaIngreso">
                                                    <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                    </CalendarProperties>
                                                    <ClientSideEvents Validation="EsRangoValido"></ClientSideEvents>
                                                    <ValidationSettings ValidationGroup="Guardar" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" ErrorText="Registre una Fecha de ingreso"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Estado: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="cbEstado" runat="server" AutoPostBack="False" SelectedIndex="0"
                                                    Width="170px" ClientInstanceName="cbEstado" IncrementalFilteringMode="StartsWith"
                                                    Enabled="False">
                                                    <Items>
                                                        <dx:ListEditItem Selected="True" Text="ACTIVO" Value="1" />
                                                        <dx:ListEditItem Text="INACTIVO" Value="0" />
                                                    </Items>
                                                    <ValidationSettings EnableCustomValidation="true" ValidationGroup="Guardar" SetFocusOnError="True"
                                                        ErrorText="Seleccione un Estado">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione un Estado"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </div>

                                            <div class="column3"></div>
                                            <div class="column4">
                                            </div>
                                        </div>
                                        <br />
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
                                                <dx:ASPxButton ID="btnRegistrarUsuario" runat="server" Text="Registrar Usuario" Style="display: inline!important;"
                                                    AutoPostBack="False" ValidationGroup="Guardar">
                                                     <ClientSideEvents Click="function(s, e) {  if(ASPxClientEdit.ValidateGroup('Guardar')){
                                                                                            if (confirm('Esta seguro que desea Crear el usuario?')) {
                                                                                               cpRegistro.PerformCallback('RegistrarUsuario|0');
                                                                                            }
                                                                                        }}" />
                                                    <Image Url="../img/save_all.png">
                                                    </Image>
                                                </dx:ASPxButton>
                                            </div>

                                            <div class="column3"></div>
                                            <div class="column4">
                                                <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar" Style="display: inline!important;"
                                                    ClientInstanceName="btnLimpiar" AutoPostBack="False" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {SeleccionaPDV(s,'LimpiarPanel|')}"></ClientSideEvents>
                                                    <Image Url="~/img/edit-clear.png">
                                                    </Image>
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                    <uc1:MensajePopUp ID="mensajero" runat="server" />
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
