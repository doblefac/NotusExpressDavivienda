Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ConfirmarDestruccionSeriales
#Region "Atributos"

    Private _numeroActaDestruccion As Integer
    Private _usuarioDestruccion As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property NumeroActaDestruccion() As Integer
        Get
            Return _numeroActaDestruccion
        End Get
        Set(ByVal value As Integer)
            _numeroActaDestruccion = value
        End Set
    End Property

    Public Property UsuarioDestruccion() As Integer
        Get
            Return _usuarioDestruccion
        End Get
        Set(ByVal value As Integer)
            _usuarioDestruccion = value
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
        If _numeroActaDestruccion > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                dbManager.iniciarTransaccion()
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@usuarioDestruccion", SqlDbType.Int).Value = _usuarioDestruccion
                    .SqlParametros.Add("@numeroActaDestruccion", SqlDbType.Int).Value = _numeroActaDestruccion
                    .ejecutarNonQuery("ConfirmarDestruccionSeriales", CommandType.StoredProcedure)

                    .confirmarTransaccion()

                End With

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para confirmar la destrucción de los seriales. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region
End Class
