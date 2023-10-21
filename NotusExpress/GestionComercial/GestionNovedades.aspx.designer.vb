'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class GestionNovedades

    '''<summary>
    '''Control form1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents form1 As Global.System.Web.UI.HtmlControls.HtmlForm

    '''<summary>
    '''Control epNotificador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents epNotificador As Global.NotusExpress.EncabezadoPagina

    '''<summary>
    '''Control cpResultadoReporte.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cpResultadoReporte As Global.DevExpress.Web.ASPxCallbackPanel

    '''<summary>
    '''Control vuControlSesion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents vuControlSesion As Global.NotusExpress.ValidacionURL

    '''<summary>
    '''Control pnlConsulta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlConsulta As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control imgSearch.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgSearch As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control cboPdv.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboPdv As Global.DevExpress.Web.ASPxComboBox

    '''<summary>
    '''Control cbPdvActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbPdvActivo As Global.DevExpress.Web.ASPxCheckBox

    '''<summary>
    '''Control cboAsesor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboAsesor As Global.DevExpress.Web.ASPxComboBox

    '''<summary>
    '''Control cbAsesorActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbAsesorActivo As Global.DevExpress.Web.ASPxCheckBox

    '''<summary>
    '''Control cboResultadoGestion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboResultadoGestion As Global.DevExpress.Web.ASPxComboBox

    '''<summary>
    '''Control cboTipoProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cboTipoProducto As Global.DevExpress.Web.ASPxComboBox

    '''<summary>
    '''Control deFechaInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents deFechaInicio As Global.DevExpress.Web.ASPxDateEdit

    '''<summary>
    '''Control deFechaFin.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents deFechaFin As Global.DevExpress.Web.ASPxDateEdit

    '''<summary>
    '''Control btnConsultar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnConsultar As Global.DevExpress.Web.ASPxButton

    '''<summary>
    '''Control btnLimpiar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnLimpiar As Global.DevExpress.Web.ASPxButton

    '''<summary>
    '''Control pnlVentasConNovedad.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlVentasConNovedad As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control gvVentasConNovedades.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvVentasConNovedades As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control pnlInfoOrigenGestion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlInfoOrigenGestion As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control cpPDV.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cpPDV As Global.EO.Web.CallbackPanel

    '''<summary>
    '''Control ddlPdv.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlPdv As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control chkPdvActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkPdvActivo As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control rfvPdv.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvPdv As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control cpAsesorComercial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cpAsesorComercial As Global.EO.Web.CallbackPanel

    '''<summary>
    '''Control ddlAsesorComercial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlAsesorComercial As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control chkAsesorActivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkAsesorActivo As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control rfvAsesorComercial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvAsesorComercial As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control pnlGestion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlGestion As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control dpFechaVenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dpFechaVenta As Global.EO.Web.DatePicker

    '''<summary>
    '''Control rfvFechaVenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvFechaVenta As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtAtendidoPor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtAtendidoPor As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control revAtendidoPor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents revAtendidoPor As Global.System.Web.UI.WebControls.RegularExpressionValidator

    '''<summary>
    '''Control rfvAtendidoPor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvAtendidoPor As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtNumIdOperadorCallCenter.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumIdOperadorCallCenter As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control revNumIdOperadorCall.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents revNumIdOperadorCall As Global.System.Web.UI.WebControls.RegularExpressionValidator

    '''<summary>
    '''Control txtNumPlanillaPreAnalisis.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumPlanillaPreAnalisis As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control revNumPlanillaPreAnalisis.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents revNumPlanillaPreAnalisis As Global.System.Web.UI.WebControls.RegularExpressionValidator

    '''<summary>
    '''Control rfvNumPlanillaPreAnalisis.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvNumPlanillaPreAnalisis As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtNumVentaPlanilla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumVentaPlanilla As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control revNumVentaPlanilla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents revNumVentaPlanilla As Global.System.Web.UI.WebControls.RegularExpressionValidator

    '''<summary>
    '''Control rfvNumVentaPlanilla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvNumVentaPlanilla As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control ddlResultadoConsulta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlResultadoConsulta As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control rfvResultadoConsulta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvResultadoConsulta As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control ddlTipoProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlTipoProducto As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control rfvTipoProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvTipoProducto As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control trInfoProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trInfoProducto As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''Control ddlProductoPadre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlProductoPadre As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control rfvTipoDeTarjetaRequerido.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvTipoDeTarjetaRequerido As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control ddlSubproducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlSubproducto As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control rfvProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvProducto As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control trInfoSerial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trInfoSerial As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''Control txtNumPagare.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumPagare As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control revNumPagare.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents revNumPagare As Global.System.Web.UI.WebControls.RegularExpressionValidator

    '''<summary>
    '''Control rfvNumPagare.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvNumPagare As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control txtSerialTarjeta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtSerialTarjeta As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control rfvSerialTarjeta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvSerialTarjeta As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control revSerialTarjeta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents revSerialTarjeta As Global.System.Web.UI.WebControls.RegularExpressionValidator

    '''<summary>
    '''Control txtObservacionOperadorCall.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtObservacionOperadorCall As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control chkDeclinarVenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkDeclinarVenta As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control trDeclinarVenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents trDeclinarVenta As Global.System.Web.UI.HtmlControls.HtmlTableRow

    '''<summary>
    '''Control txtObservacionesVentaDeclinada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtObservacionesVentaDeclinada As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control rfvObservacionVentaDeclinada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvObservacionVentaDeclinada As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control lbRegistrarVenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbRegistrarVenta As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control lbCancelarVenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbCancelarVenta As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Control loadingPnl.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents loadingPnl As Global.DevExpress.Web.ASPxLoadingPanel

    '''<summary>
    '''Control lblTransaccionExistente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTransaccionExistente As Global.System.Web.UI.WebControls.Label
End Class
