Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class ConsultarSeriales
#Region "Atributos"

    Private _numeroSerial As String
    Private _idGestionVenta As Integer

#End Region

#Region "Propiedades"

    Public Property NumeroSerial() As String
        Get
            Return _numeroSerial
        End Get
        Set(ByVal value As String)
            _numeroSerial = value
        End Set
    End Property

    Public Property IdGestionVenta() As Integer
        Get
            Return _idGestionVenta
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idGestionVenta = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _numeroSerial = ""

    End Sub

    Public Sub New(ByVal numeroSerial As String)
        Me.New()
        _numeroSerial = numeroSerial
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If Len(_numeroSerial) > 1 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numeroSerial", SqlDbType.VarChar, 20).Value = _numeroSerial
                    .ejecutarReader("VerificarSerialDestruir", CommandType.StoredProcedure)
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
                Integer.TryParse(reader("idGestionVenta").ToString, _idGestionVenta)
            End If
        End If

    End Sub


#End Region
End Class
