Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.ConfiguracionComercial
Imports DevExpress.Web
Imports System.IO
Imports GemBox
Imports GemBox.Spreadsheet

Public Class AdministrarMetasComerciales
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private oExcel As ExcelFile
    Private Shared listaEstrategia As EstrategiaComercialColeccion
    Private Shared listaPdv As PuntoDeVentaColeccion

    Private Const UploadDirectory As String = "~\ArchivosCargados\"
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Administración de Metas Comerciales")
            Session.Remove("listaMetas")
            Session.Remove("tablaErrores")
            CargarLicenciaGembox()
            CargarListadoDePDVs(cbFiltroPdv)
            CargarListadoEstrategias(cbFiltroEstrategia)
            CargarListadoProductosFiltro()
            CargarListadoEstrategias(cbEstrategia)
            CargarListadoDeMeses()
            CargarListadoTiposDeProducto()
        End If
        If cpGeneral.IsCallback Then
            CargarListadoDePDVs(cbPdv)
            CargarListadoDeMeses()
            CargarListadoTiposDeProducto()
        End If

        If Not Me.IsPostBack OrElse gvListaMetas.IsCallback Then CargarListadoDeMetasComercialesRegistradas()
        If gvError.IsCallback Then _
            If Session("tablaErrores") IsNot Nothing Then EnlazarErrores(CType(Session("tablaErrores"), DataTable))

    End Sub

    Private Sub CargarListadoProductosFiltro()
        Try
            Dim lista As New ListadoTipoProductoRegistroColeccion
            lista.CargarDatos()
            With cbFiltroProducto
                .DataSource = lista
                .TextField = "nombre"
                .ValueField = "idTipoProducto"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de productos")
        Finally
            'cbFiltroCanal.Items.Insert(0, New DevExpress.Web.ListEditItem("Seleccione un Canal", "0"))
        End Try
    End Sub

    Private Sub CargarListadoEstrategias(ByRef combo As ASPxComboBox)
        Try
            If listaEstrategia Is Nothing Then
                listaEstrategia = New EstrategiaComercialColeccion
                With listaEstrategia
                    .IdEstado = Enumerados.EstadoBinario.Activo
                    .CargarDatos()
                End With
            End If
            With combo
                .DataSource = listaEstrategia
                .TextField = "Nombre"
                .ValueField = "IdEstrategia"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de estrategias")
        Finally
            combo.Items.Insert(0, New DevExpress.Web.ListEditItem("Seleccione...", Nothing))
        End Try
    End Sub

    Private Sub CargarListadoDePDVs(ByRef combo As ASPxComboBox, Optional ByVal forzarConsulta As Boolean = False)
        Try
            If listaPdv Is Nothing OrElse forzarConsulta Then
                listaPdv = New PuntoDeVentaColeccion
                With listaPdv
                    .IdUnidadNegocio = CInt(Session("idUnidadNegocio"))
                    If combo.ID = "cbPdv" AndAlso cbEstrategia.Value IsNot Nothing Then
                        .IdEstrategia = CInt(cbEstrategia.Value)
                    Else
                        .IdEstrategia = 0
                    End If
                    .IdEstado = 1
                    .CargarDatos()
                End With
            End If

            With combo
                .DataSource = listaPdv
                .TextField = "NombrePdv"
                .ValueField = "IdPdv"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de puntos de venta")
        Finally
            combo.Items.Insert(0, New DevExpress.Web.ListEditItem("Seleccione...", Nothing))
        End Try
    End Sub

    Private Sub CargarListadoDeMeses()
        Try
            Dim dt As New DataTable

            With dt.Columns
                .Add("IdMes", GetType(Integer))
                .Add("Mes", GetType(String))
            End With
            With dt.Rows
                .Add("1", "Enero")
                .Add("2", "Febrero")
                .Add("3", "Marzo")
                .Add("4", "Abril")
                .Add("5", "Mayo")
                .Add("6", "Junio")
                .Add("7", "Julio")
                .Add("8", "Agosto")
                .Add("9", "Septiembre")
                .Add("10", "Octubre")
                .Add("11", "Noviembre")
                .Add("12", "Diciembre")
            End With

            With cbMes
                .DataSource = dt
                .TextField = "Mes"
                .ValueField = "IdMes"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de canales")
        Finally
            'cbFiltroCanal.Items.Insert(0, New DevExpress.Web.ListEditItem("Seleccione un Canal", "0"))
        End Try
    End Sub

    Private Sub CargarListadoTiposDeProducto()
        Try
            Dim lista As New MaestroProductos.TipoProductoColeccion
            With lista
                .IdEstado = Enumerados.EstadoBinario.Activo
                .CargarDatos()
            End With

            With cbTipoProducto
                .DataSource = lista
                .TextField = "Nombre"
                .ValueField = "IdTipoProducto"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de productos")
        Finally
            cbTipoProducto.Items.Insert(0, New DevExpress.Web.ListEditItem("Seleccione...", Nothing))
        End Try
    End Sub

    Private Sub CargarListadoDeMetasComercialesRegistradas(Optional ByVal forzarConsulta As Boolean = False)
        Dim listaMetas As ConfiguracionComercial.MetaComercialColeccion = Nothing

        Try
            If Session("listaMetas") Is Nothing OrElse forzarConsulta Then
                listaMetas = New ConfiguracionComercial.MetaComercialColeccion
                With listaMetas
                    If cbFiltroPdv.Value IsNot Nothing Then .IdPdv = cbFiltroPdv.Value
                    If cbFiltroEstrategia.Value IsNot Nothing Then .IdEstrategia = cbFiltroEstrategia.Value
                    If cbFiltroProducto.Value IsNot Nothing Then .IdTipoProducto = cbFiltroProducto.Value
                    .CargarDatos()
                End With
                Session("listaMetas") = listaMetas
            Else
                listaMetas = CType(Session("listaMetas"), ConfiguracionComercial.MetaComercialColeccion)
            End If

            With gvListaMetas
                .DataSource = listaMetas
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Metas Comerciales.")
        End Try
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            Select Case e.Parameter
                Case "filtrarDatos"
                    CargarListadoDeMetasComercialesRegistradas(True)
                Case "limpiarFiltros"
                    cbFiltroPdv.SelectedIndex = -1
                    cbFiltroEstrategia.SelectedIndex = -1
                    cbFiltroProducto.SelectedIndex = -1
                    CargarListadoDeMetasComercialesRegistradas(True)
                    CType(sender, ASPxCallbackPanel).JSProperties("cpLimpiar") = True
                Case "registrarDatos"
                    RegistrarNuevaMeta()
                Case "registrarDatosDeArchivo"
                    RegistrarDatosDeArchivo()
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        CargarListadoDePDVs(cbFiltroPdv)
        CargarListadoEstrategias(cbFiltroEstrategia)
        CargarListadoProductosFiltro()
        CargarListadoEstrategias(cbEstrategia)
        CargarListadoDeMeses()
        CargarListadoTiposDeProducto()
        rpFiltros.Enabled = Not pucNuevaMeta.ShowOnPageLoad
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub CargarArchivoExcel()
        '    uplArchivo.SaveAs(uplArchivo.FileName)
        RegistrarDatosDeArchivo()
    End Sub

    Private Sub RegistrarNuevaMeta()
        Try
            Dim obj As New MetaComercial
            Dim resultado As General.ResultadoProceso
            pucNuevaMeta.ShowOnPageLoad = True
            With obj
                .Meta = CInt(txtNombreMeta.Text.Trim)
                .Anio = CInt(txtAnio.Text.Trim)
                .Mes = CInt(cbMes.Value)
                .IdEstrategia = CInt(cbEstrategia.Value)
                .IdPdv = CInt(cbPdv.Value)
                .IdTipoProducto = CInt(cbTipoProducto.Value)
                .IdUsuarioRegistra = CInt(Session("userId"))
                resultado = .Registrar()
                Select Case resultado.Valor
                    Case 0
                        mensajero.MostrarMensajePopUp("La meta fue registrada satisfactoriamente.", MensajePopUp.TipoMensaje.ProcesoExitoso)
                        CargarListadoDeMetasComercialesRegistradas(True)
                    Case Else
                        Select Case resultado.Valor
                            Case 1, 200, 300
                                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.Alerta)
                            Case Else
                                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico)
                        End Select
                End Select
            End With
        Catch ex As Exception
            mensajero.MostrarErrorYNotificarViaMail("Error al tratar de registrar nueva meta comercial.", "Administración de Metas Comerciales", ex)
        End Try
    End Sub

    Private Sub gvListaMetas_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gvListaMetas.RowUpdating
        Try
            mensajero.Limpiar()
            CType(sender, ASPxGridView).JSProperties.Clear()
            Dim obj As New ConfiguracionComercial.MetaComercial(CInt(e.Keys("IdMeta")))
            With obj
                Dim resultado As General.ResultadoProceso
                If obj.Registrado Then
                    If ObtenerTexto(e.OldValues("Meta")) <> ObtenerTexto(e.NewValues("Meta")) Then
                        .Meta = ObtenerTexto(e.NewValues("Meta"))
                        .IdMeta = e.Keys("IdMeta")
                        resultado = .Actualizar()
                        Select Case resultado.Valor
                            Case 0
                                mensajero.MostrarMensajePopUp("La Meta fue actualizada satisfactoriamente.", MensajePopUp.TipoMensaje.ProcesoExitoso)
                                CargarListadoDeMetasComercialesRegistradas(True)
                            Case 200, 300
                                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.Alerta)
                            Case Else
                                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico)
                        End Select
                    Else
                        mensajero.MostrarMensajePopUp("No se realiza la actualización, puesto que no se cambiaron datos", MensajePopUp.TipoMensaje.Alerta)
                    End If
                Else
                    mensajero.MostrarMensajePopUp("No existe un registro en la base de datos con el identificador seleccionado. Por favor intente nuevamente", MensajePopUp.TipoMensaje.Alerta)
                End If
            End With
            gvListaMetas.CancelEdit()
        Catch ex As Exception
            mensajero.MostrarErrorYNotificarViaMail("Error al tratar de actualizar datos. ", "Administración de Metas Comerciales", ex)
        Finally
            If mensajero.Mensaje.Length > 0 Then
                CType(sender, ASPxGridView).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje()
                CType(sender, ASPxGridView).JSProperties("cpTituloPopUp") = mensajero.Titulo
            End If
            e.Cancel = True
        End Try
    End Sub

    'Protected Sub gvDetalle_DataSelect(ByVal sender As Object, ByVal e As EventArgs)
    '    Try
    '        Dim idMeta As Long = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()

    '        Dim objDetalleMeta As New MetaComercialColeccion()
    '        With objDetalleMeta
    '            .IdMeta = idMeta
    '            .CargarDatos()
    '            TryCast(sender, ASPxGridView).DataSource = objDetalleMeta.GenerarDataTable()
    '        End With
    '    Catch ex As Exception
    '        mensajero.MostrarErrorYNotificarViaMail("Error al tratar de gargar detalle del registro. ", "Administración de Metas Comerciales", ex)
    '    Finally
    '        CType(sender, ASPxGridView).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    '    End Try
    'End Sub

    'Protected Sub cbEstrategia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbEstrategia.SelectedIndexChanged
    '    Try
    '        Dim lista As New ListadoPdvColeccion
    '        lista.IdEstrategia = CInt(cbEstrategia.Value)
    '        lista.CargarDatos()
    '        With cbPdv
    '            .DataSource = lista
    '            .TextField = "nombre"
    '            .ValueField = "idPdv"
    '            .DataBind()
    '        End With
    '    Catch ex As Exception
    '        epNotificador.showError("Error al tratar de cargar el listado de puntos de venta")
    '    Finally
    '        'cbFiltroCanal.Items.Insert(0, New DevExpress.Web.ListEditItem("Seleccione un Canal", "0"))
    '    End Try
    'End Sub

    Private Sub cbPdv_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cbPdv.Callback
        Try
            CargarListadoDePDVs(cbPdv)
        Catch ex As Exception
            mensajero.MostrarErrorYNotificarViaMail("Error al tratar de cargar el listado de puntos de venta.", "Administración de Metas Comerciales", ex)
        Finally
            CType(sender, ASPxComboBox).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        End Try
    End Sub

    'Protected Sub cbPdv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbPdv.SelectedIndexChanged
    '    CargarListadoProductos()
    '    CargarMesesRegistro()
    'End Sub

    Private Sub uplArchivo_FileUploadComplete(sender As Object, e As DevExpress.Web.FileUploadCompleteEventArgs) Handles uplArchivo.FileUploadComplete
        Session.Remove("nombreArchivoCargado")
        If Not e.UploadedFile.IsValid Then e.CallbackData = String.Empty
        Dim nombreArchivo As String = Server.MapPath(UploadDirectory & "CagueDeMetasComerciales_" & Session("userId") & Path.GetExtension(e.UploadedFile.FileName))
        Try
            uplArchivo.UploadedFiles(0).SaveAs(nombreArchivo)
            Session("nombreArchivoCargado") = nombreArchivo
            e.CallbackData = e.UploadedFile.FileName
        Catch ex As Exception
            e.ErrorText = ex.Message
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de cargar archivo al servidor. ", "Cargar Iventario Mediante Archivo", ex)
        End Try

        'Try
        '    If Not e.UploadedFile.IsValid Then e.CallbackData = String.Empty
        '    If uplArchivo.UploadedFiles IsNot Nothing AndAlso uplArchivo.UploadedFiles.Count > 0 Then
        '        Dim fileExtension As String = Path.GetExtension(e.UploadedFile.FileName)
        '        oExcel = New ExcelFile()
        '        If fileExtension.ToUpper = ".XLS" Then
        '            oExcel.LoadXls(New MemoryStream(uplArchivo.UploadedFiles(0).FileBytes))
        '        ElseIf fileExtension.ToUpper = ".XLSX" Then
        '            oExcel.LoadXlsx(New MemoryStream(uplArchivo.UploadedFiles(0).FileBytes), XlsxOptions.None)
        '        End If

        '        Dim validacionArchivo As ResultadoProceso = ProcesarArchivo()
        '        If validacionArchivo.Valor = 0 Then
        '            mensajero.MostrarMensajePopUp("La información de Metas Comerciales fue cargada satisfactoriamente.", MensajePopUp.TipoMensaje.ProcesoExitoso)
        '            CargarListadoDeMetasComercialesRegistradas(True)
        '        ElseIf validacionArchivo.Valor = 10 Or validacionArchivo.Valor = 20 Or validacionArchivo.Valor = 30 Then
        '            mensajero.MostrarMensajePopUp(validacionArchivo.Mensaje, MensajePopUp.TipoMensaje.Alerta)
        '        Else
        '            mensajero.MostrarMensajePopUp("Se presentaron errores al tratar de cargar archivo. Por favor verifique el archivo.")
        '        End If
        '        CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = validacionArchivo.Valor
        '    End If
        '    e.CallbackData = e.UploadedFile.FileName
        'Catch ex As Exception
        '    mensajero.MostrarErrorYNotificarViaMail("Se generó un error al intentar procesar el archivo. Por favor intente nuevamente", "Administración de Metas Comerciales", ex)
        'End Try
        CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub RegistrarDatosDeArchivo()
        Try
            cpGeneral.JSProperties.Remove("cpLimpiarArchivo")
            Session.Remove("tablaErrores")
            If Session("nombreArchivoCargado") IsNot Nothing Then
                Dim cargador As New CargadorArchivoMetasComerciales(Session("nombreArchivoCargado").ToString)
                Dim resultado As ResultadoProceso
                resultado = cargador.ProcesarYRegistrarDatos()
                If Not cargador.HayErrores Then
                    mensajero.MostrarMensajePopUp("El archivo fue procesado correctamente.", MensajePopUp.TipoMensaje.ProcesoExitoso)
                    CargarListadoDeMetasComercialesRegistradas(True)
                Else
                    EnlazarErrores(cargador.TablaErrores)
                    mensajero.MostrarMensajePopUp("Se presentaron errores al procesar el archivo. " & resultado.Mensaje, MensajePopUp.TipoMensaje.Alerta)
                End If
            Else
                mensajero.MostrarMensajePopUp("No fue posible recuperar el nombre del archivo. Por favor intente realizar el cargue nuevamente", MensajePopUp.TipoMensaje.Alerta)
            End If
        Catch ex As Exception
            mensajero.MostrarErrorYNotificarViaMail("Error al tratar de procesar archivo. Por favor notifique el error a IT", "Cargar Iventario Mediante Archivo", ex)
        Finally
            cpGeneral.JSProperties("cpLimpiarArchivo") = True
        End Try
        'Dim respuesta As New ResultadoProceso
        'Try
        '    objInstruccion = New CargueMeta(oExcel)
        '    If objInstruccion.ValidarEstructura() Then
        '        If objInstruccion.ValidarInformacion() Then
        '            respuesta.EstablecerMensajeYValor(0, "Archivo Cargado Satisfactoriamente")
        '            respuesta = objInstruccion.CrearInstruccion()
        '            If respuesta.Valor = 0 Then
        '                'Guardar Archivo
        '                respuesta.EstablecerMensajeYValor(0, "Archivo Cargado Satisfactoriamente")
        '            End If
        '        Else
        '            respuesta.EstablecerMensajeYValor(2, "Datos Inválidos")
        '        End If
        '    Else
        '        respuesta.EstablecerMensajeYValor(1, "Estructura inválida")
        '    End If
        '    Session("objInstruccion") = objInstruccion
        'Catch ex As Exception
        '    Throw ex
        'End Try
        'Return respuesta
    End Sub

    Private Sub EnlazarErrores(dtError As DataTable)
        If dtError IsNot Nothing AndAlso dtError.Rows.Count > 0 Then
            pucErrores.Width = Unit.Pixel(CInt(hfDimensiones("ancho")) * 0.90000000000000002)
            pucErrores.Height = Unit.Pixel(CInt(hfDimensiones("alto")) * 0.90000000000000002)
            Dim dvError As DataView = dtError.DefaultView
            dvError.Sort = "linea ASC"
            pucErrores.ShowOnPageLoad = True
            With gvError
                .DataSource = dvError
                .DataBind()
            End With
            Session("tablaErrores") = dtError
        Else
            pucErrores.ShowOnPageLoad = False
        End If
    End Sub

    Protected Sub btnVerEjemplo_Click(sender As Object, e As EventArgs) Handles btnVerEjemplo.Click
        Try
            Dim rutaArchivo As String = Server.MapPath("~/Reportes/Plantillas/PlantillaMetasComerciales.xlsx")
            If File.Exists(rutaArchivo) Then
                ForzarDescargaDeArchivo(rutaArchivo, Path.GetFileName(rutaArchivo))
            Else
                epNotificador.showWarning("El archivo que está intentando descargar no existe.")
            End If
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de descargar archivo de ejemplo. ", "Cargar Metas Comerciales", ex)
        End Try
    End Sub

    Protected Sub btnPlantilla_Click(sender As Object, e As EventArgs) Handles btnPlantilla.Click
        Try
            CargadorArchivoMetasComerciales.GenerarYDescargarArchivoDeDatosGuia()
        Catch ex As Exception
            epNotificador.MostrarErrorYNotificarViaMail("Error al tratar de descargar datos guia. ", "Cargar Metas Comerciales", ex)
        End Try
    End Sub

End Class