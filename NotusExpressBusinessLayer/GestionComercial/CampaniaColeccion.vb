Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class CampaniaColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"
    Private _idCampania As Integer
    Private _nombreCampania As String
    Private _listIdTipoServicio As ArrayList
    Private _listIdCiudad As ArrayList
    Private _idClienteExterno As Integer
    Private _activo As Nullable(Of Boolean)
    Private _cargado As Boolean
    Private _idUsuarioConsulta As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As Campania
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As Campania)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdCampania As Integer
        Get
            Return _idCampania
        End Get
        Set(value As Integer)
            _idCampania = value
        End Set
    End Property

    Public Property NombreCampania As String
        Get
            Return _nombreCampania
        End Get
        Set(value As String)
            _nombreCampania = value
        End Set
    End Property

    Public Property Activo As Nullable(Of Boolean)
        Get
            Return _activo
        End Get
        Set(value As Nullable(Of Boolean))
            _activo = value
        End Set
    End Property

    Public Property ListaTipoServicio As ArrayList
        Get
            If _listIdTipoServicio Is Nothing Then _listIdTipoServicio = New ArrayList
            Return _listIdTipoServicio
        End Get
        Set(value As ArrayList)
            _listIdTipoServicio = value
        End Set
    End Property

    Public Property ListaIdCiudad As ArrayList
        Get
            If _listIdCiudad Is Nothing Then _listIdCiudad = New ArrayList
            Return _listIdCiudad
        End Get
        Set(value As ArrayList)
            _listIdCiudad = value
        End Set
    End Property

    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
        End Set
    End Property

    Public Property IdUsuarioConsulta As Integer
        Get
            Return _idUsuarioConsulta
        End Get
        Set(value As Integer)
            _idUsuarioConsulta = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objAbreviaturaDireccion As Type = GetType(Campania)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Campania)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As Campania)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As Campania)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As campania

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), campania)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(campania).GetProperties
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
        Using dbManager As New LMDataAccess
            With dbManager
                Try
                    .SqlParametros.Clear()
                    If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If Not String.IsNullOrEmpty(_nombreCampania) Then .SqlParametros.Add("@nombreCampania", SqlDbType.VarChar).Value = _nombreCampania
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    If _idUsuarioConsulta > 0 Then
                        .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuarioConsulta
                    End If
                    .ejecutarReader("ObtieneCampaniasVentas", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As Campania

                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New Campania
                                Integer.TryParse(.Reader("idCampania"), elDetalle.IdCampania)
                                elDetalle.Nombre = .Reader("nombre")
                                elDetalle.FechaInicio = .Reader("fechaInicio")
                                If Not IsDBNull(.Reader("fechaFin")) Then elDetalle.FechaFin = .Reader("fechaFin")
                                elDetalle.Activo = .Reader("activo")
                                Integer.TryParse(.Reader("idSistemaOrigen"), elDetalle.IdSistema)
                                If Not String.IsNullOrEmpty(.Reader("esFinanciero")) Then Integer.TryParse(.Reader("esFinanciero").ToString, elDetalle.EsFinanciero)
                                If Not String.IsNullOrEmpty(.Reader("idClienteExterno")) Then Integer.TryParse(.Reader("idClienteExterno").ToString, elDetalle.IdClienteExterno)
                                _cargado = True
                                Me.InnerList.Add(elDetalle)
                            End If
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End With
        End Using
    End Sub

#End Region

End Class
