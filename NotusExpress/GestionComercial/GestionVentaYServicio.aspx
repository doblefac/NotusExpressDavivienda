<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestionVentaYServicio.aspx.vb"
    Inherits="NotusExpress.GestionVentaYServicio" %>

<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<script type="text/javascript" src="../Scripts/jquery-1.12.4.js"></script>
<%@ Register Src="~/ControlesDeUsuario/SelectorDireccion.ascx" TagName="AdressSelector" TagPrefix="as4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Notus Express - Gestión de Servicio::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .style1 {
            width: 259px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function checkedGestion() {
            if (cbModificarDireccion.GetVisible()) {
                if (cbModificarDireccion.GetChecked()) {
                    cmbCiudad1.SetEnabled(true)
                    txtDireccionResidencia.SetEnabled(true)
                } else {
                    cmbCiudad1.SetEnabled(false)
                    txtDireccionResidencia.SetEnabled(false)
                }
            } else {
                cmbCiudad1.SetEnabled(true)
                txtDireccionResidencia.SetEnabled(true)
            }
            
        }

        function Seleccionar(s, e) {
            s.SelectAll();
        }
        function LimpiaFormularioInicial() {
            loadingPanel.Hide();
        }

    </script>
</head>
<body>

    <form id="formPrincipal" runat="server">
        <%----%>
        <div id="divPrincipal" runat="server">
            <asp:HiddenField runat="server" ID="hfIdCampania" Value="" />
            <div id="divEncabezado">
                <epg:EncabezadoPagina ID="epNotificador" runat="server" />
            </div>
            <dx:ASPxCallbackPanel ID="cpGeneral" runat="server"  Height="420">
                <ClientSideEvents EndCallback="function(s,e){ 
                ActualizarEncabezado(s,e); 
                checkedGestion();
                if (s.cpMensajeGeneral.length != 0){
                    var mensaje = s.cpMensajeGeneral.split('|');
                    var color = 'amarillo';
                    color = mensaje[1];
                    alert(mensaje[0], color)
                }
            }"></ClientSideEvents>
                <PanelCollection>
                    <dx:PanelContent>
                        <table>
                            <tr>
                                <td rowspan="2">
                                    <div style="float: left; margin-top: 5px; width: 50%;">
                                        <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Datos de Usuario" Width="80%" DefaultButton="btnRegistra">
                                            <PanelCollection>
                                                <dx:PanelContent>
                                                    <table width="100%" style="padding: 10px 10px 10px 10px; border-width: medium; background-color: #e3e3e3; border-radius: 10px 10px 10px 10px; -moz-border-radius: 10px 10px 10px 10px; -webkit-border-radius: 10px 10px 10px 10px; border: 0px solid #d900d9;">
                                                        <tr>
                                                            <td colspan="3" valign="top">
                                                                <dx:ASPxLabel ID="lblCamposObligatorios" runat="server" Text="Los campos señalados con un  asterisco (*) son obligatorios."
                                                                    CssClass="comentario" Font-Size="X-Small" Font-Bold="False" Font-Italic="True"
                                                                    Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td align="right">
                                                                <dx:ASPxButton ID="imgAgregarCliente" ClientVisible="false" runat="server" Cursor="pointer" Text="Agregar Cliente" AutoPostBack="false">
                                                                    <ClientSideEvents Click="function (s, e){
                                                                                              if(txtNumIdentificacion.GetValue()== null ){
                                                                                            
                                                                                                    alert('Por favor digite el número de cedula antes de guardar la información.');
                                                                                                } else {
                                                                                                    if(confirm('Esta seguro que desea cargar cliente fuera de base?')){
                                                                                                        
                                                                                                        cpGeneral.PerformCallback('AgregarClienteFueraBase');   

                                                                                                    }
                                                                                                } 
                                                                                            }" />
                                                                    <Image Url="../img/agregarUsuario.png"></Image>
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th colspan="4">Datos de la Venta
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td class="field" colspan="1">Tipo Identificación:
                                                            </td>
                                                            <td colspan="1" class="style1">

                                                                <dx:ASPxComboBox ID="cbTipoId" runat="server" ValueType="System.Int32" ClientInstanceName="cbTipoId"
                                                                    Width="250px" TabIndex="2">
                                                                    <Columns>
                                                                        <dx:ListBoxColumn FieldName="descripcion" Width="300px" Caption="Descripción" />
                                                                    </Columns>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Tipo identificación requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>

                                                            </td>
                                                            <td class="field">No. de Identificaci&oacute;n:
                                                            </td>
                                                            <td>

                                                               
                                                                <dx:ASPxTextBox ID="txtNumIdentificacion" runat="server" Width="250px" MaxLength="15" ClientInstanceName="txtNumIdentificacion"
                                                                    ReadOnly="False" TabIndex="3">
                                                                    <ClientSideEvents LostFocus="function(s, e) { EjecutarCallbackGeneral(s,e,'ConsultarCliente');}" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta" ErrorText="+">
                                                                        <RegularExpression ErrorText="Los caracteres ingresados no son validos" ValidationExpression="^\s*[a-zA-Z_0-9]+\s*$" />
                                                                        <RequiredField ErrorText="Número identificación requerido" IsRequired="true" />

                                                                    </ValidationSettings>

                                                                </dx:ASPxTextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="field" height="40px">Campaña:
                                                            </td>
                                                            <td colspan="3">
                                                                <dx:ASPxComboBox ID="cmbCampania" runat="server" ValueType="System.Int32" AutoPostBack="False"
                                                                    ClientInstanceName="cmbCampania" IncrementalFilteringMode="Contains" TabIndex="3" Width="653px" Height="35px" ValueField="IdCampania">
                                                                    <ClientSideEvents TextChanged="function (s, e){
                                                                                EjecutarCallbackGeneral(s, e, 'ConsultarCliente', 1)
                                                                            }" />
                                                                    <Columns>
                                                                        <dx:ListBoxColumn FieldName="IdCampania" Width="70px" Caption="Id" />
                                                                        <dx:ListBoxColumn FieldName="Nombre" Width="300px" Caption="Descripción" />
                                                                    </Columns>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="field">Nombres:
                                                            </td>
                                                            <td class="style1">

                                                                <dx:ASPxTextBox ID="txtNombres" runat="server" Width="250px" MaxLength="50" TabIndex="4"
                                                                    onkeypress="return soloLetras(event)">
                                                                    <ClientSideEvents GotFocus="Seleccionar" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>

                                                            </td>
                                                            <td class="field">Primer Apellido:
                                                            </td>
                                                            <td class="style1">

                                                                <dx:ASPxTextBox ID="txtPrimerApellido" runat="server" Width="250px" MaxLength="50"
                                                                    TabIndex="4" onkeypress="return soloLetras(event)">
                                                                    <ClientSideEvents GotFocus="Seleccionar" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="field">Segundo Apellido:
                                                            </td>
                                                            <td>

                                                                <dx:ASPxTextBox ID="txtSegundoApellido" runat="server" Width="250px" MaxLength="50"
                                                                    TabIndex="5" onkeypress="return soloLetras(event)">
                                                                    <ClientSideEvents GotFocus="Seleccionar" />
                                                                    <%--<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                            </ValidationSettings>--%>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td class="field">Teléfono Fijo:
                                                            </td>

                                                            <td>

                                                                <dx:ASPxTextBox ID="txtTelFijo" runat="server" Width="250px" MaxLength="10" TabIndex="6" ClientInstanceName="txtTelFijo"
                                                                    onkeypress="return solonumeros(event)">
                                                                    <ClientSideEvents GotFocus="Seleccionar" LostFocus="function (s, e){
                                                                                                if(parseInt(txtTelFijo.GetValue()) == '0' ){
                                                                                                    alert('Por favor digite un numero de telefono valido');
                                                                                                    txtTelFijo.SetText('');
                                                                                                }
                                                                                            }" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>

                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="field">Ciudad Residencia:
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="cmbCiudad1" runat="server" ClientInstanceName="cmbCiudad1" Width="250" ValueType="System.Int32"
                                                                    IncrementalFilteringMode="Contains" CallbackPageSize="25" EnableCallbackMode="true" TabIndex="8">
                                                                    <ClientSideEvents SelectedIndexChanged="function (s, e){                                                                                                                                
                                                                                            var seleccion = cmbCiudad1.GetValue();                                                                                
                                                                                            cpGeneral.PerformCallback('Ciudad:' + seleccion )
                                                                                         }"></ClientSideEvents>
                                                                    <Columns>
                                                                        <dx:ListBoxColumn FieldName="IdCiudad" Width="70px" Caption="Id" />
                                                                        <dx:ListBoxColumn FieldName="CiudadDepartamento" Width="300px" Caption="Ciudad" />
                                                                    </Columns>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Dato requerido" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                                <div>
                                                                    <dx:ASPxLabel ID="lblComentario" runat="server" Text="Digite parte de la ciudad."
                                                                        CssClass="comentario" Width="270px" Font-Size="XX-Small" Font-Bold="False" Font-Italic="True"
                                                                        Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                                                                    </dx:ASPxLabel>
                                                                </div>
                                                            </td>
                                                            <td class="field">Dirección Residencia:
                                                            </td>
                                                            <td class="style1">
                                                                <dx:ASPxTextBox ID="txtDireccionResidencia" ClientInstanceName="txtDireccionResidencia" runat="server" Width="250px" MaxLength="200" TabIndex="7">
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Dirección requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                                <dx:ASPxCheckBox ID="cbModificarDireccion" ClientInstanceName="cbModificarDireccion" runat="server" Checked="false" Text="Permitir cambio de direccion" ClientVisible="false">
                                                                    <ClientSideEvents CheckedChanged="function(s, e) {  
                                                                            checkedGestion()
                                                                        }" />
                                                                </dx:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="field">Celular:
                                                            </td>
                                                            <td>

                                                                <dx:ASPxTextBox ID="txtCelular" runat="server" Width="250px" MaxLength="10" TabIndex="7" ClientInstanceName="txtCelular"
                                                                    onkeypress="return solonumeros(event)">
                                                                    <ValidationSettings>
                                                                        <RegularExpression ValidationExpression="^(3)([0-9]{9})$" ErrorText="El celular debe comenzar con 3 y tener 10 digitos" />

                                                                        <RequiredField IsRequired="True" ErrorText="Infomacion Requerida"></RequiredField>
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents GotFocus="Seleccionar" LostFocus="function (s, e){
                                                                                                if(parseInt(txtCelular.GetValue()) == '0' ){
                                                                                                    alert('Por favor digite un numero de celular valido');
                                                                                                    txtCelular.SetText('');
                                                                                                }
                                                                                            }" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="El celular debe comenzar con 3 y tener 10 digitos" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>

                                                            </td>

                                                            <td class="field">Oficina de radicación:
                                                            </td>
                                                            <td>

                                                                <dx:ASPxTextBox ID="txtOficinaCliente" runat="server" Width="250px" MaxLength="6" TabIndex="16">
                                                                    <ClientSideEvents GotFocus="Seleccionar" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>

                                                                <div>
                                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Digite parte del nombre de la oficina de radicación."
                                                                        CssClass="comentario" Width="270px" Font-Size="XX-Small" Font-Bold="False" Font-Italic="True"
                                                                        Font-Names="Arial" Font-Overline="False" Font-Strikeout="False">
                                                                    </dx:ASPxLabel>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="field">Email:
                                                            </td>
                                                            <td class="style1">
                                                                <dx:ASPxTextBox ID="txtEmail" runat="server" Width="250px" MaxLength="50" TabIndex="9">
                                                                    <ClientSideEvents GotFocus="Seleccionar" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RegularExpression ErrorText="Formato no válido" ValidationExpression="\S+@\S+\.\S+" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td class="field">Ingresos Aproximados:
                                                            </td>
                                                            <td class="style1">
                                                                <dx:ASPxTextBox ID="txtIngresos" runat="server" Width="250px" MaxLength="10" TabIndex="14"
                                                                    onkeypress="return solonumeros(event)">
                                                                    <ClientSideEvents GotFocus="Seleccionar" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="field">Estado de Ánimo:
                                                            </td>
                                                            <td>

                                                                <dx:ASPxComboBox ID="cbEstadoId" runat="server" ValueType="System.Int32" ClientInstanceName="cbEstadoId" Width="250px" TabIndex="17">
                                                                    <Columns>
                                                                        <dx:ListBoxColumn FieldName="nombre" Width="300px" Caption="Nombre" />
                                                                    </Columns>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="El estado de Ánimo es Requerido" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                            <td class="field">Actividad Laboral:
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="cbActividadLaboral" runat="server" ValueType="System.Int32" ClientInstanceName="cbActividadLaboral" Width="250px" TabIndex="15">
                                                                    <Columns>
                                                                        <dx:ListBoxColumn FieldName="ALCnombre" Width="300px" Caption="Actividad" />
                                                                    </Columns>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Actividad laboral requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                                <%--  <dx:ASPxTextBox ID="txtActividadLaboral" runat="server" Width="250px" ClientInstanceName="txtActividadLaboral">
                                                                        </dx:ASPxTextBox>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="field">Resultado Proceso:
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="cmbResultadoProceso" runat="server" ValueType="System.String" AutoPostBack="False"
                                                                    ClientInstanceName="cmbResultadoProceso" TabIndex="12" Width="250px" ValueField="IdResultado">
                                                                    <ClientSideEvents TextChanged="function (s, e){
                                                                                var seleccion = cmbResultadoProceso.GetValue();
                                                                                loadingPanel.Show();
                                                                                cpGeneral.PerformCallback('Causal' + ':' + seleccion);
                                                                            }" />

                                                                    <Columns>
                                                                        <dx:ListBoxColumn FieldName="IdResultado" Width="300px" Caption="IdResultadoProceso" Visible="false" />
                                                                        <dx:ListBoxColumn FieldName="Descripcion" Width="300px" Caption="Descripción" />
                                                                    </Columns>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                            <td class="field">Causal:
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="cmbCausal" runat="server" ValueType="System.String" AutoPostBack="False"
                                                                    ClientInstanceName="cmbCausal" TabIndex="13" Width="250px" ValueField="IdTipoVenta">
                                                                    <ClientSideEvents SelectedIndexChanged="function (s, e){
                                                                                    var seleccion = cmbCausal.GetValue();
                                                                                    loadingPanel.Show();
                                                                                    cpGeneral.PerformCallback('RequiereProducto' + ':' + seleccion);
                                                                                }" />
                                                                    <Columns>
                                                                        <dx:ListBoxColumn FieldName="IdTipoVenta" Width="300px" Caption="IdTipoVenta" Visible="false" />
                                                                        <dx:ListBoxColumn FieldName="TipoVenta" Width="300px" Caption="Descripción" />
                                                                    </Columns>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="field">Observaciones:
                                                            </td>
                                                            <td colspan="2">
                                                                <dx:ASPxMemo ID="txtObservacionOperadorCall" runat="server" Height="50px" Width="250px"
                                                                    TabIndex="13">
                                                                    <%--<ValidationSettings Display="Dynamic" SetFocusOnError="true"
                                                                                ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" RequiredField-IsRequired="true">
                                                                                <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$"
                                                                                    ErrorText="El texto digitado contiene caracteres no permitidos" />
                                                                                <RegularExpression ErrorText="El texto digitado contiene caracteres no permitidos"
                                                                                    ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\&#161;\?\&#191;\b\s&#225;&#233;&#237;&#243;&#250;&#193;&#201;&#205;&#211;&#218;&#241;&#209;&#241;&#209;\-\#\[\]\(\)\/\\]+\s*$"></RegularExpression>
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="True"></RequiredField>
                                                                            </ValidationSettings>--%>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents GotFocus="Seleccionar" />
                                                                </dx:ASPxMemo>
                                                            </td>
                                                            <td align="left">
                                                                <dx:ASPxButton ID="btnAgregar" runat="server" Cursor="pointer" ClientVisible="false" Text="Agregar Producto" AutoPostBack="false">
                                                                    <ClientSideEvents Click="function (s, e){
                                                                                if(txtNumIdentificacion.GetValue()== null ){
                                                                                    alert('Por favor digite el número de cedula antes de asignar los servicios');
                                                                                } else {
                                                                                    if(cmbCampania.GetValue()== null ){
                                                                                        alert('Por favor seleccione una campaña antes de asignar los servicios');
                                                                                    } else {
                                                                                        AbrirModalProductos();
                                                                                    }
                                                                                }
                                                                            }" />
                                                                    <Image Url="../img/DxAdd32.png"></Image>
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="text-align: center" width="100%">
                                                        <tr>

                                                            <td align="right">

                                                                <dx:ASPxButton ID="btnRegistra" runat="server" Cursor="pointer" ClientVisible="true" Text="Continuar" AutoPostBack="false">
                                                                    <ClientSideEvents Click="function (s, e){
                                                                                if(ASPxClientEdit.ValidateGroup('registroVenta')){
                                                                                     loadingPanel.Show();
                                                                                        cpGeneral.PerformCallback('Registrar');
                                                                                }
                                                                            }" />
                                                                    <Image Url="../img/DxConfirm16.png"></Image>
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td align="left">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxButton ID="btnCancelar" runat="server" Cursor="pointer" ClientVisible="true" Text="Cancelar" AutoPostBack="false">
                                                                                <ClientSideEvents Click="function (s, e){                                                                                                                                                                                       
                                                                                                LimpiaFormulario();
                                                                                             }" />
                                                                                <Image Url="../img/DxCancel32.png"></Image>
                                                                            </dx:ASPxButton>
                                                                            <%--<dx:ASPxButton ID="btnPrueba" runat="server" OnClick="btnPrueba_Click" Text="Prueba"></dx:ASPxButton>--%>
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>

                                                            </td>
                                                            <td align="left">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxButton ID="btnCancelarPopUp" runat="server" Cursor="pointer" ClientVisible="false" Text="Salir" AutoPostBack="false">
                                                                                <ClientSideEvents Click="function (s, e){                                                                                                                                                                                       
                                                                                                
                                                                                                ClosePopUp();
                                                                                             }" />
                                                                                <Image Url="../img/DxCancel32.png"></Image>
                                                                            </dx:ASPxButton>
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>

                                                            </td>

                                                        </tr>
                                                    </table>

                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </div>
                                </td>
                                <td style="vertical-align: top">
                                    <div style="float: left; margin-top: 5px; width: 30%; margin-left: 10px">
                                        <dx:ASPxGridView ID="gvServiciosAux" runat="server" ClientInstanceName="gvServiciosAux" AutoGenerateColumns="false"
                                            KeyFieldName="idRegistro" Width="100%">
                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="idProducto" Caption="id Producto" Visible="false" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="1">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="tipoServicio" Caption="Tipo Servicio" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="2">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="producto" Caption="Producto" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="3">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="valorPrimaServicio" Caption="Cupo" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                    VisibleIndex="5">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                            <SettingsPager PageSize="10">
                                                <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                            </SettingsPager>
                                            <SettingsText Title="Servicios Agregados" EmptyDataRow="No se encontraron servicios agregados"
                                                CommandEdit="Editar"></SettingsText>
                                        </dx:ASPxGridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxRoundPanel ID="RpFechaAgendamiento" runat="server" ShowHeader="false" ClientVisible="false">
                                        <PanelCollection>
                                            <dx:PanelContent>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div id="divFechaAgendamiento" style="float: left; margin-top: 5px; width: 30%; margin-left: 10px">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td class="field">Fecha de Agendamiento:
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxDateEdit runat="server" ID="calFechaInicio" AutoPostBack="false" EnableMultiSelect="false"
                                                                                ShowClearButton="false" ShowWeekNumbers="false" ShowTodayButton="false">
                                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="true"
                                                                                    ErrorDisplayMode="ImageWithText" ErrorFrameStyle-Font-Size="Medium" ErrorTextPosition="Top" ValidationGroup="registroVenta">
                                                                                    <ErrorFrameStyle Font-Size="Medium"></ErrorFrameStyle>
                                                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                                </ValidationSettings>
                                                                                <ClientSideEvents DateChanged="function (s, e){
                                                                                            EjecutarCallbackGeneral(s, e, 'ObtenerCapacidadFechaAgenda');
                                                                                           }" />
                                                                            </dx:ASPxDateEdit>

                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <dx:ASPxGridView ID="gvJornadas" runat="server" ClientInstanceName="gvJornadas" AutoGenerateColumns="false"
                                                                                            KeyFieldName="idGestion" Width="90%">
                                                                                            <Columns>
                                                                                                <dx:GridViewDataTextColumn FieldName="Horario" Caption="Horario" ShowInCustomizationForm="True"
                                                                                                    HeaderStyle-HorizontalAlign="Center" VisibleIndex="0" Width="10%">
                                                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn FieldName="Cupo" Caption="Cupos" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                                                    VisibleIndex="1" Width="15%">
                                                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                                </dx:GridViewDataTextColumn>
                                                                                            </Columns>
                                                                                        </dx:ASPxGridView>

                                                                                        <dx:ASPxLabel runat="server" ID="lblCapacidadEntrega" ClientInstanceName="lblCapacidadEntrega"></dx:ASPxLabel>
                                                                                        <br />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="field">Jornada:
                                                                                    </td>
                                                                                    <td>
                                                                                        <dx:ASPxComboBox ID="cbJornada" runat="server" ClientInstanceName="cbJornada"
                                                                                            ValueType="System.Int32" Width="150px">
                                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true"
                                                                                                ErrorDisplayMode="ImageWithText" ErrorFrameStyle-Font-Size="Medium" ErrorTextPosition="Top" ValidationGroup="registroVenta">
                                                                                                <ErrorFrameStyle Font-Size="Medium"></ErrorFrameStyle>
                                                                                            </ValidationSettings>

                                                                                        </dx:ASPxComboBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <div style="float: left; margin-top: 20px; width: 100%">
                                        <dx:ASPxGridView ID="gvDetalle" runat="server" ClientInstanceName="gvDetalle" AutoGenerateColumns="false"
                                            KeyFieldName="idGestion" Width="90%">
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
                                            <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                            <SettingsPager PageSize="5">
                                                <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                            </SettingsPager>
                                            <SettingsText Title="Gestiones" EmptyDataRow="No se encontraron gestiones realizadas"
                                                CommandEdit="Editar"></SettingsText>
                                        </dx:ASPxGridView>
                                    </div>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <div style="float: left; margin-top: 20px; width: 100%">
                                        <dx:ASPxRoundPanel ID="rpInfoDevolucionCallCenter" Visible="false" runat="server" HeaderText="Info Devolucione Call Center" TabIndex="20"
                                            Width="90%">
                                            <PanelCollection>
                                                <dx:PanelContent>
                                                    <dx:ASPxGridView ID="gvInfoDevolucionCallCenter" runat="server" ClientInstanceName="gvInfoDevolucionCallCenter" AutoGenerateColumns="false"
                                                        KeyFieldName="idDevolucionCallCenter" Width="90%">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="idGestionVenta" Caption="Gestion Venta" ShowInCustomizationForm="True" Visible="false" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="0" Width="10%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="fechaRegistro" Caption="Fecha Creacion" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="1" Width="15%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="usuarioRegistra" Caption="Registrada por" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="3" Width="20%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="observacion" Caption="Observacion" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                VisibleIndex="4" Width="17%">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="center"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                                                        <SettingsPager PageSize="5">
                                                            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                                        </SettingsPager>
                                                        <SettingsText Title="Devoluciones Call Center" EmptyDataRow="No se encontraron devoluciones call center realizadas"
                                                            CommandEdit="Editar"></SettingsText>
                                                    </dx:ASPxGridView>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </div>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <msgp:MensajePopUp ID="mensajero" runat="server" />
                        <dx:ASPxPopupControl ID="pcServicio" runat="server" ClientInstanceName="dialogoServicio"
                            HeaderText="Agregar Servicio" AllowDragging="true" Width="400px" Height="300px"
                            Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                            ScrollBars="Auto" CloseAction="CloseButton">
                            <ClientSideEvents EndCallback="function(s,e){ 
                        if (s.cpResultado !=0){
                            ActualizarEncabezado(s,e);
                                } 
                        }"
                                CloseUp="function (s,e){
                            EjecutarCallbackGeneral(s, e, 'Mostrar');
                        }"></ClientSideEvents>
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="pccServicio" runat="server">
                                    <table>
                                        <tr>
                                            <td class="field">Tipo Servicio:
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbTipoServicio" runat="server" ClientInstanceName="cmbTipoServicio" ValueType="System.Int32"
                                                    Width="200px">
                                                    <ClientSideEvents SelectedIndexChanged="function (s, e){
                                                var seleccion = cmbTipoServicio.GetValue();
                                                dialogoServicio.PerformCallback('Productos:' + seleccion)
                                             }" />
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="productoExterno" Width="300px" Caption="Descripción" />
                                                    </Columns>
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroServicio">
                                                        <RequiredField ErrorText="Información requerida" IsRequired="true" />
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
                                                    <ClientSideEvents GotFocus="Seleccionar" />
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


                                                <%--<dx:ASPxComboBox ID="cmbPrima" runat="server" ClientInstanceName="cmbPrima" ValueType="System.Int32"
                                                Width="200px">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="ValorPrimaServicio" Width="300px" Caption="Descripción" />
                                                </Columns>
                                            </dx:ASPxComboBox>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxImage ID="imgAgregar" runat="server" ImageUrl="../img/DxSuperMarket.png" ToolTip="Agregar Producto"
                                                    Cursor="pointer">
                                                    <ClientSideEvents Click="function (s, e){
                                                if(ASPxClientEdit.ValidateGroup('registroServicio')){
                                                    var valor = txtNumIdentificacion.GetValue();
                                                    var campania = cmbCampania.GetValue();
                                                    var cupo = parseInt(txtCupo.GetValue());
                                                    RegistrarTransitorio(valor, campania, cupo);
                                                }
                                            }" />
                                                </dx:ASPxImage>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <dx:ASPxRoundPanel ID="rpServicios" runat="server" HeaderText="Servicios Agregados" Width="100%">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <dx:ASPxGridView ID="gvServicios" runat="server" ClientInstanceName="gvServicios" AutoGenerateColumns="false"
                                                                KeyFieldName="idRegistroOriginal" Width="100%">
                                                                <Columns>
                                                                    <dx:GridViewDataColumn Caption="Opciones" VisibleIndex="0" Width="100px">
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxHyperLink runat="server" ID="lnkEliminar" ImageUrl="../img/DXEraser16.png"
                                                                                Cursor="pointer" ToolTip="Ver Detalle" OnInit="Link_Init">
                                                                                <ClientSideEvents Click="function(s, e) { Eliminar(this, {0}); }" />
                                                                            </dx:ASPxHyperLink>
                                                                        </DataItemTemplate>
                                                                    </dx:GridViewDataColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="idRegistro" Caption="Id." ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                        VisibleIndex="1" Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="tipoServicio" Caption="Tipo Servicio" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                        VisibleIndex="2">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="producto" Caption="Producto" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                        VisibleIndex="3">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="observacion" Caption="Observación" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                        VisibleIndex="4">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="valorPrimaServicio" Caption="Cupo" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                        VisibleIndex="5">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <Settings ShowFooter="True" ShowHeaderFilterButton="true" />
                                                                <SettingsPager PageSize="10">
                                                                    <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                                                </SettingsPager>
                                                            </dx:ASPxGridView>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>
                        <dx:ASPxPopupControl ID="pcVer" runat="server" ClientInstanceName="dialogoVer" HeaderText="Información"
                            AllowDragging="true" Width="410px" Height="260px" Modal="true" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
            <dx:ASPxCallbackPanel ID="cbCallBackId" runat="server" Width="200px">
                <ClientSideEvents EndCallback="function(s,e){ActualizarEncabezado(s,e);}" />
            </dx:ASPxCallbackPanel>
            <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
                Modal="True">
            </dx:ASPxLoadingPanel>
        </div>
    </form>
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="scripts/GestionVentaYServicio.js" type="text/javascript"></script>
</body>
</html>
