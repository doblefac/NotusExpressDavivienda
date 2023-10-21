Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace WMS


    Public Class DetalleSerialOrdenInventarioFisico

#Region "Atributos"

        Private _idSerial As Long
        Private _idOrden As Integer
        Private _serial As String
        Private _fechaRegistro As Date
        Private _idRegistrador As Integer
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            _serial = ""
        End Sub

#End Region

#Region "Propiedades"
        Public Property IdSerial() As Long            Get                Return _idSerial            End Get            Set(ByVal value As Long)                _idSerial = value            End Set        End Property
        Public Property IdOrden() As Integer            Get                Return _idOrden            End Get            Set(ByVal value As Integer)                _idOrden = value            End Set        End Property
        Public Property Serial() As String            Get                Return _serial            End Get            Set(ByVal value As String)                _serial = value            End Set        End Property
        Public Property FechaRegistro() As Date            Get                Return _fechaRegistro            End Get            Set(ByVal value As Date)                _fechaRegistro = value            End Set        End Property
        Public Property IdRegistrador() As Integer            Get                Return _idRegistrador            End Get            Set(ByVal value As Integer)                _idRegistrador = value            End Set        End Property
#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As LMDataAccess = Nothing
            Try
                dbManager = New LMDataAccess
                With dbManager
                    .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                    .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idRegistrador
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .ejecutarNonQuery("RegistrarSerialEnInventarioFisico", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(300, "Imposible evaluar la respuesta del servidor de bases de datos. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace
