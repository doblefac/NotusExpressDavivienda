Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace PresupuestoGestiones

    Public Class PresupuestoDeGestionesDeVenta

#Region "Atributos"

        Private _idPresupuesto As Integer
        Private _idCorte As Byte
        Private _idPdv As Integer
        Private _idUsuarioSupervisor As Integer
        Private _idUsuarioAsesor As Integer
        Private _fechaRegistro As Date
        Private _idUsuarioRegistra As Integer
        Private _idNovedad As Short
        Private _observaciones As String
        Private _detalle As DetallePresupuestoDeGestionesDeVentaColeccion
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _observaciones = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            _idPresupuesto = identificador
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdPresupuesto As Integer            Get                Return _idPresupuesto            End Get            Protected Friend Set(ByVal value As Integer)                _idPresupuesto = value            End Set        End Property
        Public Property IdCorte As Byte            Get                Return _idCorte            End Get            Set(ByVal value As Byte)                _idCorte = value            End Set        End Property
        Public Property IdPdv As Integer            Get                Return _idPdv            End Get            Set(ByVal value As Integer)                _idPdv = value            End Set        End Property
        Public Property IdUsuarioSupervisor As Integer            Get                Return _idUsuarioSupervisor            End Get            Set(ByVal value As Integer)                _idUsuarioSupervisor = value            End Set        End Property
        Public Property IdUsuarioAsesor As Integer            Get                Return _idUsuarioAsesor            End Get            Set(ByVal value As Integer)                _idUsuarioAsesor = value            End Set        End Property
        Public Property FechaRegistro As Date            Get                Return _fechaRegistro            End Get            Set(ByVal value As Date)                _fechaRegistro = value            End Set        End Property
        Public Property IdUsuarioRegistra As Integer            Get                Return _idUsuarioRegistra            End Get            Set(ByVal value As Integer)                _idUsuarioRegistra = value            End Set        End Property
        Public Property IdNovedad As Short            Get                Return _idNovedad            End Get            Set(ByVal value As Short)                _idNovedad = value            End Set        End Property
        Public Property Observaciones As String            Get                Return _observaciones            End Get            Set(ByVal value As String)                _observaciones = value            End Set        End Property

        Public ReadOnly Property Detalle As DetallePresupuestoDeGestionesDeVentaColeccion
            Get
                If _detalle Is Nothing Then
                    If _idPresupuesto <> 0 Then
                        _detalle = New DetallePresupuestoDeGestionesDeVentaColeccion(_idPresupuesto)
                    Else
                        _detalle = New DetallePresupuestoDeGestionesDeVentaColeccion()
                    End If
                End If
                Return _detalle

            End Get
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If _idPresupuesto > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idPresupuesto > 0 Then .SqlParametros.Add("@idPresupuesto", SqlDbType.Int).Value = _idPresupuesto
                        .ejecutarReader("ObtenerPresupuestoDeGestionesDeVenta", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idPresupuesto").ToString, _idPresupuesto)
                    Byte.TryParse(reader("idCorte").ToString, _idCorte)
                    Integer.TryParse(reader("idPdv").ToString, _idPdv)
                    Integer.TryParse(reader("idUsuarioSupervisor").ToString, _idUsuarioSupervisor)
                    Integer.TryParse(reader("idUsuarioAsesor").ToString, _idUsuarioAsesor)
                    Date.TryParse(reader("fechaRegistro").ToString, _fechaRegistro)
                    Integer.TryParse(reader("idUsuarioRegistra").ToString, _idUsuarioRegistra)
                    Short.TryParse(reader("idNovedad").ToString, _idNovedad)
                    _observaciones = reader("observaciones").ToString
                    _registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            If _idCorte > 0 AndAlso _idPdv > 0 AndAlso _idUsuarioAsesor > 0 AndAlso _idUsuarioSupervisor > 0 AndAlso _idUsuarioRegistra > 0 Then
                If _detalle IsNot Nothing AndAlso _detalle.Count > 0 Then
                    Dim dbManager As New LMDataAccess
                    Try
                        Dim dtAux As DataTable = _detalle.GenerarDataTable
                        Dim gid As Guid = Guid.NewGuid
                        Dim dcAux As New DataColumn("Identificador", GetType(Guid))
                        dcAux.DefaultValue = gid
                        dtAux.Columns.Add(dcAux)

                        With dbManager
                            .iniciarTransaccion()
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "TransitoriaDetallePresupuestoDeGestionesDeVenta"
                                .ColumnMappings.Add("Identificador", "identificador")
                                .ColumnMappings.Add("IdResultadoProceso", "idResultadoProceso")
                                .ColumnMappings.Add("IdTipoVenta", "idTipoVenta")
                                .ColumnMappings.Add("Cantidad", "cantidad")
                                .ColumnMappings.Add("IdClasificacionGestion", "idClasificacionGestion")
                                .WriteToServer(dtAux)
                            End With

                            .SqlParametros.Clear()
                            .SqlParametros.Add("@guidDetalle", SqlDbType.UniqueIdentifier).Value = gid
                            .SqlParametros.Add("@idCorte", SqlDbType.TinyInt).Value = _idCorte
                            .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = _idPdv
                            .SqlParametros.Add("@idUsuarioSupervisor", SqlDbType.Int).Value = _idUsuarioSupervisor
                            .SqlParametros.Add("@idUsuarioAsesor", SqlDbType.Int).Value = _idUsuarioAsesor
                            .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                            .SqlParametros.Add("@fechaPresupuesto", SqlDbType.SmallDateTime).Value = _fechaRegistro
                            If _idNovedad <> 0 Then .SqlParametros.Add("@idNovedad", SqlDbType.TinyInt).Value = _idNovedad
                            If Not String.IsNullOrEmpty(_observaciones) Then .SqlParametros.Add("@observaciones", SqlDbType.VarChar, 400).Value = _observaciones
                            .SqlParametros.Add("@idPresupuesto", SqlDbType.Int).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("RegistrarInfoPresupuestoGestionVenta", CommandType.StoredProcedure)

                            If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                If resultado.Valor = 0 Then
                                    _idPresupuesto = CInt(.SqlParametros("@idPresupuesto").Value.ToString)
                                    _registrado = True
                                    .confirmarTransaccion()
                                Else
                                    .abortarTransaccion()
                                End If
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                            Else
                                Throw New Exception("Imposible evaluar la respuesta del servidor durante el registro de datos. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    Finally
                        If dbManager IsNot Nothing Then dbManager.Dispose()
                    End Try
                Else
                    resultado.EstablecerMensajeYValor(301, "No se ha proporcionado el detalle de gestiones para realizar el registro.")
                End If
            Else
                resultado.EstablecerMensajeYValor(300, "No se han proporcionado todos los datos requeridos para realizar el registro.")
            End If

            Return resultado
        End Function

#End Region

    End Class

End Namespace