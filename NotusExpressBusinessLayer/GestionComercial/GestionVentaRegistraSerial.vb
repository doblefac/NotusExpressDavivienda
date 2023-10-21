Imports System.Web
Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.Comunes
Imports NotusExpressBusinessLayer.General

Public Class GestionVentaRegistraSerial

#Region "Atributos"

    Private _estructuraTabla As DataTable

#End Region

#Region "Propiedades"

    Public Property EstructuraTabla() As DataTable
        Get
            Return _estructuraTabla
        End Get
        Set(ByVal value As DataTable)
            _estructuraTabla = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function CargarInformacion(ByVal idServicio As Integer) As Boolean
        Dim esValido As Boolean = True
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                    .EjecutarNonQuery("LiberarDatosTransitoriosSeriales", CommandType.StoredProcedure)

                    .InicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 600000
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaSerialGestion"
                        .ColumnMappings.Add("IdServicio", "idServicio")
                        .ColumnMappings.Add("Material", "material")
                        .ColumnMappings.Add("DescripcionMaterial", "descripcionMaterial")
                        .ColumnMappings.Add("Serial", "serial")
                        .WriteToServer(EstructuraTabla)
                    End With
                End With
            End Using
        Catch ex As Exception
            Throw ex
            esValido = False
        End Try
        Return esValido
    End Function

    Public Function RegistrarSerialesServicio(ByVal idServicio As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idServicio", SqlDbType.Int).Value = idServicio
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarSerialesServicio", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(300, "No se logró obtener la respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(400, "Se generó un error al registrar los seriales del servicio: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
