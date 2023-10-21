Imports LMDataAccessLayer

Namespace WMS

    Public Class ReporteInventarioSerializado
#Region "Atributos"

        Private _idCiudad As Integer
        Private _ciudad As String
        Private _listaPdv As ArrayList
        Private _idProducto As Integer
        Private _producto As String
        Private _idSubproducto As Integer
        Private _subproducto As String
        Private _fechaInicial As Date
        Private _fechaFinal As Date
        Private _idEstado As Integer
        Private _datosReporte As DataTable
        Private _cargado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            _idProducto = 0
            _idCiudad = 0
            _listaPdv = New ArrayList
            _idSubproducto = 0
            _idEstado = 0
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdCiudad As Integer
            Get
                Return _idCiudad
            End Get
            Set(value As Integer)
                _idCiudad = value
            End Set
        End Property

        Public Property Ciudad As String
            Get
                Return _ciudad
            End Get
            Set(value As String)
                _ciudad = value
            End Set
        End Property

        Public Property ListaPdv As ArrayList
            Get
                If _listaPdv Is Nothing Then _listaPdv = New ArrayList
                Return _listaPdv
            End Get
            Set(value As ArrayList)
                _listaPdv = value
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

        Public Property IdSubproducto As Integer
            Get
                Return _idSubproducto
            End Get
            Set(value As Integer)
                _idSubproducto = value
            End Set
        End Property

        Public Property Subproducto As String
            Get
                Return _subproducto
            End Get
            Set(value As String)
                _subproducto = value
            End Set
        End Property

        Public Property FechaInicial() As Date
            Get
                Return _fechaInicial
            End Get
            Set(ByVal value As Date)
                _fechaInicial = value
            End Set
        End Property

        Public Property FechaFinal() As Date
            Get
                Return _fechaFinal
            End Get
            Set(ByVal value As Date)
                _fechaFinal = value
            End Set
        End Property

        Public Property IdEstado As Integer
            Get
                Return _idEstado
            End Get
            Set(value As Integer)
                _idEstado = value
            End Set
        End Property

        Public ReadOnly Property DatosReporte As DataTable
            Get
                If Not _cargado OrElse _datosReporte Is Nothing Then CargarDatos()
                Return _datosReporte
            End Get
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub CargarDatos()
            Dim dbManager As LMDataAccess = Nothing
            Try
                dbManager = New LMDataAccess
                Dim dsAux As DataSet
                With dbManager
                    If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = Me._idCiudad
                    If _listaPdv IsNot Nothing AndAlso _listaPdv.Count > 0 Then _
                        .SqlParametros.Add("@listaPdv", SqlDbType.VarChar).Value = Join(_listaPdv.ToArray, ",")
                    If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = Me._idProducto
                    If _idSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = Me._idSubproducto
                    If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = Me._idEstado
                    If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    End If
                    dsAux = .EjecutarDataSet("ReporteInventarioSerializado", CommandType.StoredProcedure)
                    If dsAux IsNot Nothing AndAlso dsAux.Tables.Count > 0 Then
                        _datosReporte = dsAux.Tables(0)
                    End If
                    _cargado = True
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace
