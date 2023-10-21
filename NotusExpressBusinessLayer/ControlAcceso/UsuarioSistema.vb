Imports LMDataAccessLayer
Imports EncryptionClassLibrary.LMEncryption
Imports NotusExpressBusinessLayer.General

Namespace ControlAcceso

    Public Class UsuarioSistema

#Region "Atributos"

        Private _idUsuario As Integer
        Private _idPersona As Integer
        Private _numeroIdentificacion As String
        Private _nombreApellido As String
        Private _email As String
        Private _idTipo As Short
        Private _usuario As String
        Private _password As String
        Private _idUnidadNegocio As Integer
        Private _idCargo As Integer
        Private _idCiudad As Integer
        Private _cargo As String
        Private _idEstado As Byte
        Private _idRol As Integer
        Private _idPerfil As Integer
        Private _idEmpresa As Integer
        Private _fechaCreacion As Date
        Private _idCreador As Integer
        Private _fechaUltimaModificacion As Date
        Private _idModificador As Integer
        Private _registrado As Boolean
        Private _listaPdv As String
        Private _telefono As String
        Private _fechaIngreso As Date
        Private _estado As String
        Private _ciudad As String
        Private _rol As String
        Private _tipoPersona As String
        Private _empresa As String
        Private _fechaRetiro? As Date

#End Region

