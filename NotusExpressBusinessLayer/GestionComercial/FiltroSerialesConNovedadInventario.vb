Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class FiltroSerialesConNovedadInventario
#Region "Atributos (Campos)"

    Private _idPuntoDeVenta As Integer
    Private _serial As String
    Private _idTipoVenta As Byte
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _tipoFechas As String
    Private _datosReporte As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
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

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
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
                If _idTipoVenta > 0 Then .SqlParametros.Add("@idTipoVenta", SqlDbType.TinyInt).Value = _idTipoVenta
                If Len(_serial) > 0 Then .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                    .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                    .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    .SqlParametros.Add("@tipoFechas", SqlDbType.VarChar, 20).Value = _tipoFechas
                End If
                .TiempoEsperaComando = 600
                _datosReporte = .ejecutarDataTable("SerialesConNovedadInventarioPorFiltro", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
#End Region
End Class
