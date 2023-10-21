<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImportarVentas.aspx.vb" Inherits="NotusExpress.ImportarVentas" %>

<%@ Register Src="~/ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="~/ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../../../Jquery/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="smAjaxManager" runat="server">
        </asp:ScriptManager>
        <div>
            <%-- <uc1:ValidacionURL ID="validadorUrl" runat="server" />--%>
        </div>

        <!-- Creacion y Actualizacion de Datos -->
        <div id="divEncabezado" style="visibility: hidden;">
        </div>


        <%--<dx:ASPxCallbackPanel ID="cpGeneral" ClientInstanceName="cpGeneral" Width="1400px"
            runat="server">
            <PanelCollection>
                <dx:PanelContent>--%>

        <div id="divContenido">

            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
            <dx:ASPxRoundPanel ID="rpFiltros" Width="60%" runat="server" HeaderText="Filtros de B&uacute;squeda">
                <HeaderTemplate>
                    Filtros de B&uacute;squeda
                               
                </HeaderTemplate>
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxPanel ID="pnlFiltros" runat="server" Width="100%" ClientInstanceName="pnlFiltros">
                            <Paddings Padding="0px" />
                            <Paddings Padding="0px"></Paddings>
                            <PanelCollection>
                                <dx:PanelContent>
                                    <table cellpadding="1" cellspacing="1" class="tabla" width="100%">
                                        <tr>
                                            <td align="right">
                                                <b>Estrategia Comercial: </b>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cbFiltroEstrategia" runat="server" ClientInstanceName="cbFiltroEstrategia"
                                                    TabIndex="5" Width="250px">
                                                    <ValidationSettings ValidationGroup="grupo">
                                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <b>Ruta: </b>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="adjunto" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <dx:ASPxButton runat="server" ValidationGroup="grupo" AutoPostBack="False" Style="display: inline" Text="Enviar Ventas"
                                                    Width="200px" ID="btnEnviar" ClientInstanceName="btnEnviar">
                                                    <ClientSideEvents Click="function(s, e) { loadingPanel.Show(); } " />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="mensajeayuda">
                                                    <div class="icono">
                                                    </div>
                                                    <p>
                                                        Clic <a href="../Plantillas/PlantillaImporVentas.xlsx" target="_blank">aqui</a>
                                                        para ver plantilla requerida.
                                                    </p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <ldr:Loader ID="ldrWait" runat="server" />
            <dx:ASPxGridView ID="grdLog" SettingsPager-PageSize="1000" runat="server" Width="50%"
                AutoGenerateColumns="True" Visible="true" ClientInstanceName="grdLog" Caption="Log de Errores">
                <SettingsPager PageSize="1000">
                </SettingsPager>
                <Settings ShowHeaderFilterButton="true" ShowFilterBar="Visible" ShowFilterRowMenu="true"
                    ShowGroupButtons="true" ShowGroupPanel="True" />
                <Settings ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True"
                    ShowFilterBar="Visible"></Settings>
            </dx:ASPxGridView>
            <msgp:MensajePopUp ID="mensajero" runat="server" />
            <dx:ASPxLoadingPanel ID="loadingPanel" runat="server"  CssClass="modalBackground" ClientInstanceName="loadingPanel"
                Modal="True">
            </dx:ASPxLoadingPanel>
        </div>

        <%-- </dx:PanelContent>--%>
        <%--</PanelCollection>
            <LoadingDivStyle CssClass="modalBackground">
            </LoadingDivStyle>
        </dx:ASPxCallbackPanel>--%>


    </form>
</body>
</html>

