Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports GemBox.Spreadsheet
Imports System.Web

Public Class ReporteCumplimientoMetasPorAsesor
#Region "Atributos (Campos)"

    Private _fecha As Date
    Private _datosReporte As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal fechaInicial As Date, ByVal fechaFinal As Date)
        MyBase.New()
        _fecha = fechaFinal
    End Sub

#End Region

#Region "Propiedades"

    Public Property Fecha() As Date
        Get
            Return _fecha
        End Get
        Set(ByVal value As Date)
            _fecha = value
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
                .SqlParametros.Add("@fechaConsulta", SqlDbType.SmallDateTime).Value = _fecha
                .TiempoEsperaComando = 600
                _datosReporte = .ejecutarDataTable("ReporteCumplimientoMetasComercialesPorAsesor", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
