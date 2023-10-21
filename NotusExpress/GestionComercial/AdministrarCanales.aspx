<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministrarCanales.aspx.vb" Inherits="NotusExpress.AdministrarCanales" %>

<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Administrar Canales</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
     <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function EjecutarCallbackGeneral(s, e, parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                loadingPanel.Show();
                cpGeneral.PerformCallback(parametro);
            }
        }

        function OnExpandCollapseButtonClick(s, e) {
            var isVisible = pnlFiltros.GetVisible();
            s.SetText(isVisible ? "+" : "-");
            pnlFiltros.SetVisible(!isVisible);
        }
        function ActivarOInactivarFiltros() {
            rpFiltros.SetEnabled(!pucNuevoCanal.IsVisible());
        }
    </script>
    <style type="text/css">
        #footer
        {
            position: fixed !important;
            position: absolute;
            bottom: 0;
            right: 0;
            width: 100%;
            text-align: right !important;
        }
    </style>
</head>
<body>
    <form id="frmAdmMarca" runat="server">
    <vu:ValidacionURL ID="vuControlSesion" runat="server" />
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        <br />
    </div>
    <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="" ClientInstanceName="rpFiltros" DefaultButton="btnBuscar">
    <HeaderTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="white-space: nowrap;" align="left">
                        Filtros de B&uacute;squeda
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
                    <PanelCollection>
                        <dx:PanelContent>
                            <table id="tblFiltro">
                                <tr>
                                    <td class="field">
                                        Nombre:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtFiltroNombre" runat="server" Width="200px" MaxLength="50"
                                            ClientInstanceName="txtFiltroNombre">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Filtrado"
                                                ErrorTextPosition="Bottom">
                                                <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$"
                                                    ErrorText="El texto digitado contiene caracteres no permitidos" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Estado:
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbFiltroEstado" runat="server" ValueType="System.Byte" ClientInstanceName="cbFiltroEstado">
                                            <Items>
                                                <dx:ListEditItem Text="Todos..." Value="" Selected="true" />
                                                <dx:ListEditItem Text="Activo" Value="1" />
                                                <dx:ListEditItem Text="Inactivo" Value="0" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="padding-top: 8px">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="white-space: nowrap;">
                                                    <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline!important;
                                                        float: left;" AutoPostBack="false" ValidationGroup="Filtrado">
                                                        <Image Url="~/img/find.png">
                                                        </Image>
                                                        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'filtrarDatos');}" />
                                                    </dx:ASPxButton>
                                                    &nbsp;&nbsp;&nbsp;<dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar" Style="display: inline!important;
                                                        float: right;" AutoPostBack="false">
                                                        <Image Url="~/img/edit-clear.png">
                                                        </Image>
                                                        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'limpiarFiltros'); txtFiltroNombre.SetText(''); cbFiltroEstado.SetSelectedIndex(0);}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td style="width: 5%;">
                                                </td>
                                                <td style="width: 50%; padding-left: 5px;" align="right">
                                                    <dx:ASPxButton ID="btnNuevo" runat="server" Text="Crear Nuevo Canal" AutoPostBack="false">
                                                        <ClientSideEvents Click="function(s,e){ pucNuevoCanal.Show(); txtNombreCanal.SetText(''); }" />
                                                        <Image Url="~/img/new.png">
                                                        </Image>
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
        <ContentPaddings PaddingBottom="2px" PaddingLeft="2px" PaddingRight="2px" PaddingTop="2px" />
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
        <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e); ActivarOInactivarFiltros();}" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="gvListaCanales" runat="server" AutoGenerateColumns="False" KeyFieldName="IdCanal">
                    <ClientSideEvents EndCallback="ActualizarEncabezado" />
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0">
                            
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="IdCanal" Name="fieldIdCanal" VisibleIndex="1"
                            ReadOnly="True">
                            <PropertiesTextEdit MaxLength="50">
                            </PropertiesTextEdit>
                            <EditFormSettings VisibleIndex="0" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Nombre" FieldName="Nombre" Name="fieldNombre"
                            VisibleIndex="2">
                            <PropertiesTextEdit MaxLength="40">
                                <ValidationSettings CausesValidation="True" Display="Dynamic" 
                                    EnableCustomValidation="True" ErrorText="Valor inválido">
                                    <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$" />
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditFormSettings VisibleIndex="1" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn Caption="Estado" FieldName="Estado" Name="fieldEstado"
                            VisibleIndex="3" ReadOnly="false">
                            <PropertiesComboBox ValueType="System.Byte" ValueField="IdEstado" TextField="Estado">
                            </PropertiesComboBox>
                            <EditFormSettings VisibleIndex="2" />
                        </dx:GridViewDataComboBoxColumn>
                    </Columns>
                    <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False" />
                    <SettingsText CommandEdit="Editar" Title="Listado de Canales" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
                    <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1" />
                    <SettingsPopup>
                        <EditForm Width="40%" Modal="true" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" />
                    </SettingsPopup>
                    <StylesEditors>
                        <ReadOnlyStyle ForeColor="Gray" BackColor="LightGray">
                        </ReadOnlyStyle>
                        <ReadOnly ForeColor="Gray">
                        </ReadOnly>
                    </StylesEditors>
                </dx:ASPxGridView>
                <dx:ASPxPopupControl ID="pucNuevoCanal" runat="server" HeaderText="Crear Nuevo Canal"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Modal="True"
                    ClientInstanceName="pucNuevoCanal" ScrollBars="Auto" ShowMaximizeButton="True"
                    ShowPageScrollbarWhenModal="True" CloseAction="CloseButton" Width="400px" Height="200px"
                    MinWidth="400px" MinHeight="200px">
                    <ModalBackgroundStyle CssClass="modalBackground" />
                    <ClientSideEvents Closing="function (s,e){rpFiltros.SetEnabled(true);}" Shown="function (s,e){rpFiltros.SetEnabled(false);}" />
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <dx:ASPxPanel ID="pnlDatosCanal" runat="server" DefaultButton="btnRegistrar" TabIndex="0">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" EnableViewState="false" AlignItemCaptionsInAllGroups="True" RequiredMarkDisplayMode="RequiredOnly">
                                            <Items>
                                                <dx:LayoutGroup Caption="Informaci&oacute;n del Canal" GroupBoxDecoration="Box">
                                                    <Items>
                                                        <dx:LayoutItem Caption="Nombre" HelpText="">
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server">
                                                                    <dx:ASPxTextBox ID="txtNombreCanal" runat="server" Width="250px" NullText="Ingrese nombre del canal..."
                                                                        ClientInstanceName="txtNombreCanal" MaxLength="70">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Nombre de canal requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                            <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$"
                                                                                ErrorText="Nombre de canal no v&aacute;lido" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                    </Items>
                                                    <SettingsItemCaptions HorizontalAlign="Left" Location="Left" />
                                                    <SettingsItemHelpTexts Position="Bottom"></SettingsItemHelpTexts>
                                                </dx:LayoutGroup>
                                            </Items>
                                        </dx:ASPxFormLayout>
                                        <table width="90%">
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxButton runat="server" ID="btnRegistrar" Text="Registrar" AutoPostBack="False"
                                                        ImageSpacing="5px" ValidationGroup="Registro">
                                                        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'registrarDatos');}" />
                                                        <Image Url="~/img/save_all.png">
                                                        </Image>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxPanel>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--<msgp:MensajePopUp ID="mensajero" runat="server" />--%>
            </dx:PanelContent>
        </PanelCollection>
        <LoadingDivStyle CssClass="modalBackground">
        </LoadingDivStyle>
    </dx:ASPxCallbackPanel>
    
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
