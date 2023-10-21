Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports System.Reflection

Namespace MaestroProductos

    Public Class DocumentoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _ventaPresencial As Enumerados.EstadoBinario
        Private _idTipoProducto As Short
        Private _idCategoria As Short
        Private _idEstado As Byte
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Documento
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As Documento)
                If value IsNot Nothing AndAlso Not value.Registrado Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
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

        Public Property IdTipoProducto() As Byte
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Byte)
                _idTipoProducto = value
            End Set
        End Property

        Public Property IdCategoria() As Integer
            Get
                Return _idCategoria
            End Get
            Set(value As Integer)
                _idCategoria = value
            End Set
        End Property

        Public Property VentaPresencial() As Enumerados.EstadoBinario
            Get
                Return _ventaPresencial
            End Get
            Set(value As Enumerados.EstadoBinario)
                _ventaPresencial = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim tipoVenta As Type = GetType(Documento)
            Dim pInfo As PropertyInfo

            For Each pInfo In tipoVenta.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Documento)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Documento)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DocumentoColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Documento)
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
                With CType(Me.InnerList(index), Documento)
                    If .IdDocumento = identificador Then
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
            Dim infoProducto As Documento

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                infoProducto = CType(Me.InnerList(index), Documento)
                If infoProducto IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Documento).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(infoProducto, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next
            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                Me.Clear()
                With dbManager
                    If _idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTipoProducto
                    If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = _idEstado
                    If Me._ventaPresencial <> Enumerados.EstadoBinario.NoEstablecido Then _
                            .SqlParametros.Add("@ventaPresencial", SqlDbType.Bit).Value = IIf(Me._ventaPresencial = Enumerados.EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ObtenerInfoDocumento", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim infoDocumento As Documento
                        While .Reader.Read
                            infoDocumento = New Documento
                            infoDocumento.AsignarValorAPropiedades(.Reader)
                            Me.InnerList.Add(infoDocumento)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Function GenerarDataTablePresencial() As DataTable
            If Not _cargado Then CargarDatosPresencial()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim infoProducto As Documento

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                infoProducto = CType(Me.InnerList(index), Documento)
                If infoProducto IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Documento).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(infoProducto, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next
            Return dtAux
        End Function

        Public Sub CargarDatosPresencial()
            Dim dbManager As New LMDataAccess
            Try
                Me.Clear()
                With dbManager
                    .SqlParametros.Add("@idCategoria", SqlDbType.SmallInt).Value = _idCategoria
                    .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTipoProducto
                    .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = _idEstado
                    .SqlParametros.Add("@ventaPresencial", SqlDbType.Bit).Value = IIf(Me._ventaPresencial = Enumerados.EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ObtenerInfoDocumentoPresencial", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim infoDocumento As Documento
                        While .Reader.Read
                            infoDocumento = New Documento
                            infoDocumento.AsignarValorAPropiedades(.Reader)
                            Me.InnerList.Add(infoDocumento)
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