Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace GestionComercial

    Public Class GestionDeVentaDetalle

#Region "Atributos"

        Private _idDetalle As Long
        Private _idGestionVenta As Long
        Private _idCliente As Integer
        Private _fechaRegistro As DateTime
        Private _idUsuarioAsesor As Integer
        Private _idProducto As Integer
        Private _producto As String
        Private _tipoProducto As String
        Private _idValorPrima As Integer
        Private _valorPrima As String
        Private _productoTipoServicio As String
        Private _observacion As String
        Private _numeroIdentificacion As String

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdDetalle As Long
            Get
                Return _idDetalle
            End Get
            Set(value As Long)
                _idDetalle = value
            End Set
        End Property

        Public Property IdGestionVenta As Long
            Get
                Return _idGestionVenta
            End Get
            Set(value As Long)
                _idGestionVenta = value
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

        Public Property FechaRegistro As DateTime
            Get
                Return _fechaRegistro
            End Get
            Set(value As DateTime)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdUsuarioAsesor As Integer
            Get
                Return _idUsuarioAsesor
            End Get
            Set(value As Integer)
                _idUsuarioAsesor = value
            End Set
        End Property

        Public Property IdProducto As Integer
            Get
                Return _idProducto
            End Get
            Set(value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property Producto As String
            Get
                Return _producto
            End Get
            Set(value As String)
                _producto = value
            End Set
        End Property

        Public Property TipoProducto As String
            Get
                Return _tipoProducto
            End Get
            Set(value As String)
                _tipoProducto = value
            End Set
        End Property

        Public Property IdValorPrima As Integer
            Get
                Return _idValorPrima
            End Get
            Set(value As Integer)
                _idValorPrima = value
            End Set
        End Property

        Public Property ValorPrima As String
            Get
                Return _valorPrima
            End Get
            Set(value As String)
                _valorPrima = value
            End Set
        End Property

        Public Property ProductoTipoServicio As String
            Get
                Return _productoTipoServicio
            End Get
            Set(value As String)
                _productoTipoServicio = value
            End Set
        End Property

        Public Property Observacion As String
            Get
                Return _observacion
            End Get
            Set(value As String)
                _observacion = value
            End Set
        End Property

        Public Property NumeroIdentificacion As String
            Get
                Return _numeroIdentificacion
            End Get
            Set(value As String)
                _numeroIdentificacion = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idDetalle As Long)
            MyBase.New()
            _idDetalle = idDetalle
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idDetalle > 0 Then .SqlParametros.Add("@listIdDetalle", SqlDbType.Int).Value = CStr(_idDetalle)
                    .ejecutarReader("ObtenerInfoGestionVentaDetalle", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                        .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                        .Add("@idValorPrima", SqlDbType.Int).Value = _idValorPrima
                        .Add("@valorCupo", SqlDbType.Int).Value = ValorPrima
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .TiempoEsperaComando = 0
                    .EjecutarNonQuery("RegistraGestionDeVentaDetalle", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        .ConfirmarTransaccion()
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se presento un error al realizar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function Eliminar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idDetalle", SqlDbType.BigInt).Value = _idDetalle
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("EliminaGestionDeVentaDetalle", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        .ConfirmarTransaccion()
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se presento un error al realizar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Long.TryParse(reader("idDetalle"), _idDetalle)
                    Long.TryParse(reader("idGestionVenta"), _idGestionVenta)
                    If Not IsDBNull(reader("idCliente")) Then Integer.TryParse(reader("idCliente"), _idCliente)
                    If Not IsDBNull(reader("fechaRegistro")) Then _fechaRegistro = CDate(reader("fechaRegistro").ToString)
                    If Not IsDBNull(reader("idUsuarioAsesor")) Then Integer.TryParse(reader("idUsuarioAsesor"), _idUsuarioAsesor)
                    If Not IsDBNull(reader("idProducto")) Then Integer.TryParse(reader("idProducto"), _idProducto)
                    If Not IsDBNull(reader("producto")) Then _producto = (reader("producto").ToString)
                    If Not IsDBNull(reader("tipoProducto")) Then _tipoProducto = (reader("tipoProducto").ToString)
                    If Not IsDBNull(reader("idValorPrima")) Then Integer.TryParse(reader("idValorPrima"), _idValorPrima)
                    If Not IsDBNull(reader("valorPrimaServicio")) Then _valorPrima = (reader("valorPrimaServicio").ToString)
                    If Not IsDBNull(reader("observacion")) Then _observacion = (reader("observacion").ToString)
                    If Not IsDBNull(reader("productoTipoServicio")) Then _productoTipoServicio = (reader("productoTipoServicio").ToString)
                    If Not IsDBNull(reader("numeroIdentificacion")) Then _numeroIdentificacion = (reader("numeroIdentificacion").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
