<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectorDireccion.ascx.vb" Inherits="NotusExpress.SelectorDireccion" %>

<style type="text/css">
    .auto-style1 {
        width: 50px;
    }
</style>

<script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script language="javascript" type="text/jscript">
    function ObtenerDireccion(memo) {
        try {
            if (mmDireccionTmp.GetValue() != null) {
                memo.SetText(mmDireccionTmp.GetValue());
            }
        } catch (e) { alert("No se logro obtener la dirección: " + e.Message); }
    }

    function DireccionTemporal(memo) {
        try {

            if (txtAdicional.GetValue() == null || txtAdicional.GetValue() == '') { var textoAdicional = '' } else { var textoAdicional = txtAdicional.GetValue() }
            if (cmbNombreVia.GetValue() == null) { var NombreVia = '' } else { var NombreVia = cmbNombreVia.GetValue() + " " }
            if (spNumeroVia.GetValue() == null || spNumeroVia.GetValue() == '0') { var NumeroVia = '' } else { var NumeroVia = spNumeroVia.GetValue() + " " }
            if (cmbLetraVia.GetValue() == null) { var LetraVia = '' } else { var LetraVia = cmbLetraVia.GetValue() + " " }
            if (cmbBisVia.GetValue() == null) { var BisVia = '' } else { var BisVia = cmbBisVia.GetValue() + " " }
            if (cmbOrientacionVia.GetValue() == null) { var OrientacionVia = '' } else { var OrientacionVia = cmbOrientacionVia.GetValue() + " " }
            if (spNumeroViaSec.GetValue() == null || spNumeroViaSec.GetValue() == '0') { var NumeroViaSec = '' } else { var NumeroViaSec = spNumeroViaSec.GetValue() + " " }
            if (cmbLetraViaSec.GetValue() == null) { var LetraViaSec = '' } else { var LetraViaSec = cmbLetraViaSec.GetValue() + " " }
            if (cmbBisViaSec.GetValue() == null) { var BisViaSec = '' } else { var BisViaSec = cmbBisViaSec.GetValue() + " " }
            if (spNumeroNomenclatura.GetValue() == null || spNumeroNomenclatura.GetValue() == '0') { var NumeroNomenclatura = '' } else { var NumeroNomenclatura = spNumeroNomenclatura.GetValue() + " " }
            if (cmbOrientacionViaSec.GetValue() == null) { var OrientacionViaSec = '' } else { var OrientacionViaSec = cmbOrientacionViaSec.GetValue() + " " }
            memo.SetText(NombreVia + NumeroVia + LetraVia + BisVia + OrientacionVia + NumeroViaSec + LetraViaSec + BisViaSec + NumeroNomenclatura + OrientacionViaSec + textoAdicional);

        } catch (e) { alert("No se logro obtener la dirección: " + e.Message); }
    }

</script>

