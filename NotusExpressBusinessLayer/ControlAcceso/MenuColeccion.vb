Imports LMDataAccessLayer
Imports System.Reflection

Namespace ControlAcceso

    Public Class MenuColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idUsuario As Integer
        Private _idPerfil As Integer
        Private _idUnidadNegocio As Integer
        Private _idTipoApp As Byte
        Private _cargado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idUsuario As Integer)
            Me.New()
            _idUsuario = idUsuario
            CargarDatos()
        End Sub

        Public Sub New(ByVal idPerfil As Integer, ByVal idUnidadNegocio As Integer)
            Me.New()
            _idPerfil = idPerfil
            _idUnidadNegocio = idUnidadNegocio
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Menu
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As Menu)
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

        Public Property IdPerfil() As Integer
            Get
                Return _idPerfil
            End Get
            Set(ByVal value As Integer)
                _idPerfil = value
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

        Public Property IdTipoApp() As Byte
            Get
                Return _idTipoApp
            End Get
            Set(ByVal value As Byte)
                _idTipoApp = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miMenu As Type = GetType(Menu)
            Dim pInfo As PropertyInfo

            For Each pInfo In miMenu.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Menu)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Menu)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As MenuColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Menu)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idMenu As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), Menu)
                    If .IdMenu = idMenu Then
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
            Dim miMenu As Menu

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miMenu = CType(Me.InnerList(index), Menu)
                If miMenu IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Menu).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miMenu, Nothing)
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
                    If Me._idPerfil > 0 Then .SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = Me._idPerfil
                    If Me._idUnidadNegocio > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.Int).Value = Me._idUnidadNegocio
                    If _idTipoApp > 0 Then .SqlParametros.Add("@idTipoApp", SqlDbType.TinyInt).Value = Me._idTipoApp
                    .ejecutarReader("ObtenerInfoMenus", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim miMenu As Menu
                        While .Reader.Read
                            miMenu = New Menu
                            miMenu.CargarResultadoConsulta(.Reader)
                            Me.InnerList.Add(miMenu)
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