Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class EstadosSeriales
#Region "Atributos"

    Private _descripcion As String
    Private _idEstado As Byte
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _descripcion = ""
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        _idEstado = identificador
        CargarDatos()
    End Sub
#End Region

#Region "Propiedades"

    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    Public Property IdEstado() As Byte
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Byte)
            _idEstado = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = _idEstado
                    .ejecutarReader("ObtenerInfoEstadosSeriales", CommandType.StoredProcedure)
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
                _descripcion = reader("descripcion").ToString
                Byte.TryParse(reader("idEstado").ToString, _idEstado)
                _registrado = True
            End If
        End If

    End Sub

#End Region

End Class
