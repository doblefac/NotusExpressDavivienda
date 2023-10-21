<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaGestionesNoEfectivasCallCenter.aspx.vb" Inherits="NotusExpress.BusquedaGestionesNoEfectivasCallCenter" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Búsqueda Ventas No Gestionadas Call Center::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

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

        function testtimeout() {
            setTimeout("Temporizador()", 12000);
        }

        function Temporizador() {
            loadingPanel.Hide();
        }

        function Gestionar(valor) {
            testtimeout()
            loadingPanel.Show();
            TamanioVentana();
            dialogoVer.SetContentUrl("GestionVentaYServicio.aspx?id=" + valor);
            dialogoVer.SetSize(myWidth * 0.9, myHeight * 0.9);
            dialogoVer.ShowWindow();
        }

        function OnExpandCollapseButtonClick(s, e) {
            var isVisible = pnlFiltros.GetVisible();
            s.SetText(isVisible ? "+" : "-");
            pnlFiltros.SetVisible(!isVisible);
        }

        function ValidacionDeFecha(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;
            if (fechaInicio == null || fechaInicio == false || fechaFin == null || fechaFin == false) { return; }
            if (fechaInicio > fechaFin) { e.isValid = false; }
            var diff = Math.floor((fechaFin.getTime() - fechaInicio.getTime()) / (1000 * 60 * 60 * 24));
            if (diff > 60) { e.isValid = false; }
        }

        function LimpiaFormulario() {
            if (confirm("¿Realmente desea limpiar los campos del formulario?")) {
                ASPxClientEdit.ClearEditorsInContainerById('formPrincipal');
            }
        }

        function ValidarFiltros(s, e) {
            if (deFechaInicio.GetValue() == null && deFechaFin.GetValue() == null && txtIdentificacion.GetValue() == null
                && cmbCiudad.GetValue() == null && cmbEstrategia.GetValue() == null && cmbCausal.GetValue() == null) {
                alert('Debe seleccionar por lo menos un filtro de búsqueda.');
            } else {
                if (deFechaInicio.GetValue() == null && deFechaFin.GetValue() != null) {
                    alert('Debe digitar los dos rangos de fechas.');
                } else {
                    if (deFechaInicio.GetValue() != null && deFechaFin.GetValue() == null) {
                        alert('Debe digitar los dos rangos de fechas.');
                    } else { EjecutarCallbackGeneral(s, e, 'filtrarDatos'); }
                }
            }
        }

        function toggle(control) {
            $("#" + control).slideToggle("slow");
        }

        function ValidarFiltroLimpiar(s, e) {
            EjecutarCallbackGeneral(s, e, 'LimpiarConsulta');
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
        <div id="divEncabezado">
            <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        </div>
        <br />
        <div id="divGeneral" runat="server">
            <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" ClientInstanceName="cpGeneral" OnCallback="cpGeneral_Callback" >
                <ClientSideEvents EndCallback="function(s,e){
                    loadingPanel.Hide();
                    $('#divEncabezado').html(s.cpMensaje);
                 }" />
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtros de B&uacute;squeda" Width="1000px">
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
                                        <Paddings Padding="0px"></Paddings>
                                        <PanelCollection>
                                            <dx:PanelContent>
                                                <table width="95%">
                                                    <tr>
                                                        <td class="field">Documento cliente:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtIdentificacion" Width="250px" runat="server" ClientInstanceName="txtIdentificacion"
                                                                MaxLength="15" onkeypress="return solonumeros(event);" TabIndex="3">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="field">Ciudad:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cmbCiudad" runat="server" ClientInstanceName="cmbCiudad" IncrementalFilteringMode="Contains"
                                                                ValueType="System.Int32" TabIndex="2" Width="250px" FilterMinLength="4">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                        <td class="field">Estrategia Comercial:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cmbEstrategia" runat="server" ClientInstanceName="cmbEstrategia" IncrementalFilteringMode="Contains"
                                                                ValueType="System.Int32" TabIndex="4" Width="250px">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="field">Fecha Recepción Inicial:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio"
                                                                TabIndex="5" Width="250px">
                                                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                                </CalendarProperties>
                                                                <ClientSideEvents Validation="ValidacionDeFecha"></ClientSideEvents>
                                                                <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inválido. Fecha inicial menor que Fecha final. Rango menor que 60 días"
                                                                    ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                                </ValidationSettings>
                                                                <ClientSideEvents Validation="ValidacionDeFecha" />
                                                            </dx:ASPxDateEdit>
                                                        </td>
                                                        <td class="field">Fecha Recepción Final:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin" TabIndex="6"
                                                                Width="250px">
                                                                <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                                </CalendarProperties>
                                                                <ClientSideEvents Validation="ValidacionDeFecha"></ClientSideEvents>
                                                                <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inválido. Fecha inicial menor que Fecha final. Rango menor que 60 días"
                                                                    ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                                </ValidationSettings>
                                                                <ClientSideEvents Validation="ValidacionDeFecha" />
                                                            </dx:ASPxDateEdit>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" style="padding-top: 8px">
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td style="white-space: nowrap;" align="center">
                                                                        <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline!important;"
                                                                            AutoPostBack="false" ValidationGroup="Filtrado" TabIndex="7" ClientInstanceName="btnBuscar" HorizontalAlign="Justify">
                                                                            <ClientSideEvents Click="function(s, e) { ValidarFiltros(s, e); }"></ClientSideEvents>
                                                                            <Image Url="~/img/find.gif">
                                                                            </Image>
                                                                        </dx:ASPxButton>
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;<dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar"
                                                                            Style="display: inline!important;" AutoPostBack="false" TabIndex="8" HorizontalAlign="Justify">
                                                                            <ClientSideEvents Click="function(s, e) { LimpiaFormulario(); }"></ClientSideEvents>
                                                                            <Image Url="~/img/eraser.png">
                                                                            </Image>
                                                                            <ClientSideEvents Click="function(s, e) { 
                                                                                LimpiaFormulario(); 
                                                                                ValidarFiltroLimpiar(s, e);
                                                                            }" />
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
                        </dx:ASPxRoundPanel>
                        <br />
                        <div id="divGrilla" style="float: left; margin-top: 5px; width: 100%; visibility: visible">
                            <dx:ASPxRoundPanel ID="rpDatos" runat="server" ClientInstanceName="rpDatos" Width="100%" HeaderText="Información" Visible="true" HorizontalAlign="left">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxHiddenField ID="hfUsuario" ClientInstanceName="hfUsuario" runat="server">
                                        </dx:ASPxHiddenField>
                                        <dx:ASPxGridView ID="gvDatos" runat="server" ClientInstanceName="gvDatos" AutoGenerateColumns="false"
                                            KeyFieldName="IdCliente" Width="100%">
                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="IdCliente" Caption="IdCliente" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="0">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="NumeroIdentificacion" Caption="Núm. Identificación" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="1" Width="10%">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="EstadoActual" Caption="Estado" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="2">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="FechaRegistro" Caption="Fecha Registro" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="3">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="Agente" Caption="Agente" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="4" Width="70%">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="Estrategia" Caption="Estrategia" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="5">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ActividadLaboral" Caption="Actividad Laboral" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="6">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="EstadoNotus" Caption="Estado Notus" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="7">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="CiudadResedencia" Caption="Ciudad" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="8">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="NombreApellido" Caption="Nombre Cliente" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="9">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="CantidadGestiones" Caption="Cnt. Gestiones" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="10" Width="5%">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataColumn Caption="Opciones" VisibleIndex="10" CellStyle-HorizontalAlign="Center"
                                                    Width="75px">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <DataItemTemplate>
                                                        <dx:ASPxHyperLink runat="server" ID="lnkGestionar" ImageUrl="../img/Update.png" OnInit="Link_Init"
                                                            Cursor="pointer" ToolTip="Registrar Gestión">
                                                            <ClientSideEvents Click="function(s, e) {
                                                                Gestionar({1});
                                                            }" />
                                                        </dx:ASPxHyperLink>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Center">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                            <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                            <SettingsPager PageSize="10">
                                                <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                            </SettingsPager>
                                            <SettingsText Title="Información" EmptyDataRow="No se encontraron datos asociados al documento consultado."
                                                CommandEdit="Editar"></SettingsText>
                                            <SettingsDetail ShowDetailRow="True"></SettingsDetail>
                                            <Templates>
                                                <DetailRow>
                                                    <dx:ASPxGridView ID="gvDetalle" ClientInstanceName="gvDetalle" runat="server" AutoGenerateColumns="false"
                                                        Width="100%" OnBeforePerformDataSelect="gvDetalle_DataSelect" KeyFieldName="IdDetalle">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="ResultadoProceso" Caption="Resultado de Proceso" ShowInCustomizationForm="True"
                                                                HeaderStyle-HorizontalAlign="Center" VisibleIndex="0" Width="10%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="TipoVenta" Caption="Causal Generica" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="1" Width="15%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ObservacionCallCenter" Caption="Observacion" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="2" Width="40%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="FechaGestion" Caption="Fecha Gestion" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="3" Width="17%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="UsuarioRegistra" Caption="Registrada por" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="4" Width="20%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsDetail ExportMode="All" />
                                                        <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                                        <SettingsPager PageSize="5">
                                                            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                                        </SettingsPager>
                                                    </dx:ASPxGridView>
                                                </DetailRow>
                                            </Templates>
                                            <SettingsText CommandEdit="Editar" Title="Detalle Gestiones realizadas"
                                                EmptyDataRow="No se encontraron datos acordes con los filtros de búsqueda" />
                                            <SettingsBehavior EnableCustomizationWindow="False" AutoExpandAllGroups="False" />
                                        </dx:ASPxGridView>
                                        <dx:ASPxGridViewExporter ID="gveExportador" runat="server">
                                        </dx:ASPxGridViewExporter>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </div>
                        <br />
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
        <br />
        <dx:ASPxPopupControl ID="pcVer" runat="server" ClientInstanceName="dialogoVer" HeaderText="Ventana de Gestión"
            AllowDragging="true" Width="410px" Height="260px" Modal="true" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                </dx:PopupControlContentControl>
            </ContentCollection>
            <ClientSideEvents CloseButtonClick="function(s,e){
                ValidarFiltros(s, e);
            }" />
        </dx:ASPxPopupControl>
        <br />
        <div id="bluebar" class="menuFlotante">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4"></b></b>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 300px">
                        <dx:ASPxComboBox ID="cmbDetalle" runat="server" ShowImageInEditBox="true"
                            SelectedIndex="-1" ValueType="System.String" EnableCallbackMode="True" AutoResizeWithContainer="true"
                             ClientInstanceName="cmbDetalle" Width="250px">
                            <Items>
                                <dx:ListEditItem Text="Exportar sin detalles" Value="0" Selected="true" />
                                <dx:ListEditItem Text="Exportar solo con el detalle expandido" Value="1" />
                                <dx:ListEditItem Text="Exportar todos los detalles" Value="2" />
                            </Items>
                        </dx:ASPxComboBox>
                    </td>
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
                                <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular"></RegularExpression>
                                <RequiredField IsRequired="true" ErrorText="Formato a exportar requerido" />
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div1" style="float: right; visibility: visible; margin-right: 5px; margin-bottom: 5px; margin-top: 5px; width: 2%; position: fixed; overflow: hidden; display: block; bottom: 0px">
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
        <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
            Modal="True">
        </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
