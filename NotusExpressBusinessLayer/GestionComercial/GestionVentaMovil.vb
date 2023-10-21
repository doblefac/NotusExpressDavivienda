Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class GestionVentaMovil

#Region "Atributos"

    Private _idGestionVenta As Integer
    Private _identificacionCliente As String
    Private _telefonoContacto As String
    Private _fechaGestion As Date
    Private _numeroPlanilla As Integer
    Private _idResultadoProceso As Short
    Private _idTipoVenta As Short
    Private _idSubproducto As Integer
    Private _serial As String
    Private _idUsuarioRegistra As Integer
    Private _idAsesorComercial As Integer
    Private _idPdv As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

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

    Public Property IdentificacionCliente() As String
        Get
            Return _identificacionCliente
        End Get
        Set(ByVal value As String)
            _identificacionCliente = value
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

    Public Property FechaGestion() As Date
        Get
            Return _fechaGestion
        End Get
        Set(ByVal value As Date)
            _fechaGestion = value
        End Set
    End Property

    Public Property NumeroPlanilla() As Integer
        Get
            Return _numeroPlanilla
        End Get
        Set(ByVal value As Integer)
            _numeroPlanilla = value
        End Set
    End Property

    Public Property IdResultadoProceso() As Short
        Get
            Return _idResultadoProceso
        End Get
        Set(ByVal value As Short)
            _idResultadoProceso = value
        End Set
    End Property

    Public Property IdTipoVenta() As Short
        Get
            Return _idTipoVenta
        End Get
        Set(ByVal value As Short)
            _idTipoVenta = value
        End Set
    End Property

    Public Property IdSubproducto() As Integer
        Get
            Return _idSubproducto
        End Get
        Set(ByVal value As Integer)
            _idSubproducto = value
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

    Public Property IdUsuarioRegistra() As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
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

    Public Property IdPdv() As Integer
        Get
            Return _idPdv
        End Get
        Set(ByVal value As Integer)
            _idPdv = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idUsuarioRegistra > 0 AndAlso _idAsesorComercial > 0 AndAlso Not String.IsNullOrEmpty(_identificacionCliente) _
            AndAlso _idResultadoProceso > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager

                    .SqlParametros.Add("@identificacionCliente", SqlDbType.VarChar, 20).Value = _identificacionCliente.Trim
                    If Not String.IsNullOrEmpty(_telefonoContacto) Then _
                        .SqlParametros.Add("@telefonoContacto", SqlDbType.VarChar, 20).Value = _telefonoContacto.Trim
                    If _fechaGestion > Date.MinValue Then .SqlParametros.Add("@fechaGestion", SqlDbType.DateTime).Value = _fechaGestion
                    .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                    If _numeroPlanilla > 0 Then .SqlParametros.Add("@numeroPlanilla", SqlDbType.Int).Value = _numeroPlanilla
                    .SqlParametros.Add("@idResultadoProceso", SqlDbType.SmallInt).Value = _idResultadoProceso
                    If _idTipoVenta > 0 Then .SqlParametros.Add("@idTipoVenta", SqlDbType.SmallInt).Value = _idTipoVenta
                    If _idSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = _idSubproducto
                    If Not String.IsNullOrEmpty(_serial) Then _
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial.Trim.ToUpper
                    .SqlParametros.Add("@idAsesorComercial", SqlDbType.Int).Value = _idAsesorComercial
                    If _idPdv > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv

                    .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarGestionVentaMovil", CommandType.StoredProcedure)

                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        If resultado.Valor = 0 Then
                            _idGestionVenta = CInt(.SqlParametros("@idGestionVenta").Value.ToString)
                            _registrado = True
                        End If
                        .confirmarTransaccion()
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor de BD. Por favor intente nuevamente")
                        .abortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor("100", "No se han proporcionado todos los datos requeridos para registrar la venta. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region

End Class
