Imports System.Text
Imports System.String
Imports DevExpress.Web
Imports NotusExpressBusinessLayer

<ValidationPropertyAttribute("value")> _
Public Class SelectorDireccion
    Inherits System.Web.UI.UserControl

#Region "Atributos"

    Private _htDireccion As Hashtable
    Private _dtAdicional As DataTable

#End Region

#Region "Propiedades"

    Public Property HTDireccion() As Hashtable
        Get
            If _htDireccion Is Nothing Then CrearEstructuraTabla()
            Return _htDireccion
        End Get
        Set(ByVal value As Hashtable)
            _htDireccion = value
        End Set
    End Property

    Public Property DireccionEdicion() As String
        Get
            Return hdDireccionEdicion.Value
        End Get
        Set(value As String)
            hdDireccionEdicion.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite que el control pueda ser validado cómo requerido en un RequiredFieldValidator.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property value() As String
        Get
            Return memoDireccion.Text
        End Get
        Set(ByVal value As String)
            memoDireccion.Text = value
        End Set
    End Property

#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("$htDireccion") IsNot Nothing Then
                    _htDireccion = Session("$htDireccion")
                Else
                    CrearEstructuraTabla()
                End If
                CargarDatosAbreviaturas()
                CargarDatosEquivalencias()
                CrearEstructuraTablaAdicionales()
            Else
                memoDireccion.Text = Request.Form(memoDireccion.UniqueID)
                hdDireccionEdicion.Value = Request.Form(hdDireccionEdicion.UniqueID)
                If memoDireccion.Text <> "" Then
                    mmDireccionTmp.Text = ""
                    lblDireccionOriginal.ClientVisible = True
                    lblDireccionOriginal.Text = "Dirección actual: " & memoDireccion.Text.Trim
                Else
                    lblDireccionOriginal.ClientVisible = False
                End If
                If Session("$htDireccion") IsNot Nothing Then
                    _htDireccion = Session("$htDireccion")
                End If
            End If
        Catch : End Try
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If _htDireccion Is Nothing Then CrearEstructuraTabla()

            If cmbNombreVia.SelectedItem IsNot Nothing Then _htDireccion("NombreVia") = cmbNombreVia.SelectedItem.Value
            _htDireccion("NumeroVia") = spNumeroVia.Value
            If cmbLetraVia.SelectedItem IsNot Nothing Then _htDireccion("LetraVia") = cmbLetraVia.SelectedItem.Value
            If cmbBisVia.SelectedItem IsNot Nothing Then _htDireccion("BisVia") = cmbBisVia.SelectedItem.Value
            If cmbOrientacionVia.SelectedItem IsNot Nothing Then _htDireccion("OrientacionVia") = cmbOrientacionVia.SelectedItem.Value
            _htDireccion("NumeroViaSec") = spNumeroViaSec.Value
            If cmbLetraViaSec.SelectedItem IsNot Nothing Then _htDireccion("LetraViaSec") = cmbLetraViaSec.SelectedItem.Value
            If cmbBisViaSec.SelectedItem IsNot Nothing Then _htDireccion("BisViaSec") = cmbBisViaSec.SelectedItem.Value
            _htDireccion("NumeroNomenclatura") = spNumeroNomenclatura.Value
            If cmbOrientacionViaSec.SelectedItem IsNot Nothing Then _htDireccion("OrientacionViaSec") = cmbOrientacionViaSec.SelectedItem.Value

            Session("$htDireccion") = _htDireccion
        Catch : End Try
    End Sub

    Public Sub cpnl_Callback(source As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpnl.Callback
        Dim direccion As String = ""
        Dim resultado As String = ""
        Try
            Dim arrAccion As String()
            arrAccion = e.Parameter.Split(":")
            direccion = arrAccion(0)
            If CType(Session("dtAdicional"), DataTable) Is Nothing Then
                CrearEstructuraTablaAdicionales()
                Session("dtAdicional") = _dtAdicional
            Else
                _dtAdicional = Session("dtAdicional")
            End If
            Dim dt As DataTable = CType(Session("dtEquivalencias"), DataTable)
            If txtTextoAdicional.Text.Trim <> "" Then
                Dim strTmp() As String = txtTextoAdicional.Text.Split(" ")
                If strTmp.Length > 0 Then
                    For i As Integer = 0 To strTmp.Length - 1
                        Dim _drAbreviatura As DataRow = _dtAdicional.NewRow
                        If dt.Select("equivalencia='" & strTmp(i).Trim.ToUpper.ToString & "'").Length > 0 Then
                            Dim drTmp() As DataRow = dt.Select("equivalencia='" & strTmp(i).Trim.ToUpper.ToString & "'")
                            If _dtAdicional Is Nothing OrElse _dtAdicional.Rows.Count = 0 Then
                                _drAbreviatura("abreviatura") = drTmp(0).Item("abreviatura")
                                _dtAdicional.Rows.Add(_drAbreviatura)
                                If txtAdicional.Text.Trim = "" Then
                                    txtAdicional.Text = drTmp(0).Item("abreviatura") & " "
                                Else
                                    txtAdicional.Text = txtAdicional.Text & drTmp(0).Item("abreviatura") & " "
                                End If
                                If direccion.ToLower.Trim = "null" Then
                                    direccion = drTmp(0).Item("abreviatura")
                                Else
                                    direccion = direccion & " " & drTmp(0).Item("abreviatura")
                                End If
                                txtTextoAdicional.Text = ""
                                Session("dtAdicional") = _dtAdicional
                                CType(source, ASPxCallbackPanel).JSProperties("cpDireccion") = direccion
                            Else
                                If _dtAdicional.Select("abreviatura='" & drTmp(0).Item("abreviatura") & "'").Length > 0 Then
                                    resultado = "Dato adicional ya fue insertado en la direccion"
                                    txtTextoAdicional.Text = ""
                                Else
                                    _drAbreviatura("abreviatura") = drTmp(0).Item("abreviatura")
                                    _dtAdicional.Rows.Add(_drAbreviatura)
                                    If txtAdicional.Text.Trim = "" Then
                                        txtAdicional.Text = drTmp(0).Item("abreviatura") & " "
                                    Else
                                        txtAdicional.Text = txtAdicional.Text & drTmp(0).Item("abreviatura") & " "
                                    End If
                                    If direccion.ToLower.Trim = "null" Then
                                        direccion = drTmp(0).Item("abreviatura").ToString.ToUpper
                                    Else
                                        direccion = direccion & " " & drTmp(0).Item("abreviatura").ToString.ToUpper
                                    End If
                                    txtTextoAdicional.Text = ""
                                    Session("dtAdicional") = _dtAdicional
                                    CType(source, ASPxCallbackPanel).JSProperties("cpDireccion") = direccion
                                End If
                            End If
                        Else
                            If txtAdicional.Text.Trim = "" Then
                                txtAdicional.Text = strTmp(i).ToString.Trim.ToUpper & " "
                            Else
                                txtAdicional.Text = txtAdicional.Text & strTmp(i).ToString.Trim.ToUpper & " "
                            End If
                            If direccion.ToLower.Trim = "null" Then
                                direccion = strTmp(i).ToString.Trim.ToUpper
                            Else
                                direccion = direccion & " " & strTmp(i).ToString.Trim.ToUpper
                            End If
                            txtTextoAdicional.Text = ""
                            Session("dtAdicional") = _dtAdicional
                            CType(source, ASPxCallbackPanel).JSProperties("cpDireccion") = direccion
                        End If
                    Next
                End If
            Else
                Dim dr As DataRow = _dtAdicional.NewRow
                If cmbDatosAdicional.SelectedIndex <> -1 Then
                    If _dtAdicional Is Nothing OrElse _dtAdicional.Rows.Count = 0 Then
                        dr("abreviatura") = cmbDatosAdicional.Value
                        _dtAdicional.Rows.Add(dr)
                        If txtAdicional.Text.Trim = "" Then
                            txtAdicional.Text = cmbDatosAdicional.Value & " "
                        Else
                            txtAdicional.Text = txtAdicional.Text & cmbDatosAdicional.Value & " "
                        End If
                        If direccion.ToLower.Trim = "null" Then
                            direccion = cmbDatosAdicional.Value
                        Else
                            direccion = direccion & " " & cmbDatosAdicional.Value
                        End If
                        cmbDatosAdicional.SelectedIndex = -1
                        Session("dtAdicional") = _dtAdicional
                        CType(source, ASPxCallbackPanel).JSProperties("cpDireccion") = direccion
                    Else
                        If _dtAdicional.Select("abreviatura='" & cmbDatosAdicional.Value & "'").Length > 0 Then
                            resultado = "Dato adicional ya fue insertado en la direccion"
                            cmbDatosAdicional.SelectedIndex = -1
                        Else
                            dr("abreviatura") = cmbDatosAdicional.Value
                            _dtAdicional.Rows.Add(dr)
                            If txtAdicional.Text.Trim = "" Then
                                txtAdicional.Text = cmbDatosAdicional.Value & " "
                            Else
                                txtAdicional.Text = txtAdicional.Text & cmbDatosAdicional.Value & " "
                            End If
                            If direccion.ToLower.Trim = "null" Then
                                direccion = cmbDatosAdicional.Value
                            Else
                                direccion = direccion & " " & cmbDatosAdicional.Value
                            End If
                            cmbDatosAdicional.SelectedIndex = -1
                            Session("dtAdicional") = _dtAdicional
                            CType(source, ASPxCallbackPanel).JSProperties("cpDireccion") = direccion
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            resultado = "Se generó un error al tratar de obtener los datos de acuerdo a la palabra clave digitada: " & ex.Message
        End Try
        CType(source, ASPxCallbackPanel).JSProperties("cpResultado") = resultado
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CrearEstructuraTabla()
        Try
            _htDireccion = New Hashtable
            With _htDireccion
                .Add("NombreVia", String.Empty)
                .Add("NumeroVia", String.Empty)
                .Add("LetraVia", String.Empty)
                .Add("BisVia", String.Empty)
                .Add("OrientacionVia", String.Empty)
                .Add("NumeroViaSec", String.Empty)
                .Add("LetraViaSec", String.Empty)
                .Add("BisViaSec", String.Empty)
                .Add("NumeroNomenclatura", String.Empty)
                .Add("OrientacionViaSec", String.Empty)
            End With
        Catch : End Try
    End Sub

    Private Sub CargarDatosAbreviaturas()
        Try
            Dim infoAbreviatura As New AbreviaturaDireccionaColeccion
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            With infoAbreviatura
                .IdEstado = 1
                .jerarquia = 1
                dt1 = .GenerarDataTable()
            End With

            With cmbNombreVia
                .DataSource = dt1
                .TextField = "combinacion"
                .ValueField = "abreviatura"
                .DataBind()
                .SelectedIndex = -1
            End With

            With infoAbreviatura
                .IdEstado = 1
                .jerarquia = 2
                dt2 = .GenerarDataTable()
            End With

            With cmbDatosAdicional
                .DataSource = dt2
                .TextField = "combinacion"
                .ValueField = "abreviatura"
                .DataBind()
                .SelectedIndex = -1
            End With
        Catch : End Try
    End Sub

    Private Sub CargarDatosEquivalencias()
        Try
            Dim infoEquivalencia As New EquivalenciasDireccionColeccion
            Dim dtDatos As New DataTable
            With infoEquivalencia
                .IdEstado = 1
                dtDatos = .GenerarDataTable()
            End With
            Session("dtEquivalencias") = dtDatos
        Catch : End Try
    End Sub

    Private Sub CrearEstructuraTablaAdicionales()
        Try
            _dtAdicional = New DataTable
            With _dtAdicional
                .Columns.Add("abreviatura", GetType(String))
                .AcceptChanges()
            End With
        Catch : End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Sub Limpiar()
        Try
            _htDireccion = Nothing
            cmbNombreVia.Value = Nothing
            spNumeroVia.Value = 0
            cmbLetraVia.Value = Nothing
            cmbBisVia.Value = Nothing
            cmbOrientacionVia.Value = Nothing
            spNumeroViaSec.Value = 0
            cmbLetraViaSec.Value = Nothing
            cmbBisViaSec.Value = Nothing
            spNumeroNomenclatura.Value = 0
            cmbOrientacionViaSec.Value = Nothing
        Catch : End Try
    End Sub

#End Region

End Class