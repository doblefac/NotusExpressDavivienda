Imports GemBox.Spreadsheet
Imports System.Web
Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General

Namespace RecursoHumano

    Public Class AsesorComercial

#Region "Atributos"

        Private _idUsuarioSistema As Integer
        Private _idPersona As Integer
        Private _nombreAsesor As String
        Private _docIdentificacion As String
        Private _idUnidadNegocio As Short
        Private _unidadNegocio As String
        Private _idCliente As Integer
        Private _idModificador As Integer
        Private _idAsesor As Integer
        Private _email As String
        Private oExcel As ExcelFile
        Private _tablaDatos As DataTable
        Private _tablaErrores As DataTable
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _unidadNegocio = ""
        End Sub

        Public Sub New(idUsuarioSistema As Integer)
            Me.New()
            Me._idUsuarioSistema = idUsuarioSistema
            CargarInformacion()
        End Sub

        Public Sub New(numIdentificacion As String)
            Me.New()
            DocIdentificacion = numIdentificacion
            CargarInformacion()
        End Sub

        Public Sub New(ByRef ArchivoExcel As ExcelFile)
            MyBase.New()
            oExcel = ArchivoExcel
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdUsuarioSistema As Integer
            Get
                Return _idUsuarioSistema
            End Get
            Set(value As Integer)
                _idUsuarioSistema = value
            End Set
        End Property

        Public Property IdPersona As Integer
            Get
                Return _idPersona
            End Get
            Set(value As Integer)
                _idPersona = value
            End Set
        End Property

        Public Property NombreAsesor As String
            Get
                Return _nombreAsesor
            End Get
            Set(value As String)
                _nombreAsesor = value
            End Set
        End Property

        Public Property DocIdentificacion As String
            Get
                Return _docIdentificacion
            End Get
            Set(value As String)
                _docIdentificacion = value
            End Set
        End Property

        Public Property IdUnidadNegocio As Short
            Get
                Return _idUnidadNegocio
            End Get
            Set(value As Short)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property UnidadNegocio As String
            Get
                Return _unidadNegocio
            End Get
            Protected Friend Set(value As String)
                _unidadNegocio = value
            End Set
        End Property

        Public Property IdCliente As Integer
            Get
                Return _idCliente
            End Get
            Set(value As Integer)
                _idCliente = value
            End Set
        End Property

        Public Property IdModificador As Integer
            Get
                Return _idModificador
            End Get
            Set(value As Integer)
                _idModificador = value
            End Set
        End Property

        Public Property IdAsesor As Integer
            Get
                Return _idAsesor
            End Get
            Set(value As Integer)
                _idAsesor = value
            End Set
        End Property

        Public Property ArchivoExcel As ExcelFile
            Get
                Return oExcel
            End Get
            Set(value As ExcelFile)
                oExcel = value
            End Set
        End Property

        Public Property Email As String
            Get
                Return _email
            End Get
            Set(value As String)
                _email = value
            End Set
        End Property

        Public Property TablaDatos() As DataTable
            Get
                If _tablaDatos Is Nothing Then
                    EstructuraDatos()
                End If
                Return _tablaDatos
            End Get
            Set(ByVal value As DataTable)
                _tablaDatos = value
            End Set
        End Property

        Public Property TablaErrores() As DataTable
            Get
                If _tablaErrores Is Nothing Then
                    EstructuraDatosErrores()
                End If
                Return _tablaErrores
            End Get
            Set(ByVal value As DataTable)
                _tablaErrores = value
            End Set
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub EstructuraDatos()
            Try
                Dim dtDatos As New DataTable
                If _tablaDatos Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("idRegistro", GetType(Integer)))
                        .Add(New DataColumn("documentoCliente", GetType(String)))
                        .Add(New DataColumn("documentoAsesor", GetType(String)))
                        .Add(New DataColumn("idUsuarioRegistra", GetType(Integer)))
                    End With
                    dtDatos.AcceptChanges()
                    _tablaDatos = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub AdicionarError(ByVal id As Integer, ByVal nombre As String, ByVal descripcion As String)
            Try
                With TablaErrores
                    Dim drError As DataRow = .NewRow()
                    With drError
                        .Item("id") = id
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
                If _tablaErrores Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("id", GetType(Integer)))
                        .Add(New DataColumn("descripcion", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _tablaErrores = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

#Region "Méetodos Protegidos"

        Protected Overloads Sub CargarInformacion()
            If Me.IdPersona > 0 OrElse Not String.IsNullOrEmpty(Me.DocIdentificacion) OrElse Me._idUsuarioSistema > 0 Then
                Dim dbManager As LMDataAccess = Nothing
                Try
                    dbManager = New LMDataAccess
                    With dbManager
                        If Me._idUsuarioSistema > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuarioSistema
                        If Me.IdPersona > 0 Then .SqlParametros.Add("@idPersona", SqlDbType.Int).Value = Me.IdPersona
                        If Not String.IsNullOrEmpty(Me.DocIdentificacion) Then _
                            .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = Me.DocIdentificacion.Trim
                        .ejecutarReader("ObtenerInfoAsesorComercial", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then AsignarValorAPropiedades(.Reader)
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idUsuarioSistema").ToString, Me._idUsuarioSistema)
                    Integer.TryParse(reader("idPersona").ToString, Me._idPersona)
                    _nombreAsesor = reader("nombreApellido").ToString
                    _docIdentificacion = reader("numeroIdentificacion").ToString
                    Short.TryParse(reader("idUnidadNegocio").ToString, Me._idUnidadNegocio)
                    _unidadNegocio = reader("unidadNegocio").ToString
                    _email = reader("email").ToString
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function ActualizarAsesor() As General.ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim resultado As ResultadoProceso
            Try
                resultado = ActualizarAsesor(dbManager)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function ValidarArchivo() As Boolean
            Dim errorColumnas As Boolean = False
            Dim indexFila As Integer = 0
            Dim esValido As Boolean = True
            Dim index As Integer = 1
            Try
                If _tablaDatos Is Nothing Then
                    EstructuraDatos()
                End If

                If oExcel.Worksheets(0).Rows(0).AllocatedCells.Count <> Me._tablaDatos.Columns.Count - 2 Then
                    AdicionarError(index, "Columnas inválidas", "El Número de columnas es inválido.")
                    errorColumnas = True
                End If
                Dim entroEstructura As Boolean = False
                If Not errorColumnas Then
                    For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                        Dim indexColumna As Integer = 0
                        Dim drAux As DataRow
                        Dim entroDatos As Boolean = False
                        drAux = _tablaDatos.NewRow
                        If entroEstructura = False Then
                            Dim h As Integer
                            For h = 0 To _tablaDatos.Columns.Count - 3
                                If fila.AllocatedCells(h).Value.ToString.Trim.ToLower <> _tablaDatos.Columns(h + 1).ColumnName.Trim.ToLower Then
                                    AdicionarError(index, "Archivo Con estructura errada", "El archivo no contiene el orden de columnas esperado.")
                                    errorColumnas = True
                                    Exit For
                                End If
                            Next
                        End If
                        entroEstructura = True
                        If errorColumnas Then
                            Exit For
                        End If
                        For Each columna As ExcelCell In fila.AllocatedCells
                            If indexFila > 0 Then
                                If indexColumna <= Me._tablaDatos.Columns.Count - 1 Then
                                    If fila.AllocatedCells(0).Value IsNot Nothing Or fila.AllocatedCells(1).Value IsNot Nothing Then
                                        entroDatos = True
                                        drAux(0) = indexFila
                                        If fila.AllocatedCells(0).Value Is Nothing Then
                                            drAux(1) = ""
                                        Else
                                            drAux(1) = fila.AllocatedCells(0).Value.ToString.Trim
                                        End If
                                        If fila.AllocatedCells(1).Value Is Nothing Then
                                            drAux(2) = ""
                                        Else
                                            drAux(2) = fila.AllocatedCells(1).Value.ToString.Trim
                                        End If
                                        drAux(3) = _idUsuarioSistema
                                    End If
                                    indexColumna += 1
                                    If indexColumna > _tablaDatos.Columns.Count - 1 Then
                                        Exit For
                                    End If
                                Else
                                    indexColumna += 1
                                End If
                            Else
                                Exit For
                            End If
                        Next
                        If entroDatos Then
                            _tablaDatos.Rows.Add(drAux)
                        End If
                        indexFila = indexFila + 1
                    Next
                End If
                If Not errorColumnas Then
                    If _tablaDatos.Rows.Count = 0 Then
                        AdicionarError(_tablaErrores.Rows.Count + 1, "Archivo Sin Datos", "El archivo no contiene datos para actualizar.")
                        errorColumnas = True
                    End If
                End If
                If _tablaErrores IsNot Nothing AndAlso _tablaErrores.Rows.Count > 0 Then
                    esValido = Not (_tablaErrores.Rows.Count > 0)
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return esValido
        End Function

        Public Function ValidarInformacion(ByVal dtDatos As DataTable) As Boolean
            Dim esValido As Boolean = True
            Try
                TablaErrores.Clear()
                Using dbManager As New LMDataAccess
                    With dbManager
                        .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                        .TiempoEsperaComando = 600000
                        With .BulkCopy
                            .DestinationTableName = "TransitoriaActualizacionAsesorCliente"
                            .ColumnMappings.Add("idRegistro", "idRegistro")
                            .ColumnMappings.Add("documentoCliente", "documentoCliente")
                            .ColumnMappings.Add("documentoAsesor", "documentoAsesor")
                            .ColumnMappings.Add("idUsuarioRegistra", "idUsuarioRegistra")
                            .WriteToServer(dtDatos)
                        End With
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioSistema
                        TablaErrores = .ejecutarDataTable("ValidarDatosCambioAsesorCliente", CommandType.StoredProcedure)
                        esValido = (TablaErrores.Rows.Count = 0)
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
            Return esValido
        End Function

        Protected Friend Function ActualizarAsesor(ByVal dbManager As LMDataAccess) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idCliente > 0 AndAlso _idModificador > 0 AndAlso _idAsesor > 0 Then

                Dim finalizarObjAccesoDatos As Boolean = False
                Try
                    If dbManager Is Nothing Then
                        dbManager = New LMDataAccess
                        finalizarObjAccesoDatos = True
                    End If
                    With dbManager
                        .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _idCliente
                        .SqlParametros.Add("@idAsesor", SqlDbType.Int).Value = _idAsesor
                        .SqlParametros.Add("@idModificador", SqlDbType.Int).Value = _idModificador
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("ActualizarAsesorClienteFinal", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                        Else
                            resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante la actualización de los datos del asesor. Por favor intente nuevamente")
                        End If
                    End With
                Finally
                    If finalizarObjAccesoDatos AndAlso dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para actualizar los datos del asesor. Por favor verifique")
            End If

            Return resultado
        End Function

        Public Function ActualizarAsesorClienteArchivo() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idModificador", SqlDbType.Int).Value = _idModificador
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("ActualizarAsesorClienteFinalArchivo", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante la actualización de los datos. Por favor intente nuevamente")
                    End If
                End With
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se generó un error al tratar de actualizar los asesores: " & ex.Message)
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace