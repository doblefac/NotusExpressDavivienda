Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ObtenerIdCliente

#Region "Atributos"

    Private _numeroIdentificacion As String
    Private _idCliente As Integer

#End Region

#Region "Propiedades"

    Public Property NumeroIdentificacion() As String
        Get
            Return _numeroIdentificacion
        End Get
        Set(ByVal value As String)
            _numeroIdentificacion = value
        End Set
    End Property

    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idCliente = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _numeroIdentificacion = ""

    End Sub

    Public Sub New(ByVal numeroIdentificacion As Integer)
        Me.New()
        _numeroIdentificacion = numeroIdentificacion
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If Len(_numeroIdentificacion) > 1 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = _numeroIdentificacion
                    .ejecutarReader("ObtenerIdCliente", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
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

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idCliente").ToString, _idCliente)
            End If
        End If

    End Sub


#End Region

End Class
