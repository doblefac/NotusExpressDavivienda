Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General
Imports NotusExpressBusinessLayer.WMS

Public Class PuntoDeVenta
    Inherits BodegaBase

#Region "Atributos"

    Private _idPdv As Integer
    Private _codigoPdv As String
    Private _codigoOficina As String
    Private _nombrePdv As String
    Private _idCadena As Short
    Private _cadena As String
    Private _telefonoContacto As String
    Private _codigoSucursal As String
    Private _fechaCreacion As Date
    Private _idCreador As Integer
    Private _fechaUltimaModificacion As Date
    Private _idModificador As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idPdv As Integer)
        Me.New()
        Me._idPdv = idPdv
        CargarInfoPdv()
    End Sub

    Public Sub New(ByVal codigoPdv As String)
        Me.New()
        Me._codigoPdv = codigoPdv
        CargarInfoPdv()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdPdv() As Integer
        Get
            Return _idPdv
        End Get
        Set(ByVal value As Integer)
            _idPdv = value
        End Set
    End Property

    Public Property CodigoPdv() As String
        Get
            Return _codigoPdv
        End Get
        Set(ByVal value As String)
            _codigoPdv = value
        End Set
    End Property

    Public Property CodigoOficina() As String
        Get
            Return _codigoOficina
        End Get
        Set(ByVal value As String)
            _codigoOficina = value
        End Set
    End Property

    Public Property NombrePdv() As String
        Get
            Return _nombrePdv
        End Get
        Set(ByVal value As String)
            _nombrePdv = value
        End Set
    End Property

    Public Property IdCadena() As Short
        Get
            Return _idCadena
        End Get
        Set(ByVal value As Short)
            _idCadena = value
        End Set
    End Property

    Public Property Cadena() As String
        Get
            Return _cadena
        End Get
        Protected Friend Set(ByVal value As String)
            _cadena = value
        End Set
    End Property

    Public Property TelefonoContacto() As String
        Get
            Return _telefonoContacto
        End Get
        Set(ByVal value As String)
            _telefonoContacto = value
        End Set
    End Property

    Public Property CodigoSucursal()
        Get
            Return _codigoSucursal
        End Get
        Set(ByVal value)
            _codigoSucursal = value
        End Set
    End Property

    Public Property FechaCreacion() As Date
        Get
            Return _fechaCreacion
        End Get
        Set(ByVal value As Date)
            _fechaCreacion = value
        End Set
    End Property

    Public Property IdCreador() As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal value As Integer)
            _idCreador = value
        End Set
    End Property

    Public Property FechaUltimaModificacion() As Date
        Get
            Return _fechaUltimaModificacion
        End Get
        Set(ByVal value As Date)
            _fechaUltimaModificacion = value
        End Set
    End Property

    Public Property IdModificador() As Integer
        Get
            Return _idModificador
        End Get
        Set(ByVal value As Integer)
            _idModificador = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarInfoPdv()
        If Me._idPdv > 0 OrElse Not String.IsNullOrEmpty(Me._codigoPdv) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If Me._idPdv > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = Me._idPdv
                    If Not String.IsNullOrEmpty(_codigoPdv) Then _
                        .SqlParametros.Add("@codigoPdv", SqlDbType.VarChar, 20).Value = Me._codigoPdv
                    .ejecutarReader("ObtenerInfoPuntoDeVenta", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then AsignarValorAPropiedades(.Reader)
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

    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                '***Información de Punto de Venta***'
                Integer.TryParse(reader("idPdv").ToString, Me._idPdv)
                Me._codigoPdv = reader("codigoPdv").ToString
                Me._codigoOficina = reader("codigoOficina").ToString
                Me._nombrePdv = reader("nombrePdv").ToString
                Short.TryParse(reader("idCadena").ToString, Me._idCadena)
                Me._cadena = reader("cadena").ToString
                Me._telefonoContacto = reader("telefonoContacto").ToString
                Me._codigoSucursal = reader("codigoSucursal").ToString
                Date.TryParse(reader("fechaCreacion").ToString, Me._fechaCreacion)
                Integer.TryParse(reader("idCreador").ToString, Me._idCreador)
                Date.TryParse(reader("fechaUltimaModificacion").ToString, Me._fechaUltimaModificacion)
                Integer.TryParse(reader("idModificador").ToString, Me._idCreador)
                '***********************************'
                '***Información de Bodega***'
                Integer.TryParse(reader("idBodega").ToString, Me._idBodega)
                Me._codigoInterno = reader("codigoInterno").ToString
                Me._nombreBodega = reader("nombreBodega").ToString
                Short.TryParse(reader("idTipo").ToString, Me._idTipo)
                Integer.TryParse(reader("idCiudad").ToString, Me._idCiudad)
                Me._ciudad = reader("ciudad").ToString
                Byte.TryParse(reader("idEstado").ToString, Me._idEstado)
                Short.TryParse(reader("idEmpresa").ToString, Me._idEmpresa)
                Me._empresa = reader("empresa").ToString
                Short.TryParse(reader("idUnidadNegocio").ToString, Me._idUnidadNegocio)
                Me._unidadNegocio = reader("unidadNegocio").ToString
                Me._codigoCentro = reader("codigoCentro").ToString
                Me._codigoAlmacen = reader("codigoAlmacen").ToString
                Integer.TryParse(reader("idBodegaPadre").ToString, Me._idPadre)
                '***************************'
                Me._registrado = True
            End If
        End If

    End Sub

    Protected Friend Overloads Function Registrar(ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

    Protected Friend Overloads Function Actualizar(ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

#End Region

#Region "Métodos Públicos"

    Public Overrides Function Registrar() As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resultado As ResultadoProceso
        Try
            resultado = Registrar(dbManager)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Overrides Function Actualizar() As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resultado As ResultadoProceso
        Try
            resultado = Actualizar(dbManager)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

#End Region
End Class
