Option Explicit On
Option Strict On

Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.IO
Imports iTextSharp.text.pdf
Imports System.Xml
Imports LMDataAccessLayer
Imports System.Web


Public Class DescargarSolicitudCreditoPersonaNatural
    Inherits System.Web.UI.Page

#Region "Atributos"
    Private _nombreArchivo As String
    Private _idServicio As String
#End Region

#Region "Propiedades"

    Public Property NombreArchivo As String
        Get
            Return _nombreArchivo
        End Get
        Set(value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public Property IdServicio As String
        Get
            Return _idServicio
        End Get
        Set(value As String)
            _idServicio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    'Public Function GenerarYDescargarSolicitudCredito(ByVal identificacionCliente As Integer) As Boolean
    '    Try
    '        Dim dtDatosVenta As New DataSet
    '        dtDatosVenta = ConsultarInformacionPrestamoCliente(identificacionCliente)

    '        If (dtDatosVenta.Rows.Count > 0) Then
    '            Dim rutaPlantilla As String = Server.MapPath("~/GestionComercial/Archivos/FormatoSolicitudCreditoPN.pdf") '"D:\FormatoSolicitudCreditoPN.pdf"
    '            Dim rutaXml As String = Server.MapPath("~/GestionComercial/Archivos/PlantillaXml.xml") '"D:\PlantillaXml.xml"
    '            Dim rutaArchivoDestino As String = Server.MapPath("~/GestionComercial/Archivos/FormatoSolicitudCreditoPNFIN_" & identificacionCliente & ".pdf")
    '            Dim inputPdfStream As Stream = New FileStream(rutaPlantilla, FileMode.Open, FileAccess.Read, FileShare.Read)
    '            Dim outputPdfStream As Stream = New FileStream(rutaArchivoDestino, FileMode.Create, FileAccess.ReadWrite)
    '            Dim pdfReader As New PdfReader(inputPdfStream)
    '            Dim pdfStamper As New PdfStamper(pdfReader, outputPdfStream)
    '            Dim form As AcroFields = pdfStamper.AcroFields
    '            Dim xfa As XfaForm = form.Xfa

    '            Dim xmlDoc As XmlDocument = New XmlDocument
    '            xmlDoc.LoadXml(form.Xfa.DatasetsNode.FirstChild.OuterXml)

    '            Dim xmlDil As XmlNode = xmlDoc.DocumentElement.SelectSingleNode("form1/fechadiligen")
    '            If xmlDil IsNot Nothing Then
    '                xmlDil.ChildNodes(7).InnerText = (Date.Now).ToShortDateString       'fecha
    '                xmlDil.ChildNodes(0).InnerText = dtDatosVenta.Rows(0)(0).ToString() 'Ciudad
    '                xmlDil.ChildNodes(1).InnerText = dtDatosVenta.Rows(0)(1).ToString() 'codigoOficina
    '                xmlDil.ChildNodes(4).InnerText = dtDatosVenta.Rows(0)(2).ToString() 'nombreOficina
    '                xmlDil.ChildNodes(2).InnerText = dtDatosVenta.Rows(0)(3).ToString() 'codigoSucursal
    '                xmlDil.ChildNodes(3).InnerText = dtDatosVenta.Rows(0)(23).ToString() 'CodEstrategia
    '                'xmlDil.ChildNodes(6).InnerText = 'CodigoConvenio
    '                'xmlDil.ChildNodes(5).InnerText = 'NombreConvenio
    '                xmlDil.ChildNodes(8).InnerText = dtDatosVenta.Rows(0)(4).ToString() 'CodEstrategiaCliente
    '                xmlDil.ChildNodes(9).InnerText = dtDatosVenta.Rows(0)(5).ToString() 'CodAgenteVendedor
    '            End If

    '            Dim xmlCred As XmlNode = xmlDoc.DocumentElement.SelectSingleNode("form1/creditodeconsumo")
    '            If xmlCred IsNot Nothing Then
    '                'xmlCred.ChildNodes(0).InnerText = "15" 'campoNoExiste
    '                'xmlCred.ChildNodes(1).InnerText = "456" 'No.CuentaDesembolso
    '                'xmlCred.ChildNodes(2).InnerText = "36" 'PlazoEnMeses
    '                'xmlCred.ChildNodes(3).InnerText = "58585" 'CupoSolicitado

    '                'xmlCred.ChildNodes(4).InnerText = "1" 'CheckFoto: No
    '                If dtDatosVenta.Rows.Count > 1 Then
    '                    Dim cont As Integer = 0
    '                    Dim temp As String = String.Empty
    '                    For Each dr As DataRow In dtDatosVenta.Rows
    '                        temp += dtDatosVenta.Rows(cont)(7).ToString() & ", "
    '                        xmlCred.ChildNodes(5).InnerText = temp 'otroCual?
    '                        cont += +1
    '                    Next
    '                Else
    '                    xmlCred.ChildNodes(5).InnerText = dtDatosVenta.Rows(0)(7).ToString() 'otroCual?
    '                End If

    '                xmlCred.ChildNodes(6).InnerText = dtDatosVenta.Rows(0)(8).ToString()

    '                'xmlCred.ChildNodes(7).InnerText = "347"
    '                'xmlCred.ChildNodes(8).InnerText = "III"
    '                'xmlCred.ChildNodes(9).InnerText = "456"
    '                'xmlCred.ChildNodes(10).InnerText = "6544"
    '            End If

    '            Dim xmlDP As XmlNode = xmlDoc.DocumentElement.SelectSingleNode("form1/infobasicaPN")
    '            If xmlDP IsNot Nothing Then

    '                xmlDP.ChildNodes(0).InnerText = dtDatosVenta.Rows(0)(9).ToString() 'nombres
    '                xmlDP.ChildNodes(1).InnerText = dtDatosVenta.Rows(0)(10).ToString() 'primerApellido
    '                xmlDP.ChildNodes(2).InnerText = dtDatosVenta.Rows(0)(11).ToString() 'segundoApellido
    '                xmlDP.ChildNodes(3).InnerText = dtDatosVenta.Rows(0)(13).ToString() 'fechaNacimiento
    '                'xmlDP.ChildNodes(4).InnerText = 'ciudadNacimiento
    '                'xmlDP.ChildNodes(5).InnerText = "FFF" 'ciudadExpedicionCedula
    '                'xmlDP.ChildNodes(6).InnerText = "04/08/2017" 'fechaExpedicionCedula
    '                xmlDP.ChildNodes(7).InnerText = dtDatosVenta.Rows(0)(27).ToString() 'estadoCivil: 1-7
    '                xmlDP.ChildNodes(8).InnerText = dtDatosVenta.Rows(0)(16).ToString() 'profesion
    '                'xmlDP.ChildNodes(9).InnerText = "3" 'no.PersonasCargo
    '                xmlDP.ChildNodes(10).InnerText = dtDatosVenta.Rows(0)(25).ToString() 'tipoVivienda
    '                xmlDP.ChildNodes(11).InnerText = dtDatosVenta.Rows(0)(26).ToString() 'sexo
    '                xmlDP.ChildNodes(12).InnerText = dtDatosVenta.Rows(0)(14).ToString() 'tipoIdentificaicon

    '            End If

    '            Dim xmlLoc As XmlNode = xmlDoc.DocumentElement.SelectSingleNode("form1/localizacion")
    '            If xmlLoc IsNot Nothing Then

    '                'Localizacion
    '                xmlLoc.ChildNodes(13).InnerText = dtDatosVenta.Rows(0)(17).ToString() 'direccionResidencia
    '                xmlLoc.ChildNodes(14).InnerText = dtDatosVenta.Rows(0)(0).ToString() 'ciudadResidencia
    '                xmlLoc.ChildNodes(15).InnerText = dtDatosVenta.Rows(0)(18).ToString() 'telefonoResidencia
    '                xmlLoc.ChildNodes(16).InnerText = dtDatosVenta.Rows(0)(19).ToString() 'direccionOficina
    '                xmlLoc.ChildNodes(17).InnerText = dtDatosVenta.Rows(0)(0).ToString() 'ciudadOficina
    '                xmlLoc.ChildNodes(18).InnerText = dtDatosVenta.Rows(0)(20).ToString() 'telefonoOficina
    '                'xmlLoc.ChildNodes(19).InnerText = 'extensionTelefonoOficina
    '                xmlLoc.ChildNodes(20).InnerText = dtDatosVenta.Rows(0)(21).ToString() 'email
    '                xmlLoc.ChildNodes(21).InnerText = dtDatosVenta.Rows(0)(22).ToString() 'celular
    '                ' xmlLoc.ChildNodes(22).InnerText = "2222" 'No existe

    '                'Actividad Laboral
    '                xmlLoc.ChildNodes(0).InnerText = dtDatosVenta.Rows(0)(40).ToString() ' TipoActividadlaboral: Empleado, Independiente, Rentista de Capital
    '                xmlLoc.ChildNodes(2).InnerText = dtDatosVenta.Rows(0)(28).ToString()  'ActividadLaboral:   NombreEntidad
    '                xmlLoc.ChildNodes(1).InnerText = dtDatosVenta.Rows(0)(29).ToString()  'ActividadLaboral:   ActividadEconomica
    '                xmlLoc.ChildNodes(4).InnerText = dtDatosVenta.Rows(0)(30).ToString()  'ActividadLaboral:   Cargo
    '                xmlLoc.ChildNodes(3).InnerText = dtDatosVenta.Rows(0)(31).ToString()  'ActividadLaboral:   Nit
    '                xmlLoc.ChildNodes(5).InnerText = dtDatosVenta.Rows(0)(32).ToString()  'ActividadLaboral:   AñosActividadLaboral
    '                xmlLoc.ChildNodes(9).InnerText = dtDatosVenta.Rows(0)(33).ToString()  'ActividadLaboral:   FechaIngreso
    '                xmlLoc.ChildNodes(6).InnerText = dtDatosVenta.Rows(0)(34).ToString()  'ActividadLaboral:   TipoContrato
    '                xmlLoc.ChildNodes(8).InnerText = dtDatosVenta.Rows(0)(35).ToString()  'ActividadLaboral:   Independiente: Ocupacion
    '                xmlLoc.ChildNodes(7).InnerText = dtDatosVenta.Rows(0)(36).ToString()  'ActividadLaboral:   Independiente: NombreEmpresa
    '                xmlLoc.ChildNodes(10).InnerText = dtDatosVenta.Rows(0)(37).ToString() 'ActividadLaboral:   Rentista Cap: Nit
    '                xmlLoc.ChildNodes(11).InnerText = dtDatosVenta.Rows(0)(38).ToString() 'ActividadLaboral:   Independiente: fechaConstitucion
    '                xmlLoc.ChildNodes(12).InnerText = dtDatosVenta.Rows(0)(39).ToString() 'ActividadLaboral:   Independiente: ActividadEconomica

    '            End If

    '            Dim xmlFin As XmlNode = xmlDoc.DocumentElement.SelectSingleNode("form1/infofinanciera")
    '            If xmlFin IsNot Nothing Then

    '                'Informacion Financiera Ingresos
    '                xmlFin.ChildNodes(75).InnerText = dtDatosVenta.Rows(0)(42).ToString() 'Honorarios
    '                xmlFin.ChildNodes(73).InnerText = dtDatosVenta.Rows(0)(44).ToString() 'Total Ingresos

    '                'Informacion Financiera Egresos
    '                xmlFin.ChildNodes(80).InnerText = dtDatosVenta.Rows(0)(45).ToString() 'Arriendo
    '                xmlFin.ChildNodes(79).InnerText = dtDatosVenta.Rows(0)(46).ToString() 'GastosFamiliares
    '                xmlFin.ChildNodes(78).InnerText = dtDatosVenta.Rows(0)(47).ToString() 'TotalCuotaCreditos
    '                xmlFin.ChildNodes(77).InnerText = dtDatosVenta.Rows(0)(48).ToString() 'TotalEgresos

    '                xmlFin.ChildNodes(62).InnerText = "1"

    '                'Activos y Pasivos
    '                xmlFin.ChildNodes(42).InnerText = dtDatosVenta.Rows(0)(49).ToString() 'Otros activos: Descripcion
    '                xmlFin.ChildNodes(41).InnerText = dtDatosVenta.Rows(0)(50).ToString() 'Otros activos: Valor
    '                xmlFin.ChildNodes(40).InnerText = dtDatosVenta.Rows(0)(51).ToString() 'Otros activos: Total Activos
    '                xmlFin.ChildNodes(44).InnerText = dtDatosVenta.Rows(0)(52).ToString() 'Otros pasivos: Descripcion
    '                xmlFin.ChildNodes(43).InnerText = dtDatosVenta.Rows(0)(53).ToString() 'Otros pasivos: Valor

    '                'Referencias
    '                xmlFin.ChildNodes(27).InnerText = dtDatosVenta.Rows(0)(55).ToString() 'Familiar
    '                xmlFin.ChildNodes(39).InnerText = dtDatosVenta.Rows(0)(56).ToString() 'Parentesco
    '                xmlFin.ChildNodes(30).InnerText = dtDatosVenta.Rows(0)(57).ToString() 'Teléfono
    '                xmlFin.ChildNodes(36).InnerText = dtDatosVenta.Rows(0)(58).ToString() 'Dirección 
    '                xmlFin.ChildNodes(33).InnerText = dtDatosVenta.Rows(0)(59).ToString() 'Ciudad
    '                xmlFin.ChildNodes(28).InnerText = dtDatosVenta.Rows(0)(60).ToString() 'Personal
    '                xmlFin.ChildNodes(31).InnerText = dtDatosVenta.Rows(0)(61).ToString() 'Teléfono
    '                xmlFin.ChildNodes(37).InnerText = dtDatosVenta.Rows(0)(62).ToString() 'Dirección
    '                xmlFin.ChildNodes(34).InnerText = dtDatosVenta.Rows(0)(63).ToString() 'Ciudad
    '                xmlFin.ChildNodes(29).InnerText = dtDatosVenta.Rows(0)(64).ToString() 'Comercial
    '                xmlFin.ChildNodes(32).InnerText = dtDatosVenta.Rows(0)(65).ToString() 'Teléfono
    '                xmlFin.ChildNodes(38).InnerText = dtDatosVenta.Rows(0)(66).ToString() 'Dirección
    '                xmlFin.ChildNodes(35).InnerText = dtDatosVenta.Rows(0)(67).ToString() 'Ciudad

    '                'Finca Raíz
    '                'xmlFin.ChildNodes(61).InnerText = "11"
    '                'xmlFin.ChildNodes(62).InnerText = "22"
    '                'xmlFin.ChildNodes(63).InnerText = "33"
    '                'xmlFin.ChildNodes(64).InnerText = "44"
    '                'xmlFin.ChildNodes(65).InnerText = "55"
    '                'xmlFin.ChildNodes(66).InnerText = "66"
    '                'xmlFin.ChildNodes(67).InnerText = "77"
    '                'xmlFin.ChildNodes(68).InnerText = "88"
    '                'xmlFin.ChildNodes(28).InnerText = "99"
    '                'xmlFin.ChildNodes(29).InnerText = "1010"
    '                'xmlFin.ChildNodes(30).InnerText = "11"
    '                'xmlFin.ChildNodes(31).InnerText = "1212"
    '                'xmlFin.ChildNodes(32).InnerText = "1313"
    '                'xmlFin.ChildNodes(33).InnerText = "1414"
    '                'xmlFin.ChildNodes(27).InnerText = "1515"
    '                'xmlFin.ChildNodes(70).InnerText = "1616"
    '                'xmlFin.ChildNodes(71).InnerText = "1717"

    '            End If

    '            'Dim xmlAut As XmlNode = xmlDoc.DocumentElement.SelectSingleNode("form1/Autorizacionesydeclaraciones")
    '            'If xmlAut IsNot Nothing Then
    '            '    xmlAut.ChildNodes(4).InnerText = "111"
    '            '    '    xmlAut.ChildNodes(4).InnerText = dtDatos.Rows(vf_num_registro)("origenFondos").ToString
    '            'End If

    '            Dim xmlIdf As XmlNode = xmlDoc.DocumentElement.SelectSingleNode("form1")
    '            If xmlIdf IsNot Nothing Then
    '                xmlIdf.ChildNodes(17).InnerText = dtDatosVenta.Rows(0)(15).ToString() 'NumeroIdentificacionInformacionBasica
    '                '    xmlIdf.ChildNodes(17).InnerText = dtDatos.Rows(vf_num_registro)("identificacion").ToString
    '            End If

    '            xmlDoc.Save(rutaXml)

    '            Dim xmlReader As XmlReader = XmlReader.Create(rutaXml)
    '            xfa.FillXfaForm(xmlReader)

    '            pdfStamper.Close()
    '            System.Diagnostics.Process.Start(rutaArchivoDestino)
    '        End If


    '    Catch ex As Exception
    '        vg_mensaje = "Error Diligenciado Fomulario, Registro No. " & 22222222222.ToString() & " - " & ex.Message.ToString()
    '        Return False
    '    End Try

    '    Return True
    'End Function


    Public Function ConsultarInformacionPrestamoCliente(idGestionVenta As Integer) As DataSet

        Dim dbManager As New LMDataAccess
        Dim dtDatosCodigoEstrategia As New DataSet
        With dbManager
            .SqlParametros.Add("@idGestionVenta", SqlDbType.Int).Value = idGestionVenta
            dtDatosCodigoEstrategia = .EjecutarDataSet("ConsultaSolicitudCreditoPN", CommandType.StoredProcedure)
        End With
        Return dtDatosCodigoEstrategia
    End Function

#End Region

End Class
