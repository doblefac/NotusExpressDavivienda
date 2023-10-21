<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdministracionCampaniaFinanciero.aspx.vb" Inherits="NotusExpress.AdministracionCampaniaFinanciero" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc2" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>:: Administrador Campañas Financieras :: </title>
    <%--<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">--%>
    <link href="../Estilos/EstiloAutocomplete.css" rel="stylesheet" />
    
    <script src="../Scripts/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.12.1-ui.js" type="text/javascript"></script>
    
    <link href="../Estilos/estiloContenidos.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FuncionesJS.js" type="text/javascript"></script>

    <link href="../Estilos/StyleAlert.css" rel="stylesheet" />
    <script src="../Scripts/ScriptAlert.js"></script>    
    <%--<script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>    este jquery ya estaba pero se deja por pruebas ya que fue reemplazado por la versión 1.12--%>

    <style type="text/css">
        /*body {font-family: Arial, Helvetica, sans-serif;}*/

        #tblCodigosEstrategia {
            font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;
            font-size: 12px;
            width: 264px;
            border-collapse: collapse;
        }

            #tblCodigosEstrategia th {
                font-size: 13px;
                font-weight: normal;
                padding: 8px;
                background: #b9c9fe;
                border-top: 4px solid #aabcfe;
                border-bottom: 1px solid #fff;
                color: #039;
            }

            #tblCodigosEstrategia td {
                padding: 8px;
                background: #e8edff #57aad0;
                border-bottom: 1px solid #fff;
                color: #669;
                border-top: 1px solid transparent;
                text-align: center;
            }

            #tblCodigosEstrategia tr:hover td {
                background: #d0dafd;
                color: #339;
            }

        #btnEliminarCodigo {
            border-radius: 6px;
            background-color: #a32035;
            color: #ffffff;
            padding: 4px 3px;
        }

        .btnOperacionesCodigo {
            border-radius: 6px;
            background-color: #327fa2;
            color: #ffffff;
            padding: 4px 3px;
        }

        .txtpnCodigoExtrategia {
            width: 15%;
            border-radius: 6px;
            background-color: #e5cff7;
            padding: 4px 3px;
        }
    </style>

    <script language="javascript" type="text/javascript">

        function OnExpandCollapseButtonClick(s, e) {
            var isVisible = pnlDatos.GetVisible();
            s.SetText(isVisible ? "+" : "-");
            pnlDatos.SetVisible(!isVisible);
        }

        function LimpiaFormulario() {
            if (confirm("¿Realmente desea limpiar los campos del formulario?")) {
                ASPxClientEdit.ClearEditorsInContainerById('formPrincipal');
                rblEstado.SetSelectedIndex(0);
            }
        }

        function EjecutarCallbackRegistro(s, e, parametro, valor) {

            var codigosEstrategia;

            $('#tblCodigosEstrategia tbody tr').each(function () {
                var temp = $(this)[0].firstElementChild.innerText;

                if (temp != 'Código') {
                    if (codigosEstrategia == undefined) {
                        codigosEstrategia = temp;
                    }
                    else {
                        codigosEstrategia = codigosEstrategia + '|' + temp;
                    }
                }
            });

            $('input[id$=hdfCodigosEstrategia]').val(codigosEstrategia);

            var crearUsuarioFueraBaseCargue = $('#chkCargueUsuariosFueraBase').prop('checked');



            if (ASPxClientEdit.AreEditorsValid()) {
                LoadingPanel.Show();
                cpRegistro.PerformCallback(parametro + ':' + valor);
            }
        }

        function EvaluarFiltros() {
            if (txtFiltroCampania.GetValue() == null && cmbTipoServicio.GetValue() == null && cmbCliente.GetValue() == null && cmbCiudad.GetValue() == null) {
                alert('Debe seleccionar por lo menos un filtro de búsqueda.');
            } else {
                LoadingPanel.Show();
                gvCampanias.PerformCallback();
            }
        }

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

        function Editar(element, key) {
            TamanioVentana();
            dialogoVer.SetContentUrl("EditarCampaniaFinanciero.aspx?idCampania=" + key);
            dialogoVer.SetSize(myWidth * 0.9, myHeight * 0.9);
            dialogoVer.ShowWindow();
        }

        function ValidaNumero(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return ((tecla > 47 && tecla < 58));
        }

        function AgregarCodigoEstrategia(origen, _this) {
            if (origen == 'autocomplete') {

                $('#tblCodigosEstrategia').empty();

                tr = $('<tr/>');
                tr.append("<th>Código</th>");
                tr.append("<th>Eliminar de esta campaña</th>");
                $('#tblCodigosEstrategia').append(tr);

                tr = $('<tr/>');
                tr.append("<td id='tdContenidoFilaCodigo'>" + _this + "</td>");
                tr.append("<td><input type='button' value='-' id='btnBorrarCodigoTabla' style='cursor: pointer' onclick='AgregarCodigoEstrategia(2, this)' /></td>");
                $('#tblCodigosEstrategia').append(tr);
                $('#txtpnCodigoExtrategia').val('');
                $('#txtpnCodigoExtrategia').focus();
                $('#btnActualizarCodigo').css('display', 'none');
                $('#btnEliminarCodigo').css('display', 'none');
                $('#btnAgregarEstrategia').css('display', 'none');
            }
            else if (origen == 2) {
                    $(_this).parent().parent().remove();
                    $('#txtpnCodigoExtrategia').focus();
            }
            else {
                alert('Error al agregar código a la campaña', 'rojo');
            }
        }


        function AutoComplete() {
            var pagePath = window.location.pathname;
            var filtro = $('#txtpnCodigoExtrategia').val();

            if (ValidarCantidadCaracteres(filtro) == false) {
                return false;
            }

            $.ajax({
                type: "POST",
                url: pagePath + "/ConsultaAutocomplete",
                data: "{operacion:'" + 1 + "', filtroBusqueda:'" + filtro + "'}",
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    var codigosEstrategia = $.parseJSON(result.d);

                    $('#btnGuardarNuevoCodigo').css('display', 'none');

                    if (codigosEstrategia.length > 0) {
                        var resultadoBusqueda = [];
                        for (var x = 0; x < codigosEstrategia.length; x++) {
                            var tmp = codigosEstrategia[x].CCEcodigo;
                            resultadoBusqueda.push(tmp);
                        }

                        $("#txtpnCodigoExtrategia").autocomplete({
                            source: resultadoBusqueda,
                            open: function (event, ui) {
                                $('#btnEliminarCodigo').css('display', 'none');
                                $('#btnActualizarCodigo').css('display', 'none');
                                $('#btnAgregarEstrategia').css('display', 'none');
                            },

                            select: function (event, ui) {
                                AgregarCodigoEstrategia('autocomplete', ui.item.value);
                                $('#btnEliminarCodigo').css('display', 'inline');
                                $('#btnActualizarCodigo').css('display', 'inline');
                                //$('#btnAgregarEstrategia').css('display', 'inline');

                                EliminarCodigoEstrategia(ui);
                                ActualizarCodigoEstrategia(ui);
                                
                            }
                        });
                    }
                    else {
                        alert('El código: ' + filtro + ' no existe. Puede agregarlo si lo desea', 'azul');
                        $('#btnGuardarNuevoCodigo').css('display', 'inline');
                        $('#btnEliminarCodigo').css('display', 'none');
                        $('#btnActualizarCodigo').css('display', 'none');
                        $('#txtActualizarCodigoEstrategia').css('display', 'none');
                    }
                },
                error: function (e, f, g) {
                    alert('error: ' + e.toString(), 'rojo');
                }
            });
        }

        function GuardarNuevoCodigoEstrategia() {
            var pagePath = window.location.pathname;
            var palabraSinLimpiar = $('#txtpnCodigoExtrategia').val();
            var nuevoCodigo = LimpiarPalabras(palabraSinLimpiar);

            $.ajax({
                type: "POST",
                url: pagePath + "/GuardarNuevoCodEstrategia",
                data: "{nuevoCodigo:'" + nuevoCodigo + "'}",
                contentType: "application/json",
                dataType: "json",
                success: function (result) {

                    if(result.d.length > 0)
                    {
                        var mensaje = result.d.split('|');
                        alert(mensaje[0], mensaje[1]);
                        $('#btnGuardarNuevoCodigo').css('display', 'none');
                        $('#txtpnCodigoExtrategia').val('');
                    }
                },
                error: function (e, f, g) {
                    alert('error: ' + e.toString(), 'rojo');
                }
            });
        }

        function ActualizarCodigoEstrategia(origen) {

            if (origen.item != undefined) {
                $('#btnActualizarCodigo').val('Actualizar código: ' + origen.item.label)
            }

            if (origen == 'btnActualizar') {
                $('#txtActualizarCodigoEstrategia').css('display', 'inline');
                $('#txtActualizarCodigoEstrategia').focus();

                if ($('#txtActualizarCodigoEstrategia').val().length > 0) {
                    var codigoAnteriorC = $('#txtpnCodigoExtrategia').val();
                    var codigoNuevoC = $('#txtActualizarCodigoEstrategia').val();
                    var pagePath = window.location.pathname;

                    $.ajax({
                        type: "POST",
                        url: pagePath + "/ActualizarCodigoEstrategia",
                        data: "{codigoAnterior:'" + codigoAnteriorC + "', codigoNuevo:'" + codigoNuevoC + "'}",
                        contentType: "application/json",
                        dataType: "json",
                        success: function (result) {

                            if (result.d.length > 0) {
                                var mensaje = result.d.split('|');
                                alert(mensaje[0], mensaje[1]);
                                $('#txtpnCodigoExtrategia').val('');
                                $('#txtActualizarCodigoEstrategia').val('');
                                $('#btnEliminarCodigo').css('display', 'none');
                                $('#btnActualizarCodigo').css('display', 'none');
                                $('#txtActualizarCodigoEstrategia').css('display', 'none');
                                $('#btnAgregarEstrategia').css('display', 'none');
                            }
                        },
                        error: function (e, f, g) {
                            alert('error: ' + e.toString(), 'rojo');
                        }
                    });
                }

            }
        }

        function EliminarCodigoEstrategia(origen) {

            if (origen.item != undefined) {
                $('#btnEliminarCodigo').val('Eliminar código: ' + origen.item.label);
            }

            if (origen == 'btnEliminar') {
                var pagePath = window.location.pathname;
                var codigoEliminarC = $('#txtpnCodigoExtrategia').val();

                $.ajax({
                    type: "POST",
                    url: pagePath + "/EliminarCodigoEstrategia",
                    data: "{codigoEliminar:'" + codigoEliminarC + "'}",
                    contentType: "application/json",
                    dataType: "json",
                    success: function (result) {

                        if (result.d.length > 0) {
                            var mensaje = result.d.split('|');
                            alert(mensaje[0], mensaje[1]);
                            $('#txtpnCodigoExtrategia').val('');
                            $('#btnEliminarCodigo').css('display', 'none');
                            $('#btnActualizarCodigo').css('display', 'none');
                            $('#btnAgregarEstrategia').css('display', 'none');
                            $('#txtActualizarCodigoEstrategia').css('display', 'none');
                        }
                    },
                    error: function (e, f, g) {
                        alert('error: ' + e.toString(), 'rojo');
                    }
                });
            }
        }

        function ValidarCantidadCaracteres(conteoCaracteres)
        {
            if (conteoCaracteres.length > 10) {
                alert('El código debe ser máximo de 10 caracteres', 'rojo');
                return false;
            }
            else {
                return true;
            }
        }

        function LimpiarPalabras(palabra) {
            //Elimina caracteres especiales y espacios en blanco

            palabra = palabra.replace(/[^a-zA-Z0-9 ]/g, '').replace(/ /g, '')

            return palabra;
        }

    </script>

