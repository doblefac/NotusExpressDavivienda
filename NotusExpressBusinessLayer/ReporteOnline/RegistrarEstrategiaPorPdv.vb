Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class RegistrarEstrategiaPorPdv

#Region "Atributos"

    Private _idEstrategia As Integer
    Private _idPdv As Integer
    Private _idUsuarioRegistra As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdEstrategia() As Integer
        Get
            Return _idEstrategia
        End Get
        Set(ByVal value As Integer)
            _idEstrategia = value
        End Set
    End Property

    Public Property IdPdv() As Integer
        Get
            Return _idPdv
        End Get
        Set(ByVal value As Integer)
            _idPdv = value
        End Set
    End Property

    Public Property IdUsuarioRegistra() As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
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

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            dbManager.iniciarTransaccion()
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idEstrategia", SqlDbType.SmallInt).Value = _idEstrategia
                .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv
                .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
                .ejecutarNonQuery("RegistrarEstrategiaPorPdv", CommandType.StoredProcedure)

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
