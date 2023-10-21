Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports GemBox.Spreadsheet
Imports System.Web

Public Class ReporteDePresupuestoDiarioDeGestiones

#Region "Atributos (Campos)"

    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _datosReporte As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal fechaInicial As Date, ByVal fechaFinal As Date)
        MyBase.New()
        _fechaInicial = fechaInicial
        _fechaFinal = fechaFinal
    End Sub

#End Region

#Region "Propiedades"

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

    Public ReadOnly Property DatosReporte() As DataTable
        Get
            If _datosReporte Is Nothing Then CargarDatos()
            Return _datosReporte
        End Get
    End Property

#End Region

#Region "Métodos Públicos"

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                    .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                    .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                End If
                .TiempoEsperaComando = 600
                _datosReporte = .ejecutarDataTable("ReporteDePresupuestoDiarioDeGestionesDeVenta", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
