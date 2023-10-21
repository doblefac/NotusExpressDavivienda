Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class HistoriaTransacciones

#Region "Atributos"

    Private _numeroIdentificacion As String
    Private _datosReporte As DataTable

#End Region

#Region "Propiedades"

    Public Property NumeroIdentificacion() As String
        Get
            Return _numeroIdentificacion
        End Get
        Set(ByVal value As String)
            _numeroIdentificacion = value
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
        _numeroIdentificacion = ""

    End Sub

    Public Sub New(ByVal numeroIdentificacion As String)
        Me.New()
        _numeroIdentificacion = numeroIdentificacion
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If Not String.IsNullOrEmpty(_numeroIdentificacion) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar).Value = _numeroIdentificacion
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ObtenerInfoClienteTransaccionHistoria", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region

End Class
