Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports System.Reflection

Public Class CumplimientoColeccion
    Inherits CollectionBase

#Region "Parámetros"

    Private _idCumplimiento As Integer
    Private _inicio1 As Double
    Private _fin1 As Double
    Private _rutaColor1 As String
    Private _inicio2 As Double
    Private _fin2 As Double
    Private _rutaColor2 As String
    Private _inicio3 As Double
    Private _fin3 As Double
    Private _rutaColor3 As String
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()

    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As Cumplimiento
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As Cumplimiento)
            If value IsNot Nothing AndAlso Not value.Registrado Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdCumplimiento As Integer
        Get
            Return _idCumplimiento
        End Get
        Set(ByVal value As Integer)
            _idCumplimiento = value
        End Set
    End Property

    Public Property Inicio1 As Double
        Get
            Return _inicio1
        End Get
        Set(ByVal value As Double)
            _inicio1 = value
        End Set
    End Property

    Public Property Fin1 As Double
        Get
            Return _fin1
        End Get
        Set(ByVal value As Double)
            _fin1 = value
        End Set
    End Property

    Public Property RutaColor1 As String
        Get
            Return _rutaColor1
        End Get
        Set(ByVal value As String)
            _rutaColor1 = value
        End Set
    End Property

    Public Property Inicio2 As Double
        Get
            Return _inicio2
        End Get
        Set(ByVal value As Double)
            _inicio2 = value
        End Set
    End Property

    Public Property Fin2 As Double
        Get
            Return _fin2
        End Get
        Set(ByVal value As Double)
            _fin2 = value
        End Set
    End Property

    Public Property RutaColor2 As String
        Get
            Return _rutaColor2
        End Get
        Set(ByVal value As String)
            _rutaColor2 = value
        End Set
    End Property

    Public Property Inicio3 As Double
        Get
            Return _inicio3
        End Get
        Set(ByVal value As Double)
            _inicio3 = value
        End Set
    End Property

    Public Property Fin3 As Double
        Get
            Return _fin3
        End Get
        Set(ByVal value As Double)
            _fin3 = value
        End Set
    End Property

    Public Property RutaColor3 As String
        Get
            Return _rutaColor3
        End Get
        Set(ByVal value As String)
            _rutaColor3 = value
        End Set
    End Property
#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim obj As Type = GetType(Cumplimiento)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Cumplimiento)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Add(ByVal valor As Cumplimiento)
        Me.InnerList.Add(valor)
    End Sub


    Public Sub Remover(ByVal valor As Cumplimiento)
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
            With CType(Me.InnerList(index), Cumplimiento)
                If .IdCumplimiento = identificador Then
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
        Dim obj As Cumplimiento

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            obj = CType(Me.InnerList(index), Cumplimiento)
            If obj IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(Cumplimiento).GetProperties
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
                .ejecutarReader("ObtenerInfoCumplimiento", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim obj As Cumplimiento
                    While .Reader.Read
                        obj = New Cumplimiento
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
#End Region
End Class
