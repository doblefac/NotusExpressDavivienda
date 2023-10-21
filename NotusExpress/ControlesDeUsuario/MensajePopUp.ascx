<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MensajePopUp.ascx.vb"
    Inherits="NotusExpress.MensajePopUp" %>
<script type="text/javascript">
    function OnInitPopUpMensaje(s, e) {
        ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
            if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
                s.Hide();
        });
    }

</script>
<style type="text/css">
    .mensajeError ul
    {
        list-style-image: url('../img/error.png');
        list-style-type: square;
        font-weight: bold;
        margin-bottom: 2px;
        margin-top: 2px;
    }
    
    .mnsajeError
    {
        color: #FF0000;
        background-color: #FFFFCC;
        font-size: small;
        font-weight: bold;
    }
    
    .mensajeWarning ul
    {
        list-style-image: url('../img/exclamation.png');
        list-style-type: square;
        font-weight: bold;
        color: #FF9224;
        margin-bottom: 2px;
        margin-top: 2px;
    }
    
    .mensajeWarning
    {
        background-color: #F9F9F9;
        border-bottom: 1px solid #EAEAEA;
        border-top: 1px solid #EAEAEA;
    }
    
    .mensajeOk
    {
        color: #0033CC;
        background-color: #F9F9F9 !important;
        border-bottom: 1px solid #EAEAEA !important;
        border-top: 1px solid #EAEAEA !important;
        font-weight: bold !important;
    }
    
    .mensajeOk ul
    {
        list-style-image: url('../img/ok.png');
        list-style-type: square;
        font-weight: bold;
        margin-bottom: 2px;
        margin-top: 2px;
    }
    .auto-style1 {
        width: 469px;
    }
</style>
<dx:ASPxPopupControl ID="pucMensaje" runat="server" PopupHorizontalAlign="WindowCenter"
    PopupVerticalAlign="WindowCenter" Modal="true" HeaderText="Mensaje" ClientInstanceName="mensajePopUp"
    Width="488px" Height="238px" MinHeight="120px" MaxWidth="500px" MaxHeight="400px"
    ScrollBars="Auto" ShowMaximizeButton="True" ShowPageScrollbarWhenModal="True"
    CloseAction="CloseButton">
    <ModalBackgroundStyle CssClass="modalBackground" />
    <ContentStyle>
        <Paddings Padding="0px" />
    </ContentStyle>
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" >
            <dx:ASPxPanel ID="pnlMensaje" runat="server" DefaultButton="btnAceptar" TabIndex="0" Height="191px" Width="480px">
                <Paddings Padding="0px" />
                <PanelCollection>
                    <dx:PanelContent>
                        <table style="width: 475px; height: 178px">
                            <tr>
                                <td align="center" valign="middle" class="auto-style1">
                                    <div style="overflow: auto; vertical-align: middle;">
                                        <dx:ASPxLabel ID="lblMensaje" runat="server" Text="" ClientInstanceName="textoMensajePopUp"
                                            EnableViewState="false">
                                        </dx:ASPxLabel>
                                    </div>
                                </td>
                            </tr>
                            <tr style="height: 30px">
                                <td align="center" style="padding-top: 4px; padding-bottom: 4px;" class="auto-style1">
                                    <dx:ASPxButton ID="btnAceptar" runat="server" ClientInstanceName="btnAceptar" Text="Aceptar"
                                        AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e){mensajePopUp.Hide();}" />
                                        <Image Url="~/img/ok.png">
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
