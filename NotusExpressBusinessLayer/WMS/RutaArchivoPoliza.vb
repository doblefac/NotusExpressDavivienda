Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class RutaArchivoPoliza
#Region "Atributos"

    Private _rutaArchivo As String
    Private _rutaArchivoNovedades As String
    Private _rutaArchivoDocumentos As String
    Private _rutaArchivoPlanilla As String

#End Region

#Region "Propiedades"

    Public Property RutaArchivo() As String
        Get
            Return _rutaArchivo
        End Get
        Set(ByVal value As String)
            _rutaArchivo = value
        End Set
    End Property

    Public Property RutaArchivoNovedades() As String
        Get
            Return _rutaArchivoNovedades
        End Get
        Set(ByVal value As String)
            _rutaArchivoNovedades = value
        End Set
    End Property

    Public Property RutaArchivoDocumentos() As String
        Get
            Return _rutaArchivoDocumentos
        End Get
        Set(ByVal value As String)
            _rutaArchivoDocumentos = value
        End Set
    End Property

    Public Property RutaArchivoPlanilla() As String
        Get
            Return _rutaArchivoDocumentos
        End Get
        Set(ByVal value As String)
            _rutaArchivoDocumentos = value
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
                .ejecutarReader("ObtenerInfoRutaArchivo", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                _rutaArchivo = reader("rutaArchivoPoliza").ToString
                _rutaArchivoNovedades = reader("rutaArchivoNovedades").ToString
                _rutaArchivoDocumentos = reader("rutaArchivoDocumentos").ToString
                _rutaArchivoPlanilla = reader("rutaArchivoPlanilla").ToString
            End If
        End If

    End Sub


#End Region
End Class
