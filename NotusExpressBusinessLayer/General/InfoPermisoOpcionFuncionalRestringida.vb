Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer

Namespace General

    Public Class InfoPermisoOpcionFuncionalRestringida

#Region "Atributos"

        Private _funcionalidad As String
        Private _nombreControl As String
        Private _permisos As DataTable

#End Region

#Region "Constructores"

        Public Sub New(funcionalidad As String)
            _funcionalidad = funcionalidad
            CargarInformacion()
        End Sub

        Public Sub New(funcionalidad As String, nombreControl As String)
            _funcionalidad = funcionalidad
            _nombreControl = nombreControl
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property Funcionalidad As String
            Get
                Return _funcionalidad
            End Get
        End Property

        Public Property NombreControl As String
            Get
                Return _nombreControl
            End Get
            Set(value As String)
                _nombreControl = value
            End Set
        End Property

        Public ReadOnly Property ListaPermisos As DataTable
            Get
                If _permisos Is Nothing Then CargarInformacion()
                Return _permisos
            End Get
        End Property

#End Region

#Region "Métodos"

        Public Sub CargarInformacion()
            Dim dbManager As LMDataAccess = Nothing
            Try
                dbManager = New LMDataAccess
                With dbManager
                    .SqlParametros.Add("@nombreFormulario", SqlDbType.VarChar, 100).Value = _funcionalidad.Trim
                    If Not String.IsNullOrEmpty(_nombreControl) Then .SqlParametros.Add("@nombreControl", SqlDbType.VarChar, 100).Value = _nombreControl.Trim
                    _permisos = .ejecutarDataTable("InfoPermisoOpcionFuncionalRestringida", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Function PermitirAcceso(nombreControl As String) As Boolean
            Dim arrPerfiles As ArrayList
            Dim contexto As System.Web.HttpContext = System.Web.HttpContext.Current
            If contexto.Session("arrIdPerfiles") IsNot Nothing Then
                If String.IsNullOrEmpty(nombreControl) Then nombreControl = _nombreControl
                If _permisos Is Nothing Then CargarInformacion()
                arrPerfiles = CType(contexto.Session("arrIdPerfiles"), ArrayList)
                If arrPerfiles.Count > 0 Then
                    Dim filtro As String = "nombreControl='" & nombreControl.Trim & "' and idPerfil in (" & Join(arrPerfiles.ToArray, ",") & ")"
                    If _permisos.Select(filtro).Count > 0 Then Return True
                Else
                    Return True
                End If
            Else
                Return True
            End If

            Return False
        End Function

#End Region

    End Class

End Namespace