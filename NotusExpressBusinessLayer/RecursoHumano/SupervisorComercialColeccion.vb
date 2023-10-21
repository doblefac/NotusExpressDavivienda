Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports System.Reflection

Namespace RecursoHumano

    Public Class SupervisorComercialColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idPdv As Short
        Private _idEmpresa As Short
        Private _idUnidadNegocio As Short
        Private _idEstado As Byte
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As SupervisorComercial
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As SupervisorComercial)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdPdv() As Short
            Get
                Return _idPdv
            End Get
            Set(ByVal value As Short)
                _idPdv = value
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

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim obj As Type = GetType(SupervisorComercial)
            Dim pInfo As PropertyInfo

            For Each pInfo In obj.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As SupervisorComercial)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As SupervisorComercial)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As PuntoDeVentaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As SupervisorComercial)
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
                With CType(Me.InnerList(index), SupervisorComercial)
                    If .IdUsuarioSistema = identificador Then
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
            Dim obj As SupervisorComercial

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                obj = CType(Me.InnerList(index), SupervisorComercial)
                If obj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(SupervisorComercial).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(obj, Nothing)
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
                If Me.InnerList.Count > 0 Then Me.InnerList.Clear()
                With dbManager
                    If Me._idPdv > 0 Then .SqlParametros.Add("@idPdv", SqlDbType.Int).Value = Me._idPdv
                    If Me._idEmpresa > 0 Then .SqlParametros.Add("@idEmpresa", SqlDbType.SmallInt).Value = Me._idEmpresa
                    If Me._idUnidadNegocio > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = Me._idUnidadNegocio
                    If Me._idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = Me._idEstado

                    .ejecutarReader("ObtenerInfoSupervisorComercial", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim pdv As SupervisorComercial
                        While .Reader.Read
                            pdv = New SupervisorComercial
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

End Namespace