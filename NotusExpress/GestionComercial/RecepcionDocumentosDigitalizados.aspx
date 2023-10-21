<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RecepcionDocumentosDigitalizados.aspx.vb" Inherits="NotusExpress.RecepcionDocumentosDigitalizados" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Notus Express - Recepción Documentos Digitalizados::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        
        function EjecutarCallbackGeneral(s, e, parametro, valor) {
            if (ASPxClientEdit.AreEditorsValid()) {
                loadingPanel.Show();
                cpGeneral.PerformCallback(parametro + ',' + valor);
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return true;

            return false;
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




        String.prototype.trim = function () { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }
        function ValidarDatosMinimos(source, args) {
            try {
                var telefonoResidencia = document.getElementById("txtTelefonoResidencia").value.trim();
                var celular = document.getElementById("txtCelular").value.trim();
                var telefonoOficina = document.getElementById("txtTelefonoOficina").value.trim();
                if (telefonoResidencia.length > 0 || celular.length > 0 || telefonoOficina.length > 0) {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            } catch (e) {
                args.IsValid = false;
                alert("Imposible evaluar si se ha proporcionado un teléfono de contacto.\n" + e.description);
            }
        }

        function CallbackAfterUpdateHandler(callback, extraData) {
            try {
                MostrarOcultarDivFloater(false);
            } catch (e) {
                //alert("Error al tratar de evaluar respuesta del servidor.\n" + e.description);
            }

        }

        function MostrarOcultarDivFloater(mostrar) {
            var valorDisplay = mostrar ? "block" : "none";
            var elDiv = document.getElementById("divFloater");
            elDiv.style.display = valorDisplay;
        }

        function FiltrarDatos(source, callbackPanel, filtro, idControladorFiltro) {
            var controladorFiltro = document.getElementById(idControladorFiltro);
            try {
                if (filtro.length >= 4 || (filtro.length < 4 && controladorFiltro.value == "1")) {
                    MostrarOcultarDivFloater(true);
                    if (filtro.length < 4) { filtro = ""; }
                    eo_Callback(callbackPanel, filtro);
                    if (filtro.length >= 4) {
                        controladorFiltro.value = "1";
                    } else {
                        controladorFiltro.value = "0";
                    }
                }
                source.focus();
            } catch (e) {
                MostrarOcultarDivFloater(false);
                //alert("Error al tratar de filtrar Datos.\n" + e.description);
            }
        }

        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divEncabezado">
        <epg:EncabezadoPagina ID="epNotificador" runat="server" />
    </div>
    <div>
    <dx:ASPxCallbackPanel ID="cpGeneral" runat="server" >
        <ClientSideEvents EndCallback="function(s,e){ ActualizarEncabezado(s,e);}" />
        <PanelCollection>
            <dx:PanelContent>
            <table width="50%" class="tabla">
                                <tr>
                        <th colspan="2">
                            Información Digitalización
                        </th>
                    </tr>
                    <tr>
                        <td class="field">
                            No. de Identificaci&oacute;n:
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="txtNumIdentificacion" runat="server" Width="170px" MaxLength="15" onkeypress="return solonumeros(event);" ReadOnly="False">
                            </dx:ASPxTextBox>
                            <div style="display: block">  
                            <asp:RequiredFieldValidator ID="rfvIdentificacion" runat="server" ErrorMessage="Identificación Requerida" ControlToValidate="txtNumIdentificacion" 
                            ValidationGroup="consultarCliente" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div> 
                        </td>
                        </tr>
                        <tr>
                        <td class="field">
                            Documentos:
                        </td>
                            
                        <td>
                            <dx:ASPxComboBox ID="cbDocumentos" runat="server" ValueType="System.Int32">
                            </dx:ASPxComboBox>
                            <div style="display: block">  
                            <asp:RequiredFieldValidator ID="rfvDocumentos" runat="server" ErrorMessage="Documentos Requeridos" ControlToValidate="cbDocumentos" 
                            ValidationGroup="consultarCliente" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div> 
                        </td>
                        </tr>
                        </table>
                        
    <asp:Panel ID="Panel1" runat="server">
    <br />
        <asp:FileUpload ID="FileUpload1" runat="server" />

        <dx:ASPxButton ID="btnCargar" runat="server" Text="Cargar" ClientInstanceName="btnCargar" ValidationGroup="consultarCliente">
        <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'Cargar');}" />
        </dx:ASPxButton>
        <br />
        <dx:ASPxGridView ID="gvDocumentos" runat="server" AutoGenerateColumns="False" 
            KeyFieldName="IdDoc">
            <ClientSideEvents EndCallback="ActualizarEncabezado" />
            <Columns>
                <dx:GridViewDataTextColumn Caption="ID" FieldName="idDoc" Name="idDoc" 
                    ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0">
                    <PropertiesTextEdit MaxLength="50">
                    </PropertiesTextEdit>
                    <EditFormSettings VisibleIndex="0" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Documento" FieldName="documento" 
                    Name="documento" ShowInCustomizationForm="True" VisibleIndex="1">
                    <PropertiesTextEdit MaxLength="40">
                        <ValidationSettings CausesValidation="True" Display="Dynamic" 
                            EnableCustomValidation="True" ErrorText="Valor inválido">
                            <RegularExpression ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑñÑ\-\#\[\]\(\)\/\\]+\s*$" />
                            <RequiredField IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn Caption="Ruta" FieldName="ruta" Name="ruta" 
                    ShowInCustomizationForm="True" VisibleIndex="2">
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataTextColumn Caption="Opción" ShowInCustomizationForm="True" 
                    VisibleIndex="4">
                    <DataItemTemplate>
                        <asp:LinkButton ID="lbEliminar" runat="server" OnClick="ManejadorEliminar"><img src="../img/eliminar.gif" alt="Eliminar Documento"/>
                                </asp:LinkButton>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
            <Settings ShowHeaderFilterBlankItems="False" ShowTitlePanel="True" />
            <SettingsText CommandEdit="Editar" 
                EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda" 
                Title="Listado de Documentos Digitalizados" />
            <SettingsPopup>
                <EditForm HorizontalAlign="WindowCenter" Modal="True" 
                    VerticalAlign="WindowCenter" Width="40%" />
            </SettingsPopup>
            <StylesEditors>
                <ReadOnlyStyle BackColor="LightGray" ForeColor="Gray">
                </ReadOnlyStyle>
                <ReadOnly BackColor="LightGray" ForeColor="Gray">
                </ReadOnly>
            </StylesEditors>
        </dx:ASPxGridView>
        <br />
        <dx:ASPxButton ID="btnRegistrar" runat="server" 
            ClientInstanceName="btnRegistrar" Enabled="False" 
            Style="display: inline!important;" Text="Registrar" ValidationGroup="Registrar">
            <ClientSideEvents Click="function(s, e) { EjecutarCallbackGeneral(s,e,'Consultar');}" />
            <Image Url="~/img/continue.png">
            </Image>
        </dx:ASPxButton>
                
    </asp:Panel>                                                
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel> 
    
    </div>
    <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
