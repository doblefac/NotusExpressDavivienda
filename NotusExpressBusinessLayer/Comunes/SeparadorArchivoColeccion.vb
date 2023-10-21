Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Public Class SeparadorArchivoColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"
    Private _idSeparador As Integer
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As SeparadorArchivo
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As SeparadorArchivo)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdSeparador As Integer
        Get
            Return _idSeparador
        End Get
        Set(value As Integer)
            _idSeparador = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objAbreviaturaDireccion As Type = GetType(SeparadorArchivo)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As SeparadorArchivo)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As SeparadorArchivo)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As SeparadorArchivo)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As SeparadorArchivo

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), SeparadorArchivo)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(SeparadorArchivo).GetProperties
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
            If _idSeparador > 0 Then .SqlParametros.Add("@idSeparador", SqlDbType.Int).Value = _idSeparador
            .ejecutarReader("ObtenerInfoSeparadorArchivo", CommandType.StoredProcedure)
            Me.InnerList.Clear()
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objColumnas As New SeparadorArchivo
                While .Reader.Read
                    objColumnas = New SeparadorArchivo()
                    objColumnas.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objColumnas)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class


