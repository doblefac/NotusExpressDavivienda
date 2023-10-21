Imports System.IO
Imports System.Collections.Generic
Imports NotusExpressBusinessLayer

''' <summary>
''' Clase que permite establecer los filtros establecidos para la cosulta de Campañas 
''' </summary>
''' <remarks></remarks>
Public Class WsRegistroCampania

#Region "Atributos"

    Private _idCampania As Integer
    Private _nombre As String
    Private _fechaInicio As Date
    Private _fechafin As Date
    Private _activo As Boolean
    Private _listCiudades As List(Of Integer)
    Private _listBodegas As List(Of Integer)
    Private _listProductoExterno As List(Of Integer)
    Private _listDocumentoFinanciero As List(Of Integer)
    Private _listTiposDeServicio As List(Of Integer)
    Private _idClienteExterno As Integer
    Private _idTipoCampania As Integer
    Private _idSistema As Integer

#End Region

#Region "Propiedades"

    Public Property IdCampania As Integer
        Get
            Return _idCampania
        End Get
        Set(value As Integer)
            _idCampania = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property FechaInicio As Date
        Get
            Return _fechaInicio
        End Get
        Set(value As Date)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFin As Date
        Get
            Return _fechafin
        End Get
        Set(value As Date)
            _fechafin = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

    Public Property ListCiudades As List(Of Integer)
        Get
            If _listCiudades Is Nothing Then _listCiudades = New List(Of Integer)
            Return _listCiudades
        End Get
        Set(value As List(Of Integer))
            _listCiudades = value
        End Set
    End Property

    Public Property ListBodegas As List(Of Integer)
        Get
            If _listBodegas Is Nothing Then _listBodegas = New List(Of Integer)
            Return _listBodegas
        End Get
        Set(value As List(Of Integer))
            _listBodegas = value
        End Set
    End Property

    Public Property ListProductoExterno As List(Of Integer)
        Get
            If _listProductoExterno Is Nothing Then _listProductoExterno = New List(Of Integer)
            Return _listProductoExterno
        End Get
        Set(value As List(Of Integer))
            _listProductoExterno = value
        End Set
    End Property

    Public Property ListDocumentoFinanciero As List(Of Integer)
        Get
            If _listDocumentoFinanciero Is Nothing Then _listDocumentoFinanciero = New List(Of Integer)
            Return _listDocumentoFinanciero
        End Get
        Set(value As List(Of Integer))
            _listDocumentoFinanciero = value
        End Set
    End Property

    Public Property ListTiposDeServicio As List(Of Integer)
        Get
            If _listTiposDeServicio Is Nothing Then _listTiposDeServicio = New List(Of Integer)
            Return _listTiposDeServicio
        End Get
        Set(value As List(Of Integer))
            _listTiposDeServicio = value
        End Set
    End Property

    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
        End Set
    End Property

    Public Property IdTipoCampania As Integer
        Get
            Return _idTipoCampania
        End Get
        Set(value As Integer)
            _idTipoCampania = value
        End Set
    End Property

    Public Property IdSistema As Integer
        Get
            Return _idSistema
        End Get
        Set(value As Integer)
            _idSistema = value
        End Set
    End Property

#End Region

End Class
