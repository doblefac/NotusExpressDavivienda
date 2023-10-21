<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EncabezadoPagina.ascx.vb"
    Inherits="NotusExpress.EncabezadoPagina" %>
<%--<script src="../Scripts/jquery-1.12.4.js"></script>--%>
<link href="../Estilos/StyleAlert.css" rel="stylesheet" />
<script src="../Scripts/ScriptAlert.js"></script>
<div>
    <table width="90%">
        <tr>
            <td>
                <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="Segoe UI"
                    Font-Size="14pt" Visible="false"></asp:Label>
                <asp:Literal ID="ltDivision" runat="server" Text="<hr/>" Visible="false"></asp:Literal>
                <div style="float: left; width: 15%; vertical-align: middle">
                    <asp:Panel ID="pnlRegresar" runat="server" Visible="False" Style="margin-top: 15px; margin-bottom: 15px">
                        <asp:HyperLink ID="hlRegresar" runat="server" CssClass=".link">
                            <asp:Image ID="imgRegresar" runat="server" ImageUrl="~/img/arrow_back.png" />&nbsp;Regresar
                        </asp:HyperLink>
                    </asp:Panel>
                </div>
                <div id="divMensajes" style="float: right; text-align: center; width: 80%; vertical-align: middle; margin-top: 10px; margin-bottom: 10px">
                    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false"></asp:Label>
                    <asp:Label ID="lblWarning" runat="server" CssClass="warning" Visible="false"></asp:Label>
                    <asp:Label ID="lblSuccess" runat="server" CssClass="ok" Visible="false"></asp:Label>
                </div>


            </td>
        </tr>

    </table>
</div>
