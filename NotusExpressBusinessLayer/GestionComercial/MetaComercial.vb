Imports LMDataAccessLayer
Imports System.Web

Namespace ConfiguracionComercial

    Public Class MetaComercial
        Inherits MetaComercialBase

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(identificador As Integer)
            Me.New()
            MyBase._idMeta = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdPdv As Integer
            Get
                Return _idPdvOAsesor
            End Get
            Set(ByVal value As Integer)
                _idPdvOAsesor = value
            End Set
        End Property

        Public Property PuntoDeVenta As String
            Get
                Return MyBase._puntoOAsesor
            End Get
            Protected Friend Set(value As String)
                MyBase._puntoOAsesor = value
            End Set
        End Property

#End Region

#Region "Métodos Portegidos"

        Protected Overrides Sub CargarInformacion()
            If Me._idMeta > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idMeta > 0 Then .SqlParametros.Add("@idMeta", SqlDbType.Int).Value = Me._idMeta
                        .ejecutarReader("ObtenerInfoMetaComercial", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then MyBase.AsignarValorAPropiedades(.Reader)
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Publicos"

        Public Overrides Function Registrar() As General.ResultadoProceso
            Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
            Dim idUsuario As Integer = 0
            If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId").ToString, idUsuario)
            Dim dbManager As New LMDataAccess
            If Me._idEstrategia > 0 AndAlso Me._idPdvOAsesor > 0 AndAlso Me._idTipoProducto > 0 AndAlso Me._anio > 0 _
                    AndAlso Me._mes > 0 AndAlso Me._meta >= 0 Then
                Try
                    With dbManager
                        .SqlParametros.Add("@idEstrategia", SqlDbType.SmallInt).Value = Me._idEstrategia
                        .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = Me._idPdvOAsesor
                        .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = Me._idTipoProducto
                        .SqlParametros.Add("@anio", SqlDbType.SmallInt).Value = Me._anio
                        .SqlParametros.Add("@mes", SqlDbType.TinyInt).Value = Me._mes
                        .SqlParametros.Add("@meta", SqlDbType.Int).Value = Me._meta
                        .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("RegistrarMetaComercial", CommandType.StoredProcedure)
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

        Public Overrides Function Actualizar() As General.ResultadoProceso
            Dim resultado As New General.ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
            Dim idUsuario As Integer = 0
            If HttpContext.Current.Session("userId") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("userId").ToString, idUsuario)
            If Me._meta >= 0 AndAlso idUsuario > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idMeta", SqlDbType.Int).Value = Me._idMeta
                        .SqlParametros.Add("@meta", SqlDbType.Int).Value = Me._meta
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("ActualizarMetaComercial", CommandType.StoredProcedure)
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

    End Class

End Namespace