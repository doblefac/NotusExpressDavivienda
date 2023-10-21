Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class InsertarTemporalNovedades

#Region "Atributos"

    Private _idGestionVenta As Integer
    Private _idTipoNovedad As Integer
    Private _observacionNovedad As String
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

    Public Property IdTipoNovedad As Integer
        Get
            Return _idTipoNovedad
        End Get
        Set(value As Integer)
            _idTipoNovedad = value
        End Set
    End Property

    Public Property ObservacionNovedad As String
        Get
            Return _observacionNovedad
        End Get
        Set(value As String)
            _observacionNovedad = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Registrar(ByVal _numeroIdentificacion As String) As Integer
        Dim resultado As Integer
        Dim dbManager As New LMDataAccess
        Try
            dbManager.iniciarTransaccion()
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                .SqlParametros.Add("@idTipoNovedad", SqlDbType.Int).Value = _idTipoNovedad
                .SqlParametros.Add("@observacionNovedad", SqlDbType.VarChar, 2000).Value = _observacionNovedad
                .ejecutarNonQuery("InsertarTablaTemporalNovedades", CommandType.StoredProcedure)
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
