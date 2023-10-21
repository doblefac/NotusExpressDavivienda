Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace GestionComercial

    Public Class CategoriaCredito

#Region "Atributos"

        Private _idCategoria As Long
        Private _categoria As String
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdCategoria As Long
            Get
                Return _idCategoria
            End Get
            Set(value As Long)
                _idCategoria = value
            End Set
        End Property

        Public Property Categoria As String
            Get
                Return _categoria
            End Get
            Set(value As String)
                _categoria = value
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

        Public Sub New(ByVal idCategoria As Long)
            MyBase.New()
            _idCategoria = idCategoria
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idCategoria > 0 Then .SqlParametros.Add("@idCategoria", SqlDbType.Int).Value = _idCategoria
                    .ejecutarReader("ObtenerInfoCategoriaCredito", CommandType.StoredProcedure)
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
                    Long.TryParse(reader("idCategoria"), _idCategoria)
                    If Not IsDBNull(reader("categoria")) Then _categoria = (reader("categoria").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace