Imports LMDataAccessLayer
Imports System.Web

Public Class Cumplimiento
#Region "Atributos"

    Private _idCumplimiento As Integer
    Private _inicio1 As Double
    Private _fin1 As Double
    Private _rutaColor1 As String
    Private _inicio2 As Double
    Private _fin2 As Double
    Private _rutaColor2 As String
    Private _inicio3 As Double
    Private _fin3 As Double
    Private _rutaColor3 As String
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdCumplimiento As Integer
        Get
            Return _idCumplimiento
        End Get
        Set(ByVal value As Integer)
            _idCumplimiento = value
        End Set
    End Property

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
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                .ejecutarReader("ObtenerInfoCumplimiento", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            AsignarValorAPropiedades(.Reader)
                        End If
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
    End Sub

#End Region

#Region "Métodos Publicos"

    'Public Function Registrar() As General.ResultadoProceso
    '    Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
    '    Dim idUsuario As Integer = 0
    '    If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId").ToString, idUsuario)
    '    If Not String.IsNullOrEmpty(Me._nombre.Trim) AndAlso idUsuario > 0 Then
    '        Dim dbManager As New LMDataAccess
    '        Try
    '            With dbManager
    '                .SqlParametros.Add("@nombre", SqlDbType.VarChar, 150).Value = Me._nombre.Trim
    '                .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = idUsuario
    '                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
    '                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
    '                .ejecutarNonQuery("RegistrarCanal", CommandType.StoredProcedure)
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
    '        resultado.EstablecerMensajeYValor(300, "No se han establecido todos los datos requeridos para realizar el registro. Por favor verifique")
    '    End If

    '    Return resultado
    'End Function

    'Public Function Actualizar() As General.ResultadoProceso
    '    Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
    '    Dim idUsuario As Integer = 0
    '    If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId").ToString, idUsuario)
    '    If _idCanal > 0 AndAlso (Not String.IsNullOrEmpty(Me._nombre.Trim) OrElse _idEstado > -1) AndAlso idUsuario > 0 Then
    '        Dim dbManager As New LMDataAccess
    '        Try
    '            With dbManager
    '                .SqlParametros.Add("@idCanal", SqlDbType.Int).Value = Me._idCanal
    '                If Not String.IsNullOrEmpty(Me._nombre) Then _
    '                    .SqlParametros.Add("@nombre", SqlDbType.VarChar, 150).Value = Me._nombre.Trim
    '                If Me._idEstado > -1 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = Me._idEstado
    '                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
    '                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
    '                .ejecutarNonQuery("ActualizarInfoCanal", CommandType.StoredProcedure)
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

#Region "Métodos Protegidos"

    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idCumplimiento").ToString, Me._idCumplimiento)
                Integer.TryParse(reader("inicio1").ToString, Me._inicio1)
                Integer.TryParse(reader("fin1").ToString, Me._fin1)
                Integer.TryParse(reader("inicio2").ToString, Me._inicio2)
                Integer.TryParse(reader("fin2").ToString, Me._fin2)
                Integer.TryParse(reader("inicio3").ToString, Me._inicio3)
                Integer.TryParse(reader("fin3").ToString, Me._fin3)
                Me._registrado = True
            End If
        End If
    End Sub

#End Region
End Class