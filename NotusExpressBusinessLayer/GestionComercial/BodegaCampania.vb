Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace GestionComercial

    Public Class BodegaCampania

#Region "Atributos"

        Private _idCampania As Long
        Private _idBodega As Long
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdCampania As Long
            Get
                Return _idCampania
            End Get
            Set(value As Long)
                _idCampania = value
            End Set
        End Property

        Public Property IdBodega As Long
            Get
                Return _idBodega
            End Get
            Set(value As Long)
                _idBodega = value
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

        Public Sub New(ByVal idCampania As Long)
            MyBase.New()
            _idCampania = IdCampania
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idCampania > 0 Then .SqlParametros.Add("@IdCampania", SqlDbType.Int).Value = _idCampania
                    .ejecutarReader("ObtenerInfoBodegaCampania", CommandType.StoredProcedure)
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
                    Long.TryParse(reader("idCampania"), _idCampania)
                    Long.TryParse(reader("idBodega"), _idBodega)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace