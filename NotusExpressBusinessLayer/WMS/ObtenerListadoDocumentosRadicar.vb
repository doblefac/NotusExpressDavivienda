Imports LMDataAccessLayer
Imports System.Reflection

Public Class ObtenerListadoDocumentosRadicar
#Region "Atributos"

    Private _datosRegistros As DataTable
    Private _idEstado As Integer

#End Region

#Region "Propiedades"


    Public ReadOnly Property DatosRegistros() As DataTable
        Get
            If _datosRegistros Is Nothing Then CargarDatos()
            Return _datosRegistros
        End Get
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()

    End Sub

#End Region

#Region "Métodos Públicos"

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                .TiempoEsperaComando = 600
                _datosRegistros = .ejecutarDataTable("ObtenerListadoDocumentosRadicar", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region
End Class
