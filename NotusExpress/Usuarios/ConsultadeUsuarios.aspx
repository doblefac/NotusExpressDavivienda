<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConsultadeUsuarios.aspx.vb"
    Inherits="NotusExpress.ConsultadeUsuarios" %>

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
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        // <![CDATA[
        function OnASPxComboBoxValidation(s, e) {
            if (e.value == -1)
                e.isValid = false;
        }
        function Limpiar() {
            TextNumeroIdentificacion.SetText('');
            TextNombre.SetText('');
            cbCiudad.SetSelectedIndex(0);
            cbCargo.SetSelectedIndex(0);
            CbRol.SetSelectedIndex(0);
            gvUsuarios.Refresh();
           
        }
        function EditUsuario(element, key) {
            window.location.href("EdiciondeUsuario.aspx?idUsuario=" + key + "");
            //            gvUsuarios.PerformCallback(key);

        }
        function ModificarPDVUsuario(element, key) {
            window.location.href("AdministradorUsuariosPdv.aspx?idUsuario=" + key + "");

        }
        // ]]> 
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <%-- <vu:ValidacionURL ID="vuControlSesion" runat="server" />--%>
        <dx:ASPxHiddenField ID="hfDimensiones" ClientInstanceName="hfDimensiones" runat="server">
        </dx:ASPxHiddenField>
        <div id="divEncabezado">
            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
            <br />
        </div>
        <div>
            <dx:ASPxRoundPanel ID="rpCierreCiclo" runat="server" HeaderText="&nbsp;"
                Width="90%">
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" ClientInstanceName="cpGeneral" >
                            <PanelCollection>
                                <dx:PanelContent>
                                    <div id="tablecontainer">
                                        <div class="tablecontainerrow">
                                            <div class="column1">Numero Identificación: </div>
                                            <div class="column2">
                                                <dx:ASPxTextBox ID="TextNumeroIdentificacion" ClientInstanceName="TextNumeroIdentificacion" runat="server" Width="200px">
                                                </dx:ASPxTextBox>
                                            </div>

                                            <div class="column3">Nombre y Apelido: </div>
                                            <div class="column4">
                                                <dx:ASPxTextBox ID="TextNombre" ClientInstanceName="TextNombre" runat="server" Width="200px">
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Ciudad: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="cbCiudad" runat="server" ClientInstanceName="cbCiudad"
                                                    TextField="nombre" SelectedIndex="0" CallbackPageSize="15" Width="180px" EnableCallbackMode="True" EnableViewState="false" FilterMinLength="3"
                                                    ValueField="idCiudad" ValueType="System.Int32" IncrementalFilteringMode="Contains" TextFormatString="{0}-{1}"
                                                    OnItemRequestedByValue="cbCiudad_OnItemRequestedByValue_SQL" OnItemsRequestedByFilterCondition="cbCiudad_OnItemsRequestedByFilterCondition_SQL">
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="nombre" Caption="Ciudad" Width="100px" />
                                                        <dx:ListBoxColumn FieldName="departamento" Caption="Departamento" Width="300px" />
                                                    </Columns>
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation"></ClientSideEvents>
                                                    <ValidationSettings EnableCustomValidation="true" ValidationGroup="Guardar" SetFocusOnError="True"
                                                        ErrorText="Seleccione una ciudad">
                                                        <RequiredField IsRequired="True" ErrorText="Seleccione una ciudad"></RequiredField>
                                                    </ValidationSettings>
                                                    <ClientSideEvents Validation="OnASPxComboBoxValidation" />
                                                </dx:ASPxComboBox>
                                            </div>
                                            <div class="column3">Cargo: </div>
                                            <div class="column4">
                                                <dx:ASPxComboBox ID="cbCargo" runat="server" InitialValue="-1" SelectedIndex="0" TextField="nombre" ValueField="idCargo"
                                                    ClientInstanceName="cbCargo" IncrementalFilteringMode="StartsWith">
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">Rol: </div>
                                            <div class="column2">
                                                <dx:ASPxComboBox ID="CbRol" AutoPostBack="false" ValueField="idRol" TextField="nombre" runat="server"
                                                    ClientInstanceName="CbRol" DropDownStyle="DropDown" InitialValue="-1">                                                    
                                                </dx:ASPxComboBox>
                                            </div>

                                        </div>
                                        <div class="tablecontainerrow">
                                            <div class="column1">
                                                <dx:ASPxButton ID="btnConsultar" runat="server" AutoPostBack="False" ValidationGroup="filtrar"
                                                    Text="Consultar">
                                                    <ClientSideEvents Click="function(s, e) { cpGeneral.PerformCallback('Consultar|1');}"></ClientSideEvents>
                                                     <Image Url="~/img/find.png"> </Image>
                                                </dx:ASPxButton>
                                            </div>
                                            <div class="column3">
                                                <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar" Style="display: inline!important;"
                                                    ClientInstanceName="btnLimpiar" AutoPostBack="False">
                                                    <ClientSideEvents Click="function(s, e) {Limpiar(); }"></ClientSideEvents>
                                                    <Image Url="~/img/edit-clear.png">
                                                    </Image>
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <div>
                                        <dx:ASPxGridView ID="gvUsuarios" Width="100%" runat="server" AutoGenerateColumns="False" Font-Size="Small"
                                            ClientInstanceName="gvUsuarios" KeyFieldName="IdUsuario" EnableRowsCache="False">
                                            <SettingsBehavior EnableCustomizationWindow="true" AutoExpandAllGroups="true" />
                                            <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False"
                                                ShowGroupPanel="True"></Settings>
                                            <SettingsText Title="Resultado Consulta de Usuario" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda"
                                                CommandEdit="Editar"></SettingsText>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Idusuario" FieldName="IdUsuario" VisibleIndex="0">
                                                    <CellStyle HorizontalAlign="Center">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Numero Documento" FieldName="NumeroIdentificacion"
                                                    VisibleIndex="1">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Nombre y Apellido" FieldName="NombreApellido" VisibleIndex="2">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Ciuda" FieldName="Ciudad" VisibleIndex="3">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Cargo" FieldName="Cargo" VisibleIndex="4">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Rol" FieldName="Rol" VisibleIndex="5">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataComboBoxColumn Caption="Estado" FieldName="Estado" VisibleIndex="6">
                                                    <CellStyle HorizontalAlign="Center">
                                                    </CellStyle>
                                                </dx:GridViewDataComboBoxColumn>
                                                <dx:GridViewDataColumn Caption="Opciones" VisibleIndex="7">
                                                    <DataItemTemplate>
                                                        <dx:ASPxHyperLink runat="server" ID="lnkEditarUsuario" ImageUrl="../img/edit.gif"
                                                            ToolTip="Editar Usuario" OnInit="LinkInit">
                                                            <ClientSideEvents Click="function(s, e) { cpGeneral.PerformCallback('EditUsuario|' +{0} );}" />
                                                        </dx:ASPxHyperLink>
                                                        <dx:ASPxHyperLink runat="server" ID="lnkModPdv" ImageUrl="~/img/House_Sale.png"
                                                            ToolTip="Modificar puntos de venta del usuario " OnInit="LinkPdvInit">
                                                            <ClientSideEvents Click="function(s, e) { cpGeneral.PerformCallback('AsignarPDV|' +{0} ); }" />
                                                        </dx:ASPxHyperLink>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                            <SettingsBehavior AutoExpandAllGroups="True" EnableCustomizationWindow="True"></SettingsBehavior>
                                            <SettingsPager PageSize="50">
                                            </SettingsPager>
                                            <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False"
                                                ShowGroupPanel="true" />
                                            <SettingsText CommandEdit="Editar" Title="Resultado Consulta de Usuario" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
                                            <SettingsLoadingPanel Text="Cargando&amp;hellip;"></SettingsLoadingPanel>
                                        </dx:ASPxGridView>
                                    </div>

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
