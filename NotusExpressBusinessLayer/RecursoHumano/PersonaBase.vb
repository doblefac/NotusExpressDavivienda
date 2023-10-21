Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer

Namespace RecursoHumano

    Public MustInherit Class PersonaBase
#Region "Atributos"

        Private _idPersona As Integer
        Private _numeroIdentificacion As String
        Private _nombreApellido As String
        Private _telefonos As String
        Private _email As String
        Private _idCiudad As Integer
        Private _ciudad As String
        Private _idTipo As Short
        Private _idEstado As Short
        Private _idCargo As Short
        Private _cargo As String
        Private _idEmpresa As Short
        Private _empresa As String
        Private _fechaCreacion As Date
        Private _idCreador As Integer
        Private _fechaUltimaModificacion As Date
        Private _idModificador As Integer
        Protected _registrado As Boolean

#End Region

#Region "Constructores"


        Public Sub New()

            _numeroIdentificacion = ""
            _nombreApellido = ""
            _telefonos = ""
            _email = ""

        End Sub

#End Region

#Region "Propiedades"

        Public Property IdPersona() As Integer
            Get
                Return _idPersona
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idPersona = value
            End Set
        End Property

        Public Property NumeroIdentificacion() As String
            Get
                Return _numeroIdentificacion
            End Get
            Set(ByVal value As String)
                _numeroIdentificacion = value
            End Set
        End Property

        Public Property NombreApellido() As String
            Get
                Return _nombreApellido
            End Get
            Set(ByVal value As String)
                _nombreApellido = value
            End Set
        End Property

        Public Property Telefonos() As String
            Get
                Return _telefonos
            End Get
            Set(ByVal value As String)
                _telefonos = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
            Set(ByVal value As Integer)
                _idCiudad = value
            End Set
        End Property

        Public Property Ciudad() As String
            Get
                Return _ciudad
            End Get
            Set(ByVal value As String)
                _ciudad = value
            End Set
        End Property

        Public Property IdTipo() As Short
            Get
                Return _idTipo
            End Get
            Set(ByVal value As Short)
                _idTipo = value
            End Set
        End Property

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public Property IdCargo() As Short
            Get
                Return _idCargo
            End Get
            Set(ByVal value As Short)
                _idCargo = value
            End Set
        End Property

        Public Property Cargo As String
            Get
                Return _cargo
            End Get
            Set(value As String)
                _cargo = value
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

        Public Property Empresa As String
            Get
                Return _empresa
            End Get
            Set(value As String)
                _empresa = value
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

        Public Property FechaUltimaModificacion() As Date
            Get
                Return _fechaUltimaModificacion
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaUltimaModificacion = value
            End Set
        End Property

        Public Property IdModificador() As Integer
            Get
                Return _idModificador
            End Get
            Set(ByVal value As Integer)
                _idModificador = value
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

#Region "Métodos Portegidos"

        Protected Sub CargarInformacion()
            If Me._idPersona > 0 OrElse Not String.IsNullOrEmpty(Me._numeroIdentificacion) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idPersona > 0 Then .SqlParametros.Add("@idPersona", SqlDbType.Int).Value = Me._idPersona
                        If Not String.IsNullOrEmpty(_numeroIdentificacion) Then _
                            .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = Me._numeroIdentificacion
                        .ejecutarReader("ObtenerInfoPersona", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then AsignarValorAPropiedades(.Reader)
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        Protected Sub CargarInformacion(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                AsignarValorAPropiedades(reader)
            End If
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then

                If reader.HasRows Then
                    Integer.TryParse(reader("idPersona").ToString, Me._idPersona)
                    Me._numeroIdentificacion = reader("numeroIdentificacion").ToString
                    Me._nombreApellido = reader("nombreApellido").ToString
                    Me._telefonos = reader("telefonos").ToString
                    Me._email = reader("email").ToString
                    Integer.TryParse(reader("idCiudad").ToString, Me._idCiudad)
                    Me._ciudad = reader("ciudad").ToString
                    Short.TryParse(reader("idTipo").ToString, Me._idTipo)
                    Short.TryParse(reader("idEstado").ToString, Me._idEstado)
                    Short.TryParse(reader("idCargo").ToString, Me._idCargo)
                    Me._cargo = reader("cargo").ToString
                    Short.TryParse(reader("idEmpresa").ToString, Me._idEmpresa)
                    Me._empresa = reader("empresa").ToString
                    Me._fechaCreacion = CDate(reader("fechaCreacion"))
                    Integer.TryParse(reader("idCreador").ToString, Me._idCreador)
                    Date.TryParse(reader("fechaUltimaModificacion").ToString, _fechaUltimaModificacion)
                    Integer.TryParse(reader("idModificador").ToString, Me._idModificador)
                    Me._registrado = True
                End If
            End If
        End Sub
#End Region

#Region "Métodos Públicos"

        Public MustOverride Function Registrar() As ResultadoProceso

        Public MustOverride Function Actualizar() As ResultadoProceso
#End Region

    End Class

End Namespace
