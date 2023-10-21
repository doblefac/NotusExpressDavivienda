Imports LMDataAccessLayer
Imports System.Reflection

Public Class ListadoEstadosColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"
    Private _idEstado As Integer
    Private _idEntidad As Integer
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As EstadosSeriales
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As EstadosSeriales)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property IdEntidad() As Integer
        Get
            Return _idEntidad
        End Get
        Set(ByVal value As Integer)
            _idEntidad = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim resultado As Type = GetType(EstadosSeriales)
        Dim pInfo As PropertyInfo

        For Each pInfo In resultado.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As EstadosSeriales)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As EstadosSeriales)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As EstadosSerialesColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As EstadosSeriales)
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
            With CType(Me.InnerList(index), EstadosSeriales)
                If .IdEstado = identificador Then
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
        Dim resultado As EstadosSeriales

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            resultado = CType(Me.InnerList(index), EstadosSeriales)
            If resultado IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(EstadosSeriales).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(resultado, Nothing)
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
                If _idEntidad > 0 Then .SqlParametros.Add("@idEntidad", SqlDbType.Int).Value = _idEntidad
                .ejecutarReader("ObtenerInfoEstados", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim resultado As EstadosSeriales
                    While .Reader.Read
                        resultado = New EstadosSeriales
                        resultado.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(resultado)
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
