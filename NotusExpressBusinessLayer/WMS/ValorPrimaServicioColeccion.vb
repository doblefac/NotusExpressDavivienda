Imports LMDataAccessLayer
Imports System.Reflection

Public Class ValorPrimaServicioColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idValorPrimaServicio As Integer
    Private _valorPrimaServicio As Integer
    Private _idTipoProducto As Integer
    Private _idProducto As Integer
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As ValorPrimaServicio
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As ValorPrimaServicio)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdValorPrimaServicio() As Integer
        Get
            Return _idValorPrimaServicio
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idValorPrimaServicio = value
        End Set
    End Property

    Public Property ValorPrimaServicio() As Integer
        Get
            Return _valorPrimaServicio
        End Get
        Protected Friend Set(ByVal value As Integer)
            _valorPrimaServicio = value
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

    Public Property IdProducto As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim resultado As Type = GetType(ValorPrimaServicio)
        Dim pInfo As PropertyInfo

        For Each pInfo In resultado.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ValorPrimaServicio)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ValorPrimaServicio)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ValorPrimaServicio)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As ValorPrimaServicio)
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
            With CType(Me.InnerList(index), ValorPrimaServicio)
                If .IdValorPrimaServicio = identificador Then
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
        Dim resultado As ValorPrimaServicio

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            resultado = CType(Me.InnerList(index), ValorPrimaServicio)
            If resultado IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(ValorPrimaServicio).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(resultado, Nothing)
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
                With .SqlParametros
                    If _idValorPrimaServicio > 0 Then .Add("@idValorPrimaServicio", SqlDbType.Int).Value = _idValorPrimaServicio
                    If _valorPrimaServicio > 0 Then .Add("@valorPrimaServicio", SqlDbType.Int).Value = _valorPrimaServicio
                    If _idTipoProducto > 0 Then .Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                    If IdProducto > 0 Then .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                End With
                .ejecutarReader("ObtenerInfoValorPrima", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim resultado As ValorPrimaServicio
                    While .Reader.Read
                        resultado = New ValorPrimaServicio
                        resultado.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(resultado)
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