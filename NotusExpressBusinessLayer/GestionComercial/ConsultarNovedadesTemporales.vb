Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ConsultarNovedadesTemporales
#Region "Atributos"

    Private _idCliente As Integer
    Private _datosReporte As DataTable

#End Region

#Region "Propiedades"

    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Set(ByVal value As Integer)
            _idCliente = value
        End Set
    End Property

    Public ReadOnly Property DatosReporte() As DataTable
        Get
            If _datosReporte Is Nothing Then CargarDatos()
            Return _datosReporte
        End Get
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()


    End Sub

    Public Sub New(ByVal IdCliente As Integer)
        Me.New()
        _idCliente = IdCliente
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If Not String.IsNullOrEmpty(_idCliente) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idCliente", SqlDbType.VarChar).Value = _idCliente
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ObtenerInfoNovedadesTemporales", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region
End Class
