<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministrarIndicadoresDeCumplimiento.aspx.vb"
    Inherits="NotusExpress.AdministrarIndicadoresDeCumplimiento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administrar Indicadores de Cumplimiento</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <vu:ValidacionURL ID="vuControlSesion" runat="server" />
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        <br />
    </div>
    <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Nuevo Rango" ClientInstanceName="rpFiltros"
        DefaultButton="btnBuscar">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxPanel ID="pnlNuevoIndicador" runat="server" Width="100%" ClientInstanceName="pnlNuevoIndicador">
                    <Paddings Padding="0px" />
                    <PanelCollection>
                        <dx:PanelContent>
                            <table id="tblIndicador">
                                <tr>
                                    <td class="field">
                                        Valor Inicial:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtValorInicial" runat="server" Width="80px" ClientInstanceName="txtValorInicial"
                                            MaxLength="4">
                                            <ValidationSettings Display="Dynamic" ValidationGroup="adicionar" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="Valor requerido" />
                                                <RegularExpression ValidationExpression="^[0-9]{1,3}\.?[0-9]*$" ErrorText="Valor Incorrecto. Se espera un valor numérico entre 0.0 y 100" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Valor Final:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtValorFinal" runat="server" Width="80px" ClientInstanceName="txtValorFinal"
                                            MaxLength="4">
                                            <ValidationSettings Display="Dynamic" ValidationGroup="adicionar" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="Valor requerido" />
                                                <RegularExpression ErrorText="Valor Incorrecto. Se espera un valor numérico entre 0.0 y 100" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Imagen:
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbImagen" runat="server" ShowImageInEditBox="true" SelectedIndex="-1"
                                            ValueType="System.String" AutoResizeWithContainer="true" 
                                            ClientInstanceName="cbImagen" Width="80px">
                                            <Items>
                                                <dx:ListEditItem ImageUrl="~/img/BallRed.png" Text="" Value="~/img/BallRed.png" />
                                                <dx:ListEditItem ImageUrl="~/img/BallOrange.png" Text="" Value="~/img/BallOrange.png" />
                                                <dx:ListEditItem ImageUrl="~/img/BallGreen.png" Text="" Value="~/img/BallGreen.png" />
                                            </Items>
                                            <ValidationSettings Display="Dynamic" CausesValidation="true" ValidateOnLeave="true"
                                                ValidationGroup="adicionar">
                                                <RequiredField IsRequired="true" ErrorText="Imagen de indicador requerida" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td style="padding-left: 10px">
                                        <dx:ASPxButton ID="btnAdicionar" runat="server" Text="Adicionar" Style="display: inline!important;"
                                            AutoPostBack="false" ValidationGroup="adicionar">
                                            <Image Url="~/img/add.png">
                                            </Image>
                                            <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'filtrarDatos');}" />
                                        </dx:ASPxButton>
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
    <dx:ASPxGridView ID="gvIndicadores" runat="server" AutoGenerateColumns="False" KeyFieldName="IdIndicador">
        <ClientSideEvents EndCallback="ActualizarEncabezado" />
        <Columns>
            <dx:GridViewDataTextColumn Caption="Valor Inicial" FieldName="ValorInicial" Name="fieldValorInicial"
                VisibleIndex="0">
                <EditFormSettings VisibleIndex="0" />
                <PropertiesTextEdit DisplayFormatString="p1" MaxLength="4">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                        <RequiredField IsRequired="true" ErrorText="Valor requerido" />
                        <RegularExpression ValidationExpression="^[0-9]{1,3}\.?[0-9]*$" ErrorText="Valor Incorrecto. Se espera un valor numérico entre 0.0 y 100" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Valor Final" FieldName="ValorFinal" Name="fieldValorFinal"
                VisibleIndex="1">
                <EditFormSettings VisibleIndex="1" />
                <PropertiesTextEdit DisplayFormatString="p1" MaxLength="4">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                        <RegularExpression ValidationExpression="^[0-9]{1,3}\.?[0-9]*$" ErrorText="Valor Incorrecto. Se espera un valor numérico entre 0.0 y 100" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataImageColumn Caption="Im&aacute;gen" FieldName="RutaImagen" Name="fieldImagen"
                VisibleIndex="2" ReadOnly="true">
                <EditFormSettings VisibleIndex="2" />
                <PropertiesImage ImageHeight="24px" ImageWidth="24px">
                </PropertiesImage>
            </dx:GridViewDataImageColumn>
            <dx:GridViewCommandColumn VisibleIndex="3" ButtonType="Image" Caption="Opciones">
            </dx:GridViewCommandColumn>
            <dx:GridViewCommandColumn ButtonType="Link" Visible="false">
                </dx:GridViewCommandColumn>
        </Columns>
        <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False" />
        <SettingsText CommandDelete="Eliminar" CommandEdit="Editar" ConfirmDelete="Realmente desea eliminar el registro?"
            Title="Listado de Indicadores" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
        <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1" />
        <SettingsPopup>
            <EditForm Width="300px" Height="200px" Modal="true" HorizontalAlign="WindowCenter"
                VerticalAlign="WindowCenter" />
        </SettingsPopup>
        <StylesEditors>
            <ReadOnlyStyle ForeColor="Gray" BackColor="LightGray">
            </ReadOnlyStyle>
            <ReadOnly ForeColor="Gray">
            </ReadOnly>
        </StylesEditors>
        <SettingsBehavior ConfirmDelete="true" />
    </dx:ASPxGridView>
    <msgp:MensajePopUp ID="mensajero" runat="server" />
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
