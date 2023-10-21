Imports LMDataAccessLayer

Namespace ControlAcceso

    Public Class ColeccionPerfiles
        Inherits CollectionBase
        Public Shared Function ObtenerPerfilesUsuario(ByVal idUsuario As Integer) As DataTable

            Dim dbManager As New LMDataAccess
            Dim dtPerfil As New DataTable
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    dtPerfil = .ejecutarDataTable("SeleccionarPerfilDeUsuario", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw New Exception("Error al obtener los perfiles del usuario")
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtPerfil
        End Function

    End Class

End Namespace
