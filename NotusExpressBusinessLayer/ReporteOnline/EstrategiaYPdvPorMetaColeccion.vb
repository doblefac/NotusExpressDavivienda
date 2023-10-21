﻿Imports LMDataAccessLayer
Imports System.Reflection

Public Class EstrategiaYPdvPorMetaColeccion
    Inherits CollectionBase


#Region "Atributos (Filtros de Búsqueda)"

    Private _estrategia As String
    Private _pdv As String
    Private _idMeta As Integer
    Private _idPdv As Integer
    Private _idEstrategia As Integer
    Private _idTipoProducto As Integer
    Private _producto As String
    Private _meta As Integer
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _idMeta = 0
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As EstrategiaYPdvPorMeta
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As EstrategiaYPdvPorMeta)
            If value IsNot Nothing AndAlso Not value.Registrado Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property Estrategia As String
        Get
            Return _estrategia
        End Get
        Set(value As String)
            _estrategia = value
        End Set
    End Property

    Public Property IdMeta As Integer
        Get
            Return _idMeta
        End Get
        Set(value As Integer)
            _idMeta = value
        End Set
    End Property

    Public Property IdPdv As Integer
        Get
            Return _idPdv
        End Get
        Set(value As Integer)
            _idPdv = value
        End Set
    End Property

    Public Property IdEstrategia As Integer
        Get
            Return _idEstrategia
        End Get
        Set(value As Integer)
            _idEstrategia = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property Pdv As String
        Get
            Return _pdv
        End Get
        Set(value As String)
            _pdv = value
        End Set
    End Property

    Public Property Producto As String
        Get
            Return _producto
        End Get
        Set(value As String)
            _producto = value
        End Set
    End Property

    Public Property Meta As Integer
        Get
            Return _meta
        End Get
        Set(value As Integer)
            _meta = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim obj As Type = GetType(EstrategiaYPdvPorMeta)
        Dim pInfo As PropertyInfo

        For Each pInfo In obj.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As EstrategiaYPdvPorMeta)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Add(ByVal valor As EstrategiaYPdvPorMeta)
        Me.InnerList.Add(valor)
    End Sub


    Public Sub Remover(ByVal valor As EstrategiaYPdvPorMeta)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal identificador As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), EstrategiaYPdvPorMeta)
                If .IdMeta = identificador Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim obj As EstrategiaYPdvPorMeta

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            obj = CType(Me.InnerList(index), EstrategiaYPdvPorMeta)
            If obj IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(EstrategiaYPdvPorMeta).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(obj, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As LMDataAccess = Nothing
        Try
            dbManager = New LMDataAccess
            Me.Clear()
            With dbManager
                If Me._idPdv > 0 Then _
                    .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = Me._idPdv
                If Me._idEstrategia > 0 Then _
                    .SqlParametros.Add("@idEstrategia", SqlDbType.SmallInt).Value = Me._idEstrategia
                If Me._idTipoProducto > 0 Then _
                    .SqlParametros.Add("@idTipoProducto", SqlDbType.TinyInt).Value = Me._idTipoProducto
                .ejecutarReader("ObtenerDetalleEstrategiaYPdvPorMeta", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim obj As EstrategiaYPdvPorMeta
                    While .Reader.Read
                        obj = New EstrategiaYPdvPorMeta
                        obj.AsignarValorAPropiedades(.Reader)
                        Me.InnerList.Add(obj)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
#End Region
End Class
