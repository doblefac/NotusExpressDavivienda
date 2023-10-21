Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class EliminarNovedad
#Region "Atributos"

    Private _idNovedad As Integer
    
#End Region

#Region "Propiedades"

    Public Property IdNovedad() As Integer
        Get
            Return _idNovedad
        End Get
        Set(ByVal value As Integer)
            _idNovedad = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal IdNovedad As Integer)
        Me.New()
        _idNovedad = IdNovedad
        Eliminar()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub Eliminar()
        If Not String.IsNullOrEmpty(_idNovedad) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idNovedad", SqlDbType.VarChar).Value = _idNovedad
                    .TiempoEsperaComando = 600
                    .ejecutarNonQuery("EliminarNovedadPorTransaccion", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region
End Class
