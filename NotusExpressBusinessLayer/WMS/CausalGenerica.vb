Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.WMS

Public Class CausalGenerica
#Region "Atributos"

    Private _idCausal As Integer
    Private _descripcion As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idCausal As Integer)
        Me.New()
        Me._idCausal = idCausal
        CargarInfoCausal()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdCausal() As Integer
        Get
            Return _idCausal
        End Get
        Set(ByVal value As Integer)
            _idCausal = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property
#End Region

#Region "Métodos Privados"

    Private Sub CargarInfoCausal()
        If Me._idCausal > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .ejecutarReader("ObtenerInfoCausalGenerica", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then AsignarValorAPropiedades(.Reader)
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            Integer.TryParse(reader("idCausal").ToString, Me._idCausal)
            Me._descripcion = reader("descripcion").ToString
        End If

    End Sub

    Protected Friend Overloads Function Registrar(ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

    Protected Friend Overloads Function Actualizar(ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

#End Region

End Class
