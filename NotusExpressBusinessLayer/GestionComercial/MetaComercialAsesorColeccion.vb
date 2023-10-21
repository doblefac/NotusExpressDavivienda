Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Namespace ConfiguracionComercial

    Public Class MetaComercialAsesorColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idMeta As Integer
        Private _idEstrategia As Short
        Private _idCampania As Short
        Private _idPersonaAsesor As Integer
        Private _idTipoProducto As Short
        Private _anio As Short
        Private _mes As Short
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As MetaComercialAsesor
            Get
                Dim obj As MetaComercialAsesor = Nothing
                If index >= 0 Then obj = Me.InnerList.Item(index)
                Return obj
            End Get
            Set(ByVal value As MetaComercialAsesor)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdMeta() As Integer
            Get
                Return _idMeta
            End Get
            Set(ByVal value As Integer)
                _idMeta = value
            End Set
        End Property

        Public Property IdEstrategia As Short
            Get
                Return _idEstrategia
            End Get
            Set(value As Short)
                _idEstrategia = value
            End Set
        End Property

        Public Property IdCampania As Short
            Get
                Return _idCampania
            End Get
            Set(value As Short)
                _idCampania = value
            End Set
        End Property

        Public Property IdPersonaAsesor As Integer
            Get
                Return _idPersonaAsesor
            End Get
            Set(value As Integer)
                _idPersonaAsesor = value
            End Set
        End Property

        Public Property IdTipoProducto As Short
            Get
                Return _idTipoProducto
            End Get
            Set(value As Short)
                _idTipoProducto = value
            End Set
        End Property

        Public Property Anio As Short
            Get
                Return _anio
            End Get
            Set(value As Short)
                _anio = value
            End Set
        End Property

        Public Property Mes As Short
            Get
                Return _mes
            End Get
            Set(value As Short)
                _mes = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim infoObj As Type = GetType(MetaComercialAsesor)
            Dim pInfo As PropertyInfo

            For Each pInfo In infoObj.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As MetaComercialAsesor)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As MetaComercialAsesor)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As MetaComercialAsesorColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As MetaComercialAsesor)
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
                With CType(Me.InnerList(index), MetaComercialAsesor)
                    If .IdMeta = identificador Then
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
            Dim infoObj As MetaComercialAsesor

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                infoObj = CType(Me.InnerList(index), MetaComercialAsesor)
                If infoObj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(MetaComercialAsesor).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(infoObj, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As LMDataAccess = Nothing
            Try
                dbManager = New LMDataAccess
                Me.Clear()
                With dbManager
                    If Me._idMeta > 0 Then .SqlParametros.Add("@idMeta", SqlDbType.Int).Value = Me._idMeta
                    If Me._idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.SmallInt).Value = Me._idCampania
                    If Me._idPersonaAsesor > 0 Then .SqlParametros.Add("@idPersonaAsesor", SqlDbType.Int).Value = Me._idPersonaAsesor
                    If Me._idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = Me._idTipoProducto
                    If Me._anio > 0 Then .SqlParametros.Add("@anio", SqlDbType.SmallInt).Value = Me._anio
                    If Me._mes > 0 Then .SqlParametros.Add("@mes", SqlDbType.SmallInt).Value = Me._mes

                    .ejecutarReader("ObtenerInfoMetaComercialAsesor", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim infoObj As MetaComercialAsesor
                        While .Reader.Read
                            infoObj = New MetaComercialAsesor
                            infoObj.AsignarValorAPropiedades(.Reader, "idAsesor", "Asesor")
                            Me.InnerList.Add(infoObj)
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