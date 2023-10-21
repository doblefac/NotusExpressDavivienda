Imports NotusExpressBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports NotusExpressBusinessLayer.Enumerados

Namespace GestionComercial
    Public Class GestionDeVentaDevolucionesCall
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _idDevolucionCallCenter As Integer
        Private _idGestionVenta As Integer
        Private _fechaRegistro As Date
        Private _usuarioRegistra As String
        Private _observacion As String
        Private _cargado As Boolean


#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As GestionComercial.GestionDeVenta
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As GestionComercial.GestionDeVenta)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property FechaRegistro As Date
            Get
                Return _fechaRegistro
            End Get
            Set(value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdGestionVenta As Integer
            Get
                Return _idGestionVenta
            End Get
            Set(value As Integer)
                _idGestionVenta = value
            End Set
        End Property

        Public Property IdDevolucionCallCenter As Integer
            Get
                Return _idDevolucionCallCenter
            End Get
            Set(value As Integer)
                _idDevolucionCallCenter = value
            End Set
        End Property

        Public Property UsuarioRegistra As String
            Get
                Return _usuarioRegistra
            End Get
            Set(value As String)
                _usuarioRegistra = value
            End Set
        End Property

        Public Property Observacion As String
            Get
                Return _observacion
            End Get
            Set(value As String)
                _observacion = value
            End Set
        End Property


#End Region


#Region "Métodos Públicos"

        Public Function ConsultaDevolucionesCall() As DataTable
            Dim dtDatos As DataTable

            Dim _dbManager As New LMDataAccess

            With _dbManager
                If _idGestionVenta > 0 Then .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Value = _idGestionVenta
                .TiempoEsperaComando = 0
                dtDatos = .EjecutarDataTable("ObtenerInfoDevolucionCallCenter", CommandType.StoredProcedure)
            End With
            _dbManager.Dispose()
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace
