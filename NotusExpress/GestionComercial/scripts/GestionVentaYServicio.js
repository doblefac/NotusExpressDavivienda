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

function ClosePopUp() {
    window.parent.dialogoVer.Hide();
}

function AbrirModalProductos() {
    TamanioVentana();

    var valor = txtNumIdentificacion.GetValue();
    dialogoServicio.PerformCallback('Inicial:' + valor);
    dialogoServicio.SetSize(myWidth * 0.6, myHeight * 0.75);
    dialogoServicio.ShowWindow();
}

function EjecutarCallbackGeneral(s, e, parametro, valor) {
    if (ASPxClientEdit.AreEditorsValid()) {
        loadingPanel.Show();
        cpGeneral.PerformCallback(parametro + ':' + valor);
    }
}

function EjecutarCallbackId(s, e, parametro) {
    if (ASPxClientEdit.AreEditorsValid()) {
        loadingPanel.Show();
        cbCallBackId.PerformCallback(parametro);
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return true;

    return false;
}

function soloLetras(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 8 || tecla == 32) return true;
    patron = /[A-Za-zñÑ]/;
    te = String.fromCharCode(tecla);
    return patron.test(te);
}

function direccion(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 8) return true;
    patron = /[A-Za-zñÑ\d]/;
    te = String.fromCharCode(tecla);
    return patron.test(te);
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

function MostrarOcultarDivFloater(mostrar) {
    var valorDisplay = mostrar ? "block" : "none";
    var elDiv = document.getElementById("divFloater");
    elDiv.style.display = valorDisplay;
}

function LimpiaFormulario() {
    if (confirm("¿Realmente desea limpiar los campos del formulario?")) {
        location.reload();
    }
}

function Eliminar(s, e) {
    if (confirm("Esta seguro que desea eliminar el servicio?")) {
        dialogoServicio.PerformCallback('Eliminar:' + e);
    }
}

function AgendarVenta(valor) {
    TamanioVentana();
    dialogoVer.SetContentUrl("GestionAgendamiento.aspx?id=" + valor);
    dialogoVer.SetSize(myWidth * 0.9, myHeight * 0.9);
    dialogoVer.ShowWindow();
}

function AgregarNuevoCliente() {
    var filtroBusqueda = 1;
    var filtro1 = 1;
    var pagePath = window.location.pathname;

    $.ajax({
        type: "POST",
        url: pagePath + "/AgregarClienteFueraBase",
        data: "{operacion:'" + filtroBusqueda + "', filtroBusqueda:'" + filtro1 + "'}",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {

            $('#btnGuardarNuevoCodigo').css('display', 'none');
            $('#txtpnCodigoExtrategia').val('');
            alert(result.d);
        },
        error: function (e, f, g) {
            alert('error: ' + e.toString());
        }
    });
}//Eliminar este método..

function RegistrarTransitorio(valor, campania, cupo) {
    if (cupo >= 1) {
        dialogoServicio.PerformCallback('Registro:' + valor + ':' + campania);
    }
    else {
        alert('El valor del cupo debe ser mayor a cero', 'rojo');
    }

}