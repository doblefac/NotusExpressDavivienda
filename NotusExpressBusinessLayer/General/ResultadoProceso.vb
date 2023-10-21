Namespace General

    Public Class ResultadoProceso

#Region "Atributos"

        Private _valor As Long
        Private _mensaje As String
        Private _retorno As Long

#End Region

#Region "Constructores"

        Public Sub New()
            _mensaje = ""
        End Sub

        Public Sub New(ByVal valor As Integer, ByVal mensaje As String)
            Me.New()
            _valor = valor
            _mensaje = mensaje
        End Sub

#End Region

#Region "Propiedades"

        Public Property Valor() As Long
            Get
                Return _valor
            End Get
            Set(ByVal value As Long)
                _valor = value
            End Set
        End Property

        Public Property Retorno() As Long
            Get
                Return _retorno
            End Get
            Set(ByVal value As Long)
                _retorno = value
            End Set
        End Property

        Public Property Mensaje() As String
            Get
                Return _mensaje
            End Get
            Set(ByVal value As String)
                _mensaje = value
            End Set
        End Property



#End Region

#Region "Métodos Públicos"

        Public Sub EstablecerMensajeYValor(ByVal valor As Integer, ByVal mensaje As String)
            _valor = valor
            _mensaje = mensaje
        End Sub

#End Region

    End Class

End Namespace