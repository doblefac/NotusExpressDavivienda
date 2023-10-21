<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RadicacionDocumentos.aspx.vb"
    Inherits="NotusExpress.RadicacionDocumentos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Notus Express - Radicación Documentos::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function TamanioVentana() {
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible 
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }
        }

        function EjecutarCallbackGeneral(s, e, parametro, valor) {
            if (ASPxClientEdit.AreEditorsValid()) {
                loadingPanel.Show();
                cpGeneral.PerformCallback(parametro + ':' + valor);
            }
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
                return false;
            }

            return true;
        }

        function EvaluarFinCallback(s, e) {
            ActualizarEncabezado(s, e);
            if (s.cpListaRadicado != 0) {
                VerPlanilla(this, s.cpListaRadicado);
            }
        }

        function VerPlanilla(element, key) {
            TamanioVentana();
            dialogoVerPlanilla.SetContentUrl("Reportes/VisorPlanillaRadicacion.aspx?id=" + key)
            dialogoVerPlanilla.SetSize(myWidth * 0.6, myHeight * 0.9);
            dialogoVerPlanilla.ShowWindow();
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
    </div>
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
        <ClientSideEvents EndCallback="function(s,e){ EvaluarFinCallback(s, e);}" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Datos del Radicado"
                    Width="80%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td class="field">
                                        No. de Precinto:
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtPrecinto" runat="server" Width="250px" onkeypress="return solonumeros(event);"
                                            ReadOnly="False">
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                <RequiredField ErrorText ="Información Requerida" IsRequired ="true" />
                                            </ValidationSettings> 
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class ="field" align ="left" >
                                        Seleccione Documento:
                                    </td>
                                    <td>
                                        <dx:ASPxGridLookup ID="gluDocumentos" runat="server" KeyFieldName="idDocumento" SelectionMode="Multiple"
                                            IncrementalFilteringMode="StartsWith" TextFormatString="{0}" Width="250px" ClientInstanceName="gluDocumentos"
                                            MultiTextSeparator=", " AllowUserInput="false">
                                            <ClientSideEvents ButtonClick="function(s,e) {gluDocumentos.GetGridView().UnselectAllRowsOnPage(); gluDocumentos.HideDropDown(); }" />
                                            <Buttons>
                                                <dx:EditButton Text="X">
                                                </dx:EditButton>
                                            </Buttons>
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                                                <dx:GridViewDataTextColumn FieldName="descripcion" />
                                            </Columns>
                                            <GridViewProperties>
                                                <SettingsBehavior AllowDragDrop="False" EnableRowHotTrack="True" />
                                                <SettingsPager NumericButtonCount="5" PageSize="10" />
                                            </GridViewProperties>
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                <RequiredField ErrorText ="Información Requerida" IsRequired ="true" />
                                            </ValidationSettings> 
                                        </dx:ASPxGridLookup>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan ="2" align ="center">
                                        <dx:ASPxImage ID="imgRadica" runat="server" ImageUrl="../img/DxPdf32.png" ToolTip="Radicar Documentos"
                                            Cursor="pointer">
                                            <ClientSideEvents Click ="function (s, e){
                                                if(ASPxClientEdit.ValidateGroup(&#39;vgRegistro&#39;)){
                                                    if(confirm('Esta seguro que desea realizar la planilla para estos productos?')){
                                                        EjecutarCallbackGeneral(s,e,'Radicar')
                                                    }
                                                }
                                            }" />
                                        </dx:ASPxImage> 
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <dx:ASPxGridView ID="gvVentas" runat="server" AutoGenerateColumns="False" Font-Size="Small"
                                ClientInstanceName="gvVentas" KeyFieldName="IdDetalle">
                                <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e);}" />
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Id Gestión" FieldName="IdGestionVenta" VisibleIndex="0" Visible="True">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Producto" FieldName="Producto"
                                        VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tipo Producto" FieldName="TipoProducto" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Identificación Cliente" FieldName="NumeroIdentificacion" VisibleIndex="3">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager PageSize="50">
                                </SettingsPager>
                                <Settings ShowTitlePanel="True" ShowHeaderFilterButton="False" ShowHeaderFilterBlankItems="False" />
                                <SettingsText CommandEdit="Editar" Title="Servicios a radicar" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
                                <SettingsBehavior EnableCustomizationWindow="true" AutoExpandAllGroups="true" />
                                <SettingsDetail ShowDetailRow="False" />
                            </dx:ASPxGridView>
                            <br />
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:ASPxPopupControl ID="dialogoVerPlanilla" runat="server" 
    ClientInstanceName="dialogoVerPlanilla" Modal="true" CloseAction ="CloseButton" 
        HeaderText="Visualización Planilla Radicación" PopupHorizontalAlign="WindowCenter" 
    PopupVerticalAlign="WindowCenter" ScrollBars="Auto" AllowDragging="True">
        <ContentCollection>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
