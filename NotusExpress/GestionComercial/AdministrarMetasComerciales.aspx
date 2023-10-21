<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministrarMetasComerciales.aspx.vb"
    Inherits="NotusExpress.AdministrarMetasComerciales" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Administrar Metas Comerciales</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        $(document).ready(function () { $(window).resize(function () { ObtenerDimensionesVentana(); }) });

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
            rpFiltros.SetEnabled(!pucNuevaMeta.IsVisible());
        }

        function LimpiarFormulario() {
            txtNombreMeta.SetText("");
            txtAnio.SetText("");
            cbMes.SetSelectedIndex(-1);
            cbEstrategia.SetSelectedIndex(0);
            cbPdv.SetSelectedIndex(0);
            cbTipoProducto.SetSelectedIndex(-1);
        }

        function ObtenerDimensionesVentana() {
            var dimensiones = GetWindowSize().split(";");
            var ancho = Math.min(600, dimensiones[1] * 0.7);
            var alto = Math.min(500, dimensiones[0] * 0.95);
            hfDimensiones.Set("alto", alto);
            hfDimensiones.Set("ancho", ancho);

            pucNuevaMeta.SetHeight(alto);
            pucNuevaMeta.SetWidth(ancho);

            pucErrores.SetWidth(dimensiones[1] * 0.95);
            pucErrores.SetHeight(dimensiones[0] * 0.95);
        }

        function OnUploadStart() {
            _aspxGetElementById("contenedorArchivo").innerHTML = "";
            btnCargar.SetEnabled(false);
        }

        function OnTextChanged(s, e) {
            s.Upload();
        }

        function OnFileUploadComplete(args) {
            if (args.isValid) { AdicionarArchivoAContenedor(args.callbackData); }
            btnCargar.SetEnabled(args.isValid);
        }

        function AdicionarArchivoAContenedor(callbackData) {
            var data = callbackData;
            var label = document.createElement('span');
            _aspxGetElementById("contenedorArchivo").innerHTML = callbackData;
        }

        function LimpiarArchivoCargado(s, e) {
            if (s.cpLimpiarArchivo) {
                _aspxGetElementById("contenedorArchivo").innerHTML = "";
                btnCargar.SetEnabled(false);
            }
        }

        function EsAnioValido(s, e) {
            var valor = s.GetValue();
            var expReg = /^\s*[0-9]{1,4}\s*$/;
            if (valor) {
                e.isValid = expReg.test(valor);
                if (e.isValid) { e.isValid = valor > 0 ? true : false; }
                e.errorText = "Año no válido. Se espera un número entero mayor que cero (0)";
            } else { e.isValid = true; }
        }

        function EsMetaValida(s, e) {
            var valor = s.GetValue();
            var expReg = /^\s*[0-9]{1,4}\s*$/;
            if (valor && valor != "") {
                e.isValid = expReg.test(valor);
                if (e.isValid) { e.isValid = valor >= 0 ? true : false; }
                e.errorText = "Meta no válida. Se espera un número entero mayor o igual que cero (0)";
            } else {
                e.errorText = "Meta requerida";
                e.isValid = false;
            }
        }

        function LimpiarFiltros() {
            btnCargar.SetEnabled(false);
            cbFiltroPdv.SetSelectedIndex(-1);
            cbFiltroEstrategia.SetSelectedIndex(-1);
            cbFiltroProducto.SetSelectedIndex(-1);
        }
    </script>
