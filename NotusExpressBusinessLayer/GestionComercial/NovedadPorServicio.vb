Imports LMDataAccessLayer

Imports NotusExpressBusinessLayer.General

Public Class NovedadPorServicio

#Region "Atributos"

    Private _idNovedadServicio As Integer
    Private _idNovedad As Integer
    Private _novedad As String
    Private _causal As String
    Private _idGestionVenta As Integer
    Private _idEstado As Integer
    Private _estado As String
    Private _fechaRegistro As DateTime
    Private _idUsuarioRegistro As Integer
    Private _usuarioRegistro As String
    Private _idUsuarioGestion As Integer
    Private _usuarioGestion As String
    Private _fechaGestion As DateTime
    Private _observacionGestion As String

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdNovedadServicio As Integer
        Get
            Return _idNovedadServicio
        End Get
        Set(value As Integer)
            _idNovedadServicio = value
        End Set
    End Property

    Public Property IdNovedad As Integer
        Get
            Return _idNovedad
        End Get
        Set(value As Integer)
            _idNovedad = value
        End Set
    End Property

    Public Property Novedad As String
        Get
            Return _novedad
        End Get
        Set(value As String)
            _novedad = value
        End Set
    End Property

    Public Property Causal As String
        Get
            Return _causal
        End Get
        Set(value As String)
            _causal = value
        End Set
    End Property

    Public Property IdGestionVenta As Integer
        Get
            Return _idGestionVenta
        End Get
        Set(value As Integer)
            _idGestionVenta = value
        End Set
    End Property

    Public Property IdEstado As Integer
        Get
            Return _idEstado
        End Get
        Set(value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property Estado As String
        Get
            Return _estado
        End Get
        Set(value As String)
            _estado = value
        End Set
    End Property

    Public Property FechaRegistro As DateTime
        Get
            Return _fechaRegistro
        End Get
        Set(value As DateTime)
            _fechaRegistro = value
        End Set
    End Property

    Public Property IdUsuarioRegistro As Integer
        Get
            Return _idUsuarioRegistro
        End Get
        Set(value As Integer)
            _idUsuarioRegistro = value
        End Set
    End Property

    Public Property UsuarioRegistro As String
        Get
            Return _usuarioRegistro
        End Get
        Set(value As String)
            _usuarioRegistro = value
        End Set
    End Property

    Public Property IdUsuarioGestion As Integer
        Get
            Return _idUsuarioGestion
        End Get
        Set(value As Integer)
            _idUsuarioGestion = value
        End Set
    End Property

    Public Property UsuarioGestion As String
        Get
            Return _usuarioGestion
        End Get
        Set(value As String)
            _usuarioGestion = value
        End Set
    End Property

    Public Property FechaGestion As DateTime
        Get
            Return _fechaGestion
        End Get
        Set(value As DateTime)
            _fechaGestion = value
        End Set
    End Property

    Public Property ObservacionGestion As String
        Get
            Return _observacionGestion
        End Get
        Set(value As String)
            _observacionGestion = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idNovedadServicio As Integer)
        MyBase.New()
        _idNovedadServicio = idNovedadServicio
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idNovedadServicio > 0 Then .SqlParametros.Add("@listIdNovedadServicio", SqlDbType.VarChar, 2000).Value = CStr(_idNovedadServicio)
                .ejecutarReader("ObtenerInfoNovedadPorServicio", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        CargarResultadoConsulta(.Reader)
                        _registrado = True
                    End If
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idNovedadServicio", SqlDbType.Int).Value = _idNovedadServicio
                    If _idNovedad > 0 Then .Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                    If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    If _idUsuarioGestion > 0 Then .Add("@idUsuarioGestion", SqlDbType.Int).Value = _idUsuarioGestion
                    If _fechaGestion > Date.MinValue Then .Add("@fechaGestion", SqlDbType.DateTime).Value = _fechaGestion
                    If Not String.IsNullOrEmpty(_observacionGestion) Then .Add("@observacionGestion", SqlDbType.VarChar, 2000).Value = _observacionGestion
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("ActualizaNovedadPorServicio", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al actualizar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function Eliminar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idNovedadServicio", SqlDbType.Int).Value = _idNovedadServicio
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("EliminaNovedadPorServicio", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al actualizar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idNovedadServicio"), _idNovedadServicio)
                Integer.TryParse(reader("idNovedad"), _idNovedad)
                If Not IsDBNull(reader("novedad")) Then _novedad = (reader("novedad").ToString)
                If Not IsDBNull(reader("causal")) Then _causal = (reader("causal").ToString)
                If Not IsDBNull(reader("idGestionVenta")) Then Integer.TryParse(reader("idGestionVenta"), _idGestionVenta)
                If Not IsDBNull(reader("idEstado")) Then Integer.TryParse(reader("idEstado"), _idEstado)
                If Not IsDBNull(reader("estado")) Then _estado = (reader("estado").ToString)
                If Not IsDBNull(reader("fechaRegistro")) Then _fechaRegistro = CDate(reader("fechaRegistro").ToString)
                If Not IsDBNull(reader("idUsuarioRegistra")) Then Integer.TryParse(reader("idUsuarioRegistra"), _idUsuarioRegistro)
                If Not IsDBNull(reader("idUsuarioGestion")) Then Integer.TryParse(reader("idUsuarioGestion"), _idUsuarioGestion)
                If Not IsDBNull(reader("fechaGestion")) Then _fechaGestion = CDate(reader("fechaGestion").ToString)
                If Not IsDBNull(reader("observacionGestion")) Then _observacionGestion = (reader("observacionGestion").ToString)
            End If
        End If
    End Sub

#End Region

End Class
