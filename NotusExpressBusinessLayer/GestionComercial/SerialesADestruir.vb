Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class SerialesADestruir
#Region "Atributos"

    Private _usuarioDestruccion As Integer
    Private _datosReporte As DataTable

#End Region

#Region "Propiedades"

    Public Property UsuarioDestruccion() As Integer
        Get
            Return _usuarioDestruccion
        End Get
        Set(ByVal value As Integer)
            _usuarioDestruccion = value
        End Set
    End Property

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


    End Sub

    Public Sub New(ByVal UsuarioDestruccion As Integer)
        Me.New()
        _usuarioDestruccion = UsuarioDestruccion
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If Not String.IsNullOrEmpty(_usuarioDestruccion) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@usuarioDestruccion", SqlDbType.VarChar).Value = _usuarioDestruccion
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ObtenerSerialesADestruirTemporales", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region
End Class
