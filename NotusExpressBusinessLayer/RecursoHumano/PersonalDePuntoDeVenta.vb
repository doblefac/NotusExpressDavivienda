Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer

Namespace RecursoHumano

    Public Class PersonalDePuntoDeVenta

#Region "Atributos"

        Private _idPersonal As Integer
        Private _idPdv As Integer
        Private _pdv As String
        Private _idPersona As Integer
        Private _nombreApellido As String
        Private _fechaCreacion As Date
        Private _idCreador As Integer
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            _pdv = ""
            _nombreApellido = ""
        End Sub

        Public Sub New(identificador As Integer)
            _idPersonal = identificador
            CargarDatos()
        End Sub

        Public Sub New(idPersona As Integer, idPdv As Integer)
            _idPersona = idPersona
            _idPdv = idPdv
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()

        End Sub

#End Region

    End Class

End Namespace