Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ConsultarSerialesConNovedadInventario

#Region "Atributos"

    Private _datosReporte As DataTable

#End Region

#Region "Propiedades"
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
        CargarDatos()

    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .TiempoEsperaComando = 600
                _datosReporte = .ejecutarDataTable("ObtenerSerialesConNovedadInventario", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region
End Class
