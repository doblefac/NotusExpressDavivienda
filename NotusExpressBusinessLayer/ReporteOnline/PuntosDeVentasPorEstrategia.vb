Imports LMDataAccessLayer
Imports System.Web

Public Class PuntosDeVentasPorEstrategia
#Region "Atributos"

    Private _idEstrategia As Integer
    Private _nombre As String
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _nombre = ""
        _idEstrategia = 0
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        Me._idEstrategia = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdEstrategia As Integer
        Get
            Return _idEstrategia
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idEstrategia = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        If Me._idEstrategia > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idEstrategia", SqlDbType.Int).Value = Me._idEstrategia
                    .ejecutarReader("ObtenerDetalleEstrategiaPorPdv", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            AsignarValorAPropiedades(.Reader)
                        End If
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Me._nombre = reader("nombre").ToString
                Me._registrado = True
            End If
        End If
    End Sub

#End Region
End Class
