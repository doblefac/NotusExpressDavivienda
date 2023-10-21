Imports LMDataAccessLayer
Imports System.Web

Public Class RegistrarCumplimiento
#Region "Atributos"

    Private _inicio1 As Double
    Private _fin1 As Double
    Private _rutaColor1 As String
    Private _inicio2 As Double
    Private _fin2 As Double
    Private _rutaColor2 As String
    Private _inicio3 As Double
    Private _fin3 As Double
    Private _rutaColor3 As String
    Private _idUsuarioRegistra As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()

    End Sub

#End Region

#Region "Propiedades"

    Public Property Inicio1 As Double
        Get
            Return _inicio1
        End Get
        Set(ByVal value As Double)
            _inicio1 = value
        End Set
    End Property

    Public Property Fin1 As Double
        Get
            Return _fin1
        End Get
        Set(ByVal value As Double)
            _fin1 = value
        End Set
    End Property

    Public Property RutaColor1 As String
        Get
            Return _rutaColor1
        End Get
        Set(ByVal value As String)
            _rutaColor1 = value
        End Set
    End Property

    Public Property Inicio2 As Double
        Get
            Return _inicio2
        End Get
        Set(ByVal value As Double)
            _inicio2 = value
        End Set
    End Property

    Public Property Fin2 As Double
        Get
            Return _fin2
        End Get
        Set(ByVal value As Double)
            _fin2 = value
        End Set
    End Property

    Public Property RutaColor2 As String
        Get
            Return _rutaColor2
        End Get
        Set(ByVal value As String)
            _rutaColor2 = value
        End Set
    End Property

    Public Property Inicio3 As Double
        Get
            Return _inicio3
        End Get
        Set(ByVal value As Double)
            _inicio3 = value
        End Set
    End Property

    Public Property Fin3 As Double
        Get
            Return _fin3
        End Get
        Set(ByVal value As Double)
            _fin3 = value
        End Set
    End Property

    Public Property RutaColor3 As String
        Get
            Return _rutaColor3
        End Get
        Set(ByVal value As String)
            _rutaColor3 = value
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
                .SqlParametros.Add("@inicio1", SqlDbType.Float).Value = Me._inicio1
                .SqlParametros.Add("@fin1", SqlDbType.Float).Value = Me._fin1
                .SqlParametros.Add("@rutaColor1", SqlDbType.NVarChar, 200).Value = Me._rutaColor1
                .SqlParametros.Add("@inicio2", SqlDbType.Float).Value = Me._inicio2
                .SqlParametros.Add("@fin2", SqlDbType.Float).Value = Me._fin2
                .SqlParametros.Add("@rutaColor2", SqlDbType.NVarChar, 200).Value = Me._rutaColor2
                .SqlParametros.Add("@inicio3", SqlDbType.Float).Value = Me._inicio3
                .SqlParametros.Add("@fin3", SqlDbType.Float).Value = Me._fin3
                .SqlParametros.Add("@rutaColor3", SqlDbType.NVarChar, 200).Value = Me._rutaColor3
                .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = idUsuario
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .ejecutarNonQuery("RegistrarCumplimientoMeta", CommandType.StoredProcedure)
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


