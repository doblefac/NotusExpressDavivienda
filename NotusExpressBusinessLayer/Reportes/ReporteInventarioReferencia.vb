Imports LMDataAccessLayer

Namespace WMS

    Public Class ReporteInventarioReferencia
#Region "Atributos"

        Private _idCiudad As Integer
        Private _ciudad As String
        Private _listaPdv As ArrayList
        Private _idProducto As Integer
        Private _producto As String
        Private _idSubproducto As Integer
        Private _subproducto As String
        Private _numDiasInventario As Short
        Private _datosReporte As DataTable
        Private _cargado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            _idProducto = 0
            _idCiudad = 0
            _listaPdv = New ArrayList
            _idSubproducto = 0
            _numDiasInventario = 1
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

        Public Property NumDiasInventario As Short
            Get
                Return _numDiasInventario
            End Get
            Set(value As Short)
                _numDiasInventario = value
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
                    If _numDiasInventario > 0 Then .SqlParametros.Add("@numDiasInventario", SqlDbType.SmallInt).Value = Me._numDiasInventario
                    dsAux = .EjecutarDataSet("ReporteInventarioReferencia", CommandType.StoredProcedure)
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