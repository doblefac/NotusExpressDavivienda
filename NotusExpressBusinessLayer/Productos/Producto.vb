Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer

Namespace MaestroProductos

    Public Class Producto

#Region "Atributos"

        Private _idProducto As Integer
        Private _codigo As String
        Private _nombre As String
        Private _descripcion As String
        Private _idTipoProducto As Short
        Private _tipoProducto As String
        Private _comercializable As Boolean
        Private _serializado As Boolean
        Private _manejaSubproducto As Boolean
        Private _idEstado As Byte
        Private _fechaRegistro As Date
        Private _idCreador As Integer
        Private _registrado As Boolean

#End Region

#Region "Contructores"

        Public Sub New()
            _codigo = ""
            _nombre = ""
            _descripcion = ""
            _tipoProducto = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            _idProducto = identificador
            CargarInfoProducto()
        End Sub

        Public Sub New(ByVal codigo As String)
            Me.New()
            _codigo = codigo
            CargarInfoProducto()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idProducto = value
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

        Public Property NombreProducto() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
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

        Public Property TipoProducto() As String
            Get
                Return _tipoProducto
            End Get
            Protected Friend Set(ByVal value As String)
                _tipoProducto = value
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

        Public Property ManejaSubproducto() As Boolean
            Get
                Return _manejaSubproducto
            End Get
            Set(ByVal value As Boolean)
                _manejaSubproducto = value
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
            If Me._idProducto > 0 OrElse Not String.IsNullOrEmpty(Me._codigo) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = Me._idProducto
                        If Not String.IsNullOrEmpty(Me._codigo) Then _
                            .SqlParametros.Add("@codigo", SqlDbType.VarChar, 20).Value = Me._codigo
                        .ejecutarReader("ObtenerInfoProducto", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idProducto").ToString, Me._idProducto)
                    Me._codigo = reader("codigoProducto").ToString
                    Me._nombre = reader("nombreProducto").ToString
                    Me._descripcion = reader("descripcionProducto").ToString
                    Short.TryParse(reader("idTipoProducto").ToString, Me._idTipoProducto)
                    Me._tipoProducto = reader("tipoProducto").ToString
                    Me._comercializable = CBool(reader("comercializable").ToString)
                    Me._serializado = CBool(reader("serializado").ToString)
                    Me._manejaSubproducto = CBool(reader("manejaSubproducto").ToString)
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
