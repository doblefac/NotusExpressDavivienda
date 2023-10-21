Imports System.Web
Imports GemBox.Spreadsheet
Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.General

Public Class CargueClientesPresenciales

#Region "Atributos"

    Private oExcel As ExcelFile
    Private _estructuraTablaBase As DataTable
    Private _estructuraTabla As DataTable
    Private _estructuraTablaErrores As DataTable

#End Region

#Region "Propiedades"

    Public Property ArchivoExcel As ExcelFile
        Get
            Return oExcel
        End Get
        Set(value As ExcelFile)
            oExcel = value
        End Set
    End Property

    Public Property EstructuraTablaBase() As DataTable
        Get
            If _estructuraTablaBase Is Nothing Then
                EstructuraDatosBase()
            End If
            Return _estructuraTablaBase
        End Get
        Set(ByVal value As DataTable)
            _estructuraTablaBase = value
        End Set
    End Property

    Public Property EstructuraTabla() As DataTable
        Get
            If _estructuraTabla Is Nothing Then
                EstructuraDatos()
            End If
            Return _estructuraTabla
        End Get
        Set(ByVal value As DataTable)
            _estructuraTabla = value
        End Set
    End Property

    Public Property EstructuraTablaErrores() As DataTable
        Get
            If _estructuraTablaErrores Is Nothing Then
                EstructuraDatosErrores()
            End If
            Return _estructuraTablaErrores
        End Get
        Set(ByVal value As DataTable)
            _estructuraTablaErrores = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New(ByRef ArchivoExcel As ExcelFile)
        MyBase.New()
        oExcel = ArchivoExcel
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub EstructuraDatosBase()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTablaBase Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("instruccion", GetType(String)))
                    .Add(New DataColumn("cajaCompensacion", GetType(String)))
                    .Add(New DataColumn("estado", GetType(String)))
                    .Add(New DataColumn("tarjeta", GetType(String)))
                    .Add(New DataColumn("nombreCliente", GetType(String)))
                    .Add(New DataColumn("pSeudocodigo", GetType(String)))
                    .Add(New DataColumn("numeroIdentificacion", GetType(String)))
                    .Add(New DataColumn("tipoEmision", GetType(String)))
                    .Add(New DataColumn("companiaDistribucion", GetType(String)))
                    .Add(New DataColumn("fechaEnvioRealce", GetType(String)))
                    .Add(New DataColumn("estrategiaMercado", GetType(String)))
                    .Add(New DataColumn("formaDistribucion", GetType(String)))
                    .Add(New DataColumn("compañiaRealzadora", GetType(String)))
                    .Add(New DataColumn("fechaIngreso", GetType(String)))
                    .Add(New DataColumn("fechaGestion", GetType(String)))
                    .Add(New DataColumn("estadoDefinitivo", GetType(String)))
                    .Add(New DataColumn("distribuidor", GetType(String)))
                    .Add(New DataColumn("estadoFinal", GetType(String)))
                    .Add(New DataColumn("estadoDistribuidor", GetType(String)))
                    .Add(New DataColumn("descripcionMotivo", GetType(String)))
                    .Add(New DataColumn("codDireccion", GetType(String)))
                    .Add(New DataColumn("tipoEntrega", GetType(String)))
                    .Add(New DataColumn("direccionEntrega", GetType(String)))
                    .Add(New DataColumn("codOficinaDevolucion", GetType(String)))
                    .Add(New DataColumn("moduloCiudad", GetType(String)))
                    .Add(New DataColumn("nombrePunto", GetType(String)))
                    .Add(New DataColumn("estadoEvaluacion", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTablaBase = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub EstructuraDatos()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTabla Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("instruccion", GetType(String)))
                    .Add(New DataColumn("cajaCompensacion", GetType(String)))
                    .Add(New DataColumn("estado", GetType(String)))
                    .Add(New DataColumn("tarjeta", GetType(String)))
                    .Add(New DataColumn("nombreCliente", GetType(String)))
                    .Add(New DataColumn("pSeudocodigo", GetType(String)))
                    .Add(New DataColumn("numeroIdentificacion", GetType(String)))
                    .Add(New DataColumn("tipoEmision", GetType(String)))
                    .Add(New DataColumn("companiaDistribucion", GetType(String)))
                    .Add(New DataColumn("fechaEnvioRealce", GetType(String)))
                    .Add(New DataColumn("estrategiaMercado", GetType(String)))
                    .Add(New DataColumn("formaDistribucion", GetType(String)))
                    .Add(New DataColumn("compañiaRealzadora", GetType(String)))
                    .Add(New DataColumn("fechaIngreso", GetType(String)))
                    .Add(New DataColumn("fechaGestion", GetType(String)))
                    .Add(New DataColumn("estadoDefinitivo", GetType(String)))
                    .Add(New DataColumn("distribuidor", GetType(String)))
                    .Add(New DataColumn("estadoFinal", GetType(String)))
                    .Add(New DataColumn("estadoDistribuidor", GetType(String)))
                    .Add(New DataColumn("descripcionMotivo", GetType(String)))
                    .Add(New DataColumn("codDireccion", GetType(String)))
                    .Add(New DataColumn("tipoEntrega", GetType(String)))
                    .Add(New DataColumn("direccionEntrega", GetType(String)))
                    .Add(New DataColumn("codOficinaDevolucion", GetType(String)))
                    .Add(New DataColumn("moduloCiudad", GetType(String)))
                    .Add(New DataColumn("nombrePunto", GetType(String)))
                    .Add(New DataColumn("estadoEvaluacion", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTabla = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AdicionarError(ByVal id As Integer, ByVal nombre As String, ByVal descripcion As String)
        Try
            With EstructuraTablaErrores
                Dim drError As DataRow = .NewRow()
                With drError
                    .Item("id") = id
                    .Item("nombre") = nombre
                    .Item("descripcion") = descripcion
                End With
                .Rows.Add(drError)
                .AcceptChanges()
            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub EstructuraDatosErrores()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTablaErrores Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("id", GetType(Integer)))
                    .Add(New DataColumn("nombre", GetType(String)))
                    .Add(New DataColumn("descripcion", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTablaErrores = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ExtractDataErrorHandler(sender As Object, e As ExtractDataDelegateEventArgs)
        If e.ErrorID = ExtractDataError.WrongType Then
            If Not IsNumeric(e.ExcelValue) And e.ExcelValue = Nothing Then
                e.DataTableValue = Nothing
            Else
                e.DataTableValue = e.ExcelValue.ToString()
            End If

            If e.DataTableValue = Nothing Then
                e.Action = ExtractDataEventAction.SkipRow
            Else
                e.Action = ExtractDataEventAction.Continue
            End If
        End If
    End Sub

    Private Sub AdicionarColumnas()
        Try
            'Se crean los campos de los materiales en la estructura de tabla
            Dim index As Integer = 1
            Dim fila As ExcelRow = oExcel.Worksheets(0).Rows(0)

            Dim dtDatos As DataTable = EstructuraTabla()
            AddHandler oExcel.Worksheets(0).ExtractDataEvent, AddressOf ExtractDataErrorHandler
            oExcel.Worksheets(0).ExtractToDataTable(dtDatos, oExcel.Worksheets(0).Rows.Count, ExtractDataOptions.SkipEmptyRows, oExcel.Worksheets(0).Rows(1), oExcel.Worksheets(0).Columns(0))

            'Se crea la estructura por Filas
            For Each registro As DataRow In dtDatos.Rows
                Dim registroFinal As DataRow = EstructuraTablaBase.NewRow()
                With registroFinal
                    .Item("instruccion") = registro("instruccion").ToString.Trim
                    .Item("cajaCompensacion") = registro("cajaCompensacion").ToString.Trim
                    .Item("estado") = registro("estado").ToString.Trim
                    .Item("tarjeta") = registro("tarjeta").ToString.Trim
                    .Item("nombreCliente") = registro("nombreCliente").ToString.Trim
                    .Item("pSeudocodigo") = registro("pSeudocodigo").ToString.Trim
                    .Item("numeroIdentificacion") = registro("numeroIdentificacion").ToString.Trim
                    .Item("tipoEmision") = registro("tipoEmision").ToString.Trim
                    .Item("companiaDistribucion") = registro("companiaDistribucion").ToString.Trim
                    .Item("fechaEnvioRealce") = registro("fechaEnvioRealce").ToString.Trim
                    .Item("estrategiaMercado") = registro("estrategiaMercado").ToString.Trim
                    .Item("formaDistribucion") = registro("formaDistribucion").ToString.Trim
                    .Item("compañiaRealzadora") = registro("compañiaRealzadora").ToString.Trim
                    .Item("fechaIngreso") = registro("fechaIngreso").ToString.Trim
                    .Item("fechaGestion") = registro("fechaGestion").ToString.Trim
                    .Item("estadoDefinitivo") = registro("estadoDefinitivo").ToString.Trim
                    .Item("distribuidor") = registro("distribuidor").ToString.Trim
                    .Item("estadoFinal") = registro("estadoFinal").ToString.Trim
                    .Item("estadoDistribuidor") = registro("estadoDistribuidor").ToString.Trim
                    .Item("descripcionMotivo") = registro("descripcionMotivo").ToString.Trim
                    .Item("codDireccion") = registro("codDireccion").ToString.Trim
                    .Item("tipoEntrega") = registro("tipoEntrega").ToString.Trim
                    .Item("direccionEntrega") = registro("direccionEntrega").ToString.Trim
                    .Item("codOficinaDevolucion") = registro("codOficinaDevolucion").ToString.Trim
                    .Item("moduloCiudad") = registro("moduloCiudad").ToString.Trim
                    .Item("nombrePunto") = registro("nombrePunto").ToString.Trim
                    .Item("estadoEvaluacion") = registro("estadoEvaluacion").ToString.Trim
                End With
                EstructuraTablaBase.Rows.Add(registroFinal)
                index = index + 1
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function HayDatosEnFila(ByVal infoFila As ExcelRow)
        Dim resultado As Boolean = False
        For index As Integer = 0 To infoFila.AllocatedCells.Count
            If infoFila.AllocatedCells(index).Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(infoFila.AllocatedCells(index).Value.ToString) Then
                resultado = True
                Exit For
            End If
        Next
        Return resultado
    End Function

    Private Sub CargarInformacionTransitoria()
        Dim esValido As Boolean = True
        Try
            AdicionarColumnas()
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("userId"))

            If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
            EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario))

            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .ejecutarNonQuery("LiberarDatosTransitoriosClientePresencial", CommandType.StoredProcedure)

                    .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 600000
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaClienteFinalPresencial"
                        .ColumnMappings.Add("instruccion", "instruccion")
                        .ColumnMappings.Add("cajaCompensacion", "cajaCompensacion")
                        .ColumnMappings.Add("estado", "estado")
                        .ColumnMappings.Add("tarjeta", "tarjeta")
                        .ColumnMappings.Add("nombreCliente", "nombreCliente")
                        .ColumnMappings.Add("pSeudocodigo", "pSeudocodigo")
                        .ColumnMappings.Add("numeroIdentificacion", "numeroIdentificacion")
                        .ColumnMappings.Add("tipoEmision", "tipoEmision")
                        .ColumnMappings.Add("companiaDistribucion", "companiaDistribucion")
                        .ColumnMappings.Add("fechaEnvioRealce", "fechaEnvioRealce")
                        .ColumnMappings.Add("estrategiaMercado", "estrategiaMercado")
                        .ColumnMappings.Add("formaDistribucion", "formaDistribucion")
                        .ColumnMappings.Add("compañiaRealzadora", "compañiaRealzadora")
                        .ColumnMappings.Add("fechaIngreso", "fechaIngreso")

                        .ColumnMappings.Add("fechaGestion", "fechaGestion")
                        .ColumnMappings.Add("estadoDefinitivo", "estadoDefinitivo")
                        .ColumnMappings.Add("distribuidor", "distribuidor")
                        .ColumnMappings.Add("estadoFinal", "estadoFinal")
                        .ColumnMappings.Add("estadoDistribuidor", "estadoDistribuidor")
                        .ColumnMappings.Add("descripcionMotivo", "descripcionMotivo")
                        .ColumnMappings.Add("codDireccion", "codDireccion")
                        .ColumnMappings.Add("tipoEntrega", "tipoEntrega")
                        .ColumnMappings.Add("direccionEntrega", "direccionEntrega")
                        .ColumnMappings.Add("codOficinaDevolucion", "codOficinaDevolucion")
                        .ColumnMappings.Add("moduloCiudad", "moduloCiudad")
                        .ColumnMappings.Add("nombrePunto", "nombrePunto")
                        .ColumnMappings.Add("estadoEvaluacion", "estadoEvaluacion")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ValidarEstructura() As Boolean
        Dim esValido As Boolean = True
        Dim index As Integer = 1
        Dim hayDatos As Boolean
        Dim expresion As New ConfigValues("EXPREG_GENERAL")
        Dim expDir As New ConfigValues("EXPREG_DIRECCION")
        Dim oExpReg As New System.Text.RegularExpressions.Regex(expresion.ConfigKeyValue)
        Dim oExpRegDir As New System.Text.RegularExpressions.Regex(expDir.ConfigKeyValue)
        Try
            For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                hayDatos = HayDatosEnFila(oExcel.Worksheets(0).Rows.Item(index - 1))
                If fila.AllocatedCells.Count <> Me.EstructuraTabla.Columns.Count Then
                    AdicionarError(index, "Fila inválida", "El Número de columnas de la fila es inválido.")
                ElseIf index > 1 Then

                    If String.IsNullOrEmpty(fila.Cells(0).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo de 'Instrucion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(0).Value) Then _
                            AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para la 'Instruccion' no son válidos.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(1).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo de 'Caja de Compensacion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(1).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para 'Caja de Compensacion' no son válidos.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(2).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Estado' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(2).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para 'Estado' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(3).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo de 'Tarjeta' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(3).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para 'Tarjeta' no son válidos.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(4).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo de 'Nombre Cliente' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(4).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el 'Nombre Cliente' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(5).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'pSeudocodigo' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(5).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'pSeudocodigo' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(6).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Numero de Identificacion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(6).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Numero de Identificacion' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(7).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Tipo Emision' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(7).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Tipo Emision' no son válidos.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(8).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Compañia Distribucion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(8).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Compañia Distribucion' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(9).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Fecha Envio Realce' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(9).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Fecha Envio Realce' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(10).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Estrategia de Mercadeo' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(10).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Estrategia de Mercadeo' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(11).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Forma Distribucion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(11).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Forma Distribucion' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(12).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Compañia Realzadora' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(12).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Compañia Realzadora' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(13).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Fecha Ingreso' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(13).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Fecha Ingreso' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(14).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Fecha Gestion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(14).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Fecha Gestion' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(15).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Estado Definitivo' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(15).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Estado Definitivo' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(16).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Distribuidor' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(16).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Distribuidor' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(17).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Estado Final' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(17).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Estado Final' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(18).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Estado Distribuidor' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(18).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Estado Distribuidor' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(19).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Descripcion ó Motivo' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(19).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Descripcion ó Motivo' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(20).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Cod. Direccion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(20).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Cod. Direccion' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(21).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Tipo Entrega' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(21).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Tipo Entrega' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(22).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Direccion Entrega' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(22).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Direccion Entrega' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(23).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Cod. Oficina Devolucion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(23).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Cod. Oficina Devolucion' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(24).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Modulo Ciudad' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(24).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Modulo Ciudad' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(25).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Nombre Punto' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(25).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Nombre Punto' no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(26).Value) Then
                        AdicionarError(index, "Dato inválido", "El campo 'Estado Evaluacion' no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(26).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Estado Evaluacion' no son válidas.")
                    End If
                End If
                index += 1
            Next
            esValido = Not (EstructuraTablaErrores.Rows.Count > 0)

            If esValido Then
                CargarInformacionTransitoria()
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return esValido
    End Function

    Function CrearClientes(ByVal estrategia As String, ByVal nombreArchivo As String, ByVal tamanio As Integer, ByVal rutaAlmacenamiento As String, ByVal origen As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("userId"))
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@estrategia", SqlDbType.VarChar).Value = estrategia
                    .Add("@nombreArchivo", SqlDbType.VarChar).Value = nombreArchivo
                    .Add("@rutaAlmacenamiento", SqlDbType.VarChar).Value = rutaAlmacenamiento
                    .Add("@tamanio", SqlDbType.Int).Value = tamanio
                    .Add("@origenCargue", SqlDbType.Int).Value = origen
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .iniciarTransaccion()
                .ejecutarReader("RegistrarClientePresencial", CommandType.StoredProcedure)

                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    While .Reader.Read()
                        AdicionarError(CInt(.Reader("id")), .Reader("campo"), IIf(IsDBNull(.Reader("mensaje")), "", .Reader("mensaje")))
                    End While
                    .Reader.Close()
                End If

                Dim respuesta As Integer = .SqlParametros("@resultado").Value
                If respuesta = 0 Or respuesta = 40 Then
                    resultado.Valor = respuesta
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If Not .Reader.IsClosed Then .Reader.Close()
                    .confirmarTransaccion()
                Else
                    resultado.Valor = respuesta
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    .abortarTransaccion()
                End If
            End With

        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            resultado.EstablecerMensajeYValor(400, "Se generó un error al crear los clientes: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
