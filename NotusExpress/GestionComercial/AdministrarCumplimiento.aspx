<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministrarCumplimiento.aspx.vb" Inherits="NotusExpress.AdministrarCumplimiento" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

    <%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Administrar Cumplimiento Metas</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
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

        function solonumeros(e) {

            var key;

            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }

            if (key < 48 || key > 57) {
                if (key == 46) {
                    return true;
                }
                return false;
            }

            return true;
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
    <form id="frmAdmMeta" runat="server">
    
    <div id="div1">
        <uc1:EncabezadoPagina ID="miEncabezado" runat="server" />
    </div>
    <vu:ValidacionURL ID="vuControlSesion" runat="server" />
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        <br />
    </div>
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
        <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e);}" />
        <PanelCollection>
            <dx:PanelContent>
    <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="" ClientInstanceName="rpFiltros"
        DefaultButton="btnBuscar">
        <HeaderTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="white-space: nowrap;" align="left">
                        Cumplimiento de Metas
                    </td>
                    <td style="width: 1%; padding-left: 5px;">
                        
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
                                        Inicio:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtInicio1" runat="server" Width="100px" 
                                                                        ClientInstanceName="txtInicio1" MaxLength="4" onkeypress="return solonumeros(event);" Text="">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Valor de inicio requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Fin:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtFin1" runat="server" Width="100px" 
                                                                        ClientInstanceName="txtFin1" MaxLength="4" onkeypress="return solonumeros(event);" Text="">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Valor de fin requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Color:
                                    </td>
                                    <td>
                                        <dx:ASPxUploadControl ID="uplArchivo1" runat="server" Width="280px" ClientInstanceName="uplArchivo1">
                                            <ValidationSettings AllowedFileExtensions=".gif, .png, .bmp, .jpg, .jpeg">
                                            </ValidationSettings>
                                        </dx:ASPxUploadControl>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Inicio:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtInicio2" runat="server" Width="100px" 
                                                                        ClientInstanceName="txtInicio2" MaxLength="4" onkeypress="return solonumeros(event);" Text="">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Valor de inicio requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Fin:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtFin2" runat="server" Width="100px" 
                                                                        ClientInstanceName="txtFin2" MaxLength="4" onkeypress="return solonumeros(event);" Text="">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Valor de fin requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Color:
                                    </td>
                                    <td>
                                        <dx:ASPxUploadControl ID="uplArchivo2" runat="server" Width="280px">
                                        </dx:ASPxUploadControl>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Inicio:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtInicio3" runat="server" Width="100px" 
                                                                        ClientInstanceName="txtInicio3" MaxLength="4" onkeypress="return solonumeros(event);" Text="">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Valor de inicio requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Fin:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtFin3" runat="server" Width="100px"
                                                                        ClientInstanceName="txtFin3" MaxLength="3" onkeypress="return solonumeros(event);" Text="">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Valor de fin requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                    </td>
                                    <td class="field">
                                        Color:
                                    </td>
                                    <td>
                                        <dx:ASPxUploadControl ID="uplArchivo3" runat="server" Width="280px">
                                        </dx:ASPxUploadControl>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="padding-top: 8px">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="width:1050%; padding-left: 5px;" align="center">
                                                    <dx:ASPxButton ID="btnNuevo" runat="server" Text="Crear Cumplimiento" 
                                                        ValidationGroup="Registro">
                                                        <Image Url="~/img/new.png">
                                                        </Image>
                                                        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'crearCumplimiento');}" />
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
    </dx:PanelContent>
        </PanelCollection>
        <LoadingDivStyle CssClass="modalBackground">
        </LoadingDivStyle>
    </dx:ASPxCallbackPanel>
    <br />
          <%--      <dx:ASPxGridView ID="gvCumplimiento" runat="server" AutoGenerateColumns="False" KeyFieldName="IdCumplimiento" ClientInstanceName="gvCumplimiento">
                    
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0">
                            <EditButton Visible="false">
                            </EditButton>
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="IdCumplimiento" Name="fieldIdCumplimiento" VisibleIndex="1"
                            ReadOnly="True">
                            <PropertiesTextEdit MaxLength="50">
                            </PropertiesTextEdit>
                            <EditFormSettings VisibleIndex="0" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Inicio 1" FieldName="Inicio1" Name="fieldInicio1"
                            VisibleIndex="2">
                            <EditFormSettings VisibleIndex="1" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Fin 1" FieldName="Fin1" Name="fieldFin1"
                            VisibleIndex="2">
                            <EditFormSettings VisibleIndex="2" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Inicio 2" FieldName="Inicio2" Name="fieldInicio2"
                            VisibleIndex="2">
                            <EditFormSettings VisibleIndex="3" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Fin 2" FieldName="Fin2" Name="fieldFin2"
                            VisibleIndex="2">
                            <EditFormSettings VisibleIndex="4" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Inicio 3" FieldName="Inicio3" Name="fieldInicio3"
                            VisibleIndex="2">
                            <EditFormSettings VisibleIndex="5" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Fin 3" FieldName="Fin3" Name="fieldFin3"
                            VisibleIndex="2">
                            <EditFormSettings VisibleIndex="6" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False" />
                    <SettingsText CommandEdit="Editar" Title="Cumplimiento Metas" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
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
                </dx:ASPxGridView>--%>
           
    
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>

