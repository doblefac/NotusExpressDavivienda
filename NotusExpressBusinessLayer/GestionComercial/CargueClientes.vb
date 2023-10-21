Imports System.Web
Imports GemBox.Spreadsheet
Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.General
Imports System.Text.RegularExpressions

Public Class CargueClientes

#Region "Atributos."

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
                    .Add(New DataColumn("CATEGORIA", GetType(String)))
                    .Add(New DataColumn("NOMBRE_CAMPANA", GetType(String)))
                    .Add(New DataColumn("TIPO_IDENTIFICACION", GetType(String)))
                    .Add(New DataColumn("NUMERO_IDENTIFICACION", GetType(String)))
                    .Add(New DataColumn("NOMBRE", GetType(String)))
                    .Add(New DataColumn("APELLIDO1", GetType(String)))
                    .Add(New DataColumn("APELLIDO2", GetType(String)))
                    .Add(New DataColumn("INGRESO", GetType(String)))
                    .Add(New DataColumn("SEGMENTO", GetType(String)))
                    .Add(New DataColumn("TIPO_CLIENTE", GetType(String)))
                    .Add(New DataColumn("GENERO", GetType(String)))
                    .Add(New DataColumn("EDAD", GetType(String)))
                    .Add(New DataColumn("CLIENTE_ES_DAVIVIENDA", GetType(String)))
                    .Add(New DataColumn("CLIENTE_TIENE_CTA_AHORROS", GetType(String)))
                    .Add(New DataColumn("TELEFONO1", GetType(String)))
                    .Add(New DataColumn("DIRECCION1", GetType(String)))
                    .Add(New DataColumn("CIUDAD1", GetType(String)))
                    .Add(New DataColumn("DPTO1", GetType(String)))
                    .Add(New DataColumn("TELEFONO2", GetType(String)))
                    .Add(New DataColumn("DIRECCION2", GetType(String)))
                    .Add(New DataColumn("CIUDAD2", GetType(String)))
                    .Add(New DataColumn("DPTO2", GetType(String)))
                    .Add(New DataColumn("TELEFONO3", GetType(String)))
                    .Add(New DataColumn("DIRECCION3", GetType(String)))
                    .Add(New DataColumn("CIUDAD3", GetType(String)))
                    .Add(New DataColumn("DPTO3", GetType(String)))
                    .Add(New DataColumn("CELULAR1", GetType(String)))
                    .Add(New DataColumn("CELULAR2", GetType(String)))
                    .Add(New DataColumn("CELULAR_DAVIPLATA", GetType(String)))
                    .Add(New DataColumn("CORREO", GetType(String)))
                    .Add(New DataColumn("CIUDAD_COBERTURA", GetType(String)))
                    .Add(New DataColumn("NOMBRE_OFICINA", GetType(String)))
                    .Add(New DataColumn("OFICINA_MODELO", GetType(String)))
                    .Add(New DataColumn("DIRECCION_CAJA", GetType(String)))
                    .Add(New DataColumn("TELEFONO_CAJA", GetType(String)))
                    .Add(New DataColumn("CIUDAD_CAJA", GetType(String)))
                    .Add(New DataColumn("CELULAR_CAJA", GetType(String)))
                    .Add(New DataColumn("PRODUCTO1", GetType(String)))
                    .Add(New DataColumn("CUPO1", GetType(String)))
                    .Add(New DataColumn("PRODUCTO2", GetType(String)))
                    .Add(New DataColumn("CUPO2", GetType(String)))
                    .Add(New DataColumn("PRODUCTO3", GetType(String)))
                    .Add(New DataColumn("CUPO3", GetType(String)))
                    .Add(New DataColumn("PRODUCTO4", GetType(String)))
                    .Add(New DataColumn("CUPO4", GetType(String)))
                    .Add(New DataColumn("PRODUCTO5", GetType(String)))
                    .Add(New DataColumn("CUPO5", GetType(String)))
                    .Add(New DataColumn("PRODUCTO6", GetType(String)))
                    .Add(New DataColumn("CUPO6", GetType(String)))
                    .Add(New DataColumn("PRODUCTO7", GetType(String)))
                    .Add(New DataColumn("CUPO7", GetType(String)))
                    .Add(New DataColumn("PRODUCTO8", GetType(String)))
                    .Add(New DataColumn("CUPO8", GetType(String)))
                    .Add(New DataColumn("PRODUCTO9", GetType(String)))
                    .Add(New DataColumn("CUPO9", GetType(String)))
                    .Add(New DataColumn("PRODUCTO10", GetType(String)))
                    .Add(New DataColumn("CUPO10", GetType(String)))
                    .Add(New DataColumn("COLOR_TARJETA", GetType(String)))
                    .Add(New DataColumn("MARCADOR_PREDICTIVO", GetType(String)))
                    .Add(New DataColumn("COD_ESTRATEGIA", GetType(String)))
                    .Add(New DataColumn("COD_CAMPANA", GetType(String)))
                    .Add(New DataColumn("CANAL", GetType(String)))
                    .Add(New DataColumn("OBSERVACIONES", GetType(String)))
                    .Add(New DataColumn("ACTIVIDAD_LABORAL", GetType(String)))
                    .Add(New DataColumn("COD_SUCURSAL", GetType(String)))
                    .Add(New DataColumn("COD_AGENTE", GetType(String)))
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
                    .Add(New DataColumn("CATEGORIA", GetType(String)))
                    .Add(New DataColumn("NOMBRE_CAMPANA", GetType(String)))
                    .Add(New DataColumn("TIPO_IDENTIFICACION", GetType(String)))
                    .Add(New DataColumn("NUMERO_IDENTIFICACION", GetType(String)))
                    .Add(New DataColumn("NOMBRE", GetType(String)))
                    .Add(New DataColumn("APELLIDO1", GetType(String)))
                    .Add(New DataColumn("APELLIDO2", GetType(String)))
                    .Add(New DataColumn("INGRESO", GetType(String)))
                    .Add(New DataColumn("SEGMENTO", GetType(String)))
                    .Add(New DataColumn("TIPO_CLIENTE", GetType(String)))
                    .Add(New DataColumn("GENERO", GetType(String)))
                    .Add(New DataColumn("EDAD", GetType(String)))
                    .Add(New DataColumn("CLIENTE_ES_DAVIVIENDA", GetType(String)))
                    .Add(New DataColumn("CLIENTE_TIENE_CTA_AHORROS", GetType(String)))
                    .Add(New DataColumn("TELEFONO1", GetType(String)))
                    .Add(New DataColumn("DIRECCION1", GetType(String)))
                    .Add(New DataColumn("CIUDAD1", GetType(String)))
                    .Add(New DataColumn("DPTO1", GetType(String)))
                    .Add(New DataColumn("TELEFONO2", GetType(String)))
                    .Add(New DataColumn("DIRECCION2", GetType(String)))
                    .Add(New DataColumn("CIUDAD2", GetType(String)))
                    .Add(New DataColumn("DPTO2", GetType(String)))
                    .Add(New DataColumn("TELEFONO3", GetType(String)))
                    .Add(New DataColumn("DIRECCION3", GetType(String)))
                    .Add(New DataColumn("CIUDAD3", GetType(String)))
                    .Add(New DataColumn("DPTO3", GetType(String)))
                    .Add(New DataColumn("CELULAR1", GetType(String)))
                    .Add(New DataColumn("CELULAR2", GetType(String)))
                    .Add(New DataColumn("CELULAR_DAVIPLATA", GetType(String)))
                    .Add(New DataColumn("CORREO", GetType(String)))
                    .Add(New DataColumn("CIUDAD_COBERTURA", GetType(String)))
                    .Add(New DataColumn("NOMBRE_OFICINA", GetType(String)))
                    .Add(New DataColumn("OFICINA_MODELO", GetType(String)))
                    .Add(New DataColumn("DIRECCION_CAJA", GetType(String)))
                    .Add(New DataColumn("TELEFONO_CAJA", GetType(String)))
                    .Add(New DataColumn("CIUDAD_CAJA", GetType(String)))
                    .Add(New DataColumn("CELULAR_CAJA", GetType(String)))
                    .Add(New DataColumn("PRODUCTO1", GetType(String)))
                    .Add(New DataColumn("CUPO1", GetType(String)))
                    .Add(New DataColumn("PRODUCTO2", GetType(String)))
                    .Add(New DataColumn("CUPO2", GetType(String)))
                    .Add(New DataColumn("PRODUCTO3", GetType(String)))
                    .Add(New DataColumn("CUPO3", GetType(String)))
                    .Add(New DataColumn("PRODUCTO4", GetType(String)))
                    .Add(New DataColumn("CUPO4", GetType(String)))
                    .Add(New DataColumn("PRODUCTO5", GetType(String)))
                    .Add(New DataColumn("CUPO5", GetType(String)))
                    .Add(New DataColumn("PRODUCTO6", GetType(String)))
                    .Add(New DataColumn("CUPO6", GetType(String)))
                    .Add(New DataColumn("PRODUCTO7", GetType(String)))
                    .Add(New DataColumn("CUPO7", GetType(String)))
                    .Add(New DataColumn("PRODUCTO8", GetType(String)))
                    .Add(New DataColumn("CUPO8", GetType(String)))
                    .Add(New DataColumn("PRODUCTO9", GetType(String)))
                    .Add(New DataColumn("CUPO9", GetType(String)))
                    .Add(New DataColumn("PRODUCTO10", GetType(String)))
                    .Add(New DataColumn("CUPO10", GetType(String)))
                    .Add(New DataColumn("COLOR_TARJETA", GetType(String)))
                    .Add(New DataColumn("MARCADOR_PREDICTIVO", GetType(String)))
                    .Add(New DataColumn("COD_ESTRATEGIA", GetType(String)))
                    .Add(New DataColumn("COD_CAMPANA", GetType(String)))
                    .Add(New DataColumn("CANAL", GetType(String)))
                    .Add(New DataColumn("OBSERVACIONES", GetType(String)))
                    .Add(New DataColumn("ACTIVIDAD_LABORAL", GetType(String)))
                    .Add(New DataColumn("COD_SUCURSAL", GetType(String)))
                    .Add(New DataColumn("COD_AGENTE", GetType(String)))
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
                    .Item("Id") = id
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
                    .Item("NOMBRE_CAMPANA") = registro("NOMBRE_CAMPANA").ToString.Trim
                    .Item("TIPO_IDENTIFICACION") = registro("TIPO_IDENTIFICACION").ToString.Trim
                    .Item("NUMERO_IDENTIFICACION") = registro("NUMERO_IDENTIFICACION").ToString.Trim
                    .Item("NOMBRE") = registro("NOMBRE").ToString.Trim
                    .Item("APELLIDO1") = registro("APELLIDO1").ToString.Trim
                    .Item("APELLIDO2") = registro("APELLIDO2").ToString.Trim
                    .Item("INGRESO") = registro("INGRESO").ToString.Trim
                    .Item("SEGMENTO") = registro("SEGMENTO").ToString.Trim
                    .Item("TIPO_CLIENTE") = registro("TIPO_CLIENTE").ToString.Trim
                    .Item("GENERO") = registro("GENERO").ToString.Trim
                    .Item("EDAD") = registro("EDAD").ToString.Trim
                    .Item("CLIENTE_ES_DAVIVIENDA") = registro("CLIENTE_ES_DAVIVIENDA").ToString.Trim
                    .Item("CLIENTE_TIENE_CTA_AHORROS") = registro("CLIENTE_TIENE_CTA_AHORROS").ToString.Trim
                    .Item("TELEFONO1") = registro("TELEFONO1").ToString.Trim
                    .Item("DIRECCION1") = registro("DIRECCION1").ToString.Trim
                    .Item("CIUDAD1") = registro("CIUDAD1").ToString.Trim
                    .Item("DPTO1") = registro("DPTO1").ToString.Trim
                    .Item("TELEFONO2") = registro("TELEFONO2").ToString.Trim
                    .Item("DIRECCION2") = registro("DIRECCION2").ToString.Trim
                    .Item("CIUDAD2") = registro("CIUDAD2").ToString.Trim
                    .Item("DPTO2") = registro("DPTO2").ToString.Trim
                    .Item("TELEFONO3") = registro("TELEFONO3").ToString.Trim
                    .Item("DIRECCION3") = registro("DIRECCION3").ToString.Trim
                    .Item("CIUDAD3") = registro("CIUDAD3").ToString.Trim
                    .Item("DPTO3") = registro("DPTO3").ToString.Trim
                    .Item("CELULAR1") = registro("CELULAR1").ToString.Trim
                    .Item("CELULAR2") = registro("CELULAR2").ToString.Trim
                    .Item("CELULAR_DAVIPLATA") = registro("CELULAR_DAVIPLATA").ToString.Trim
                    .Item("CORREO") = registro("CORREO").ToString.Trim
                    .Item("CIUDAD_COBERTURA") = registro("CIUDAD_COBERTURA").ToString.Trim
                    .Item("NOMBRE_OFICINA") = registro("NOMBRE_OFICINA").ToString.Trim
                    .Item("OFICINA_MODELO") = registro("OFICINA_MODELO").ToString.Trim
                    .Item("DIRECCION_CAJA") = registro("DIRECCION_CAJA").ToString.Trim
                    .Item("TELEFONO_CAJA") = registro("TELEFONO_CAJA").ToString.Trim
                    .Item("CIUDAD_CAJA") = registro("CIUDAD_CAJA").ToString.Trim
                    .Item("CELULAR_CAJA") = registro("CELULAR_CAJA").ToString.Trim
                    .Item("PRODUCTO1") = registro("PRODUCTO1").ToString.Trim
                    .Item("CUPO1") = registro("CUPO1").ToString.Trim
                    .Item("PRODUCTO2") = registro("PRODUCTO2").ToString.Trim
                    .Item("CUPO2") = registro("CUPO2").ToString.Trim
                    .Item("PRODUCTO3") = registro("PRODUCTO3").ToString.Trim
                    .Item("CUPO3") = registro("CUPO3").ToString.Trim
                    .Item("PRODUCTO4") = registro("PRODUCTO4").ToString.Trim
                    .Item("CUPO4") = registro("CUPO4").ToString.Trim
                    .Item("PRODUCTO5") = registro("PRODUCTO5").ToString.Trim
                    .Item("CUPO5") = registro("CUPO5").ToString.Trim
                    .Item("PRODUCTO6") = registro("PRODUCTO6").ToString.Trim
                    .Item("CUPO6") = registro("CUPO6").ToString.Trim
                    .Item("PRODUCTO7") = registro("PRODUCTO7").ToString.Trim
                    .Item("CUPO7") = registro("CUPO7").ToString.Trim
                    .Item("PRODUCTO8") = registro("PRODUCTO8").ToString.Trim
                    .Item("CUPO8") = registro("CUPO8").ToString.Trim
                    .Item("PRODUCTO9") = registro("PRODUCTO9").ToString.Trim
                    .Item("CUPO9") = registro("CUPO9").ToString.Trim
                    .Item("PRODUCTO10") = registro("PRODUCTO10").ToString.Trim
                    .Item("CUPO10") = registro("CUPO10").ToString.Trim
                    .Item("COLOR_TARJETA") = registro("COLOR_TARJETA").ToString.Trim
                    .Item("MARCADOR_PREDICTIVO") = registro("MARCADOR_PREDICTIVO").ToString.Trim
                    .Item("COD_ESTRATEGIA") = registro("COD_ESTRATEGIA").ToString.Trim
                    .Item("COD_CAMPANA") = registro("COD_CAMPANA").ToString.Trim
                    .Item("CANAL") = registro("CANAL").ToString.Trim
                    .Item("OBSERVACIONES") = registro("OBSERVACIONES").ToString.Trim
                    .Item("ACTIVIDAD_LABORAL") = registro("ACTIVIDAD_LABORAL").ToString.Trim
                    .Item("COD_SUCURSAL") = registro("COD_SUCURSAL").ToString.Trim
                    .Item("COD_AGENTE") = registro("COD_AGENTE").ToString.Trim
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

