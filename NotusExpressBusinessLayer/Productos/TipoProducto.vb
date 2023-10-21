Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer

Namespace MaestroProductos

    Public Class TipoProducto

#Region "Atributos"

        Private _idTipoProducto As Short
        Private _nombre As String
        Private _idUnidadNegocio As Short
        Private _idEstado As Byte
        Private _codigo As String
        Private _registrado As Boolean

#End Region

#Region "Contructores"

        Public Sub New()
            _codigo = ""
            _nombre = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            _idTipoProducto = identificador
            CargarInformacion()
        End Sub

        Public Sub New(ByVal codigo As String)
            Me.New()
            _codigo = codigo
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdTipoProducto As Short            Get                Return _idTipoProducto            End Get            Set(ByVal value As Short)                _idTipoProducto = value            End Set        End Property
        Public Property Nombre As String            Get                Return _nombre            End Get            Set(ByVal value As String)                _nombre = value            End Set        End Property
        Public Property IdUnidadNegocio As Short            Get                Return _idUnidadNegocio            End Get            Set(ByVal value As Short)                _idUnidadNegocio = value            End Set        End Property
        Public Property IdEstado As Byte            Get                Return _idEstado            End Get            Set(ByVal value As Byte)                _idEstado = value            End Set        End Property
        Public Property Codigo As String            Get                Return _codigo            End Get            Set(ByVal value As String)                _codigo = value            End Set        End Property
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

        Private Sub CargarInformacion()
            If Me._idTipoProducto > 0 OrElse Not String.IsNullOrEmpty(Me._codigo) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = Me._idTipoProducto
                        If Not String.IsNullOrEmpty(Me._codigo) Then _
                            .SqlParametros.Add("@codigo", SqlDbType.VarChar, 20).Value = Me._codigo
                        .ejecutarReader("ObtenerInfoTipoProducto", CommandType.StoredProcedure)
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
                    Short.TryParse(reader("idTipoProducto").ToString, Me._idTipoProducto)
                    Me._codigo = reader("codigo").ToString
                    Me._nombre = reader("nombre").ToString
                    Short.TryParse(reader("idEstado").ToString, Me._idEstado)
                    Short.TryParse(reader("idUnidadNegocio").ToString, Me._idUnidadNegocio)
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
