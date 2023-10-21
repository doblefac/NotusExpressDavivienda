Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ReporteExtendidoLegalizacion

#Region "Filtros de Búsqueda"

    Private _listGestionVenta As List(Of Long)
    Private _fechaInicio As Date
    Private _fechaFinal As Date
    Private _fechaInicioRec As Date
    Private _fechaFinalRec As Date
    Private _idEstrategiaComercial As List(Of Integer)
    Private _idAsesor As List(Of Integer)
    Private _idEstado As List(Of Integer)
    Private _identificacionCliente As List(Of String)
    Private _listIdPdv As List(Of Integer)
    Private _listIdResultadoProceso As List(Of Integer)
    Private _listIdEstadoNotus As List(Of Integer)
    Private _listIdCiudad As List(Of Integer)
    Private _listIdEstadoNovedad As List(Of Integer)
    Private _listIdNovedad As List(Of Integer)
    Private _listIdCausal As List(Of Integer)

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Propiedades"

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

#End Region

#Region "Métodos Públicos"

    Public Function GenerarReporte() As DataTable
        Dim dtDatos As New DataTable
        Dim dbManager As New LMDataAccess

        With dbManager
            With .SqlParametros
                If _listGestionVenta IsNot Nothing AndAlso _listGestionVenta.Count > 0 Then _
                .Add("@listGestionVenta", SqlDbType.VarChar).Value = String.Join(",", _listGestionVenta.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _idEstrategiaComercial IsNot Nothing AndAlso _idEstrategiaComercial.Count > 0 Then _
                    .Add("@idEstrategiaComercial", SqlDbType.VarChar).Value = String.Join(",", _idEstrategiaComercial.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _idAsesor IsNot Nothing AndAlso _idAsesor.Count > 0 Then _
                    .Add("@idAsesor", SqlDbType.VarChar).Value = String.Join(",", _idAsesor.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _idEstado IsNot Nothing AndAlso _idEstado.Count > 0 Then _
                    .Add("@idEstado", SqlDbType.VarChar).Value = String.Join(",", _idEstado.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _identificacionCliente IsNot Nothing AndAlso _identificacionCliente.Count > 0 Then _
                    .Add("@identificacionCliente", SqlDbType.VarChar).Value = String.Join(",", _identificacionCliente.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdPdv IsNot Nothing AndAlso _listIdPdv.Count > 0 Then _
                    .Add("@listIdPdv", SqlDbType.VarChar).Value = String.Join(",", _listIdPdv.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdResultadoProceso IsNot Nothing AndAlso _listIdResultadoProceso.Count > 0 Then _
                    .Add("@listIdResultadoProceso", SqlDbType.VarChar).Value = String.Join(",", _listIdResultadoProceso.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdEstadoNotus IsNot Nothing AndAlso _listIdEstadoNotus.Count > 0 Then _
                    .Add("@listIdEstadoNotus", SqlDbType.VarChar).Value = String.Join(",", _listIdEstadoNotus.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdCiudad IsNot Nothing AndAlso _listIdCiudad.Count > 0 Then _
                    .Add("@listIdCiudad", SqlDbType.VarChar).Value = String.Join(",", _listIdCiudad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdEstadoNovedad IsNot Nothing AndAlso _listIdEstadoNovedad.Count > 0 Then _
                    .Add("@listIdEstadoNovedad", SqlDbType.VarChar).Value = String.Join(",", _listIdEstadoNovedad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdNovedad IsNot Nothing AndAlso _listIdNovedad.Count > 0 Then _
                    .Add("@listIdNovedad", SqlDbType.VarChar).Value = String.Join(",", _listIdNovedad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdCausal IsNot Nothing AndAlso _listIdCausal.Count > 0 Then _
                    .Add("@listIdCausal", SqlDbType.VarChar).Value = String.Join(",", _listIdCausal.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _fechaInicio > Date.MinValue Then .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                If _fechaFinal > Date.MinValue Then .Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
                If _fechaInicioRec > Date.MinValue Then .Add("@fechaInicioRec", SqlDbType.DateTime).Value = _fechaInicioRec
                If _fechaFinalRec > Date.MinValue Then .Add("@fechaFinalRec", SqlDbType.DateTime).Value = _fechaFinal
            End With
            dtDatos = .EjecutarDataTable("ReporteExtendidoLegalizaciones", CommandType.StoredProcedure)
        End With
        Return dtDatos
    End Function

#End Region

End Class
