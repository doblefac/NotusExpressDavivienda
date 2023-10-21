Imports LMDataAccessLayer

Public Class TipoPersonaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idTipoPersona As Integer
    Private _idEstado As Byte
  Private _cargado As Boolean
#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As TipoPersona
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As TipoPersona)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdTipoPersona() As Integer
        Get
            Return _idTipoPersona
        End Get
        Set(ByVal value As Integer)
            _idTipoPersona = value
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

    Public Sub New(ByVal IdTipoPersona As Integer)
        Me.New()
        _idTipoPersona = IdTipoPersona
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Públicos"
    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If Me._idTipoPersona > 0 Then .SqlParametros.Add("@idTipoPersona", SqlDbType.SmallInt).Value = Me._idTipoPersona
                If Me._idEstado <> Enumerados.EstadoBinario.NoEstablecido > 0 Then _
                    .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = IIf(Me._idEstado = Enumerados.EstadoBinario.Activo, 1, 0)
                .ejecutarReader("ObtenerInfoTipoPersona", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim miTipoPersona As TipoPersona
                    While .Reader.Read
                        miTipoPersona = New TipoPersona
                        miTipoPersona.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(miTipoPersona)
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
