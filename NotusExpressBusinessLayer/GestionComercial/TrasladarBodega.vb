Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class TrasladarBodega

#Region "Atributos"

    Private _idBodega As Integer
    Private _serial As String
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
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
                .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
                .ejecutarNonQuery("ActualizarBodegaSerial", CommandType.StoredProcedure)

                .confirmarTransaccion()

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
