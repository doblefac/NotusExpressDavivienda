Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer

Namespace RecursoHumano

    Public Class SupervisorComercial
        Inherits PersonaBase

#Region "Atributos"

        Private _idUsuarioSistema As Integer
        Private _idUnidadNegocio As Short
        Private _unidadNegocio As String

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _unidadNegocio = ""
        End Sub

        Public Sub New(idUsuarioSistema As Integer)
            Me.New()
            Me._idUsuarioSistema = idUsuarioSistema
            CargarInformacion()
        End Sub

        Public Sub New(numIdentificacion As String)
            Me.New()
            Me.NumeroIdentificacion = numIdentificacion
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdUsuarioSistema As String
            Get
                Return _idUsuarioSistema
            End Get
            Set(value As String)
                _idUsuarioSistema = value
            End Set
        End Property

        Public Property IdUnidadNegocio As Short
            Get
                Return _idUnidadNegocio
            End Get
            Set(value As Short)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property UnidadNegocio As String
            Get
                Return _unidadNegocio
            End Get
            Protected Friend Set(value As String)
                _unidadNegocio = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"


#End Region

#Region "Métodos Protegidos"

        Protected Overloads Sub CargarInformacion()
            If Me.IdPersona > 0 OrElse Not String.IsNullOrEmpty(Me.NumeroIdentificacion) OrElse Me._idUsuarioSistema > 0 Then
                Dim dbManager As LMDataAccess = Nothing
                Try
                    dbManager = New LMDataAccess
                    With dbManager
                        If Me._idUsuarioSistema > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuarioSistema
                        If Me.IdPersona > 0 Then .SqlParametros.Add("@idPersona", SqlDbType.Int).Value = Me.IdPersona
                        If Not String.IsNullOrEmpty(Me.NumeroIdentificacion) Then _
                            .SqlParametros.Add("@numeroIdentificacion", SqlDbType.VarChar, 20).Value = Me.NumeroIdentificacion.Trim
                        .ejecutarReader("ObtenerInfoSupervisorComercial", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then AsignarValorAPropiedades(.Reader)
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then

                If reader.HasRows Then
                    Integer.TryParse(reader("idUsuarioSistema").ToString, Me._idUsuarioSistema)
                    Short.TryParse(reader("idUnidadNegocio").ToString, Me._idUnidadNegocio)
                    Me._unidadNegocio = reader("unidadNegocio").ToString
                    MyBase.CargarInformacion(reader)
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Overrides Function Actualizar() As General.ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

        Public Overrides Function Registrar() As General.ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

#End Region

    End Class

End Namespace