</head>
<body onload="ObtenerDimensionesVentana();">
    <form id="frmAdmMeta" runat="server">
    <dx:ASPxHiddenField ID="hfDimensiones" ClientInstanceName="hfDimensiones" runat="server">
    </dx:ASPxHiddenField>
    <vu:ValidacionURL ID="vuControlSesion" runat="server" />
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        <br />
    </div>
    <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="" ClientInstanceName="rpFiltros"
        DefaultButton="btnBuscar">
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
                                        Punto de Venta:
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbFiltroPdv" runat="server" ClientInstanceName="cbFiltroPdv"
                                            IncrementalFilteringMode="StartsWith" Width="300px">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td class="field">
                                        Estrategia:
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbFiltroEstrategia" runat="server" ClientInstanceName="cbFiltroEstrategia"
                                            IncrementalFilteringMode="StartsWith" Width="350px">
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Producto:
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cbFiltroProducto" runat="server" ClientInstanceName="cbFiltroProducto"
                                            IncrementalFilteringMode="StartsWith" Width="250px">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="padding-top: 8px">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="white-space: nowrap; width: 230px" align="left">
                                                    <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline-table!important;"
                                                        AutoPostBack="false" ValidationGroup="Filtrado">
                                                        <Image Url="~/img/find.png">
                                                        </Image>
                                                        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'filtrarDatos');}" />
                                                    </dx:ASPxButton>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar"
                                                        Style="display: inline-table!important;" AutoPostBack="false">
                                                        <Image Url="~/img/edit-clear.png">
                                                        </Image>
                                                        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'limpiarFiltros'); LimpiarFiltros();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="center">
                                                    <dx:ASPxButton ID="btnNuevo" runat="server" Text="Crear Nueva Meta" AutoPostBack="false">
                                                        <ClientSideEvents Click="function(s,e){ LimpiarFormulario(); pucNuevaMeta.Show(); }" />
                                                        <Image Url="~/img/new.png">
                                                        </Image>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td style="padding-left: 5px;" align="right">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxUploadControl ID="uplArchivo" runat="server" Size="50" Style="display: inline!important;"
                                                                    ClientInstanceName="uplArchivo" NullText="Haga click aquí para buscar archivos...">
                                                                    <ClientSideEvents FileUploadComplete="function(s, e) { OnFileUploadComplete(e); }"
                                                                        FileUploadStart="function(s, e) { OnUploadStart(); }" TextChanged="OnTextChanged">
                                                                    </ClientSideEvents>
                                                                    <ValidationSettings AllowedFileExtensions=".xls,.xlsx" GeneralErrorText="Falló el cargue del archivo debido a un error interno. Por favor intente nuevamente"
                                                                        MaxFileSizeErrorText="El tamaño del archivo excede el tamaño máximo permitido, que es {0} bytes"
                                                                        NotAllowedFileExtensionErrorText="Esta extensión de archivo no está permitida"
                                                                        MaxFileSize="10485760">
                                                                    </ValidationSettings>
                                                                </dx:ASPxUploadControl>
                                                                <div id="contenedorArchivo" class="contenedorArchivo">
                                                                </div>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                                <dx:ASPxButton ID="btnCargar" runat="server" Text="Cargar Archivo" Style="display: inline!important;"
                                                                    AutoPostBack="false" ClientInstanceName="btnCargar" ClientEnabled="False" ValidationGroup="procesarArchivo">
                                                                    <Image Url="~/img/Upload.png">
                                                                    </Image>
                                                                    <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'registrarDatosDeArchivo');}" />
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="right">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxButton ID="btnVerEjemplo" runat="server" Text="Ver Archivo Ejemplo" Wrap="False"
                                                                    Style="display: inline-table!important;">
                                                                    <Image Url="~/img/Excel.gif">
                                                                    </Image>
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td>
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <dx:ASPxButton ID="btnPlantilla" runat="server" Text="Descargar Datos Guía" Wrap="False"
                                                                    Style="display: inline-table!important;">
                                                                    <Image Url="~/img/Excel.gif">
                                                                    </Image>
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
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
        <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e); ActivarOInactivarFiltros(); LimpiarArchivoCargado(s,e);}" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="gvListaMetas" runat="server" AutoGenerateColumns="False" KeyFieldName="IdMeta">
                    <ClientSideEvents EndCallback="ActualizarEncabezado" />
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0">
                            
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Estrategia" FieldName="Estrategia" Name="fieldEstrategia"
                            VisibleIndex="1" ReadOnly="true">
                            <EditFormSettings VisibleIndex="0" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Punto de Venta" FieldName="PuntoDeVenta" Name="fieldPuntoDeVenta"
                            VisibleIndex="2" ReadOnly="true">
                            <EditFormSettings VisibleIndex="1" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Producto" FieldName="TipoProducto" Name="fieldTipoProducto"
                            VisibleIndex="3" ReadOnly="true">
                            <EditFormSettings VisibleIndex="2" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Año" FieldName="Anio" Name="fieldAnio" VisibleIndex="4"
                            ReadOnly="true">
                            <EditFormSettings VisibleIndex="3" />
                            <PropertiesTextEdit NullDisplayText="" Width="50px">
                                <ValidationSettings ErrorDisplayMode="ImageWithText" Display="Dynamic" SetFocusOnError="True"
                                    ErrorTextPosition="Bottom" EnableCustomValidation="true" ErrorText="Año no válido. Se espera un n&uacute;mero entero mayor que cero (0)">
                                    <RequiredField IsRequired="true" ErrorText="Año requerido" />
                                </ValidationSettings>
                                <ClientSideEvents Validation="EsAnioValido" />
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Mes" FieldName="Mes" Name="fieldMes" VisibleIndex="5"
                            ReadOnly="true">
                            <EditFormSettings VisibleIndex="4" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Meta" FieldName="Meta" Name="fieldMeta" VisibleIndex="6">
                            <EditFormSettings VisibleIndex="5" />
                            <PropertiesTextEdit NullDisplayText="" Width="50px">
                                <ValidationSettings ErrorDisplayMode="ImageWithText" Display="Dynamic" SetFocusOnError="True"
                                    ErrorTextPosition="Bottom" EnableCustomValidation="true" ErrorText="Meta no válida. Se espera un n&uacute;mero entero mayor o igual que cero (0)">
                                    <RequiredField IsRequired="true" ErrorText="Meta requerida" />
                                </ValidationSettings>
                                <ClientSideEvents Validation="EsMetaValida" />
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False" />
                    <SettingsText CommandEdit="Editar" Title="Listado de Metas Comerciales" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
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
                <dx:ASPxPopupControl ID="pucNuevaMeta" runat="server" HeaderText="Crear Nueva Meta"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Modal="True"
                    ClientInstanceName="pucNuevaMeta" ScrollBars="Auto" ShowMaximizeButton="True"
                    ShowPageScrollbarWhenModal="True" CloseAction="CloseButton">
                    <ModalBackgroundStyle CssClass="modalBackground" />
                    <ClientSideEvents Closing="function (s,e){rpFiltros.SetEnabled(true);}" Shown="function (s,e){rpFiltros.SetEnabled(false);}" />
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <dx:ASPxPanel ID="pnlDatosMeta" runat="server" DefaultButton="btnRegistrar" TabIndex="0">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" EnableViewState="false" AlignItemCaptionsInAllGroups="True"
                                            RequiredMarkDisplayMode="RequiredOnly">
                                            <Items>
                                                <dx:LayoutGroup Caption="Informaci&oacute;n de la Meta" GroupBoxDecoration="Box">
                                                    <Items>
                                                        <dx:LayoutItem Caption="Estrategia" HelpText="">
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server">
                                                                    <dx:ASPxComboBox ID="cbEstrategia" runat="server" ClientInstanceName="cbEstrategia"
                                                                        Width="250px">
                                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){cbPdv.PerformCallback();}" />
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Estrategia requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxComboBox>
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="Punto de Venta" HelpText="">
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer5" runat="server">
                                                                    <dx:ASPxComboBox ID="cbPdv" runat="server" ClientInstanceName="cbPdv" Width="350px">
                                                                        <ClientSideEvents EndCallback="ActualizarEncabezado" />
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Punto de venta requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxComboBox>
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="Tipo de Producto" HelpText="">
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer6" runat="server">
                                                                    <dx:ASPxComboBox ID="cbTipoProducto" runat="server" ClientInstanceName="cbTipoProducto">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Producto requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxComboBox>
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="Año" HelpText="">
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer4" runat="server">
                                                                    <dx:ASPxTextBox ID="txtAnio" runat="server" Width="100px" NullText="Ingrese el año..."
                                                                        ClientInstanceName="txtAnio" MaxLength="4">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Año requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                            <RegularExpression ValidationExpression="^\s*[0-9]+\s*$" ErrorText="Año no v&aacute;lida" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="Mes" HelpText="">
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server">
                                                                    <dx:ASPxComboBox ID="cbMes" runat="server" ClientInstanceName="cbMes">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Mes requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxComboBox>
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="Meta" HelpText="">
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server">
                                                                    <dx:ASPxTextBox ID="txtNombreMeta" runat="server" Width="100px" NullText="Ingrese la meta..."
                                                                        ClientInstanceName="txtNombreMeta" MaxLength="70">
                                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                            RequiredField-ErrorText="Meta requerido" ErrorTextPosition="Bottom">
                                                                            <RequiredField IsRequired="True" />
                                                                            <RegularExpression ValidationExpression="^\s*[0-9]+\s*$" ErrorText="Meta no v&aacute;lida" />
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
                <dx:ASPxPopupControl ID="pucErrores" runat="server" HeaderText="Log de Errores" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" Modal="True" ClientInstanceName="pucErrores"
                    ScrollBars="Auto" ShowMaximizeButton="True" ShowPageScrollbarWhenModal="True"
                    CloseAction="CloseButton">
                    <ModalBackgroundStyle CssClass="modalBackground" />
                    <ClientSideEvents Closing="function (s,e){rpFiltros.SetEnabled(true);}" Shown="function (s,e){rpFiltros.SetEnabled(false);}" />
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <dx:ASPxGridView ID="gvError" runat="server" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Línea" ShowInCustomizationForm="True" VisibleIndex="0"
                                        FieldName="linea">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Descripcion Error" ShowInCustomizationForm="True"
                                        VisibleIndex="1" FieldName="descripcionError">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Dato/Columna" ShowInCustomizationForm="True"
                                        VisibleIndex="2" FieldName="dato">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager PageSize="50">
                                </SettingsPager>
                                <Settings ShowTitlePanel="True" />
                                <SettingsText Title="Listado de Errores" />
                            </dx:ASPxGridView>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <msgp:MensajePopUp ID="mensajero" runat="server" />
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
