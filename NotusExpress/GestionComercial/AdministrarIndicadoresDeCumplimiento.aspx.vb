Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.ConfiguracionComercial
Imports DevExpress.Web

Public Class AdministrarIndicadoresDeCumplimiento
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private _indicadores As IndicadorCumplimientoMetaColeccion

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Administración de Indicadores de Cumplimiento")
            EnlazarIndicadores()
            CargarImagenes()
        End If
        If gvIndicadores.IsCallback Then EnlazarIndicadores()
    End Sub

    Private Sub CargarIndicadores()
        If Session("indicadoresCumplimiento") Is Nothing Then
            _indicadores = New IndicadorCumplimientoMetaColeccion
            _indicadores.CargarDatos()
        Else
            _indicadores = CType(Session("indicadoresCumplimiento"), IndicadorCumplimientoMetaColeccion)
        End If
    End Sub

    Private Sub EnlazarIndicadores()
        Try
            If _indicadores Is Nothing OrElse _indicadores.Count = 0 Then CargarIndicadores()
            With gvIndicadores
                .DataSource = _indicadores
                .DataBind()
            End With
        Catch ex As Exception
            mensajero.MostrarErrorYNotificarViaMail("Error al tratar de enlazar indicadores. Por favor recargue la página. ", "Administración de Indicadores de Cumplimiento", ex)
        End Try

    End Sub

    Private Sub CargarImagenes()
        Try
            cbImagen.Items.Clear()
            Dim arrImagen As New ArrayList("~/img/BallRed.png,~/img/BallOrange.png,~/img/BallGreen.png".Split(","))
            Dim dtImagenUtilizada As DataTable
            If _indicadores Is Nothing OrElse _indicadores.Count = 0 Then CargarIndicadores()
            dtImagenUtilizada = _indicadores.GenerarDataTable()
            Dim pcKey() As DataColumn = {dtImagenUtilizada.Columns("RutaImagen")}
            dtImagenUtilizada.PrimaryKey = pcKey
            For Each img As String In arrImagen
                If dtImagenUtilizada.Rows.Find(img) Is Nothing Then cbImagen.Items.Add(New ListEditItem("", img))
            Next
            cbImagen.SelectedIndex = -1
            btnAdicionar.Enabled = CBool(cbImagen.Items.Count)
        Catch ex As Exception
            mensajero.MostrarErrorYNotificarViaMail("Error al tratar de cargar imágenes. Por favor recargue la página. ", "Administración de Indicadores de Cumplimiento", ex)
        End Try
    End Sub

    Private Sub gvIndicadores_CellEditorInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles gvIndicadores.CellEditorInitialize
        If gvIndicadores.IsNewRowEditing Then Return
        If e.Column.FieldName = "ValorInicial" OrElse e.Column.FieldName = "ValorFinal" Then
            With CType(e.Editor, ASPxTextBox)
                If Not String.IsNullOrEmpty(.Text) Then
                    If CDec(.Text) < 1 Then .Text = (CDec(.Text) * 100).ToString("n1")
                    .Text = .Text.Replace(",", ".")
                End If
            End With
        End If
    End Sub

    Private Sub gvIndicadores_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gvIndicadores.RowUpdating
        Try
            mensajero.Limpiar()
            CType(sender, ASPxGridView).JSProperties.Clear()
            If _indicadores Is Nothing Then CargarIndicadores()
            Dim idIndicador As Integer = CInt(e.Keys("IdIndicador"))
            Dim indice As Integer = _indicadores.IndiceDe(idIndicador)
            Dim indicador As IndicadorCumplimientoMeta = _indicadores.ItemDe(idIndicador)

            If indice >= 0 Then
                Dim resultado As ResultadoProceso
                Dim actualizacionValida As Boolean = True

                With indicador
                    If Not EsNuloOVacio(e.NewValues("ValorInicial")) Then .ValorInicial = Math.Round(CDec(e.NewValues("ValorInicial")) / 100, 3)
                    If Not EsNuloOVacio(e.NewValues("ValorFinal")) Then .ValorFinal = Math.Round(CDec(e.NewValues("ValorFinal")) / 100, 3)
                End With

                If indicador.ValorFinal = 0 Then indicador.ValorFinal = 100
                If indicador.ValorInicial > 1 OrElse indicador.ValorFinal > 1 Then
                    actualizacionValida = False
                    mensajero.MostrarMensajePopUp("Los valores del rango no pueden ser mayores que 100. Por favor verifique")
                End If

                If Not indicador.ValorInicial <= indicador.ValorFinal Then
                    actualizacionValida = False
                    mensajero.MostrarMensajePopUp("El valor inicial del rango debe ser menor o igual al valor final")
                End If

                If indice > 0 Then
                    Dim indicadorAnterior As IndicadorCumplimientoMeta = CType(_indicadores(indice - 1), IndicadorCumplimientoMeta)
                    If indicador.ValorInicial <= indicadorAnterior.ValorInicial Then
                        actualizacionValida = False
                        mensajero.MostrarMensajePopUp("El valor inicial del rango no es valido. Se espera un valor mayor al valor inicial del rango inmediatamente anterior", MensajePopUp.TipoMensaje.Alerta, "Valor No Válido")
                    ElseIf (indicador.ValorInicial <= indicadorAnterior.ValorFinal) _
                            OrElse (indicador.ValorInicial - indicadorAnterior.ValorFinal) > 0.001 Then
                        indicadorAnterior.ValorFinal = indicador.ValorInicial - 0.001
                        indicadorAnterior.Modificado = True
                    End If
                End If

                If indice < _indicadores.Count - 1 Then
                    Dim indicadorSiguiente As IndicadorCumplimientoMeta = CType(_indicadores(indice + 1), IndicadorCumplimientoMeta)
                    If indicador.ValorInicial >= indicadorSiguiente.ValorInicial Then
                        actualizacionValida = False
                        mensajero.MostrarMensajePopUp("El valor inicial del rango no es valido. Se espera un valor menor al valor inicial del rango siguiente", MensajePopUp.TipoMensaje.Alerta, "Valor No Válido")
                    ElseIf indicador.ValorFinal >= indicadorSiguiente.ValorFinal Then
                        actualizacionValida = False
                        mensajero.MostrarMensajePopUp("El valor final del rango no es valido. Se espera un valor menor al valor final del rango siguiente", MensajePopUp.TipoMensaje.Alerta, "Valor No Válido")
                    ElseIf (indicador.ValorFinal >= indicadorSiguiente.ValorInicial) _
                        OrElse (indicadorSiguiente.ValorInicial - indicador.ValorFinal) > 0.001 Then
                        indicadorSiguiente.ValorInicial = indicador.ValorFinal + 0.001
                        indicadorSiguiente.Modificado = True
                    End If
                End If
                If actualizacionValida Then
                    indicador.Modificado = True
                    resultado = _indicadores.Actualizar()
                    If resultado.Valor = 0 Then
                        mensajero.MostrarMensajePopUp("El rango fue actualizado satisfactoriamente.", MensajePopUp.TipoMensaje.ProcesoExitoso, "Actualización Exitosa")
                        EnlazarIndicadores()
                    Else
                        Select Case resultado.Valor
                            Case 200 Or 300
                                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.Alerta)
                            Case Else
                                mensajero.MostrarMensajePopUp(resultado.Mensaje, MensajePopUp.TipoMensaje.ErrorCritico)
                        End Select
                    End If
                End If
            Else
                mensajero.MostrarMensajePopUp("No se ha logrado encontrar la información del indicador en memoria. Por favor recargue la página", MensajePopUp.TipoMensaje.Alerta, "Información No Encontrada")
            End If
            EnlazarIndicadores()
        Catch ex As Exception
            mensajero.MostrarErrorYNotificarViaMail("Error al tratar de actualizar datos. Por favor intente nuevamente", "Administración de Indicadores de Cumplimiento", ex)
        Finally
            If mensajero.Mensaje.Length > 0 Then
                CType(sender, ASPxGridView).JSProperties("cpMensajePopUp") = mensajero.RenderHtmlDeMensaje()
                CType(sender, ASPxGridView).JSProperties("cpTituloPopUp") = mensajero.Titulo
            End If
            gvIndicadores.CancelEdit()
            e.Cancel = True
        End Try
    End Sub
End Class