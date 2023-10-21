Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.Reportes

Public Class RegistrarTarjetaDestruido
    Inherits System.Web.UI.Page

    Public i As Integer

#Region "Atributos Privados"

    Dim idRol As Integer


#End Region

#Region "Métodos Privados"

    Private Sub CambiarVistaTabScript(ByVal proceso As String)
        Select Case proceso
            Case "LISTADO"
                pnlVentasDestruir.Visible = True
                tsInfoGestion.Items(0).Disabled = False
                tsInfoGestion.SelectedIndex = 0
        End Select
    End Sub

    Private Sub LimpiarYControlarVisualizacionDeCamposDeProducto(ByVal visible As Boolean)

    End Sub

    Private Sub InicializarCampos()
        For Each ctrl As Control In pnlVentasDestruir.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = ""
            ElseIf TypeOf ctrl Is DropDownList Then
                CType(ctrl, DropDownList).ClearSelection()
            End If
        Next

    End Sub

    Public Sub ObtenerDatosSerialesTemporales()
        Dim dtDatos As New DataTable

        Dim infoHistoria As New SerialesADestruir
        With infoHistoria
            .UsuarioDestruccion = CInt(Session("userId"))
            dtDatos = .DatosReporte
        End With

        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            EnlazarDatos(dtDatos)
        Else

        End If
    End Sub

    Private Sub EnlazarDatos(ByVal dtDatos As DataTable)
        With gvVentasDestruir
            .DataSource = dtDatos
            .DataBind()
        End With

    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Session("idRol") IsNot Nothing Then Integer.TryParse(Session("idRol").ToString, idRol)
        If Not Me.IsPostBack Then
            With epNotificador
                .setTitle("Registrar Tarjeta Destruído")
            End With
            If Len(Request.QueryString("Param")) > 0 Then
                txtNumeroSerial.Text = Request.QueryString("Param")
            End If
            i = 0
            pnlGestion.Visible = False
            pnlVentasDestruir.Visible = False
            tblMotivoDestruccion.Visible = False
        End If
    End Sub

    Protected Sub lbCancelarHistorico_Click(sender As Object, e As EventArgs) Handles lbCancelar.Click
        gvVentasDestruir.DataBind()
        txtObservacionDestruccion.Text = ""
        tblMotivoDestruccion.Visible = False
    End Sub

    Protected Sub lbDestruir_Click(sender As Object, e As EventArgs) Handles lbDestruir.Click
        Try
            If Len(txtObservacionDestruccion.Text.Trim) < 25 Then
                epNotificador.showWarning("La justificación para destruir debe contener al menos 25 caracteres.")
                Exit Sub
            End If
            Dim numeroacta As Integer
            'Se confirma la destrucción
            'Dim consecutivoacta As New ConsecutivoActaDestruccion(1)
            'With consecutivoacta
            'numeroacta = Val(.NumeroActaDestruccion) + 1
            'End With
            'Se recorre el gridview para obtener los números de venta y cambiar el estado
            Dim seriales As String = ""
            Dim cant As Integer
            For cant = 0 To gvVentasDestruir.Rows.Count - 1
                Dim index As Integer = Convert.ToInt32(cant)
                Dim row As GridViewRow = gvVentasDestruir.Rows(index)
                'actualizamos el numero de acta y el motivo de destrucción
                Dim registrarinfodestruccion As New RegistrarInfoDestruccion
                registrarinfodestruccion.MotivoDestruccion = txtObservacionDestruccion.Text.Trim
                registrarinfodestruccion.UsuarioDestruccion = Session("userId")
                registrarinfodestruccion.Actualizar()
                numeroacta = registrarinfodestruccion.IdActa
            Next
            Dim destruir As New ConfirmarDestruccionSeriales
            destruir.NumeroActaDestruccion = numeroacta
            destruir.UsuarioDestruccion = Session("userId")
            destruir.Actualizar()
            'creardocumento(numeroacta)
            gvVentasDestruir.DataBind()
            txtObservacionDestruccion.Text = ""
            pnlGestion.Visible = False
            pnlVentasDestruir.Visible = False
            tblMotivoDestruccion.Visible = False
            epNotificador.showSuccess("Seriales destruídos satisfactoriamente.")
            txtNumeroSerial.Focus()
            Dim script As String = "<script type='text/javascript'>window.open('Reportes/VisorXtraReport.aspx?id=" + numeroacta.ToString + "');</script>"
            Response.Write(script)


        Catch ex As Exception
            epNotificador.showError(ex.Message)
        End Try
    End Sub

    Protected Sub lbAgregar_Click(sender As Object, e As EventArgs) Handles lbAgregar.Click
        Try
            If Len(txtNumeroSerial.Text.Trim) = 14 Or Len(txtNumeroSerial.Text.Trim) = 16 Then
                Dim consultaserial As New ConsultarSeriales(txtNumeroSerial.Text.Trim)
                If Len(consultaserial.IdGestionVenta) >= 0 Then
                    'Se puede destruír
                    'Insertamos en la temporal
                    Dim insertartemporal As New InsertarTemporalDestruirSerial
                    With insertartemporal
                        .IdGestionVenta = consultaserial.IdGestionVenta
                        .serial = txtNumeroSerial.Text.Trim
                        .UsuarioDestruccion = CInt(Session("userId"))
                        .Registrar()
                    End With
                    pnlGestion.Visible = True
                    pnlVentasDestruir.Visible = True
                    'aquí mostramos en la grilla los seriales en temporal
                    ObtenerDatosSerialesTemporales()
                    txtNumeroSerial.Text = ""
                    txtNumeroSerial.Focus()
                Else
                    'No se puede destruír
                    epNotificador.showWarning("El serial digitado no existe o no se puede destruir.")
                    txtNumeroSerial.Text = ""
                    txtNumeroSerial.Focus()
                End If
            Else
                'No se puede destruír
                epNotificador.showWarning("El serial digitado no existe o no se puede destruir.")
                txtNumeroSerial.Text = ""
                txtNumeroSerial.Focus()
            End If
        Catch ex As Exception
            epNotificador.showError("Error al agregar un serial.")
        End Try
    End Sub

    Protected Sub lbContinuar_Click(sender As Object, e As EventArgs) Handles lbContinuar.Click
        tblMotivoDestruccion.Visible = True
        txtObservacionDestruccion.Focus()
    End Sub
End Class