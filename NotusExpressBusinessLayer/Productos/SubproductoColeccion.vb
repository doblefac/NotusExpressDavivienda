Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports System.Reflection

Namespace MaestroProductos

    Public Class SubproductoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idProducto As Integer
        Private _idTipoProducto As Short
        Private _comercializable As Enumerados.EstadoBinario
        Private _serializado As Enumerados.EstadoBinario
        Private _nombreSubproducto As String
        Private _idEstado As Byte
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombreSubproducto = ""
            _comercializable = Enumerados.EstadoBinario.NoEstablecido
            _serializado = Enumerados.EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Subproducto
            Get
                Dim elItem As Subproducto = Nothing
                If index >= 0 Then elItem = Me.InnerList.Item(index)
                Return elItem
            End Get
            Set(ByVal value As Subproducto)
                If value IsNot Nothing AndAlso Not value.Registrado Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdProductoPadre() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
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

        Public Property NombreSubproducto() As String
            Get
                Return _nombreSubproducto
            End Get
            Set(ByVal value As String)
                _nombreSubproducto = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objeto As Type = GetType(Subproducto)
            Dim pInfo As PropertyInfo

            For Each pInfo In objeto.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Subproducto)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Subproducto)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ProductoColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Subproducto)
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
                With CType(Me.InnerList(index), Subproducto)
                    If .IdSubproducto = identificador Then
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
            Dim infoObjeto As Subproducto

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                infoObjeto = CType(Me.InnerList(index), Subproducto)
                If infoObjeto IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Subproducto).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(infoObjeto, Nothing)
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

                    If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    If _idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTipoProducto
                    If _comercializable <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@comercializable", SqlDbType.Bit).Value = IIf(_comercializable = Enumerados.EstadoBinario.Activo, 1, 0)
                    If _serializado <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@serializado", SqlDbType.Bit).Value = IIf(_serializado = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Not String.IsNullOrEmpty(Me._nombreSubproducto) Then _
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 200).Value = Me._nombreSubproducto
                    If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = _idEstado
                    .ejecutarReader("ObtenerInfoSubproducto", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim infoObjeto As Subproducto
                        While .Reader.Read
                            infoObjeto = New Subproducto
                            infoObjeto.AsignarValorAPropiedades(.Reader)
                            Me.InnerList.Add(infoObjeto)
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
