Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class TipoDeNovedad

#Region "Atributos"

    Private _idTipoNovedad As Integer
    Private _descripcion As String
    Private _cargado As Boolean
    Private _registrado As Boolean
    
#End Region

#Region "Constructores"

    Public Sub New()
        _descripcion = ""
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        _idTipoNovedad = identificador
        CargarDatos()
    End Sub
#End Region

#Region "Propiedades"

    Public Property IdTipoNovedad() As Integer
        Get
            Return _idTipoNovedad
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idTipoNovedad = value
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

    Private Sub CargarDatos()

        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .ejecutarReader("ObtenerInfoTipoDeNovedad", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Byte.TryParse(reader("idTipoNovedad").ToString, _idTipoNovedad)
                _descripcion = reader("descripcion").ToString
                _registrado = True
            End If
        End If

    End Sub

#End Region

End Class
