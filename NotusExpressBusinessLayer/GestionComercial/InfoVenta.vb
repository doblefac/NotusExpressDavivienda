Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class InfoVenta

#Region "Atributos"

    Private _idGestionVenta As Integer
    Private _fechaGestion As Date
    Private _operadorCall As String = ""
    Private _identificacionOperadorCall As String = ""
    Private _numPlanillaPreAnalisis As String = ""
    Private _numVentaPlanilla As String = ""
    Private _idResultadoProceso As Integer = 0
    Private _idTipoVenta As Integer = 0
    Private _idProducto As Integer = 0
    Private _idSubProducto As Integer = 0
    Private _numPagare As String = ""
    Private _serial As String = ""
    Private _observacionCallCenter As String = ""
    Private _esNovedad As Boolean
    Private _idUsuarioAsesor As Integer
    Private _idPdv As Integer
    Private _usuarioasesor As Integer
    
#End Region

#Region "Propiedades"

    Public Property IdGestionVenta() As Integer
        Get
            Return _idGestionVenta
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idGestionVenta = value
        End Set
    End Property

    Public Property FechaGestion() As Date
        Get
            Return _fechaGestion
        End Get
        Protected Friend Set(ByVal value As Date)
            _fechaGestion = value
        End Set
    End Property

    Public Property OperadorCall() As String
        Get
            Return _operadorCall
        End Get
        Protected Friend Set(ByVal value As String)
            _operadorCall = value
        End Set
    End Property

    Public Property IdentificacionOperadorCall() As String
        Get
            Return _identificacionOperadorCall
        End Get
        Protected Friend Set(ByVal value As String)
            _identificacionOperadorCall = value
        End Set
    End Property

    Public Property NumPlanillaPreAnalisis() As String
        Get
            Return _numPlanillaPreAnalisis
        End Get
        Protected Friend Set(ByVal value As String)
            _numPlanillaPreAnalisis = value
        End Set
    End Property

    Public Property NumVentaPlanilla() As String
        Get
            Return _numVentaPlanilla
        End Get
        Protected Friend Set(ByVal value As String)
            _numVentaPlanilla = value
        End Set
    End Property

    Public Property IdResultadoProceso() As Integer
        Get
            Return _idResultadoProceso
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idResultadoProceso = value
        End Set
    End Property

    Public Property IdTipoVenta() As Integer
        Get
            Return _idTipoVenta
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idTipoVenta = value
        End Set
    End Property

    Public Property IdProducto() As Integer
        Get
            Return _idProducto
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property IdSubProducto() As Integer
        Get
            Return _idSubProducto
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idSubProducto = value
        End Set
    End Property

    Public Property NumPagare() As String
        Get
            Return _numPagare
        End Get
        Protected Friend Set(ByVal value As String)
            _numPagare = value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Protected Friend Set(ByVal value As String)
            _serial = value
        End Set
    End Property

    Public Property ObservacionCallCenter() As String
        Get
            Return _observacionCallCenter
        End Get
        Protected Friend Set(ByVal value As String)
            _observacionCallCenter = value
        End Set
    End Property

    Public Property EsNovedad() As Boolean
        Get
            Return _esNovedad
        End Get
        Protected Friend Set(ByVal value As Boolean)
            _esNovedad = value
        End Set
    End Property

    Public Property IdUsuarioAsesor() As Integer
        Get
            Return _idUsuarioAsesor
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idUsuarioAsesor = value
        End Set
    End Property

    Public Property IdPdv() As Integer
        Get
            Return _idPdv
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idPdv = value
        End Set
    End Property

    Public Property UsuarioAsesor() As Integer
        Get
            Return _usuarioasesor
        End Get
        Protected Friend Set(ByVal value As Integer)
            _usuarioasesor = value
        End Set
    End Property
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _idGestionVenta = 0

    End Sub

    Public Sub New(ByVal idGestionVenta As Integer)
        Me.New()
        _idGestionVenta = idGestionVenta
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If _idGestionVenta > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                    .ejecutarReader("ObtenerInfoVenta", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                _fechaGestion = reader("fechaGestion").ToString
                _operadorCall = reader("operadorCall").ToString
                _identificacionOperadorCall = reader("identificacionOperadorCall").ToString
                _numPlanillaPreAnalisis = reader("numPlanillaPreAnalisis").ToString
                _numVentaPlanilla = reader("numVentaPlanilla").ToString
                _idResultadoProceso = reader("idResultadoProceso").ToString
                _idTipoVenta = reader("idTipoVenta").ToString
                _idProducto = reader("idProducto").ToString
                _idSubProducto = reader("idSubProducto").ToString
                _numPagare = reader("numPagare").ToString
                _serial = reader("serial").ToString
                _observacionCallCenter = reader("observacionCallCenter").ToString
                If reader("esNovedad").ToString = "" Then
                    _esNovedad = False
                Else
                    _esNovedad = reader("esNovedad").ToString
                End If
                _idUsuarioAsesor = reader("idUsuarioAsesor").ToString
                _idPdv = reader("idPdv").ToString
                _usuarioasesor = reader("usuarioAsesor").ToString
                
            End If
        End If

    End Sub


#End Region

End Class
