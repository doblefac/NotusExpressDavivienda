<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CambioMasivoAsesor.aspx.vb"
    Inherits="NotusExpress.CambioMasivoAsesor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title>Actualización masiva de asesores</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function ProcesarCarga(s, e) {
            if (s.cpMensaje != null) {
                $('#divEncabezado').html(s.cpMensaje);
            }

            if (e.callbackData != null) {
                $('#divFileContainer').html(e.callbackData);
            }

            if (s.cpResultadoProceso != null) {
                if (s.cpResultadoProceso == 1) {
                    gvLog.PerformCallback();
                    $('#divResultadoError').css('visibility', 'visible');
                    $('#divResultadoError').css('display', 'inline');
                } else {
                    $('#divResultadoError').css('visibility', 'hidden');
                    $('#divResultadoError').css('display', 'none');
                }
            }
        }

        function UpdateUploadButton() {
            ucArchivo.Upload();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
            <PanelCollection>
                <dx:PanelContent>
                    <div id="divEncabezado">
                        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
                        <br />
                    </div>
                    <div id="divArchivo" runat="server">
                        <dx:ASPxRoundPanel ID="rpCargue" runat="server" HeaderText="Información del archivo" Width="500px">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxUploadControl ID="ucArchivo" runat="server" Width="100%" UploadMode="Advanced" Enabled="true"
                                        NullText="Seleccione un archivo..." ClientInstanceName="ucArchivo" ShowProgressPanel="true">
                                        <ClientSideEvents
                                            TextChanged="function(s, e) { UpdateUploadButton(); }"
                                            FileUploadComplete="function(s, e) { ProcesarCarga(s, e); }"
                                            FilesUploadComplete="function(s, e) 
                                            { 
                                                switch (e.errorText)
                                                    {
                                                    case '1':
                                                        alert('Tamaño de archivo no válido, el tamaño máximo es de 15 Megas');
                                                        break;
                                                    case '2':
                                                        alert('Extensión de archivo no permitida.');
                                                        break;
                                                    case '3':
                                                        alert('Carga de archivo no exitosa.');
                                                        break;
                                                    case '4':
                                                        alert('¡Atención! Los siguientes archivos no son válidos porque superan el tamaño de archivo permitido (10 Megas) o sus extensiones no están permitidas. Estos archivos se han eliminado de la selección, por lo que no se cargarán.');
                                                        break;
                                                    default:
                                                        if(e.errorText.length &gt; 1) 
                                                        { 
                                                            alert('No fue posible cargar el archivo, por favor verifique que el mismo no se encuentre abierto e intente nuevamente.'); 
                                                        } 
                                                    }
                                            }" />
                                        <ValidationSettings AllowedFileExtensions=".xls, .xlsx" MaxFileSize="15728640" MaxFileSizeErrorText="1"
                                            NotAllowedFileExtensionErrorText="2." GeneralErrorText="3" MultiSelectionErrorText="4"
                                            ShowErrors="false">
                                        </ValidationSettings>
                                    </dx:ASPxUploadControl>
                                    <asp:LinkButton ID="lbVerArchivo" runat="server">(Ver Archivo Ejemplo)</asp:LinkButton>
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <div style="float: left; margin-left: 0px; margin-bottom: 5px; margin-top: 5px;">
                                                    <fieldset style="font-style:italic">
                                                        <div id="divFileContainer" style="width: auto">
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>
                    <br />
                    <div id="divResultadoError" style="display:none; visibility:hidden">
                        <dx:ASPxRoundPanel ID="rpLog" runat="server" ClientInstanceName="rpLog" Width="600px" HeaderText="Inconsistencias en la actualización" Visible="true" HorizontalAlign="left">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxGridView ID="gvLog" runat="server" ClientInstanceName="gvLog" AutoGenerateColumns="false"
                                        KeyFieldName="id" Width="100%">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="id" Caption="Id" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                VisibleIndex="0" Width="20%">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="descripcion" Caption="Descripción" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                VisibleIndex="1">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                        <SettingsPager PageSize="5">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                        </SettingsPager>
                                        <SettingsText Title="Log de Inconsistencias" EmptyDataRow="No existen resultados para mostrar."
                                            CommandEdit="Log"></SettingsText>
                                    </dx:ASPxGridView>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
    </form>
</body>
</html>
