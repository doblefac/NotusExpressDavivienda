Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class NovedadPorServicioColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _listIdNovedadServicio As List(Of Integer)
    Private _listIdNovedad As List(Of Integer)
    Private _listIdGestionVenta As List(Of Integer)
    Private _listIdEstado As List(Of Integer)

    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As NovedadPorServicio
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As NovedadPorServicio)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property ListIdNovedadServicio As List(Of Integer)
        Get
            If _listIdNovedadServicio Is Nothing Then _listIdNovedadServicio = New List(Of Integer)
            Return _listIdNovedadServicio
        End Get
        Set(value As List(Of Integer))
            _listIdNovedadServicio = value
        End Set
    End Property

    Public Property ListIdNovedad As List(Of Integer)
        Get
            If _listIdNovedad Is Nothing Then _listIdNovedad = New List(Of Integer)
            Return _listIdNovedad
        End Get
        Set(value As List(Of Integer))
            _listIdNovedad = value
        End Set
    End Property

    Public Property ListIdGestionVenta As List(Of Integer)
        Get
            If _listIdGestionVenta Is Nothing Then _listIdGestionVenta = New List(Of Integer)
            Return _listIdGestionVenta
        End Get
        Set(value As List(Of Integer))
            _listIdGestionVenta = value
        End Set
    End Property

    Public Property ListIdEstado As List(Of Integer)
        Get
            If _listIdEstado Is Nothing Then _listIdEstado = New List(Of Integer)
            Return _listIdEstado
        End Get
        Set(value As List(Of Integer))
            _listIdEstado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objNovedadPorServicio As Type = GetType(NovedadPorServicio)
        Dim pInfo As PropertyInfo

        For Each pInfo In objNovedadPorServicio.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As NovedadPorServicio)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As NovedadPorServicio)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As NovedadPorServicio)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As NovedadPorServicio

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), NovedadPorServicio)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(NovedadPorServicio).GetProperties
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
            If _listIdNovedadServicio IsNot Nothing AndAlso _listIdNovedadServicio.Count > 0 Then _
                .SqlParametros.Add("@listIdNovedadServicio", SqlDbType.VarChar).Value = String.Join(",", _listIdNovedadServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdNovedad IsNot Nothing AndAlso _listIdNovedad.Count > 0 Then _
                .SqlParametros.Add("@listIdNovedad", SqlDbType.VarChar).Value = String.Join(",", _listIdNovedad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdGestionVenta IsNot Nothing AndAlso _listIdGestionVenta.Count > 0 Then _
                .SqlParametros.Add("@listIdGestionVenta", SqlDbType.VarChar).Value = String.Join(",", _listIdGestionVenta.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdEstado IsNot Nothing AndAlso _listIdEstado.Count > 0 Then _
                .SqlParametros.Add("@listIdEstado", SqlDbType.VarChar).Value = String.Join(",", _listIdEstado.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

            .ejecutarReader("ObtenerInfoNovedadPorServicio", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objNovedadPorServicio As NovedadPorServicio
                While .Reader.Read
                    objNovedadPorServicio = New NovedadPorServicio()
                    objNovedadPorServicio.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objNovedadPorServicio)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
