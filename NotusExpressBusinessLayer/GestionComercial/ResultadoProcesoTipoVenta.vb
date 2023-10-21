Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ResultadoProcesoTipoVenta

#Region "Atributos"

    Private _idRelacion As Short
    Private _idResultadoProceso As Byte
    Private _idTipoVenta As Byte
    Private _tipoVenta As String
    Private _requiereEntregarProducto As Boolean
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _tipoVenta = ""
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        _idRelacion = identificador
        CargarDatos()
    End Sub
#End Region

#Region "Propiedades"

    Public Property IdRelacion() As Byte
        Get
            Return _idResultadoProceso
        End Get
        Protected Friend Set(ByVal value As Byte)
            _idResultadoProceso = value
        End Set
    End Property

    Public Property IdResultadoProceso() As Byte
        Get
            Return _idResultadoProceso
        End Get
        Set(ByVal value As Byte)
            _idResultadoProceso = value
        End Set
    End Property

    Public Property IdTipoVenta() As Byte
        Get
            Return _idTipoVenta
        End Get
        Protected Friend Set(ByVal value As Byte)
            _idTipoVenta = value
        End Set
    End Property

    Public Property TipoVenta() As String
        Get
            Return _tipoVenta
        End Get
        Set(ByVal value As String)
            _tipoVenta = value
        End Set
    End Property

    Public Property RequiereEntregarProductoProducto() As Boolean
        Get
            Return _requiereEntregarProducto
        End Get
        Protected Friend Set(ByVal value As Boolean)
            _requiereEntregarProducto = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If _idRelacion > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idRelacion", SqlDbType.TinyInt).Value = _idRelacion
                    .ejecutarReader("ObtenerInfoResultadoProcesoTipoDeVenta", CommandType.StoredProcedure)
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
                Short.TryParse(reader("idRelacion").ToString, _idRelacion)
                Byte.TryParse(reader("idResultadoProceso").ToString, _idResultadoProceso)
                Byte.TryParse(reader("idTipoVenta").ToString, _idTipoVenta)
                _requiereEntregarProducto = CBool(reader("requiereEntregarProducto").ToString)
                _tipoVenta = reader("tipoVenta").ToString
                _registrado = True
            End If
        End If

    End Sub

#End Region

End Class