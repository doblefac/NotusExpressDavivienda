<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditarCampaniaFinanciero.aspx.vb" Inherits="NotusExpress.EditarCampaniaFinanciero" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: Editar Campañas Financieras :: </title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        function EjecutarCallbackRegistro(s, e, parametro, valor) {
            if (ASPxClientEdit.AreEditorsValid()) {
                LoadingPanel.Show();
                cpRegistro.PerformCallback(parametro + ':' + valor);
            }
        }

        function ValidaNumero(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58));
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
    <div id="divEncabezado">
        <uc1:EncabezadoPagina ID="miEncabezado" runat="server" />
    </div>
    <dx:ASPxCallbackPanel ID="cpRegistro" runat="server"  ClientInstanceName ="cpRegistro">
            <ClientSideEvents EndCallback="function(s,e){ 
            $('html, body').animate({ scrollTop: 0 }, 'slow');
            $('#divEncabezado').html(s.cpMensaje);
            LoadingPanel.Hide();
        }"></ClientSideEvents>
        
        <PanelCollection>
            <dx:PanelContent>
                <asp:Label ID="lblMensajeGenerico" runat="server" ></asp:Label>
                <dx:ASPxRoundPanel ID="rpCampania" runat="server" HeaderText="Editar Campañas Financiero"
                    Width="70%" Theme="SoftOrange">
                    <PanelCollection>
                        <dx:PanelContent>
                            <dx:ASPxPanel ID="pnlDatos" runat="server" Width="100%" ClientInstanceName="pnlDatos">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <table>
                                                <tr>
                                                    <td class="field" align="left">
                                                        Nombre Campaña:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtNombreCampania" runat="server" Width="150px" TabIndex="1">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                <RequiredField ErrorText="El nombre de la campaña  es requerido" IsRequired="True" />
                                                                <RegularExpression ErrorText="Formato no valido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td class ="field" align ="left">
                                                        Cliente:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="cmbCl" runat="server" Width="150px" IncrementalFilteringMode="Contains"
                                                            ClientInstanceName="cmbCl" DropDownStyle="DropDownList" TabIndex="2" ValueType ="System.Int32">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="nombre" Width="250px" Caption="Descripción" />
                                                            </Columns>
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                <RequiredField ErrorText="El cliente de la campaña  es requerido" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field" align="left">
                                                        Fecha Vigencia:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxDateEdit ID="dateFechaInicio" runat="server" NullText="Inicial..." ClientInstanceName="dateFechaInicio"
                                                            Width="100px" TabIndex="3">
                                                            <ClientSideEvents ValueChanged="function(s, e){
                                                                dateFechaFin.SetMinDate(dateFechaInicio.GetDate());
                                                            }" />
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                <RequiredField IsRequired="true" ErrorText="Registro requerido" />
                                                            </ValidationSettings>
                                                        </dx:ASPxDateEdit>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxDateEdit ID="dateFechaFin" runat="server" NullText="Final..." ClientInstanceName="dateFechaFin"
                                                            Width="100px" TabIndex="4">
                                                            <ClientSideEvents ValueChanged="function(s, e){
                                                                dateFechaInicio.SetMaxDate(dateFechaFin.GetDate());
                                                            }" />
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                <RequiredField IsRequired="true" ErrorText="Registro requerido" />
                                                            </ValidationSettings>
                                                        </dx:ASPxDateEdit>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxCheckBox ID="cbActivo" runat="server">
                                                        </dx:ASPxCheckBox>
                                                        <div>
                                                            <dx:ASPxLabel ID="lblComentario" runat="server" Text="Activo S/N."
                                                                CssClass="comentario" Width="80px" Font-Size="XX-Small" Font-Bold="False" Font-Italic="True"
                                                                Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                    <tr>
                                                        <td class="field" align="left" style="width: 30%">Meta Cliente:
                                                        </td>
                                                        <td style="width: 40%">
                                                            <div style="display: inline; float: left; width: 80%">
                                                                <dx:ASPxTextBox ID="txtMetaCliente" runat="server" ClientInstanceName="txtMetaCliente" Width="125px" MaxLength="3" TabIndex="5"
                                                                    onkeypress="javascript:return ValidaNumero(event);">
                                                                    <ClientSideEvents LostFocus ="function(s,e){
                                                                        if (txtMetaCliente.GetText() != ''){
                                                                            var meta = txtMetaCliente.GetText()
                                                                            if ( meta >= 101){
                                                                                alert('El valor del porcentaje de la meta de Davivienda no puede ser mayor a 100');
                                                                                txtMetaCliente.SetText('');
                                                                                txtMetaCliente.SetFocus();
                                                                            }  else if (meta <= 0) { 
                                                                                alert('El valor del porcentaje de la meta deDavivienda debe ser mayor a 0');
                                                                                txtMetaCliente.SetText('');
                                                                                txtMetaCliente.SetFocus();                                                                        
                                                                            }
                                                                        }
                                                                    }" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                        <RequiredField ErrorText="la Meta de cumplimiento del cliente es requerido" IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                            <div style="display: inline; float: left; align-items: center">
                                                                %
                                                            </div>
                                                        </td>
                                                        <td class="field" align="left" style="width: 100%">Meta Callcenter:
                                                        </td>
                                                        <td>
                                                            <div style="display: inline; float: left; width: 80%">
                                                                <dx:ASPxTextBox ID="txtMetaCall" runat="server" ClientInstanceName="txtMetaCall" Width="125px" MaxLength="3" TabIndex="6"
                                                                    onkeypress="javascript:return ValidaNumero(event);">
                                                                    <ClientSideEvents LostFocus ="function(s,e){
                                                                        if (txtMetaCall.GetText() != ''){
                                                                            var metaCall = txtMetaCall.GetText()
                                                                            if (metaCall >= 101){
                                                                                alert('El valor del porcentaje de la meta del Callcenter no puede ser mayor a 100');
                                                                                txtMetaCall.SetText('');
                                                                                txtMetaCall.SetFocus();
                                                                            } else if (metaCall <= 0) { 
                                                                                alert('El valor del porcentaje de la meta del Callcenter debe ser mayor a 0');
                                                                                txtMetaCall.SetText('');
                                                                                txtMetaCall.SetFocus();                                                                        
                                                                            }
                                                                        }
                                                                    }" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                        <RequiredField ErrorText="la Meta de cumplimiento del Callcenter es requerido" IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                            <div style="display: inline; float: left">
                                                                %
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="field" align="left" style="width: 30%">Tipo de Campaña:
                                                        </td>
                                                        <td style="width: 40%">
                                                            <dx:ASPxComboBox ID="cmbTipoCampania" runat="server" Width="100%" IncrementalFilteringMode="Contains"
                                                                ClientInstanceName="cmbTipoCampania" DropDownStyle="DropDownList" TabIndex="7" ValueType="System.Int32">
                                                                  <Columns>
                                                                    <dx:ListBoxColumn FieldName="tipoCampania" Width="250px" Caption="Tipo" />
                                                                </Columns>
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                    <RequiredField ErrorText="El tipo de campaña  es requerido" IsRequired="True" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                        <td class="field" align="left" style="width: 30%">Producto personalizado:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxRadioButtonList ID="rblPersonalizacion" runat="server" RepeatDirection="Horizontal" TabIndex="8"
                                                                ClientInstanceName="rblPersonalizacion" Font-Size="XX-Small" Height="10px" AutoPostBack="false">
                                                                <Items>
                                                                    <dx:ListEditItem Text="Con Realce" Value="1" />
                                                                    <dx:ListEditItem Text="Sin Realce" Value="0" Selected="true" />
                                                                </Items>
                                                                <Border BorderStyle="None"></Border>
                                                                <ClientSideEvents ValueChanged="function(s,e){
                                                                    if (rblPersonalizacion.GetValue()==1){
                                                                        $('#cpRegistro_rpCampania_pnlDatos_divTr').css('display', 'inline');
                                                                    } else {
                                                                        $('#cpRegistro_rpCampania_pnlDatos_divTr').css('display', 'none');
                                                                    }
                                                                }" />
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                                    <RequiredField ErrorText ="Información Requerida" IsRequired ="true" />
                                                                </ValidationSettings> 
                                                            </dx:ASPxRadioButtonList>
                                                        </td>
                                                    </tr>
                               
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id="divTr" runat="server" style="display:inline">
                                                                <table>
                                                                    <tr>
                                                                         <td class="field" align="left" style="width: 30%">Fecha de Entrega:
                                                                         </td>
                                                                        <td  style="width: 40%">
                                                                            <dx:ASPxDateEdit ID="dateFechaLlegada" runat="server" NullText="Fecha..." ClientInstanceName="dateFechaLlegada"
                                                                                Width="85%" TabIndex="9">
                                                                            </dx:ASPxDateEdit>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                <caption>
                                                    <br />
                                                    <tr>
                                                        <td style="width: 30%;"><span id="spnCargueUsuariosFueraBase">Cargar usuarios fuera de la base</span> </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCargueUsuariosFueraBase" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="4">
                                                            <dx:ASPxPageControl ID="pcAsociadosCampania" runat="server" ActiveTabIndex="0" Width="100%">
                                                                <TabPages>
                                                                    <dx:TabPage Text="Tipo Servicio">
                                                                        <TabImage Url="../img/structure.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl runat="server">
                                                                                <dx:ASPxPanel ID="pnlServicios" runat="server" Height="250px" ScrollBars="Auto">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent runat="server">
                                                                                            <dx:ASPxListBox ID="lbServicios" runat="server" ClientInstanceName="lbServicios" Height="250px" ValueField="IdTipoServicio" Width="250px">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn Caption="Tipo Servicio" FieldName="Nombre" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dx:ASPxPanel>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>
                                                                    <dx:TabPage Text="Bodegas CEM">
                                                                        <TabImage Url="../img/list_num.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl runat="server">
                                                                                <dx:ASPxPanel ID="pnlBodega" runat="server" Height="250px" ScrollBars="Auto">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent runat="server">
                                                                                            <dx:ASPxListBox ID="lbBodegas" runat="server" ClientInstanceName="lbBodegas" Height="250px" SelectionMode="CheckColumn" ValueField="IdBodega" Width="250px">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn Caption="Bodega" FieldName="bodega" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dx:ASPxPanel>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>
                                                                    <dx:TabPage Text="Producto Externo">
                                                                        <TabImage Url="../img/DxPikingList.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl runat="server">
                                                                                <dx:ASPxPanel ID="pnlProductoExt" runat="server" Height="250px" ScrollBars="Auto">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent runat="server">
                                                                                            <dx:ASPxListBox ID="lbProductoExt" runat="server" ClientInstanceName="lbProductoExt" Height="250px" SelectionMode="CheckColumn" ValueField="IdProductoComercial" Width="250px">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn Caption="Producto" FieldName="ProductoExterno" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dx:ASPxPanel>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>
                                                                    <dx:TabPage Text="Documentos">
                                                                        <TabImage Url="../img/documents_stack.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl runat="server">
                                                                                <dx:ASPxPanel ID="pnlDocumentos" runat="server" Height="250px" ScrollBars="Auto">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent runat="server">
                                                                                            <dx:ASPxListBox ID="lbDocumentos" runat="server" ClientInstanceName="lbDocumentos" Height="250px" SelectionMode="CheckColumn" ValueField="IdProducto" Width="250px">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn Caption="Documentos" FieldName="Nombre" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>
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
                                                    <tr>
                                                        <td align="center" colspan="4">
                                                            <dx:ASPxImage ID="imgCrear" runat="server" Cursor="pointer" ImageUrl="../img/DxConfirm16.png" ToolTip="Actualizar Campaña">
                                                                <ClientSideEvents Click="function (s, e){
                                                                if(ASPxClientEdit.ValidateGroup('vgCampania')){
                                                                    if(lbServicios.GetSelectedValues().length==0 || lbBodegas.GetSelectedValues().length==0 || lbProductoExt.GetSelectedValues().length==0 || lbDocumentos.GetSelectedValues().length==0){
                                                                        alert('No se han seleccionado todos los valores requeridos, por favor verifique que esten seleccionados Servicios, Ciudades, Productos y Documentos.');
                                                                    } else {
                                                                        EjecutarCallbackRegistro(s,e,'Actualizar');
                                                                    }
                                                                }
                                                            }" />
                                                            </dx:ASPxImage>
                                                        </td>
                                                    </tr>
                                                </caption>
                                            </table>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxPanel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel> 
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel> 
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
