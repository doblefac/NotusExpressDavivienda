Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer

Namespace WMS

    Public MustInherit Class BodegaBase

#Region "Atributos"

        Protected _idBodega As Integer
        Protected _codigoInterno As String
        Protected _nombreBodega As String
        Protected _idTipo As Short
        Protected _idCiudad As Integer
        Protected _ciudad As String
        Protected _idEstado As Byte
        Protected _idEmpresa As Short
        Protected _empresa As String
        Protected _idUnidadNegocio As Short
        Protected _unidadNegocio As String
        Protected _codigoCentro As String
        Protected _codigoAlmacen As String
        Protected _idPadre As Integer
        Protected _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            _codigoInterno = ""
            _nombreBodega = ""
            _ciudad = ""
            _empresa = ""
            _unidadNegocio = ""
            _codigoCentro = ""
            _codigoAlmacen = ""

        End Sub

#End Region

#Region "Propiedades"

        Public Property IdBodega() As Integer
            Get
                Return _idBodega
            End Get
            Set(ByVal value As Integer)
                _idBodega = value
            End Set
        End Property

        Public Property CodigoInterno() As String
            Get
                Return _codigoInterno
            End Get
            Set(ByVal value As String)
                _codigoInterno = value
            End Set
        End Property

        Public Property NombreBodega() As String
            Get
                Return _nombreBodega
            End Get
            Set(ByVal value As String)
                _nombreBodega = value
            End Set
        End Property

        Public Property IdTipo() As Short
            Get
                Return _idTipo
            End Get
            Set(ByVal value As Short)
                _idTipo = value
            End Set
        End Property

        Public Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
            Set(ByVal value As Integer)
                _idCiudad = value
            End Set
        End Property

        Public Property Ciudad() As String
            Get
                Return _ciudad
            End Get
            Protected Friend Set(ByVal value As String)
                _ciudad = value
            End Set
        End Property

        Public Property IdEstado() As Byte
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Byte)
                _idEstado = value
            End Set
        End Property

        Public Property IdEmpresa() As Short
            Get
                Return _idEmpresa
            End Get
            Set(ByVal value As Short)
                _idEmpresa = value
            End Set
        End Property

        Public Property Empresa() As String
            Get
                Return _empresa
            End Get
            Protected Friend Set(ByVal value As String)
                _empresa = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As Short
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Short)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property UnidadNegocio() As String
            Get
                Return _unidadNegocio
            End Get
            Protected Friend Set(ByVal value As String)
                _unidadNegocio = value
            End Set
        End Property

        Public Property CodigoCentro() As String
            Get
                Return _codigoCentro
            End Get
            Set(ByVal value As String)
                _codigoCentro = value
            End Set
        End Property

        Public Property CodigoAlmacen() As String
            Get
                Return _codigoAlmacen
            End Get
            Set(ByVal value As String)
                _codigoAlmacen = value
            End Set
        End Property

        Public Property IdPadre() As Integer
            Get
                Return _idPadre
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idPadre = value
            End Set
        End Property

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Métodos Portegidos"

        Protected Sub CargarInfoBodega()
            If Me._idBodega > 0 OrElse Not String.IsNullOrEmpty(Me._codigoInterno) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = Me._idBodega
                        If Not String.IsNullOrEmpty(_codigoInterno) Then _
                            .SqlParametros.Add("@codigoInterno", SqlDbType.VarChar, 20).Value = Me._codigoInterno
                        .ejecutarReader("ObtenerInfoBodega", CommandType.StoredProcedure)
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

        Protected Sub CargarInfoBodega(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                AsignarValorAPropiedades(reader)
            End If
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idBodega").ToString, Me._idBodega)
                    Me._codigoInterno = reader("codigoInterno").ToString
                    Me._nombreBodega = reader("nombre").ToString
                    Short.TryParse(reader("idTipo").ToString, Me._idTipo)
                    Integer.TryParse(reader("idCiudad").ToString, Me._idCiudad)
                    Me._ciudad = reader("ciudad").ToString
                    Byte.TryParse(reader("idEstado").ToString, Me._idEstado)
                    Short.TryParse(reader("idEmpresa").ToString, Me._idEmpresa)
                    Me._empresa = reader("empresa").ToString
                    Short.TryParse(reader("idUnidadNegocio").ToString, Me._idUnidadNegocio)
                    Me._codigoCentro = reader("codigoCentro").ToString
                    Me._codigoAlmacen = reader("codigoAlmacen").ToString
                    Integer.TryParse(reader("idPadre").ToString, Me._idPadre)
                    Me._registrado = True
                End If
            End If
        End Sub
#End Region

#Region "Métodos Públicos"

        Public MustOverride Function Registrar() As ResultadoProceso

        Public MustOverride Function Actualizar() As ResultadoProceso
#End Region

    End Class

End Namespace