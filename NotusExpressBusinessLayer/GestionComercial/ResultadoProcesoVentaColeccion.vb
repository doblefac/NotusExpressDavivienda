Imports LMDataAccessLayer
Imports System.Reflection

Public Class ResultadoProcesoVentaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"
    Private _idEstado As Byte
    Private _cargado As Boolean
    Private _gestionCallCenter As Boolean = False
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As ResultadoProcesoVenta
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As ResultadoProcesoVenta)
            If value IsNot Nothing Then
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

    Public Property GestionCallCenter() As Byte
        Get
            Return _gestionCallCenter
        End Get
        Set(ByVal value As Byte)
            _gestionCallCenter = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim resultado As Type = GetType(ResultadoProcesoVenta)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ResultadoProcesoVenta)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ResultadoProcesoVenta)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ResultadoProcesoVentaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As ResultadoProcesoVenta)
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
            With CType(Me.InnerList(index), ResultadoProcesoVenta)
                If .IdResultado = identificador Then
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
        Dim resultado As ResultadoProcesoVenta

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            resultado = CType(Me.InnerList(index), ResultadoProcesoVenta)
            If resultado IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(ResultadoProcesoVenta).GetProperties
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
                If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = _idEstado
                .SqlParametros.Add("@gestionCallCenter", SqlDbType.Bit).Value = _gestionCallCenter
                .ejecutarReader("ObtenerInfoResultadoProcesoVenta", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim resultado As ResultadoProcesoVenta
                    While .Reader.Read
                        resultado = New ResultadoProcesoVenta
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
