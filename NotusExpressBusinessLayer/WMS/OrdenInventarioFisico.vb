Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace WMS

    Public Class OrdenInventarioFisico

#Region "Atributos (Campos)"

        Private _idOrden As Integer
        Private _idBodega As Integer
        Private _bodega As String
        Private _idProducto As Integer
        Private _productoPadre As String
        Private _idSubproducto As Integer
        Private _subproducto As String
        Private _fechaCreacion As Date
        Private _idCreador As Integer
        Private _creador As String
        Private _idEstado As Byte
        Private _estado As String
        Private _observacion As String
        Private _cantidadEnInventario As Integer
        Private _fechaCierre As Date
        Private _idResponsableCierre As Integer
        Private _responsableCierre As String
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            _bodega = ""
            _productoPadre = ""
            _subproducto = ""
            _creador = ""
            _estado = ""
            _observacion = ""
            _responsableCierre = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            _idOrden = identificador
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdOrden() As Integer
            Get
                Return _idOrden
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idOrden = value
            End Set
        End Property

        Public Property IdBodega() As Integer
            Get
                Return _idBodega
            End Get
            Set(ByVal value As Integer)
                _idBodega = value
            End Set
        End Property

        Public Property Bodega() As String
            Get
                Return _bodega
            End Get
            Protected Friend Set(ByVal value As String)
                _bodega = value
            End Set
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property ProductoPadre() As String
            Get
                Return _productoPadre
            End Get
            Protected Friend Set(ByVal value As String)
                _productoPadre = value
            End Set
        End Property

        Public Property IdSubproducto() As Integer
            Get
                Return _idSubproducto
            End Get
            Set(ByVal value As Integer)
                _idSubproducto = value
            End Set
        End Property

        Public Property Subproducto() As String
            Get
                Return _subproducto
            End Get
            Protected Friend Set(ByVal value As String)
                _subproducto = value
            End Set
        End Property

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaCreacion = value
            End Set
        End Property

        Public Property IdCreador() As Integer
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Integer)
                _idCreador = value
            End Set
        End Property

        Public Property Creador() As String
            Get
                Return _creador
            End Get
            Protected Friend Set(ByVal value As String)
                _creador = value
            End Set
        End Property

        Public Property IdEstado() As Byte
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Byte)
                _idEstado = value
            End Set
        End Property

        Public Property Estado() As String
            Get
                Return _estado
            End Get
            Protected Friend Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property Observacion() As String
            Get
                Return _observacion
            End Get
            Set(ByVal value As String)
                _observacion = value
            End Set
        End Property

        Public Property CantidadEnInventario() As Integer
            Get
                Return _cantidadEnInventario
            End Get
            Set(ByVal value As Integer)
                _cantidadEnInventario = value
            End Set
        End Property

        Public Property FechaCierre() As Date
            Get
                Return _fechaCierre
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaCierre = value
            End Set
        End Property

        Public Property IdResponsableCierre() As Integer
            Get
                Return _idResponsableCierre
            End Get
            Set(ByVal value As Integer)
                _idResponsableCierre = value
            End Set
        End Property

        Public Property ResponsableCierre() As String
            Get
                Return _responsableCierre
            End Get
            Protected Friend Set(ByVal value As String)
                _responsableCierre = value
            End Set
        End Property

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If _idOrden > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idOrden > 0 Then .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                        .ejecutarReader("ObtenerInfoOrdenInventarioFisico", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then CargarResultadoConsulta(.Reader)
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idOrden").ToString, _idOrden)
                    Integer.TryParse(reader("idBodega").ToString, _idBodega)
                    _bodega = reader("bodega").ToString
                    Integer.TryParse(reader("idProducto").ToString, _idProducto)
                    _productoPadre = reader("productoPadre").ToString
                    Integer.TryParse(reader("idSubproducto").ToString, _idSubproducto)
                    _subproducto = reader("subproducto").ToString
                    Date.TryParse(reader("fechaCreacion").ToString, _fechaCreacion)
                    Integer.TryParse(reader("idCreador").ToString, _idCreador)
                    _creador = reader("creador").ToString
                    Byte.TryParse(reader("idEstado").ToString, _idEstado)
                    _estado = reader("estado").ToString
                    _observacion = reader("observacion").ToString
                    Integer.TryParse(reader("cantidadEnInventario").ToString, _cantidadEnInventario)
                    Date.TryParse(reader("fechaCierre").ToString, _fechaCierre)
                    Integer.TryParse(reader("idResponsableCierre").ToString, _idResponsableCierre)
                    _responsableCierre = reader("responsableCierre").ToString
                    _registrado = True
                End If
            End If
        End Sub
#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idBodega > 0 AndAlso _idProducto > 0 AndAlso _idCreador > 0 Then
                Dim dbManager As LMDataAccess = Nothing
                Try
                    dbManager = New LMDataAccess
                    With dbManager
                        .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                        .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        If _idSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = _idSubproducto
                        If Not String.IsNullOrEmpty(_observacion) Then _
                            .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        .SqlParametros.Add("@idOrden", SqlDbType.Int).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("CrearOrdenInventarioFisico", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                        Else
                            resultado.EstablecerMensajeYValor(300, "Imposible evaluar la respuesta del servidor de bases de datos. Por favor intente nuevamente")
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(200, "No se han establecido todos los valores requeridos para realizar el registro. Por favor verifique")
            End If

            Return resultado
        End Function

        Public Function CerrarOrden() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idOrden > 0 AndAlso _idResponsableCierre > 0 Then
                Dim dbManager As LMDataAccess = Nothing
                Try
                    dbManager = New LMDataAccess
                    With dbManager
                        .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                        .SqlParametros.Add("@idResponsableCierre", SqlDbType.Int).Value = _idResponsableCierre
                        If Not String.IsNullOrEmpty(_observacion) Then _
                            .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("CerrarOrdenInventarioFisico", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                        Else
                            resultado.EstablecerMensajeYValor(300, "Imposible evaluar la respuesta del servidor de bases de datos. Por favor intente nuevamente")
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(200, "No se han establecido todos los valores requeridos para realizar el registro. Por favor verifique")
            End If

            Return resultado
        End Function

#End Region

    End Class

End Namespace
