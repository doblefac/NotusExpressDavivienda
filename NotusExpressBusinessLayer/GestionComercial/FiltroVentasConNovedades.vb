Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class FiltroVentasConNovedades

#Region "Atributos (Campos)"

    Private _idPuntoDeVenta As Integer
    Private _idAsesorComercial As Integer
    Private _numIdCliente As String
    Private _idResultadoProceso As Byte
    Private _idTipoVenta As Byte
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _tipoFechas As String
    Private _datosReporte As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _numIdCliente = ""
        _tipoFechas = "fv"
    End Sub

    Public Sub New(ByVal fechaInicial As Date, ByVal fechaFinal As Date)
        Me.New()
        _fechaInicial = fechaInicial
        _fechaFinal = fechaFinal
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdPuntoDeVenta() As Integer
        Get
            Return _idPuntoDeVenta
        End Get
        Set(ByVal value As Integer)
            _idPuntoDeVenta = value
        End Set
    End Property

    Public Property IdAsesorComercial() As Integer
        Get
            Return _idAsesorComercial
        End Get
        Set(ByVal value As Integer)
            _idAsesorComercial = value
        End Set
    End Property

    Public Property NumIDCliente As String
        Get
            Return _numIdCliente
        End Get
        Set(value As String)
            _numIdCliente = value
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

    Public Property IdTipoVenta As Byte
        Get
            Return _idTipoVenta
        End Get
        Set(value As Byte)
            _idTipoVenta = value
        End Set
    End Property

    Public Property FechaInicial() As Date
        Get
            Return _fechaInicial
        End Get
        Set(ByVal value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal() As Date
        Get
            Return _fechaFinal
        End Get
        Set(ByVal value As Date)
            _fechaFinal = value
        End Set
    End Property

    Public Property TipoFechas As String
        Get
            Return _tipoFechas
        End Get
        Set(value As String)
            _tipoFechas = value
        End Set
    End Property

    Public ReadOnly Property DatosReporte() As DataTable
        Get
            If _datosReporte Is Nothing Then CargarDatos()
            Return _datosReporte
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idPuntoDeVenta > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPuntoDeVenta
                If _idAsesorComercial > 0 Then .SqlParametros.Add("@idAsesor", SqlDbType.Int).Value = _idAsesorComercial
                If Not String.IsNullOrEmpty(_numIdCliente) Then .SqlParametros.Add("@numIdCliente", SqlDbType.VarChar, 30).Value = _numIdCliente.Trim
                If _idResultadoProceso > 0 Then .SqlParametros.Add("@idResultadoProceso", SqlDbType.TinyInt).Value = _idResultadoProceso
                If _idTipoVenta > 0 Then .SqlParametros.Add("@idTipoVenta", SqlDbType.TinyInt).Value = _idTipoVenta
                If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                    .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                    .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    .SqlParametros.Add("@tipoFechas", SqlDbType.VarChar, 20).Value = _tipoFechas
                End If
                .TiempoEsperaComando = 600
                _datosReporte = .ejecutarDataTable("VentasConNovedadesPorFiltro", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
#End Region
End Class
