Imports LMDataAccessLayer

Namespace Localizacion

    Public Class Ciudad

#Region "Campos"

        Private _idCiudad As Integer
        Private _nombre As String
        Private _departamento As String
        Private _idPais As Short
        Private _pais As String
        Private _idRegion As Short
        Private _region As String
        Private _estado As Boolean
        Private _ciudadDepartamento As String
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
            Set(ByVal value As Integer)
                _idCiudad = value
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

        Public Property Departamento() As String
            Get
                Return _departamento
            End Get
            Set(ByVal value As String)
                _departamento = value
            End Set
        End Property

        Public Property IdPais() As Short
            Get
                Return _idPais
            End Get
            Set(ByVal value As Short)
                _idPais = value
            End Set
        End Property

        Public ReadOnly Property Pais() As String
            Get
                Return _pais
            End Get
        End Property

        Public Property IdRegion() As Short
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Short)
                _idRegion = value
            End Set
        End Property

        Public ReadOnly Property Region() As String
            Get
                Return _region
            End Get
        End Property

        Public Property Activo() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property CiudadDepartamento() As String
            Get
                Return _ciudadDepartamento
            End Get
            Set(ByVal value As String)
                _ciudadDepartamento = value
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

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idCiudad = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idCiudad > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        .ejecutarReader("ObtenerInfoCiudad", CommandType.StoredProcedure)
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

#Region "Métodos Públicos"

        Public Function Registrar() As Short

        End Function

        Public Function Actualizar() As Short

        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idCiudad").ToString, _idCiudad)
                    _nombre = reader("nombre").ToString
                    _departamento = reader("departamento").ToString
                    _estado = reader("estado")
                    _ciudadDepartamento = reader("ciudadDepartamento").ToString
                    _registrado = True
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace


