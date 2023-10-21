Imports LMDataAccessLayer
Public Class VentasMasivo
#Region "Atributos"

    Private _idImportacion As Integer
    Property idImportacion() As Integer
        Get
            Return _idImportacion
        End Get
        Set(ByVal Value As Integer)
            _idImportacion = Value
        End Set
    End Property

    Private _idEstrategiaComercial As Integer
    Property IdEstrategiaComercial() As Integer
        Get
            Return _idEstrategiaComercial
        End Get
        Set(ByVal Value As Integer)
            _idEstrategiaComercial = Value
        End Set
    End Property

    Private _idCreador As Integer
    Property IdCreador() As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal Value As Integer)
            _idCreador = Value
        End Set
    End Property

    Private _dtLog As DataTable
    Property DtLog() As DataTable
        Get
            Return _dtLog
        End Get
        Set(ByVal Value As DataTable)
            _dtLog = Value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"
    Public Shared Function ObtenerTablaVentaMasivoArchivo() As DataTable
        Dim dtDatos As New DataTable
        dtDatos.TableName = "tbVentaMasivoArchivo"
        dtDatos.Columns.Add("NOMBRE EMPLEADO", GetType(String))
        dtDatos.Columns.Add("IDENTIFICACIÓN", GetType(String))
        dtDatos.Columns.Add("TIPO", GetType(String))
        dtDatos.Columns.Add("CAJA DE COMPENSACIÓN", GetType(String))
        dtDatos.Columns.Add("COD CONVENIO", GetType(String))
        dtDatos.Columns.Add("EMPRESA", GetType(String))
        dtDatos.Columns.Add("NIT", GetType(String))
        dtDatos.Columns.Add("IDPRODUCTO SOLICITADO", GetType(String))
        dtDatos.Columns.Add("PRODUCTO SOLICITADO", GetType(String))
        dtDatos.Columns.Add("TIPO PRODUCTO SOLICITADO", GetType(String))
        dtDatos.Columns.Add("PREAPROBACIÓN", GetType(String))
        dtDatos.Columns.Add("CUPO PRE-APROBADO", GetType(String))
        dtDatos.Columns.Add("FECHA VISITA (DD/MM/AAA)", GetType(String))
        dtDatos.Columns.Add("DOCUMENTOS COMPLETOS", GetType(String))
        dtDatos.Columns.Add("FECHA RECOLECCIÓN", GetType(String))
        dtDatos.Columns.Add("FECHA RADICACIÓN", GetType(String))
        dtDatos.Columns.Add("NOMBRE ASESOR", GetType(String))
        dtDatos.Columns.Add("IDENTIFICACIÓN ASESOR", GetType(String))
        dtDatos.Columns.Add("IDCIUDAD", GetType(String))
        dtDatos.Columns.Add("CIUDAD", GetType(String))
        dtDatos.Columns.Add("CODIGO DE ESTRATEGIA", GetType(String))
        dtDatos.Columns.Add("CODIGO DE AGENTE VENDEDOR", GetType(String))
        dtDatos.Columns.Add("OBSERVACIONES1", GetType(String))
        dtDatos.Columns.Add("SUCURSAL", GetType(String))
        dtDatos.Columns.Add("CODIGO DE SUCURSAL", GetType(String))
        dtDatos.Columns.Add("REGIONAL", GetType(String))
        dtDatos.Columns.Add("ESTADO ASESOR", GetType(String))
        dtDatos.Columns.Add("SUPERVISOR", GetType(String))
        dtDatos.Columns.Add("PROCESO", GetType(String))
        dtDatos.Columns.Add("MES", GetType(String))
        dtDatos.Columns.Add("SEGURO", GetType(String))
        dtDatos.Columns.Add("APROBADOS", GetType(String))
        dtDatos.Columns.Add("FECHA APROBACIÓN", GetType(String))
        dtDatos.Columns.Add("ACTIVOS", GetType(String))
        dtDatos.Columns.Add("OBSERVACIONES2", GetType(String))
        dtDatos.Columns.Add("APROBADO", GetType(String))
        dtDatos.Columns.Add("NEGADO", GetType(String))
        dtDatos.Columns.Add("ACTIVO", GetType(String))
        dtDatos.Columns.Add("VR APROB", GetType(String))
        dtDatos.Columns.Add("VR NEGADO", GetType(String))
        dtDatos.Columns.Add("VR ACTIVO", GetType(String))
        Return dtDatos
    End Function
    Public Shared Function ObtenerTablaVentaMasivo() As DataTable
        Dim dtDatos As New DataTable
        dtDatos.TableName = "tbVentaMasivo"
        dtDatos.Columns.Add("linea", GetType(Integer))
        dtDatos.Columns.Add("nombreEmpleado", GetType(String))
        dtDatos.Columns.Add("identificacion", GetType(String))
        dtDatos.Columns.Add("tipo", GetType(Short))
        dtDatos.Columns.Add("cajaDeCompensacion", GetType(String))
        dtDatos.Columns.Add("codigoConvenio", GetType(String))
        dtDatos.Columns.Add("empresa", GetType(String))
        dtDatos.Columns.Add("nit", GetType(String))
        dtDatos.Columns.Add("idProducto", GetType(Integer))
        dtDatos.Columns.Add("productoSolicitado", GetType(String))
        dtDatos.Columns.Add("tipoProductoSolicitado", GetType(String))
        dtDatos.Columns.Add("preAprobacion", GetType(String))
        dtDatos.Columns.Add("cupoPreaprobado", GetType(Decimal))
        dtDatos.Columns.Add("fechaVisita", GetType(Date))
        dtDatos.Columns.Add("documentosCompletos", GetType(String))
        dtDatos.Columns.Add("fechaRecoleccion", GetType(Date))
        dtDatos.Columns.Add("fechaRadicacion", GetType(Date))
        dtDatos.Columns.Add("nombreAsesor", GetType(String))
        dtDatos.Columns.Add("identificacionAsesor", GetType(String))
        dtDatos.Columns.Add("idciudad", GetType(Integer))
        dtDatos.Columns.Add("ciudad", GetType(String))
        dtDatos.Columns.Add("codigoEstrategia", GetType(String))
        dtDatos.Columns.Add("codigoAgenteVendedor", GetType(String))
        dtDatos.Columns.Add("observaciones1", GetType(String))
        dtDatos.Columns.Add("sucursal", GetType(String))
        dtDatos.Columns.Add("codigoSucursal", GetType(String))
        dtDatos.Columns.Add("regional", GetType(String))
        dtDatos.Columns.Add("estadoAsesor", GetType(String))
        dtDatos.Columns.Add("supervisor", GetType(String))
        dtDatos.Columns.Add("proceso", GetType(String))
        dtDatos.Columns.Add("mes", GetType(String))
        dtDatos.Columns.Add("seguro", GetType(String))
        dtDatos.Columns.Add("aprobados", GetType(String))
        dtDatos.Columns.Add("fechaAprobacion", GetType(Date))
        dtDatos.Columns.Add("activos", GetType(String))
        dtDatos.Columns.Add("observaciones2", GetType(String))
        dtDatos.Columns.Add("aprobado", GetType(Short))
        dtDatos.Columns.Add("negado", GetType(Short))
        dtDatos.Columns.Add("activo", GetType(Short))
        dtDatos.Columns.Add("valorAprobado", GetType(Decimal))
        dtDatos.Columns.Add("valorNegado", GetType(Decimal))
        dtDatos.Columns.Add("valorActivo", GetType(Decimal))
        Return dtDatos
    End Function
    Public Function ObtenerTablaLog() As DataTable
        Dim dtDatos As New DataTable
        dtDatos.TableName = "tbLog"
        dtDatos.Columns.Add("linea", GetType(Integer))
        dtDatos.Columns.Add("mensaje", GetType(String))
        Return dtDatos
    End Function
    Public Sub Registrar(dtDatos As DataTable)
        Dim dbManager As New LMDataAccess
        dbManager.TiempoEsperaComando = 1600
        Dim DS As New DataSet
        Try
            With dbManager
                With .SqlParametros
                    .AddWithValue("@tbVentasMasivo", dtDatos)
                    .Add("@idCreador", SqlDbType.Decimal).Value = 1
                    .Add("@idEstrategiaComercial", SqlDbType.VarChar).Value = Me.IdEstrategiaComercial
                    .Add("@idImportacion", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                Me.DtLog = .EjecutarDataTable("registrarVentasMasivo", CommandType.StoredProcedure)

                Integer.TryParse(.SqlParametros("@idImportacion").Value.ToString, Me.idImportacion)

                If Me.DtLog.Rows.Count = 0 And Me.idImportacion > 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            End With
        Catch ex As Exception
            dbManager.AbortarTransaccion()
            Throw New Exception("Error registrando datos de archivos en Ventas : " + ex.Message)
        End Try
    End Sub
   

#End Region
End Class
