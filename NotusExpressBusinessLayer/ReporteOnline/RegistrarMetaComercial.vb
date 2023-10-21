Imports LMDataAccessLayer
Imports System.Web

Public Class RegistrarMetaComercial

#Region "Atributos"

    Private _idEstrategia As Integer
    Private _idPdv As Integer
    Private _idTipoProducto As Integer
    Private _anio As Integer
    Private _mes As Integer
    Private _meta As Integer
    Private _idUsuarioRegistra As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _idEstrategia = 0
        _idPdv = 0
        _idTipoProducto = 0
        _anio = 0
        _mes = 0
        _meta = 0
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()

    End Sub

#End Region

#Region "Propiedades"

    Public Property IdEstrategia As Integer
        Get
            Return _idEstrategia
        End Get
        Set(ByVal value As Integer)
            _idEstrategia = value
        End Set
    End Property

    Public Property IdPdv As Integer
        Get
            Return _idPdv
        End Get
        Set(ByVal value As Integer)
            _idPdv = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(ByVal value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property Anio As Integer
        Get
            Return _anio
        End Get
        Set(ByVal value As Integer)
            _anio = value
        End Set
    End Property

    Public Property Mes As Integer
        Get
            Return _mes
        End Get
        Set(value As Integer)
            _mes = value
        End Set
    End Property

    Public Property Meta As Integer
        Get
            Return _meta
        End Get
        Set(ByVal value As Integer)
            _meta = value
        End Set
    End Property

    Public Property IdUsuarioRegistra As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
        End Set
    End Property

    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Publicos"

    Public Function Registrar() As General.ResultadoProceso
        Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
        Dim idUsuario As Integer = 0
        If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId").ToString, idUsuario)
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idEstrategia", SqlDbType.SmallInt).Value = Me._idEstrategia
                .SqlParametros.Add("@idPdv", SqlDbType.SmallInt).Value = Me._idPdv
                .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = Me._idTipoProducto
                .SqlParametros.Add("@anio", SqlDbType.SmallInt).Value = Me._anio
                .SqlParametros.Add("@mes", SqlDbType.TinyInt).Value = Me._mes
                .SqlParametros.Add("@meta", SqlDbType.Int).Value = Me._meta
                .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = idUsuario
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .ejecutarNonQuery("RegistrarMetaComercial", CommandType.StoredProcedure)
                If Short.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta emitida por el servidor. Por favor intente nuevamente")
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    'Public Function Actualizar() As General.ResultadoProceso
    '    Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
    '    Dim idUsuario As Integer = 0
    '    If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId").ToString, idUsuario)
    '    If _idEstrategia > 0 AndAlso (Not String.IsNullOrEmpty(Me._nombre.Trim) OrElse _idEstado > -1) AndAlso idUsuario > 0 Then
    '        Dim dbManager As New LMDataAccess
    '        Try
    '            With dbManager
    '                .SqlParametros.Add("@idEstrategia", SqlDbType.Int).Value = Me._idEstrategia
    '                If Not String.IsNullOrEmpty(Me._nombre) Then _
    '                    .SqlParametros.Add("@nombre", SqlDbType.VarChar, 150).Value = Me._nombre.Trim
    '                If Me._idEstado > -1 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = Me._idEstado
    '                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
    '                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
    '                .ejecutarNonQuery("ActualizarInfoEstrategiaComercial", CommandType.StoredProcedure)
    '                If Short.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
    '                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
    '                Else
    '                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta emitida por el servidor. Por favor intente nuevamente")
    '                End If
    '            End With
    '        Finally
    '            If dbManager IsNot Nothing Then dbManager.Dispose()
    '        End Try
    '    Else
    '        resultado.EstablecerMensajeYValor(300, "No se han establecido todos los datos requeridos para realizar la actualización. Por favor verifique")
    '    End If

    '    Return resultado
    'End Function

#End Region

    '#Region "Métodos Protegidos"

    '    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
    '        If reader IsNot Nothing Then
    '            If reader.HasRows Then
    '                Integer.TryParse(reader("idEstrategia").ToString, Me._idEstrategia)
    '                Me._nombre = reader("nombre").ToString
    '                Me._canal = reader("canal").ToString
    '                SByte.TryParse(reader("idEstado").ToString, Me._idEstado)
    '                Me._estado = reader("estado").ToString
    '                Date.TryParse(reader("fechaRegistro").ToString, Me._fechaRegistro)
    '                Integer.TryParse(reader("idUsuarioRegistra").ToString, Me._idCreador)
    '                Me._registrado = True
    '            End If
    '        End If
    '    End Sub

    '#End Region
End Class
