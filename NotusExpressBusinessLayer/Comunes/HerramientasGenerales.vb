Imports LMDataAccessLayer
Imports System.Web
Imports System.IO
Imports System.Text

Namespace Comunes

    Public Module HerramientasGenerales

#Region "Métodos Generales"

        Public Function ConsultarTipoIdentificacion() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    dtDatos = .EjecutarDataTable("ObtenerTipoIdentificacion", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarEstadosAnimo() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    dtDatos = .EjecutarDataTable("ObtenerEstadoAnimo", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarActividadLaboral() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    dtDatos = .EjecutarDataTable("ConsultarActividadLaboralCliente", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function


        Public Function ObtenerInfoPermisosOpcionesRestringidas() As DataTable
            Dim dtPermisos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                Dim nombreFormulario As String = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Path)

                If HttpContext.Current.Session("dtInfoPermisosOpcRestringidas") Is Nothing OrElse _
                    CType(HttpContext.Current.Session("dtInfoPermisosOpcRestringidas"), DataTable).Select("nombreFormulario='" & nombreFormulario & "'").Length = 0 Then
                    With dbManager
                        .SqlParametros.Add("@nombreFormulario", SqlDbType.VarChar, 100).Value = nombreFormulario
                        dtPermisos = .EjecutarDataTable("ObtenerInfoPermisosOpcionesRestringidas", CommandType.StoredProcedure)
                        HttpContext.Current.Session("dtInfoPermisosOpcRestringidas") = dtPermisos
                    End With
                Else
                    dtPermisos = HttpContext.Current.Session("dtInfoPermisosOpcRestringidas")
                End If
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtPermisos
        End Function


        Public Sub ForzarDescargaDeArchivo(ByVal contextoHttp As HttpContext, ByVal rutaArchivo As String)
            Dim infoArchivo As FileInfo
            infoArchivo = New FileInfo(rutaArchivo)
            If infoArchivo.Exists Then
                Dim nombreArchivo As String = Path.GetFileName(rutaArchivo)
                ForzarDescargaDeArchivo(contextoHttp, rutaArchivo, nombreArchivo)
            End If
        End Sub

        Public Sub ForzarDescargaDeArchivo(ByVal contextoHttp As HttpContext, ByVal rutaArchivo As String, _
                                      ByVal nombreMostrarArchivo As String)
            Dim infoArchivo As FileInfo
            Try

                infoArchivo = New FileInfo(rutaArchivo)
                If infoArchivo.Exists Then
                    Dim startBytes As Long = 0
                    With contextoHttp.Response
                        .Clear()
                        .Buffer = False
                        .ContentType = "application/octet-stream"
                        .AddHeader("Content-Disposition", "attachment; filename=" & nombreMostrarArchivo)
                        .AddHeader("Content-Length", (infoArchivo.Length - startBytes).ToString())
                        If infoArchivo.Length > 104857 Then
                            Dim myFile As New FileStream(rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                            Dim binaryReader As New BinaryReader(myFile)
                            Dim lastUpdateTiemStamp As String = File.GetLastWriteTimeUtc(rutaArchivo).ToString("r")
                            Dim encodedData As String = HttpUtility.UrlEncode(nombreMostrarArchivo, Encoding.UTF8) + lastUpdateTiemStamp
                            .AddHeader("Accept-Ranges", "bytes")
                            .AppendHeader("Last-Modified", lastUpdateTiemStamp)
                            .AppendHeader("ETag", Chr(34) & encodedData & Chr(34))
                            .AddHeader("Connection", "Keep-Alive")
                            '.ContentEncoding = Encoding.UTF8
                            binaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin)
                            Dim maxCount As Integer = CInt(Math.Ceiling((infoArchivo.Length - startBytes + 0.0) / 1024))
                            Dim index As Integer
                            While index < maxCount And .IsClientConnected
                                .BinaryWrite(binaryReader.ReadBytes(1024))
                                .Flush()
                                index += 1
                            End While
                        Else
                            .WriteFile(rutaArchivo)
                            If .IsClientConnected Then .Flush()
                        End If
                        If .IsClientConnected Then .End()
                    End With
                End If
            Catch ex As System.Threading.ThreadAbortException
            End Try
        End Sub
#End Region

    End Module

End Namespace


