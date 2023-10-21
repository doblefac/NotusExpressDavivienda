<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministracionClientes.aspx.vb" Inherits="NotusExpress.AdministracionClientes" %>

<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>

<script type="text/javascript" src="../Scripts/jquery-1.12.4.js"></script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Administración Clientes::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
       

        function CargaCompleta(e) {
            if (e.errorText.length > 0) {
                if (e.errorText.indexOf('Violation') >= 0) {
                    alert('No fue posible cargar el archivo, por favor verifique que el mismo no se encuentre abierto e intente nuevamente.');
                } else {
                    alert(e.errorText);
                }
                LoadingPanel.Hide();
            }
        }

        function ProcesarCarga(s, e) {
            if (s.cpMensaje != null) {
                $('#divEncabezado').html(s.cpMensaje);
            }

            if (e.callbackData != null) {
                $('#divFileContainer').html(e.callbackData);
            }

            if (s.cpResultadoProceso != null) {
                if (s.cpResultadoProceso == 0) {
                    $('#divReporte').css('visibility', 'visible');
                } else {
                    if (s.cpResultadoProceso == 1 || s.cpResultadoProceso == 2 || s.cpResultadoProceso == 10 || s.cpResultadoProceso == 20 || s.cpResultadoProceso == 30 || s.cpResultadoProceso == 40) {
                        gvErrores.PerformCallback();
                        TamanioVentana();
                        cmbCampania.SetValue('');
                        dialogoErrores.SetSize(myWidth * 0.6, myHeight * 0.6);
                        dialogoErrores.ShowWindow();
                    }
                }
            }
            LoadingPanel.Hide();
        }

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
                LoadingPanel.Show();
                callback.PerformCallback(parametro + ':' + valor);
            }
        }

        function DescargarReporte() {
            window.location.href = 'DescargaDocumento.aspx?id=1';
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
        <div id="divEncabezado">
            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        </div>
        <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
            <ClientSideEvents EndCallback="function(s,e){ 
                ActualizarEncabezado(s,e);
                 ProcesarCarga(s, e);
            }"></ClientSideEvents>
            <PanelCollection>
                <dx:PanelContent>
                    <dx:ASPxCallback ID="callback" runat="server" ClientInstanceName="callback">
                        <ClientSideEvents EndCallback="function (s,e){
                    }" />
                    </dx:ASPxCallback>
                    <dx:ASPxRoundPanel ID="rpAdministradorCliente" runat="server" HeaderText="Administración de Clientes"
                        Width="40%">
                        <PanelCollection>
                            <dx:PanelContent>
                                <table cellpadding="1" width="100%">
                                    <tr>
                                        <td class="field">Campaña:
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cmbCampania" runat="server" ValueType="System.Int32" AutoPostBack="False"
                                                ClientInstanceName="cmbCampania" TabIndex="1" Width="100%" ValueField="IdCampania">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="Nombre" Width="300px" Caption="Descripción" />
                                                </Columns>
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroCliente">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Archivo:
                                        </td>
                                        <td>
                                            <div id="cargarArchivo">

                                                <asp:FileUpload ID="fuArchivo" runat="server" />
                                                <asp:Label ID="lblObligatorio" runat="server" ForeColor="Red" Text="*" Width="50px" />
                                                <div>
                                                    <asp:RegularExpressionValidator ID="revArchivo" runat="server"
                                                        CssClass="listSearchTheme" ErrorMessage="Formato del archivo incorrecto<br/>" ControlToValidate="fuArchivo" Display="Dynamic"
                                                        ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.xls|.xlsx|.XLSX|.XLS|.Xlsx|.Xls)$" ValidationGroup="validacion"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvArchivo" runat="server" ErrorMessage="Es necesario seleccionar un archivo."
                                                        ControlToValidate="fuArchivo" Display="Dynamic" ValidationGroup="validacion" />
                                                </div>
                                                <div class="comentario" style="font-size: small">Cargar archivos con extensión (xls o xlsx)</div>

                                            </div>
                                            <asp:LinkButton ID="lbVerArchivo" runat="server">(Ver Archivo Ejemplo)</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <div style="float: inherit; margin-top: 5px; margin-bottom: 5px;">
                                                <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="false" Text="Procesar archivo"
                                                    ClientInstanceName="btnUpload" ClientEnabled="true">
                                                    <ClientSideEvents Click="function(s, e) { 
                                                        if(ASPxClientEdit.ValidateGroup('registroCliente')){
                                                            var seleccion = cmbCampania.GetValue();
                                                            LoadingPanel.Show(); 
                                                           
                                                          }
                                                        }"></ClientSideEvents>
                                                    <Image Url="../img/upload.png">
                                                    </Image>
                                                </dx:ASPxButton>
                                            </div>
                                            <div style="float: left; margin-left: 5px; margin-bottom: 5px; margin-top: 5px;">
                                                <fieldset>
                                                    <div id="divFileContainer" style="width: auto">
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </td>
                                        <td>
                                            <dx:ASPxHyperLink ID="imgDescarga" runat="server" ImageUrl="../img/MSExcel.png" ToolTip="Maestro Ciudades"
                                                Cursor="pointer">
                                                <ClientSideEvents Click="function (s, e){
                                                    DescargarReporte();
                                                }" />
                                            </dx:ASPxHyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <dx:ASPxPopupControl ID="pcErrores" runat="server" ClientInstanceName="dialogoErrores" ScrollBars="Auto"
                        HeaderText="Resultado Consulta - Log de Errores" AllowDragging="true" Width="400px" Height="200px"
                        Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="pccError" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxComboBox ID="cbFormatoExportar" runat="server" ShowImageInEditBox="true"
                                                SelectedIndex="-1" ValueType="System.String" EnableCallbackMode="true" AutoResizeWithContainer="true"
                                                AutoPostBack="false"  ClientInstanceName="cbFormatoExportar"
                                                Width="250px">
                                                <Items>
                                                    <dx:ListEditItem ImageUrl="../img/excel.gif" Text="Exportar a XLS" Value="xls"
                                                        Selected="true" />
                                                    <dx:ListEditItem ImageUrl="../img/pdf.png" Text="Exportar a PDF" Value="pdf" />
                                                    <dx:ListEditItem ImageUrl="../img/xlsx_win.png" Text="Exportar a XLSX" Value="xlsx" />
                                                    <dx:ListEditItem ImageUrl="../img/csv.png" Text="Exportar a CSV" Value="csv" />
                                                </Items>
                                                <Buttons>
                                                    <dx:EditButton Text="Exportar" ToolTip="Exportar Reporte al formato seleccionado">
                                                        <Image Url="../img/upload.png">
                                                        </Image>
                                                    </dx:EditButton>
                                                </Buttons>
                                                <ValidationSettings ErrorText="Formato a exportar requerido" RequiredField-ErrorText="Formato a exportar requerido"
                                                    Display="Dynamic" CausesValidation="true" ValidateOnLeave="true" ValidationGroup="exportar">
                                                    <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular"></RegularExpression>
                                                    <RequiredField IsRequired="true" ErrorText="Formato a exportar requerido" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                                <dx:ASPxGridView ID="gvErrores" runat="server" AutoGenerateColumns="true" ClientInstanceName="gvErrores">
                                    <SettingsPager PageSize="10">
                                    </SettingsPager>
                                </dx:ASPxGridView>
                                <dx:ASPxGridViewExporter ID="gveErrores" runat="server" GridViewID="gvErrores">
                                </dx:ASPxGridViewExporter>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true">
                    </dx:ASPxLoadingPanel>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
    </form>
</body>
</html>
