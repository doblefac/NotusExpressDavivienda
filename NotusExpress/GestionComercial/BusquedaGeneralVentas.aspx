<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusquedaGeneralVentas.aspx.vb"
    Inherits="NotusExpress.BusquedaGeneralVentas" %>

<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Búsqueda General de Ventas</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
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

        function EnValidacionDeRango(s, e) {
            var fechaInicio = deFechaInicio.date;
            var fechaFin = deFechaFin.date;

            if (fechaInicio == null || fechaInicio == false || fechaFin == null || fechaFin == false) { return; }

            if (fechaInicio > fechaFin) { e.isValid = false; }

            var diff = Math.floor((fechaFin.getTime() - fechaInicio.getTime()) / (1000 * 60 * 60 * 24));

            if (diff > 60) { e.isValid = false; }
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

        function ValidarFiltros(s, e) {
            if (deFechaFinR.GetValue() == null && deFechaInicioR.GetValue() == null && deFechaFin.GetValue() == null && deFechaInicio.GetValue() == null &&
                cbFiltroCausal.GetValue() == null && cbFiltroNovedad.GetValue() == null && cbFiltroEstado.GetValue() == null &&
                txtFiltroIdentificacion.GetValue() == null && cbFiltroAsesor.GetValue() == null && cbFiltroCiudad.GetValue() == null) {
                alert('Debe seleccionar por lo menos un filtro de búsqueda.');
            } else {
                if (deFechaInicioR.GetValue() == null && deFechaFinR.GetValue() != null) {
                    alert('Debe digitar los dos rangos de fechas.');
                } else {
                    if (deFechaInicioR.GetValue() != null && deFechaFinR.GetValue() == null) {
                        alert('Debe digitar los dos rangos de fechas.');
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
            }
        }

        function ValidarFiltrosExtendido(s, e) {
            if (deFechaFinR.GetValue() == null && deFechaInicioR.GetValue() == null && deFechaFin.GetValue() == null && deFechaInicio.GetValue() == null &&
                cbFiltroCausal.GetValue() == null && cbFiltroNovedad.GetValue() == null && cbFiltroEstado.GetValue() == null &&
                txtFiltroIdentificacion.GetValue() == null && cbFiltroAsesor.GetValue() == null && cbFiltroCiudad.GetValue() == null) {
                alert('Debe seleccionar por lo menos un filtro de búsqueda.');
            } else {
                if (deFechaInicioR.GetValue() == null && deFechaFinR.GetValue() != null) {
                    alert('Debe digitar los dos rangos de fechas.');
                } else {
                    if (deFechaInicioR.GetValue() != null && deFechaFinR.GetValue() == null) {
                        alert('Debe digitar los dos rangos de fechas.');
                    } else {
                        if (deFechaInicio.GetValue() == null && deFechaFin.GetValue() != null) {
                            alert('Debe digitar los dos rangos de fechas.');
                        } else {
                            if (deFechaInicio.GetValue() != null && deFechaFin.GetValue() == null) {
                                alert('Debe digitar los dos rangos de fechas.');
                            } else {
                                loadingPanel.Show();
                                callback.PerformCallback('DescargarExtendido');
                            }
                        }
                    }
                }
            }
        }

        function LimpiaFormulario() {
            if (confirm("¿Realmente desea limpiar los campos del formulario?")) {
                ASPxClientEdit.ClearEditorsInContainerById('formPrincipal');
            }
        }

        function ValidarEnter(flag) {
            var kCode = (event.keyCode ? event.keyCode : event.which);
            if (kCode.toString() == "13") {
                if (flag == 1) {
                    btnBuscar.DoClick();
                }
            }
        }

        function Gestionar(element, key) {
            TamanioVentana();
            dialogoVerNovedad.PerformCallback('Inicial:' + key);
            dialogoVerNovedad.SetSize(myWidth * 0.6, myHeight * 0.6);
            dialogoVerNovedad.ShowWindow();
        }

        function VerRadicado(key) {
            TamanioVentana();
            dialogoVerRadicar.SetContentUrl("RadicacionDocumentos.aspx?id=" + key)
            dialogoVerRadicar.SetSize(myWidth * 0.9, myHeight * 0.9);
            dialogoVerRadicar.ShowWindow();
        }

        function EliminarNovedad(element, key) {
            if (confirm("Esta seguro de eliminar la novedad seleccionada?")) {
                gvNovedades.PerformCallback('Eliminar:' + key);
            }
        }

        function GestionarNovedad(element, key) {
            TamanioVentana();
            dialogoGestion.PerformCallback('Inicial:' + key);
            dialogoGestion.SetSize(myWidth * 0.7, myHeight * 0.4);
            dialogoGestion.ShowWindow();
        }

        function VerPlanilla(element, key) {
            
        }

        function toggle(control) {
            $("#" + control).slideToggle("slow");
        }

        function SeleccionarODesmarcarTodo(ctrl, targetName) {
            $('#formPrincipal input[type="checkbox"]').each(function () {
                if (!$(this).attr("disabled"))
                    $(this).attr("checked", ctrl.checked);
            });
        }

        function ValidarSeleccion(s, e) {
            var cantidad = 0;
            var listaGestion = new Array();

            $('#formPrincipal input[type="checkbox"]').each(function () {
                if ($(this).attr("checked")) {
                    var row = $(this).parent("span").parent("td");
                    var idGestion = row.find('span');
                    if (idGestion.attr('class')) {
                        listaGestion[cantidad] = idGestion.attr('class');
                        cantidad = cantidad + 1;
                    }

                }
            });
            if (cantidad > 0) {
                if (confirm('¿Realmente desea radicar:  [' + cantidad + '], gestiones seleccionadas?')) {
                    VerRadicado(listaGestion);
                }
            } else { alert('Debe seleccionar por lo menos un registro.'); }
        }

        function EvaluarFinCallback(s, e) {
            ActualizarEncabezado(s, e);
            if (s.cpListaRadicado) {
                if (s.cpListaRadicado != 0) {
                    VerPlanilla(this, s.cpListaRadicado);
                }
            }  
        }

        function VerPlanilla(element, key) {
            TamanioVentana();
            dialogoVerPlanilla.SetContentUrl("Reportes/VisorPlanillaRadicacion.aspx?id=" + key)
            dialogoVerPlanilla.SetSize(myWidth * 0.6, myHeight * 0.9);
            dialogoVerPlanilla.ShowWindow();
        }

        function DescargarReporte() {
            window.location.href = 'DescargaDocumento.aspx?id=2';
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
        <ClientSideEvents EndCallback="function(s,e){ 
            ActualizarEncabezado(s, e);
        }"></ClientSideEvents> 
        <PanelCollection>
            <dx:PanelContent>
                <vu:ValidacionURL ID="vuControlSesion" runat="server" />
                <div id="divEncabezado">
                    <epg:EncabezadoPagina ID="epNotificador" runat="server" />
                    <br />
                </div>
                <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtros de B&uacute;squeda"
                    Width="80%">
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
                                <Paddings Padding="0px"></Paddings>
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <table onkeydown="ValidarEnter(1);">
                                            <tr>
                                                <td class="field">
                                                    Ciudad:
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox runat="server" ClientInstanceName="cbFiltroCiudad" ID="cbFiltroCiudad"
                                                        AutoPostBack="False" TabIndex="1" ValueType="System.Int32" Width="200px" IncrementalFilteringMode="Contains"
                                                        CallbackPageSize="25" EnableCallbackMode="true" FilterMinLength="3">
                                                    </dx:ASPxComboBox>
                                                    <div>
                                                        <dx:ASPxLabel ID="lblComentario" runat="server" Text="Digite parte de la ciudad."
                                                            CssClass="comentario" Width="170px" Font-Size="XX-Small" Font-Bold="False" Font-Italic="True"
                                                            Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                                                        </dx:ASPxLabel>
                                                    </div>
                                                </td>
                                                <td class="field">
                                                    Asesor:
                                                </td>
                                                <td style="vertical-align: middle !important" nowrap="nowrap">
                                                    <dx:ASPxComboBox ID="cbFiltroAsesor" runat="server" ClientInstanceName="cbFiltroAsesor"
                                                        ValueType="System.Int32" TabIndex="2" IncrementalFilteringMode="Contains" Width="200px">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td class="field">
                                                    No. Identificación:
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtFiltroIdentificacion" Width="200px" runat="server" ClientInstanceName="txtFiltroIdentificacion"
                                                        MaxLength="15" onkeypress="return solonumeros(event);" TabIndex="3">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="field">
                                                    Estado:
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbFiltroEstado" runat="server" ClientInstanceName="cbFiltroEstado"
                                                        ValueType ="System.Int32" TabIndex="4" IncrementalFilteringMode="Contains" Width="200px">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td class="field">
                                                    Novedad:
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbFiltroNovedad" runat="server" ClientInstanceName="cbFiltroNovedad"
                                                        ValueType="System.Int32" TabIndex="5" IncrementalFilteringMode="Contains" Width="200px">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td class="field">
                                                    Causal Genérica:
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbFiltroCausal" runat="server" ClientInstanceName="cbFiltroCausal"
                                                        ValueType="System.Int32" TabIndex="6" IncrementalFilteringMode="Contains" Width="200px">
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="field">
                                                    Fecha Inicial Registro:
                                                </td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="deFechaInicio" runat="server" ClientInstanceName="deFechaInicio"
                                                        TabIndex="7" Width="200px">
                                                        <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                        </CalendarProperties>
                                                        <ClientSideEvents Validation="EnValidacionDeRango"></ClientSideEvents>
                                                        <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final. Rango menor que 60 d&iacute;as"
                                                            ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                                <td class="field">
                                                    Fecha Final Registro:
                                                </td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="deFechaFin" runat="server" ClientInstanceName="deFechaFin" TabIndex="8"
                                                        Width="200px">
                                                        <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                        </CalendarProperties>
                                                        <ClientSideEvents Validation="EnValidacionDeRango"></ClientSideEvents>
                                                        <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final. Rango menor que 60 d&iacute;as"
                                                            ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="field">
                                                    Fecha Inicial Recepción:
                                                </td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="deFechaInicioR" runat="server" ClientInstanceName="deFechaInicioR"
                                                        TabIndex="9" Width="200px">
                                                        <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                        </CalendarProperties>
                                                        <ClientSideEvents Validation="EnValidacionDeRango"></ClientSideEvents>
                                                        <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final. Rango menor que 60 d&iacute;as"
                                                            ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                                <td class="field">
                                                    Fecha Final Recepción:
                                                </td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="deFechaFinR" runat="server" ClientInstanceName="deFechaFinR"
                                                        TabIndex="10" Width="200px">
                                                        <CalendarProperties ClearButtonText="Limpiar" TodayButtonText="Hoy">
                                                        </CalendarProperties>
                                                        <ClientSideEvents Validation="EnValidacionDeRango"></ClientSideEvents>
                                                        <ValidationSettings SetFocusOnError="True" EnableCustomValidation="true" ErrorText="Dato Inv&aacute;lido. Fecha inicial menor que Fecha final. Rango menor que 60 d&iacute;as"
                                                            ErrorDisplayMode="ImageWithText" Display="Dynamic" ErrorTextPosition="Bottom">
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                                <td align ="center" colspan ="2">
                                                    <dx:ASPxImage ID="imgAgrega" runat="server" ImageUrl="../img/DxConfirm32.png" ToolTip="Realizar Búsqueda"
                                                        Cursor="pointer" TabIndex ="11">
                                                        <ClientSideEvents Click="function (s, e){
                                                                ValidarFiltros(s, e);
                                                            }"></ClientSideEvents>
                                                    </dx:ASPxImage>
                                                    &nbsp
                                                    <dx:ASPxImage ID="imgCancela" runat="server" ImageUrl="../img/DxCancel32.png" ToolTip="Cancelar"
                                                        Cursor="pointer" TabIndex ="12">
                                                        <ClientSideEvents Click="function (s, e){
                                                            LimpiaFormulario();
                                                        }"></ClientSideEvents>
                                                    </dx:ASPxImage>  
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" style="padding-top: 8px">
                                                    <dx:ASPxButton ID="btnBuscar" runat="server" AutoPostBack="false" ClientInstanceName ="btnBuscar"
                                                        ClientVisible ="false">
                                                        <ClientSideEvents Click="function(s, e) { ValidarFiltros(s, e);}"></ClientSideEvents>
                                                    </dx:ASPxButton>
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
                                <ClientSideEvents EndCallback="function(s,e){ EvaluarFinCallback(s,e);}"></ClientSideEvents>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Id" FieldName="IdGestionVenta" VisibleIndex="0">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Id. Notus" FieldName="IdServicioNotus"
                                        VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Fecha Agenda" FieldName="FechaAgendaVer" VisibleIndex="3">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Fecha Gestión" FieldName="FechaGestion" VisibleIndex="4">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Identificación" FieldName="IdentificacionCliente" VisibleIndex="5">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Cliente" FieldName="Cliente" VisibleIndex="6">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ciudad" FieldName="CiudadCliente" VisibleIndex="7">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Cod. Estrategia" FieldName="CodigoEstrategia"
                                        VisibleIndex="8">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataColumn Caption="Radicar" VisibleIndex="9">
                                        <HeaderCaptionTemplate>
                                            <asp:CheckBox ID="chkSeleccionarTodo" runat="server" AutoPostBack="false" onclick="SeleccionarODesmarcarTodo(this,'chkRadica');" />
                                            <asp:Panel ID="pnlRadica" runat="server" Style="display: inline">
                                                <dx:ASPxImage ID="imgRadica" runat="server" ImageUrl="../img/DxPikingList.png"
                                                    ClientInstanceName="imgRadica" ToolTip="Radicar Documentos" Cursor="pointer">
                                                    <ClientSideEvents Click="function (s, e){
                                                        ValidarSeleccion(s, e);
                                                    }" />
                                                </dx:ASPxImage>
                                            </asp:Panel>
                                        </HeaderCaptionTemplate>
                                        <DataItemTemplate>
                                            <asp:CheckBox ID="chkRadica" runat="server" OnInit="Chk_Init" CssClass="{0}" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn Caption="Opciones" ReadOnly="True" ShowInCustomizationForm="True"
                                        VisibleIndex="10">
                                        <DataItemTemplate>
                                            <dx:ASPxHyperLink runat="server" ID="lnkGestionar" ImageUrl="../img/DxEdit.png"
                                                Cursor="pointer" ToolTip="Gestionar Servicio" OnInit="Link_Init">
                                                <ClientSideEvents Click="function(s, e) { Gestionar(this, {0}); }" />
                                            </dx:ASPxHyperLink>
                                            <dx:ASPxHyperLink runat="server" ID="lnkPlanilla" ImageUrl="../img/DxPdf.png"
                                                Cursor="pointer" ToolTip="Ver Planilla" OnInit="Link_Init">
                                                <ClientSideEvents Click="function(s, e) { gvVentas.PerformCallback('verPlanilla:' + {0}); }" />
                                            </dx:ASPxHyperLink>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AutoExpandAllGroups="True" EnableCustomizationWindow="True"></SettingsBehavior>
                                <SettingsPager PageSize="50">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                    <PageSizeItemSettings ShowAllItem="True" Visible="True">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                                <SettingsDetail ShowDetailRow="True" />
                                <Settings ShowTitlePanel="True" ShowHeaderFilterBlankItems="False"></Settings>
                                <SettingsText Title="B&#250;squeda General de Ventas" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" CommandEdit="Editar"></SettingsText>
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
                                            </Columns>
                                            <Settings ShowFooter="false" />
                                        </dx:ASPxGridView>
                                    </DetailRow>
                                </Templates>
                            </dx:ASPxGridView>
                            <dx:ASPxGridViewExporter ID="gveExportador" runat="server" GridViewID="gvVentas">
                            </dx:ASPxGridViewExporter>
                        </dx:PanelContent>
                    </PanelCollection> 
                </dx:ASPxRoundPanel>
            </dx:PanelContent>
        </PanelCollection>
<ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e); }"></ClientSideEvents>

        <LoadingDivStyle CssClass="modalBackground">
        </LoadingDivStyle>
    </dx:ASPxCallbackPanel>
    <dx:ASPxPopupControl ID="dialogoVerNovedad" runat="server" ClientInstanceName="dialogoVerNovedad"
        Modal="true" HeaderText="Gestión Servicio" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ScrollBars="Auto" AllowDragging="True" CloseAction ="CloseButton">
        <ModalBackgroundStyle CssClass="modalBackground" />
        <ClientSideEvents EndCallback ="function (s, e){
            if(s.cpResultado==0){ 
                ActualizarEncabezado(s,e); 
            } if (s.cpResultado!=10) {
                ActualizarEncabezado(s,e); 
            }
        }" Closing ="function (s, e) { EjecutarCallbackGeneral(s, e, 'filtrarDatos'); }"/>
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="lblIdGestion" runat ="server" ClientInstanceName ="lblIdGestion" ClientVisible ="false"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxPageControl ID="pcAsociadosCampania" runat="server" ActiveTabIndex="0" 
                                Width="100%">
                                <TabPages>
                                    <dx:TabPage Text="Recepción Documentos">
                                        <TabImage Url="../img/DxPikingList.png"></TabImage>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl1" runat="server">
                                                <dx:ASPxPanel ID="pnlRecepcion" runat="server" ScrollBars="Auto" Height="250px">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <table runat ="server">
                                                                <tr>
                                                                    <td class ="field" align ="left">
                                                                        Tipo Recepción:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxRadioButtonList ID="rblTipoRecepcion" runat="server" RepeatDirection="Horizontal"
                                                                            ClientInstanceName="rblTipoRecepcion" Font-Size="XX-Small" Height="10px">
                                                                            <Items>
                                                                                <dx:ListEditItem Text="Recepción" Value="1" Selected="true" />
                                                                                <dx:ListEditItem Text="Devolución" Value="0" />
                                                                            </Items>
                                                                            <Border BorderStyle="None"></Border>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                                                <RequiredField ErrorText ="Información Requerida" IsRequired ="true" />
                                                                            </ValidationSettings> 
                                                                        </dx:ASPxRadioButtonList>
                                                                    </td>
                                                                    <td class ="field" align ="left">
                                                                        Observación
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxMemo ID="meObservacion" runat="server" Height="50px" Width="270px" NullText="Ingrese una observación..."
                                                                            ClientInstanceName="meObservacion">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                                                <RegularExpression ErrorText="Formato no valido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-]+\s*$" />
                                                                                <RequiredField ErrorText ="Observación Requerida" IsRequired ="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxMemo>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class ="field" align ="left">
                                                                        Número de Guía:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtGuia" runat ="server" ClientInstanceName ="txtGuia">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                                                <RegularExpression ErrorText="Formato no valido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-]+\s*$" />
                                                                            </ValidationSettings> 
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td class ="field" align ="left">
                                                                        Agregar Novedad:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxRadioButtonList ID="rblNovedad" runat="server" RepeatDirection="Horizontal"
                                                                            ClientInstanceName="rblNovedad" Font-Size="XX-Small" Height="10px">
                                                                            <Items>
                                                                                <dx:ListEditItem Text="Si" Value="1" />
                                                                                <dx:ListEditItem Text="No" Value="0" Selected ="true" />
                                                                            </Items>
                                                                            <ClientSideEvents SelectedIndexChanged ="function (s, e){
                                                                                var key =lblIdGestion.GetValue();
                                                                                dialogoVerNovedad.PerformCallback('Novedad:' + key);
                                                                            }" />
                                                                            <Border BorderStyle="None"></Border>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                                                <RequiredField ErrorText ="Información Requerida" IsRequired ="true" />
                                                                            </ValidationSettings> 
                                                                        </dx:ASPxRadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td id ="tdNovedad" class ="field" align ="left">
                                                                        Novedad:
                                                                    </td>
                                                                    <td id ="tdNovedad1">
                                                                        <dx:ASPxGridLookup ID="gluDocumentos" runat="server" KeyFieldName="IdNovedad" SelectionMode="Multiple"
                                                                            IncrementalFilteringMode="StartsWith" TextFormatString="{0}" Width="250px" ClientInstanceName="gluDocumentos"
                                                                            MultiTextSeparator=", " AllowUserInput="false">
                                                                            <ClientSideEvents ButtonClick="function(s,e) {gluDocumentos.GetGridView().UnselectAllRowsOnPage(); gluDocumentos.HideDropDown(); }" />
                                                                            <Buttons>
                                                                                <dx:EditButton Text="X">
                                                                                </dx:EditButton>
                                                                            </Buttons>
                                                                            <Columns>
                                                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width ="250px" />
                                                                                <dx:GridViewDataTextColumn FieldName="Nombre" Width ="250px" />
                                                                                <dx:GridViewDataTextColumn FieldName="Causal" Width ="250px" />
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
                                                                    <td colspan ="2" align ="center">
                                                                        <dx:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="../img/DxConfirm32.png" ToolTip="Editar"
                                                                            Cursor="pointer">
                                                                            <ClientSideEvents Click ="function (s, e){
                                                                                if(ASPxClientEdit.ValidateGroup(&#39;vgRegistro&#39;)){
                                                                                    var key =lblIdGestion.GetValue();
                                                                                    dialogoVerNovedad.PerformCallback('Actualizar:' + key);
                                                                                }
                                                                            }" />
                                                                        </dx:ASPxImage> 
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxPanel> 
                                            </dx:ContentControl> 
                                        </ContentCollection> 
                                    </dx:TabPage>
                                    <dx:TabPage Text="Gestión Novedades">
                                    <TabImage Url="../img/DxAdd16.png"></TabImage>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl2" runat="server">
                                                <dx:ASPxPanel ID="pnlNovedades" runat="server" ScrollBars="Auto" Height="250px">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <dx:ASPxGridView ID="gvNovedades" runat ="server" ClientInstanceName ="gvNovedades" 
                                                                AutoGenerateColumns ="false" KeyFieldName="IdNovedadServicio">
                                                                <ClientSideEvents EndCallback ="function (s, e){
                                                                    if(s.cpResultado==0){ 
                                                                        ActualizarEncabezado(s,e); 
                                                                    } if (s.cpResultado!=10) {
                                                                        ActualizarEncabezado(s,e); 
                                                                    }
                                                                }" />
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="Id" FieldName="IdNovedadServicio" VisibleIndex="0">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Novedad" FieldName="Novedad" VisibleIndex="1">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Casual" FieldName="Causal" VisibleIndex="2">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Estado" FieldName="Estado" VisibleIndex="3">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Opciones" ReadOnly="True" ShowInCustomizationForm="True"
                                                                        VisibleIndex="4">
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxHyperLink runat="server" ID="lnkNovedad" ImageUrl="../img/DxMarker.png"
                                                                                Cursor="pointer" ToolTip="Gestionar Novedad" OnInit="LinkNovedad_Init">
                                                                                <ClientSideEvents Click="function(s, e) { GestionarNovedad(this, {0}); }" />
                                                                            </dx:ASPxHyperLink>
                                                                            <dx:ASPxHyperLink runat="server" ID="lnkEliminar" ImageUrl="../img/DXEraser16.png"
                                                                                Cursor="pointer" ToolTip="Eliminar Novedad" OnInit="LinkNovedad_Init">
                                                                                <ClientSideEvents Click="function(s, e) { EliminarNovedad(this, {0}); }" />
                                                                            </dx:ASPxHyperLink>
                                                                        </DataItemTemplate>
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <Settings ShowTitlePanel="True" ShowHeaderFilterButton="False" ShowHeaderFilterBlankItems="False"
                                                                    ShowGroupPanel="false" />
                                                                <SettingsText CommandEdit="Editar" Title="Novedades Servicio" 
                                                                    EmptyDataRow="No se encontraron novedades asociadas al servicio" />
                                                                <SettingsBehavior EnableCustomizationWindow="true" AutoExpandAllGroups="true" />
                                                            </dx:ASPxGridView>
                                                        </dx:PanelContent> 
                                                    </PanelCollection>
                                                </dx:ASPxPanel>
                                            </dx:ContentControl>
                                        </ContentCollection> 
                                    </dx:TabPage> 
                                </TabPages> 
                            </dx:ASPxPageControl> 
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="dialogoGestion" runat="server" ClientInstanceName="dialogoGestion"
        Modal="true" HeaderText="Gestión Novedad" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ScrollBars="Auto" AllowDragging="True" CloseAction ="CloseButton">
        <ModalBackgroundStyle CssClass="modalBackground" />
        <ClientSideEvents EndCallback ="function (s, e){
            if(s.cpResultado==0){ 
                ActualizarEncabezado(s,e); 
            } if (s.cpResultado!=10) {
                ActualizarEncabezado(s,e); 
            } if (s.cpConsulta==1){
                var key = lblIdGestion.GetValue();
                gvNovedades.PerformCallback('Carga:' + key)
            }
        }" />
            <ContentCollection>
                <dx:PopupControlContentControl>
                    <table>
                        <tr>
                            <td colspan ="4">
                                <dx:ASPxLabel ID="lblNovedad" runat ="server" ClientInstanceName ="lblNovedad" ClientVisible ="false">
                                </dx:ASPxLabel> 
                            </td>
                        </tr>
                        <tr>
                            <td class ="field" align ="left" >
                                Seleccione Gestión:
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="cmbGestion" runat="server" ClientInstanceName="cmbGestion"
                                    ValueType="System.Int32" IncrementalFilteringMode="Contains" Width="200px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgGestion">
                                    <RequiredField ErrorText ="Información Requerida" IsRequired ="true" />
                                    </ValidationSettings> 
                                </dx:ASPxComboBox>
                            </td>
                            <td class ="field" align ="left">
                                Observación:
                            </td>
                            <td>
                                <dx:ASPxMemo ID="meGestion" runat="server" Height="50px" Width="270px" NullText="Ingrese una observación..."
                                    ClientInstanceName="meGestion">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgGestion">
                                        <RegularExpression ErrorText="Formato no valido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-]+\s*$" />
                                        <RequiredField ErrorText ="Observación Requerida" IsRequired ="true" />
                                    </ValidationSettings>
                                </dx:ASPxMemo>
                            </td>
                        </tr>
                        <tr>
                            <td colspan ="4" align ="center">
                                <dx:ASPxImage ID="imgGestiona" runat="server" ImageUrl="../img/DxConfirm32.png" ToolTip="Gestionar"
                                    Cursor="pointer">
                                    <ClientSideEvents Click ="function (s, e){
                                        if(ASPxClientEdit.ValidateGroup(&#39;vgGestion&#39;)){
                                            var key =lblNovedad.GetValue();
                                            dialogoGestion.PerformCallback('Gestionar:' + key);
                                        }
                                    }" />
                                </dx:ASPxImage> 
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
    </dx:ASPxPopupControl> 
    <dx:ASPxPopupControl ID="dialogoVerRadicar" runat="server" ClientInstanceName="dialogoVerRadicar"
        Modal="true" HeaderText="Radicación Documentos" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ScrollBars="Auto" AllowDragging="True" CloseAction ="CloseButton">
        <ClientSideEvents Closing ="function (s, e) { EjecutarCallbackGeneral(s, e, 'filtrarDatos'); }"/>
        <ContentCollection>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="dialogoVerPlanilla" runat="server" 
    ClientInstanceName="dialogoVerPlanilla" Modal="true" CloseAction ="CloseButton" 
        HeaderText="Visualización Planilla Radicación" PopupHorizontalAlign="WindowCenter" 
    PopupVerticalAlign="WindowCenter" ScrollBars="Auto" AllowDragging="True">
        <ContentCollection>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    <msgp:MensajePopUp ID="mensajero" runat="server" />
    <dx:ASPxCallback ID ="callback" runat ="server" ClientInstanceName ="callback">
        <ClientSideEvents EndCallback ="function (s, e){
            ActualizarEncabezado(s, e);
            if (s.cpResultado == 0){
                DescargarReporte();
            }
        }" />
    </dx:ASPxCallback>
    <div id="bluebar" class="menuFlotante">
        <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
        </b></b>
        <table style="width: 100%;">
            <tr>
                <td align ="right">
                    <dx:ASPxImage ID="imgExpandir" runat ="server" ClientInstanceName ="imgExpandir" ToolTip ="Expandir Todos Los Grupos"
                            ImageUrl ="../img/expandir.png">
                        <ClientSideEvents Click ="function (s, e){
                            gvVentas.PerformCallback('expandir');
                        }" />
                    </dx:ASPxImage>
                </td> 
                <td align="left">
                    <dx:ASPxImage ID="imgContraer" runat ="server" ClientInstanceName ="imgContraer" ToolTip ="Contraer Todos Los Grupos"
                            ImageUrl ="../img/contraer.png">
                        <ClientSideEvents Click ="function (s, e){
                            gvVentas.PerformCallback('contraer');
                        }" />
                    </dx:ASPxImage>
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
                            <RegularExpression ErrorText="Fall&#243; la validaci&#243;n de expresi&#243;n Regular">
                            </RegularExpression>
                            <RequiredField IsRequired="true" ErrorText="Formato a exportar requerido" />
                        </ValidationSettings>
                    </dx:ASPxComboBox>
                </td>
                <td align="right">
                    <dx:ASPxImage ID="imgExtendido" runat="server" ImageUrl="../img/MSExcel.png" ToolTip="Reporte Extendido"
                        Cursor="pointer" Height ="30%" Width ="30%">
                        <ClientSideEvents Click ="function (s, e){
                            ValidarFiltrosExtendido(s,e);
                        }" />
                    </dx:ASPxImage> 
                </td>
                <td>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Descargar Reporte Extendido."
                        CssClass="comentario" Width="170px" Font-Size="XX-Small" Font-Bold="False" Font-Italic="True"
                        Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                    </dx:ASPxLabel>
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
