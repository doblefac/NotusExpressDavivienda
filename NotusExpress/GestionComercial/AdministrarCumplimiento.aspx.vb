Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer
Imports DevExpress.Web
Imports System.IO

Public Class AdministrarCumplimiento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        epNotificador.clear()
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Administración de Cumplimiento de Metas")
            'CargarListadoDeCumplimientoRegistrado()
        End If
        'If Not Me.IsPostBack Then 
        'CargarListadoDeCumplimientoRegistrado()
    End Sub

    'Private Sub CargarListadoDeCumplimientoRegistrado(Optional ByVal forzarConsulta As Boolean = False)
    '    Dim listaCumplimiento As CumplimientoColeccion = Nothing

    '    Try
    '        If Session("listaMarcas") Is Nothing OrElse forzarConsulta Then
    '            listaCumplimiento = New CumplimientoColeccion
    '            With listaCumplimiento
    '                .CargarDatos()
    '            End With
    '            Session("listaMarcas") = listaCumplimiento
    '        Else
    '            listaCumplimiento = CType(Session("listaMarcas"), CumplimientoColeccion)
    '        End If

    '        With gvCumplimiento
    '            .DataSource = listaCumplimiento
    '            .DataBind()
    '        End With
    '    Catch ex As Exception
    '        epNotificador.showError("Error al tratar de cargar el listado de Canales.")
    '    End Try
    'End Sub

    Private Sub cpGeneral_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpGeneral.Callback
        Try
            Select Case e.Parameter
                Case "crearCumplimiento"
                    'crearCumplimiento()
            End Select
        Catch ex As Exception
            epNotificador.showError("Error al tratar de manejar CallBack.")
        End Try
        CType(sender, ASPxCallbackPanel).JSProperties("cpMensaje") = epNotificador.RenderHtml()
    End Sub

    Private Sub crearCumplimiento()
        Try
            Dim rutaAlmacenamiento As String = "\ArchivosCargados\"
            uplArchivo1.SaveAs(Server.MapPath("~") & rutaAlmacenamiento & uplArchivo1.FileName.ToString)
            uplArchivo2.SaveAs(Server.MapPath("~") & rutaAlmacenamiento & uplArchivo2.FileName.ToString)
            uplArchivo3.SaveAs(Server.MapPath("~") & rutaAlmacenamiento & uplArchivo3.FileName.ToString)

            Dim registrarC As New RegistrarCumplimiento
            With registrarC
                .Inicio1 = CDbl(txtInicio1.Text)
                .Fin1 = CDbl(txtFin1.Text)
                .RutaColor1 = rutaAlmacenamiento & uplArchivo1.FileName.ToString
                .Inicio2 = CDbl(txtInicio2.Text)
                .Fin2 = CDbl(txtFin2.Text)
                .RutaColor2 = rutaAlmacenamiento & uplArchivo2.FileName.ToString
                .Inicio3 = CDbl(txtInicio3.Text)
                .Fin3 = CDbl(txtFin3.Text)
                .RutaColor3 = rutaAlmacenamiento & uplArchivo3.FileName.ToString
                .Registrar()
            End With
            epNotificador.showSuccess("Cumplimientos guardados satisfactoriamente.")
        Catch
            epNotificador.showError("Hubo un error el guardar los cumplimientos")
        End Try
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        crearCumplimiento()
    End Sub
End Class