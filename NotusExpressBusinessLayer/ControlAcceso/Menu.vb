Imports LMDataAccessLayer

Namespace ControlAcceso
    Public Class Menu

#Region "Campos"
        Private _idMenu As Integer
        Private _idPadre As Integer
        Private _nombre As String
        Private _urlFormulario As String
        Private _idTipoAplicativo As Byte
        Private _posicionOrdinal As Short
        Private _idEstado As Byte
        Private _nivel As Byte
        Private _registrado As Boolean
#End Region

#Region "Propiedades"

        Public Property IdMenu() As Integer
            Get
                Return _idMenu
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idMenu = value
            End Set
        End Property

        Public Property IdPadre() As Integer
            Get
                Return _idPadre
            End Get
            Set(ByVal value As Integer)
                _idPadre = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property UrlFormulario() As String
            Get
                Return _urlFormulario
            End Get
            Set(ByVal value As String)
                _urlFormulario = value
            End Set
        End Property

        Public Property IdTipoAplicativo() As Byte
            Get
                Return _idTipoAplicativo
            End Get
            Set(ByVal value As Byte)
                _idTipoAplicativo = value
            End Set
        End Property

        Public Property PosicionOrdinal() As Short
            Get
                Return _posicionOrdinal
            End Get
            Set(ByVal value As Short)
                _posicionOrdinal = value
            End Set
        End Property

        Public Property IdEstado() As Byte
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Byte)
                _idEstado = value
            End Set
        End Property

        Public Property Nivel() As Byte
            Get
                Return _nivel
            End Get
            Protected Friend Set(ByVal value As Byte)
                _nivel = value
            End Set
        End Property

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property
#End Region

#Region "Constructores"

        Public Sub New()
            _nombre = ""
            _urlFormulario = ""
        End Sub

        Public Sub New(ByVal idMenu As Integer)
            Me.New()
            If idMenu <> 0 Then
                _idMenu = idMenu
                CargarDatos()
            End If
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If _idMenu > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idMenu", SqlDbType.Int).Value = _idMenu
                        .ejecutarReader("ObtenerInfoMenus", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idMenu").ToString, _idMenu)
                    Integer.TryParse(reader("idPadre").ToString, _idPadre)
                    _nombre = reader("nombre").ToString
                    _urlFormulario = reader("urlFormulario").ToString
                    Byte.TryParse(reader("idTipoApp").ToString, _idTipoAplicativo)
                    Short.TryParse(reader("posicionOrdinal").ToString, _posicionOrdinal)
                    Byte.TryParse(reader("idEstado").ToString, _idEstado)
                    Byte.TryParse(reader("nivel").ToString, _nivel)
                    _registrado = True
                    If _idPadre = 0 Then _idPadre = -1
                End If
            End If

        End Sub

#End Region

    End Class
End Namespace

