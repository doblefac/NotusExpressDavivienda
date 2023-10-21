Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ListadoEstrategiaRegistro
#Region "Atributos"

    Private _idEstrategia As Integer
    Private _nombre As String
    Private _cargado As Boolean
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _nombre = ""
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        _idEstrategia = identificador
        CargarDatos()
    End Sub
#End Region

#Region "Propiedades"

    Public Property IdEstrategia() As Integer
        Get
            Return _idEstrategia
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idEstrategia = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()

        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .ejecutarReader("ObtenerListadoEstrategiaComercial", CommandType.StoredProcedure)
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
                Byte.TryParse(reader("idEstrategia").ToString, _idEstrategia)
                _nombre = reader("nombre").ToString
                _registrado = True
            End If
        End If

    End Sub

#End Region
End Class
