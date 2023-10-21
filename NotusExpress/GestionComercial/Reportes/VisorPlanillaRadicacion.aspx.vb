Imports System.Collections.Generic
Public Class VisorPlanillaRadicacion
    Inherits System.Web.UI.Page
#Region "Atributos"

    Private _idRadicado As Integer

#End Region

#Region "Propiedades"

    Public Property IdRadicado As Integer
        Get
            Return _idRadicado
        End Get
        Set(value As Integer)
            _idRadicado = value
        End Set
    End Property

#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.QueryString("id") IsNot Nothing AndAlso Request.QueryString("id").Length > 0 Then
                _idRadicado = CInt(Request.QueryString("id"))
                rvPicking.Report = CrearReporte(_idRadicado)
            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearReporte(ByVal id As Integer) As DevExpress.XtraReports.UI.XtraReport
        Dim rptPlanillaRadicacion As New PlanillaRadicacionReporte()
        rptPlanillaRadicacion.EstablecerParametro(id)
        Return rptPlanillaRadicacion
    End Function

#End Region
End Class