Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Public Class EstatusLaboralColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"
    Private _idEstado As EstadoBinario
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _idEstado = EstadoBinario.Activo
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As EstatusLaboral
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As EstatusLaboral)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdEstado() As EstadoBinario
        Get
            Return _idEstado
        End Get
        Set(ByVal value As EstadoBinario)
            _idEstado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miEstatus As Type = GetType(EstatusLaboral)
        Dim pInfo As PropertyInfo

        For Each pInfo In miEstatus.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As EstatusLaboral)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As EstatusLaboral)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As EstatusLaboralColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As EstatusLaboral)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idEstatus As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), EstatusLaboral)
                If .IdEstatusLaboral = idEstatus Then
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
        Dim miEstatus As EstatusLaboral

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miEstatus = CType(Me.InnerList(index), EstatusLaboral)
            If miEstatus IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(EstatusLaboral).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miEstatus, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                .ejecutarReader("ObtenerListaEstatusLaboral", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim miEstatus As EstatusLaboral
                    While .Reader.Read
                        miEstatus = New EstatusLaboral
                        miEstatus.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(miEstatus)
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
