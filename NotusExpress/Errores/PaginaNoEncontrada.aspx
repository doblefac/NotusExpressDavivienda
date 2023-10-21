<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PaginaNoEncontrada.aspx.vb"
    Inherits="NotusExpress.PaginaNoEncontrada" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        <div class="title">
            ALGO NO ESTÁ DEL TODO BIEN
        </div>
        <br />
        <br />
        <div class="error_title">
            Error 404: Página No Encontrada
        </div>
        <div class="content">
            Lamentablemente no se ha podido encontrar la página que está buscando.
            Muchas cosas pudieron haber salido mal para traerlo hasta éste punto,
            por eso le ofrecemos regresar a la página anterior a través de el siguiente enlace:&nbsp;<a href="javascript: void();" onclick="javascript: history.back();">Regresar</a>
            <br />
            <br />
            Si Ud. tiene una pregunta o comentario, por favor no dude en enviarnos un email:&nbsp;<a href="mailto:itdevelopment@logytechmobile.com">itdevelopment@logytechmobile.com</a>.
        </div>
    </div>
    </form>
</body>
</html>
