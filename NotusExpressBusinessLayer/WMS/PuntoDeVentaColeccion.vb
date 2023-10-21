Imports LMDataAccessLayer
Imports System.Reflection

Public Class PuntoDeVentaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idCadena As Short
    Private _idEmpresa As Short
    Private _idUnidadNegocio As Short
    Private _idEstado As Byte
    Private _idUsuario As Integer
    Private _idCiudadB As Integer
    Private _idEstrategia As Integer
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As PuntoDeVenta
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As PuntoDeVenta)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdCadena() As Short
        Get
            Return _idCadena
        End Get
        Set(ByVal value As Short)
            _idCadena = value
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

    Public Property IdUnidadNegocio() As Short
        Get
            Return _idUnidadNegocio
        End Get
        Set(ByVal value As Short)
            _idUnidadNegocio = value
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

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property IdEstrategia As Integer
        Get
            Return _idEstrategia
        End Get
        Set(value As Integer)
            _idEstrategia = value
        End Set
    End Property

    Public Property IdCudadB() As Integer
        Get
            Return _idCiudadB
        End Get
        Set(ByVal value As Integer)
            _idCiudadB = value
        End Set
    End Property
#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim pdv As Type = GetType(PuntoDeVenta)
        Dim pInfo As PropertyInfo

        For Each pInfo In pdv.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As PuntoDeVenta)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As PuntoDeVenta)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As PuntoDeVentaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As PuntoDeVenta)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idPdv As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), PuntoDeVenta)
                If .IdPdv = idPdv Then
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
        Dim pdv As PuntoDeVenta

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            pdv = CType(Me.InnerList(index), PuntoDeVenta)
            If pdv IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(PuntoDeVenta).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(pdv, Nothing)
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
                If Me._idCadena > 0 Then .SqlParametros.Add("@idCadena", SqlDbType.Int).Value = Me._idCadena
                If Me._idEmpresa > 0 Then .SqlParametros.Add("@idEmpresa", SqlDbType.SmallInt).Value = Me._idEmpresa
                If Me._idUnidadNegocio > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = Me._idUnidadNegocio
                If Me._idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = Me._idEstado
                If Me._idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                If Me._idEstrategia > 0 Then .SqlParametros.Add("@idEstrategia", SqlDbType.Int).Value = Me._idEstrategia
                If Me._idCiudadB > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = Me._idCiudadB
                .ejecutarReader("ObtenerInfoPuntoDeVenta", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim pdv As PuntoDeVenta
                    While .Reader.Read
                        pdv = New PuntoDeVenta
                        pdv.AsignarValorAPropiedades(.Reader)
                        Me.InnerList.Add(pdv)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
    Public Sub LitaPdvNoasignados()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                If Me._idCiudadB > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = Me._idCiudadB
                .ejecutarReader("ObtenerInfoPuntoDeVentaNoAsignados", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim pdv As PuntoDeVenta
                    While .Reader.Read
                        pdv = New PuntoDeVenta
                        pdv.AsignarValorAPropiedades(.Reader)
                        Me.InnerList.Add(pdv)
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
