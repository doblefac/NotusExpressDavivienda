Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class VentasConNovedadGeneral
#Region "Atributos"

    Private _datosNovedades As DataTable

#End Region

#Region "Propiedades"

    Public ReadOnly Property DatosNovedades() As DataTable
        Get
            If _datosNovedades Is Nothing Then CargarDatos()
            Return _datosNovedades
        End Get
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                .TiempoEsperaComando = 600
                _datosNovedades = .ejecutarDataTable("VentasConNovedades", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
    End Sub

#End Region
End Class
