<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaGestionesCallCenter.aspx.vb"
    Inherits="NotusExpress.BusquedaGestionesCallCenter" %>

<script type="text/javascript" src="../Scripts/jquery-1.12.4.js"></script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Búsqueda General de Ventas Call Center</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function EjecutarCallbackGeneral(s, e, parametro, valor) {
            if (ASPxClientEdit.AreEditorsValid()) {
                loadingPanel.Show();
                cpGeneral.PerformCallback(parametro + ':' + valor);
            }
        }

        function OnExpandCollapseButtonClick(s, e) {
            var isVisible = pnlFiltros.GetVisible();
            s.SetText(isVisible ? "+" : "-");
            pnlFiltros.SetVisible(!isVisible);
        }

        function Seleccionar(s, e) {
            s.SelectAll();
        }

        function EnValidacionDeRango(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;

            if (fechaInicio == null || fechaInicio == false || fechaFin == null || fechaFin == false) { return; }

            if (fechaInicio > fechaFin) { e.isValid = false; }

            var diff = Math.floor((fechaFin.getTime() - fechaInicio.getTime()) / (1000 * 60 * 60 * 24));

            if (diff > 60) { e.isValid = false; }
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

        function soloLetras(e) {
            tecla = (document.all) ? e.keyCode : e.which;
            if (tecla == 8 || tecla == 32) return true;
            patron = /[A-Za-zñÑ]/;
            te = String.fromCharCode(tecla);
            return patron.test(te);
        }

        function direccion(e) {
            tecla = (document.all) ? e.keyCode : e.which;
            if (tecla == 8) return true;
            patron = /[A-Za-zñÑ\d]/;
            te = String.fromCharCode(tecla);
            return patron.test(te);
        }

        function Reload()
        {
            window.opener.document.location.reload() 
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

        function LimpiaFormulario() {
            if (confirm("¿Realmente desea limpiar los campos del formulario?")) {
                ASPxClientEdit.ClearEditorsInContainerById('formPrincipal');
            }
        }

        function ValidarFiltros(s, e) {
            if (deFechaInicio.GetValue() == null && deFechaFin.GetValue() == null && txtFiltroIdentificacion.GetValue() == null && cbFiltroEstado.GetValue() == null
                && cbFiltroUsuario.GetValue() == null && cbFiltroEstrategia.GetValue() == null && cmbEstadoNotus.GetValue() == null && txtFiltroIdOportunidad.GetValue() == null) {
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

        function ValidarEnter(flag) {
            var kCode = (event.keyCode ? event.keyCode : event.which);
            if (kCode == 13) {
                if (flag == 1) {
                    btnBuscar.DoClick();
                }
                if (flag == 2) {
                    btnRegistrar.DoClick();
                }
            }
        }

        function Agendar(element, key) {
            TamanioVentana();
            dialogoVer.SetContentUrl("GestionVentaYServicio.aspx?id=" + key);
            dialogoVer.SetSize(myWidth * 0.95, myHeight * 0.95);
            dialogoVer.ShowWindow();
        }

        function Eliminar(element, key) {
            TamanioVentana();
            dialogoVerCancelar.PerformCallback('Inicial:' + key);
            dialogoVerCancelar.SetSize(myWidth * 0.35, myHeight * 0.3);
            dialogoVerCancelar.ShowWindow();
        }

        function Agregar(element, key) {
            TamanioVentana();
            dialogoServicio.PerformCallback('Inicial:' + key);
            dialogoServicio.SetSize(myWidth * 0.5, myHeight * 0.6);
            dialogoServicio.ShowWindow();
        }

        function Editar(element, key) {
            TamanioVentana();
            dialogoEditar.PerformCallback('InicialEdita:' + key);
            dialogoEditar.SetSize(myWidth * 0.8, myHeight * 0.6);
            dialogoEditar.ShowWindow();
        }

        function ImprimirFormularioVenta(element, key) {
            dialogoEditar.PerformCallback('ImprimirFormulario:' + key);
        }


        function DescargarDocumento(rutaOrigenArchivo) {
            location.href = '../Handlers/DescargarDocumento.ashx?rutaArchivoOrigen=' + rutaOrigenArchivo;
        }
        
    </script>
    <style type="text/css">
        .auto-style2 {
            height: 57px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
            <ClientSideEvents EndCallback="function(s,e){ 
            if(s.cpResultado==1){ 
                dialogoServicio.SetVisible(false);
                ActualizarEncabezado(s,e);
            }
            rdResultado.SetVisible(true);
            loadingPanel.Hide();
        }" />
            <ClientSideEvents EndCallback="function(s,e){ 
            if(s.cpResultado==1){ 
                ActualizarEncabezado(s,e); }
            rdResultado.SetVisible(true);
            loadingPanel.Hide();
            if(s.cpRutaArchivo.length != 0){
                DescargarDocumento(s.cpRutaArchivo);
            }
                
            }"></ClientSideEvents>
            <LoadingDivStyle CssClass="modalBackground">
            </LoadingDivStyle>
            <PanelCollection>
                <dx:PanelContent>
                    <vu:ValidacionURL ID="vuControlSesion" runat="server" />
                    <dx:ASPxHiddenField ID="hfDimensiones" ClientInstanceName="hfDimensiones" runat="server">
                    </dx:ASPxHiddenField>
                    <div id="divEncabezado">
                        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
                        <br />
                    </div>
                    <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtros de B&uacute;squeda">
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
                                            <table onkeydown="ValidarEnter(1);">
                                                <tr>
                                                    <td class="field">No. Identificación:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtFiltroIdentificacion" Width="250px" runat="server" ClientInstanceName="txtFiltroIdentificacion"
                                                            MaxLength="15" onkeypress="return solonumeros(event);" TabIndex="1">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td class="field">Resultado Proceso:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="cbFiltroEstado" runat="server" ClientInstanceName="cbFiltroEstado"
                                                            ValueType="System.Byte" TabIndex="2" Width="250px">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">Fecha Inicial Registro:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio"
                                                            TabIndex="3" Width="250px">
                                                            <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                            </CalendarProperties>
                                                            <ClientSideEvents Validation="EnValidacionDeRango"></ClientSideEvents>
                                                            <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final. Rango menor que 60 d&iacute;as"
                                                                ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                            </ValidationSettings>
                                                            <ClientSideEvents Validation="EnValidacionDeRango" />
                                                        </dx:ASPxDateEdit>
                                                    </td>
                                                    <td class="field">Fecha Final Registro:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin" TabIndex="4"
                                                            Width="250px">
                                                            <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                            </CalendarProperties>
                                                            <ClientSideEvents Validation="EnValidacionDeRango"></ClientSideEvents>
                                                            <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final. Rango menor que 60 d&iacute;as"
                                                                ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                            </ValidationSettings>
                                                            <ClientSideEvents Validation="EnValidacionDeRango" />
                                                        </dx:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">Usuario Registro:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="cbFiltroUsuario" runat="server" ClientInstanceName="cbFiltroUsuario"
                                                            TabIndex="4" Width="250px" IncrementalFilteringMode="Contains" CallbackPageSize="25"
                                                            EnableCallbackMode="false" FilterMinLength="3">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td class="field">Estrategia Comercial:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="cbFiltroEstrategia" runat="server" ClientInstanceName="cbFiltroEstrategia"
                                                            TabIndex="5" Width="250px">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">Estado Radicado:
                                                    </td>
                                                    <td>

                                                        <dx:ASPxComboBox ID="cmbEstadoNotus" runat="server" ClientInstanceName="cmbEstadoNotus"
                                                            TabIndex="6" Width="250px" IncrementalFilteringMode="Contains">
                                                        </dx:ASPxComboBox>

                                                    </td>
                                                    <td class="field">IdOportunidad:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtFiltroIdOportunidad" Width="250px" runat="server" ClientInstanceName="txtFiltroIdOportunidad"
                                                            MaxLength="15" TabIndex="7">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="padding-top: 8px" class="auto-style2">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="white-space: nowrap;" align="center">
                                                                    <dx:ASPxButton ID="btnBuscar" runat="server" Text="Buscar" Style="display: inline!important;"
                                                                        AutoPostBack="false" ValidationGroup="Filtrado" TabIndex="7" ClientInstanceName="btnBuscar">
                                                                        <ClientSideEvents Click="function(s, e) { ValidarFiltros(s, e); }"></ClientSideEvents>
                                                                        <Image Url="~/img/find.gif">
                                                                        </Image>
                                                                        <ClientSideEvents Click="function(s, e) { ValidarFiltros(s, e); }" />
                                                                    </dx:ASPxButton>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;<dx:ASPxButton ID="btnLimpiar" runat="server" Text="Limpiar"
                                                                        Style="display: inline!important;" AutoPostBack="false" TabIndex="8">
                                                                        <ClientSideEvents Click="function(s, e) { LimpiaFormulario(); }"></ClientSideEvents>
                                                                        <Image Url="~/img/eraser.png">
                                                                        </Image>
                                                                        <ClientSideEvents Click="function(s, e) { LimpiaFormulario(); }" />
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
                    <dx:ASPxRoundPanel ID="rdResultado" runat="server" HeaderText="Resultado de Búsqueda"
                        ClientInstanceName="rdResultado">
                        <PanelCollection>
                            <dx:PanelContent>
                                <dx:ASPxGridView ID="gvVentas" runat="server" AutoGenerateColumns="False" Font-Size="Small"
                                    ClientInstanceName="gvVentas" KeyFieldName="IdGestionVenta">
                                    <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e);}"></ClientSideEvents>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Id" FieldName="IdGestionVenta" VisibleIndex="0"
                                            Visible="True">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="No. Identificación" FieldName="NumeroIdentificacion"
                                            VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Estado" FieldName="Estado" VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Fecha Creación" FieldName="FechaRegistro" VisibleIndex="3">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Usuario" FieldName="Cliente" VisibleIndex="4">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Estrategia" FieldName="Estrategia" VisibleIndex="5">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="IdOportunidad" FieldName="IdOportunidad" VisibleIndex="6">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Actividad Laboral" FieldName="ActividadLaboral" VisibleIndex="7">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Fecha Agenda" FieldName="FechaAgendaVer" VisibleIndex="8">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Estado Notus" FieldName="EstadoNotus" VisibleIndex="9">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Opciones" ReadOnly="True" ShowInCustomizationForm="True"
                                            VisibleIndex="10">
                                            <DataItemTemplate>
                                                <dx:ASPxHyperLink runat="server" ID="lnkEliminar" ImageUrl="../img/error.png"
                                                    Cursor="pointer" ToolTip="Eliminar Servicio" OnInit="Link_Init">
                                                    <ClientSideEvents Click="function(s, e) { Eliminar(this, {0}); }" />
                                                </dx:ASPxHyperLink>
                                                <dx:ASPxHyperLink runat="server" ID="lnkAgendar" ImageUrl="../img/NewProcess.png"
                                                    Cursor="pointer" ToolTip="Agendar Servicio" OnInit="Link_Init">
                                                    <ClientSideEvents Click="function(s, e) { Agendar(this, {0}); }" />
                                                </dx:ASPxHyperLink>
                                                <dx:ASPxHyperLink runat="server" ID="lnkAgregar" ImageUrl="../img/DxAdd16.png"
                                                    Cursor="pointer" ToolTip="Agregar Producto" OnInit="Link_Init">
                                                    <ClientSideEvents Click="function(s, e) { Agregar(this, {0}); }" />
                                                </dx:ASPxHyperLink>
                                                <dx:ASPxHyperLink runat="server" ID="lnkEditar" ImageUrl="../img/DxEdit.png"
                                                    Cursor="pointer" ToolTip="Registrar Resultado Calidad" OnInit="Link_Init">
                                                    <ClientSideEvents Click="function(s, e) { Editar(this, {0}); }" />
                                                </dx:ASPxHyperLink>
                                                <dx:ASPxHyperLink runat="server" ID="lnkReimprimirFormulario" ImageUrl="../img/ReimprimirPDF.png"
                                                    Cursor="pointer" ToolTip="Imprimir formulario de venta" OnInit="Link_Init">
                                                    <%--<ClientSideEvents Click="function(s, e) { ImprimirFormularioVenta(this, {0}); }" />--%>
                                                    <ClientSideEvents Click="function (s, e){
                                                       EjecutarCallbackGeneral(s,e,'ImprimirFormulario',{0})
                                            }"></ClientSideEvents>

                                                </dx:ASPxHyperLink>

                                                <%--<dx:ASPxHyperLink runat="server" ID="linkEditCalidad" ImageUrl="../img/DxEdit.png"
                                                Cursor="pointer" ToolTip="Editar Datos Venta (calidad)" OnInit="Link_Init">
                                                <ClientSideEvents Click="function(s, e) { EditarC(this, {0}); }" />
                                            </dx:ASPxHyperLink>--%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                    <SettingsPager PageSize="10">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                        <PageSizeItemSettings ShowAllItem="True" Visible="True">
                                        </PageSizeItemSettings>
                                    </SettingsPager>
                                    <Settings ShowHeaderFilterButton="True"></Settings>
                                    <SettingsText Title="B&#250;squeda General de Ventas Call Center" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda"
                                        CommandEdit="Editar"></SettingsText>
                                    <SettingsDetail ShowDetailRow="True"></SettingsDetail>
                                    <Templates>
                                        <DetailRow>
                                            <dx:ASPxGridView ID="gvDetalle" ClientInstanceName="gvDetalle" runat="server" AutoGenerateColumns="false"
                                                Width="100%" OnBeforePerformDataSelect="gvDetalle_DataSelect" KeyFieldName="IdDetalle">
                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="Producto" Caption="Producto" ShowInCustomizationForm="True"
                                                        VisibleIndex="0">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="TipoProducto" Caption="Tipo Producto" ShowInCustomizationForm="True"
                                                        VisibleIndex="1">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Observacion" Caption="Observación" ShowInCustomizationForm="True"
                                                        VisibleIndex="2">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Opciones" ReadOnly="True" ShowInCustomizationForm="True"
                                                        VisibleIndex="3">
                                                        <DataItemTemplate>
                                                            <dx:ASPxHyperLink runat="server" ID="lnkEliminarDetalle" ImageUrl="../img/DXEraser16.png"
                                                                Cursor="pointer" ToolTip="Eliminar Servicio" OnInit="Link_InitDetalle">
                                                                <ClientSideEvents Click="function(s, e) { 
                                                                if(confirm('Realmente desea eliminar este producto?')){
                                                                    var valor= {0};
                                                                    EjecutarCallbackGeneral(s, e,'Eliminar', valor); 
                                                                }
                                                            }" />
                                                            </dx:ASPxHyperLink>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFooter="false" />
                                            </dx:ASPxGridView>
                                        </DetailRow>
                                    </Templates>
                                    <SettingsText CommandEdit="Editar" Title="Búsqueda General de Ventas Call Center"
                                        EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" />
                                    <SettingsBehavior EnableCustomizationWindow="False" AutoExpandAllGroups="False" />
                                </dx:ASPxGridView>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <dx:ASPxGridViewExporter ID="gveExportador" runat="server" GridViewID="gvVentas">
                    </dx:ASPxGridViewExporter>
                    <dx:ASPxPopupControl ID="dialogoVerCancelar" runat="server" HeaderText="Cancelación Gestión"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Modal="True"
                        ClientInstanceName="dialogoVerCancelar" ScrollBars="Auto" ShowMaximizeButton="True"
                        ShowPageScrollbarWhenModal="True" CloseAction="CloseButton" Width="385px" Height="250px">
                        <ModalBackgroundStyle CssClass="modalBackground" />
                        <ClientSideEvents EndCallback="function(s ,e){
                        if(s.cpResultado!=0){ 
                            ActualizarEncabezado(s,e); 
                            dialogoVerCancelar.SetVisible(false);
                            if(s.cpResultado==1){
                                rdResultado.SetVisible(false);
                            }
                        }
                    }"></ClientSideEvents>
                        <ModalBackgroundStyle CssClass="modalBackground">
                        </ModalBackgroundStyle>
                        <ContentCollection>
                            <dx:PopupControlContentControl>
                                <dx:ASPxPanel ID="pnlDatosCanal" runat="server" DefaultButton="btnRegistrar" TabIndex="0">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <table width="90%">
                                                <tr>
                                                    <td align="left" class="field">Id. Gestión:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxLabel ID="lblIdGestion" runat="server" ClientInstanceName="lblIdGestion"
                                                            Font-Bold="True" Font-Italic="True" Font-Overline="False" Font-Size="Medium">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="field">Justificación:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxMemo ID="meJustificacion" runat="server" Height="71px" Width="270px" NullText="Ingrese la justificación..."
                                                            ClientInstanceName="meJustificacion" TabIndex="1">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Registro">
                                                                <RegularExpression ErrorText="Formato no valido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$" />
                                                                <RequiredField IsRequired="true" ErrorText="Justificación Requerida" />
                                                                <RequiredField IsRequired="True" ErrorText="Justificaci&#243;n Requerida"></RequiredField>
                                                                <RegularExpression ErrorText="Formato no valido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\&#161;\?\&#191;\b\s&#225;&#233;&#237;&#243;&#250;&#193;&#201;&#205;&#211;&#218;&#241;&#209;\-\#]+\s*$"></RegularExpression>
                                                            </ValidationSettings>
                                                        </dx:ASPxMemo>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <dx:ASPxButton runat="server" ID="btnRegistrar" Text="Aceptar" AutoPostBack="False"
                                                            ImageSpacing="5px" ValidationGroup="Registro" ClientInstanceName="btnRegistrar">
                                                            <ClientSideEvents Click="function(s, e) { dialogoVerCancelar.PerformCallback(&#39;Cancelar: &#39; + lblIdGestion.GetValue())}"></ClientSideEvents>
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
                    <dx:ASPxPopupControl ID="pcServicio" runat="server" ClientInstanceName="dialogoServicio"
                        HeaderText="Agregar Servicio" AllowDragging="true"
                        Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                        ScrollBars="Auto" CloseAction="CloseButton">
                        <ClientSideEvents EndCallback="function(s,e){ 
                        if (s.cpResultado !=0){
                            ActualizarEncabezado(s,e);} 
                        }"></ClientSideEvents>
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="pccServicio" runat="server">
                                <table>
                                    <tr>
                                        <td class="field" colspan="2">Id. Gestión Venta:
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="lblIdGestionVenta" runat="server" ClientInstanceName="lblIdGestionVenta"
                                                Font-Bold="True" Font-Italic="True" Font-Overline="False" Font-Size="Medium">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Tipo Servicio:
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cmbTipoServicio" runat="server" ClientInstanceName="cmbTipoServicio" ValueType="System.Int32"
                                                Width="200px">
                                                <ClientSideEvents SelectedIndexChanged="function (s, e){
                                                var seleccion = cmbTipoServicio.GetValue();
                                                var valor = lblIdGestionVenta.GetValue();
                                                dialogoServicio.PerformCallback(&#39;Productos:&#39; + seleccion + ':' + valor)
                                             }"></ClientSideEvents>
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="productoExterno" Width="300px" Caption="Descripción" />
                                                </Columns>
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroServicio">
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td class="field">Producto:
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cmbProducto" runat="server" ClientInstanceName="cmbProducto" ValueType="System.Int32"
                                                Width="200px">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="productoExterno" Width="300px" Caption="Descripción" />
                                                </Columns>
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroServicio">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field" rowspan="2">Observación:
                                        </td>
                                        <td rowspan="2">
                                            <dx:ASPxMemo ID="memoObservacionServicio" runat="server" Height="71px" Width="200px">
                                                <ValidationSettings Display="Dynamic" SetFocusOnError="true" ValidationGroup="registroServicio"
                                                    ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Bottom">
                                                    <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$"
                                                        ErrorText="El texto digitado contiene caracteres no permitidos" />
                                                    <RegularExpression ErrorText="El texto digitado contiene caracteres no permitidos"
                                                        ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\&#161;\?\&#191;\b\s&#225;&#233;&#237;&#243;&#250;&#193;&#201;&#205;&#211;&#218;&#241;&#209;&#241;&#209;\-\#\[\]\(\)\/\\]+\s*$"></RegularExpression>
                                                </ValidationSettings>
                                            </dx:ASPxMemo>
                                        </td>
                                        <td class="field">Cupo:
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtCupo" runat="server" Width="200px" MaxLength="10" TabIndex="9" onkeypress="return solonumeros(event)" ClientInstanceName="txtCupo">
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroServicio">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <%-- <td class="field">Prima:
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cmbPrima" runat="server" ClientInstanceName="cmbPrima" ValueType="System.Int32"
                                                Width="200px">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="ValorPrimaServicio" Width="300px" Caption="Descripción" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxImage ID="imgAgregar" runat="server" ImageUrl="../img/DxSuperMarket.png" ToolTip="Agregar Producto"
                                                Cursor="pointer">
                                                <ClientSideEvents Click="function (s, e){
                                                if(ASPxClientEdit.ValidateGroup(&#39;registroServicio&#39;)){
                                                    var valor = lblIdGestionVenta.GetValue();
                                                    EjecutarCallbackGeneral(s,e,'Registrar',valor)
                                                }
                                            }"></ClientSideEvents>
                                            </dx:ASPxImage>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <dx:ASPxPopupControl ID="dialogoEditar" runat="server" HeaderText="Edición Registro"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Modal="True"
                        ClientInstanceName="dialogoEditar" ScrollBars="Auto" ShowMaximizeButton="True"
                        ShowPageScrollbarWhenModal="True" CloseAction="CloseButton" Width="385px" Height="250px"
                        MinWidth="400px" MinHeight="200px">
                        <ContentCollection>
                            <dx:PopupControlContentControl>
                                <table width="100%">
                                    <tr>
                                        <td class="field" colspan="2">Id. Gestión Venta:
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="lblIdGestionEdit" runat="server" ClientInstanceName="lblIdGestionEdit"
                                                Font-Bold="True" Font-Italic="True" Font-Overline="False" Font-Size="Medium">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field" colspan="1">Tipo Identificación:
                                        </td>
                                        <td colspan="1" class="style1">
                                            <dx:ASPxComboBox ID="cbTipoId" runat="server" ValueType="System.Int32" ClientInstanceName="cbTipoId" ClientEnabled="false"
                                                Width="250px" TabIndex="1">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="descripcion" Width="300px" Caption="Descripción" />
                                                </Columns>
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Tipo identificación requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Tipo identificaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td class="field">No. de Identificaci&oacute;n:
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtNumIdentificacion" runat="server" Width="250px" MaxLength="15" ClientInstanceName="txtNumIdentificacion"
                                                ReadOnly="False" onkeypress="return solonumeros(event);" TabIndex="2" ClientEnabled="false">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Número identificación requerido" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="N&#250;mero identificaci&#243;n requerido"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Nombres:
                                        </td>
                                        <td class="style1">
                                            <dx:ASPxTextBox ID="txtNombres" runat="server" Width="250px" MaxLength="50" TabIndex="3"
                                                onkeypress="return soloLetras(event)" ClientEnabled="false">
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                                <ClientSideEvents GotFocus="Seleccionar"></ClientSideEvents>

                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td class="field">Primer Apellido:
                                        </td>
                                        <td class="style1">
                                            <dx:ASPxTextBox ID="txtPrimerApellido" runat="server" Width="250px" MaxLength="50"
                                                TabIndex="4" onkeypress="return soloLetras(event)" ClientEnabled="false">
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                                <ClientSideEvents GotFocus="Seleccionar"></ClientSideEvents>

                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Segundo Apellido:
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtSegundoApellido" runat="server" Width="250px" MaxLength="50" ClientEnabled="false"
                                                TabIndex="5" onkeypress="return soloLetras(event)">
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                                <ClientSideEvents GotFocus="Seleccionar"></ClientSideEvents>

                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td class="field">Dirección Residencia:
                                        </td>
                                        <td class="style1">
                                            <dx:ASPxTextBox ID="txtDireccionResidencia" runat="server" Width="250px" MaxLength="200" ClientEnabled="false"
                                                TabIndex="6">
                                                <ClientSideEvents GotFocus="Seleccionar"></ClientSideEvents>

                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$"
                                                        ErrorText="El texto digitado contiene caracteres no permitidos" />
                                                    <RegularExpression ErrorText="El texto digitado contiene caracteres no permitidos"
                                                        ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\&#161;\?\&#191;\b\s&#225;&#233;&#237;&#243;&#250;&#193;&#201;&#205;&#211;&#218;&#241;&#209;&#241;&#209;\-\#\[\]\(\)\/\\]+\s*$"></RegularExpression>
                                                    <RequiredField ErrorText="Información Requerida" IsRequired="True"></RequiredField>
                                                </ValidationSettings>
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                            </dx:ASPxTextBox>
                                            <dx:ASPxCheckBox ID="cbModificarDireccion" ClientInstanceName="cbModificarDireccion" runat="server" Checked="false" Text="Permitir cambio de direccion" ClientVisible="false">
                                            </dx:ASPxCheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Ciudad Residencia:
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cmbCiudad1" runat="server" ClientInstanceName="cmbCiudad1" Width="250" ValueType="System.Int32" ClientEnabled="false"
                                                IncrementalFilteringMode="Contains" CallbackPageSize="25" EnableCallbackMode="true" FilterMinLength="3" TabIndex="7">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="IdCiudad" Width="70px" Caption="Id" />
                                                    <dx:ListBoxColumn FieldName="CiudadDepartamento" Width="300px" Caption="Ciudad" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                            <div>
                                                <dx:ASPxLabel ID="lblComentario" runat="server" Text="Digite parte de la ciudad."
                                                    CssClass="comentario" Width="270px" Font-Size="XX-Small" Font-Bold="False" Font-Italic="True"
                                                    Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                                                </dx:ASPxLabel>
                                            </div>
                                        </td>
                                        <td class="field">Teléfono Fijo:
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtTelFijo" runat="server" Width="250px" MaxLength="10" TabIndex="8" ClientEnabled="false"
                                                onkeypress="return solonumeros(event)">
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                                <ClientSideEvents GotFocus="Seleccionar"></ClientSideEvents>

                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Celular:
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtCelular" runat="server" Width="250px" MaxLength="10" TabIndex="9" ClientEnabled="false"
                                                onkeypress="return solonumeros(event)">
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                                <ClientSideEvents GotFocus="Seleccionar"></ClientSideEvents>

                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td class="field">Email:
                                        </td>
                                        <td class="style1">
                                            <dx:ASPxTextBox ID="txtEmail" runat="server" Width="250px" MaxLength="50" TabIndex="10" ClientEnabled="false">
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                                <ClientSideEvents GotFocus="Seleccionar"></ClientSideEvents>

                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RegularExpression ErrorText="Formato no válido" ValidationExpression="\S+@\S+\.\S+" />
                                                    <RegularExpression ErrorText="Formato no v&#225;lido" ValidationExpression="\S+@\S+\.\S+"></RegularExpression>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Ingresos Aproximados:
                                        </td>
                                        <td class="style1">
                                            <dx:ASPxTextBox ID="txtIngresos" runat="server" Width="250px" MaxLength="10" TabIndex="11" ClientEnabled="false"
                                                onkeypress="return solonumeros(event)">
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                                <ClientSideEvents GotFocus="Seleccionar"></ClientSideEvents>
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>

                                        <td class="field">Estado calidad:
                                        </td>

                                        <td>
                                            <dx:ASPxComboBox ID="cmbEstadoCalidad" runat="server" ClientInstanceName="cmbEstadoCalidad" Width="250"
                                                ValueType="System.Int32" TabIndex="7" Visible="true">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                                <ClientSideEvents TextChanged="function(s,e){
                                                    dialogoEditar.PerformCallback('ValidaResultado:0');
                                                 }" />
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Causa Rechazo:
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cmbEstadoRechazo" runat="server" ClientInstanceName="cmbEstadoRechazo" Width="250"
                                                ValueType="System.Int32" TabIndex="8" Visible="true" ClientEnabled="false">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Informaci&#243;n requerida"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td class="field">Observaciones Calidad
                                        </td>
                                        <td>
                                            <dx:ASPxMemo ID="txtObservacionCalidad" runat="server" Height="50px" Width="250px" Enabled="true"
                                                TabIndex="15">
                                                <ValidationSettings Display="Dynamic" SetFocusOnError="true" ValidationGroup="registroVenta"
                                                    ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" RequiredField-IsRequired="true">
                                                    <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$"
                                                        ErrorText="El texto digitado contiene caracteres no permitidos" />

                                                    <RequiredField IsRequired="True"></RequiredField>
                                                </ValidationSettings>
                                                <ClientSideEvents GotFocus="Seleccionar" />
                                            </dx:ASPxMemo>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <dx:ASPxButton ID="imgAgrega" runat="server" ToolTip="Editar" ValidationGroup="registroVenta" TabIndex="12" ClientInstanceName="imgAgrega">
                                                <ClientSideEvents Click="function(s, e) { 
                                                        if (cmbEstadoCalidad.GetValue() == 233 && cbModificarDireccion.GetVisible() && !cbModificarDireccion.GetChecked()){
                                                           alert('Debe checkear el campo permitir el cambio de direccion para continuar');
                                                           e.processOnServer=false;
                                                        }else{
                                                            var valor = lblIdGestionEdit.GetValue();
                                                            EjecutarCallbackGeneral(s,e,'Editar',valor)
                                                            dialogoEditar.Hide();
                                                            Reload();
                                                        }
                                                    }"></ClientSideEvents>
                                                <Image Url="../img/DxAdd32.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <msgp:MensajePopUp ID="mensajero" runat="server" />
                    <dx:ASPxPopupControl ID="pcVer" runat="server" ClientInstanceName="dialogoVer" HeaderText="Información"
                        AllowDragging="true" Width="410px" Height="260px" Modal="true" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton">
                        <ClientSideEvents CloseUp="function (s, e){
                        EjecutarCallbackGeneral(s, e, 'filtrarDatos');
                            if (s.cpResultado == 1){
                            dialogoEditar.Hide();} 
                    }" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
        <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
            Modal="True">
        </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
