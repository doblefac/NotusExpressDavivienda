Imports LMDataAccessLayer
Imports System.Reflection

Public Class ClienteFinalColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"
    Private _idAgente As Integer
    Private _idCiudad As Integer
    Private _numeroIdentificacion As String
    Private _idEstrategia As Integer
    Private _fechaInicio As Date
    Private _fechaFin As Date
    Private _idCausal As Integer
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As ClienteFinal
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As ClienteFinal)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdAgente As Integer
        Get
            Return _idAgente
        End Get
        Set(value As Integer)
            _idAgente = value
        End Set
    End Property

    Public Property IdCiudad As Integer
        Get
            Return _idCiudad
        End Get
        Set(value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property NumeroIdentificacion As String
        Get
            Return _numeroIdentificacion
        End Get
        Set(value As String)
            _numeroIdentificacion = value
        End Set
    End Property

    Public Property IdEstrategia As Integer
        Get
            Return _idEstrategia
        End Get
        Set(value As Integer)
            _idEstrategia = value
        End Set
    End Property

    Public Property FechaInicio As Date
        Get
            Return _fechaInicio
        End Get
        Set(value As Date)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFin As Date
        Get
            Return _fechaFin
        End Get
        Set(value As Date)
            _fechaFin = value
        End Set
    End Property

    Public Property IdCausal As Integer
        Get
            Return _idCausal
        End Get
        Set(value As Integer)
            _idCausal = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miClienteFinal As Type = GetType(ClienteFinal)
        Dim pInfo As PropertyInfo

        For Each pInfo In miClienteFinal.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ClienteFinal)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ClienteFinal)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ClienteFinalColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As ClienteFinal)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idCliente As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), ClienteFinal)
                If .IdCliente = idCliente Then
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
        Dim miCliente As ClienteFinal

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miCliente = CType(Me.InnerList(index), ClienteFinal)
            If miCliente IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(ClienteFinal).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miCliente, Nothing)
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
                If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                If _numeroIdentificacion IsNot Nothing Then .SqlParametros.Add("@Identificacion", SqlDbType.VarChar).Value = _numeroIdentificacion
                If _idEstrategia > 0 Then .SqlParametros.Add("@idEstrategia", SqlDbType.Int).Value = _idEstrategia
                If _fechaInicio <> Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.Date).Value = _fechaInicio
                If _fechaFin <> Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.Date).Value = _fechaFin
                If _idCausal > 0 Then .SqlParametros.Add("@idCausal", SqlDbType.Int).Value = _idCausal
                .ejecutarReader("ObtenerInfoClienteFinal", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim miCliente As ClienteFinal
                    While .Reader.Read
                        miCliente = New ClienteFinal
                        miCliente.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(miCliente)
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
