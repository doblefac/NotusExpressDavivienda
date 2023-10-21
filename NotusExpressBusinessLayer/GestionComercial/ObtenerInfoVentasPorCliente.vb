Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ObtenerInfoVentasPorCliente

#Region "Atributos"

    Private _idCliente As Integer
    Private _ventas As Integer

#End Region

#Region "Propiedades"

    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idCliente = value
        End Set
    End Property

    Public Property Ventas() As Integer
        Get
            Return _ventas
        End Get
        Set(ByVal value As Integer)
            _ventas = value
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

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If _idCliente > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _idCliente
                    .ejecutarReader("ObtenerInfoVentasPorCliente", CommandType.StoredProcedure)
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
                _ventas = reader("ventas").ToString

            End If
        End If

    End Sub


#End Region

End Class
