Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Namespace WMS

    Public Class Bodega
        Inherits BodegaBase

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idBodega As Integer)
            Me.New()
            Me._idBodega = idBodega
            Me.CargarInfoBodega()
        End Sub

        Public Sub New(ByVal codigoInterno As String)
            Me.New()
            Me._codigoInterno = codigoInterno
            Me.CargarInfoBodega()
        End Sub

#End Region

#Region "Métodos Privados"

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            Me.CargarInfoBodega(reader)
        End Sub

        Protected Friend Overloads Function Registrar(ByVal dbManager As LMDataAccess) As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

        Protected Friend Overloads Function Actualizar(ByVal dbManager As LMDataAccess) As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

#End Region

#Region "Métodos Públicos"

        Public Overrides Function Registrar() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim resultado As ResultadoProceso
            Try
                resultado = Registrar(dbManager)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Overrides Function Actualizar() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim resultado As ResultadoProceso
            Try
                resultado = Actualizar(dbManager)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace
