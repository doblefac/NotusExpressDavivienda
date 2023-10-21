Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ClienteFinalProducto

#Region "Atributos"

    Private _idRegistro As Long
    Private _idCliente As Integer
    Private _idProducto As Integer
    Private _producto As String
    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdRegistro As Long
        Get
            Return _idRegistro
        End Get
        Set(value As Long)
            _idRegistro = value
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

    Public Property IdProducto As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property NombreProducto As String
        Get
            Return _producto
        End Get
        Set(value As String)
            _producto = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idCliente As Integer)
        Me.New()
        _idCliente = idCliente
        CargarDatos()
    End Sub

    Public Sub New(ByVal idProducto As String)
        Me.New()
        _idProducto = idProducto
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If _idCliente > 0 OrElse _idProducto > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        If _idCliente > 0 Then .Add("@listaIdCliente", SqlDbType.VarChar, 2000).Value = CStr(_idCliente)
                        If _idProducto > 0 Then .Add("@listaIdProducto", SqlDbType.VarChar, 2000).Value = CStr(_idProducto)
                    End With
                    .ejecutarReader("ObtenerClienteFinalProducto", CommandType.StoredProcedure)
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
                Long.TryParse(reader("idRegistro"), _idRegistro)
                Integer.TryParse(reader("idCliente"), _idCliente)
                Integer.TryParse(reader("idProducto"), _idProducto)
                If Not String.IsNullOrEmpty(reader("producto")) Then _producto = (reader("producto"))
            End If
        End If
    End Sub

#End Region

End Class
