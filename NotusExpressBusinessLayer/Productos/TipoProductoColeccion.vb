Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports System.Reflection

Namespace MaestroProductos

    Public Class TipoProductoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _ventaPresencial As Enumerados.EstadoBinario
        Private _idCategoria As Integer
        Private _idEstado As Enumerados.EstadoBinario
        Private _listCategoria As List(Of Integer)
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _idEstado = Enumerados.EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As TipoProducto
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TipoProducto)
                If value IsNot Nothing AndAlso Not value.Registrado Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property VentaPresencial As Enumerados.EstadoBinario
            Get
                Return _ventaPresencial
            End Get
            Set(value As Enumerados.EstadoBinario)
                _ventaPresencial = value
            End Set
        End Property

        Public Property IdCategoria As Integer
            Get
                Return _idCategoria
            End Get
            Set(value As Integer)
                _idCategoria = value
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

        Public Property ListCategoria As List(Of Integer)
            Get
                If _listCategoria Is Nothing Then _listCategoria = New List(Of Integer)
                Return _listCategoria
            End Get
            Set(value As List(Of Integer))
                _listCategoria = value
            End Set
        End Property


#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim obj As Type = GetType(TipoProducto)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoProducto)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As TipoProducto)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ProductoColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As TipoProducto)
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
                With CType(Me.InnerList(index), TipoProducto)
                    If .IdTipoProducto = identificador Then
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
            Dim obj As TipoProducto

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                obj = CType(Me.InnerList(index), TipoProducto)
                If obj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(TipoProducto).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(obj, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Function GenerarDataTableServicioCategoria() As DataTable
            If Not _cargado Then CargarInformacionServicioCategoria()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim obj As TipoProducto

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                obj = CType(Me.InnerList(index), TipoProducto)
                If obj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(TipoProducto).GetProperties
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
                    If Me._ventaPresencial <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@ventaPresencial", SqlDbType.Bit).Value = IIf(Me._ventaPresencial = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Me._idCategoria > 0 Then _
                        .SqlParametros.Add("@idCategoria", SqlDbType.Int).Value = _idCategoria
                    If _listCategoria IsNot Nothing AndAlso _listCategoria.Count > 0 Then _
                        .SqlParametros.Add("@listaCategoria", SqlDbType.VarChar).Value = String.Join(",", _listCategoria.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    .ejecutarReader("ObtenerInfoTipoProducto", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim obj As TipoProducto
                        While .Reader.Read
                            obj = New TipoProducto
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

        Public Sub CargarInformacionServicioCategoria()

            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idCategoria", SqlDbType.Int).Value = Me._idCategoria
                    .ejecutarReader("ObtenerInfoTipoProductoCategoria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim obj As TipoProducto
                        While .Reader.Read
                            obj = New TipoProducto
                            obj.AsignarValorAPropiedades(.Reader)
                            Me.InnerList.Add(obj)
                        End While
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace