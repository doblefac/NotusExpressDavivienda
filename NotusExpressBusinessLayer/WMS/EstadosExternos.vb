Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class EstadosExternos

#Region "Atributos"

    Private _idEstado As Integer
    Private _nombreEstado As String
    Private _idSistemaExterno As Integer
    Private _sistemaExterno As String

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdEstado As Integer
        Get
            Return _idEstado
        End Get
        Set(value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property NombreEstado As String
        Get
            Return _nombreEstado
        End Get
        Set(value As String)
            _nombreEstado = value
        End Set
    End Property

    Public Property IdSistemaExterno As Integer
        Get
            Return _idSistemaExterno
        End Get
        Set(value As Integer)
            _idSistemaExterno = value
        End Set
    End Property

    Public Property SistemaExterno As String
        Get
            Return _sistemaExterno
        End Get
        Set(value As String)
            _sistemaExterno = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idEstado As Integer)
        MyBase.New()
        _idEstado = idEstado
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idEstado > 0 Then .SqlParametros.Add("@listIdEstado", SqlDbType.VarChar, 2000).Value = CStr(_idEstado)

                .ejecutarReader("ObtenerEstadosExternos", CommandType.StoredProcedure)
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

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Long.TryParse(reader("idEstado"), _idEstado)
                If Not IsDBNull(reader("nombreEstado")) Then _nombreEstado = (reader("nombreEstado").ToString)
                Long.TryParse(reader("idSistemaExterno"), _idSistemaExterno)
                If Not IsDBNull(reader("sistemaExterno")) Then _sistemaExterno = (reader("sistemaExterno").ToString)
            End If
        End If
    End Sub

#End Region

End Class
