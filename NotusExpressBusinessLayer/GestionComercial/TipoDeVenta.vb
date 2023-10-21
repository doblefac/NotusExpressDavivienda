Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class TipoDeVenta

#Region "Atributos"

    Private _idTipoVenta As Byte
    Private _tipoVenta As String
    Private _idEstado As Byte
    Private _fechaCreacion As Date
    Private _idCreador As Integer
    Private _requiereEntregarProducto As Boolean
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _tipoVenta = ""
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        _idTipoVenta = identificador
        CargarDatos()
    End Sub
#End Region

#Region "Propiedades"

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

    Public Property IdEstado() As Byte
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Byte)
            _idEstado = value
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

    Public Property RequiereEntregarProducto() As Boolean
        Get
            Return _requiereEntregarProducto
        End Get
        Set(ByVal value As Boolean)
            _requiereEntregarProducto = True
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
        If _idTipoVenta > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idTipoVenta", SqlDbType.TinyInt).Value = _idTipoVenta
                    .ejecutarReader("ObtenerInfoTipoDeVenta", CommandType.StoredProcedure)
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
                Byte.TryParse(reader("idTipoVenta").ToString, _idTipoVenta)
                _tipoVenta = reader("descripcion").ToString
                Byte.TryParse(reader("idEstado").ToString, _idEstado)
                Date.TryParse(reader("fechaCreacion").ToString, _fechaCreacion)
                Integer.TryParse(reader("idCreador").ToString, _idCreador)
                _requiereEntregarProducto = CBool(reader("requiereEntregarProducto"))
                _registrado = True
            End If
        End If

    End Sub

#End Region

End Class
