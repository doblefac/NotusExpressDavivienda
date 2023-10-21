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


Partial Public Class GestionVentaYServicio
    
    '''<summary>
    '''Control formPrincipal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents formPrincipal As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Control divPrincipal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents divPrincipal As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control hfIdCampania.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfIdCampania As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control epNotificador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents epNotificador As Global.NotusExpress.EncabezadoPagina
    
    '''<summary>
    '''Control cpGeneral.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cpGeneral As Global.DevExpress.Web.ASPxCallbackPanel
    
    '''<summary>
    '''Control rpFiltros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rpFiltros As Global.DevExpress.Web.ASPxRoundPanel
    
    '''<summary>
    '''Control lblCamposObligatorios.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCamposObligatorios As Global.DevExpress.Web.ASPxLabel
    
    '''<summary>
    '''Control imgAgregarCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgAgregarCliente As Global.DevExpress.Web.ASPxButton
    
    '''<summary>
    '''Control cbTipoId.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbTipoId As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control txtNumIdentificacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumIdentificacion As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control cmbCampania.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbCampania As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control txtNombres.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNombres As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control txtPrimerApellido.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtPrimerApellido As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control txtSegundoApellido.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtSegundoApellido As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control txtTelFijo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtTelFijo As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control cmbCiudad1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbCiudad1 As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control lblComentario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblComentario As Global.DevExpress.Web.ASPxLabel
    
    '''<summary>
    '''Control txtDireccionResidencia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtDireccionResidencia As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control cbModificarDireccion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbModificarDireccion As Global.DevExpress.Web.ASPxCheckBox
    
    '''<summary>
    '''Control txtCelular.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtCelular As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control txtOficinaCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtOficinaCliente As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control ASPxLabel1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ASPxLabel1 As Global.DevExpress.Web.ASPxLabel
    
    '''<summary>
    '''Control txtEmail.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtEmail As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control txtIngresos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtIngresos As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control cbEstadoId.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbEstadoId As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control cbActividadLaboral.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbActividadLaboral As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control cmbResultadoProceso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbResultadoProceso As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control cmbCausal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbCausal As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control txtObservacionOperadorCall.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtObservacionOperadorCall As Global.DevExpress.Web.ASPxMemo
    
    '''<summary>
    '''Control btnAgregar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAgregar As Global.DevExpress.Web.ASPxButton
    
    '''<summary>
    '''Control btnRegistra.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnRegistra As Global.DevExpress.Web.ASPxButton
    
    '''<summary>
    '''Control btnCancelar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCancelar As Global.DevExpress.Web.ASPxButton
    
    '''<summary>
    '''Control btnCancelarPopUp.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCancelarPopUp As Global.DevExpress.Web.ASPxButton
    
    '''<summary>
    '''Control gvServiciosAux.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvServiciosAux As Global.DevExpress.Web.ASPxGridView
    
    '''<summary>
    '''Control RpFechaAgendamiento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RpFechaAgendamiento As Global.DevExpress.Web.ASPxRoundPanel
    
    '''<summary>
    '''Control calFechaInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents calFechaInicio As Global.DevExpress.Web.ASPxDateEdit
    
    '''<summary>
    '''Control gvJornadas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvJornadas As Global.DevExpress.Web.ASPxGridView
    
    '''<summary>
    '''Control lblCapacidadEntrega.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCapacidadEntrega As Global.DevExpress.Web.ASPxLabel
    
    '''<summary>
    '''Control cbJornada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbJornada As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control gvDetalle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvDetalle As Global.DevExpress.Web.ASPxGridView
    
    '''<summary>
    '''Control rpInfoDevolucionCallCenter.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rpInfoDevolucionCallCenter As Global.DevExpress.Web.ASPxRoundPanel
    
    '''<summary>
    '''Control gvInfoDevolucionCallCenter.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvInfoDevolucionCallCenter As Global.DevExpress.Web.ASPxGridView
    
    '''<summary>
    '''Control mensajero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents mensajero As Global.NotusExpress.MensajePopUp
    
    '''<summary>
    '''Control pcServicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pcServicio As Global.DevExpress.Web.ASPxPopupControl
    
    '''<summary>
    '''Control pccServicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pccServicio As Global.DevExpress.Web.PopupControlContentControl
    
    '''<summary>
    '''Control cmbTipoServicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbTipoServicio As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control cmbProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbProducto As Global.DevExpress.Web.ASPxComboBox
    
    '''<summary>
    '''Control memoObservacionServicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents memoObservacionServicio As Global.DevExpress.Web.ASPxMemo
    
    '''<summary>
    '''Control txtCupo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtCupo As Global.DevExpress.Web.ASPxTextBox
    
    '''<summary>
    '''Control imgAgregar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgAgregar As Global.DevExpress.Web.ASPxImage
    
    '''<summary>
    '''Control rpServicios.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rpServicios As Global.DevExpress.Web.ASPxRoundPanel
    
    '''<summary>
    '''Control gvServicios.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvServicios As Global.DevExpress.Web.ASPxGridView
    
    '''<summary>
    '''Control pcVer.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pcVer As Global.DevExpress.Web.ASPxPopupControl
    
    '''<summary>
    '''Control PopupControlContentControl1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents PopupControlContentControl1 As Global.DevExpress.Web.PopupControlContentControl
    
    '''<summary>
    '''Control cbCallBackId.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbCallBackId As Global.DevExpress.Web.ASPxCallbackPanel
    
    '''<summary>
    '''Control loadingPanel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents loadingPanel As Global.DevExpress.Web.ASPxLoadingPanel
End Class
