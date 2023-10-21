Imports LMDataAccessLayer
Imports System.Reflection

Namespace ControlAcceso

    Public Class PerfilColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idRol As Short
        Private _idUsuario As Integer
        Private _idEstado As Enumerados.EstadoBinario
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

        Default Public Property Item(ByVal index As Integer) As Perfil
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As Perfil)
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
            Dim miPerfil As Type = GetType(Perfil)
            Dim pInfo As PropertyInfo

            For Each pInfo In miPerfil.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Perfil)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Perfil)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As PerfilColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Perfil)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idPerfil As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), Perfil)
                    If .IdPerfil = idPerfil Then
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
            Dim miPerfil As Perfil

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miPerfil = CType(Me.InnerList(index), Perfil)
                If miPerfil IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Perfil).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miPerfil, Nothing)
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
                    If Me._idRol > 0 Then .SqlParametros.Add("@idRol", SqlDbType.SmallInt).Value = Me._idRol
                    If Me._idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                    If Me._idEstado <> Enumerados.EstadoBinario.NoEstablecido > 0 Then _
                        .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = IIf(Me._idEstado = Enumerados.EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ObtenerInfoPerfil", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim miPerfil As Perfil
                        While .Reader.Read
                            miPerfil = New Perfil
                            miPerfil.CargarResultadoConsulta(.Reader)
                            Me.InnerList.Add(miPerfil)
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