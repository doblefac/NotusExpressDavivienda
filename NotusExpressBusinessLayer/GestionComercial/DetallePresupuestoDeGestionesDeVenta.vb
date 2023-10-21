Imports LMDataAccessLayer

Namespace PresupuestoGestiones

    Public Class DetallePresupuestoDeGestionesDeVenta

#Region "Atributos"

        Private _idDetalle As Integer
        Private _idPresupuesto As Integer
        Private _idResultadoProceso As Short
        Private _idTipoVenta As Short
        Private _cantidad As Integer
        Private _idClasificacionGestion As Byte
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            _idDetalle = identificador
            CargarDatos()
        End Sub

        Public Sub New(ByVal idResultadoProceso As Short, ByVal cantidad As Integer, ByVal idClasificacionGestion As Byte, Optional ByVal idTipoVenta As Short = 0)
            _idResultadoProceso = idResultadoProceso
            _idTipoVenta = idTipoVenta
            _cantidad = cantidad
            _idClasificacionGestion = idClasificacionGestion
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdDetalle As Integer            Get                Return _idDetalle            End Get            Set(ByVal value As Integer)                _idDetalle = value            End Set        End Property
        Public Property IdPresupuesto As Integer            Get                Return _idPresupuesto            End Get            Set(ByVal value As Integer)                _idPresupuesto = value            End Set        End Property
        Public Property IdResultadoProceso As Short            Get                Return _idResultadoProceso            End Get            Set(ByVal value As Short)                _idResultadoProceso = value            End Set        End Property
        Public Property IdTipoVenta As Short            Get                Return _idTipoVenta            End Get            Set(ByVal value As Short)                _idTipoVenta = value            End Set        End Property
        Public Property Cantidad As Integer            Get                Return _cantidad            End Get            Set(ByVal value As Integer)                _cantidad = value            End Set        End Property

        Public Property IdClasificacionGestion As Byte
            Get
                Return _idClasificacionGestion
            End Get
            Set(value As Byte)
                _idClasificacionGestion = value
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

        Private Sub CargarDatos()
            If _idDetalle > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = _idDetalle
                        .ejecutarReader("ObtenerDetallePresupuestoDeGestionesDeVenta", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idDetalle").ToString, _idDetalle)
                    Integer.TryParse(reader("idPresupuesto").ToString, _idPresupuesto)
                    Short.TryParse(reader("idResultadoProceso").ToString, _idResultadoProceso)
                    Short.TryParse(reader("idTipoVenta").ToString, _idTipoVenta)
                    Integer.TryParse(reader("cantidad").ToString, _cantidad)
                    Byte.TryParse(reader("idClasificacionGestion").ToString, _idClasificacionGestion)
                    _registrado = True
                End If
            End If
        End Sub
#End Region

    End Class

End Namespace