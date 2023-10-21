Imports System.Collections.Generic
Public Class VisorXtraReport
    Inherits System.Web.UI.Page
#Region "Atributos"

    Private _idActa As Integer

#End Region

#Region "Propiedades"

    Public Property IdActa As Integer
        Get
            Return _idActa
        End Get
        Set(value As Integer)
            _idActa = value
        End Set
    End Property

#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.QueryString("id") IsNot Nothing AndAlso Request.QueryString("id").Length > 0 Then
                _idActa = CInt(Request.QueryString("id"))
                rvPicking.Report = CrearReporte(_idActa)
            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearReporte(ByVal id As Integer) As DevExpress.XtraReports.UI.XtraReport
        Dim rptActaDestruccion As New ActaDestruccionSeriales()
        rptActaDestruccion.EstablecerParametro(id)
        Return rptActaDestruccion
    End Function

#End Region
End Class