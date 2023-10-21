Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace GestionComercial

    Public Class Empresas

#Region "Atributos"

        Private _idEmpresa As Long
        Private _nombre As String
        Private _codigo As String
        Private _idTipoEmpresa As Long
        Private _idEstado As Integer
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdEmpresa As Long
            Get
                Return _idEmpresa
            End Get
            Set(value As Long)
                _idEmpresa = value
            End Set
        End Property

        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set
        End Property

        Public Property Codigo As String
            Get
                Return _codigo
            End Get
            Set(value As String)
                _codigo = value
            End Set
        End Property

        Public Property IdTipoEmpresa As Long
            Get
                Return _idTipoEmpresa
            End Get
            Set(value As Long)
                _idTipoEmpresa = value
            End Set
        End Property

        Public Property IdEstado As Integer
            Get
                Return _idEstado
            End Get
            Set(value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idEmpresa As Long)
            MyBase.New()
            _idEmpresa = idEmpresa
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idEmpresa > 0 Then .SqlParametros.Add("@IdEmpresa", SqlDbType.Int).Value = _idEmpresa
                    .ejecutarReader("ObtenerInfoEmpresa", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub

#End Region

#Region "Métodos Públicos"

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Long.TryParse(reader("idEmpresa"), _idEmpresa)
                    _nombre = reader("nombre")
                    _codigo = reader("codigo")
                    Long.TryParse(reader("idTipoEmpresa"), _idTipoEmpresa)
                    Integer.TryParse(reader("idEstado"), _idEstado)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace