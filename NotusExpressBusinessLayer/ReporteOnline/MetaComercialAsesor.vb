Imports LMDataAccessLayer
Imports System.Web

Public Class MetaComercialAsesor
#Region "Atributos"

    Private _idMeta As Integer
    Private _estrategia As String
    Private _asesor As String
    Private _producto As String
    Private _meta As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        _estrategia = ""
        _asesor = ""
        _idMeta = 0
    End Sub

    Public Sub New(ByVal identificador As Integer)
        Me.New()
        Me._idMeta = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdMeta As Integer
        Get
            Return _idMeta
        End Get
        Set(ByVal value As Integer)
            _idMeta = value
        End Set
    End Property

    Public Property Estrategia As String
        Get
            Return _estrategia
        End Get
        Set(ByVal value As String)
            _estrategia = value
        End Set
    End Property

    Public Property Asesor As String
        Get
            Return _asesor
        End Get
        Set(ByVal value As String)
            _asesor = value
        End Set
    End Property

    Public Property Producto As String
        Get
            Return _producto
        End Get
        Set(ByVal value As String)
            _producto = value
        End Set
    End Property

    Public Property Meta As Integer
        Get
            Return _meta
        End Get
        Set(ByVal value As Integer)
            _meta = value
        End Set
    End Property

    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        If Me._idMeta > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    '.SqlParametros.Add("@idMeta", SqlDbType.Int).Value = Me._idMeta
                    .ejecutarReader("ObtenerDetalleEstrategiaYPdvPorMetaAsesor", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            AsignarValorAPropiedades(.Reader)
                        End If
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region

#Region "Métodos Publicos"

    Public Function Actualizar() As General.ResultadoProceso
        Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
        Dim idUsuario As Integer = 0

        If _meta >= 0 And _meta <= 100 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idMeta", SqlDbType.Int).Value = Me._idMeta
                    .SqlParametros.Add("@meta", SqlDbType.Int).Value = Me._meta
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("ActualizarMetaComercialAsesor", CommandType.StoredProcedure)
                    If Short.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta emitida por el servidor. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(300, "No se han establecido todos los datos requeridos para realizar la actualización. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Me._estrategia = reader("estrategia").ToString
                Me._asesor = reader("asesor").ToString
                Me._idMeta = reader("idMeta").ToString
                Me._producto = reader("producto").ToString
                Me._meta = reader("meta").ToString
                Me._registrado = True
            End If
        End If
    End Sub

#End Region
End Class
