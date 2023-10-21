Imports LMDataAccessLayer

Public Class Permiso

#Region "Atributos"

    Private _idPermiso As Integer
    Private _idPerfil As Integer
    Private _perfil As String
    Private _idMenu As Integer
    Private _menu As String

#End Region

#Region "Propiedades"

    Public ReadOnly Property IdPermiso As Integer
        Get
            Return _idPermiso
        End Get
    End Property

    Public Property IdPerfil() As Short
        Get
            Return _idPerfil
        End Get
        Set(ByVal value As Short)
            _idPerfil = value
        End Set
    End Property

    Public ReadOnly Property Perfil() As String
        Get
            Return _perfil
        End Get
    End Property

    Public Property IdMenu() As Short
        Get
            Return _idMenu
        End Get
        Set(ByVal value As Short)
            _idMenu = value
        End Set
    End Property

    Public ReadOnly Property Menu() As String
        Get
            Return _menu
        End Get
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _perfil = ""
        _menu = ""
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos(ByVal identificador As Integer)
        Dim dbManager As New LMDataAccess

        Try
            With dbManager
                .SqlParametros.Add("@idPermiso", SqlDbType.Int).Value = identificador
                .ejecutarReader("ObtenerListadoDePermisos", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.Read Then
                    Integer.TryParse(.Reader("idPermiso").ToString, _idPermiso)
                    Short.TryParse(.Reader("idPerfil").ToString, _idPerfil)
                    _perfil = .Reader("perfil").ToString
                    Short.TryParse(.Reader("idMenu").ToString, _idMenu)
                    _menu = .Reader("menu").ToString
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

#End Region

End Class
