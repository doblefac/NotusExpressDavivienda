Imports LMDataAccessLayer

Public Class EstatusLaboral

#Region "Atributos"

    Private _idEstatusLaboral As Byte
    Private _descripcion As String
    Private _idEstado As Byte
    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdEstatusLaboral() As Byte
        Get
            Return _idEstatusLaboral
        End Get
        Protected Friend Set(ByVal value As Byte)
            _idEstatusLaboral = value
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

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _descripcion = ""
    End Sub

    Public Sub New(ByVal idEstatus As Byte)
        Me.New()
        _idEstatusLaboral = idEstatus
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If _idEstatusLaboral > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idEstatusLaboral > 0 Then .SqlParametros.Add("@idEstatusLaboral", SqlDbType.TinyInt).Value = _idEstatusLaboral
                    .ejecutarReader("ObtenerListaEstatusLaboral", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Byte.TryParse(reader("idEstatusLaboral").ToString, _idEstatusLaboral)
                _descripcion = reader("descripcion").ToString
                Byte.TryParse(reader("idEstado").ToString, _idEstado)
                _registrado = True
            End If
        End If

    End Sub

#End Region

End Class
