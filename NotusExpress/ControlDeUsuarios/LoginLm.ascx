<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LoginLm.ascx.vb" Inherits="NotusExpress.LoginLm" %>
<div>
    <table border="0" style="margin-left: auto; margin-right: auto; width: 600px;">
        <tr style="height: 30px">
            <td align="center">
                <asp:Label ID="lblCambioConfirmacion" runat="server" CssClass="error" ForeColor ="Green"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <dx:ASPxRoundPanel ID="rpPrincipal" runat="server" BackColor="#E28181" ShowHeader="false" View="GroupBox">
                    <PanelCollection>
                        <dx:PanelContent BackColor ="#F1CFCF">
                            <asp:Login ID="lgAutenticador" runat="server" BackColor="#F1CFCF" BorderPadding="4" DisplayRememberMe="False" FailureText="El usuario y contraseña que se encuentre ingresando no corresponde, por favor intentar nuevamente" 
                                Font-Names="Verdana" Font-Size="0.9em" ForeColor="#666666" Height="136px" LoginButtonText="Iniciar Sesión" 
                                PasswordLabelText="Contraseña:" RememberMeText="Recordárme la próxima vez." TitleText="Inicio de Sesión" 
                                UserNameLabelText="Usuario:" Width="308px">
                                <TextBoxStyle Font-Size="0.8em" />
                                <LoginButtonStyle BackColor="#7C1010" BorderColor="Maroon" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" ForeColor="White" />
                                <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                <TitleTextStyle BackColor="#7C1010" Font-Bold="True" Font-Names="Verdana" Font-Size="Small" ForeColor="White" />
                            </asp:Login>
                            <asp:LinkButton runat ="server" ID="lbRecuperarContrasena" Text="Recuperar Contraseña"></asp:LinkButton>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr style="height: 30px">
            <td  align="center">
                 <dx:ASPxPopupControl ID="PopCambioDeContrasenaIngresoPrimeraVez" runat="server" CloseAction="CloseButton" HeaderText = "Modificación de Clave por ingreso primera vez"
            Modal ="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowOnPageLoad ="false"  Height="136px" Width ="500px">
            <ContentCollection>
                <dx:PopupControlContentControl>
                <dx:ASPxRoundPanel ID="RpCambioDeContrasenaIngresoPrimeraVez" runat="server" BackColor="#E28181" ShowHeader="false" View="GroupBox">
                    <PanelCollection>
                        <dx:PanelContent>
                            
                                <asp:ChangePassword ID="CambioContrasena" runat="server"  BackColor="#F1CFCF" OnChangingPassword="OnChangingPassword"
                                 EnableTheming="False" EnableViewState="False" ChangePasswordFailureText="Password incorrect or New Password invalid. New Password length minimum: {11}. Non-alphanumeric characters required: {1}.">
                                    <CancelButtonStyle  BackColor="#7C1010"  BorderColor="Maroon" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" ForeColor="White" />
                                    <ChangePasswordButtonStyle BackColor="#7C1010"  BorderColor="Maroon" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" ForeColor="White"  />
                                    <TextBoxStyle Font-Size = "0.8em" /> 
                                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                    <TitleTextStyle BackColor="#7C1010" Font-Bold="True" Font-Names="Verdana" Font-Size="Small" ForeColor="White" />
       
                                </asp:ChangePassword>
                                    <br />                           
                                        <asp:Label ID="lblMessage" runat="server" />
                            </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

            </td>
        </tr>
           <tr style="height: 30px">
               <td  align="center">
                   <dx:ASPxPopupControl ID="PopRecuperarContrasena" runat="server" CloseAction="CloseButton" HeaderText = "Recuperación De Contraseña"
            Modal ="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowOnPageLoad ="false">
            <ContentCollection>
                <dx:PopupControlContentControl>
                <dx:ASPxRoundPanel ID="RpRecuperarContrasena" runat="server" BackColor="#E28181" ShowHeader="false" View="GroupBox">
                    <PanelCollection>
                        <dx:PanelContent>
                            <asp:PasswordRecovery ID="recuperarContrasena"  OnVerifyingUser ="recuperarContrasena_VerifyingUser" QuestionLabelText ="Por favor ingrese su identificación para recuperar la contraseña" 
                                UserNameLabelText="Identificación" MembershipProvider ="textboxInformacion"  runat="server" BackColor="#F1CFCF" BorderPadding="4" Font-Names="Verdana" 
                                Font-Size="0.9em" ForeColor="#666666"  Height="136px" Width ="500px">
                                <TextBoxStyle Font-Size="0.8em" />
                                <SubmitButtonStyle BackColor="#7C1010"  BorderColor="Maroon" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Font-Names="Verdana" Font-Size="X-Small" ForeColor="White" />
                                <TitleTextStyle  BackColor="#7C1010" Font-Bold="True" Font-Names="Verdana" Font-Size="Small" ForeColor="White" />
                            </asp:PasswordRecovery>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
               </td>
           </tr> 
        <tr>
            <td>
                <dx:ASPxPopupControl ID="pcCaptcha" runat="server" CloseAction="None" HeaderText="Verificación: escriba el código que aparece a continuación"
            Modal="True" PopupHorizontalAlign="WindowCenter" 
            PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
            <ContentCollection>
                <dx:PopupControlContentControl>
                    <dx:ASPxCaptcha ID="captcha" runat="server" ClientInstanceName="captcha">
                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                            <RequiredField IsRequired="True" ErrorText="Dato requerido" />
                        </ValidationSettings>
                        <RefreshButton Text="Mostrar otro código">
                        </RefreshButton>
                        <TextBox LabelText="Escriba el código mostrado:" />
                    </dx:ASPxCaptcha>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <dx:ASPxButton ID="btnConfirmar" runat="server" Text="Continuar"></dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
            </td>
        </tr>

    </table>
    
</div>

