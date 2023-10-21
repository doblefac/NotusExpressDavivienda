Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Namespace GestionComercial

    Public Class GestionDeVentaDetalleColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _listIdDetalle As List(Of Long)
        Private _listGestionVenta As List(Of Long)
        Private _listCliente As List(Of Long)
        Private _listProducto As List(Of Integer)

        Private _cargado As Boolean

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As GestionComercial.GestionDeVentaDetalle
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As GestionComercial.GestionDeVentaDetalle)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property listIdDetalle As List(Of Long)
            Get
                If _listIdDetalle Is Nothing Then _listIdDetalle = New List(Of Long)
                Return _listIdDetalle
            End Get
            Set(value As List(Of Long))
                _listIdDetalle = value
            End Set
        End Property

        Public Property listGestionVenta As List(Of Long)
            Get
                If _listGestionVenta Is Nothing Then _listGestionVenta = New List(Of Long)
                Return _listGestionVenta
            End Get
            Set(value As List(Of Long))
                _listGestionVenta = value
            End Set
        End Property

        Public Property listCliente As List(Of Long)
            Get
                If _listCliente Is Nothing Then _listCliente = New List(Of Long)
                Return _listCliente
            End Get
            Set(value As List(Of Long))
                _listCliente = value
            End Set
        End Property

        Public Property listProducto As List(Of Integer)
            Get
                If _listProducto Is Nothing Then _listProducto = New List(Of Integer)
                Return _listProducto
            End Get
            Set(value As List(Of Integer))
                _listProducto = value
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
            Dim objGestionDeServicio As Type = GetType(GestionComercial.GestionDeVentaDetalle)
            Dim pInfo As PropertyInfo

            For Each pInfo In objGestionDeServicio.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GestionComercial.GestionDeVentaDetalle)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As GestionComercial.GestionDeVentaDetalle)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As GestionComercial.GestionDeVentaDetalle)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Function GenerarDataTable() As DataTable
            If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miRegistro As GestionComercial.GestionDeVentaDetalle

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), GestionComercial.GestionDeVentaDetalle)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(GestionComercial.GestionDeVentaDetalle).GetProperties
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
                If _listIdDetalle IsNot Nothing AndAlso _listIdDetalle.Count > 0 Then _
                                .SqlParametros.Add("@listIdDetalle", SqlDbType.VarChar).Value = String.Join(",", _listIdDetalle.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listGestionVenta IsNot Nothing AndAlso _listGestionVenta.Count > 0 Then _
                                .SqlParametros.Add("@listGestionVenta", SqlDbType.VarChar).Value = String.Join(",", _listGestionVenta.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listCliente IsNot Nothing AndAlso _listCliente.Count > 0 Then _
                                .SqlParametros.Add("@listCliente", SqlDbType.VarChar).Value = String.Join(",", _listCliente.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listProducto IsNot Nothing AndAlso _listProducto.Count > 0 Then _
                                .SqlParametros.Add("@listProducto", SqlDbType.VarChar).Value = String.Join(",", _listProducto.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                .ejecutarReader("ObtenerInfoGestionVentaDetalle", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objGestionDeServicio As GestionComercial.GestionDeVentaDetalle
                    While .Reader.Read
                        objGestionDeServicio = New GestionComercial.GestionDeVentaDetalle()
                        objGestionDeServicio.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(objGestionDeServicio)
                    End While
                    _cargado = True
                End If
            End With
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Sub

#End Region

    End Class

End Namespace