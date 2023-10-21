<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConsultaVentaPresencial.aspx.vb" Inherits="NotusExpress.ConsultaVentaPresencial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Consulta de Ventas Presenciales::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" style="overflow: auto" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        function IniciarToggle() {
            $("#cpGeneral_divDatos").slideToggle("slow");
        }

        function toggle(control) {
            $("#" + control).slideToggle("slow");
        }

        function validarTrValor() {
            if (cmbTipoServicio.GetValue() == 1) {
                cpGeneral_pcServicio_trValorPrima.style.display = 'none';
                cpGeneral_pcServicio_trValorPreaprobado.style.display = 'table-row';
            } else if (cmbTipoServicio.GetValue() == 2) {
                cpGeneral_pcServicio_trValorPrima.style.display = 'table-row';
                cpGeneral_pcServicio_trValorPreaprobado.style.display = 'none';
            }
        }

    </script>
</head>
<body>
    <form id="formPrincipal" runat="server">
        <div id="divEncabezado">
            <uc1:EncabezadoPagina ID="miEncabezado" runat="server" />
        </div>
        <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" ClientInstanceName="cpGeneral" ScrollBars="auto" Height="100%">
            <ClientSideEvents EndCallback="function(s, e) { 
                        if (s.cpRespuesta != 99) {
                            $('html, body').animate({scrollTop:0}, 1250);
                        }
                        if (s.cpRespuestaProducto == 1) {
                            validarTrValor();
                        }
                        loadingPanel.Hide(); 
                        $('#divEncabezado').html(s.cpMensaje);
                    }" />
            <PanelCollection>
                <dx:PanelContent>
                    <div id="divFiltro" runat="server">
                        <dx:ASPxRoundPanel ID="rpFiltros" runat="server" HeaderText="Filtrar por número de documento" ShowHeader="true"
                            Width="400px" Theme="default">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <table>
                                        <tr>
                                            <td>Número de documento:
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="txtNumeroDocumento" runat="server" ClientInstanceName="txtNumeroDocumento"
                                                    NullText="Digite número de documento" Width="250px" TabIndex="1" onkeypress="return SoloNumeros(event)">
                                                    <ClientSideEvents GotFocus="Seleccionar" />
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                        <RequiredField ErrorText="Número de documento es requerido" IsRequired="true" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <dx:ASPxImage ID="imgFiltrar" runat="server" ImageUrl="../img/DxFilter32.png"
                                                    ToolTip="Consultar información de la venta" Cursor="pointer" TabIndex="2">
                                                    <ClientSideEvents Click="function (s, e){
                                                            if(ASPxClientEdit.ValidateGroup('vgRegistro')){
                                                                loadingPanel.Show();
                                                                cpGeneral.PerformCallback('ConsultarCliente:0');
                                                                toggle('cpGeneral_divDatos');
                                                            }
                                                        }" />
                                                </dx:ASPxImage>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>
                    <br />
                    <div id="divDatos" runat="server" name="divDatos">
                        <dx:ASPxRoundPanel ID="rpDatos" runat="server" HeaderText="Resultado de la consulta" ShowHeader="true" Enabled="true"
                            Width="70%" Theme="default">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <dx:ASPxRoundPanel ID="rpDatosCliente" runat="server" HeaderText="Datos de la Venta" Width="96%">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="field" colspan="1">Tipo Identificación:
                                                                    </td>
                                                                    <td colspan="1" class="style1">
                                                                        <dx:ASPxComboBox ID="cbTipoId" runat="server" ValueType="System.Int32" ClientInstanceName="cbTipoId"
                                                                            Width="250px" TabIndex="1">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Tipo identificación requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td class="field">No. de Identificaci&oacute;n:</td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtNumIdentificacion" runat="server" Width="250px" MaxLength="15" ClientInstanceName="txtNumIdentificacion"
                                                                            ReadOnly="False" TabIndex="2" NullText="">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RegularExpression ErrorText="Los caracteres ingresados no son validos" ValidationExpression="^\s*[a-zA-Z_0-9]+\s*$" />
                                                                                <RequiredField ErrorText="Número identificación requerido" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                        <asp:HiddenField ID="hflidcliente" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="field">Nombres:</td>
                                                                    <td class="style1">
                                                                        <dx:ASPxTextBox ID="txtNombres" runat="server" Width="250px" MaxLength="50" TabIndex="3" NullText=""
                                                                            onkeypress="return SoloLetras(event)">
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td class="field">Primer Apellido:
                                                                    </td>
                                                                    <td class="style1">
                                                                        <dx:ASPxTextBox ID="txtPrimerApellido" runat="server" Width="250px" MaxLength="50" NullText=""
                                                                            TabIndex="4" onkeypress="return SoloLetras(event)">
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
                                                                        <dx:ASPxTextBox ID="txtSegundoApellido" runat="server" Width="250px" MaxLength="50" NullText=""
                                                                            TabIndex="5" onkeypress="return SoloLetras(event)">
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td class="field">Dirección Residencia:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtDireccion" runat="server" NullText="" Width="250px"></dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="field">Ciudad Residencia:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="cmbCiudad1" runat="server" ClientInstanceName="cmbCiudad1" Width="250"
                                                                            ValueType="System.Int32" TabIndex="6" IncrementalFilteringMode="Contains"
                                                                            CallbackPageSize="10" EnableCallbackMode="true">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Dato requerido" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td class="field">Teléfono Fijo:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtTelFijo" runat="server" Width="250px" MaxLength="10" TabIndex="7" NullText=""
                                                                            onkeypress="return SoloNumeros(event)">
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="field">Teléfono 2 (Opcional):
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtTelefono2" runat="server" Width="250px" MaxLength="10" TabIndex="9" NullText=""
                                                                            onkeypress="return SoloNumeros(event)">
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td class="field">Celular:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtCelular" runat="server" Width="250px" MaxLength="10" TabIndex="8" NullText=""
                                                                            onkeypress="return SoloNumeros(event)">
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="field">Email:
                                                                    </td>
                                                                    <td class="style1">
                                                                        <dx:ASPxTextBox ID="txtEmail" runat="server" Width="250px" MaxLength="50" TabIndex="9" NullText="">
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RegularExpression ErrorText="Formato no válido" ValidationExpression="\S+@\S+\.\S+" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td class="field">Sexo:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="cmbSexo" runat="server" ClientInstanceName="cmbSexo" Width="250px"
                                                                            ValueType="System.String" TabIndex="12" IncrementalFilteringMode="Contains"
                                                                            CallbackPageSize="10" EnableCallbackMode="true">
                                                                            <Items>
                                                                                <dx:ListEditItem Text="Femenino" Value="F" />
                                                                                <dx:ListEditItem Text="Masculino" Value="M" />
                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="RegistroCliente">
                                                                                <RequiredField ErrorText="Dato requerido" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="field">Ingresos Aproximados:
                                                                    </td>
                                                                    <td class="style1">
                                                                        <dx:ASPxTextBox ID="txtIngresos" runat="server" Width="250px" MaxLength="12" TabIndex="10"
                                                                            NullText="" onkeypress="return SoloNumeros(event)">
                                                                            <MaskSettings Mask="$<0..999999999g>"></MaskSettings>
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td class="field">Empresa:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="cmbEmpresa" runat="server" ClientInstanceName="cmbEmpresa" Width="250px"
                                                                            ValueType="System.Int32" TabIndex="11" IncrementalFilteringMode="Contains" CallbackPageSize="10" EnableCallbackMode="true">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="field">Resultado Ubica
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td style="width: 100%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 50%" align="left">
                                                                                                <dx:ASPxComboBox ID="cmbUbica" runat="server" ValueType="System.Int32" ClientInstanceName="cmbUbica"
                                                                                                    Width="110%" TabIndex="12" ClientEnabled="true">
                                                                                                    <Items>
                                                                                                        <dx:ListEditItem Text="Positivo" Value="1" />
                                                                                                        <dx:ListEditItem Text="Negativo" Value="0" />
                                                                                                    </Items>
                                                                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                                                                            if (cmbUbica.GetValue()==1){
                                                                                                                UbicaPositivo();
                                                                                                            } else {
                                                                                                                UbicaNegativo();
                                                                                                            }
                                                                                                        }" />
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                                        <RequiredField ErrorText="informacion requerida" IsRequired="true" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxComboBox>
                                                                                            </td>
                                                                                            <td style="width: 50%">
                                                                                                <dx:ASPxTextBox ID="txtUbica" runat="server" NullText="" Width="100%" ClientInstanceName="txtUbica">
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                                        <RequiredField ErrorText="informacion requerida" IsRequired="true" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td class="field">Resultado Evidente
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td style="width: 100%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 50%">
                                                                                                <dx:ASPxComboBox ID="cmbEvidente" runat="server" ValueType="System.Int32" ClientInstanceName="cmbEvidente"
                                                                                                    Width="110%" TabIndex="13" ClientEnabled="true">
                                                                                                    <Items>
                                                                                                        <dx:ListEditItem Text="Positivo" Value="1" />
                                                                                                        <dx:ListEditItem Text="Negativo" Value="0" />
                                                                                                    </Items>
                                                                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                                                                            if (cmbEvidente.GetValue()==1){
                                                                                                                EvidentePositivo();
                                                                                                                UbicaPositivo();
                                                                                                            } else {
                                                                                                                EvidenteNegativo();
                                                                                                            UbicaNegativo();
                                                                                                            }
                                                                                                        }" />
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                                        <RequiredField ErrorText="informacion requerida" IsRequired="true" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxComboBox>
                                                                                            </td>
                                                                                            <td style="width: 50%">
                                                                                                <dx:ASPxTextBox ID="txtEvidente" runat="server" NullText="" Width="100%" ClientInstanceName="txtEvidente">
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                                        <RequiredField ErrorText="informacion requerida" IsRequired="true" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="field">Resultado Datacredito
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td style="width: 100%">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 50%">
                                                                                                <dx:ASPxComboBox ID="cmbDataCredito" runat="server" ValueType="System.Int32" ClientInstanceName="cmbDataCredito"
                                                                                                    Width="110%" TabIndex="12" ClientEnabled="true">
                                                                                                    <Items>
                                                                                                        <dx:ListEditItem Text="Positivo" Value="1" />
                                                                                                        <dx:ListEditItem Text="Negativo" Value="0" />
                                                                                                    </Items>
                                                                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                                                                            if (cmbDataCredito.GetValue()==1){
                                                                                                                DataCreditoPositivo();
                                                                                                            UbicaPositivo();
                                                                                                            } else {
                                                                                                                DataCreditoNegativo();
                                                                                                            UbicaNegativo();
                                                                                                            }
                                                                                                        }" />
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                                        <RequiredField ErrorText="informacion requerida" IsRequired="true" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxComboBox>
                                                                                            </td>
                                                                                            <td style="width: 50%">
                                                                                                <dx:ASPxTextBox ID="txtDataCredito" runat="server" NullText="" Width="100%" ClientInstanceName="txtDataCredito">
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                                        <RequiredField ErrorText="informacion requerida" IsRequired="true" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td class="field">Resultado Proceso:
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="cmbResultadoProceso" runat="server" ValueType="System.Int32" ClientInstanceName="cmbResultadoProceso" ClientEnabled="false"
                                                                            Width="250px" TabIndex="13" AutoPostBack="false">
                                                                            <Items>
                                                                                <dx:ListEditItem Text="Aprobado" Value="1" />
                                                                                <dx:ListEditItem Text="Negada" Value="2" />
                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="informacion requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="field">Cupo Preaprobado
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txtCupo1" runat="server" NullText="" ClientInstanceName="txtCupo1"
                                                                            Width="250px" onkeypress="return SoloNumeros(event)" TabIndex="14">
                                                                            <MaskSettings Mask="$<0..999999999g>"></MaskSettings>
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td class="field">Observaciones:
                                                                    </td>
                                                                    <td colspan="1">
                                                                        <dx:ASPxMemo ID="txtObservacionOperadorCall" runat="server" Height="35px" Width="250px"
                                                                            TabIndex="15">
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ValidationGroup="registroVenta"
                                                                                ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                                                                <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$"
                                                                                    ErrorText="El texto digitado contiene caracteres no permitidos" />
                                                                                <RegularExpression ErrorText="El texto digitado contiene caracteres no permitidos"
                                                                                    ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\&#161;\?\&#191;\b\s&#225;&#233;&#237;&#243;&#250;&#193;&#201;&#205;&#211;&#218;&#241;&#209;&#241;&#209;\-\#\[\]\(\)\/\\]+\s*$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                                        </dx:ASPxMemo>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trConsecutivo" runat="server" style="display: none">
                                                                    <td colspan="4">
                                                                        <table style="width: 95%">
                                                                            <tr>
                                                                                <td class="field">Consecutivo Formulario DataCredito:
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="txtConsecutivo" runat="server" NullText="" Width="250px">
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroVenta">
                                                                                            <RequiredField ErrorText="informacion requerida" IsRequired="true" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxRoundPanel ID="rpServiciosAgregados" runat="server" HeaderText="Servicios Agregados" Width="96%">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <dx:ASPxGridView ID="gvDatosIniciales" runat="server" ClientInstanceName="gvDatosIniciales" AutoGenerateColumns="false"
                                                                KeyFieldName="idDetalle" Width="100%">
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn FieldName="idDetalle" Caption="Id." ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
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
                                                                    <dx:GridViewDataTextColumn FieldName="valorPrimaServicio" Caption="Prima/ValorPreaprobado" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                        VisibleIndex="4">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="observacion" Caption="Observación" ShowInCustomizationForm="True" HeaderStyle-HorizontalAlign="Center"
                                                                        VisibleIndex="5">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataColumn Caption="Opciones" VisibleIndex="6" Width="100px" CellStyle-HorizontalAlign="Center">
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxHyperLink runat="server" ID="lnkEliminar" ImageUrl="../img/DXEraser16.png"
                                                                                Cursor="pointer" ToolTip="Eliminar Producto" OnInit="Link_Init">
                                                                                <ClientSideEvents Click="function(s, e) { 
                                                                                        loadingPanel.Show();
                                                                                        cpGeneral.PerformCallback('Eliminar:' + {0});
                                                                                    }" />
                                                                            </dx:ASPxHyperLink>
                                                                        </DataItemTemplate>
                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                    </dx:GridViewDataColumn>
                                                                </Columns>
                                                                <Settings ShowFooter="True" ShowHeaderFilterButton="true" />
                                                                <SettingsPager PageSize="10">
                                                                    <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                                                </SettingsPager>
                                                                <SettingsDetail ShowDetailRow="True"></SettingsDetail>
                                                                <Templates>
                                                                    <DetailRow>
                                                                        <dx:ASPxGridView ID="gvDetalle" ClientInstanceName="gvDetalle" runat="server" AutoGenerateColumns="false"
                                                                            Width="100%" OnBeforePerformDataSelect="gvDetalle_DataSelect" KeyFieldName="IdDetalle">
                                                                            <Columns>
                                                                                <dx:GridViewDataTextColumn FieldName="documento" Caption="Documentos" ShowInCustomizationForm="True"
                                                                                    HeaderStyle-HorizontalAlign="Center" VisibleIndex="0" Width="10%">
                                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
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
                                                                    EmptyDataRow="No existen datos para mostrar" />
                                                                <SettingsBehavior EnableCustomizationWindow="False" AutoExpandAllGroups="False" />
                                                            </dx:ASPxGridView>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <dx:ASPxImage ID="imgAgrega" runat="server" ImageUrl="../img/DxAdd32.png" ToolTip="Agregar Servicio"
                                                    Cursor="pointer" TabIndex="16" ClientVisible="false" ClientInstanceName="imgAgrega">
                                                    <ClientSideEvents Click="function (s, e){if(txtNumIdentificacion.GetValue()== null || txtCupo1.GetValue()=='$0'){
                                                            alert('Por favor digite el número de cedula y/o cupo preaprobado antes de asignar los servicios');
                                                        } else {
                                                            //AbrirModalProductos();
                                                            var valor = txtNumIdentificacion.GetValue();
                                                            dialogoServicio.PerformCallback('Inicial:' + valor);
                                                            dialogoServicio.ShowWindow();
                                                        }}" />
                                                </dx:ASPxImage>
                                                &nbsp
                                                    <dx:ASPxImage ID="imgActualiza" runat="server" ImageUrl="../img/DxConfirm16.png" ToolTip="Registrar"
                                                        Cursor="pointer" TabIndex="17">
                                                        <ClientSideEvents Click="function (s, e){
                                                            if(ASPxClientEdit.ValidateGroup('registroVenta')){
                                                               loadingPanel.Show();
                                                               cpGeneral.PerformCallback('Actualizar:0');
                                                            }
                                                         }" />
                                                    </dx:ASPxImage>
                                                &nbsp
                                                    <dx:ASPxImage ID="imgCancela" runat="server" ImageUrl="../img/DxCancel32.png" ToolTip="Cancelar"
                                                        Cursor="pointer" TabIndex="18">
                                                        <ClientSideEvents Click="function (s, e){
                                                            toggle('cpGeneral_divDatos');
                                                            LimpiaFormulario();
                                                        }" />
                                                    </dx:ASPxImage>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>
                    <br />
                    <dx:ASPxPopupControl ID="pcServicio" runat="server" ClientInstanceName="dialogoServicio"
                        HeaderText="Agregar Servicio" AllowDragging="true" Width="800px" Height="300px"
                        Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                        ScrollBars="Auto" CloseAction="CloseButton" ShowPageScrollbarWhenModal="true">
                        <ClientSideEvents
                            EndCallback="function(s,e){
                                    loadingPanel.Hide();
                                 }"
                            CloseButtonClick="function(s,e){
                                    dialogoServicio.Hide();  
                                    loadingPanel.Show();
                                    cpGeneral.PerformCallback('CerrarProductos:' + '0');
                                }" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="pccServicio" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td class="field">Categoria:</td>
                                        <td colspan="3">
                                            <dx:ASPxComboBox ID="cmbCategoria" runat="server" ClientInstanceName="cmbCategoria" ValueType="System.Int32"
                                                Width="250px">
                                                <ClientSideEvents SelectedIndexChanged="function (s, e){
                                                        var seleccion = cmbCategoria.GetValue();
                                                        dialogoServicio.PerformCallback('Servicios:' + seleccion);
                                                    }" />
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroServicio">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Tipo Servicio:
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cmbTipoServicio" runat="server" ClientInstanceName="cmbTipoServicio" ValueType="System.Int32"
                                                Width="250px">
                                                <ClientSideEvents SelectedIndexChanged="function (s, e){
                                                        var seleccion = cmbTipoServicio.GetValue();
                                                        dialogoServicio.PerformCallback('Productos:' + seleccion);
                                                        gluDocumentos.GetGridView().UnselectAllRowsOnPage();
                                                    }" />
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroServicio">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td class="field" style="width: 82px">Productos:
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="cmbProductos" runat="server" ClientInstanceName="cmbProductos" ValueType="System.Int32"
                                                Width="250px">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="NombreProducto" Width="250px" Caption="Descripción" />
                                                </Columns>
                                                <ClientSideEvents SelectedIndexChanged="function (s, e){
                                                        dialogoServicio.PerformCallback('Primas:' + '0');
                                                        gluDocumentos.GetGridView().UnselectAllRowsOnPage();
                                                    }" />
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroServicio">
                                                    <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">Documentos:
                                        </td>
                                        <td>
                                            <dx:ASPxGridLookup GridViewProperties-Settings-VerticalScrollBarMode="Auto" ID="gluDocumentos" runat="server" KeyFieldName="IdDocumento" SelectionMode="Multiple"
                                                IncrementalFilteringMode="StartsWith" TextFormatString="{0}" Width="250px" ClientInstanceName="gluDocumentos"
                                                MultiTextSeparator=", " AllowUserInput="false">
                                                <ClientSideEvents ButtonClick="function(s,e) {gluDocumentos.GetGridView().UnselectAllRowsOnPage(); gluDocumentos.HideDropDown(); }" />
                                                <Buttons>
                                                    <dx:EditButton Text="X">
                                                    </dx:EditButton>
                                                </Buttons>
                                                <Columns>
                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="250px" />
                                                    <dx:GridViewDataTextColumn FieldName="Documento" Width="500px" />
                                                </Columns>
                                                <GridViewProperties>
                                                    <SettingsBehavior AllowDragDrop="False" EnableRowHotTrack="True" />
                                                    <SettingsPager NumericButtonCount="5" PageSize="10" />

