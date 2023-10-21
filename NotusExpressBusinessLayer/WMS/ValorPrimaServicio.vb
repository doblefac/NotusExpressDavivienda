Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.WMS

Public Class ValorPrimaServicio

#Region "Atributos"

    Private _idValorPrimaServicio As Integer
    Private _valorPrimaServicio As Integer
    Private _idTipoProducto As Integer
    Private _cargado As Boolean
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        _idValorPrimaServicio = identificador
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdValorPrimaServicio() As Integer
        Get
            Return _idValorPrimaServicio
        End Get
        Set(ByVal value As Integer)
            _idValorPrimaServicio = value
        End Set
    End Property

    Public Property ValorPrimaServicio() As Integer
        Get
            Return _valorPrimaServicio
        End Get
        Set(ByVal value As Integer)
            _valorPrimaServicio = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .ejecutarReader("ObtenerInfoValorPrima", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("IdValorPrimaServicio").ToString, Me._idValorPrimaServicio)
                Integer.TryParse(reader("ValorPrimaServicio").ToString, Me._valorPrimaServicio)
                Integer.TryParse(reader("idTipoProducto").ToString, Me._idTipoProducto)
                _registrado = True
            End If
        End If
    End Sub

#End Region

End Class
