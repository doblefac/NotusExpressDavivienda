Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ListadoEstados

#Region "Atributos"

    Private _descripcion As String
    Private _idEstado As Integer
    Private _idEntidad As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _descripcion = ""
    End Sub

    Public Sub New(Optional identificador As Integer = Nothing, Optional idEntidad As Integer = Nothing)
        Me.New()
        _idEstado = identificador
        _idEntidad = idEntidad
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

    Public Property IdEntidad() As Integer
        Get
            Return _idEntidad
        End Get
        Set(ByVal value As Integer)
            _idEntidad = value
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
                If _idEntidad > 0 Then .SqlParametros.Add("@idEntidad", SqlDbType.Int).Value = _idEntidad
                .ejecutarReader("ObtenerInfoEstados", CommandType.StoredProcedure)
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

#Region "Métodos Públicos"
    Public Function ObtenerDatos() As DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = _idEstado
            If _idEntidad > 0 Then .SqlParametros.Add("@idEntidad", SqlDbType.Int).Value = _idEntidad
            Return dbManager.EjecutarDataTable("ObtenerInfoEstados", CommandType.StoredProcedure)
        End With
    End Function
#End Region

End Class
