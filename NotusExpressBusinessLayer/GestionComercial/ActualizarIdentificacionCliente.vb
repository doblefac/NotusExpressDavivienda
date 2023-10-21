Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ActualizarIdentificacionCliente

#Region "Atributos"

    Private _idCliente As Integer
    Private _numeroIdentificacion As String
    Private _registrado As Boolean
    
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Set(ByVal value As Integer)
            _idCliente = value
        End Set
    End Property

    Public Property NumeroIdentificacion() As String
        Get
            Return _numeroIdentificacion
        End Get
        Set(ByVal value As String)
            _numeroIdentificacion = value
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
        If _idCliente > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                dbManager.iniciarTransaccion()
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = _idCliente
                    .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = _numeroIdentificacion
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
                    .ejecutarNonQuery("ActualizarIdentificacionCliente", CommandType.StoredProcedure)

                    .confirmarTransaccion()

                End With

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para realizar el registro de la preventa. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region

End Class
