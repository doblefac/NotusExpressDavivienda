Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace GestionComercial

    Public Class PlanoClienteFinal

#Region "Atributos"

        Private _idPlano As Long
        Private _nombreBase As String
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdPlano As Long
            Get
                Return _idPlano
            End Get
            Set(value As Long)
                _idPlano = value
            End Set
        End Property

        Public Property NombreBase As String
            Get
                Return _nombreBase
            End Get
            Set(value As String)
                _nombreBase = value
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

        Public Sub New(ByVal idPlano As Long)
            MyBase.New()
            _idPlano = idPlano
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idPlano > 0 Then .SqlParametros.Add("@idPlano", SqlDbType.Int).Value = _idPlano
                    .ejecutarReader("ObtenerInfoPlanoClienteFinal", CommandType.StoredProcedure)
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
                    Long.TryParse(reader("idPlano"), _idPlano)
                    If Not IsDBNull(reader("nombreBase")) Then _nombreBase = (reader("nombreBase").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace