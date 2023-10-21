Imports LMDataAccessLayer

Public Class TipoPersona

#Region "Atributos (Campos)"

    Private _idTipoPersona As Integer
    Private _descripcion As String
    Private _idEstado As Byte
    Private _fechaCreacion As Date
    Private _idCreador As Integer
    Private _creador As String
    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property idTipoPersona() As Integer
        Get
            Return _idTipoPersona
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idTipoPersona = value
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

    Public Property Creador() As String
        Get
            Return _creador
        End Get
        Protected Friend Set(ByVal value As String)
            _creador = value
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

#Region "Constructores"

    Sub New()
        MyBase.New()
        _idTipoPersona = 0
        _descripcion = ""
         _creador = ""

    End Sub

    Sub New(ByVal idTipoPersona As Integer)
        MyBase.New()
        _idTipoPersona = idTipoPersona
        Me.CargarInformacion()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        If _idTipoPersona > 0 Then
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idTipoPersona", SqlDbType.Int).Value = _idTipoPersona
                    .ejecutarReader("ObtenerInfoTipoPersona", CommandType.StoredProcedure)
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

                Integer.TryParse(reader("idTipoPersona").ToString, _idTipoPersona)
                _descripcion = reader("descripcion").ToString
                Byte.TryParse(reader("idEstado").ToString, _idEstado)
                Date.TryParse(reader("fechaCreacion").ToString, _fechaCreacion)
                Integer.TryParse(reader("idCreador").ToString, _idCreador)
                _creador = reader("creador").ToString
                _registrado = True
            End If
        End If

    End Sub

#End Region
End Class
