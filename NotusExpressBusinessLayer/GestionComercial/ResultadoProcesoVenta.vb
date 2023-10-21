Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ResultadoProcesoVenta

#Region "Atributos"

    Private _idResultado As Byte
    Private _descripcion As String
    Private _solicitarReferido As Boolean
    Private _idEstado As Byte
    Private _fechaCreacion As Date
    Private _idCreador As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _descripcion = ""
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        _idResultado = identificador
        CargarDatos()
    End Sub
#End Region

#Region "Propiedades"

    Public Property IdResultado() As Byte
        Get
            Return _idResultado
        End Get
        Protected Friend Set(ByVal value As Byte)
            _idResultado = value
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

    Public Property SolicitarReferido() As Boolean
        Get
            Return _solicitarReferido
        End Get
        Set(ByVal value As Boolean)
            _solicitarReferido = value
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

    Public Property FechaCreacion() As Date
        Get
            Return _fechaCreacion
        End Get
        Protected Friend Set(ByVal value As Date)
            _fechaCreacion = value
        End Set
    End Property

    Public Property IdCreador() As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal value As Integer)
            _idCreador = value
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
        If _idResultado > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idResultado > 0 Then .SqlParametros.Add("@idResultado", SqlDbType.TinyInt).Value = _idResultado
                    .ejecutarReader("ObtenerInfoResultadoProcesoVenta", CommandType.StoredProcedure)
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
                Byte.TryParse(reader("idResultado").ToString, _idResultado)
                _descripcion = reader("descripcion").ToString
                _solicitarReferido = CBool(reader("solicitarReferido").ToString)
                Byte.TryParse(reader("idEstado").ToString, _idEstado)
                Date.TryParse(reader("fechaCreacion").ToString, _fechaCreacion)
                Integer.TryParse(reader("idCreador").ToString, _idCreador)
                _registrado = True
            End If
        End If

    End Sub

#End Region

End Class
