<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="NotusExpress.Login" %>

<%@ Register assembly="DevExpress.Web.v18.1, Version=18.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register Src="~/ControlDeUsuarios/LoginLm.ascx" TagPrefix="epg" TagName="LoginLm" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>:: NotusExpress :: Inicio de Sesión ::</title>
    <style type="text/css">
        .error ul
        {
            list-style-image: url('../img/error.png');
            list-style-type: square;
            font-family: Arial;
            font-size: small;
            font-weight: bold;
        }
        .error
        {
            color: #FF0000;
            background-color: #FFFFCC;
            font-family: Arial;
            font-size: small;
            font-weight: bold;
        }
        .warning ul
        {
            list-style-image: url('../img/exclamation.png');
            list-style-type: square;
            font-family: Arial;
            font-size: small;
            font-weight: bold;
            color: #FF9224;
        }
        .warning
        {
            background-color: #F9F9F9;
            border-bottom: 1px solid #EAEAEA;
            border-top: 1px solid #EAEAEA;
            font-family: Arial;
            font-size: small;
            font-weight: bold;
            color: #FF9224;
        }
        
        .loginDiv
        {
            position: relative;
            top: -15px;
            left: -10px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        if (window.frameElement) {
            window.parent.location = "Login.aspx";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <div class="loginDiv" style="padding-bottom: 100px!important" >
        <table style="margin-left: auto; margin-right: auto; width: 600px;" border="0" cellpadding="0"
            cellspacing="0">
            <tr>
                <td width="31" valign="bottom">
                    <img alt="" src="img/tbllogin_left.png" align="bottom" />
                </td>
                <td style="background-image: url('img/tbllogin_topborder.png'); background-repeat: repeat-x;">
                </td>
                <td width="31" valign="bottom">
                    <img alt="" src="img/tbllogin_right.png" align="bottom" />
                </td>
            </tr>
            <tr>
                <td rowspan="4" style="background-image: url('img/tbllogin_leftborder.png'); background-repeat: repeat-y;">
                </td>
                <td align="center">
                    <img alt="" src="img/logoLM_Small.jpg" />
                </td>
                <td rowspan="4" style="background-image: url('img/tbllogin_rightborder.png'); background-repeat: repeat-y;"
                    align="right">
                </td>
            </tr>
            <tr style="height: 30px">
                <td align="center">
                    <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
                </td>
            </tr>
            <tr style="height: 120px">
                <td align="center" valign="top">
                    <epg:LoginLm runat="server" id="LoginLm" />
                </td>
            </tr>
            <tr style="height: 45px">
                <td align="center" style="font-family: Verdana; font-size: x-small; font-style: italic;
                    color: Gray; vertical-align: top;">
                    Recuerde que la contraseña es sensible
                    <br />
                    a mayúsculas y minúsculas
                </td>
            </tr>
            <tr>
                <td>
                    <img alt="" src="img/tbllogin_botomleft.png" />
                </td>
                <td style="background-image: url('img/tbllogin_botomborder.png'); background-repeat: repeat-x;"
                    valign="bottom">
                </td>
                <td>
                    <img alt="" src="img/tbllogin_botomright.png" />
                </td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>