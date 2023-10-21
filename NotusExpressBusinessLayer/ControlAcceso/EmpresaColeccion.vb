Imports LMDataAccessLayer

Public Class EmpresaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idEstado As Byte
    Private _idEmpresa As Short
    Private _cargado As Boolean
#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As Empresa
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As Empresa)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
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

    Public Sub New(ByVal idEmpresa As Integer)
        Me.New()
        _idEmpresa = idEmpresa
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Públicos"
    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If Me._idEmpresa > 0 Then .SqlParametros.Add("@idEmpresa", SqlDbType.SmallInt).Value = Me._idEmpresa
                If Me._idEstado <> Enumerados.EstadoBinario.NoEstablecido > 0 Then _
                    .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = IIf(Me._idEstado = Enumerados.EstadoBinario.Activo, 1, 0)
                .ejecutarReader("ObtenerInfoEmpresa", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim miUnidadDeNegocio As Empresa
                    While .Reader.Read
                        miUnidadDeNegocio = New Empresa
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
    Public Function CargarComboEmpresas() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtEmpresa As New DataTable
        Try
            With dbManager
                dtEmpresa = .ejecutarDataTable("ObtenerInfoEmpresaCombo", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtEmpresa
    End Function


#End Region
End Class
