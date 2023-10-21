Imports LMDataAccessLayer
Imports System.Reflection

Namespace PresupuestoGestiones

    Public Class DetallePresupuestoDeGestionesDeVentaColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idPresupuesto As Byte
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(idPresupuesto As Integer)
            _idPresupuesto = idPresupuesto
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As DetallePresupuestoDeGestionesDeVenta
            Get
                If index >= 0 Then
                    Return Me.InnerList.Item(index)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As DetallePresupuestoDeGestionesDeVenta)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdPresupuesto() As Byte
            Get
                Return _idPresupuesto
            End Get
            Set(ByVal value As Byte)
                _idPresupuesto = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim relacion As Type = GetType(DetallePresupuestoDeGestionesDeVenta)
            Dim pInfo As PropertyInfo

            For Each pInfo In relacion.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetallePresupuestoDeGestionesDeVenta)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As DetallePresupuestoDeGestionesDeVenta)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DetallePresupuestoDeGestionesDeVentaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As DetallePresupuestoDeGestionesDeVenta)
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
                With CType(Me.InnerList(index), DetallePresupuestoDeGestionesDeVenta)
                    If .IdDetalle = identificador Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function GenerarDataTable() As DataTable
            'If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim obj As DetallePresupuestoDeGestionesDeVenta

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                obj = CType(Me.InnerList(index), DetallePresupuestoDeGestionesDeVenta)
                If obj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(DetallePresupuestoDeGestionesDeVenta).GetProperties
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
            Dim dbManager As New LMDataAccess
            Try
                Me.InnerList.Clear()
                With dbManager
                    If _idPresupuesto > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = _idPresupuesto
                    .ejecutarReader("ObtenerDetallePresupuestoDeGestionesDeVenta", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim obj As DetallePresupuestoDeGestionesDeVenta
                        While .Reader.Read
                            obj = New DetallePresupuestoDeGestionesDeVenta
                            obj.CargarResultadoConsulta(.Reader)
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

#End Region

    End Class

End Namespace