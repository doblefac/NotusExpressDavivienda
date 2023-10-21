Imports LMDataAccessLayer
Imports System.Reflection

Namespace PresupuestoGestiones

    Public Class CorteReportePresupuestoGestionesColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _fecha As Date
        Private _horaActual As Date
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _fecha = Now
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As CorteReportePresupuestoGestiones
            Get
                If index >= 0 Then
                    Return Me.InnerList.Item(index)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As CorteReportePresupuestoGestiones)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property Fecha() As Date
            Get
                Return _fecha
            End Get
            Set(ByVal value As Date)
                _fecha = value
            End Set
        End Property

        Public Property HoraActual() As Date
            Get
                Return _horaActual
            End Get
            Set(ByVal value As Date)
                _horaActual = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim relacion As Type = GetType(CorteReportePresupuestoGestiones)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As CorteReportePresupuestoGestiones)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As CorteReportePresupuestoGestiones)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As CorteReportePresupuestoGestionesColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As CorteReportePresupuestoGestiones)
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
                With CType(Me.InnerList(index), CorteReportePresupuestoGestiones)
                    If .IdCorte = identificador Then
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
            Dim obj As CorteReportePresupuestoGestiones

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                obj = CType(Me.InnerList(index), CorteReportePresupuestoGestiones)
                If obj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(CorteReportePresupuestoGestiones).GetProperties
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
                    If _fecha > Date.MinValue Then .SqlParametros.Add("@fecha", SqlDbType.Date).Value = _fecha
                    If _horaActual > Date.MinValue Then .SqlParametros.Add("@horaActual", SqlDbType.Time).Value = _horaActual.TimeOfDay
                    .ejecutarReader("ObtenerCorteReportePresupuestoGestiones", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim obj As CorteReportePresupuestoGestiones
                        While .Reader.Read
                            obj = New CorteReportePresupuestoGestiones
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