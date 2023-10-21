Imports System.Web.Services
Imports System.ComponentModel
Imports System.Web.Services.Protocols
Imports NotusExpressBusinessLayer
Imports NotusExpressBusinessLayer.General


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://www.logytechmobile.com/NotusExpressWebService/", Description:="WebService que provee los procedimientos y funciones necesarios para llevar a cabo la sincronización de información de BancaSegurosCEM", name:="NotusExpressService")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class NotusExpressService
    Inherits System.Web.Services.WebService

#Region "Métodos Públicos"

    <WebMethod(Description:="Función que permite modificar una gestión de venta")> _
    Public Function ActualizaGestionVenta(ByVal infoGestion As WsGestionVenta) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        resultado = ActualizaGestion(infoGestion)
        Return resultado
    End Function

    <WebMethod(Description:="Función que permite registrar los seriales de una gestión de venta")> _
    Public Function RegistraSerialGestionVenta(ByVal idServicio As Integer, ByVal dsDatos As DataSet) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        resultado = RegistraSerial(idServicio, dsDatos)
        Return resultado
    End Function

    <WebMethod(Description:="Función que permite registrar una campania")> _
    Public Function RegistraCampania(ByVal infoCampania As WsRegistroCampania) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        resultado = RegistrarCampania(infoCampania)
        Return resultado
    End Function

    <WebMethod(Description:="Función que permite actualizar una campania")> _
    Public Function ActualizaCampania(ByVal infoCampania As WsRegistroCampania) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        resultado = ActualizarCampania(infoCampania)
        Return resultado
    End Function

#End Region

#Region "Métodos Privados"

    Private Function ActualizaGestion(ByVal infoGestion As WsGestionVenta) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim objGestion As New GestionComercial.GestionDeVenta
        With objGestion
            If infoGestion.IdGestionVenta > 0 Then
                .IdGestionVenta = infoGestion.IdGestionVenta
            Else
                Dim miGestion As New GestionComercial.GestionDeVenta(idServicioNotus:=infoGestion.IdServicioNotus)
                .IdGestionVenta = miGestion.IdGestionVenta
            End If
            If infoGestion.IdCliente > 0 Then .IdCliente = infoGestion.IdCliente
            If infoGestion.FechaAgenda > Date.MinValue Then .FechaAgenda = infoGestion.FechaAgenda
            If infoGestion.IdResultadoProceso > 0 Then .IdResultadoProceso = infoGestion.IdResultadoProceso
            If infoGestion.IdEstado > 0 Then .IdEstado = infoGestion.IdEstado
            If Not String.IsNullOrEmpty(infoGestion.ObservacionCallCenter) Then .ObservacionCallCenter = infoGestion.ObservacionCallCenter
            If Not String.IsNullOrEmpty(infoGestion.ObservacionVendedor) Then .ObservacionVendedor = infoGestion.ObservacionVendedor
            If infoGestion.FechaRecepcionDocumentos > Date.MinValue Then .FechaRecepcionDocumentos = infoGestion.FechaRecepcionDocumentos
            If infoGestion.IdReceptorDocumento > 0 Then .IdReceptorDocumento = infoGestion.IdReceptorDocumento
            If infoGestion.FechaLegalizacion > Date.MinValue Then .FechaLegalizacion = infoGestion.FechaLegalizacion
            If infoGestion.IdLegalizador > 0 Then .IdLegalizador = infoGestion.IdLegalizador
            If Not String.IsNullOrEmpty(infoGestion.ObservacionDeclinar) Then .ObservacionDeclinar = infoGestion.ObservacionDeclinar
            If infoGestion.IdNovedad > 0 Then .IdNovedad = infoGestion.IdNovedad
            If Not String.IsNullOrEmpty(infoGestion.ObservacionNovedad) Then .ObservacionNovedad = infoGestion.ObservacionNovedad
            If infoGestion.IdCampaniaNotus > 0 Then .IdCampaniaNotus = infoGestion.IdCampaniaNotus
            If infoGestion.IdServicioNotus > 0 Then .IdServicioNotus = infoGestion.IdServicioNotus
            If infoGestion.IdModificador > 0 Then .IdModificador = infoGestion.IdModificador
            If infoGestion.IdEstadoServicioMensajeria > 0 Then .IdEstadoServicioMensajeria = infoGestion.IdEstadoServicioMensajeria

            resultado = .Actualizar()
        End With
        Return resultado
    End Function

    Private Function RegistraSerial(ByVal idServicio As Integer, ByVal dsDatos As DataSet) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim objSerial As New GestionVentaRegistraSerial
        Dim esValido As Boolean
        With objSerial
            .EstructuraTabla = dsDatos.Tables(0)
            esValido = .CargarInformacion(idServicio)
            If esValido Then
                resultado = .RegistrarSerialesServicio(idServicio)
            Else
                resultado.EstablecerMensajeYValor(1, "No se logró registrar los seriales asociados al servicio.")
            End If
        End With
        Return resultado
    End Function

    Private Function RegistrarCampania(ByVal infoCampania As WsRegistroCampania) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miCampania As New Campania
        With miCampania
            .Nombre = infoCampania.Nombre
            .FechaInicio = infoCampania.FechaInicio
            .FechaFin = infoCampania.FechaFin
            .Activo = infoCampania.Activo
            .ListTiposDeServicio = infoCampania.ListTiposDeServicio
            .ListBodegas = infoCampania.ListBodegas
            .ListProductoExterno = infoCampania.ListProductoExterno
            .ListDocumentoFinanciero = infoCampania.ListDocumentoFinanciero
            .IdClienteExterno = infoCampania.IdClienteExterno
            .IdTipoCampania = infoCampania.IdTipoCampania
            .IdSistema = infoCampania.IdSistema
            resultado = .RegistrarFinanciero()
        End With
        Return resultado
    End Function

    Private Function ActualizarCampania(ByVal infoCampania As WsRegistroCampania) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim miCampania As New Campania
        With miCampania
            .IdCampania = infoCampania.IdCampania
            .Nombre = infoCampania.Nombre
            .FechaInicio = infoCampania.FechaInicio
            .FechaFin = infoCampania.FechaFin
            .Activo = infoCampania.Activo
            .ListTiposDeServicio = infoCampania.ListTiposDeServicio
            .ListBodegas = infoCampania.ListBodegas
            .ListProductoExterno = infoCampania.ListProductoExterno
            .ListDocumentoFinanciero = infoCampania.ListDocumentoFinanciero
            .IdClienteExterno = infoCampania.IdClienteExterno
            .IdTipoCampania = infoCampania.IdTipoCampania
            .IdSistema = infoCampania.IdSistema
            resultado = .ActualizarFinanciero()
        End With
        Return resultado
    End Function

#End Region

End Class