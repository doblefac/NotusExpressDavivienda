Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ConsultarBodegaOrigen
#Region "Atributos"

    Private _serial As String
    Private _bodega As String

#End Region

#Region "Propiedades"

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property

    Public Property Bodega() As String
        Get
            Return _bodega
        End Get
        Set(ByVal value As String)
            _bodega = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        
    End Sub

    Public Sub New(ByVal serial As String)
        Me.New()
        _serial = serial
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If Len(_serial) > 1 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                    .ejecutarReader("ObtenerBodegaOrigenSerial", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
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

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                _bodega = reader("bodega").ToString
            End If
        End If

    End Sub


#End Region
End Class
