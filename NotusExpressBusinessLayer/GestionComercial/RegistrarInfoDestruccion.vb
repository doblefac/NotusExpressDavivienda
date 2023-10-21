Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class RegistrarInfoDestruccion
#Region "Atributos"

    Private _usuarioDestruccion As Integer
    Private _idActa As Integer
    Private _motivoDestruccion As String
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

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

    Public Property IdActa() As Integer
        Get
            Return _idActa
        End Get
        Set(ByVal value As Integer)
            _idActa = value
        End Set
    End Property

    Public Property MotivoDestruccion() As String
        Get
            Return _motivoDestruccion
        End Get
        Set(ByVal value As String)
            _motivoDestruccion = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                dbManager.iniciarTransaccion()
                With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@usuarioDestruccion", SqlDbType.Int).Value = _usuarioDestruccion
                .SqlParametros.Add("@motivoDestruccion", SqlDbType.VarChar, 2000).Value = _motivoDestruccion
                .SqlParametros.Add("@idActa", SqlDbType.Int).Direction = ParameterDirection.Output
                .ejecutarNonQuery("RegistrarInfoDestruccion", CommandType.StoredProcedure)

                .confirmarTransaccion()
                _idActa = CInt(.SqlParametros("@idActa").Value.ToString)
                End With

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        
        Return resultado
    End Function

#End Region
End Class
