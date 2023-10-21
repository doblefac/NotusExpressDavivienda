Imports LMDataAccessLayer
Imports System.Web

Public Class Canal
#Region "Atributos"

    Private _idCanal As Integer
    Private _nombre As String
    Private _idEstado As SByte
    Private _estado As String
    Private _fechaRegistro As Date
    Private _idCreador As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _nombre = ""
        _idEstado = -1
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        Me._idCanal = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdCanal As Integer
        Get
            Return _idCanal
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idCanal = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property IdEstado As SByte
        Get
            Return _idEstado
        End Get
        Set(ByVal value As SByte)
            _idEstado = value
        End Set
    End Property

    Public Property Estado As String
        Get
            Return _estado
        End Get
        Set(value As String)
            _idEstado = value
        End Set
    End Property

    Public Property FechaRegistro As Date
        Get
            Return _fechaRegistro
        End Get
        Protected Friend Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property IdCreador As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal value As Integer)
            _idCreador = value
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

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        If Me._idCanal > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If Me._idCanal > 0 Then .SqlParametros.Add("@idCanal", SqlDbType.Int).Value = Me._idCanal
                    .ejecutarReader("ObtenerInfoCanal", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            AsignarValorAPropiedades(.Reader)
                        End If
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region

#Region "Métodos Publicos"

    Public Function Registrar() As General.ResultadoProceso
        Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
        Dim idUsuario As Integer = 0
        If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId").ToString, idUsuario)
        If Not String.IsNullOrEmpty(Me._nombre.Trim) AndAlso idUsuario > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@nombre", SqlDbType.VarChar, 150).Value = Me._nombre.Trim
                    .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("RegistrarCanal", CommandType.StoredProcedure)
                    If Short.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta emitida por el servidor. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(300, "No se han establecido todos los datos requeridos para realizar el registro. Por favor verifique")
        End If

        Return resultado
    End Function

    Public Function Actualizar() As General.ResultadoProceso
        Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
        Dim idUsuario As Integer = 0
        If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId").ToString, idUsuario)
        If _idCanal > 0 AndAlso (Not String.IsNullOrEmpty(Me._nombre.Trim) OrElse _idEstado > -1) AndAlso idUsuario > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idCanal", SqlDbType.Int).Value = Me._idCanal
                    If Not String.IsNullOrEmpty(Me._nombre) Then _
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 150).Value = Me._nombre.Trim
                    If Me._idEstado > -1 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = Me._idEstado
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@idModificador", SqlDbType.Int).Value = Me._idCreador
                    .ejecutarNonQuery("ActualizarInfoCanal", CommandType.StoredProcedure)
                    If Short.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta emitida por el servidor. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(300, "No se han establecido todos los datos requeridos para realizar la actualización. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idCanal").ToString, Me._idCanal)
                Me._nombre = reader("nombre").ToString
                SByte.TryParse(reader("idEstado").ToString, Me._idEstado)
                Me._estado = reader("estado").ToString
                Date.TryParse(reader("fechaRegistro").ToString, Me._fechaRegistro)
                Integer.TryParse(reader("idUsuarioRegistra").ToString, Me._idCreador)
                Me._registrado = True
            End If
        End If
    End Sub

#End Region
End Class
