Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Public Class AbreviaturaDireccionaColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"
    Private _idEstado As Boolean
    Private _jerarquia As Integer
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As GestionComercial.AbreviaturaDireccion
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As GestionComercial.AbreviaturaDireccion)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdEstado As Boolean
        Get
            Return _idEstado
        End Get
        Set(value As Boolean)
            _idEstado = value
        End Set
    End Property

    Public Property Jerarquia As Integer
        Get
            Return _jerarquia
        End Get
        Set(value As Integer)
            _jerarquia = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objAbreviaturaDireccion As Type = GetType(GestionComercial.AbreviaturaDireccion)
        Dim pInfo As PropertyInfo

        For Each pInfo In objAbreviaturaDireccion.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GestionComercial.AbreviaturaDireccion)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As GestionComercial.AbreviaturaDireccion)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As GestionComercial.AbreviaturaDireccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As GestionComercial.AbreviaturaDireccion

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), GestionComercial.AbreviaturaDireccion)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(GestionComercial.AbreviaturaDireccion).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next
        _cargado = False
        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        If _cargado Then Me.InnerList.Clear()
        With dbManager
            .SqlParametros.Add("@idEstado", SqlDbType.Bit).Value = _idEstado
            .SqlParametros.Add("@jerarquia", SqlDbType.Int).Value = _jerarquia
            .ejecutarReader("ObtenerInfoAbreviaturasDirecciones", CommandType.StoredProcedure)
            Me.InnerList.Clear()
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objAbreviaturaDireccion As New GestionComercial.AbreviaturaDireccion
                While .Reader.Read
                    objAbreviaturaDireccion = New GestionComercial.AbreviaturaDireccion()
                    objAbreviaturaDireccion.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objAbreviaturaDireccion)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class

