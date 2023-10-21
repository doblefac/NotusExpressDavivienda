Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ObtenerIdEstrategia

#Region "Atributos"

    Private _idEstrategia As Integer

#End Region

#Region "Propiedades"

    
    Public Property IdEstrategiaCliente() As Integer
        Get
            Return _idEstrategia
        End Get
        Set(ByVal value As Integer)
            _idEstrategia = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()

    End Sub

    Public Sub New(ByVal idEstrategiaC As Integer)
        Me.New()

        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                .ejecutarReader("ObtenerIdEstrategia", CommandType.StoredProcedure)
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
                Integer.TryParse(reader("idEstrategia").ToString, _idEstrategia)
            End If
        End If

    End Sub


#End Region
End Class
