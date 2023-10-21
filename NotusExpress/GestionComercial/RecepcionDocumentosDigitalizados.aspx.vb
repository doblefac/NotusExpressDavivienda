Imports DevExpress.Web
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports System.IO

Public Class RecepcionDocumentosDigitalizados
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Recepción Documentos Digitalizados")
            cargarDocumentos()
            If Session("AbreMenu") = "0" Then
                'Me.btnContinuar.Visible = False
                Me.txtNumIdentificacion.ReadOnly = True
                txtNumIdentificacion.Text = "123"
            Else
                'Me.uplDocumentos.Enabled = True
                Me.btnRegistrar.Enabled = False
            End If
            'FileUpload1.Enabled = False
            'btnCargar.Enabled = False
        End If
    End Sub

    Private Sub cargarDocumentos()
        Try
            Dim dtDatos As DataTable
            Dim objDocumentos As New ObtenerListadoDocumentosRadicar
            With objDocumentos
                .IdEstado = 1
                .CargarDatos()
                dtDatos = .DatosRegistros
            End With

            With cbDocumentos
                .DataSource = dtDatos
                .TextField = "descripcion"
                .ValueField = "idDocumento"
                .DataBindItems()
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRegistrar_Click(sender As Object, e As System.EventArgs) Handles btnRegistrar.Click

        If Session("AbreMenu") = "0" Then
            'Me.btnContinuar.Visible = True
            Session("AbreMenu") = "1"
        End If

        txtNumIdentificacion.Text = ""
        'btnContinuar.Enabled = True
        FileUpload1.Enabled = False
        btnRegistrar.Enabled = False
        txtNumIdentificacion.ReadOnly = False
        epNotificador.showSuccess("Documentos cargados satisfactoriamente")
        Session("AbreMenu") = "1"
        Session("listaDoc") = Nothing
    End Sub

    'Private Sub btnContinuar_Click(sender As Object, e As System.EventArgs) Handles btnContinuar.Click
    '    'Me.btnContinuar.Enabled = False
    '    Me.txtNumIdentificacion.ReadOnly = True
    '    Me.btnRegistrar.Enabled = True
    '    Me.uplDocumentos.Enabled = True
    'End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            CType(sender, ASPxCallbackPanel).JSProperties.Remove("cpLimpiarFiltros")
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(",")
            Select Case arrayAccion(0)
                'Case "Consultar"
                '    txtSerial.Text = "123456789"
                '    txtNombres.Text = "Nombres"
                '    txtPrimerApellido.Text = "Primer Apellido"
                '    txtSegundoApellido.Text = "Segundo Apellido"
                '    txtDireccionPoliza.Text = "Direccion"
                '    deFechaNacimiento.Date = "01/01/1985"
                '    cbSexo.SelectedIndex = 1
                '    tblInfoUsuario.Visible = True
                '    btnRegistrar.Visible = True
                '    btnCancelar.Visible = True
                '    btnConsultar.Enabled = False
                'Case "registrarDatos"
                '    'Dim mensaje As String
                '    'mensaje = RegistrarDatos()
                '    'Select Case mensaje
                '    'Case "Registro Exitoso"
                '    'epNotificador.showSuccess("Registro Exitoso")
                '    limpiarControles()
                '    tblInfoUsuario.Visible = False
                '    btnRegistrar.Visible = False
                '    btnCancelar.Visible = False
                '    btnConsultar.Enabled = True
                '    CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
                '    'Case Else
                '    '    epNotificador.showWarning(mensaje)
                '    '    CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
                '    '    Exit Sub
                'Case "Cancelar"
                '    limpiarControles()
                '    tblInfoUsuario.Visible = False
                '    btnRegistrar.Visible = False
                '    btnCancelar.Visible = False
                '    btnConsultar.Enabled = True
                'Case "Novedad"
                '    If rbNovedad.Value = 1 Then
                '        trNovedad.Visible = True
                '    Else
                '        trNovedad.Visible = False
                '    End If
            End Select

        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub btnCargar_Click(sender As Object, e As System.EventArgs) Handles btnCargar.Click
        'Dim path As String = Server.MapPath("~/UploadedImages/")
        Dim infoRuta As New RutaArchivoPoliza
        infoRuta.CargarDatos()
        Dim ruta As String
        ruta = infoRuta.RutaArchivoDocumentos
        Dim carpetaPrincipal As String
        carpetaPrincipal = "BancaSeguros"
        Dim carpetaSecundaria As String
        carpetaSecundaria = "DocumentosDigitalizados"
        Dim carpetaMes As String = ""
        Dim mes As String = ""
        Select Case DateTime.Now.Month
            Case 1
                carpetaMes = "Enero"
                mes = "01"
            Case 2
                carpetaMes = "Febrero"
                mes = "02"
            Case 3
                carpetaMes = "Marzo"
                mes = "03"
            Case 4
                carpetaMes = "Abril"
                mes = "04"
            Case 5
                carpetaMes = "Mayo"
                mes = "05"
            Case 6
                carpetaMes = "Junio"
                mes = "06"
            Case 7
                carpetaMes = "Julio"
                mes = "07"
            Case 8
                carpetaMes = "Agosto"
                mes = "08"
            Case 9
                carpetaMes = "Septiembre"
                mes = "09"
            Case 10
                carpetaMes = "Octubre"
                mes = "10"
            Case 11
                carpetaMes = "Noviembre"
                mes = "11"
            Case 12
                carpetaMes = "Diciembre"
                mes = "12"
        End Select
        'Verifica carpeta principal
        Dim rutaCompleta As String
        If Not Directory.Exists(ruta & carpetaPrincipal) Then
            Directory.CreateDirectory(ruta & carpetaPrincipal)
        End If
        'Verifica carpeta secundaria
        If Not Directory.Exists(ruta & carpetaPrincipal & "\" & carpetaSecundaria) Then
            Directory.CreateDirectory(ruta & carpetaPrincipal & "\" & carpetaSecundaria)
        End If
        rutaCompleta = ruta & carpetaPrincipal & "\" & carpetaSecundaria
        'Verifica subcarpeta mes
        If Not Directory.Exists(ruta & carpetaPrincipal & "\" & carpetaSecundaria & "\" & DateTime.Now.Date.Day & mes & Right(DateTime.Now.Date.Year, 2)) Then
            Directory.CreateDirectory(ruta & carpetaPrincipal & "\" & carpetaSecundaria & "\" & DateTime.Now.Date.Day & mes & Right(DateTime.Now.Date.Year, 2))
        End If
        rutaCompleta = ruta & carpetaPrincipal & "\" & carpetaSecundaria & "\" & DateTime.Now.Date.Day & mes & Right(DateTime.Now.Date.Year, 2) & "\"
        Dim path As String = rutaCompleta
        Dim fileOK As Boolean = False
        If FileUpload1.HasFile Then
            Dim fileExtension As String
            fileExtension = System.IO.Path. _
                GetExtension(FileUpload1.FileName).ToLower()
            Dim allowedExtensions As String() = _
                {".jpg", ".jpeg", ".png", ".gif"}
            For i As Integer = 0 To allowedExtensions.Length - 1
                If fileExtension = allowedExtensions(i) Then
                    fileOK = True
                End If
            Next
            If fileOK Then
                Try
                    FileUpload1.PostedFile.SaveAs(path & _
                         FileUpload1.FileName)
                    btnRegistrar.Enabled = True
                    Dim regDoc As New RegistrarDocumentoDigitalizado
                    With regDoc
                        .NumeroIdentificacion = txtNumIdentificacion.Text.Trim
                        .Estado = 1
                        .IdUsuarioRegistra = CInt(Session("userId"))
                        .Ruta = path & _
                         FileUpload1.FileName
                        .IdDocumento = cbDocumentos.Value
                        .Registrar()
                        CargarListadoDeDocumentosDigitalizados()
                        epNotificador.clear()
                        cbDocumentos.SelectedIndex = -1
                    End With
                    'Label1.Text = "File uploaded!"
                Catch ex As Exception
                    'Label1.Text = "File could not be uploaded."
                End Try
            Else
                'Label1.Text = "Cannot accept files of this type."
            End If
        Else
            epNotificador.showWarning("Por favor seleccione un archivo.")
        End If
    End Sub

    Protected Sub ManejadorEliminar(sender As Object, e As EventArgs)
        Try
            Dim index As Integer = CType(CType(sender, LinkButton).NamingContainer, GridViewDataRowTemplateContainer).VisibleIndex
            Dim idDoc As Integer = gvDocumentos.GetRowValues(index, "idDoc")
            Dim ruta As String = gvDocumentos.GetRowValues(index, "ruta")
            Dim eliminarDoc As New RegistrarDocumentoDigitalizado
            With eliminarDoc
                .IdDoc = idDoc
                .Eliminar()
            End With
            
            My.Computer.FileSystem.DeleteFile(ruta, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)
            CargarListadoDeDocumentosDigitalizados(True)

        Catch ex As Exception
            'mensajero.MostrarErrorYNotificarViaMail("Error al tratar de descargar archivo. Por favor contacte a IT", "Reporte de Archivos Generados de Ventas de Seguros", ex)
        End Try
    End Sub

    Private Sub CargarListadoDeDocumentosDigitalizados(Optional ByVal forzarConsulta As Boolean = False)
        Dim listaDoc As RegistrarDocumentoDigitalizado = Nothing

        Try
            listaDoc = New RegistrarDocumentoDigitalizado
            Dim dtDatos As DataTable
            With listaDoc
                If Not String.IsNullOrEmpty(txtNumIdentificacion.Text.Trim) Then .NumeroIdentificacion = txtNumIdentificacion.Text.Trim
                .CargarDatos()
                dtDatos = New DataTable
                dtDatos = .DatosRegistros
            End With
            If dtDatos.Rows.Count = 0 Then
                btnRegistrar.Enabled = False
            End If

            With gvDocumentos
                .DataSource = dtDatos
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Novedades.")
        End Try
    End Sub

    Private Sub cbDocumentos_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cbDocumentos.SelectedIndexChanged
        FileUpload1.Enabled = True
        btnCargar.Enabled = True
    End Sub

    Private Sub cbDocumentos_TextChanged(sender As Object, e As System.EventArgs) Handles cbDocumentos.TextChanged
        FileUpload1.Enabled = True
        btnCargar.Enabled = True
    End Sub

    Private Sub cbDocumentos_ValueChanged(sender As Object, e As System.EventArgs) Handles cbDocumentos.ValueChanged
        FileUpload1.Enabled = True
        btnCargar.Enabled = True
    End Sub

    Private Sub FileUpload1_DataBinding(sender As Object, e As System.EventArgs) Handles FileUpload1.DataBinding
        epNotificador.clear()
    End Sub
End Class