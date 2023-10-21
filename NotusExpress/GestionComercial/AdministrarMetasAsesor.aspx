<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministrarMetasAsesor.aspx.vb"
    Inherits="NotusExpress.AdministrarMetasAsesor" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Administrar Metas Comerciales - Por Asesor</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function EjecutarCallbackGeneral(s, e, parametro) {
            if (ASPxClientEdit.AreEditorsValid()) {
                loadingPanel.Show();
                cpGeneral.PerformCallback(parametro);
            }
        }

        function toggle(control) {
            $("#" + control).slideToggle("slow");
        }

        function OnExpandCollapseButtonClick(s, e) {
            var isVisible = pnlFiltros.GetVisible();
            s.SetText(isVisible ? "+" : "-");
            pnlFiltros.SetVisible(!isVisible);
        }

        function ActivarOInactivarFiltros() {
            rpFiltros.SetEnabled(!pucNuevaMeta.IsVisible());
        }

        function LimpiarFormularioRegistro() {
            txtNombreMeta.SetText("");
            txtAnio.SetText("");
            cbMes.SetSelectedIndex(-1);
            cbCampania.SetSelectedIndex(-1);
            cbAsesor.SetSelectedIndex(-1);
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
                if (e.isValid) {
                    e.isValid = (valor >= 0 && valor <= 100) ? true : false;
                }
                e.errorText = "Meta no válida. Se espera un número entero mayor o igual que cero (0) ó un numero entero menor o igual a cien (100).";
            } else {
                e.errorText = "Meta requerida";
                e.isValid = false;
            }
        }

    </script>
    <style type="text/css">
        .auto-style2 {
            width: 530px;
        }
    </style>