#End Region

#Region "Métodos Públicos"

    Public Function ValidarEstructura(ByVal codEstrategia As String) As Boolean
        Dim esValido As Boolean = True
        Dim index As Integer = 1
        Dim numInte As Integer
        Dim dato As String
        Dim hayDatos As Boolean
        Dim expresion As New ConfigValues("EXPREG_GENERAL")
        Dim oExpReg As New System.Text.RegularExpressions.Regex(expresion.ConfigKeyValue)
        Try
            For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                hayDatos = HayDatosEnFila(oExcel.Worksheets(0).Rows.Item(index - 1))
                If fila.AllocatedCells.Count <> Me.EstructuraTabla.Columns.Count And index = 1 Then
                    AdicionarError(index, "Error", "El Número de columnas es diferebte al esperado")
                    Exit Function
                End If
                If index > 1 Then

                    If String.IsNullOrEmpty(fila.Cells(3).Value) Then
                        AdicionarError(index, "Dato inválido", "El número de identificación no puede estar vacío.")
                    Else
                        If Not oExpReg.IsMatch(fila.Cells(3).Value) Then _
                            AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para la identificación no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(4).Value) Then
                        AdicionarError(index, "Dato inválido", "El nombre del cliente no puede estar vacío.")
                    Else
                        dato = fila.Cells(4).Value
                        If dato.Length > 150 Then _
                            AdicionarError(index, "Dato inválido", "La longitud del nombre del cliente no es válida.")

                        If Not oExpReg.IsMatch(fila.Cells(4).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para nombre no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(5).Value) Then
                        AdicionarError(index, "Dato inválido", "El primer apellido del cliente no puede estar vacío.")
                    Else
                        dato = fila.Cells(5).Value
                        If dato.Length > 150 Then _
                            AdicionarError(index, "Dato inválido", "La longitud del primer apellido del cliente no es válida.")

                        If Not oExpReg.IsMatch(fila.Cells(5).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el primer apellido no son válidas.")
                    End If

                    'If String.IsNullOrEmpty(fila.Cells(3).Value) Then
                    '    AdicionarError(index, "Dato inválido", "El segundo apellido del cliente no puede estar vacío.")
                    'Else
                    If fila.Cells(6).Value IsNot Nothing Then
                        dato = fila.Cells(6).Value.ToString.Trim
                        If dato.Length > 150 Then _
                            AdicionarError(index, "Dato inválido", "La longitud del segundo apellido del cliente no es válida.")

                        If Not oExpReg.IsMatch(fila.Cells(6).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el segundo apellido no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(10).Value) Then
                        AdicionarError(index, "Dato inválido", "El sexo del cliente no puede estar vacío.")
                    Else
                        dato = fila.Cells(10).Value
                        If dato.Length < 1 Then _
                            AdicionarError(index, "Dato inválido", "La longitud del sexo del cliente no es válida.")

                        If Not oExpReg.IsMatch(fila.Cells(10).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el sexo no son válidas.")
                    End If

                    'If String.IsNullOrEmpty(fila.Cells(5).Value) Then
                    '    AdicionarError(index, "Dato inválido", "El campo cargo ó actividad laboral del cliente no puede estar vacío.")
                    'Else
                    '    dato = fila.Cells(5).Value
                    '    If dato.Length > 50 Then _
                    '        AdicionarError(index, "Dato inválido", "La longitud del campo cargo ó actividad laboral del cliente no es válida.")

                    '    If Not oExpReg.IsMatch(fila.Cells(5).Value) Then _
                    '            AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo cargo ó actividad laboral no son válidas.")
                    'End If

                    If String.IsNullOrEmpty(fila.Cells(15).Value) Then
                        AdicionarError(index, "Dato inválido", "La dirección del cliente no puede estar vacía.")
                    Else
                        dato = fila.Cells(15).Value
                        If dato.Length > 150 Then _
                            AdicionarError(index, "Dato inválido", "La longitud de la dirección del cliente no es válida.")

                        If Not oExpReg.IsMatch(fila.Cells(15).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para la dirección no son válidas.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(31).Value) Then
                        AdicionarError(index, "Dato inválido", "La Ciudad de cobertura no puede estar vacía.")
                    Else
                        dato = fila.Cells(31).Value
                        If dato.Length > 150 Then _
                            AdicionarError(index, "Dato inválido", "La longitud de la ciudad del cliente no es válida.")

                        If Not oExpReg.IsMatch(fila.Cells(31).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para la ciudad no son válidas.")
                    End If

                    dato = fila.Cells(14).Value
                    If dato IsNot Nothing Then
                        If dato.Length > 15 Then _
                            AdicionarError(index, "Dato inválido", "La longitud del TELEFONO1 fijo del cliente no es válida.")
                    Else
                        AdicionarError(index, "Dato inválido", "El campo telefono fijo no puede estar vacio.")
                    End If

                    dato = fila.Cells(26).Value
                    If dato IsNot Nothing Then
                        If dato.Length > 20 Then _
                            AdicionarError(index, "Dato inválido", "La longitud del CELULAR1  del cliente no es válida.")
                    Else
                        AdicionarError(index, "Dato inválido", "El campo telefono celular no puede estar vacio.")
                    End If

                    dato = fila.Cells(29).Value
                    If dato IsNot Nothing Then
                        dato = dato.ToLower()
                        dato = dato.Replace(" ", "")
                        Dim re As Regex = New Regex("([\w-+]+(?:\.[\w-+]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7})")
                        Dim m As Match = re.Match(dato)

                        If Not (m.Captures.Count <> 0) Then
                            AdicionarError(index, "Dato inválido", "El formato del correo no es válido.")
                        ElseIf dato.Length > 150 Then
                            AdicionarError(index, "Dato inválido", "La longitud del e-mail del cliente no es válida.")
                        End If

                    End If

                    dato = fila.Cells(59).Value

                    'If (dato <> codEstrategia) Then
                    '    AdicionarError(index, "Dato inválido", "El código de estrategia válido para esta campaña es " & codEstrategia)
                    'End If
                    If dato IsNot Nothing Then
                        If Not oExpReg.IsMatch(dato) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el código de estrategía no son válidas.")
                    Else
                        AdicionarError(index, "Dato inválido", "El campo codigo de estrategia no puede estar vacio.")
                    End If

                    'If String.IsNullOrEmpty(fila.Cells(13).Value) Then
                    '    AdicionarError(index, "Dato inválido", "El campo idOportunidad no puede estar vacío.")
                    'Else
                    '    If Not oExpReg.IsMatch(fila.Cells(13).Value) Then _
                    '        AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo idOportunidad no son válidos.")
                    'End If

                    'If String.IsNullOrEmpty(fila.Cells(14).Value) Then
                    '    AdicionarError(index, "Dato inválido", "El campo nombreOportunidad no puede estar vacío.")
                    'Else
                    '    If Not oExpReg.IsMatch(fila.Cells(14).Value) Then _
                    '        AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo nombreOportunidad no son válidos.")
                    'End If

                    dato = fila.Cells(32).Value
                    If dato IsNot Nothing Then
                        If Not oExpReg.IsMatch(fila.Cells(32).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el código de oficina 1 no son válidas.")
                    Else
                        AdicionarError(index, "Dato inválido", "El campo codigo de oficina 1 no puede estar vacio.")
                    End If

                    'dato = fila.Cells(31).Value
                    'If dato IsNot Nothing Then
                    '    If Not oExpReg.IsMatch(fila.Cells(31).Value) Then _
                    '            AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el código de oficina 2 no son válidas.")
                    'Else
                    '    AdicionarError(index, "Dato inválido", "El campo codigo de oficina 2 no puede estar vacio.")
                    'End If

                    'dato = fila.Cells(17).Value
                    'If dato IsNot Nothing Then
                    '    If Not oExpReg.IsMatch(fila.Cells(17).Value) Then _
                    '            AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el código de sucursal no son válidas.")
                    'Else
                    '    AdicionarError(index, "Dato inválido", "El campo codigo de sucursal no puede estar vacio.")
                    'End If

                    'dato = fila.Cells(18).Value
                    'If dato IsNot Nothing Then
                    '    If Not oExpReg.IsMatch(fila.Cells(18).Value) Then _
                    '            AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el código de agente no son válidas.")
                    'Else
                    '    AdicionarError(index, "Dato inválido", "El campo codigo de agente no puede estar vacio.")
                    'End If

                    dato = fila.Cells(31).Value
                    If dato IsNot Nothing Then
                        If Not oExpReg.IsMatch(fila.Cells(31).Value) Then _
                            AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el nombre de la oficina no son válidas.")
                    Else
                        AdicionarError(index, "Dato inválido", "El campo nombre de la oficina no puede estar vacio.")
                    End If
                    dato = fila.Cells(38).Value
                    If dato Is Nothing Then
                        AdicionarError(index, "Dato inválido", "El campo cupo de credito no puede estar vacio.")
                    End If
                End If
                index += 1
            Next
            esValido = Not (EstructuraTablaErrores.Rows.Count > 0)
        Catch ex As Exception
            Throw ex
        End Try
        Return esValido
    End Function

    Public Function ValidarInformacion(ByVal idCampania As Integer) As Boolean
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
                    .EjecutarNonQuery("LiberarDatosTransitoriosCliente", CommandType.StoredProcedure)

                    .InicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 0
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaClienteFinal"
                        .ColumnMappings.Add("CATEGORIA", "CATEGORIA")
                        .ColumnMappings.Add("NOMBRE_CAMPANA", "NOMBRE_CAMPANA")
                        .ColumnMappings.Add("TIPO_IDENTIFICACION", "idTipoIdentificacion")
                        .ColumnMappings.Add("NUMERO_IDENTIFICACION", "identificacionCliente")
                        .ColumnMappings.Add("NOMBRE", "nombreCliente")
                        .ColumnMappings.Add("APELLIDO1", "primerApellido")
                        .ColumnMappings.Add("APELLIDO2", "segundoApellido")
                        .ColumnMappings.Add("INGRESO", "INGRESO")
                        .ColumnMappings.Add("SEGMENTO", "SEGMENTO")
                        .ColumnMappings.Add("TIPO_CLIENTE", "TIPO_CLIENTE")
                        .ColumnMappings.Add("GENERO", "sexo")
                        .ColumnMappings.Add("EDAD", "EDAD")
                        'ok
                        .ColumnMappings.Add("CLIENTE_ES_DAVIVIENDA", "CLIENTE_ES_DAVIVIENDA")
                        .ColumnMappings.Add("CLIENTE_TIENE_CTA_AHORROS", "CLIENTE_TIENE_CTA_AHORROS")
                        .ColumnMappings.Add("TELEFONO1", "telefonoFijo")
                        .ColumnMappings.Add("DIRECCION1", "direccion")
                        .ColumnMappings.Add("CIUDAD1", "ciudad")
                        .ColumnMappings.Add("DPTO1", "departamento")
                        ''ok
                        .ColumnMappings.Add("TELEFONO2", "TELEFONO2")
                        .ColumnMappings.Add("DIRECCION2", "DIRECCION2")
                        .ColumnMappings.Add("CIUDAD2", "CIUDAD2")
                        .ColumnMappings.Add("DPTO2", "DPTO2")
                        .ColumnMappings.Add("TELEFONO3", "TELEFONO3")
                        .ColumnMappings.Add("DIRECCION3", "DIRECCION3")
                        .ColumnMappings.Add("CIUDAD3", "CIUDAD3")
                        .ColumnMappings.Add("DPTO3", "DPTO3")
                        .ColumnMappings.Add("CELULAR1", "telefonoCelular")
                        .ColumnMappings.Add("CELULAR2", "CELULAR2")
                        .ColumnMappings.Add("CELULAR_DAVIPLATA", "CELULAR_DAVIPLATA")
                        .ColumnMappings.Add("CORREO", "mail")
                        .ColumnMappings.Add("CIUDAD_COBERTURA", "CIUDAD_COBERTURA")
                        .ColumnMappings.Add("NOMBRE_OFICINA", "nombreOficina")
                        .ColumnMappings.Add("OFICINA_MODELO", "codOficina1")
                        .ColumnMappings.Add("DIRECCION_CAJA", "DIRECCION_CAJA")
                        .ColumnMappings.Add("TELEFONO_CAJA", "TELEFONO_CAJA")
                        .ColumnMappings.Add("CIUDAD_CAJA", "CIUDAD_CAJA")
                        .ColumnMappings.Add("CELULAR_CAJA", "CELULAR_CAJA")
                        .ColumnMappings.Add("PRODUCTO1", "PRODUCTO1")
                        ''ok
                        .ColumnMappings.Add("CUPO1", "cupoCredito")
                        .ColumnMappings.Add("PRODUCTO2", "PRODUCTO2")
                        .ColumnMappings.Add("CUPO2", "CUPO2")
                        .ColumnMappings.Add("PRODUCTO3", "PRODUCTO3")
                        .ColumnMappings.Add("CUPO3", "CUPO3")
                        .ColumnMappings.Add("PRODUCTO4", "PRODUCTO4")
                        .ColumnMappings.Add("CUPO4", "CUPO4")
                        .ColumnMappings.Add("PRODUCTO5", "PRODUCTO5")
                        .ColumnMappings.Add("CUPO5", "CUPO5")
                        .ColumnMappings.Add("PRODUCTO6", "PRODUCTO6")
                        .ColumnMappings.Add("CUPO6", "CUPO6")
                        .ColumnMappings.Add("PRODUCTO7", "PRODUCTO7")
                        .ColumnMappings.Add("CUPO7", "CUPO7")
                        .ColumnMappings.Add("PRODUCTO8", "PRODUCTO8")
                        .ColumnMappings.Add("CUPO8", "CUPO8")
                        .ColumnMappings.Add("PRODUCTO9", "PRODUCTO9")
                        .ColumnMappings.Add("CUPO9", "CUPO9")
                        .ColumnMappings.Add("PRODUCTO10", "PRODUCTO10")
                        .ColumnMappings.Add("CUPO10", "CUPO10")
                        'ok
                        .ColumnMappings.Add("COLOR_TARJETA", "COLOR_TARJETA")
                        .ColumnMappings.Add("MARCADOR_PREDICTIVO", "MARCADOR_PREDICTIVO")
                        .ColumnMappings.Add("COD_ESTRATEGIA", "codigoEstrategia")
                        .ColumnMappings.Add("COD_CAMPANA", "COD_CAMPANA")
                        .ColumnMappings.Add("CANAL", "CANAL")
                        .ColumnMappings.Add("OBSERVACIONES", "OBSERVACIONES")
                        .ColumnMappings.Add("ACTIVIDAD_LABORAL", "actividadLaboral")
                        .ColumnMappings.Add("COD_SUCURSAL", "codSucursal")
                        .ColumnMappings.Add("COD_AGENTE", "codAgenteCliente")
                        .ColumnMappings.Add("idUsuario", "idUsuario")

                        .WriteToServer(EstructuraTablaBase)
                    End With

                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    _estructuraTablaErrores = .EjecutarDataTable("ValidarDatosClienteFinal", CommandType.StoredProcedure)

                    esValido = (EstructuraTablaErrores.Rows.Count = 0)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return esValido
    End Function

    Function CrearClientes(ByVal idCampania As Integer, ByVal nombreArchivo As String, ByVal tamanio As Integer, ByVal rutaAlmacenamiento As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("userId"))
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@idCampania", SqlDbType.Int).Value = idCampania
                    .Add("@nombreArchivo", SqlDbType.VarChar).Value = nombreArchivo
                    .Add("@rutaAlmacenamiento", SqlDbType.VarChar).Value = rutaAlmacenamiento
                    .Add("@tamanio", SqlDbType.Int).Value = tamanio
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .TiempoEsperaComando = 0
                .IniciarTransaccion()
                .ejecutarReader("RegistrarCliente", CommandType.StoredProcedure)

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
                    .ConfirmarTransaccion()
                Else
                    resultado.Valor = respuesta
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    .AbortarTransaccion()
                End If
            End With

        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(400, "Se generó un error al crear los clientes: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function ConsultaCodEstrategia(ByVal idCampania As Integer) As DataTable

        Dim dbManager As New LMDataAccess
        Dim dtEstrategia As New DataTable
        With dbManager
            .SqlParametros.Add("@IdCampania", SqlDbType.Int).Value = idCampania
            dtEstrategia = .EjecutarDataTable("ConsultarCodEstrategiaCampania", CommandType.StoredProcedure)
        End With
        Return dtEstrategia
    End Function

#End Region

End Class
