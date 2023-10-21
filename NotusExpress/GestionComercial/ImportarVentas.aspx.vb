Imports GemBox.Spreadsheet
Imports System.IO
Imports DevExpress.Web
Imports NotusExpressBusinessLayer

Public Class ImportarVentas
    Inherits System.Web.UI.Page

    Dim dtDatos As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epNotificador.clear()
        If Not IsPostBack Then
            epNotificador.setTitle("Importación de Ventas")
            Dim infoEstrategia As New EstrategiaComercialColeccion
            infoEstrategia.CargarDatos()
            With cbFiltroEstrategia
                .DataSource = infoEstrategia
                .TextField = "nombre"
                .ValueField = "idEstrategia"
                .DataBindItems()
            End With
        End If
    End Sub

    Private Sub Procesar(archivo As String)
        Session("DatosLog") = Nothing
        grdLog.DataSource = Nothing
        grdLog.DataBind()
        Herramientas.setGemBoxLicense()
        Dim dtDatosLeidos As New DataTable
        Dim ventaMasivo As New NotusExpressBusinessLayer.VentasMasivo
        ventaMasivo.IdEstrategiaComercial = Integer.Parse(cbFiltroEstrategia.Value)
        dtDatos = VentasMasivo.ObtenerTablaVentaMasivoArchivo()
        Dim dtDatosDataTable As New DataTable
        dtDatosDataTable = VentasMasivo.ObtenerTablaVentaMasivo()
        Dim dtLog As New DataTable
        Try

            Dim cabecerasDataTable As String
            cabecerasDataTable = "linea|nombreEmpleado|identificacion|tipo|cajaDeCompensacion|codigoConvenio|empresa|nit|idProducto|productoSolicitado|tipoProductoSolicitado|preAprobacion|cupoPreaprobado|fechaVisita|documentosCompletos|fechaRecoleccion|fechaRadicacion|nombreAsesor|identificacionAsesor|idciudad|ciudad|codigoEstrategia|codigoAgenteVendedor|observaciones1|sucursal|codigoSucursal|regional|estadoAsesor|supervisor|proceso|mes|seguro|aprobados|fechaAprobacion|activos|observaciones2|aprobado|negado|activo|valorAprobado|valorNegado|valorActivo"

            dtDatosLeidos = Herramientas.LeerArchivoExcel2010yPasarloaDataSet(archivo,
                                                               "NOMBRE EMPLEADO|IDENTIFICACIÓN|TIPO|CAJA DE COMPENSACIÓN|COD CONVENIO|EMPRESA|NIT|" & _
                                                               "PRODUCTO SOLICITADO|TIPO PRODUCTO SOLICITADO|PREAPROBACIÓN|CUPO PRE-APROBADO|FECHA VISITA (DD/MM/AAA)|DOCUMENTOS COMPLETOS|FECHA RECOLECCIÓN|" & _
                                                               "FECHA RADICACIÓN|NOMBRE ASESOR|IDENTIFICACIÓN ASESOR|CIUDAD|CODIGO DE ESTRATEGIA|CODIGO DE AGENTE VENDEDOR|OBSERVACIONES1|SUCURSAL|" & _
                                                               "CODIGO DE SUCURSAL|REGIONAL|ESTADO ASESOR|SUPERVISOR|PROCESO|MES|SEGURO|APROBADOS|FECHA APROBACIÓN|ACTIVOS|OBSERVACIONES2|APROBADO|NEGADO|ACTIVO|VR APROB|VR NEGADO|VR ACTIVO", dtDatos, True)


            ventaMasivo.DtLog = ObtenerTablaLogError()
            Dim i As Integer = 2

            For Each fila As DataRow In dtDatosLeidos.Rows
                If (fila("TIPO").ToString().Trim().Length = 0) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El TIPO no puede estar vacio!")
                If (fila("TIPO").ToString().Trim().Length > 1) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El TIPO no puede exceder la longitud de 1 caracter!")

                If (fila("IDENTIFICACIÓN").ToString().Trim().Length = 0) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "La IDENTIFICACIÓN no puede estar vacio!")
                If (fila("IDENTIFICACIÓN").ToString().Trim().Length > 30) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "La IDENTIFICACIÓN no puede exceder la longitud de 30 caracteres!")

                If (fila("NOMBRE EMPLEADO").ToString().Trim().Length = 0) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El NOMBRE EMPLEADO no puede estar vacio!")
                If (fila("NOMBRE EMPLEADO").ToString().Trim().Length > 100) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El NOMBRE EMPLEADO no puede exceder la longitud de 100 caracteres!")

                If (fila("IDENTIFICACIÓN ASESOR").ToString().Trim().Length = 0) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "La IDENTIFICACIÓN ASESOR no puede estar vacio!")
                If (fila("IDENTIFICACIÓN ASESOR").ToString().Trim().Length > 30) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "La IDENTIFICACIÓN ASESOR TIPO no puede exceder la longitud de 30 caracteres!")

                If (fila("NOMBRE ASESOR").ToString().Trim().Length = 0) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El NOMBRE ASESOR  no puede estar vacio!")
                If (fila("NOMBRE ASESOR").ToString().Trim().Length > 100) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El NOMBRE ASESOR  no puede exceder la longitud de 100 caracteres!")

                If (fila("CODIGO DE ESTRATEGIA").ToString().Trim().Length = 0) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El CODIGO DE ESTRATEGIA no puede estar vacio!")
                If (fila("CODIGO DE ESTRATEGIA").ToString().Trim().Length > 10) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El CODIGO DE ESTRATEGIA no puede exceder la longitud de 10 caracteres!")

                If (fila("CODIGO DE AGENTE VENDEDOR").ToString().Trim().Length = 0) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El CODIGO DE AGENTE VENDEDOR no puede estar vacio!")
                If (fila("CODIGO DE AGENTE VENDEDOR").ToString().Trim().Length > 10) Then ventaMasivo.DtLog.Rows.Add(i.ToString, "El CODIGO DE AGENTE VENDEDOR no puede exceder la longitud de 10 caracteres!")

                i = i + 1
            Next

            If ventaMasivo.DtLog.Rows.Count > 0 Then
                epNotificador.showError("Error en la grabación de ventas, Veer Log de Errores ")

                Session("DatosLog") = ventaMasivo.DtLog
                grdLog.DataSource = ventaMasivo.DtLog
                grdLog.DataBind()
                epNotificador.showError("Por favor verifique la estructura del archivo, las validaciones no fueron superadas")
            Else

                i = 1
                For Each fila As DataRow In dtDatosLeidos.Rows

                    Dim valorPreAprobado As Decimal
                    Dim fechaRadicacion As Date
                    Dim fechaVisita As Date
                    Dim fechaAprobacion As Date
                    Dim fechaRecoleccion As Date
                    Dim idProducto As Short = 0

                    Dim aprobado As Short
                    Dim negado As Short
                    Dim activo As Short

                    Dim valorAprobado As Decimal
                    Dim valorNegado As Decimal
                    Dim valorActivo As Decimal

                    Decimal.TryParse(fila("CUPO PRE-APROBADO").ToString(), valorPreAprobado)

                    Date.TryParse(fila("FECHA RADICACIÓN").ToString(), fechaRadicacion)
                    Date.TryParse(fila("FECHA VISITA (DD/MM/AAA)").ToString(), fechaVisita)
                    Date.TryParse(fila("FECHA APROBACIÓN").ToString(), fechaAprobacion)
                    Date.TryParse(fila("FECHA RECOLECCIÓN").ToString(), fechaRecoleccion)

                    Short.TryParse(fila("APROBADO").ToString.Trim(), aprobado)
                    Short.TryParse(fila("NEGADO").ToString.Trim(), negado)
                    Short.TryParse(fila("ACTIVO").ToString.Trim(), activo)

                    Decimal.TryParse(fila("VR APROB").ToString.Trim(), valorAprobado)
                    Decimal.TryParse(fila("VR NEGADO").ToString.Trim(), valorNegado)
                    Decimal.TryParse(fila("VR ACTIVO").ToString.Trim(), valorActivo)

                    dtDatosDataTable.Rows.Add(i.ToString, fila("NOMBRE EMPLEADO").ToString().Trim(), fila("IDENTIFICACIÓN").ToString().Trim(),
                                    fila("TIPO").ToString().Trim(),
                                    fila("CAJA DE COMPENSACIÓN").ToString().Trim(),
                                    fila("COD CONVENIO").ToString().Trim(),
                                    fila("EMPRESA").ToString().Trim(),
                                    fila("NIT").ToString().Trim(),
                                    -1,
                                    fila("PRODUCTO SOLICITADO").ToString().Trim(),
                                    fila("TIPO PRODUCTO SOLICITADO").ToString().Trim(),
                                    fila("PREAPROBACIÓN").ToString().Trim(),
                                    valorPreAprobado,
                                    fila("FECHA VISITA (DD/MM/AAA)").ToString().Trim(),
                                    fila("DOCUMENTOS COMPLETOS").ToString().Trim(),
                                    IIf(fechaRecoleccion = Date.MinValue, Nothing, fechaRecoleccion),
                                    IIf(fechaRadicacion = Date.MinValue, Nothing, fechaRadicacion),
                                    fila("NOMBRE ASESOR").ToString().Trim(),
                                    fila("IDENTIFICACIÓN ASESOR").ToString().Trim(),
                                    -1,
                                    fila("CIUDAD").ToString().Trim(),
                                    fila("CODIGO DE ESTRATEGIA").ToString().Trim(),
                                    fila("CODIGO DE AGENTE VENDEDOR").ToString().Trim(),
                                    fila("OBSERVACIONES1").ToString().Trim(),
                                    fila("SUCURSAL").ToString().Trim(),
                                    fila("CODIGO DE SUCURSAL").ToString().Trim(),
                                    fila("REGIONAL").ToString().Trim(),
                                    fila("ESTADO ASESOR").ToString().Trim(),
                                    fila("SUPERVISOR").ToString().Trim(),
                                    fila("PROCESO").ToString().Trim(),
                                    fila("MES").ToString().Trim(),
                                    fila("SEGURO").ToString().Trim(),
                                    fila("APROBADOS").ToString().Trim(),
                                    IIf(fechaAprobacion = Date.MinValue, Nothing, fechaAprobacion),
                                    fila("ACTIVOS").ToString().Trim(),
                                    fila("OBSERVACIONES2").ToString().Trim(),
                                    aprobado, negado, activo, valorAprobado, valorNegado, valorActivo)

                    i = i + 1
                Next

                ventaMasivo.Registrar(dtDatosDataTable)

                If ventaMasivo.DtLog.Rows.Count = 0 Then
                    epNotificador.showSuccess("Proceso completado con éxito No : " & ventaMasivo.idImportacion.ToString)
                Else
                    Session("DatosLog") = ventaMasivo.DtLog
                    grdLog.DataSource = ventaMasivo.DtLog
                    grdLog.DataBind()
                    epNotificador.showError("Error en el proceso de importación de ventas")
                End If

            End If

        Catch ex As Exception
            epNotificador.showError("Error registrando ventas : " & ex.Message)
        End Try
    End Sub

    Public Function ObtenerTablaLogError() As DataTable
        Dim dtDatos As New DataTable
        dtDatos.TableName = "tbLogError"
        dtDatos.Columns.Add("linea", GetType(Short))
        dtDatos.Columns.Add("descripcion", GetType(String))
        Return dtDatos
    End Function

    Protected Sub grdLog_DataBinding(sender As Object, e As EventArgs) Handles grdLog.DataBinding
        grdLog.DataSource = CType(Session("DatosLog"), DataTable)
    End Sub

    Protected Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        If (adjunto.FileName <> String.Empty) Then
            adjunto.SaveAs(Server.MapPath("~/GestionComercial/Archivos/") + Path.GetFileName(adjunto.FileName))
            If (Server.MapPath("~/GestionComercial/Archivos/") + Path.GetFileName(adjunto.FileName) <> String.Empty) Then
                Procesar(Server.MapPath("~/GestionComercial/Archivos/") + Path.GetFileName(adjunto.FileName))
            Else
                epNotificador.showError("Por favor seleccione un archivo")
            End If
        End If
    End Sub
  
End Class