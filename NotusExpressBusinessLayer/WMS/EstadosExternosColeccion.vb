Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Public Class EstadosExternosColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _listIdEstado As List(Of Integer)
    Private _listIdSistemaExterno As List(Of Integer)
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As EstadosExternos
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As EstadosExternos)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
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

    Public Property ListIdSistemaExterno As List(Of Integer)
        Get
            If _listIdSistemaExterno Is Nothing Then _listIdSistemaExterno = New List(Of Integer)
            Return _listIdSistemaExterno
        End Get
        Set(value As List(Of Integer))
            _listIdSistemaExterno = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objEstadosExternos As Type = GetType(EstadosExternos)
        Dim pInfo As PropertyInfo

        For Each pInfo In objEstadosExternos.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As EstadosExternos)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As EstadosExternos)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As EstadosExternos)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As EstadosExternos

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), EstadosExternos)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(EstadosExternos).GetProperties
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
            If _listIdEstado IsNot Nothing AndAlso _listIdEstado.Count > 0 Then _
                .SqlParametros.Add("@listIdEstado", SqlDbType.VarChar).Value = String.Join(",", _listIdEstado.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdSistemaExterno IsNot Nothing AndAlso _listIdSistemaExterno.Count > 0 Then _
                .SqlParametros.Add("@listIdSistema", SqlDbType.VarChar).Value = String.Join(",", _listIdSistemaExterno.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

            .ejecutarReader("ObtenerEstadosExternos", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objEstadosExternos As EstadosExternos
                While .Reader.Read
                    objEstadosExternos = New EstadosExternos()
                    objEstadosExternos.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objEstadosExternos)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

    Public Function ObtenerEstadoCalidad() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As New DataTable

        With dbManager
            dt = .EjecutarDataTable("ObtenerEstadosExternoCalidad", CommandType.StoredProcedure)
        End With

        If dbManager IsNot Nothing Then dbManager.Dispose()
        Return dt
    End Function

    Public Function ObtenerEstadoRechazoCalidad() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As New DataTable

        With dbManager
            dt = .EjecutarDataTable("ObtenerEstadosRechazoCalidad", CommandType.StoredProcedure)
        End With

        If dbManager IsNot Nothing Then dbManager.Dispose()
        Return dt
    End Function

#End Region

End Class
