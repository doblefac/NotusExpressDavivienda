Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class RegistrarDocumentoDigitalizado
#Region "Atributos"

    Private _idDoc As Integer
    Private _idDocumento As Integer
    Private _estado As Integer
    Private _ruta As String
    Private _idUsuarioRegistra As Integer
    Private _numeroIdentificacion As String
    Private _datosRegistros As DataTable
    Private _planilla As Integer
    Private _idRadicado As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdDoc() As Integer
        Get
            Return _idDoc
        End Get
        Set(ByVal value As Integer)
            _idDoc = value
        End Set
    End Property

    Public Property IdDocumento() As Integer
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As Integer)
            _idDocumento = value
        End Set
    End Property

    Public Property Estado() As Integer
        Get
            Return _estado
        End Get
        Set(ByVal value As Integer)
            _estado = value
        End Set
    End Property

    Public Property Ruta() As String
        Get
            Return _ruta
        End Get
        Set(ByVal value As String)
            _ruta = value
        End Set
    End Property

    Public Property IdUsuarioRegistra() As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
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

    Public Property Planilla() As Integer
        Get
            Return _planilla
        End Get
        Set(ByVal value As Integer)
            _planilla = value
        End Set
    End Property

    Public Property IdRadicado() As Integer
        Get
            Return _idRadicado
        End Get
        Set(ByVal value As Integer)
            _idRadicado = value
        End Set
    End Property

    Public ReadOnly Property DatosRegistros() As DataTable
        Get
            If _datosRegistros Is Nothing Then CargarDatos()
            Return _datosRegistros
        End Get
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

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            dbManager.iniciarTransaccion()
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idDocumento", SqlDbType.Int).Value = _idDocumento
                .SqlParametros.Add("@ruta", SqlDbType.NVarChar, 1000).Value = _ruta
                .SqlParametros.Add("@estado", SqlDbType.Int).Value = _estado
                .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = _numeroIdentificacion
                If _planilla = 1 Then
                    .SqlParametros.Add("@planilla", SqlDbType.Int).Value = _planilla
                    .SqlParametros.Add("@idRadicado", SqlDbType.Int).Value = _idRadicado
                End If
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
                .ejecutarNonQuery("RegistrarDocumentoDigitalizado", CommandType.StoredProcedure)

                .confirmarTransaccion()

            End With

        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            dbManager.iniciarTransaccion()
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@estado", SqlDbType.Int).Value = _estado
                .SqlParametros.Add("@idDoc", SqlDbType.Int).Value = _idDoc
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
                .ejecutarNonQuery("ActualizarDocumentoDigitalizado", CommandType.StoredProcedure)

                .confirmarTransaccion()

            End With

        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    Public Function Eliminar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            dbManager.iniciarTransaccion()
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idDoc", SqlDbType.Int).Value = _idDoc
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
                .ejecutarNonQuery("EliminarDocumentoDigitalizado", CommandType.StoredProcedure)

                .confirmarTransaccion()

            End With

        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If Len(_numeroIdentificacion) > 0 Then .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = _numeroIdentificacion
                .TiempoEsperaComando = 600
                _datosRegistros = .ejecutarDataTable("ObtenerListadoDocumentosDigitalizados", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
#End Region
End Class
