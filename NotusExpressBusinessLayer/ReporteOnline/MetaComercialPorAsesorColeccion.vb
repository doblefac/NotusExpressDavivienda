Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.ConfiguracionComercial
Public Class MetaComercialPorAsesorColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idMeta As Integer
    Private _meta As Integer
    Private _cargado As Boolean
    Private _anio As Integer
    Private _mes As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _idMeta = 0
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As MetaComercial
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As MetaComercial)
            If value IsNot Nothing AndAlso Not value.Registrado Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
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

    Public Property Meta As Integer
        Get
            Return _meta
        End Get
        Set(value As Integer)
            _meta = value
        End Set
    End Property

    Public Property Anio As Integer
        Get
            Return _anio
        End Get
        Set(value As Integer)
            _anio = value
        End Set
    End Property

    Public Property Mes As Integer
        Get
            Return _mes
        End Get
        Set(value As Integer)
            _mes = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim obj As Type = GetType(MetaComercial)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As MetaComercial)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Add(ByVal valor As MetaComercial)
        Me.InnerList.Add(valor)
    End Sub


    Public Sub Remover(ByVal valor As MetaComercial)
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
            With CType(Me.InnerList(index), MetaComercial)
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
        Dim obj As MetaComercial

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            obj = CType(Me.InnerList(index), MetaComercial)
            If obj IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(MetaComercial).GetProperties
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
                'If Me._meta > 0 Then _
                '    .SqlParametros.Add("@meta", SqlDbType.Int).Value = Me._meta
                'If Me._anio > 0 Then _
                '    .SqlParametros.Add("@anio", SqlDbType.SmallInt).Value = Me._anio
                'If Me._mes > 0 Then _
                '    .SqlParametros.Add("@mes", SqlDbType.TinyInt).Value = Me._mes
                .SqlParametros.Add("@idMeta", SqlDbType.Int).Value = Me._idMeta
                .ejecutarReader("ObtenerInfoMetaComercialAsesor", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim obj As MetaComercial
                    While .Reader.Read
                        obj = New MetaComercial
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
