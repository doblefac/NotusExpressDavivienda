Imports LMDataAccessLayer
Imports System.Web

Public Class PuntosDeVentaEC

#Region "Atributos"

    Private _idPdv As Integer
    Private _nombre As String
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _nombre = ""
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        Me._idPdv = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdPdv As Integer
        Get
            Return _idPdv
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idPdv = value
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
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                .ejecutarReader("ObtenerInfoPdvEC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            AsignarValorAPropiedades(.Reader)
                        End If
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idPdv").ToString, Me._idPdv)
                Me._nombre = reader("nombre").ToString
                Me._registrado = True
            End If
        End If
    End Sub

#End Region
End Class
