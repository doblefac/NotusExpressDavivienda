Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ConsultarNovedades

#Region "Atributos"

    Private _idGestionVenta As Integer
    Private _datosReporte As DataTable

#End Region

#Region "Propiedades"

    Public Property IdGestionVenta() As integer
        Get
            Return _idGestionVenta
        End Get
        Set(ByVal value As Integer)
            _idGestionVenta = value
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

    Public Sub New(ByVal IdGestionVenta As Integer)
        Me.New()
        _idGestionVenta = IdGestionVenta
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If Not String.IsNullOrEmpty(_idGestionVenta) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idGestionVenta", SqlDbType.VarChar).Value = _idGestionVenta
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ObtenerInfoNovedadesPorTransaccion", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region
End Class
