Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace GestionComercial

    Public Class EquivalenciasDireccion

#Region "Atributos"

        Private _idEquivalencia As Long
        Private _idAbreviatura As Long
        Private _abreviatura As String
        Private _equivalencia As String
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdEquivalencia As Long
            Get
                Return _idEquivalencia
            End Get
            Set(value As Long)
                _idEquivalencia = value
            End Set
        End Property

        Public Property IdAbreviatura As Long
            Get
                Return _idAbreviatura
            End Get
            Set(value As Long)
                _idAbreviatura = value
            End Set
        End Property

        Public Property Abreviatura As String
            Get
                Return _abreviatura
            End Get
            Set(value As String)
                _abreviatura = value
            End Set
        End Property

        Public Property Equivalencia As String
            Get
                Return _equivalencia
            End Get
            Set(value As String)
                _equivalencia = value
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

        Public Sub New(ByVal idEquivalencia As Long)
            MyBase.New()
            _idEquivalencia = idEquivalencia
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idAbreviatura > 0 Then .SqlParametros.Add("@IdEquivalencia", SqlDbType.Int).Value = _idEquivalencia
                    .ejecutarReader("ObtenerInfoEquivalenciaDirecciones", CommandType.StoredProcedure)
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
                    Long.TryParse(reader("idEquivalencia"), _idEquivalencia)
                    Long.TryParse(reader("idAbreviatura"), _idAbreviatura)
                    If Not IsDBNull(reader("abreviatura")) Then _abreviatura = (reader("abreviatura").ToString)
                    If Not IsDBNull(reader("equivalencia")) Then _equivalencia = (reader("equivalencia").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace