Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer

Namespace MaestroProductos

    Public Class Documento

#Region "Atributos"

        Private _idDocumento As Integer
        Private _documento As String
        Private _idTipoProducto As Short
        Private _idEstado As Byte
        Private _idRegistro As Integer
        Private _estado As String
        Private _registrado As Boolean

#End Region

#Region "Contructores"

        Public Sub New()
            _idDocumento = 0
            _documento = ""
            _idTipoProducto = 0
            _idRegistro = 0
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            _idDocumento = identificador
            CargarInfoDocumento()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdDocumento() As Integer
            Get
                Return _idDocumento
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idDocumento = value
            End Set
        End Property

        Public Property Documento() As String
            Get
                Return _documento
            End Get
            Set(ByVal value As String)
                _documento = value
            End Set
        End Property

        Public Property IdTipoProducto() As Short
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Short)
                _idTipoProducto = value
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

        Public Property IdRegistro() As Integer
            Get
                Return _idRegistro
            End Get
            Set(value As Integer)
                _idRegistro = value
            End Set
        End Property

        Public Property Estado() As String
            Get
                Return _estado
            End Get
            Set(value As String)
                _estado = value
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

#Region "Métodos Privados"

        Private Sub CargarInfoDocumento()
            If Me._idDocumento > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idDocumento > 0 Then .SqlParametros.Add("@idDocumento", SqlDbType.Int).Value = Me._idDocumento
                        .ejecutarReader("ObtenerInfoDocumento", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idDocumento").ToString, Me._idDocumento)
                    Me._documento = reader("documento").ToString
                    Short.TryParse(reader("idTipoProducto").ToString, Me._idTipoProducto)
                    Byte.TryParse(reader("idEstado").ToString, Me._idEstado)
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function obtenerDocumentosProductos() As DataTable
            Dim dt As DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idRegistro > 0 Then .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = Me._idRegistro
                    If _estado IsNot Nothing Then .SqlParametros.Add("@estado", SqlDbType.VarChar).Value = Me._estado
                    dt = .ejecutarDataTable("ObtenerInfoDocumentoProducto", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Function obtenerDocumentosProductosGestionVenta() As DataTable
            Dim dt As DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = Me._idRegistro
                    dt = .ejecutarDataTable("ObtenerInfoDocumentoProductoGestionVenta", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

#End Region

    End Class

End Namespace
