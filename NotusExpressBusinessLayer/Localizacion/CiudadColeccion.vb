Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.Comunes
Imports System.Net

Namespace Localizacion

    Public Class CiudadColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idPais As Integer
        Private _nombre As String
        Private _departamento As String
        Private _idRegion As Integer
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _idPais = 170
        End Sub

        Public Sub New(ByVal idPais As Integer)
            Me.New()
            _idPais = idPais
            CargarDatos()
        End Sub

        Public Sub New(ByVal departamento As String)
            Me.New()
            _departamento = departamento
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Ciudad
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As Ciudad)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdPais() As Integer
            Get
                Return _idPais
            End Get
            Set(ByVal value As Integer)
                _idPais = value
            End Set
        End Property

        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set
        End Property

        Public Property Departamento() As String
            Get
                Return _departamento
            End Get
            Set(ByVal value As String)
                _departamento = value
            End Set
        End Property

        Public Property IdRegion() As Integer
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Integer)
                _idRegion = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miCiudad As Type = GetType(Ciudad)
            Dim pInfo As PropertyInfo

            For Each pInfo In miCiudad.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Ciudad)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Ciudad)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As CiudadColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Ciudad)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idCiudad As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), Ciudad)
                    If .IdCiudad = idCiudad Then
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
            Dim miCiudad As Ciudad

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miCiudad = CType(Me.InnerList(index), Ciudad)
                If miCiudad IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Ciudad).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miCiudad, Nothing)
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
                    If Me._idPais > 0 Then .SqlParametros.Add("@idPais", SqlDbType.Int).Value = Me._idPais
                    If Not String.IsNullOrEmpty(Me._nombre) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = Me._nombre
                    If Not String.IsNullOrEmpty(Me._departamento) Then .SqlParametros.Add("@departamento", SqlDbType.VarChar, 100).Value = Me._departamento
                    .ejecutarReader("ObtenerInfoCiudad", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim miCiudad As Ciudad
                        While .Reader.Read
                            miCiudad = New Ciudad
                            miCiudad.CargarResultadoConsulta(.Reader)
                            _cargado = True
                            Me.InnerList.Add(miCiudad)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub
        Public Function ObtenerCiudadesCobox(Ciudad As String, startIndex As Integer, endIndex As Integer) As DataTable

            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@Ciudad", SqlDbType.NVarChar).Value = String.Format("{0}%", Ciudad)
                    If (startIndex > 0) Then .SqlParametros.Add("@startIndex", SqlDbType.Int).Value = startIndex
                    If (endIndex > 0) Then .SqlParametros.Add("@endIndex", SqlDbType.Int).Value = endIndex
                    .TiempoEsperaComando = 0
                    Return .EjecutarDataTable("ObtenerCiudadesCombobox", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Function

        Public Function ObtenerCiudadCampania(idCampania) As DataTable
            Dim dtDatos As New DataTable
            Dim dsDatos As New DataSet
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
            Dim objCampania As New NotusIlsService.NotusIlsService
            Dim infoWs As New InfoUrlService(objCampania, True)
            Dim Wsresultado As New NotusExpressBusinessLayer.NotusIlsService.ResultadoProceso
            Wsresultado = objCampania.ConsultarCiudadesCampania(dsDatos, idCampania)
            dtDatos = dsDatos.Tables(0)
            Return dtDatos
        End Function
#End Region

    End Class

End Namespace