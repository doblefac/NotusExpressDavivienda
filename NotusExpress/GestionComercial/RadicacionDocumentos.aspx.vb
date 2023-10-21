Imports DevExpress.Web
Imports NotusExpressBusinessLayer.Localizacion
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.RecursoHumano
Imports NotusExpressBusinessLayer.General

Public Class RadicacionDocumentos
    Inherits System.Web.UI.Page

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Me.IsPostBack Then
                epNotificador.setTitle("Radicación Documentos")
                If Request.QueryString("id") IsNot Nothing Then
                    Dim listaGestion As New List(Of Long)
                    Dim lista As String = CStr(Request.QueryString("id"))

                    For Each idGes As String In lista.Split(",")
                        listaGestion.Add(idGes)
                    Next
                    Session("listIdGestion") = listaGestion
                    CargarListadoDeServicios(listaGestion)
                    CargarDocumentos()
                Else
                    epNotificador.showWarning("No se logró establecer el listado de gestiones a radicar, por favor regrese a la página anterior")
                    imgRadica.ClientVisible = False
                End If
            End If
        Catch ex As Exception
            epNotificador.showError("Se presentó un error al cargar la página: " & ex.Message)
        End Try
    End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Dim idRadicado As Integer = 0
        Dim resultado As New ResultadoProceso
        Try
            Dim arrayAccion As String()
            arrayAccion = e.Parameter.Split(":")
            Select Case arrayAccion(0)
                Case "Radicar"
                    resultado = RadicarDocumentos(idRadicado)
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack: " & ex.Message)
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
        CType(sender, ASPxCallbackPanel).JSProperties("cpListaRadicado") = idRadicado
    End Sub

    Private Sub gvVentas_DataBinding(sender As Object, e As System.EventArgs) Handles gvVentas.DataBinding
        gvVentas.DataSource = Session("dtDatos")
    End Sub


#End Region

#Region "Métodos Privados"

    Private Sub CargarListadoDeServicios(ByVal listIdgestion As List(Of Long))
        Dim miDetalleGestion As New GestionComercial.GestionDeVentaDetalleColeccion
        Dim dtDatos As New DataTable
        With miDetalleGestion
            .listGestionVenta = listIdgestion
            dtDatos = .GenerarDataTable()
            Session("dtDatos") = dtDatos
        End With

        With gvVentas
            .DataSource = dtDatos
            .DataBind()
        End With

    End Sub

    Private Sub CargarDocumentos()
        Try
            Dim dtDatos As DataTable
            Dim objDocumentos As New ObtenerListadoDocumentosRadicar
            With objDocumentos
                .IdEstado = 1
                .CargarDatos()
                dtDatos = .DatosRegistros
            End With

            With gluDocumentos
                .DataSource = dtDatos
                .DataBind()
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Function RadicarDocumentos(ByRef idRadicado As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim listIdgestion As List(Of Long)
        If Session("listIdGestion") IsNot Nothing Then
            Dim listaDoc As List(Of Object) = gluDocumentos.GridView().GetSelectedFieldValues("idDocumento")
            Dim objRadicar As New RadicarDocumentos
            listIdgestion = Session("listIdGestion")

            With objRadicar
                .IdUsuarioRegistra = CInt(Session("userId"))
                .NumeroPrecinto = txtPrecinto.Text.Trim
                .ListaDocumentos.AddRange(listaDoc)
                .ListaGestiones = listIdgestion
                resultado = .Registrar()
            End With
            If resultado.Valor = 0 Then
                epNotificador.showSuccess(resultado.Mensaje)
                imgRadica.ClientVisible = False
                idRadicado = objRadicar.IdRadicado
            Else
                epNotificador.showError(resultado.Mensaje)
                idRadicado = 0
            End If
        Else
            epNotificador.showWarning("No se pudo recuperar el listado de servicios a radicar, por favor regrese a la página anterior.")
            idRadicado = 0
        End If
        Return resultado
    End Function

#End Region

End Class