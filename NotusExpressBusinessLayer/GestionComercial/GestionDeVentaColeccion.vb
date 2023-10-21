Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Public Class GestionDeVentaColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _listIdCliente As List(Of Integer)
    Private _listGestionVenta As List(Of Long)
    Private _fechaInicio As Date
    Private _fechaFinal As Date
    Private _fechaInicioRec As Date
    Private _fechaFinalRec As Date
    Private _idEstrategiaComercial As List(Of Integer)
    Private _idAsesor As List(Of Integer)
    Private _idBase As List(Of Integer)
    Private _idEstado As List(Of Integer)
    Private _identificacionCliente As List(Of String)
    Private _idOportunidad As List(Of String)
    Private _listIdPdv As List(Of Integer)
    Private _listIdResultadoProceso As List(Of Integer)
    Private _listIdEstadoNotus As List(Of Integer)
    Private _listIdCiudad As List(Of Integer)
    Private _listIdEstadoNovedad As List(Of Integer)
    Private _listIdNovedad As List(Of Integer)
    Private _listIdCausal As List(Of Integer)
    Private _cargado As Boolean
    Private _nuevaVentaDifCampania As Boolean
    Private _idCampania As String
    Private _idUsuarioConsulta As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As GestionComercial.GestionDeVenta
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As GestionComercial.GestionDeVenta)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property listGestionVenta As List(Of Long)
        Get
            If _listGestionVenta Is Nothing Then _listGestionVenta = New List(Of Long)
            Return _listGestionVenta
        End Get
        Set(value As List(Of Long))
            _listGestionVenta = value
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
            Return _fechaFinal
        End Get
        Set(value As Date)
            _fechaFinal = value
        End Set
    End Property

    Public Property FechaInicioRecepcion As Date
        Get
            Return _fechaInicioRec
        End Get
        Set(value As Date)
            _fechaInicioRec = value
        End Set
    End Property

    Public Property FechaFinRecepcion As Date
        Get
            Return _fechaFinalRec
        End Get
        Set(value As Date)
            _fechaFinalRec = value
        End Set
    End Property

    Public Property IdEstrategiaComercial As List(Of Integer)
        Get
            If _idEstrategiaComercial Is Nothing Then _idEstrategiaComercial = New List(Of Integer)
            Return _idEstrategiaComercial
        End Get
        Set(value As List(Of Integer))
            _idEstrategiaComercial = value
        End Set
    End Property

    Public Property IdAsesor As List(Of Integer)
        Get
            If _idAsesor Is Nothing Then _idAsesor = New List(Of Integer)
            Return _idAsesor
        End Get
        Set(value As List(Of Integer))
            _idAsesor = value
        End Set
    End Property

    Public Property IdBase As List(Of Integer)
        Get
            If _idBase Is Nothing Then _idBase = New List(Of Integer)
            Return _idBase
        End Get
        Set(value As List(Of Integer))
            _idBase = value
        End Set
    End Property

    Public Property IdEstado As List(Of Integer)
        Get
            If _idEstado Is Nothing Then _idEstado = New List(Of Integer)
            Return _idEstado
        End Get
        Set(value As List(Of Integer))
            _idEstado = value
        End Set
    End Property

    Public Property IdentificacionCliente As List(Of String)
        Get
            If _identificacionCliente Is Nothing Then _identificacionCliente = New List(Of String)
            Return _identificacionCliente
        End Get
        Set(value As List(Of String))
            _identificacionCliente = value
        End Set
    End Property

    Public Property IdOportunidad As List(Of String)
        Get
            If _idOportunidad Is Nothing Then _idOportunidad = New List(Of String)
            Return _idOportunidad
        End Get
        Set(value As List(Of String))
            _idOportunidad = value
        End Set
    End Property

    Public Property ListIdCliente As List(Of Integer)
        Get
            If _listIdCliente Is Nothing Then _listIdCliente = New List(Of Integer)
            Return _listIdCliente
        End Get
        Set(value As List(Of Integer))
            _listIdCliente = value
        End Set
    End Property

    Public Property ListIdPdv As List(Of Integer)
        Get
            If _listIdPdv Is Nothing Then _listIdPdv = New List(Of Integer)
            Return _listIdPdv
        End Get
        Set(value As List(Of Integer))
            _listIdPdv = value
        End Set
    End Property

    Public Property ListIdResultadoProceso As List(Of Integer)
        Get
            If _listIdResultadoProceso Is Nothing Then _listIdResultadoProceso = New List(Of Integer)
            Return _listIdResultadoProceso
        End Get
        Set(value As List(Of Integer))
            _listIdResultadoProceso = value
        End Set
    End Property

    Public Property ListIdEstadoNotus As List(Of Integer)
        Get
            If _listIdEstadoNotus Is Nothing Then _listIdEstadoNotus = New List(Of Integer)
            Return _listIdEstadoNotus
        End Get
        Set(value As List(Of Integer))
            _listIdEstadoNotus = value
        End Set
    End Property

    Public Property ListIdCiudad As List(Of Integer)
        Get
            If _listIdCiudad Is Nothing Then _listIdCiudad = New List(Of Integer)
            Return _listIdCiudad
        End Get
        Set(value As List(Of Integer))
            _listIdCiudad = value
        End Set
    End Property

    Public Property ListIdEstadoNovedad As List(Of Integer)
        Get
            If _listIdEstadoNovedad Is Nothing Then _listIdEstadoNovedad = New List(Of Integer)
            Return _listIdEstadoNovedad
        End Get
        Set(value As List(Of Integer))
            _listIdEstadoNovedad = value
        End Set
    End Property

    Public Property ListIdNovedad As List(Of Integer)
        Get
            If _listIdNovedad Is Nothing Then _listIdNovedad = New List(Of Integer)
            Return _listIdNovedad
        End Get
        Set(value As List(Of Integer))
            _listIdNovedad = value
        End Set
    End Property

    Public Property ListIdCausal As List(Of Integer)
        Get
            If _listIdCausal Is Nothing Then _listIdCausal = New List(Of Integer)
            Return _listIdCausal
        End Get
        Set(value As List(Of Integer))
            _listIdCausal = value
        End Set
    End Property

    Public Property NuevaVentaDifCampania As Boolean
        Get
            Return _nuevaVentaDifCampania
        End Get
        Set(value As Boolean)
            _nuevaVentaDifCampania = value
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
        Dim objGestionDeVenta As Type = GetType(GestionComercial.GestionDeVenta)
        Dim pInfo As PropertyInfo

        For Each pInfo In objGestionDeVenta.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GestionComercial.GestionDeVenta)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As GestionComercial.GestionDeVenta)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As GestionComercial.GestionDeVenta)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As GestionComercial.GestionDeVenta

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), GestionComercial.GestionDeVenta)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(GestionComercial.GestionDeVenta).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        If _cargado Then Me.InnerList.Clear()
        With dbManager
            If _listGestionVenta IsNot Nothing AndAlso _listGestionVenta.Count > 0 Then _
                .SqlParametros.Add("@listGestionVenta", SqlDbType.VarChar).Value = String.Join(",", _listGestionVenta.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _idEstrategiaComercial IsNot Nothing AndAlso _idEstrategiaComercial.Count > 0 Then _
                .SqlParametros.Add("@idEstrategiaComercial", SqlDbType.VarChar).Value = String.Join(",", _idEstrategiaComercial.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _idAsesor IsNot Nothing AndAlso _idAsesor.Count > 0 Then _
                .SqlParametros.Add("@idAsesor", SqlDbType.VarChar).Value = String.Join(",", _idAsesor.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _idBase IsNot Nothing AndAlso _idBase.Count > 0 Then _
                .SqlParametros.Add("@idBase", SqlDbType.VarChar).Value = String.Join(",", _idBase.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _idEstado IsNot Nothing AndAlso _idEstado.Count > 0 Then _
                .SqlParametros.Add("@idEstado", SqlDbType.VarChar).Value = String.Join(",", _idEstado.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _identificacionCliente IsNot Nothing AndAlso _identificacionCliente.Count > 0 Then _
                .SqlParametros.Add("@identificacionCliente", SqlDbType.VarChar).Value = String.Join(",", _identificacionCliente.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdCliente IsNot Nothing AndAlso _listIdCliente.Count > 0 Then _
                .SqlParametros.Add("@listIdCliente", SqlDbType.VarChar).Value = String.Join(",", _listIdCliente.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdPdv IsNot Nothing AndAlso _listIdPdv.Count > 0 Then _
                .SqlParametros.Add("@listIdPdv", SqlDbType.VarChar).Value = String.Join(",", _listIdPdv.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdResultadoProceso IsNot Nothing AndAlso _listIdResultadoProceso.Count > 0 Then _
                .SqlParametros.Add("@listIdResultadoProceso", SqlDbType.VarChar).Value = String.Join(",", _listIdResultadoProceso.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdEstadoNotus IsNot Nothing AndAlso _listIdEstadoNotus.Count > 0 Then _
                .SqlParametros.Add("@listIdEstadoNotus", SqlDbType.VarChar).Value = String.Join(",", _listIdEstadoNotus.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdCiudad IsNot Nothing AndAlso _listIdCiudad.Count > 0 Then _
                .SqlParametros.Add("@listIdCiudad", SqlDbType.VarChar).Value = String.Join(",", _listIdCiudad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdEstadoNovedad IsNot Nothing AndAlso _listIdEstadoNovedad.Count > 0 Then _
                .SqlParametros.Add("@listIdEstadoNovedad", SqlDbType.VarChar).Value = String.Join(",", _listIdEstadoNovedad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdNovedad IsNot Nothing AndAlso _listIdNovedad.Count > 0 Then _
                .SqlParametros.Add("@listIdNovedad", SqlDbType.VarChar).Value = String.Join(",", _listIdNovedad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdCausal IsNot Nothing AndAlso _listIdCausal.Count > 0 Then _
                .SqlParametros.Add("@listIdCausal", SqlDbType.VarChar).Value = String.Join(",", _listIdCausal.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _fechaInicio > Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
            If _fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
            If _fechaInicioRec > Date.MinValue Then .SqlParametros.Add("@fechaInicioRec", SqlDbType.DateTime).Value = _fechaInicioRec
            If _fechaFinalRec > Date.MinValue Then .SqlParametros.Add("@fechaFinalRec", SqlDbType.DateTime).Value = _fechaFinal
            If _idOportunidad IsNot Nothing AndAlso _idOportunidad.Count > 0 Then _
                .SqlParametros.Add("@idOportunidad", SqlDbType.VarChar).Value = String.Join(",", _idOportunidad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _idCampania IsNot Nothing Then
                .SqlParametros.Add("@idCampania", SqlDbType.VarChar).Value = _idCampania
            End If
            If _idUsuarioConsulta > 0 Then
                .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuarioConsulta
            End If

            .ejecutarReader("ObtenerInfoGestionVenta", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objGestionComercial As GestionComercial.GestionDeVenta
                While .Reader.Read
                    objGestionComercial = New GestionComercial.GestionDeVenta()
                    objGestionComercial.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objGestionComercial)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
