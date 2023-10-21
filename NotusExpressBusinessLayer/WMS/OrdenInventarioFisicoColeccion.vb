Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Namespace WMS

    Public Class OrdenInventarioFisicoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idBodega As Integer
        Private _idProducto As Integer
        Private _idSubproducto As Integer
        Private _fechaInicial As Date
        Private _fechaFinal As Date
        Private _idTipoFecha As TipoFechaConsulta
        Private _idEstado As Byte
        Private _idCreador As Integer
        Private _idResponsableCierre As Integer
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _idTipoFecha = TipoFechaConsulta.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As OrdenInventarioFisico
            Get
                Dim obj As OrdenInventarioFisico = Nothing
                If index >= 0 Then obj = Me.InnerList.Item(index)
                Return obj
            End Get
            Set(ByVal value As OrdenInventarioFisico)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdBodega() As Integer
            Get
                Return _idBodega
            End Get
            Set(ByVal value As Integer)
                _idBodega = value
            End Set
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property IdSubproducto() As Integer
            Get
                Return _idSubproducto
            End Get
            Set(ByVal value As Integer)
                _idSubproducto = value
            End Set
        End Property

        Public Property FechaInicial() As Date
            Get
                Return _fechaInicial
            End Get
            Set(ByVal value As Date)
                _fechaInicial = value
            End Set
        End Property

        Public Property FechaFinal() As Date
            Get
                Return _fechaFinal
            End Get
            Set(ByVal value As Date)
                _fechaFinal = value
            End Set
        End Property

        Public Property IdTipoFecha() As TipoFechaConsulta
            Get
                Return _idTipoFecha
            End Get
            Set(ByVal value As TipoFechaConsulta)
                _idTipoFecha = value
            End Set
        End Property

        Public Property IdEstado() As Byte
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Byte)
                _idEstado = value
            End Set
        End Property

        Public Property IdCreador() As Integer
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Integer)
                _idCreador = value
            End Set
        End Property

        Public Property IdResponsableCierre() As Integer
            Get
                Return _idResponsableCierre
            End Get
            Set(ByVal value As Integer)
                _idResponsableCierre = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim infoObj As Type = GetType(OrdenInventarioFisico)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As OrdenInventarioFisico)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As OrdenInventarioFisico)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As PuntoDeVentaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As OrdenInventarioFisico)
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
                With CType(Me.InnerList(index), OrdenInventarioFisico)
                    If .IdOrden = identificador Then
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
            Dim infoObj As OrdenInventarioFisico

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                infoObj = CType(Me.InnerList(index), OrdenInventarioFisico)
                If infoObj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(OrdenInventarioFisico).GetProperties
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
                    If Me._idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = Me._idBodega
                    If Me._idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = Me._idProducto
                    If Me._idSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = Me._idSubproducto
                    If Me._fechaInicial > Date.MinValue AndAlso Me._fechaFinal > Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = Me._fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = Me._fechaFinal
                        If Me.IdTipoFecha <> TipoFechaConsulta.NoEstablecido Then .SqlParametros.Add("@idTipoFecha", SqlDbType.TinyInt).Value = Me._idTipoFecha
                    End If
                    If Me._idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = Me._idEstado
                    If Me._idCreador > 0 Then .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = Me._idCreador
                    If Me._idResponsableCierre > 0 Then .SqlParametros.Add("@idResponsableCierre", SqlDbType.Int).Value = Me._idResponsableCierre

                    .ejecutarReader("ObtenerInfoOrdenInventarioFisico", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim infoObj As OrdenInventarioFisico
                        While .Reader.Read
                            infoObj = New OrdenInventarioFisico
                            infoObj.CargarResultadoConsulta(.Reader)
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

#End Region

    End Class

End Namespace
