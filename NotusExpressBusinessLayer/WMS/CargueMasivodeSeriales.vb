Imports LMDataAccessLayer

Public Class CargueMasivodeSeriales

    Public Function CargarSeriales(ByVal dt_informacionSeriales As DataTable, ByVal idUsuario As Integer, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        dt_informacionSeriales.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        With dbManager
            Try

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminaCargueInventarioEnBodega", CommandType.StoredProcedure)
                .iniciarTransaccion()
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "CargueInventarioEnBodega"
                    .ColumnMappings.Add("CUPO", "cupo")
                    .ColumnMappings.Add("PRODUCTO PADRE", "productoPadre")
                    .ColumnMappings.Add("CODIGO BARRAS", "serial")
                    .ColumnMappings.Add("NOMBRE BODEGA-PDV", "nombreBodega")
                    .ColumnMappings.Add("idUsuario", "idUsuarioCargue")
                    .WriteToServer(dt_informacionSeriales)
                End With

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                End With
                dt = .ejecutarDataTable("CargueMasivoInventarioEnBodega", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)

                If resul = 0 Then
                    .abortarTransaccion()
                    resultado = 0
                    Return dt
                Else
                    resultado = 1
                    .confirmarTransaccion()
                    Return dt
                End If

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        dbManager.Dispose()

    End Function

    Public Function ProductosBodegas() As DataSet
        Dim dts As New DataSet
        Dim dbManager As New LMDataAccess
        With dbManager
            Try

                dts = .EjecutarDataSet("ConsultaProductosBodegas", CommandType.StoredProcedure)
                 Return dts
            Catch ex As Exception
               Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        dbManager.Dispose()

    End Function
End Class
