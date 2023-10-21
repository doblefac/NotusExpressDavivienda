Imports LMDataAccessLayer

Public Class UnidadDeNegocio


#Region "Atributos (Campos)"

    Private _idUnidadNegocio As Integer
    Private _nombre As String
    Private _codigo As String
    Private _idEstado As Byte
    Private _idEmpresa As Integer
    Private _fechaCreacion As Date
    Private _idCreador As Integer
    Private _creador As String
    Private _registrado As Boolean

#End Region
#Region "Propiedades"

    Public Property IdUnidadNegocio() As Integer
        Get
            Return _idUnidadNegocio
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idUnidadNegocio = value
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

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
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

    Public Property IdEmpresa() As Short
        Get
            Return _idEmpresa
        End Get
        Set(ByVal value As Short)
            _idEmpresa = value
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

#Region "Métodos Protegidos"
    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then

                Integer.TryParse(reader("idUnidadNegocio").ToString, _idUnidadNegocio)
                _nombre = reader("nombre").ToString
                _codigo = reader("codigo").ToString
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
