Imports LMDataAccessLayer

Namespace PresupuestoGestiones

    Public Class CorteReportePresupuestoGestiones

#Region "Atributos"

        Private _idCorte As Byte
        Private _descripcion As String
        Private _horaInicial As Date
        Private _horaFinal As Date
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdCorte() As Byte
            Get
                Return _idCorte
            End Get
            Protected Friend Set(ByVal value As Byte)
                _idCorte = value
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

        Public Property HoraInicial() As Date
            Get
                Return _horaInicial
            End Get
            Set(ByVal value As Date)
                _horaInicial = value
            End Set
        End Property

        Public Property HoraFinal() As Date
            Get
                Return _horaFinal
            End Get
            Set(ByVal value As Date)
                _horaFinal = value
            End Set
        End Property

        Public Property Registrtrado As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
        End Sub

        Public Sub New(ByVal identificador As Byte)
            Me.New()
            _idCorte = identificador
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If _idCorte > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idCorte > 0 Then .SqlParametros.Add("@idCorte", SqlDbType.TinyInt).Value = _idCorte
                        .ejecutarReader("ObtenerCorteReportePresupuestoGestiones", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then CargarResultadoConsulta(.Reader)
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Byte.TryParse(reader("idCorte").ToString, _idCorte)
                    _descripcion = reader("descripcion").ToString
                    Date.TryParse(reader("horaInicial").ToString, _horaInicial)
                    Date.TryParse(reader("horaFinal").ToString, _horaFinal)
                    _registrado = True
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace