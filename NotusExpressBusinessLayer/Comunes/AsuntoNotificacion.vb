Imports NotusExpressBusinessLayer.Estructuras
Imports NotusExpressBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace Comunes

    Public Class AsuntoNotificacion

#Region "Campos"

        Private _idAsuntoNotificacion As Integer
        Private _nombre As String
        Private _estado As Short
        Private _idUsuarioCreacion As Integer
        Private _fechaCreacion As Date
        Private _usuarioCreacion As String
        Private _idPerfil As Integer

#End Region

#Region "Propiedades"

        Public Property IdUsuarioNotificacion() As Integer
            Get
                Return _idAsuntoNotificacion
            End Get
            Set(ByVal value As Integer)
                _idAsuntoNotificacion = value
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

        Public Property Estado() As Short
            Get
                Return _estado
            End Get
            Set(ByVal value As Short)
                _estado = value
            End Set
        End Property

        Public Property UsuarioCreacion() As String
            Get
                Return _usuarioCreacion
            End Get
            Set(ByVal value As String)
                _usuarioCreacion = value
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

        Public Property IdUsuarioCreacion() As Integer
            Get
                Return _idUsuarioCreacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCreacion = value
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

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idAsuntoNotificacion = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idAsuntoNotificacion > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.Int).Value = _idAsuntoNotificacion
                        .ejecutarReader("ObtenerInfoAsuntoNotificacion", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing AndAlso .Reader.Read Then
                            _nombre = .Reader("nombre").ToString
                            Short.TryParse(.Reader("estado").ToString, _estado)
                            _usuarioCreacion = .Reader("usuarioCreacion").ToString
                            Date.TryParse(.Reader("fechaCreacion").ToString, _fechaCreacion)
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

#Region "Métodos Compartidos"

#End Region

#Region "Enum"
        Public Enum Tipo
            EnvioLectura = 1
            EnvioPrueba = 2
            CreacionInstruccion = 3
            DiferenciasOrdenInventario = 4
            MaterialEnCuarentena = 5
            TransportadoraDespacho = 8
            ValorMaterialDespacho = 9
            LiberacionMaterial = 10
            NacionalizacionProducto = 12
            AutorizacionCambioSoftware = 13
            NotificaciónInstrucciónReproceso = 14
            NotificaciónEnvioLecturaReproceso = 15
            Notificación_Solución_Novedad_POP = 16
            NotificacionCreacionCampaniaPOP = 17
            NotificacionCreacionDistribucionPOP = 18
            NotificacionCreacionInstruccionPOP = 19
            NotificacionCierreDespachoPOP = 20
            NotificacionRecepcionProducto = 21
            SinDisponibilidadInventario = 22
            NotificaciónVencimientoSiembra = 23
            NotificaciónNuevosProductos = 25
            GeneracionRutaSamsungTransportadora = 41
            GeneracionServiciosMasivosSAMSUNG = 42
        End Enum

#End Region

    End Class

End Namespace