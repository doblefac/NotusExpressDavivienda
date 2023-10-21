Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class InsertarTemporalDestruirSerial
#Region "Atributos"

    Private _idGestionVenta As Integer
    Private _serial As String
    Private _usuarioDestruccion As String
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdGestionVenta As Integer
        Get
            Return _idGestionVenta
        End Get
        Set(value As Integer)
            _idGestionVenta = value
        End Set
    End Property

    Public Property serial As String
        Get
            Return _serial
        End Get
        Set(value As String)
            _serial = value
        End Set
    End Property

    Public Property UsuarioDestruccion As String
        Get
            Return _usuarioDestruccion
        End Get
        Set(value As String)
            _usuarioDestruccion = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As Integer
        Dim resultado As Integer
        Dim dbManager As New LMDataAccess
        Try
            dbManager.iniciarTransaccion()
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                .SqlParametros.Add("@usuarioDestruccion", SqlDbType.VarChar, 2000).Value = _usuarioDestruccion
                .ejecutarNonQuery("InsertarTablaTemporalSerialesDestruir", CommandType.StoredProcedure)
                .confirmarTransaccion()
                resultado = 1
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
            resultado = 0
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

#End Region
End Class
