Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports System.Reflection

Namespace MaestroProductos

    Public Class ProductoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipoProducto As Short
        'Private _ventaPresencial As Enumerados.EstadoBinario
        Private _ventaPresencial As Nullable(Of Boolean)
        Private _comercializable As Enumerados.EstadoBinario
        Private _serializado As Enumerados.EstadoBinario
        Private _nombreProducto As String
        Private _idTipoVenta As Short
        Private _idEstado As Byte
        Private _cargado As Boolean
        Private _idCategoria As Integer

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombreProducto = ""
            _comercializable = Enumerados.EstadoBinario.NoEstablecido
            _serializado = Enumerados.EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Producto
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As Producto)
                If value IsNot Nothing AndAlso Not value.Registrado Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdEstado() As Byte
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Byte)
                _idEstado = value
            End Set
        End Property

        Public Property IdTipoProducto() As Byte
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Byte)
                _idTipoProducto = value
            End Set
        End Property

        Public Property VentaPresencial() As Nullable(Of Boolean)
            Get
                Return _ventaPresencial
            End Get
            Set(value As Nullable(Of Boolean))
                _ventaPresencial = value
            End Set
        End Property

        Public Property Comercializable() As Enumerados.EstadoBinario
            Get
                Return _comercializable
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _comercializable = value
            End Set
        End Property

        Public Property Serializado() As Enumerados.EstadoBinario
            Get
                Return _serializado
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _serializado = value
            End Set
        End Property

        Public Property NombreProducto() As String
            Get
                Return _nombreProducto
            End Get
            Set(ByVal value As String)
                _nombreProducto = value
            End Set
        End Property

        Public Property IdTipoVenta() As Short
            Get
                Return _idTipoVenta
            End Get
            Set(ByVal value As Short)
                _idTipoVenta = value
            End Set
        End Property

        Public Property IdCategoria() As Integer
            Get
                Return _idCategoria
            End Get
            Set(ByVal value As Integer)
                _idCategoria = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim tipoVenta As Type = GetType(Producto)
            Dim pInfo As PropertyInfo

            For Each pInfo In tipoVenta.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Producto)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Producto)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ProductoColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Producto)
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
                With CType(Me.InnerList(index), Producto)
                    If .IdProducto = identificador Then
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
            Dim infoProducto As Producto

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                infoProducto = CType(Me.InnerList(index), Producto)
                If infoProducto IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Producto).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(infoProducto, Nothing)
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

                    If _idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTipoProducto
                    If _comercializable <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@comercializable", SqlDbType.Bit).Value = IIf(_comercializable = Enumerados.EstadoBinario.Activo, 1, 0)
                    If _serializado <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@serializado", SqlDbType.Bit).Value = IIf(_serializado = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Not String.IsNullOrEmpty(Me._nombreProducto) Then _
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 200).Value = Me._nombreProducto
                    'If Me._ventaPresencial <> Enumerados.EstadoBinario.NoEstablecido Then _
                    '    .SqlParametros.Add("@ventaPresencial", SqlDbType.Bit).Value = IIf(Me._ventaPresencial = Enumerados.EstadoBinario.Activo, 1, 0)
                    If _ventaPresencial IsNot Nothing Then .SqlParametros.Add("@ventaPresencial", SqlDbType.Bit).Value = _ventaPresencial
                    If _idTipoVenta > 0 Then .SqlParametros.Add("@idTipoVenta", SqlDbType.SmallInt).Value = _idTipoVenta
                    If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = _idEstado
                    If _idCategoria > 0 Then .SqlParametros.Add("@idCategoria", SqlDbType.TinyInt).Value = _idCategoria
                    .ejecutarReader("ObtenerInfoProducto", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim infoProducto As Producto
                        While .Reader.Read
                            infoProducto = New Producto
                            infoProducto.AsignarValorAPropiedades(.Reader)
                            Me.InnerList.Add(infoProducto)
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

End Namespace