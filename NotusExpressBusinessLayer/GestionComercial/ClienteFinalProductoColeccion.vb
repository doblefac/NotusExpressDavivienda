Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class ClienteFinalProductoColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _listaRegistro As List(Of Long)
    Private _listaIdCliente As List(Of Integer)
    Private _listaIdProducto As List(Of Integer)
    
    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As ClienteFinalProducto
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As ClienteFinalProducto)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property ListaRegistro As List(Of Long)
        Get
            If _listaRegistro Is Nothing Then _listaRegistro = New List(Of Long)
            Return _listaRegistro
        End Get
        Set(value As List(Of Long))
            _listaRegistro = value
        End Set
    End Property

    Public Property ListaIdCliente As List(Of Integer)
        Get
            If _listaIdCliente Is Nothing Then _listaIdCliente = New List(Of Integer)
            Return _listaIdCliente
        End Get
        Set(value As List(Of Integer))
            _listaIdCliente = value
        End Set
    End Property

    Public Property ListaIdProducto As List(Of Integer)
        Get
            If _listaIdProducto Is Nothing Then _listaIdProducto = New List(Of Integer)
            Return _listaIdProducto
        End Get
        Set(value As List(Of Integer))
            _listaIdProducto = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objClienteFinalProducto As Type = GetType(ClienteFinalProducto)
        Dim pInfo As PropertyInfo

        For Each pInfo In objClienteFinalProducto.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ClienteFinalProducto)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ClienteFinalProducto)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ClienteFinalProducto)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As ClienteFinalProducto

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), ClienteFinalProducto)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(ClienteFinalProducto).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        If _cargado Then Me.InnerList.Clear()
        With dbManager
            If _listaRegistro IsNot Nothing AndAlso _listaRegistro.Count > 0 Then _
                        .SqlParametros.Add("@listaRegistro", SqlDbType.VarChar).Value = String.Join(",", _listaRegistro.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listaIdCliente IsNot Nothing AndAlso _listaIdCliente.Count > 0 Then _
                        .SqlParametros.Add("@listaIdCliente", SqlDbType.VarChar).Value = String.Join(",", _listaIdCliente.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listaIdProducto IsNot Nothing AndAlso _listaIdProducto.Count > 0 Then _
                        .SqlParametros.Add("@listaIdProducto", SqlDbType.VarChar).Value = String.Join(",", _listaIdProducto.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            .ejecutarReader("ObtenerClienteFinalProducto", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objClienteFinalProducto As ClienteFinalProducto
                While .Reader.Read
                    objClienteFinalProducto = New ClienteFinalProducto()
                    objClienteFinalProducto.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objClienteFinalProducto)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
