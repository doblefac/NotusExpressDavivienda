Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Web
Imports System.Text
Imports System.Reflection

Module HerramientasBL

    Public Sub CargarLicenciaGembox()
        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
    End Sub


    Public Sub ForzarDescargaDeArchivo(ByVal rutaArchivo As String, ByVal nombreMostrarArchivo As String)
        Dim infoArchivo As FileInfo
        Dim contextoHttp As HttpContext = HttpContext.Current

        Try

            infoArchivo = New FileInfo(rutaArchivo)
            If infoArchivo.Exists Then
                Dim myFile As New FileStream(rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Dim binaryReader As New BinaryReader(myFile)
                Dim startBytes As Long = 0
                Dim lastUpdateTiemStamp As String = File.GetLastWriteTimeUtc(rutaArchivo).ToString("r")
                Dim encodedData As String = HttpUtility.UrlEncode(nombreMostrarArchivo, Encoding.UTF8) + lastUpdateTiemStamp

                With contextoHttp.Response
                    .Clear()
                    .Buffer = False
                    .ContentType = "application/octet-stream"
                    .AddHeader("Content-Disposition", "attachment; filename=" & nombreMostrarArchivo)
                    .AddHeader("Content-Length", (infoArchivo.Length - startBytes).ToString())
                    If infoArchivo.Length > 10485760 Then
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
                        .Flush()
                    End If
                    .End()
                End With
            End If
        Catch ex As System.Threading.ThreadAbortException
        End Try
    End Sub

    Public Function GenerarDataTableDesdeArray(lista As Array) As DataTable
        Dim dtAux As New DataTable
        Dim objType As Type = lista.GetType().GetElementType()
        Dim pInfo As PropertyInfo

        For Each pInfo In objType.GetProperties
            If pInfo.PropertyType.Namespace = "System" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            End If
        Next

        Dim drAux As DataRow
        Dim obj As Object

        For index As Integer = 0 To lista.Length - 1
            drAux = dtAux.NewRow
            obj = lista(index)
            If obj IsNot Nothing Then
                For Each pInfo In obj.GetType().GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(obj, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Function GenerarDataTableDeColeccion(lista As ArrayList) As DataTable
        Dim dtAux As New DataTable
        Dim objType As Type = lista.GetType().GetElementType()
        Dim pInfo As PropertyInfo

        For Each pInfo In objType.GetProperties
            If pInfo.PropertyType.Namespace = "System" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            End If
        Next

        Dim drAux As DataRow
        Dim obj As Object

        For index As Integer = 0 To lista.Count - 1
            drAux = dtAux.NewRow
            obj = lista(index)
            If obj IsNot Nothing Then
                For Each pInfo In obj.GetType().GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(obj, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Function EsNumerico(ByVal cadena As String) As Boolean
        Dim miRegExp As New RegularExpressions.Regex("^[0-9]+$")
        Dim resultado As Boolean = miRegExp.IsMatch(cadena)
        Return resultado
    End Function

    Public Function EsNuloOVacio(ByVal cadena As Object) As Boolean
        If cadena IsNot Nothing AndAlso cadena.ToString.Trim.Length > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

End Module

Namespace Enumerados

    Public Enum EstadoBinario
        Activo = 1
        Inactivo = 0
        NoEstablecido = -1
    End Enum

    Public Enum TipoFechaConsulta
        NoEstablecido = 0
        FechaCreacion = 1
        FechaCierre = 2
    End Enum

End Namespace

