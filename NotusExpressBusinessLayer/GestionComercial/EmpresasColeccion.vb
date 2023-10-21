Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class EmpresasColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"
    Private _idTipoEmpresa As Integer
    Private _idEstado As Nullable(Of Boolean)
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As GestionComercial.Empresas
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As GestionComercial.Empresas)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdTipoEmpresa As Integer
        Get
            Return _idTipoEmpresa
        End Get
        Set(value As Integer)
            _idTipoEmpresa = value
        End Set
    End Property

    Public Property IdEstado As String
        Get
            Return _idEstado
        End Get
        Set(value As String)
            _idEstado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objAbreviaturaDireccion As Type = GetType(GestionComercial.Empresas)
        Dim pInfo As PropertyInfo

        For Each pInfo In objAbreviaturaDireccion.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GestionComercial.Empresas)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As GestionComercial.Empresas)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As GestionComercial.Empresas)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As GestionComercial.Empresas

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), GestionComercial.Empresas)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(GestionComercial.Empresas).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next
        _cargado = False
        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim idUsuarioConsulta As Integer = 0
        Using dbManager As New LMDataAccess
            With dbManager
                Try
                    .SqlParametros.Clear()
                    If _idTipoEmpresa > 0 Then .SqlParametros.Add("@idTipoEmpresa", SqlDbType.Int).Value = _idTipoEmpresa
                    If _idEstado IsNot Nothing Then .SqlParametros.Add("@idEstado", SqlDbType.Bit).Value = _idEstado

                    .ejecutarReader("ObtenerInfoEmpresa", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As GestionComercial.Empresas
                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New GestionComercial.Empresas
                                Long.TryParse(.Reader("idEmpresa"), elDetalle.IdEmpresa)
                                elDetalle.Nombre = .Reader("nombre")
                                elDetalle.Codigo = .Reader("codigo")
                                Long.TryParse(.Reader("idTipoEmpresa"), elDetalle.IdTipoEmpresa)
                                Integer.TryParse(.Reader("idEstado"), elDetalle.IdEstado)
                                _cargado = True
                                Me.InnerList.Add(elDetalle)
                            End If
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End With
        End Using
    End Sub

#End Region

End Class
