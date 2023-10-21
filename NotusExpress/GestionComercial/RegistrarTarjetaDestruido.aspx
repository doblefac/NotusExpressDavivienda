<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegistrarTarjetaDestruido.aspx.vb" Inherits="NotusExpress.RegistrarTarjetaDestruido" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/ValidacionURL.ascx" TagName="ValidacionURL"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Notus Express - Gestión de Ventas::</title>
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return true;

            return false;
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

        function url() {

            hidden = open('ActadeDestruccion.aspx', 'NewWindow', 'top=0,left=0,status=yes,resizable=yes,scrollbars=yes');

        }

        function VerPicking(key) {
            TamanioVentana();
            dialogoVerPicking.SetContentUrl("Reportes/VisorXtraReport.aspx?id=" + key)
            dialogoVerPicking.SetSize(myWidth * 0.6, myHeight * 0.9);
            dialogoVerPicking.ShowWindow();
        }

        var myWidth, myHeight;

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
    </script>
    <style type="text/css">
        .style1
        {
            width: 1010px;
        }
        .style2
        {
            font-weight: bold;
            background: #A32035;
            color: White;
            width: 1010px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfControlFiltroCiudad" runat="server" />
    <eo:CallbackPanel ID="cpValidacionYNotificacion" runat="server" Width="100%" UpdateMode="Always">
        <uc1:ValidacionURL ID="vuControlSesion" runat="server" />
        <uc2:EncabezadoPagina ID="epNotificador" runat="server" />
    </eo:CallbackPanel>
    <eo:CallbackPanel ID="cpGeneral" runat="server" Width="100%" UpdateMode="Group" GroupName="General"
        LoadingDialogID="ldrWait_dlgWait" Triggers="{ControlID:lbAgregar;Parameter:}">
        <asp:Panel ID="pnlConsulta" runat="server">
            <table class="tabla" style="width: 70%">
                <tr>
                    <th colspan="2">
                        Consulta de Seriales
                    </th>
                </tr>
                <tr>
                    <td class="field" style="width: 160px">
                        No. de Serial
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumeroSerial" runat="server" MaxLength="16" onkeypress="return solonumeros(event);"></asp:TextBox>
                        <div style="display: block">
                            <asp:RequiredFieldValidator ID="rfvNumeroSerial" runat="server" ErrorMessage="No. de Serial requerido"
                                ControlToValidate="txtNumeroSerial" Display="Dynamic" 
                                ValidationGroup="consultaSerial" Enabled="False"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revNumeroSerial" runat="server" ErrorMessage="El dato proporionado tiene formato no v&aacute;lido"
                                ControlToValidate="txtNumeroSerial" Display="Dynamic" ValidationGroup="consultaSerial"
                                ValidationExpression="^[\s]{0,}[a-zA-Z0-9\-\.]+[\s]{0,}$"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                        <p>
                            <asp:LinkButton ID="lbAgregar" runat="server" CssClass="search" 
                                ValidationGroup="consultaSerial"><img src="../img/add.png" alt="" />&nbsp;Agregar</asp:LinkButton></p>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div id="divFloater" style="display: none; position: static; height: 35px; width: 200px;">
            <table align="center">
                <tr>
                    <td align="center" valign="middle" style="height: 100%">
                        <asp:Image ID="imgLoading" runat="server" ImageUrl="~/img/loader_dots.gif" ImageAlign="Middle" />
                        <b>Procesando...</b>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Panel ID="pnlGestion" runat="server">
            <eo:TabStrip runat="server" ID="tsInfoGestion" ControlSkinID="None" MultiPageID="mpInfoGestion">
                <TopGroup>
                    <Items>
                        <eo:TabItem Text-Html="Seriales a Destruir ">
                        </eo:TabItem>
                    </Items>
                </TopGroup>
                <LookItems>
                    <eo:TabItem ItemID="_Default" RightIcon-Url="00010223" RightIcon-SelectedUrl="00010226"
                        NormalStyle-CssText="color: #606060" SelectedStyle-CssText="color: #2f4761; font-weight: bold;"
                        LeftIcon-Url="00010221" LeftIcon-SelectedUrl="00010224" Image-Url="00010222"
                        Image-SelectedUrl="00010225" Image-Mode="TextBackground" Image-BackgroundRepeat="RepeatX">
                        <SubGroup Style-CssText="font-family: tahoma; font-size: 8pt; background-image: url(00010220); background-repeat: repeat-x; cursor: hand;"
                            OverlapDepth="8">
                        </SubGroup>
                    </eo:TabItem>
                </LookItems>
            </eo:TabStrip>
            <eo:MultiPage runat="server" ID="mpInfoGestion">
                <eo:PageView ID="pvDatosCliente" runat="server">
                    
                    <%--Nuevo panel de historico de ventas--%>
                    <asp:Panel ID="pnlVentasDestruir" runat="server" width="100%">
                        <eo:CallbackPanel ID="cpVentasDestruir" runat="server" LoadingDialogID="ldrWait_dlgWait"
                            UpdateMode="Self" 
                            
                            Triggers="{ControlID:lbCancelar;Parameter:},{ControlID:lbContinuar;Parameter:}" 
                            Width="100%">
                            <table class="tabla">
                                <tr>
                                    <th colspan="6" class="style1">
                                        Seriales a destruír
                                    </th>
                                </tr>
                                <tr>
                                    <td class="style2" colspan="6">
                                        

                                        <asp:GridView ID="gvVentasDestruir" runat="server" EnableModelValidation="True" 
                                            ForeColor="Black" Width="100%">
                                        </asp:GridView>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="style1">
                                        <br />
                                        <p>
                                            <asp:LinkButton ID="lbCancelar" runat="server" CssClass="search" 
                                                
                                                OnClientClick="return confirm('¿Realmente desea cancelar el registro?\nSe perderá toda la información proporcionada');"><img src="../img/cancelar.png" alt="" />&nbsp;Cancelar</asp:LinkButton>
                                                <asp:LinkButton ID="lbContinuar" runat="server" CssClass="search"><img src="../img/new.png" alt="" />&nbsp;Continuar</asp:LinkButton>
                                        </p>
                                    </td>
                                </tr>
                                
                            </table>
                            <table id="tblMotivoDestruccion" runat="server">
                            <tr id="trObservacionDestruccion" runat="server">
                                <td class="field">
                                Motivo Destrucción
                                </td>
                                <td colspan="4">
                                <asp:TextBox ID="txtObservacionDestruccion" Text="" runat="server" 
                                        TextMode="MultiLine" Width="800" Height="50" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                            <td></td>
                                <td >
                                <asp:LinkButton ID="lbDestruir" runat="server" CssClass="search"><img src="../img/new.png" alt="" />&nbsp;Destruír</asp:LinkButton>
                                </td>
                            </tr>
                            </table>
                        </eo:CallbackPanel>
                    </asp:Panel>
                </eo:PageView>
            </eo:MultiPage>
        </asp:Panel>
    </eo:CallbackPanel>
    <eo:CallbackPanel ID="cpMensajeConfirmacion" runat="server" Width="100%" UpdateMode="Group"
        GroupName="General" LoadingDialogID="ldrWait_dlgWait" ChildrenAsTriggers="true">
        <eo:Dialog runat="server" ID="dlgMensajeReferido" ControlSkinID="None" Height="150px"
            Width="500px" HeaderHtml="Solicitud de Referido" BackColor="White" CancelButton="btnNo"
            BackShadeColor="Gray" BackShadeOpacity="50">
            <ContentTemplate>
                <div style="text-align: center; padding: 5px; width: 100%;">
                    <asp:Label ID="lblRegistroOk" CssClass="ok" runat="server"></asp:Label><br />
                    <asp:Label ID="lblSolicitudReferido" ForeColor="Gray" Font-Bold="true" runat="server"
                        Text="¿El Cliente desea proporcionar información de Referido?"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btnSi" runat="server" Text="Si" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnNo" runat="server" Text="No" />
                </div>
            </ContentTemplate>
            <FooterStyleActive CssText="padding-right: 4px; padding-left: 4px; font-size: 8pt; padding-bottom: 4px; padding-top: 4px; font-family: tahoma">
            </FooterStyleActive>
            <HeaderStyleActive CssText="background-image:url('00020311');color:black;font-family:'trebuchet ms';font-size:10pt;font-weight:bold;padding-bottom:5px;padding-left:8px;padding-right:3px;padding-top:0px;">
            </HeaderStyleActive>
            <ContentStyleActive CssText="padding-right: 4px; padding-left: 4px; font-size: 8pt; padding-bottom: 4px; padding-top: 4px; font-family: tahoma">
            </ContentStyleActive>
        </eo:Dialog>
    </eo:CallbackPanel>
    <uc3:Loader ID="ldrWait" runat="server" />
    <asp:Label ID="lblTipoConsulta" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lblExisteTransaccion" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lblTransaccionExistente" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lblEsNueva" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lblIdentificacionAnterior" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lblTieneVentas" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lblIdClienteOriginal" runat="server" Text="0" Visible="false"></asp:Label>
    </form>
</body>
</html>
