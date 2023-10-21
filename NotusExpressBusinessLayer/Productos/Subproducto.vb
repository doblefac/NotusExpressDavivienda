Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer

Namespace MaestroProductos

    Public Class Subproducto

#Region "Atributos"

        Private _idSubproducto As Integer
        Private _idProducto As Integer
        Private _nombre As String
        Private _codigo As String
        Private _cantidadUnitaria As Integer
        Private _comercializable As Boolean
        Private _serializado As Boolean
        Private _fechaRegistro As Date
        Private _idCreador As Integer
        Private _idEstado As Byte
        Private _registrado As Boolean

#End Region

#Region "Contructores"

        Public Sub New()
            _codigo = ""
            _nombre = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            _idSubproducto = identificador
            CargarInfoProducto()
        End Sub

        Public Sub New(ByVal codigo As String)
            Me.New()
            _codigo = codigo
            CargarInfoProducto()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdSubproducto() As Integer
            Get
                Return _idSubproducto
            End Get
            Set(ByVal value As Integer)
                _idSubproducto = value
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

        Public Property NombreSubproducto() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Codigo() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
            End Set
        End Property

        Public Property CantidadUnitaria() As Integer
            Get
                Return _cantidadUnitaria
            End Get
            Set(ByVal value As Integer)
                _cantidadUnitaria = value
            End Set
        End Property

        Public Property Comercializable() As Boolean
            Get
                Return _comercializable
            End Get
            Set(ByVal value As Boolean)
                _comercializable = value
            End Set
        End Property

        Public Property Serializado() As Boolean
            Get
                Return _serializado
            End Get
            Set(ByVal value As Boolean)
                _serializado = value
            End Set
        End Property

        Public Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaRegistro = value
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

        Public Property IdEstado() As Byte
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Byte)
                _idEstado = value
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

        Private Sub CargarInfoProducto()
            If Me._idSubproducto > 0 OrElse Not String.IsNullOrEmpty(Me._codigo) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = Me._idSubproducto
                        If Not String.IsNullOrEmpty(Me._codigo) Then _
                            .SqlParametros.Add("@codigo", SqlDbType.VarChar, 20).Value = Me._codigo
                        .ejecutarReader("ObtenerInfoSubproducto", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idSubproducto").ToString, Me._idSubproducto)
                    Integer.TryParse(reader("idProductoPadre").ToString, Me._idProducto)
                    Me._codigo = reader("codigoSubproducto").ToString
                    Me._nombre = reader("nombreSubproducto").ToString
                    Integer.TryParse(reader("cantidadUnitaria").ToString, Me._cantidadUnitaria)
                    Me._comercializable = CBool(reader("comercializable").ToString)
                    Me._serializado = CBool(reader("serializado").ToString)
                    Byte.TryParse(reader("idEstado").ToString, Me._idEstado)
                    Date.TryParse(reader("fechaRegistro").ToString, Me._fechaRegistro)
                    Integer.TryParse(reader("idCreador").ToString, Me._idCreador)
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