<div style="float: left; margin-right: 0px;">
    <dx:ASPxMemo ID="memoDireccion" runat="server" Border-BorderStyle="Solid" ClientInstanceName="memoDireccion"
        Style="display: inline-block;" Enabled="False" Rows="2" Columns="40" ForeColor="Black" Text="">
        <ValidationSettings CausesValidation="True" ValidationGroup="vgRegistrar" ErrorText="Prueba 1">
            <RequiredField IsRequired="True" />
        </ValidationSettings>
        <Border BorderStyle="Solid" />
    </dx:ASPxMemo>
    <dx:ASPxHyperLink ID="hlHome" runat="server" Text="" Style="display: inline;"
        ImageUrl="~/img/home.png"
        ToolTip="Seleccione para editar la dirección." Cursor="pointer">
    </dx:ASPxHyperLink>
    <asp:HiddenField ID="hdDireccionEdicion" runat="server" Value="" />
    <div style="display: inline">
        <dx:ASPxPopupControl ID="pcDireccion" runat="server" LoadContentViaCallback="OnFirstShow"
            PopupElementID="hlHome" PopupVerticalAlign="Below"
            PopupHorizontalAlign="WindowCenter" AllowDragging="True"
            ShowFooter="True" Width="250px" Height="222px"
            HeaderText="Establecer dirección" ClientInstanceName="popupControl"
            Modal="True">
            <ContentCollection>
                <dx:PopupControlContentControl ID="pcContentContenido" runat="server">
                    <div style="vertical-align: middle">
                        <dx:ASPxLabel ID="lblDireccionOriginal" runat="server" Text="" Width="270px" Font-Size="Small" Font-Bold="False" Font-Italic="True"
                            Font-Names="Arial" Font-Overline="False" Font-Strikeout="False" ForeColor="#333333">
                        </dx:ASPxLabel>
                        <br />
                        <dx:ASPxRoundPanel ID="rpDireccion" runat="server" Height="30%" Width="95%" HeaderText="Datos Dirección" ShowHeader="true"
                            BackColor="White" HorizontalAlign="left">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                    <table>
                                        <tr>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbNombreVia" runat="server" IncrementalFilteringMode="Contains" ClientInstanceName="cmbNombreVia"
                                                    AutoResizeWithContainer="True" Width="175px">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                        DireccionTemporal(mmDireccionTmp);
                                                     }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td class="auto-style1">
                                                <dx:ASPxSpinEdit ID="spNumeroVia" runat="server" Height="21px" Number="0" ClientInstanceName="spNumeroVia" DecimalPlaces="2"
                                                    MaxValue="500" MinValue="1" Width="50px" NumberType="Integer">
                                                    <ClientSideEvents NumberChanged="function(s,e){
                                                        DireccionTemporal(mmDireccionTmp);
                                                     }" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbLetraVia" runat="server" IncrementalFilteringMode="Contains" ClientInstanceName="cmbLetraVia"
                                                    AutoResizeWithContainer="True" Width="40px">
                                                    <Items>
                                                        <dx:ListEditItem Value=" " Text=" " />
                                                        <dx:ListEditItem Value="A" Text="A" />
                                                        <dx:ListEditItem Value="B" Text="B" />
                                                        <dx:ListEditItem Value="C" Text="C" />
                                                        <dx:ListEditItem Value="D" Text="D" />
                                                        <dx:ListEditItem Value="E" Text="E" />
                                                        <dx:ListEditItem Value="F" Text="F" />
                                                        <dx:ListEditItem Value="G" Text="G" />
                                                        <dx:ListEditItem Value="H" Text="H" />
                                                        <dx:ListEditItem Value="I" Text="I" />
                                                        <dx:ListEditItem Value="J" Text="J" />
                                                        <dx:ListEditItem Value="K" Text="K" />
                                                        <dx:ListEditItem Value="L" Text="L" />
                                                        <dx:ListEditItem Value="M" Text="M" />
                                                        <dx:ListEditItem Value="N" Text="N" />
                                                        <dx:ListEditItem Value="O" Text="O" />
                                                        <dx:ListEditItem Value="P" Text="P" />
                                                        <dx:ListEditItem Value="Q" Text="Q" />
                                                        <dx:ListEditItem Value="R" Text="R" />
                                                        <dx:ListEditItem Value="S" Text="S" />
                                                        <dx:ListEditItem Value="T" Text="T" />
                                                        <dx:ListEditItem Value="U" Text="U" />
                                                        <dx:ListEditItem Value="V" Text="V" />
                                                        <dx:ListEditItem Value="W" Text="W" />
                                                        <dx:ListEditItem Value="X" Text="X" />
                                                        <dx:ListEditItem Value="Y" Text="Y" />
                                                        <dx:ListEditItem Value="Z" Text="Z" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                            DireccionTemporal(mmDireccionTmp);
                                        }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbBisVia" runat="server" IncrementalFilteringMode="Contains" ClientInstanceName="cmbBisVia"
                                                    AutoResizeWithContainer="True" Width="50px">
                                                    <Items>
                                                        <dx:ListEditItem Value=" " Text=" " />
                                                        <dx:ListEditItem Value="BIS" Text="BIS" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                        DireccionTemporal(mmDireccionTmp);
                                                     }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbOrientacionVia" runat="server" IncrementalFilteringMode="Contains" ClientInstanceName="cmbOrientacionVia"
                                                    AutoResizeWithContainer="True" Width="100px">
                                                    <Items>
                                                        <dx:ListEditItem Value=" " Text=" " />
                                                        <dx:ListEditItem Value="N" Text="Norte" />
                                                        <dx:ListEditItem Value="S" Text="Sur" />
                                                        <dx:ListEditItem Value="E" Text="Este" />
                                                        <dx:ListEditItem Value="O" Text="Oeste" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                        DireccionTemporal(mmDireccionTmp);
                                                     }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">#</td>
                                            <td class="auto-style1">
                                                <dx:ASPxSpinEdit ID="spNumeroViaSec" runat="server" Height="21px" Number="0" ClientInstanceName="spNumeroViaSec"
                                                    MaxValue="500" MinValue="1" Width="50px" NumberType="Integer">
                                                    <ClientSideEvents NumberChanged="function(s,e){
                                                        DireccionTemporal(mmDireccionTmp);
                                                     }" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbLetraViaSec" runat="server" IncrementalFilteringMode="Contains" ClientInstanceName="cmbLetraViaSec"
                                                    AutoResizeWithContainer="True" Width="40px">
                                                    <Items>
                                                        <dx:ListEditItem Value=" " Text=" " />
                                                        <dx:ListEditItem Value="A" Text="A" />
                                                        <dx:ListEditItem Value="B" Text="B" />
                                                        <dx:ListEditItem Value="C" Text="C" />
                                                        <dx:ListEditItem Value="D" Text="D" />
                                                        <dx:ListEditItem Value="E" Text="E" />
                                                        <dx:ListEditItem Value="F" Text="F" />
                                                        <dx:ListEditItem Value="G" Text="G" />
                                                        <dx:ListEditItem Value="H" Text="H" />
                                                        <dx:ListEditItem Value="I" Text="I" />
                                                        <dx:ListEditItem Value="J" Text="J" />
                                                        <dx:ListEditItem Value="K" Text="K" />
                                                        <dx:ListEditItem Value="L" Text="L" />
                                                        <dx:ListEditItem Value="M" Text="M" />
                                                        <dx:ListEditItem Value="N" Text="N" />
                                                        <dx:ListEditItem Value="O" Text="O" />
                                                        <dx:ListEditItem Value="P" Text="P" />
                                                        <dx:ListEditItem Value="Q" Text="Q" />
                                                        <dx:ListEditItem Value="R" Text="R" />
                                                        <dx:ListEditItem Value="S" Text="S" />
                                                        <dx:ListEditItem Value="T" Text="T" />
                                                        <dx:ListEditItem Value="U" Text="U" />
                                                        <dx:ListEditItem Value="V" Text="V" />
                                                        <dx:ListEditItem Value="W" Text="W" />
                                                        <dx:ListEditItem Value="X" Text="X" />
                                                        <dx:ListEditItem Value="Y" Text="Y" />
                                                        <dx:ListEditItem Value="Z" Text="Z" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                            DireccionTemporal(mmDireccionTmp);
                                         }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="cmbBisViaSec" runat="server" IncrementalFilteringMode="Contains" ClientInstanceName="cmbBisViaSec"
                                                    AutoResizeWithContainer="True" Width="50px">
                                                    <Items>
                                                        <dx:ListEditItem Value=" " Text=" " />
                                                        <dx:ListEditItem Value="BIS" Text="BIS" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                        DireccionTemporal(mmDireccionTmp);
                                                     }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">-</td>
                                            <td class="auto-style1">
                                                <dx:ASPxSpinEdit ID="spNumeroNomenclatura" runat="server" Height="21px" Number="00" ClientInstanceName="spNumeroNomenclatura"
                                                    MaxValue="500" MinValue="1" Width="50px" NumberType="Integer">
                                                    <ClientSideEvents NumberChanged="function(s,e){
                                                        DireccionTemporal(mmDireccionTmp);
                                                     }" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td colspan="2">
                                                <dx:ASPxComboBox ID="cmbOrientacionViaSec" runat="server" IncrementalFilteringMode="Contains" ClientInstanceName="cmbOrientacionViaSec"
                                                    AutoResizeWithContainer="True" Width="100px">
                                                    <Items>
                                                        <dx:ListEditItem Value=" " Text=" " />
                                                        <dx:ListEditItem Value="N" Text="Norte" />
                                                        <dx:ListEditItem Value="S" Text="Sur" />
                                                        <dx:ListEditItem Value="E" Text="Este" />
                                                        <dx:ListEditItem Value="O" Text="Oeste" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                        DireccionTemporal(mmDireccionTmp);
                                                     }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <div id="divAdicional" runat="server">
                            <dx:ASPxCallbackPanel ID="cpnl" runat="server" ClientInstanceName="cpnl" OnCallback="cpnl_Callback">
                                <ClientSideEvents EndCallback="function(s,e){
                                    if (s.cpResultado != '') {
                                        alert(s.cpResultado);
                                    }
                                    mmDireccionTmp.SetText(s.cpDireccion);
                                }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                        <dx:ASPxRoundPanel ID="rpAdicional" runat="server" Height="30%" Width="95%" HeaderText="Datos adicionales" ShowHeader="true"
                                            BackColor="White" HorizontalAlign="left">
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 30%">Datos a Adicionar
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="cmbDatosAdicional" runat="server" ClientInstanceName="cmbDatosAdicional"
                                                                    IncrementalFilteringMode="Contains" Width="60%" AutoResizeWithContainer="true">
                                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                                        cpnl.PerformCallback(mmDireccionTmp.GetValue() + ':validar');
                                                                    }" />
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 30%">Texto adicional:
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="txtTextoAdicional" runat="server" ClientInstanceName="txtTextoAdicional" Width="100%">
                                                                    <ClientSideEvents LostFocus="function(s,e){
                                                                        cpnl.PerformCallback(mmDireccionTmp.GetValue() + ':validar');
                                                                    }" />
                                                                </dx:ASPxTextBox>
                                                                <div>
                                                                    <dx:ASPxLabel ID="lblComentario" runat="server" Text="Tab para validar y agregar la información a la dirección." Width="270px" Font-Size="XX-Small" Font-Bold="False" Font-Italic="True"
                                                                        Font-Names="Arial" Font-Overline="False" Font-Strikeout="False" ForeColor="#999999">
                                                                    </dx:ASPxLabel>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                            <Border BorderColor="#8B8B8B" BorderStyle="Solid" BorderWidth="1px" />
                                        </dx:ASPxRoundPanel>
                                        <dx:ASPxTextBox ID="txtAdicional" runat="server" ClientVisible="false" Text="" ClientInstanceName="txtAdicional"></dx:ASPxTextBox>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>
                        </div>
                        <br />
                        Vista Previa de la direccion:<br />
                        <table>
                            <tr>
                                <td style="align-items: center">
                                    <dx:ASPxMemo ID="mmDireccionTmp" runat="server" Border-BorderStyle="Solid" ClientInstanceName="mmDireccionTmp"
                                        Enabled="False" Rows="2" Columns="85" ForeColor="Black" Text="">
                                        <Border BorderStyle="Solid" />
                                    </dx:ASPxMemo>
                                </td>
                            </tr>
                        </table>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <FooterTemplate>
                <div style="float: right; margin: 3px;">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="btnReiniciar" runat="server" Text="Reiniciar direccion"
                                    AutoPostBack="False">
                                    <ClientSideEvents Click="function(s, e){
                                        if(confirm('Esta seguro de reiniciar los datos de la dirección?')) { 
                                            mmDireccionTmp.SetText('');
                                            cmbNombreVia.SetValue('');
                                            spNumeroVia.SetValue(0);
                                            cmbLetraVia.SetValue('');
                                            cmbBisVia.SetValue('');
                                            cmbOrientacionVia.SetValue('');
                                            spNumeroViaSec.SetValue(0);
                                            cmbLetraViaSec.SetValue('');
                                            cmbBisViaSec.SetValue('');
                                            spNumeroNomenclatura.SetValue(0);
                                            cmbOrientacionViaSec.SetValue('');
                                            txtAdicional.SetText('');
                                        }
                                    }" />
                                </dx:ASPxButton>
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <dx:ASPxButton ID="btnGuardar" runat="server" Text="Aceptar"
                                    AutoPostBack="False">
                                    <ClientSideEvents Click="function(s, e){
                                    ObtenerDireccion(memoDireccion);
                                    popupControl.Hide();
                            }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>


                </div>
            </FooterTemplate>
        </dx:ASPxPopupControl>
    </div>
</div>

