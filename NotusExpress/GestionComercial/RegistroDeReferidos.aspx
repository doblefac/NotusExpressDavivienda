<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegistroDeReferidos.aspx.vb"
    Inherits="NotusExpress.RegistroDeReferidos"%>
   

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Notus Express - Registro de Referidos::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
   <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        String.prototype.trim = function() { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }
        function ValidarDatosMinimos(source, args) {
            try {
                var nombre = document.getElementById("txtNombreApellido").value.trim();
                var telefonoResidencia = document.getElementById("txtTelefonoResidencia").value.trim();
                var celular = document.getElementById("txtCelular").value.trim();
                var telefonoOficina = document.getElementById("txtTelefonoOficina").value.trim();
                if (nombre.length > 0 && (telefonoResidencia.length > 0 || celular.length > 0 || telefonoOficina.length > 0)) {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            } catch (e) {
                args.IsValid = false;
                alert("Imposible evaluar si se han proporcionado los datos mínimos para realizar el registro.\n" + e.description);
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <eo:CallbackPanel ID="cpValidacionYNotificacion" runat="server" Width="100%" UpdateMode="Always">
        <uc1:ValidacionURL ID="vuControlSesion" runat="server" />
        <uc2:EncabezadoPagina ID="epNotificador" runat="server" />
    </eo:CallbackPanel>
    <eo:CallbackPanel ID="cpGeneral" runat="server" Width="100%" ChildrenAsTriggers="true"
        LoadingDialogID="ldrWait_dlgWait">
        <asp:Panel ID="pnlInformacionPersonal" runat="server">
            <table class="tabla" width="50%">
                <tr>
                    <th colspan="4">
                        Datos Personales del Cliente
                    </th>
                </tr>
                <tr>
                    <td class="field">
                        Tipo de Identificación
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoIdentificacion" runat="server">
                            <asp:ListItem Text="Seleccione un Tipo" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Cédula de Ciudadanía" Value="1"></asp:ListItem>
                            <asp:ListItem Text="NUIP" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Cédula de Extranjería" Value="3"></asp:ListItem>
                            <asp:ListItem Text="NIT" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="field" style="width: 150px">
                        No. de Identificación
                    </td>
                    <td>
                        <asp:TextBox ID="txtIdentificacion" runat="server" MaxLength="15"></asp:TextBox>&nbsp;<asp:LinkButton ID="lbConsultar" runat="server" CssClass="search" ValidationGroup="consultaIdentificacion">&nbsp;<img src="../img/find.gif" alt="" />&nbsp;</asp:LinkButton>
                        <div style="display: block">
                            <asp:RegularExpressionValidator ID="revIdentificacion" runat="server" ErrorMessage="El dato proporionado tiene formato no v&aacute;lido"
                                ControlToValidate="txtIdentificacion" Display="Dynamic" ValidationGroup="registro"
                                ValidationExpression="^[\s]{0,}[a-zA-Z0-9\-\.]+[\s]{0,}$"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Nombres y Apellidos
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreApellido" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revNombreApellido" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtNombreApellido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                    <td class="field">
                        Ciudad de Residencia
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCiudadResidencia" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Direcci&oacute;n de Residencia
                    </td>
                    <td>
                        <asp:TextBox ID="txtDireccionResidencia" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revDireccionResidencia" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtdireccionResidencia" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                    <td class="field">
                        Barrio Residencia
                    </td>
                    <td>
                        <asp:TextBox ID="txtBarrioResidencia" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revBarrioResidencia" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtBarrioResidencia" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Tel&eacute;fono de Residencia
                    </td>
                    <td>
                        <asp:TextBox ID="txtTelefonoResidencia" runat="server" MaxLength="20"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revtTelefonoResidencia" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtTelefonoResidencia" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                    <td class="field">
                        Celular
                    </td>
                    <td>
                        <asp:TextBox ID="txtCelular" runat="server" MaxLength="20"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revCelular" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtCelular" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Direccion Oficina
                    </td>
                    <td>
                        <asp:TextBox ID="txtDireccionOficina" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revDireccionOficina" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtDireccionOficina" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                    <td class="field">
                        Tel&eacute;fono Oficina
                    </td>
                    <td>
                        <asp:TextBox ID="txtTelefonoOficina" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revTelefonoOficina" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtTelefonoOficina" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Ingreso Aproximado
                    </td>
                    <td>
                        <asp:TextBox ID="txtIngreso" runat="server" MaxLength="15"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revIngreso" runat="server" ErrorMessage="El dato proporcionado tiene formato no válido. Se espera un varlo numerico entero"
                            Display="Dynamic" ControlToValidate="txtIngreso" ValidationExpression="^[0-9]+$"
                            ValidationGroup="registro"></asp:RegularExpressionValidator>
                    </td>
                    <td class="field">
                        E-Mail
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtEmail" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\@\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Referido Por
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtReferidoPor" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revReferidoPor" runat="server" ErrorMessage="El texto proporcionado contiene caracteres no v&aacute;lidos. Por favor verifique"
                                Display="Dynamic" ControlToValidate="txtReferidoPor" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚ\-\#]+\s*$"
                                ValidationGroup="registro"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:CustomValidator ID="cusDatosMinimos" runat="server" ErrorMessage="No se han porporcionado los datos mínimos para realizar el registro. Nombre + Tel&eacute;fono de contacto."
                            ValidationGroup="registro" Display="Dynamic" ClientValidationFunction="ValidarDatosMinimos"></asp:CustomValidator>
                        <br />
                        <p>
                            <asp:LinkButton ID="lbRegistrar" runat="server" CssClass="search" ValidationGroup="registro"
                                OnClientClick="return ValidarDatosYMostrarConfirmacion('registro', '¿Realmente desea continuar con el registro de datos?');"><img src="../img/save_all.png" alt="" />&nbsp;Registrar Datos</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lbNuevo" runat="server" CssClass="search"><img src="../img/new.png" alt="" />&nbsp;Nuevo Referido</asp:LinkButton>
                        </p>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </eo:CallbackPanel>
    <uc3:Loader ID="ldrWait" runat="server" />
    </form>
</body>
</html>
