
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.WMS
Imports NotusExpressBusinessLayer.MaestroProductos
Imports NotusExpressBusinessLayer.General

Partial Public Class RegistrarDetalleDeInventarioFisico
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("idOrden")) Then
                ObtenerInfoOrden()
            Else
                pnlGeneral.Enabled = False
                epNotificador.showWarning("No se logró recuperar el identificador de la orden. Por favor cierre la ventana modal y vuelva a intentar.")
            End If
        End If
    End Sub

    Private Sub ObtenerInfoOrden()
        Dim idOrden As Integer
        Try
            Integer.TryParse(Request.QueryString("idOrden"), idOrden)
            Dim infoOrden As New OrdenInventarioFisico(idOrden)
            If infoOrden.Registrado Then
                With infoOrden
                    lblIdOrden.Text = .IdOrden.ToString
                    lblBodega.Text = .Bodega
                    lblProductoPadre.Text = .ProductoPadre
                    lblSubproducto.Text = .Subproducto
                    lblCantidadLeida.Text = .CantidadEnInventario.ToString
                End With
                lbCerrarOrden.Enabled = CBool(lblCantidadLeida.Text)
            Else
                pnlGeneral.Enabled = False
                epNotificador.showWarning("No se encontró información asociada a la orden proporcionada. Por favor verifique")
            End If

        Catch ex As Exception
            pnlGeneral.Enabled = False
            epNotificador.showError("Error al tratar de obtener la información de la orden. ")
        End Try

    End Sub

    Private Sub cpSerial_Execute(ByVal sender As Object, ByVal e As EO.Web.CallbackEventArgs) Handles cpSerial.Execute
        If e.Parameter = "registrar" Then
            RegistrarSerialEnInventario()
        End If
    End Sub

    Private Sub RegistrarSerialEnInventario()
        Dim detalle As New DetalleSerialOrdenInventarioFisico
        Dim idOrden As Integer
        Dim resultado As New ResultadoProceso

        Try
            Integer.TryParse(Request.QueryString("idOrden"), idOrden)
            With detalle
                .IdOrden = idOrden
                .Serial = txtSerial.Text.Trim
                .IdRegistrador = CInt(Session("userId"))
                resultado = .Registrar()
            End With
            If resultado.Valor = 0 Then
                lblCantidadLeida.Text = (CInt(lblCantidadLeida.Text) + 1).ToString
                lbCerrarOrden.Enabled = CBool(lblCantidadLeida.Text)
                epNotificador.showSuccess(resultado.Mensaje)
            Else
                epNotificador.showError(resultado.Mensaje)
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de registrar serial en el inventario. ")
        End Try
    End Sub

    Protected Sub lbCerrarOrden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCerrarOrden.Click
        Try
            Dim infoOrden As New OrdenInventarioFisico(CInt(lblIdOrden.Text))
            Dim resultado As ResultadoProceso
            With infoOrden
                .IdResponsableCierre = CInt(Session("userId"))
                resultado = .CerrarOrden()
            End With
            If resultado.Valor = 0 Then
                epNotificador.showSuccess("La orden de inventario No. " & lblIdOrden.Text & " fue Cerrada satisfactoriamente.")
                pnlGeneral.Enabled = False
            Else
                epNotificador.showError(resultado.Mensaje)
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de crerrar la orden. ")
        End Try
    End Sub
End Class