</head>
<body>
    <form id="frmAdmMeta" runat="server">
        <dx:ASPxHiddenField ID="hfDimensiones" ClientInstanceName="hfDimensiones" runat="server">
        </dx:ASPxHiddenField>
        <vu:ValidacionURL ID="vuControlSesion" runat="server" />
        <div id="divEncabezado">
            <uc1:encabezadopagina id="miEncabezado" runat="server" />
        </div>
        <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
            <ClientSideEvents EndCallback="function(s,e){ 
                $('#divEncabezado').html(s.cpMensaje);
                loadingPanel.Hide();
                ActivarOInactivarFiltros(); 
            }" />
            <PanelCollection>
                <dx:PanelContent>
                    <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="" ClientInstanceName="rpFiltros"
                        DefaultButton="btnBuscar">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="white-space: nowrap;" align="left">Filtros de B&uacute;squeda
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
                                                    <td class="field">Agente:&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="cbFiltroAsesor" runat="server" ClientInstanceName="cbFiltroAsesor"
                                                            IncrementalFilteringMode="Contains" Width="300px">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="docIdentificacion" Width="150px" Caption="Documento" />
                                                                <dx:ListBoxColumn FieldName="nombreAsesor" Width="250px" Caption="Nombre" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td class="field">Campaña:&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="cbFiltroCampania" runat="server" ClientInstanceName="cbFiltroCampania"
                                                            IncrementalFilteringMode="Contains" Width="350px">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="padding-top: 8px">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="white-space: nowrap;" align="center" class="auto-style2">
                                                                    <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline!important;"
                                                                        AutoPostBack="false" ValidationGroup="Filtrado" HorizontalAlign="Justify">
                                                                        <Image Url="~/img/find.png">
                                                                        </Image>
                                                                        <ClientSideEvents Click="function(s, e) { 
                                                                            if (cbFiltroAsesor.GetValue()==null && cbFiltroCampania.GetValue()==null){
                                                                                alert('Debe seleccionar minimo uno de los dos campos de filtro para continuar con la busqueda.');
                                                                            } else {
                                                                                EjecutarCallbackGeneral(s,e,'filtrarDatos');
                                                                            }
                                                                        }" />
                                                                    </dx:ASPxButton>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;<dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar"
                                                                        Style="display: inline!important;" AutoPostBack="false" HorizontalAlign="Justify">
                                                                        <Image Url="~/img/edit-clear.png">
                                                                        </Image>
                                                                        <ClientSideEvents Click="function(s, e) { 
                                                                            cbFiltroAsesor.SetSelectedIndex(-1); 
                                                                            cbFiltroCampania.SetSelectedIndex(-1); 
                                                                        }" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td align="right">
                                                                    <dx:ASPxButton ID="btnNuevo" runat="server" Text="Crear Nueva Meta" AutoPostBack="false" Width="177px">
                                                                        <ClientSideEvents Click="function(s,e){ 
                                                                            LimpiarFormularioRegistro(); 
                                                                            pucNuevaMeta.Show(); 
                                                                        }" />
                                                                        <Image Url="~/img/new.png">
                                                                        </Image>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
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
                    <dx:ASPxGridView ID="gvListaMetas" runat="server" AutoGenerateColumns="False" KeyFieldName="IdMeta" Width="80%">
                        <ClientSideEvents EndCallback="ActualizarEncabezado" />
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Asesor" FieldName="Asesor" Name="fieldAsesor"
                                VisibleIndex="0" ReadOnly="true">
                                <EditFormSettings VisibleIndex="0" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Campaña" FieldName="Campania" Name="fieldCampania"
                                VisibleIndex="1" ReadOnly="true">
                                <EditFormSettings VisibleIndex="1" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Año" FieldName="Anio" Name="fieldAnio" VisibleIndex="2"
                                ReadOnly="true">
                                <EditFormSettings VisibleIndex="2" />
                                <PropertiesTextEdit NullDisplayText="" Width="70px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" Display="Dynamic" SetFocusOnError="True"
                                            ErrorTextPosition="Bottom" EnableCustomValidation="true" 
                                            ErrorText="Año no válido. Se espera un n&uacute;mero entero mayor que cero (0)">
                                            <RequiredField IsRequired="true" ErrorText="Año requerido" />
                                    </ValidationSettings>
                                    <ClientSideEvents Validation="EsAnioValido" />
                                </PropertiesTextEdit>
                                <HeaderStyle HorizontalAlign="Center" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Mes" FieldName="Mes" Name="fieldMes" VisibleIndex="3"
                                ReadOnly="true">
                                <EditFormSettings VisibleIndex="3" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Meta" FieldName="Meta" Name="fieldMeta" VisibleIndex="4">
                                <EditFormSettings VisibleIndex="4" />
                                <PropertiesTextEdit NullDisplayText="" Width="70px" Style-HorizontalAlign="Left">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" Display="Dynamic" SetFocusOnError="True"
                                        ErrorTextPosition="Bottom" EnableCustomValidation="true" 
                                        ErrorText="Meta no válida. Se espera un n&uacute;mero entero mayor o igual que cero (0)">
                                        <RequiredField IsRequired="true" ErrorText="Meta requerida" />
                                    </ValidationSettings>
                                    <ClientSideEvents Validation="EsMetaValida" />
                                    <Style HorizontalAlign="Left"></Style>
                                </PropertiesTextEdit>
                                <HeaderStyle HorizontalAlign="Center" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataCheckColumn Caption="Activo" ShowInCustomizationForm="true" VisibleIndex="5"
                                    FieldName="Estado">
                                <HeaderStyle HorizontalAlign="Center" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewCommandColumn VisibleIndex="5" Caption="Opción" ButtonType="Image" Width="30px">
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewCommandColumn>
                        </Columns>
                        <Settings ShowTitlePanel="True" ShowHeaderFilterButton="True" ShowHeaderFilterBlankItems="False" />
                        <SettingsText Title="Listado de Metas Registradas" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
                        <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1" />
                        <SettingsPopup>
                            <EditForm Width="50%" Modal="true" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" />
                        </SettingsPopup>
                        <StylesEditors>
                            <ReadOnlyStyle ForeColor="Gray" BackColor="LightGray">
                            </ReadOnlyStyle>
                            <ReadOnly ForeColor="Gray">
                            </ReadOnly>
                        </StylesEditors>
                    </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="gveExportador" runat="server" GridViewID="gvListaMetas">
                    </dx:ASPxGridViewExporter>
                    <br />
                    <dx:ASPxPopupControl ID="pucNuevaMeta" runat="server" HeaderText="Crear Nueva Meta" Width="475px"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Modal="True"  
                        ClientInstanceName="pucNuevaMeta" ScrollBars="Auto" ShowMaximizeButton="false"
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
                                                            <dx:LayoutItem Caption="Agente" HelpText="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer5" runat="server">
                                                                        <dx:ASPxComboBox ID="cbAsesor" runat="server" ClientInstanceName="cbAsesor" Width="340px" IncrementalFilteringMode="Contains">
                                                                            <ClientSideEvents EndCallback="ActualizarEncabezado" />
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                                RequiredField-ErrorText="Asesor requerido" ErrorDisplayMode="ImageWithTooltip">
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="Campaña" HelpText="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server">
                                                                        <dx:ASPxComboBox ID="cbCampania" runat="server" ClientInstanceName="cbCampania" IncrementalFilteringMode="Contains"
                                                                            Width="340px">
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                                RequiredField-ErrorText="Campaña requerido" ErrorDisplayMode="ImageWithTooltip">
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
                                                                                RequiredField-ErrorText="Año requerido" ErrorDisplayMode="ImageWithTooltip">
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
                                                                        <dx:ASPxComboBox ID="cbMes" runat="server" ClientInstanceName="cbMes" IncrementalFilteringMode="Contains">
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                                RequiredField-ErrorText="Mes requerido" ErrorDisplayMode="ImageWithTooltip">
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
                                                                            ClientInstanceName="txtNombreMeta" MaxLength="3">
                                                                                <ClientSideEvents LostFocus ="function(s,e){
                                                                                    if (txtNombreMeta.GetText() != ''){
                                                                                        var meta = txtNombreMeta.GetText()
                                                                                        if ( meta > 100){
                                                                                            alert('El valor de la meta no puede ser mayor a 100');
                                                                                            txtNombreMeta.SetText('');
                                                                                            txtNombreMeta.SetFocus();
                                                                                        }  else if (meta < 1) { 
                                                                                            alert('El valor de la meta debe ser mayor a 0');
                                                                                            txtNombreMeta.SetText('');
                                                                                            txtNombreMeta.SetFocus();                                                                        
                                                                                        }
                                                                                    }
                                                                                }" />
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ValidationGroup="Registro"
                                                                                RequiredField-ErrorText="Meta requerido" ErrorDisplayMode="ImageWithTooltip">
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
                    <msgp:MensajePopUp ID="mensajero" runat="server" />
                </dx:PanelContent>
            </PanelCollection>
            <LoadingDivStyle CssClass="modalBackground">
            </LoadingDivStyle>
        </dx:ASPxCallbackPanel>
        <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
            Modal="True">
        </dx:ASPxLoadingPanel>
            <div id="bluebar" class="menuFlotante">
        <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
        </b></b>
        <table style="width: 100%;">
            <tr>
                <td>
                    <dx:ASPxComboBox ID="cbFormatoExportar" runat="server" ShowImageInEditBox="true"
                        SelectedIndex="-1" ValueType="System.String" EnableCallbackMode="True" AutoResizeWithContainer="true"
                         ClientInstanceName="cbFormatoExportar" Width="210px">
                        <Items>
                            <dx:ListEditItem ImageUrl="~/img/excel.gif" Text="Exportar a XLS" Value="xls" Selected="true" />
                            <dx:ListEditItem ImageUrl="~/img/xlsx_win.png" Text="Exportar a XLSX" Value="xlsx" />
                            <dx:ListEditItem ImageUrl="~/img/csv.png" Text="Exportar a CSV" Value="csv" />
                        </Items>
                        <Buttons>
                            <dx:EditButton Text="Exportar" ToolTip="Exportar Reporte al formato seleccionado">
                                <Image Url="~/img/Download.png">
                                </Image>
                            </dx:EditButton>
                        </Buttons>
                        <ValidationSettings ErrorText="Formato a exportar requerido" RequiredField-ErrorText="Formato a exportar requerido"
                            Display="Dynamic" CausesValidation="true" ValidateOnLeave="true" ValidationGroup="exportar">
                            <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular">
                            </RegularExpression>
                            <RequiredField IsRequired="true" ErrorText="Formato a exportar requerido" />
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </td>
            </tr>
        </table> 
    </div> 
    <div id="div1" style="float: right; visibility: visible; margin-right: 5px; margin-bottom: 5px;
        margin-top: 5px; width: 2%; position: fixed; overflow: hidden; display: block;
        bottom: 0px">
        <table>
            <tr>
                <td align="right">
                    <a style="color: Black; font-size: 15px; cursor: hand; cursor: pointer;" id="a1"
                        onclick="toggle('bluebar');">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/structure.png" ToolTip="Ocultar/Mostrar, Menú "
                            Width="16px" /></a>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
