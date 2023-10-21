<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CargueSerialesPorArchivo.aspx.vb"
    Inherits="NotusExpress.CargueSerialesPorArchivo" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="uc1" TagName="ValidacionURL" Src="~/ControlesDeUsuario/ValidacionURL.ascx" %>
<%@ Register TagPrefix="uc2" TagName="EncabezadoPagina" Src="~/ControlesDeUsuario/EncabezadoPagina.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts\jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Uploader_OnUploadStart() {
            btnUpload.SetEnabled(false);
            btProcesar.SetEnabled(false);
        }
        function UpdateUploadButton() {
            btnUpload.SetEnabled(uploader.GetText(0) != "");

        }
        function UpdateUploadButtonbtProcesar() {
            btProcesar.SetEnabled(btProcesar.GetText(0) != "");

            //            if (btnUpload.ClientEnabled = true) {
            //                btProcesar.SetEnabled(true);
            //            } else {
            //                btProcesar.SetEnabled(false);
            //            }
        }
        function Uploader_OnFileUploadComplete(s, e) {
            document.getElementById('divpnlErrores').innerHTML = s.cprpLogErrores;
        }
        function Uploader_OnFilesUploadComplete(args) {
            UpdateUploadButton();
        }
        function btProcesar_OnFilesUploadComplete(args) {
            UpdateUploadButtonbtProcesar();
        }
        function Uploader_OnFilesUploadCompletebtProcesar(args) {
            UpdateUploadButtonbtProcesar();
        }
        function ProcesarCarga(s, e) {
            if (s.cpMensaje != null) {
                $('#divEncabezado').html(s.cpMensaje);
            }

            if (e.callbackData != null) {
                $('#divFileContainer').html(e.callbackData);
            }
        }

        //        function VerEjemplo(ctrl) {
        //            var newWindow = window.open("../img/EjemploCargueSeriales.JPG", "EjemploArchivo", "toolbar=0,location=0,menubar=0,scrollbars=1;resizable=1,status=0;titlebar=0");
        //            try {
        //                newWindow.document.title = "Archivo de Ejemplo";
        //            } catch (e) { }
        //        }

        $(document).ready(function () {
            $("#VerEjemplo").click(function () {
                if ($(this).text().trim() == "(Ver archivo Ejemplo)") {
                    $("#imagenEjemplo").slideDown();
                    $(this).text('(Ocultar Ejemplo)')
                } else {
                    $("#imagenEjemplo").slideUp();
                    $(this).text('(Ver archivo Ejemplo)');
                }
            });
        });

        $(document).ready(function () {
            $("#VerEjemplo2").click(function () {
                if ($(this).text().trim() == "(Ver Ejemplo)") {
                    $("#imagen").slideDown();
                    $(this).text('(Ocultar Ejemplo)')
                } else {
                    $("#imagen").slideUp();
                    $(this).text('(Ver Ejemplo)');
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hfControlFiltroCiudad" runat="server" />
        <uc1:ValidacionURL ID="vuControlSesion" runat="server" />
        <uc2:EncabezadoPagina ID="miEncabezado" runat="server" />
    </div>
    <div>
        <dx:ASPxRoundPanel ID="rpAdicionPlan" runat="server" HeaderText="Cargue de archivos"
            Width="100%">
            <PanelCollection>
                <dx:PanelContent>
                    <table cellpadding="1" width="90%">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="BtnExportarProductoBodega" runat="server" Text="Exportar Productos y Bodegas">
                                </dx:ASPxButton>
                               <br />
                                <dx:ASPxUploadControl ID="ucCargueArchivoRadicados" runat="server" ClientInstanceName="uploader"
                                    ShowProgressPanel="True" NullText="Haga clic aquí para buscar el archivos..."
                                    OnFileUploadComplete="CargarInformacion" Size="35">
                                    <ClientSideEvents FileUploadComplete="function(s, e)  { Uploader_OnFileUploadComplete(s, e); } "
                                        FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); Uploader_OnFilesUploadCompletebtProcesar(e); } "
                                        FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }" TextChanged="function(s, e) { UpdateUploadButton(); }">
                                    </ClientSideEvents>
                                    <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".Xls,.Xlsx" GeneralErrorText="La carga de archivos ha fallado debido a un error externo"
                                        MaxFileSizeErrorText="Tamaño del archivo supera el tamaño máximo permitido, que es de {0} bytes"
                                        MultiSelectionErrorText="¡Atención! Los siguientes {0} archivos no son válidos porque superan el tamaño de archivo permitido ({1}) o sus extensiones no están permitidos. 
                                                    Estos archivos se han eliminado de la selección, por lo que no se cargarán.
                                                    {2}" NotAllowedFileExtensionErrorText="Esta extensión de archivo no está permitido">
                                    </ValidationSettings>
                                </dx:ASPxUploadControl>
                                <div>
                                    <a href="javascript:void(0);" id="VerEjemplo"><font color="#0000ff">(Ver archivo Ejemplo)</font></a>
                                    <div id="imagenEjemplo" style="display: none;">
                                        <img id="seriales" alt="Ver Ejemplo" src="../img/EjemploCargueSeriales.JPG" name="seriales" />
                                    </div>
                                </div>
                                <div style="float: left; margin-top: 5px; margin-bottom: 5px;">
                                    <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Cargar archivo"
                                        ClientInstanceName="btnUpload" ClientEnabled="False">
                                        <ClientSideEvents Click="function(s, e) { uploader.Upload(); }  "></ClientSideEvents>
                                        <Image Url="../img/old-go-top.png" />
                                    </dx:ASPxButton>
                                </div>
                                <div style="float: left; margin-left: 5px; margin-bottom: 5px; margin-top: 5px;">
                                    <fieldset>
                                        <div id="divFileContainer" style="width: auto">
                                        </div>
                                    </fieldset>
                                </div>
                                <div style="float: left; margin-top: 5px; margin-bottom: 5px;">
                                    <dx:ASPxButton ID="btProcesar" runat="server" AutoPostBack="False" Text="Procesar"
                                        ClientEnabled="False" ClientInstanceName="btProcesar">
                                        <Image Url="../img/Running_process.png" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="float: left; margin-top: 5px; margin-bottom: 5px;">
                                    <dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar Campos">
                                        <Image Url="../img/eraserminus.png" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="divpnlErrores">
                        <dx:ASPxGridViewExporter ID="gveExportador" runat="server" GridViewID="gvErrores">
                        </dx:ASPxGridViewExporter>
                        <dx:ASPxRoundPanel ID="rpLogErrores" runat="server" Visible="false" Width="90%" ScrollBars="Auto"
                            HeaderText="Log de Errores">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxButton ID="btnExportar" runat="server" Text="Exportar Errores a Excel">
                                    </dx:ASPxButton>
                                    <dx:ASPxGridView ID="gvErrores" runat="server" AutoGenerateColumns="False" Width="100%"
                                        GroupSummarySortInfo="False">
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Linea Archivo" FieldName="lineaArchivo" ShowInCustomizationForm="False"
                                                VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Serial" FieldName="serial" ShowInCustomizationForm="False"
                                                VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Descripción" FieldName="descripcion" ShowInCustomizationForm="False"
                                                VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsPager PageSize="20">
                                        </SettingsPager>
                                    </dx:ASPxGridView>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>
                    <div id="divPanelResultado">
                        <dx:ASPxGridViewExporter ID="gveExportadorResultado" runat="server" GridViewID="gvSerialcargado">
                        </dx:ASPxGridViewExporter>
                        <dx:ASPxRoundPanel ID="rpResultado" runat="server" Visible="false" Width="90%" HeaderText="Registros cargados">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <div>
                                        <table width="30%" align="left">
                                            <tr>
                                                <td align="right">
                                                    Seriales Cargados:
                                                </td>
                                                <td align="left">
                                                    <dx:ASPxLabel ID="lbInfogeneral" runat="server" Text='<%# Eval("Infogeneral") %>'
                                                        Font-Size="10pt">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="80%" align="left">
                                            <tr align="center">
                                                <th align="center">
                                                    Seriales ya cargados
                                                </th>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="btnExportarResul" runat="server" Text="Exportar Resultado a Excel">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td>
                                                    <dx:ASPxGridView ID="gvSerialcargado" runat="server" AutoGenerateColumns="False"
                                                        Width="90%" GroupSummarySortInfo="False">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Linea Archivo" FieldName="lineaArchivo" Visible="False"
                                                                ShowInCustomizationForm="False" VisibleIndex="0">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Serial" FieldName="serial" ShowInCustomizationForm="False"
                                                                VisibleIndex="1">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Descripción" FieldName="descripcion" ShowInCustomizationForm="False"
                                                                VisibleIndex="2">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsPager PageSize="20">
                                                        </SettingsPager>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </div>
    </form>
</body>
</html>
