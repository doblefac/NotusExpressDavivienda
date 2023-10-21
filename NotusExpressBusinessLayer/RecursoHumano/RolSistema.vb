Imports LMDataAccessLayer

Public Class RolSistema
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idRol As Integer
    Private _nombre As String
    Private _descripcion As String
    Private _idEstado As Boolean
    Private _estado As String
#End Region


#Region "Propiedades"


    Public Property IdRol() As Integer
        Get
            Return _idRol
        End Get
        Set(ByVal value As Integer)
            _idRol = value
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

    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    Public Property IdEstado() As Boolean
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Boolean)
            _idEstado = value
        End Set
    End Property
    Public Property Estado As String
        Get
            Return _estado
        End Get
        Set(value As String)
            _estado = value
        End Set
    End Property
#End Region

#Region "Métodos Públicos"
    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If Me._idRol > 0 Then .SqlParametros.Add("@idRol", SqlDbType.Int).Value = Me._idRol
                If Not String.IsNullOrEmpty(Me._nombre) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = Me._nombre
                .ejecutarReader("ObtenerRol", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim mirol As RolSistema
                    While .Reader.Read
                        mirol = New RolSistema
                        mirol.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(mirol)
                    End While
                    .Reader.Close()
                End If
            End With

        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub


#End Region
#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idrol").ToString, _idRol)
                _nombre = reader("nombre").ToString
                _descripcion = reader("descripcion").ToString
                Short.TryParse(reader("idestado").ToString, _idEstado)
                _estado = reader("Estado").ToString

            End If
        End If

    End Sub

#End Region
End Class
