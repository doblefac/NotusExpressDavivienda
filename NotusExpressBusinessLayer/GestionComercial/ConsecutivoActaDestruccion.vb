Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ConsecutivoActaDestruccion
#Region "Atributos"

    Private _numeroActaDestruccion As Integer

#End Region

#Region "Propiedades"

    Public Property NumeroActaDestruccion() As Integer
        Get
            Return _numeroActaDestruccion
        End Get
        Protected Friend Set(ByVal value As Integer)
            _numeroActaDestruccion = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()

    End Sub

    Public Sub New(ByVal numeroActaDestruccion As Integer)
        Me.New()
        _numeroActaDestruccion = NumeroActaDestruccion
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                .ejecutarReader("ConsecutivoActaDestruccion", CommandType.StoredProcedure)
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
                Integer.TryParse(reader("numeroActaDestruccion").ToString, _numeroActaDestruccion)
            End If
        End If

    End Sub


#End Region
End Class
