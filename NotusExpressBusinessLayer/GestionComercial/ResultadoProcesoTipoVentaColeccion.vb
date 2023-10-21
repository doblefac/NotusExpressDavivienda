Imports LMDataAccessLayer
Imports System.Reflection

Public Class ResultadoProcesoTipoVentaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"
    Private _idResultadoProceso As Byte
    Private _idTipoVenta As Byte
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As ResultadoProcesoTipoVenta
        Get
            If index >= 0 Then
                Return Me.InnerList.Item(index)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As ResultadoProcesoTipoVenta)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdTipoVenta() As Byte
        Get
            Return _idTipoVenta
        End Get
        Set(ByVal value As Byte)
            _idTipoVenta = value
        End Set
    End Property

    Public Property IdResultadoProceso() As Byte
        Get
            Return _idResultadoProceso
        End Get
        Set(ByVal value As Byte)
            _idResultadoProceso = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim relacion As Type = GetType(ResultadoProcesoTipoVenta)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ResultadoProcesoTipoVenta)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ResultadoProcesoTipoVenta)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ResultadoProcesoTipoVentaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As ResultadoProcesoTipoVenta)
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
            With CType(Me.InnerList(index), ResultadoProcesoTipoVenta)
                If .IdRelacion = identificador Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function Elemento(ByVal identificador As Integer) As ResultadoProcesoTipoVenta
        Dim aux As ResultadoProcesoTipoVenta = Nothing
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), ResultadoProcesoTipoVenta)
                If .IdRelacion = identificador Then
                    aux = CType(Me.InnerList(index), ResultadoProcesoTipoVenta)
                    Exit For
                End If
            End With
        Next
        Return aux
    End Function

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim relacion As ResultadoProcesoTipoVenta

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            relacion = CType(Me.InnerList(index), ResultadoProcesoTipoVenta)
            If relacion IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(ResultadoProcesoTipoVenta).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(relacion, Nothing)
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
                If _idResultadoProceso > 0 Then .SqlParametros.Add("@idResultadoProceso", SqlDbType.TinyInt).Value = _idResultadoProceso
                If _idTipoVenta > 0 Then .SqlParametros.Add("@idTipoVenta", SqlDbType.TinyInt).Value = _idTipoVenta
                .ejecutarReader("ObtenerInfoResultadoProcesoTipoDeVenta", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim relacion As ResultadoProcesoTipoVenta
                    While .Reader.Read
                        relacion = New ResultadoProcesoTipoVenta
                        relacion.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(relacion)
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
