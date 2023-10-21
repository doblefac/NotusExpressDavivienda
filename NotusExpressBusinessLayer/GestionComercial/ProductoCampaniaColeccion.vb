﻿Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados
Imports NotusExpressBusinessLayer.Comunes
Imports System.Net

Public Class ProductoCampaniaColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"
    Private _idCampania As Long
    Private _cargado As Boolean
    Private _tipoProducto As Boolean = False
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As GestionComercial.ProductoCampania
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As GestionComercial.ProductoCampania)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdCampania As Long
        Get
            Return _idCampania
        End Get
        Set(value As Long)
            _idCampania = value
        End Set
    End Property

    Public Property TipoProducto As Boolean
        Get
            Return _tipoProducto
        End Get
        Set(value As Boolean)
            _tipoProducto = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objProductoCampania As Type = GetType(GestionComercial.ProductoCampania)
        Dim pInfo As PropertyInfo

        For Each pInfo In objProductoCampania.GetProperties
            If pInfo.PropertyType.Namespace = "System" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            End If
        Next
        Return dtAux
    End Function

#End Region

#Region "Métodos Públicos"

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GestionComercial.ProductoCampania)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As GestionComercial.ProductoCampania)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As GestionComercial.ProductoCampania)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As GestionComercial.ProductoCampania

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), GestionComercial.ProductoCampania)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(GestionComercial.ProductoCampania).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next
        _cargado = False
        Return dtAux
    End Function

    Public Function CargarDatos()
        Dim dtDatos As New DataTable
        Dim dsDatos As New DataSet
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
        Dim objCampania As New NotusIlsService.NotusIlsService
        Dim infoWs As New InfoUrlService(objCampania, True)
        Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
        Wsresultado = objCampania.ConsultarCampaniaProducto(dsDatos, _idCampania, _tipoProducto)
        dtDatos = dsDatos.Tables(0)
        Return dtDatos
    End Function

#End Region

End Class