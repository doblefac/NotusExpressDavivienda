Imports LMDataAccessLayer
Imports System.Reflection

Public Class UnidadDeNegocioColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idUnidadNegocio As Integer
    Private _idEstado As Byte
    Private _idEmpresa As Short
    Private _cargado As Boolean
#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As UnidadDeNegocio
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As UnidadDeNegocio)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdUnidadNegocio() As Integer
        Get
            Return _idUnidadNegocio
        End Get
        Set(ByVal value As Integer)
            _idUnidadNegocio = value
        End Set
    End Property

    Public Property IdEmpresa() As Short
        Get
            Return _idEmpresa
        End Get
        Set(ByVal value As Short)
            _idEmpresa = value
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
#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _idEstado = Enumerados.EstadoBinario.Activo
    End Sub

    Public Sub New(ByVal idUnidadNegocio As Integer)
        Me.New()
        _idUnidadNegocio = idUnidadNegocio
        CargarDatos()
    End Sub

#End Region

#Region "Metodos Privados"
    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miUsuario As Type = GetType(UnidadDeNegocio)
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
        Dim miUsuarioSistema As UnidadDeNegocio

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miUsuarioSistema = CType(Me.InnerList(index), UnidadDeNegocio)
            If miUsuarioSistema IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(UnidadDeNegocio).GetProperties
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
            With dbManager
                If (Me._idEmpresa > 0) Then .SqlParametros.Add("@idEmpresa", SqlDbType.SmallInt).Value = Me._idEmpresa
                If Me._idEstado <> Enumerados.EstadoBinario.NoEstablecido > 0 Then _
                    .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = IIf(Me._idEstado = Enumerados.EstadoBinario.Activo, 1, 0)
                .ejecutarReader("ObtenerInfoUnidadDeNegocio", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim miUnidadDeNegocio As UnidadDeNegocio
                    While .Reader.Read
                        miUnidadDeNegocio = New UnidadDeNegocio
                        miUnidadDeNegocio.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(miUnidadDeNegocio)
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
