Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace GestionComercial

    Public Class AbreviaturaDireccion

#Region "Atributos"

        Private _idAbreviatura As Long
        Private _abreviatura As String
        Private _nombre As String
        Private _combinacion As String
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

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

        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set
        End Property

        Public Property Combinacion As String
            Get
                Return _combinacion
            End Get
            Set(value As String)
                _combinacion = value
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

        Public Sub New(ByVal idAbreviatura As Long)
            MyBase.New()
            _idAbreviatura = idAbreviatura
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idAbreviatura > 0 Then .SqlParametros.Add("@IdAbreviatura", SqlDbType.Int).Value = _idAbreviatura
                    .ejecutarReader("ObtenerInfoAbreviaturasDirecciones", CommandType.StoredProcedure)
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
                    Long.TryParse(reader("idAbreviatura"), _idAbreviatura)
                    If Not IsDBNull(reader("abreviatura")) Then _abreviatura = (reader("abreviatura").ToString)
                    If Not IsDBNull(reader("nombre")) Then _nombre = (reader("nombre").ToString)
                    If Not IsDBNull(reader("combinacion")) Then _combinacion = (reader("combinacion").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace