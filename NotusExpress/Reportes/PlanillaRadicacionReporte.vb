Imports LMDataAccessLayer
Imports System.Collections.Generic

Public Class PlanillaRadicacionReporte
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ObtenerListadoPlanillasRadicacionReporteTableAdapter.Connection = New LMDataAccess().ConexionSQL
    End Sub

    Public Sub EstablecerParametro(ByVal numeroRadicado As Integer)
        Try
            'DsPlanillaRadicacion1.Clear()
            ObtenerListadoPlanillasRadicacionReporteTableAdapter.Fill(DsPlanillaRadicacion1.ObtenerListadoPlanillasRadicacionReporte, numeroRadicado)
        Catch ex As Exception

        End Try
    End Sub
End Class