<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ErrorGeneral.aspx.vb"
    Inherits="NotusExpress.ErrorGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <link href="../Estilos/Errores.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="top">
        <div id="encabezado" style="width: 95%">
            <img alt="Logytech" src="../img/LogoHome.png" style="height: 78px; width: 175px" />
        </div>
        <br />
        <br />
        <a href="javascript: void();" onclick="javascript: history.back();">Regresar</a><br />
        <br />
        <div class="error_title">
            Ha Ocurrido un error inesperado
        </div>
        <div class="content">
            <asp:Label ID="lblAux" runat="server" Text="Descripción:" CssClass="title"></asp:Label><br />
            <asp:Label ID="lblDescripcion" runat="server" Text=""></asp:Label>
            <br />
            <br />
            Si el problema persiste, por favor contacte a IT Development: <a href="mailto:itedevelopment@logytechmobile.com">
                itdevelopment@logytechmobile.com</a>.
        </div>
    </div>
    </form>
</body>
</html>
