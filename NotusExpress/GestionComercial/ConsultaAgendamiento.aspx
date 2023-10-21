<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConsultaAgendamiento.aspx.vb" Inherits="NotusExpress.ConsultaAgendamiento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../Estilos/estilo.css" rel="stylesheet" type="text/css" />
    <script src="../../../Jquery/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/FuncionesJS.js" type="text/javascript"></script>
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
        function Buscar(tipo) {
            grdProducto.PerformCallback();
        }
        function Nuevo() {
            $('#divEncabezado').html('Creando...');
            AjustarVentana();
            cpGeneral.PerformCallback("-1|0"); //Nuevo
        }
        function AjustarVentana() {
            TamanioVentana();
            ppProducto.SetSize(myWidth * 0.4, myHeight * 0.1);
            ppProducto.ShowWindow();
        }
        function FuncionBotones(s, e) {
            if (e.buttonID == "btnEditar") {
                AjustarVentana();
                var parametro = "-2|" + grdProducto.GetRowKey(grdProducto.GetFocusedRowIndex()).toString();
                $('#divEncabezado').html('Editando...');
                cpGeneral.PerformCallback(parametro);
            }
            if (e.buttonID == "btnEliminar") {
                if (confirm("¿Está seguro de eliminar este registro?")) {
                    var parametro = "-5|" + grdProducto.GetRowKey(grdProducto.GetFocusedRowIndex()).toString();
                    $('#divEncabezado').html('Eliminando...');
                    cpGeneral.PerformCallback(parametro);
                    grdProducto.PerformCallback();
                }
            }
        }
        function Guardar() {
            cpGeneral.PerformCallback('-3|0');
            grdProducto.PerformCallback();
        }
        function Actualizar() {
            cpGeneral.PerformCallback('-4|0');
            grdProducto.PerformCallback();
        }
        function ActivarOInactivarFiltros() {
            //ppZona.Hide();
            //rpFiltros.SetEnabled(!pucNuevoProducto.IsVisible());
        }
        function OnExpandCollapseButtonClick(s, e) {
            var isVisible = pnlFiltros.GetVisible();
            s.SetText(isVisible ? "+" : "-");
            pnlFiltros.SetVisible(!isVisible);
        }
        function AjustarVentanaImportar() {
            TamanioVentana();
            ppProductoImportar.SetSize(myWidth * 0.4, myHeight * 0.1);
            ppProductoImportar.ShowWindow();
        }
        function Importar() {
            $('#divEncabezado').html('Creando...');
            AjustarVentanaImportar();
        }
        //Operaciones con archivos
        function Uploader_OnUploadStart() {
            _aspxGetElementById("contenedorArchivo").innerHTML = "";
            btnGuardarArchivo.SetEnabled(false);
        }

        function Uploader_OnTextChanged(s, e) {
            s.Upload();
        }

        function Uploader_OnFilesUploadComplete(args) {
            //UpdateUploadButton();
        }

        function UpdateUploadButton() {
            btnGuardarArchivo.SetEnabled(uploader.GetText(0) != "");
        }

        function Uploader_OnFileUploadComplete(args) {
            if (args.isValid && ASPxClientEdit.ValidateGroup('cargarResultado')) { AdicionarArchivoAContenedor(args.callbackData); }
            btnGuardarArchivo.SetEnabled(args.isValid);
        }

        function AdicionarArchivoAContenedor(callbackData) {
            var data = callbackData;
            var label = document.createElement('span');
            _aspxGetElementById("contenedorArchivo").innerHTML = callbackData;
        }

        function LimpiarArchivoCargado(s, e) {
            if (s.cpLimpiarArchivo) {
                _aspxGetElementById("contenedorArchivo").innerHTML = "";
                btnGuardarArchivo.SetEnabled(false);
            }
        }
        //Operaciones con archivoss
    </script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="smAjaxManager" runat="server">
        </asp:ScriptManager>
        <div>
            <%-- <uc1:ValidacionURL ID="validadorUrl" runat="server" />--%>
        </div>
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
        <!-- Creacion y Actualizacion de Datos -->
        <div id="divEncabezado" style="visibility: hidden;">
        </div>
        <dx:ASPxCallbackPanel ID="cpGeneral" ClientInstanceName="cpGeneral" Width="1400px"
            runat="server">
            <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e); ActivarOInactivarFiltros();}" />
            <PanelCollection>
                <dx:PanelContent>
                    <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtros de B&uacute;squeda"
                        Width="900px">
                        <HeaderTemplate>
                            <table cellpadding="1" cellspacing="2" width="100%">
                                <tr>
                                    <td align="left">Filtros de B&uacute;squeda
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
                                    <PanelCollection>
                                        <dx:PanelContent>

                                            <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" ColCount="3">
                                                <Items>
                                                    <dx:LayoutItem Caption="Asesor Comercial">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server">
                                                                <dx:ASPxTextBox ID="txtAsesorComercialFiltro" runat="server" Width="170px" MaxLength="50">
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>

                                                    <dx:LayoutItem Caption="Ciudad">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server">
                                                                <dx:ASPxComboBox IncrementalFilteringMode="Contains" ID="ddlCiudadFiltro" ValueType="System.Int32" ValueField="IdCiudad" ClientInstanceName="ddlCiudadFiltro"
                                                                    runat="server" SelectedIndex="0">
                                                                </dx:ASPxComboBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="Identificación Cliente">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer7">
                                                                <dx:ASPxTextBox ID="txtIdentificacionClienteFiltro" runat="server" Width="170px" MaxLength="50">
                                                                </dx:ASPxTextBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="Estrategia">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer8">
                                                                <dx:ASPxComboBox IncrementalFilteringMode="Contains" ID="ddlEstrategiaComercialFiltro" ValueType="System.Int16" ValueField="IdEstrategia" TextField="Nombre" ClientInstanceName="ddlEstrategiaComercialFiltro"
                                                                    runat="server" SelectedIndex="0">
                                                                </dx:ASPxComboBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="Fecha Inicial">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer9">
                                                                <dx:ASPxDateEdit ID="txtFechaInicialFiltro" runat="server">
                                                                </dx:ASPxDateEdit>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="Fecha Final">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer18">
                                                                <dx:ASPxDateEdit ID="txtFechaFinalFiltro" runat="server">
                                                                </dx:ASPxDateEdit>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="Empresa">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer19">
                                                                <dx:ASPxComboBox IncrementalFilteringMode="Contains" ID="ddlEmpresaFiltro" ValueType="System.Int32" ValueField="IdEmpresa" TextField="Nombre" ClientInstanceName="ddlEmpresaFiltro"
                                                                    runat="server" SelectedIndex="0">
                                                                </dx:ASPxComboBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="Estado">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server">
                                                                <dx:ASPxComboBox IncrementalFilteringMode="Contains" ID="ddlEstadoFiltro" TextField="Nombre" ValueField="IdEstado" ClientInstanceName="ddlEstadoFiltro"
                                                                    runat="server" SelectedIndex="0">
                                                                </dx:ASPxComboBox>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="Layout Item" ColSpan="3" ShowCaption="False">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer4" runat="server">
                                                                <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" ColCount="5">
                                                                    <Items>
                                                                        <dx:LayoutItem Caption="Layout Item" ShowCaption="False">
                                                                            <LayoutItemNestedControlCollection>
                                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer5" runat="server">
                                                                                    <dx:ASPxButton ID="btnLimpiar" runat="server" AutoPostBack="False" Image-Url="<%$ Resources:Recursos, BotonLimpiar %>" Text="Limpiar">
                                                                                    </dx:ASPxButton>
                                                                                </dx:LayoutItemNestedControlContainer>
                                                                            </LayoutItemNestedControlCollection>
                                                                        </dx:LayoutItem>
                                                                        <dx:LayoutItem Caption="Layout Item" ShowCaption="False">
                                                                            <LayoutItemNestedControlCollection>
                                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer6" runat="server">
                                                                                    <dx:ASPxButton ID="btnConsultar" runat="server" AutoPostBack="False" Image-Url="<%$ Resources:Recursos, BotonConsultar %>" Text="Consultar" ValidationGroup="filtrar">
                                                                                        <ClientSideEvents Click="function(s, e) { cpGeneral.PerformCallback('Consultar|1');}" />
                                                                                    </dx:ASPxButton>
                                                                                </dx:LayoutItemNestedControlContainer>
                                                                            </LayoutItemNestedControlCollection>
                                                                        </dx:LayoutItem>
                                                                        <dx:LayoutItem Caption="Layout Item" ShowCaption="False">
                                                                            <LayoutItemNestedControlCollection>
                                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer10" runat="server">
                                                                                    <dx:ASPxButton ID="btnAdicionar" runat="server" AutoPostBack="False" Image-Url="<%$ Resources:Recursos, BotonAdicionar %>" Text="Adicionar">
                                                                                        <ClientSideEvents Click="function(s, e){ Nuevo(); }" />
                                                                                    </dx:ASPxButton>
                                                                                </dx:LayoutItemNestedControlContainer>
                                                                            </LayoutItemNestedControlCollection>
                                                                        </dx:LayoutItem>
                                                                        <dx:LayoutItem Caption="Layout Item" ShowCaption="False">
                                                                            <LayoutItemNestedControlCollection>
                                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer11" runat="server">
                                                                                    <dx:ASPxButton ID="btnImportar" Width="100px" Height="30px" runat="server" Text="Importar"
                                                                                        Style="display: inline" AutoPostBack="false" Image-Url="<%$ Resources:Recursos, BotonImportar %>">
                                                                                        <ClientSideEvents Click="function(s, e){ Importar(); }"></ClientSideEvents>
                                                                                    </dx:ASPxButton>
                                                                                </dx:LayoutItemNestedControlContainer>
                                                                            </LayoutItemNestedControlCollection>
                                                                        </dx:LayoutItem>
                                                                        <dx:LayoutItem Caption="Layout Item" ShowCaption="False">
                                                                            <LayoutItemNestedControlCollection>
                                                                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer12" runat="server">
                                                                                </dx:LayoutItemNestedControlContainer>
                                                                            </LayoutItemNestedControlCollection>
                                                                        </dx:LayoutItem>
                                                                    </Items>
                                                                </dx:ASPxFormLayout>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                </Items>
                                            </dx:ASPxFormLayout>

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxPanel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                    <br />
                    <dx:ASPxGridView ID="grdProducto" Width="1100px" SettingsPager-PageSize="<%$ Resources:Recursos, GrillaCantidadPaginas %>"
                        runat="server" AutoGenerateColumns="False" ClientInstanceName="grdProducto" KeyFieldName="idProducto">
                        <Columns>
                            <dx:GridViewCommandColumn ButtonType="Image" FixedStyle="Left" Caption="Opciones"
                                VisibleIndex="0">
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btnEditar">
                                        <Image ToolTip="Editar" Url="<%$ Resources:Recursos, BotonEditar %>">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="idProducto" VisibleIndex="1" Visible="false" Caption="idProducto">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nombre" VisibleIndex="2" Caption="Nombre">
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nombreProductoSerprinter" VisibleIndex="2" Caption="Nombre Serprinter">
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="servicio" VisibleIndex="2" Caption="Servicio">
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nombreTipoServicio" VisibleIndex="2" Caption="Tipo de Servicio">
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="cantidadMaxima" VisibleIndex="2" Caption="Cant.Máxima">
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="fechaCreacion" VisibleIndex="3" Caption="F.Creación">
                                <HeaderStyle HorizontalAlign="Center" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nombreEstado" VisibleIndex="4" Caption="Estado">
                                <HeaderStyle HorizontalAlign="Center" />
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <ClientSideEvents CustomButtonClick="function(s, e){ FuncionBotones(s,e) }" />
                        <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="Control"></SettingsBehavior>
                        <SettingsPager>
                            <AllButton Visible="True">
                            </AllButton>
                        </SettingsPager>
                        <Settings ShowHeaderFilterButton="true" ShowFilterBar="Visible" ShowFilterRowMenu="true"
                            ShowGroupButtons="true" ShowGroupPanel="True" />
                    </dx:ASPxGridView>
                    <dx:ASPxGridView ID="grdLog" Caption="Log de Errores" runat="server" SettingsPager-PageSize="100"
                        AutoGenerateColumns="True" ClientInstanceName="grdResultado" KeyFieldName="Linea"
                        Width="100%" Visible="False">
                        <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="Control" />
                        <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="Control"></SettingsBehavior>
                        <SettingsPager>
                            <AllButton Visible="True">
                            </AllButton>
                        </SettingsPager>
                        <Settings ShowFilterBar="Hidden" ShowFilterRowMenu="False" ShowGroupPanel="False"
                            ShowHeaderFilterButton="False" />
                    </dx:ASPxGridView>
                    <dx:ASPxPopupControl ID="ppProductoImportar" runat="server" AllowDragging="True" ClientInstanceName="ppProductoImportar"
                        HeaderText="Importación de Productos" Modal="True" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" Width="750px">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                <!-- popup-->
                                <dx:ASPxFormLayout ID="formaPreventa" runat="server" Width="100%">
                                    <Items>
                                        <dx:LayoutGroup GroupBoxDecoration="None" ColCount="1" SettingsItemCaptions-Location="Left">
                                            <Items>
                                                <dx:LayoutItem Caption="Adjunto (Máximo 10 MB) xls, xlsx">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer13">
                                                            <dx:ASPxUploadControl ID="uplAdjunto" runat="server" ClientInstanceName="uploader"
                                                                ShowProgressPanel="True" NullText="Haga click aquí para buscar archivos..." Size="50">
                                                                <ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
                                                                    FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
                                                                    TextChanged="Uploader_OnTextChanged"></ClientSideEvents>
                                                                <ValidationSettings AllowedFileExtensions=".xls,.xlsx" GeneralErrorText="Falló el cargue del archivo debido a un error interno. Por favor intente nuevamente"
                                                                    MaxFileSizeErrorText="El tamaño del archivo excede el tamaño máximo permitido, que es {0} bytes"
                                                                    NotAllowedFileExtensionErrorText="Esta extensión de archivo no está permitida"
                                                                    MaxFileSize="10485760">
                                                                </ValidationSettings>
                                                            </dx:ASPxUploadControl>
                                                            <fieldset class="groupBox" style="padding-top: 4px">
                                                                <legend>Archivo Cargado</legend>
                                                                <div id="contenedorArchivo" class="contenedorArchivo">
                                                                </div>
                                                            </fieldset>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Botones" ColSpan="1" ShowCaption="False">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer14" runat="server">
                                                            <div class="boton" style="float: right">
                                                                <dx:ASPxButton ID="btnGuardarArchivo" ClientInstanceName="btnGuardarArchivo" runat="server"
                                                                    Image-Url="<%$ Resources:Recursos, BotonImportar %>" AutoPostBack="False" Height="30px"
                                                                    Text="Enviar" Width="100px">
                                                                    <ClientSideEvents Click="function (s, e){
                                                                             if(ASPxClientEdit.ValidateGroup('Guardar')){
                                                                                cpGeneral.PerformCallback('Enviar|1');loadingPanel.Show();
                                                                                }
                                                                            }" />
                                                                    <ClientSideEvents Click="function (s, e){
                                                                             if(ASPxClientEdit.ValidateGroup(&#39;Guardar&#39;)){
                                                                                cpGeneral.PerformCallback(&#39;Enviar|1&#39;);loadingPanel.Show();
                                                                                }
                                                                            }"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                            </div>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                            <SettingsItemCaptions Location="Left"></SettingsItemCaptions>
                                        </dx:LayoutGroup>
                                    </Items>
                                    <Border BorderStyle="Solid" BorderWidth="1px" />
                                    <Border BorderStyle="Solid" BorderWidth="1px"></Border>
                                </dx:ASPxFormLayout>
                                <!-- popup-->
                                <asp:TextBox ID="txtIdSolicitud" runat="server" Style="display: none;"></asp:TextBox>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <dx:ASPxPopupControl ID="ppProducto" runat="server" Modal="True" Width="500px" ClientInstanceName="ppProducto"
                        PopupHorizontalAlign="WindowCenter" AllowDragging="True" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true"
                        HeaderText="Guardar/Actualizar">
                        <ContentCollection>
                            <dx:PopupControlContentControl>
                                <dx:ASPxFormLayout ID="ASPxFormLayoutProductoEditar" runat="server" Width="100%">
                                    <Items>
                                        <dx:LayoutGroup GroupBoxDecoration="None" ColCount="1" SettingsItemCaptions-Location="Left">
                                            <Items>
                                                <dx:LayoutItem Caption="Nombre">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer>
                                                            <dx:ASPxTextBox ID="txtNombreEditar" runat="server" Width="100%" MaxLength="50">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="El campo nombre es requerido"
                                                                    SetFocusOnError="true" ValidationGroup="Guardar" ErrorText="El campo nombre es requerido"
                                                                    ErrorTextPosition="Right" RequiredField-IsRequired="true">
                                                                    <RequiredField IsRequired="True"></RequiredField>
                                                                    <ErrorImage AlternateText="El campo nombre es requerido"
                                                                        ToolTip="El campo nombre es requerido">
                                                                    </ErrorImage>
                                                                    <RegularExpression ValidationExpression="<%$ Resources:Recursos, ValidaSoloLetrasyNumeros %>"
                                                                        ErrorText="El texto digitado contiene caracteres no permitidos." />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Nombre Serprinter">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer>
                                                            <dx:ASPxTextBox ID="txtNombreSeprinterEditar" runat="server" Width="100%" MaxLength="50">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="El campo nombre serprinter es requerido"
                                                                    SetFocusOnError="true" ValidationGroup="Guardar" ErrorText="El campo nombre serprinter es requerido"
                                                                    ErrorTextPosition="Right" RequiredField-IsRequired="true">
                                                                    <RequiredField IsRequired="True"></RequiredField>
                                                                    <ErrorImage AlternateText="El campo nombre serprinter es requerido"
                                                                        ToolTip="El campo nombre serprinter es requerido">
                                                                    </ErrorImage>

                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Tipo Servicio">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer15">
                                                            <dx:ASPxComboBox ID="ddlTipoServicioEditar" runat="server" SelectedIndex="0" TextField="nombre" ValueField="idtiposervicio" ValueType="System.Int16">
                                                            </dx:ASPxComboBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Servicio">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer16">
                                                            <dx:ASPxComboBox ID="ddlServicioEditar" runat="server" SelectedIndex="0" TextField="Nombre" ValueField="Idservicio" ValueType="System.Int32">
                                                            </dx:ASPxComboBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Cantidad Máxima">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer>
                                                            <dx:ASPxTextBox ID="txtCantidadMaxima" runat="server" Width="100%" MaxLength="50">
                                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="El campo cant. máxima serprinter es requerido"
                                                                    SetFocusOnError="true" ValidationGroup="Guardar" ErrorText="El campo cant. máxima es requerido"
                                                                    ErrorTextPosition="Right" RequiredField-IsRequired="true">
                                                                    <RequiredField IsRequired="True"></RequiredField>
                                                                    <ErrorImage AlternateText="El campo cant. máxima es requerido"
                                                                        ToolTip="El campo cant. máxima es requerido">
                                                                    </ErrorImage>
                                                                    <RegularExpression ValidationExpression="<%$ Resources:Recursos, ValidaSoloNumerosPositivos %>"
                                                                        ErrorText="El texto digitado contiene caracteres no permitidos." />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Estado">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer ID="filaEstado">
                                                            <dx:ASPxComboBox ID="ddlEstadoEditar" runat="server" SelectedIndex="0" TextField="Nombre" ValueField="IdEstado" ValueType="System.Int16">
                                                            </dx:ASPxComboBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Botones" ColSpan="1" ShowCaption="False">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer17" runat="server">
                                                            <table cellpadding="0" style="width: 100%">
                                                                <tr align="center">
                                                                    <td style="width: 50%">
                                                                        <dx:ASPxButton ID="btnAdicionarActualizar" runat="server" AutoPostBack="False" Style="display: inline"
                                                                            Image-Url="<%$ Resources:Recursos, BotonGuardar %>" Text="<%$ Resources:Recursos, Guardar %>"
                                                                            ValidationGroup="Guardar" Width="80px">
                                                                            <ClientSideEvents Click="function(s, e) {
                                                        if(ASPxClientEdit.ValidateGroup('Guardar')){
                                                            if ($('#divEncabezado').html()=='Editando...')
                                                            {
                                                                Actualizar();
                                                                ppProducto.Hide();
                                                            }
                                                            else
                                                            {
                                                                Guardar();
                                                                ppProducto.Hide();
                                                            }
                                                        }
                                                    }" />
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                    <td style="width: 50%">
                                                                        <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" Style="display: inline"
                                                                            ClientInstanceName="btnCancel" Text="Cancelar" Width="80px" Image-Url="<%$ Resources:Recursos, BotonCancelar %>">
                                                                            <ClientSideEvents Click="function(s, e) {
                                                        ASPxClientEdit.ClearGroup('Guardar');
	ppProducto.Hide();
    
}" />
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                            <SettingsItemCaptions Location="Left"></SettingsItemCaptions>
                                        </dx:LayoutGroup>
                                    </Items>
                                </dx:ASPxFormLayout>
                                <asp:TextBox ID="txtId" runat="server" Style="display: none;"></asp:TextBox>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <msgp:MensajePopUp ID="mensajero" runat="server" />
                    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
                        Modal="True">
                    </dx:ASPxLoadingPanel>
                </dx:PanelContent>
            </PanelCollection>
            <LoadingDivStyle CssClass="modalBackground">
            </LoadingDivStyle>
        </dx:ASPxCallbackPanel>
        <!-- Consulta de Datos -->
    </form>
</body>
</html>
