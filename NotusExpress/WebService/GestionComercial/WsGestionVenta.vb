Imports System.IO
Imports System.Collections.Generic
Imports NotusExpressBusinessLayer

''' <summary>
''' Clase que permite establecer los objetos relacionados en la gestión de venta
''' </summary>
''' <remarks></remarks>
Public Class WsGestionVenta

#Region "Atributos"

    Private _idGestionVenta As Long
    Private _idCliente As Integer
    Private _idResultadoProceso As Enumerados.ResuladoProcesoVenta
    Private _idEstado As Enumerados.EstadosGestionDeVenta
    Private _observacionCallCenter As String
    Private _observacionVendedor As String
    Private _fechaRecepcionDocumentos As Date
    Private _idReceptorDocumento As Integer
    Private _fechaLegalizacion As Date
    Private _idLegalizador As Integer
    Private _esNovedad As Boolean
    Private _observacionDeclinar As String
    Private _idNovedad As Integer
    Private _observacionNovedad As String
    Private _idCampaniaNotus As Integer
    Private _idServicioNotus As Long
    Private _idModificador As Integer
    Private _fechaAgenda As Date
    Private _idEstadoServicioMensajeria As Integer
    Private _listaNovedades As ArrayList

#End Region

#Region "Propiedades"

    Public Property IdGestionVenta As Long
        Get
            Return _idGestionVenta
        End Get
        Set(value As Long)
            _idGestionVenta = value
        End Set
    End Property

    Public Property IdCliente As Integer
        Get
            Return _idCliente
        End Get
        Set(value As Integer)
            _idCliente = value
        End Set
    End Property

    Public Property IdResultadoProceso As Enumerados.ResuladoProcesoVenta
        Get
            Return _idResultadoProceso
        End Get
        Set(value As Enumerados.ResuladoProcesoVenta)
            _idResultadoProceso = value
        End Set
    End Property

    Public Property IdEstado As Enumerados.EstadosGestionDeVenta
        Get
            Return _idEstado
        End Get
        Set(value As Enumerados.EstadosGestionDeVenta)
            _idEstado = value
        End Set
    End Property

    Public Property ObservacionCallCenter As String
        Get
            Return _observacionCallCenter
        End Get
        Set(value As String)
            _observacionCallCenter = value
        End Set
    End Property

    Public Property ObservacionVendedor As String
        Get
            Return _observacionVendedor
        End Get
        Set(value As String)
            _observacionVendedor = value
        End Set
    End Property

    Public Property FechaRecepcionDocumentos As Date
        Get
            Return _fechaRecepcionDocumentos
        End Get
        Set(value As Date)
            _fechaRecepcionDocumentos = value
        End Set
    End Property

    Public Property IdReceptorDocumento As Integer
        Get
            Return _idReceptorDocumento
        End Get
        Set(value As Integer)
            _idReceptorDocumento = value
        End Set
    End Property

    Public Property FechaLegalizacion As Date
        Get
            Return _fechaLegalizacion
        End Get
        Set(value As Date)
            _fechaLegalizacion = value
        End Set
    End Property

    Public Property IdLegalizador As Integer
        Get
            Return _idLegalizador
        End Get
        Set(value As Integer)
            _idLegalizador = value
        End Set
    End Property

    Public Property EsNovedad As Boolean
        Get
            Return _esNovedad
        End Get
        Set(value As Boolean)
            _esNovedad = value
        End Set
    End Property

    Public Property ObservacionDeclinar As String
        Get
            Return _observacionDeclinar
        End Get
        Set(value As String)
            _observacionDeclinar = value
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

    Public Property ObservacionNovedad As String
        Get
            Return _observacionNovedad
        End Get
        Set(value As String)
            _observacionNovedad = value
        End Set
    End Property

    Public Property IdCampaniaNotus As Integer
        Get
            Return _idCampaniaNotus
        End Get
        Set(value As Integer)
            _idCampaniaNotus = value
        End Set
    End Property

    Public Property IdServicioNotus As Long
        Get
            Return _idServicioNotus
        End Get
        Set(value As Long)
            _idServicioNotus = value
        End Set
    End Property

    Public Property IdModificador As Integer
        Get
            Return _idModificador
        End Get
        Set(value As Integer)
            _idModificador = value
        End Set
    End Property

    Public Property FechaAgenda As Date
        Get
            Return _fechaAgenda
        End Get
        Set(value As Date)
            _fechaAgenda = value
        End Set
    End Property

    Public Property IdEstadoServicioMensajeria As Integer
        Get
            Return _idEstadoServicioMensajeria
        End Get
        Set(value As Integer)
            _idEstadoServicioMensajeria = value
        End Set
    End Property

    Public Property ListaNovedades As ArrayList
        Get
            If _listaNovedades Is Nothing Then _listaNovedades = New ArrayList
            Return _listaNovedades
        End Get
        Set(value As ArrayList)
            _listaNovedades = value
        End Set
    End Property

#End Region

End Class
