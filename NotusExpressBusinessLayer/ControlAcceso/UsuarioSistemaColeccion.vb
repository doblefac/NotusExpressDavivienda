Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.ControlAcceso
Imports System.Reflection

Public Class UsuarioSistemaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idUsuario As Integer
    Private _numeroIdentificacion As String
    Private _nombreApellido As String
    Private _idCargo As Integer
    Private _idCiudad As Integer
    Private _idEstado As Enumerados.EstadoBinario
    Private _idRol As Integer
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _idEstado = Enumerados.EstadoBinario.NoEstablecido
    End Sub

    Public Sub New(ByVal idUsuario As Integer)
        Me.New()
        _idUsuario = idUsuario
        CargarDatos()
    End Sub

#End Region
#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As UsuarioSistema
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As UsuarioSistema)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property NumeroIdentificacion() As String
        Get
            Return _numeroIdentificacion
        End Get
        Set(ByVal value As String)
            _numeroIdentificacion = value
        End Set
    End Property

    Public Property NombreApellido() As String
        Get
            Return _nombreApellido
        End Get
        Set(ByVal value As String)
            _nombreApellido = value
        End Set
    End Property

    Public Property IdCargo() As Integer
        Get
            Return _idCargo
        End Get
        Set(ByVal value As Integer)
            _idCargo = value
        End Set
    End Property

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set

    End Property

    Public Property IdRol() As Short
        Get
            Return _idRol
        End Get
        Set(ByVal value As Short)
            _idRol = value
        End Set
    End Property

    Public Property IdEstado() As Enumerados.EstadoBinario
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _idEstado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miUsuario As Type = GetType(UsuarioSistema)
        Dim pInfo As PropertyInfo

        For Each pInfo In miUsuario.GetProperties
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

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miUsuarioSistema As UsuarioSistema

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miUsuarioSistema = CType(Me.InnerList(index), UsuarioSistema)
            If miUsuarioSistema IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(UsuarioSistema).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miUsuarioSistema, Nothing)
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
                If Me._idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                If Me._numeroIdentificacion <> "" Then .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 15).Value = Me._numeroIdentificacion
                If Me._nombreApellido <> "" Then .SqlParametros.Add("@nombreApellido", SqlDbType.VarChar, 100).Value = Me._nombreApellido
                If Me._idCiudad > 0 Then .SqlParametros.Add("@idCuidad", SqlDbType.SmallInt).Value = Me._idCiudad
                If Me._idCargo > 0 Then .SqlParametros.Add("@idCargo", SqlDbType.SmallInt).Value = Me._idCargo
                If Me._idRol > 0 Then .SqlParametros.Add("@idRol", SqlDbType.SmallInt).Value = Me._idRol
                If Me._idEstado <> Enumerados.EstadoBinario.NoEstablecido > 0 Then _
                    .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = IIf(Me._idEstado = Enumerados.EstadoBinario.Activo, 1, 0)
                .ejecutarReader("ObtenerInfoUsuarioSistema", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim miUsuarioSistema As UsuarioSistema
                    While .Reader.Read
                        miUsuarioSistema = New UsuarioSistema
                        miUsuarioSistema.CargarResultadoConsultaEdicion(.Reader)
                        Me.InnerList.Add(miUsuarioSistema)
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
