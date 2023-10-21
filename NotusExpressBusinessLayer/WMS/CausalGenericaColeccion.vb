Imports LMDataAccessLayer
Imports System.Reflection

Public Class CausalGenericaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idCausal As Integer
    Private _descripcion As String
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As CausalGenerica
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As CausalGenerica)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdCausal As Integer
        Get
            Return _idCausal
        End Get
        Set(value As Integer)
            _idCausal = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property
#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim pdv As Type = GetType(CausalGenerica)
        Dim pInfo As PropertyInfo

        For Each pInfo In pdv.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As CausalGenerica)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As CausalGenerica)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As CausalGenericaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As CausalGenerica)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal IdTipoLectura As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), CausalGenerica)
                If .IdCausal = IdTipoLectura Then
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
        Dim tdl As CausalGenerica

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            tdl = CType(Me.InnerList(index), CausalGenerica)
            If tdl IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(CausalGenerica).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(tdl, Nothing)
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
                .ejecutarReader("ObtenerinfoCausalGenerica", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim tdl As CausalGenerica
                    While .Reader.Read
                        tdl = New CausalGenerica
                        tdl.AsignarValorAPropiedades(.Reader)
                        Me.InnerList.Add(tdl)
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