</head>
<body>
    <form id="formPrincipal" runat="server">
        <div id="divEncabezado">
            <uc2:encabezadopagina id="miEncabezado" runat="server" />
        </div>
        <div style="float: left; margin-right: 30px; margin-bottom: 5px; margin-top: 5px; width: 70%;">
            <dx:ASPxCallbackPanel ID="cpRegistro" runat="server"  ClientInstanceName="cpRegistro">
                <ClientSideEvents EndCallback="function(s,e){ 
                    var color = 'amarillo';
                    if(s.cpValor == 1 || s.cpValor == 400)
                    {
                        color = 'rojo';
                    }
                    else if(s.cpValor == 0)
                    {
                        color = 'verde';
                    }

                    alert(s.cpMensaje, color);
                    
                    LoadingPanel.Hide();
        
                }"></ClientSideEvents>
               
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxRoundPanel ID="rpCampania" runat="server" HeaderText="Administración Campañas Financiero"
                            Width="100%">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="white-space: nowrap;" align="left">Administraci&oacute;n 
                                        </td>
                                        <td style="width: 1%; padding-left: 5px;">
                                            <dx:ASPxButton ID="btnExpandCollapse" runat="server" Text="-" AllowFocus="False"
                                                AutoPostBack="False" Width="20px">
                                                <Paddings Padding="1px" />
                                                <FocusRectPaddings Padding="0" />
                                                <ClientSideEvents Click="OnExpandCollapseButtonClick" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxPanel ID="pnlDatos" runat="server" Width="100%" ClientInstanceName="pnlDatos">
                                        <PanelCollection>
                                            <dx:PanelContent>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="field" align="left" style="width: 30%">Nombre Campaña:
                                                        </td>
                                                        <td style="width: 40%">
                                                            <dx:ASPxTextBox ID="txtNombreCampania" runat="server" Width="100%" TabIndex="1">
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                    <RequiredField ErrorText="El nombre de la campaña  es requerido" IsRequired="True" />
                                                                    <RegularExpression ErrorText="Formato no valido" ValidationExpression="^\s*[a-zA-Z_0-9,;:\.\*\!\¡\?\¿\b\sáéíóúÁÉÍÓÚñÑ\-\#]+\s*$" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td class="field" align="left" style="width: 100%">Cliente:
                                                        </td>
                                                        <td style="width: 100%">
                                                            <dx:ASPxComboBox ID="cmbCl" runat="server" Width="110%" IncrementalFilteringMode="Contains"
                                                                ClientInstanceName="cmbCl" Enabled="false" SelectedIndex="0" DropDownStyle="DropDownList" TabIndex="2" ValueType="System.Int32">
                                                                <Columns>
                                                                    <dx:ListBoxColumn FieldName="nombre" Width="250px" Caption="Descripción" />
                                                                </Columns>
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                    <RequiredField ErrorText="El cliente de la campaña  es requerido" IsRequired="True" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="field" align="left" style="width: 30%">Fecha Vigencia Inicial:
                                                        </td>
                                                        <td style="width: 40%">
                                                            <dx:ASPxDateEdit ID="dateFechaInicio" runat="server" NullText="Inicial..." ClientInstanceName="dateFechaInicio"
                                                                Width="100%" TabIndex="3">
                                                                <ClientSideEvents ValueChanged="function(s, e){
                                                                    dateFechaFin.SetMinDate(dateFechaInicio.GetDate());
                                                                }" />
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                    <RequiredField IsRequired="true" ErrorText="Registro requerido" />
                                                                </ValidationSettings>
                                                            </dx:ASPxDateEdit>
                                                        </td>
                                                        <td class="field" align="left" style="width: 100%">Fecha Vigencia Final:
                                                        </td>
                                                        <td style="width: 100%">
                                                            <dx:ASPxDateEdit ID="dateFechaFin" runat="server" NullText="Final..." ClientInstanceName="dateFechaFin"
                                                                Width="110%" TabIndex="4">
                                                                <ClientSideEvents ValueChanged="function(s, e){
                                                                    dateFechaInicio.SetMaxDate(dateFechaFin.GetDate());
                                                                }" />
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                    <RequiredField IsRequired="true" ErrorText="Registro requerido" />
                                                                </ValidationSettings>
                                                            </dx:ASPxDateEdit>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="field" align="left" style="width: 30%">Meta Cliente:
                                                        </td>
                                                        <td style="width: 40%">
                                                            <div style="display: inline; float: left; width: 80%">
                                                                <dx:ASPxTextBox ID="txtMetaCliente" runat="server" ClientInstanceName="txtMetaCliente" Width="125px" MaxLength="3" TabIndex="5"
                                                                    onkeypress="javascript:return ValidaNumero(event);">
                                                                    <ClientSideEvents LostFocus ="function(s,e){
                                                                        if (txtMetaCliente.GetText() != ''){
                                                                            var meta = txtMetaCliente.GetText()
                                                                            if ( meta >= 101){
                                                                                alert('El valor del porcentaje de la meta de Davivienda no puede ser mayor a 100');
                                                                                txtMetaCliente.SetText('');
                                                                                txtMetaCliente.SetFocus();
                                                                            }  else if (meta <= 0) { 
                                                                                alert('El valor del porcentaje de la meta deDavivienda debe ser mayor a 0');
                                                                                txtMetaCliente.SetText('');
                                                                                txtMetaCliente.SetFocus();                                                                        
                                                                            }
                                                                        }
                                                                    }" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                        <RequiredField ErrorText="la Meta de cumplimiento del cliente es requerido" IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                            <div style="display: inline; float: left; align-items: center">
                                                                %
                                                            </div>
                                                        </td>
                                                        <td class="field" align="left" style="width: 100%">Meta Callcenter:
                                                        </td>
                                                        <td>
                                                            <div style="display: inline; float: left; width: 80%">
                                                                <dx:ASPxTextBox ID="txtMetaCall" runat="server" ClientInstanceName="txtMetaCall" Width="125px" MaxLength="3" TabIndex="6"
                                                                    onkeypress="javascript:return ValidaNumero(event);">
                                                                    <ClientSideEvents LostFocus ="function(s,e){
                                                                        if (txtMetaCall.GetText() != ''){
                                                                            var metaCall = txtMetaCall.GetText()
                                                                            if (metaCall >= 101){
                                                                                alert('El valor del porcentaje de la meta del Callcenter no puede ser mayor a 100');
                                                                                txtMetaCall.SetText('');
                                                                                txtMetaCall.SetFocus();
                                                                            } else if (metaCall <= 0) { 
                                                                                alert('El valor del porcentaje de la meta del Callcenter debe ser mayor a 0');
                                                                                txtMetaCall.SetText('');
                                                                                txtMetaCall.SetFocus();                                                                        
                                                                            }
                                                                        }
                                                                    }" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                        <RequiredField ErrorText="la Meta de cumplimiento del Callcenter es requerido" IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                            <div style="display: inline; float: left">
                                                                %
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="field" align="left" style="width: 30%">Tipo de Campaña:
                                                        </td>
                                                        <td style="width: 40%">
                                                            <dx:ASPxComboBox ID="cmbTipoCampania" runat="server" Width="100%" IncrementalFilteringMode="Contains"
                                                                ClientInstanceName="cmbTipoCampania" DropDownStyle="DropDownList" TabIndex="7" ValueType="System.Int32">
                                                                  <Columns>
                                                                    <dx:ListBoxColumn FieldName="tipoCampania" Width="250px" Caption="Tipo" />
                                                                </Columns>
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgCampania">
                                                                    <RequiredField ErrorText="El tipo de campaña  es requerido" IsRequired="True" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                        <td class="field" align="left" style="width: 30%">Producto personalizado:
                                                        </td>
                                                        <td>
                                                            <dx:ASPxRadioButtonList ID="rblPersonalizacion" runat="server" RepeatDirection="Horizontal" TabIndex="8"
                                                                ClientInstanceName="rblPersonalizacion" Font-Size="XX-Small" Height="10px" AutoPostBack="false">
                                                                <Items>
                                                                    <dx:ListEditItem Text="Con Realce" Value="1" />
                                                                    <dx:ListEditItem Text="Sin Realce" Value="0" Selected="true" />
                                                                </Items>
                                                                <Border BorderStyle="None"></Border>
                                                                <ClientSideEvents ValueChanged="function(s,e){
                                                                    if (rblPersonalizacion.GetValue()==1){
                                                                        $('#cpRegistro_rpCampania_pnlDatos_divTr').css('display', 'inline');
                                                                    } else {
                                                                        $('#cpRegistro_rpCampania_pnlDatos_divTr').css('display', 'none');
                                                                    }
                                                                }" />
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="vgRegistro">
                                                                    <RequiredField ErrorText ="Información Requerida" IsRequired ="true" />
                                                                </ValidationSettings> 
                                                            </dx:ASPxRadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 59%;text-align: center;">
                                                            <span id="spnCargueUsuariosFueraBase">Cargar usuarios fuera de la base</span>
                                                        </td>
                                                        <td>
                                                            <%--<input type="checkbox" id="chkCargueUsuariosFueraBase" style="margin-left: 5%;"/>--%>
                                                            <asp:CheckBox ID="chkCargueUsuariosFueraBase" runat="server" />
                                                        </td>
                                                    </tr>
                                                 
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id="divTr" runat="server" style="display:none">
                                                                <table>
                                                                    <tr>
                                                                         <td class="field" align="left" style="width: 30%">Fecha de Entrega:
                                                                         </td>
                                                                        <td  style="width: 40%">
                                                                            <dx:ASPxDateEdit ID="dateFechaLlegada" runat="server" NullText="Fecha..." ClientInstanceName="dateFechaLlegada"
                                                                                Width="85%" TabIndex="9">
                                                                            </dx:ASPxDateEdit>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <dx:ASPxPageControl ID="pcAsociadosCampania" runat="server" ActiveTabIndex="0" Width="100%" TabIndex="10">
                                                                <TabPages>
                                                                    <dx:TabPage Text="Tipo Servicio">
                                                                        <TabImage Url="../img/structure.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl ID="ContentControl1" runat="server">
                                                                                <dx:ASPxPanel ID="pnlServicios" runat="server" ScrollBars="Auto" Height="250px">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent>
                                                                                            <dx:ASPxListBox ID="lbServicios" runat="server" Width="250px" SelectionMode="Single"
                                                                                                ValueField="IdTipoServicio" Height="250px" ClientInstanceName="lbServicios">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn FieldName="Nombre" Caption="Tipo Servicio" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dx:ASPxPanel>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>
                                                                    <dx:TabPage Text="Bodegas CEM">
                                                                        <TabImage Url="../img/list_num.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl ID="ContentControl2" runat="server">
                                                                                <dx:ASPxPanel ID="pnlBodegas" runat="server" ScrollBars="Auto" Height="250px">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent>
                                                                                            <dx:ASPxListBox ID="lbBodegas" runat="server" Width="250px" SelectionMode="CheckColumn"
                                                                                                ValueField="idbodega" Height="250px" ClientInstanceName="lbBodegas">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn FieldName="bodega" Caption="Bodega" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dx:ASPxPanel>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>
                                                                    <dx:TabPage Text="Producto Externo">
                                                                        <TabImage Url="../img/DxPikingList.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl ID="ContentControl3" runat="server">
                                                                                <dx:ASPxPanel ID="pnlProductoExt" runat="server" ScrollBars="Auto" Height="250px">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent>
                                                                                            <dx:ASPxListBox ID="lbProductoExt" runat="server" Width="250px" SelectionMode="CheckColumn"
                                                                                                ValueField="idProductoComercial" Height="250px" ClientInstanceName="lbProductoExt">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn FieldName="productoExterno" Caption="Producto" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dx:ASPxPanel>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>
                                                                    <dx:TabPage Text="Documentos">
                                                                        <TabImage Url="../img/documents_stack.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl ID="ContentControl4" runat="server">
                                                                                <dx:ASPxPanel ID="pnlDocumentos" runat="server" ScrollBars="Auto" Height="250px">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent>
                                                                                            <dx:ASPxListBox ID="lbDocumentos" runat="server" Width="250px" SelectionMode="CheckColumn"
                                                                                                ValueField="IdProducto" Height="250px" ClientInstanceName="lbDocumentos">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn FieldName="Nombre" Caption="Documentos" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dx:ASPxPanel>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>

                                                                    <dx:TabPage Text="Estrategia">
                                                                        <TabImage Url="../img/documents_stack.png">
                                                                        </TabImage>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl ID="ContentControl5" runat="server">
                                                                                <dx:ASPxPanel ID="ASPxPanel1" runat="server" ScrollBars="Auto" Height="250px">
                                                                                    <PanelCollection>
                                                                                        <dx:PanelContent>
                                                                                            <div style="margin-top: 2%;margin-left: 22%;">
                                                                                                <div>
                                                                                                    <span id="spnCodigoExtrategia">Código Estrategia</span>
                                                                                                    <input type="text" id="txtpnCodigoExtrategia" class="txtpnCodigoExtrategia" style="width: 18%" maxlength="10" onkeyup="AutoComplete()" placeholder="Ej. 2kp"/>
                                                                                                    <input type="text" id="txtActualizarCodigoEstrategia" class="txtpnCodigoExtrategia" onkeyup="ValidarCantidadCaracteres($('#txtActualizarCodigoEstrategia').val())" maxlength="10" placeholder="Nuevo" style="display: none"/>
                                                                                                </div>
                                                                                                <div style="margin-top: 2%; margin-left: 1%;">
                                                                                                    <input type="button" id="btnAgregarEstrategia" value="+ Agregar a campaña" onclick="AgregarCodigoEstrategia()" style="display:none" />
                                                                                                    <input type="button" id="btnGuardarNuevoCodigo" class="btnOperacionesCodigo" value ="Guardar nuevo código" onclick="GuardarNuevoCodigoEstrategia()" style="display:none" />
                                                                                                    <input type="button" id="btnActualizarCodigo" class="btnOperacionesCodigo" value ="" onclick="ActualizarCodigoEstrategia('btnActualizar')" style="display:none" />
                                                                                                    <input type="button" id="btnEliminarCodigo" value ="" onclick="EliminarCodigoEstrategia('btnEliminar')" style="display:none" />
                                                                                                </div>

                                                                                                <div style="margin-left: 1%;margin-top: 4%;">
                                                                                                    <table id="tblCodigosEstrategia">
                                                                                                    
                                                                                                       
                                                                                                    </table>
                                                                                                    <asp:HiddenField ID="hdfCodigosEstrategia" runat="server" />
                                                                                                </div>
                                                                                            </div>
                                                                                            
                                                                                            <%--<dx:ASPxListBox ID="ASPxListBox1" runat="server" Width="250px" SelectionMode="CheckColumn"
                                                                                                ValueField="IdProducto" Height="250px" ClientInstanceName="lbDocumentos">
                                                                                                <Columns>
                                                                                                    <dx:ListBoxColumn FieldName="Nombre" Caption="Documentos" Width="250px" />
                                                                                                </Columns>
                                                                                            </dx:ASPxListBox>--%>
                                                                                        </dx:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dx:ASPxPanel>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>

                                                                </TabPages>
                                                            </dx:ASPxPageControl>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <dx:ASPxImage ID="imgCrear" runat="server" ImageUrl="../img/DxConfirm32.png" TabIndex="11"
                                                                ToolTip="Crear Campaña" Cursor="pointer">
                                                                <ClientSideEvents Click="function (s, e){
                                                                    if(ASPxClientEdit.ValidateGroup('vgCampania')){
                                                                        if(lbServicios.GetSelectedValues().length==0){
                                                                            alert('No se han seleccionado los Servicios requeridos', 'rojo');
                                                                        } else if (lbBodegas.GetSelectedValues().length==0) {
                                                                            alert('No se han seleccionado las Ciudades(Bodegas) requeridas', 'rojo');
                                                                        } else if (lbProductoExt.GetSelectedValues().length==0){
                                                                            alert('No se han seleccionado los Productos requeridos', 'rojo');
                                                                        } else if (lbDocumentos.GetSelectedValues().length==0){
                                                                            alert('No se han seleccionado los Documentos requeridos', 'rojo');
                                                                        } else if (rblPersonalizacion.GetValue()==1 && dateFechaLlegada.GetValue()==null){
                                                                            alert('La fecha de llegada del producto es obligatoria', 'rojo');
                                                                        } else if ($('#tblCodigosEstrategia #tdContenidoFilaCodigo').text().length == 0){
                                                                            alert('Debe elegir un código de estrategia', 'rojo');
                                                                        } else {
                                                                            //alert('Registrar');
                                                                            EjecutarCallbackRegistro(s,e,'Registrar');
                                                                        }
                                                                    }
                                                                }" />
                                                            </dx:ASPxImage>
                                                            <dx:ASPxImage ID="imgCancelar" runat="server" ImageUrl="../img/DxCancel32.png" TabIndex="12"
                                                                ToolTip="Cancelar" Cursor="pointer">
                                                                <ClientSideEvents Click="function(s, e){
                                                                    LimpiaFormulario();
                                                                }" />
                                                            </dx:ASPxImage>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxPanel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>

        <div style="float: left; margin-top: 5px; width: 10%; height:10%">
            <dx:ASPxCallbackPanel runat="server" ID="cpBusquedaCampanias" ClientInstanceName="cpBusquedaCampanias"
                >
                <PanelCollection>
                    <dx:PanelContent>
                        <div style="margin-bottom: 15px">
                            <dx:ASPxRoundPanel ID="rpFiltroCampanias" runat="server" HeaderText="Filtro de Búsqueda">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <table cellpadding="1">
                                            <tr>
                                                <td class="field" align="left">Nombre Campaña:
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtFiltroCampania" runat="server" Width="250px" TabIndex="13" ClientInstanceName="txtFiltroCampania">
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td class="field">Tipo Servicio
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cmbTipoServicio" runat="server" Width="250px" IncrementalFilteringMode="Contains"
                                                        ClientInstanceName="cmbTipoServicio" DropDownStyle="DropDownList" TabIndex="14" ValueType="System.Int32">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="nombre" Width="250px" Caption="Descripción" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="field" align="left">Cliente Externo:
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cmbCliente" runat="server" Width="250px" IncrementalFilteringMode="Contains"
                                                        ClientInstanceName="cmbCliente" ClientEnabled="false" SelectedIndex="0" DropDownStyle="DropDownList" TabIndex="15" ValueType="System.Int32">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="nombre" Width="250px" Caption="Descripción" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td class="field" align="left">Ciudad:
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cmbCiudad" runat="server" Width="250px" IncrementalFilteringMode="Contains"
                                                        ClientInstanceName="cmbCiudad" DropDownStyle="DropDownList" TabIndex="16" ValueType="System.Int32">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="ciudadDepartamento" Width="250px" Caption="Descripción" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="field" align="left">Estado:
                                                </td>
                                                <td>
                                                    <dx:ASPxRadioButtonList ID="rblEstado" runat="server" RepeatDirection="Horizontal"
                                                        ClientInstanceName="rblEstado" Font-Size="XX-Small" TabIndex="17">
                                                        <Items>
                                                            <dx:ListEditItem Text="Activo" Value="1" Selected="true" />
                                                            <dx:ListEditItem Text="Inactivo" Value="0" />
                                                        </Items>
                                                        <Border BorderStyle="None"></Border>
                                                    </dx:ASPxRadioButtonList>
                                                </td>
                                                <td colspan="2" align="center">
                                                    <dx:ASPxImage ID="imgFiltro" runat="server" ImageUrl="../img/DxAdd32.png" TabIndex="18"
                                                        ToolTip="Filtrar" Cursor="pointer">
                                                        <ClientSideEvents Click="function (s, e){
                                                            EvaluarFiltros();
                                                        }" />
                                                    </dx:ASPxImage>
                                                    <dx:ASPxImage ID="imgCancela" runat="server" ImageUrl="../img/DxCancel32.png" TabIndex="19"
                                                        ToolTip="Cancelar" Cursor="pointer">
                                                        <ClientSideEvents Click="function(s, e){
                                                        LimpiaFormulario();
                                                    }" />
                                                    </dx:ASPxImage>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </div>
                        <div style="margin-bottom: 5px;">
                            <dx:ASPxRoundPanel ID="rpResultadoCampanias" runat="server" HeaderText="Listado de Campañas" TabIndex="20"
                                Width="100%">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxGridView ID="gvCampanias" runat="server" Width="100%" ClientInstanceName="gvCampanias"
                                            AutoGenerateColumns="False" KeyFieldName="IdCampania" SettingsLoadingPanel-Mode="Disabled">
                                            <ClientSideEvents EndCallback="function(s,e){ 
                                            $('#divEncabezado').html(s.cpMensaje);
                                            LoadingPanel.Hide();
                                        }"></ClientSideEvents>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="ID" ShowInCustomizationForm="True" VisibleIndex="0"
                                                    FieldName="IdCampania">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Nombre Campaña" ShowInCustomizationForm="True"
                                                    VisibleIndex="1" FieldName="Nombre">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Fecha Inicio" FieldName="FechaInicio" ShowInCustomizationForm="True"
                                                    VisibleIndex="2">
                                                    <PropertiesTextEdit DisplayFormatString="{0:d}">
                                                    </PropertiesTextEdit>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn Caption="Fecha Fin" ShowInCustomizationForm="true" VisibleIndex="3"
                                                    FieldName="FechaFin">
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataCheckColumn Caption="Activo" ShowInCustomizationForm="true" VisibleIndex="5"
                                                    FieldName="Activo">
                                                </dx:GridViewDataCheckColumn>
                                                <dx:GridViewDataColumn Caption="" VisibleIndex="6">
                                                    <DataItemTemplate>
                                                        <dx:ASPxHyperLink runat="server" ID="lnkEditar" ImageUrl="../img/Edit-User.png"
                                                            Cursor="pointer" ToolTip="Ver / Editar Plan" OnInit="Link_Init">
                                                            <ClientSideEvents Click="function(s, e) { Editar(this, {0}); }" />
                                                        </dx:ASPxHyperLink>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                            <SettingsText Title="B&#250;squeda General de Campañas" EmptyDataRow="No se encontraron datos acordes con los filtros de b&amp;uacute;squeda"></SettingsText>
                                            <SettingsLoadingPanel Mode="Disabled"></SettingsLoadingPanel>
                                        </dx:ASPxGridView>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>

        <dx:ASPxPopupControl ID="pcVer" runat="server" ClientInstanceName="dialogoVer" HeaderText="Información"
            AllowDragging="true" Width="410px" Height="260px" Modal="true" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
            Modal="True">
        </dx:ASPxLoadingPanel>
    </form>
</body>
</html>