<Settings VerticalScrollBarMode="Auto"></Settings>
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </td>
                                        <td colspan="2">
                                            <table style="vertical-align: top">
                                                <tr id="trValorPrima" runat="server" style="display: none">
                                                    <td class="field">Valor:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="cmbPrima" runat="server" ClientInstanceName="cmbPrima" ValueType="System.Int32"
                                                            Width="250px">
                                                            <Columns>
                                                                <dx:ListBoxColumn FieldName="ValorPrimaServicio" Width="300px" Caption="Descripción" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr id="trValorPreaprobado" runat="server" style="display: none">
                                                    <td class="field">Valor Preaprobado:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtCupo2" runat="server" NullText="Digite cupo preaprobado" ClientInstanceName="txtCupo2"
                                                            Width="250px" onkeypress="return SoloNumeros(event)" TabIndex="14">
                                                            <MaskSettings Mask="$<0..999999999g>"></MaskSettings>
                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="registroServicio">
                                                                <RequiredField ErrorText="Información requerida" IsRequired="true" />
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">Observación:
                                                    </td>
                                                    <td>
                                                        <dx:ASPxMemo ID="mmObservaciones" runat="server" Height="75px" Width="250px">
                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ValidationGroup="registroServicio"
                                                                ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Bottom">
                                                                <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$"
                                                                    ErrorText="El texto digitado contiene caracteres no permitidos" />
                                                                <RegularExpression ErrorText="El texto digitado contiene caracteres no permitidos"
                                                                    ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\&#161;\?\&#191;\b\s&#225;&#233;&#237;&#243;&#250;&#193;&#201;&#205;&#211;&#218;&#241;&#209;&#241;&#209;\-\#\[\]\(\)\/\\]+\s*$"></RegularExpression>
                                                                <RequiredField ErrorText="Información requerida" IsRequired="True"></RequiredField>
                                                            </ValidationSettings>
                                                            <ClientSideEvents GotFocus="Seleccionar" />
                                                        </dx:ASPxMemo>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <dx:ASPxImage ID="imgAgregarproducto" runat="server" ImageUrl="../img/DxSuperMarket.png" ToolTip="Agregar Producto"
                                                Cursor="pointer">
                                                <ClientSideEvents Click="function (s, e){
                                                        if(ASPxClientEdit.ValidateGroup('registroServicio')){
                                                            loadingPanel.Show();
                                                            dialogoServicio.PerformCallback('RegistroServicioTemporal:' + '0');
                                                        }
                                                    }" />
                                            </dx:ASPxImage>
                                        </td>
                                    </tr>
                                </table>
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
