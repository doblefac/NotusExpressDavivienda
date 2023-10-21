Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports System.Reflection

Public Class EstrategiaComercialColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _nombre As String
    Private _idEstado As Enumerados.EstadoBinario
    Private _cargado As Boolean
    Private _idCanal As Integer
    Private _canal As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _idEstado = Enumerados.EstadoBinario.NoEstablecido
        _idCanal = 0
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As EstrategiaComercial
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As EstrategiaComercial)
            If value IsNot Nothing AndAlso Not value.Registrado Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
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

    Public Property Canal As String
        Get
            Return _canal
        End Get
        Set(value As String)
            _canal = value
        End Set
    End Property

    Public Property IdEstado As Enumerados.EstadoBinario
        Get
            Return _idEstado
        End Get
        Set(value As Enumerados.EstadoBinario)
            _idEstado = value
        End Set
    End Property

    Public Property IdCanal As Integer
        Get
            Return _idCanal
        End Get
        Set(value As Integer)
            _idCanal = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim obj As Type = GetType(EstrategiaComercial)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As EstrategiaComercial)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Add(ByVal valor As EstrategiaComercial)
        Me.InnerList.Add(valor)
    End Sub


    Public Sub Remover(ByVal valor As EstrategiaComercial)
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
            With CType(Me.InnerList(index), EstrategiaComercial)
                If .IdEstrategia = identificador Then
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
        Dim obj As EstrategiaComercial

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            obj = CType(Me.InnerList(index), EstrategiaComercial)
            If obj IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(EstrategiaComercial).GetProperties
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
                If Me._idEstado <> Enumerados.EstadoBinario.NoEstablecido Then _
                    .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = IIf(Me._idEstado = Enumerados.EstadoBinario.Activo, 1, 0)
                If Not String.IsNullOrEmpty(Me._nombre) Then _
                    .SqlParametros.Add("@nombre", SqlDbType.VarChar, 150).Value = Me._nombre
                If Me._idCanal > 0 Then _
                    .SqlParametros.Add("@idCanal", SqlDbType.Int).Value = Me._idCanal
                .ejecutarReader("ObtenerInfoEstrategiaComercial", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim obj As EstrategiaComercial
                    While .Reader.Read
                        obj = New EstrategiaComercial
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
