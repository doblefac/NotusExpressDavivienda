Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados
Imports NotusExpressBusinessLayer.General
Imports System.Web

Namespace ConfiguracionComercial

    Public Class IndicadorCumplimientoMetaColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _valor As Decimal
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _valor = -1
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As IndicadorCumplimientoMeta
            Get
                Dim obj As IndicadorCumplimientoMeta = Nothing
                If index >= 0 Then obj = Me.InnerList.Item(index)
                Return obj
            End Get
            Set(ByVal value As IndicadorCumplimientoMeta)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property ItemDe(ByVal id As Integer) As IndicadorCumplimientoMeta
            Get
                For index As Integer = 0 To Me.InnerList.Count - 1
                    With CType(Me.InnerList(index), IndicadorCumplimientoMeta)
                        If .IdIndicador = id Then Return Me.InnerList(index)
                    End With
                Next
                Return Nothing
            End Get
        End Property

        Public Property Valor() As Decimal
            Get
                Return _valor
            End Get
            Set(ByVal value As Decimal)
                _valor = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim infoObj As Type = GetType(IndicadorCumplimientoMeta)
            Dim pInfo As PropertyInfo

            For Each pInfo In infoObj.GetProperties
                If pInfo.PropertyType.Namespace = "System" Then
                    With dtAux
                        .Columns.Add(pInfo.Name, pInfo.PropertyType)
                    End With
                End If
            Next

            Return dtAux
        End Function

#End Region

#Region "Métodos Públicos"

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As IndicadorCumplimientoMeta)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As IndicadorCumplimientoMeta)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As IndicadorCumplimientoMetaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As IndicadorCumplimientoMeta)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal identificador As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), IndicadorCumplimientoMeta)
                    If .IdIndicador = identificador Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function GenerarDataTable() As DataTable
            If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim infoObj As IndicadorCumplimientoMeta

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                infoObj = CType(Me.InnerList(index), IndicadorCumplimientoMeta)
                If infoObj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(IndicadorCumplimientoMeta).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(infoObj, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As LMDataAccess = Nothing
            Try
                dbManager = New LMDataAccess
                Me.Clear()
                With dbManager
                    If Me._valor >= 0 Then .SqlParametros.Add("@valorInicial", SqlDbType.Decimal).Value = Me._valor
                    .ejecutarReader("ObtenerIndicadoresDeCumplimiento", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim infoObj As IndicadorCumplimientoMeta
                        While .Reader.Read
                            infoObj = New IndicadorCumplimientoMeta
                            infoObj.AsignarValorAPropiedades(.Reader)
                            Me.InnerList.Add(infoObj)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As LMDataAccess = Nothing
            Try
                dbManager = New LMDataAccess
                Dim indicador As IndicadorCumplimientoMeta
                Dim idUsuario As Integer
                Dim contexto As HttpContext = HttpContext.Current
                If contexto IsNot Nothing AndAlso contexto.Session IsNot Nothing AndAlso contexto.Session("userId") IsNot Nothing Then _
                    Integer.TryParse(contexto.Session("userId").ToString, idUsuario)
                If idUsuario = 0 Then idUsuario = 1
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@idIndicador", SqlDbType.SmallInt)
                    .SqlParametros.Add("@valorInicial", SqlDbType.Decimal)
                    .SqlParametros.Add("@valorFinal", SqlDbType.Decimal)
                    .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output
                    .iniciarTransaccion()
                    For index As Integer = 0 To Me.InnerList.Count - 1
                        indicador = CType(Me.InnerList(index), IndicadorCumplimientoMeta)
                        If indicador.Modificado Then
                            .SqlParametros("@idIndicador").Value = indicador.IdIndicador
                            .SqlParametros("@valorInicial").Value = indicador.ValorInicial
                            .SqlParametros("@valorFinal").Value = indicador.ValorFinal
                            .ejecutarNonQuery("ActualizarIndicadorDeCumplimiento", CommandType.StoredProcedure)
                        End If
                        If Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor) Then
                            If resultado.Valor <> 0 Then
                                If .estadoTransaccional Then .abortarTransaccion()
                                Exit For
                            Else
                                indicador.Modificado = False
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(300, "Imposible evaluar la respuesta proveniente del servidor de bases de datos. Por favor intente nuevamente")
                            Exit For
                        End If
                    Next

                    If resultado.Valor = 0 Then
                        If .estadoTransaccional Then .confirmarTransaccion()
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return resultado
        End Function

#End Region

    End Class

End Namespace