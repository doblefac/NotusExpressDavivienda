Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class RadicarDocumentos

#Region "Atributos"

    Private _idRadicado As Integer
    Private _idUsuarioRegistra As Integer
    Private _numeroPrecinto As String
    Private _listaDocumentos As ArrayList
    Private _listaGestiones As List(Of Long)

    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdRadicado() As Integer
        Get
            Return _idRadicado
        End Get
        Set(ByVal value As Integer)
            _idRadicado = value
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

    Public Property NumeroPrecinto() As Integer
        Get
            Return _numeroPrecinto
        End Get
        Set(ByVal value As Integer)
            _numeroPrecinto = value
        End Set
    End Property

    Public Property ListaDocumentos As ArrayList
        Get
            If _listaDocumentos Is Nothing Then _listaDocumentos = New ArrayList
            Return _listaDocumentos
        End Get
        Set(value As ArrayList)
            _listaDocumentos = value
        End Set
    End Property

    Public Property ListaGestiones As List(Of Long)
        Get
            If _listaGestiones Is Nothing Then _listaGestiones = New List(Of Long)
            Return _listaGestiones
        End Get
        Set(value As List(Of Long))
            _listaGestiones = value
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

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                    .Add("@numeroPrecinto", SqlDbType.VarChar, 450).Value = _numeroPrecinto
                    If _listaDocumentos IsNot Nothing AndAlso _listaDocumentos.Count > 0 Then _
                        .Add("@listaDocumentos", SqlDbType.VarChar).Value = Join(_listaDocumentos.ToArray, ",")
                    If _listaGestiones IsNot Nothing AndAlso _listaGestiones.Count > 0 Then _
                        .Add("@listaGestiones", SqlDbType.VarChar).Value = String.Join(",", _listaGestiones.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@idRadicado", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarRadicacionDocumentos", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                        _idRadicado = .SqlParametros("@idRadicado").Value
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intente nuevamente.")
                End If

            End With

        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al realizar la radicación de documentos: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