#Region "Propiedades"

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set

        End Property

        Public Property IdPersona() As Integer
            Get
                Return _idPersona
            End Get
            Set(ByVal value As Integer)
                _idPersona = value
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
        Public Property NumeroIdentificacion() As String
            Get
                Return _numeroIdentificacion
            End Get
            Set(ByVal value As String)
                _numeroIdentificacion = value
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
        Public Property NombreApellido() As String
            Get
                Return _nombreApellido
            End Get
            Set(ByVal value As String)
                _nombreApellido = value
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

        Public Property Usuario() As String
            Get
                Return _usuario
            End Get
            Set(ByVal value As String)
                _usuario = value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As Integer
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Integer)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property IdCargo() As Integer
            Get
                Return _idCargo
            End Get
            Set(ByVal value As Integer)
                _idCargo = value
            End Set
        End Property

        Public Property Cargo() As String
            Get
                Return _cargo
            End Get
            Set(ByVal value As String)
                _cargo = value
            End Set
        End Property

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property IdRol() As Integer
            Get
                Return _idRol
            End Get
            Set(ByVal value As Integer)
                _idRol = value
            End Set
        End Property
        Public Property IdEmpresa() As Integer
            Get
                Return _idEmpresa
            End Get
            Set(ByVal value As Integer)
                _idEmpresa = value
            End Set
        End Property

        Public Property IdPerfil() As Integer
            Get
                Return _idPerfil
            End Get
            Set(ByVal value As Integer)
                _idPerfil = value
            End Set
        End Property

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
            End Set
        End Property

        Public Property FechaRetiro() As Date
            Get
                Return _fechaRetiro
            End Get
            Set(ByVal value As Date)
                _fechaRetiro = value
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
            Set(ByVal value As Date)
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
            Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

        Public Property ListaPdv() As String
            Get
                Return _listaPdv
            End Get
            Set(ByVal value As String)
                _listaPdv = value
            End Set
        End Property
        Public Property Telefono() As String
            Get
                Return _telefono
            End Get
            Set(ByVal value As String)
                _telefono = value
            End Set

        End Property
        Public Property FechaIngreso() As Date
            Get
                Return _fechaIngreso
            End Get
            Set(ByVal value As Date)
                _fechaIngreso = value
            End Set
        End Property
        Public Property Estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
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

        Public Property Rol() As String
            Get
                Return _rol
            End Get
            Set(ByVal value As String)
                _rol = value
            End Set
        End Property


        Public Property TipoPersona() As String
            Get
                Return _tipoPersona
            End Get
            Set(ByVal value As String)
                _tipoPersona = value
            End Set
        End Property


        Public Property Empresa() As String
            Get
                Return _empresa
            End Get
            Set(ByVal value As String)
                _empresa = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            _numeroIdentificacion = ""
            _nombreApellido = ""
            _usuario = ""
            _password = ""
            _cargo = ""
        End Sub

        Public Sub New(ByVal idUsuario As Integer)
            Me.New()
            _idUsuario = idUsuario
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Públicos"

        ''' <summary>
        ''' Obtiene la página que se debe mostrar por defecto al usuario siempre que la misma esté establecida
        ''' </summary>
        ''' <remarks></remarks>
        Public Function ObtenerPaginaPorDefecto() As String
            Dim url As String = ""
            If _idUsuario <> 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .ejecutarReader("ObtenerPaginaPorDefecto", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            With .Reader
                                If .Read Then url = .Item("paginaInicial").ToString
                                .Close()
                            End With
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
            Return url
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Shared Function CambiarPassword(ByVal idUsuario As Integer, ByVal passwordActual As String, ByVal passwordNuevo As String) As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@passwordActual", SqlDbType.VarChar, 100).Value = passwordActual
                    .SqlParametros.Add("@passwordNuevo", SqlDbType.VarChar, 100).Value = passwordNuevo
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .ejecutarNonQuery("CambiarPasswordDeUsuario", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor durante el cambio de password. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function CrearUsuario() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try
                With dbManager
                    .iniciarTransaccion()
                    .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 15).Value = _numeroIdentificacion
                    .SqlParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = _nombreApellido
                    .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = _email
                    .SqlParametros.Add("@usuario", SqlDbType.VarChar, 50).Value = _usuario
                    .SqlParametros.Add("@pwd", SqlDbType.VarChar, 100).Value = _password
                    .SqlParametros.Add("@telefonos", SqlDbType.VarChar, 30).Value = _telefono
                    .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    .SqlParametros.Add("@idRol", SqlDbType.SmallInt).Value = _idRol
                    .SqlParametros.Add("@idPerfil", SqlDbType.SmallInt).Value = _idPerfil
                    .SqlParametros.Add("@idTipo", SqlDbType.SmallInt).Value = _idTipo
                    .SqlParametros.Add("@idCargo", SqlDbType.SmallInt).Value = _idCargo
                    .SqlParametros.Add("@idEmpresa", SqlDbType.SmallInt).Value = _idEmpresa
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = _idUnidadNegocio
                    If Not (_listaPdv = "") Then
                        .SqlParametros.Add("@listaPdv", SqlDbType.VarChar, 100).Value = _listaPdv
                    End If
                    .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = 1 '_idCreador
                    .SqlParametros.Add("@fechaIngreso", SqlDbType.DateTime).Value = _fechaIngreso
                    .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = _idEstado
                    .SqlParametros.Add("@idRolEnPdv", SqlDbType.TinyInt).Value = _idRol
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("CrearUsuarioDeSistema", CommandType.StoredProcedure)

                    If Long.TryParse(.SqlParametros("@idUsuario").Value, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Se presentó el siguiente error al crear el usuario. Por favor intente nuevamente")
                    End If
                    .confirmarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                resultado.Mensaje = ex.Message
                resultado.Valor = 0
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function ActualizarUsuario() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try

                With dbManager
                    .iniciarTransaccion()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@idPersona", SqlDbType.Int).Value = _idPersona
                    .SqlParametros.Add("@numeroIdentificacion", SqlDbType.Int).Value = _numeroIdentificacion
                    .SqlParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = _nombreApellido
                    .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = _email
                    .SqlParametros.Add("@usuario", SqlDbType.VarChar, 50).Value = _usuario
                    If Not (_password = "") Then
                        .SqlParametros.Add("@pwd", SqlDbType.VarChar, 100).Value = _password
                    End If
                    .SqlParametros.Add("@telefonos", SqlDbType.VarChar, 30).Value = _telefono
                    .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    .SqlParametros.Add("@idRol", SqlDbType.SmallInt).Value = _idRol
                    .SqlParametros.Add("@idPerfil", SqlDbType.SmallInt).Value = _idPerfil
                    .SqlParametros.Add("@idTipo", SqlDbType.SmallInt).Value = _idTipo
                    .SqlParametros.Add("@idCargo", SqlDbType.SmallInt).Value = _idCargo
                    .SqlParametros.Add("@idEmpresa", SqlDbType.SmallInt).Value = _idEmpresa
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = _idUnidadNegocio
                    If Not (_listaPdv = "") Then
                        .SqlParametros.Add("@listaPdv", SqlDbType.VarChar, 100).Value = _listaPdv
                    End If
                    .SqlParametros.Add("@idModificador", SqlDbType.Int).Value = _idCreador
                    .SqlParametros.Add("@fechaIngreso", SqlDbType.DateTime).Value = _fechaIngreso
                    If Not (_fechaRetiro = "01/01/1900") Then
                        .SqlParametros.Add("@fechaRetiro", SqlDbType.DateTime).Value = _fechaRetiro
                    End If
                    .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = _idEstado
                    .SqlParametros.Add("@idRolEnPdv", SqlDbType.TinyInt).Value = _idRol
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .ejecutarNonQuery("ActualizaUsuarioDeSistema", CommandType.StoredProcedure)
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    resultado.Valor = 0
                    .confirmarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                resultado.Mensaje = ex.Message
                resultado.Valor = 1
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function
        Public Function ActualizarPDVUsuario() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try

                With dbManager
                    .iniciarTransaccion()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@idPersona", SqlDbType.Int).Value = _idPersona
                    If Not (_listaPdv = "") Then
                        .SqlParametros.Add("@listaPdv", SqlDbType.VarChar, 100).Value = _listaPdv
                    End If
                    .SqlParametros.Add("@idRolEnPdv", SqlDbType.TinyInt).Value = _idRol
                    .SqlParametros.Add("@idModificador", SqlDbType.Int).Value = _idCreador
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .ejecutarNonQuery("ActualizaPDVUsuarioDeSistema", CommandType.StoredProcedure)
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    resultado.Valor = 0
                    .confirmarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                resultado.Mensaje = ex.Message
                resultado.Valor = 1
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function
#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .ejecutarReader("ObtenerInformacionDeUsuario", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idUsuario").ToString, _idUsuario)
                    Integer.TryParse(reader("idPersona").ToString, _idPersona)
                    _numeroIdentificacion = reader("numeroIdentificacion").ToString
                    _nombreApellido = reader("nombreApellido").ToString
                    Short.TryParse(reader("idTipo").ToString, _idTipo)
                    _usuario = reader("usuario").ToString
                    _password = reader("pwd").ToString
                    Short.TryParse(reader("idUnidadNegocio").ToString, _idUnidadNegocio)
                    Short.TryParse(reader("idCargo").ToString, _idCargo)
                    _cargo = reader("cargo").ToString
                    Byte.TryParse(reader("idEstado").ToString, _idEstado)
                    Short.TryParse(reader("idRol").ToString, _idRol)
                    Integer.TryParse(reader("idPerfil").ToString, _idPerfil)
                    Integer.TryParse(reader("idCiudad").ToString, _idCiudad)
                    Date.TryParse(reader("fechaCreacion").ToString, _fechaCreacion)
                    Integer.TryParse(reader("idCreador").ToString, _idCreador)
                    Date.TryParse(reader("fechaUltimaModificacion").ToString, _fechaUltimaModificacion)
                    Integer.TryParse(reader("idModificador").ToString, _idModificador)
                    _registrado = True
                End If
            End If

        End Sub

        Protected Friend Sub CargarResultadoConsultaEdicion(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idUsuario").ToString, _idUsuario)
                    Integer.TryParse(reader("idPersona").ToString, _idPersona)
                    _numeroIdentificacion = reader("numeroIdentificacion").ToString
                    _nombreApellido = reader("nombreApellido").ToString
                    Short.TryParse(reader("idTipo").ToString, _idTipo)
                    _tipoPersona = reader("TipoPersona").ToString
                    _usuario = reader("usuario").ToString
                    _password = reader("pwd").ToString
                    Short.TryParse(reader("idUnidadNegocio").ToString, _idUnidadNegocio)
                    Short.TryParse(reader("idCargo").ToString, _idCargo)
                    _cargo = reader("cargo").ToString
                    Byte.TryParse(reader("idEstado").ToString, _idEstado)
                    _estado = reader("Estado").ToString
                    Short.TryParse(reader("idRol").ToString, _idRol)
                    _rol = reader("Rol").ToString
                    Date.TryParse(reader("fechaCreacion").ToString, _fechaCreacion)
                    Integer.TryParse(reader("idCreador").ToString, _idCreador)
                    Date.TryParse(reader("fechaUltimaModificacion").ToString, _fechaUltimaModificacion)
                    Integer.TryParse(reader("idModificador").ToString, _idModificador)
                    Date.TryParse(reader("fechaIngreso").ToString, _fechaIngreso)
                    Short.TryParse(reader("idPerfil").ToString, _idPerfil)
                    Short.TryParse(reader("idCiudad").ToString, _idCiudad)
                    _ciudad = reader("Ciudad").ToString
                    Short.TryParse(reader("idEmpresa").ToString, _idEmpresa)
                    _empresa = reader("Empresa").ToString
                    _telefono = reader("telefonos").ToString
                    _email = reader("email").ToString

                    _registrado = True
                End If
            End If

        End Sub
#End Region

    End Class

End Namespace