Imports LMDataAccessLayer
Imports System.Collections.Generic

Public Class ActaDestruccionSeriales

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ActaDestruccionTableAdapter.Connection = New LMDataAccess().ConexionSQL
    End Sub

    Public Sub EstablecerParametro(ByVal numeroActa As Integer)
        Try
            DsActaDestruccion1.Clear()
            ActaDestruccionTableAdapter.Fill(DsActaDestruccion1.ActaDestruccion, numeroActa)
        Catch ex As Exception

        End Try
    End Sub

End Class