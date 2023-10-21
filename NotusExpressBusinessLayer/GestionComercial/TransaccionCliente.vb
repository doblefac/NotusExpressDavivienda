Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class TransaccionCliente


#Region "Atributos"

    Private _idGestionVenta As Integer
    Private _numeroIdentificacion As String
    
#End Region

#Region "Propiedades"

    Public Property IdGestionVenta() As Integer
        Get
            Return _idGestionVenta
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idGestionVenta = value
        End Set
    End Property

    Public Property NumeroIdentificacion() As String
        Get
            Return _numeroIdentificacion
        End Get
        Set(ByVal value As String)
            _numeroIdentificacion = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _numeroIdentificacion = ""
        
    End Sub

    Public Sub New(ByVal idGestionVenta As Integer)
        Me.New()
        _idGestionVenta = idGestionVenta
        CargarDatos()
    End Sub

    Public Sub New(ByVal numeroIdentificacion As String)
        Me.New()
        _numeroIdentificacion = numeroIdentificacion
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If _idGestionVenta > 0 OrElse Not String.IsNullOrEmpty(_numeroIdentificacion) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Value = _numeroIdentificacion
                    .ejecutarReader("ObtenerInfoClienteTransaccion", CommandType.StoredProcedure)
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
                _numeroIdentificacion = reader("numeroIdentificacion").ToString
                
            End If
        End If

    End Sub

    
#End Region


End Class
