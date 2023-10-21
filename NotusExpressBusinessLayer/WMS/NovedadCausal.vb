Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.WMS

Public Class NovedadCausal
#Region "Atributos"

    Private _idNovedad As Integer
    Private _descripcion As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idNovedad As Integer)
        Me.New()
        Me._idNovedad = idNovedad
        CargarInfoNovedad()
    End Sub

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

    Private Sub CargarInfoNovedad()
        If Me._idNovedad > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .ejecutarReader("ObtenerInfoNovedadCausal", CommandType.StoredProcedure)
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
            Integer.TryParse(reader("idNovedad").ToString, Me._idNovedad)
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
