Imports LMDataAccessLayer
Imports System.Web

Namespace ConfiguracionComercial

    Public MustInherit Class MetaComercialBase

#Region "Atributos"

        Protected _idMeta As Integer
        Protected _idCampania As Short
        Protected _campania As String
        Protected _idEstrategia As Short
        Protected _estrategia As String
        Protected _idPdvOAsesor As Integer
        Protected _puntoOAsesor As String
        Protected _idTipoProducto As Short
        Protected _tipoProducto As String
        Protected _anio As Short
        Protected _mes As Byte
        Protected _meta As Integer
        Protected _fechaRegistro As Date
        Protected _idUsuarioRegistra As Integer
        Protected _estado As Boolean = True
        Protected _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdMeta As Integer
            Get
                Return _idMeta
            End Get
            Set(ByVal value As Integer)
                _idMeta = value
            End Set
        End Property

        Public Property IdCampania As Short
            Get
                Return _idCampania
            End Get
            Set(ByVal value As Short)
                _idCampania = value
            End Set
        End Property

        Public Property Campania As String
            Get
                Return _campania
            End Get
            Protected Friend Set(value As String)
                _campania = value
            End Set
        End Property

        Public Property IdEstrategia As Short
            Get
                Return _idEstrategia
            End Get
            Set(value As Short)
                _idEstrategia = value
            End Set
        End Property

        Public Property Estrategia As String
            Get
                Return _estrategia
            End Get
            Set(value As String)
                _estrategia = value
            End Set
        End Property

        Public Property IdTipoProducto As Short
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Short)
                _idTipoProducto = value
            End Set
        End Property

        Public Property TipoProducto As String
            Get
                Return _tipoProducto
            End Get
            Protected Friend Set(value As String)
                _tipoProducto = value
            End Set
        End Property

        Public Property Anio As Short
            Get
                Return _anio
            End Get
            Set(ByVal value As Short)
                _anio = value
            End Set
        End Property

        Public Property Mes As Byte
            Get
                Return _mes
            End Get
            Set(ByVal value As Byte)
                _mes = value
            End Set
        End Property

        Public Property Meta As Integer
            Get
                Return _meta
            End Get
            Set(ByVal value As Integer)
                _meta = value
            End Set
        End Property

        Public Property FechaRegistro As Date
            Get
                Return _fechaRegistro
            End Get
            Set(ByVal value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdUsuarioRegistra As Integer
            Get
                Return _idUsuarioRegistra
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRegistra = value
            End Set
        End Property

        Public Property Estado As Boolean
            Get
                Return _estado
            End Get
            Set(value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(value As Boolean)
                _registrado = value
            End Set
        End Property
#End Region

#Region "Métodos Privados"

        Protected MustOverride Sub CargarInformacion()

#End Region

#Region "Métodos Portegidos"

        Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader, _
                                                    Optional ByVal nombreColumnaEspecial1 As String = "idPdv", _
                                                    Optional ByVal nombreColumnaEspecial2 As String = "puntoDeVenta")
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idMeta").ToString, Me._idMeta)
                    Short.TryParse(reader("idCampania").ToString, Me._idCampania)
                    Me._campania = reader("campania").ToString
                    Integer.TryParse(reader(nombreColumnaEspecial1).ToString, Me._idPdvOAsesor)
                    Me._puntoOAsesor = reader(nombreColumnaEspecial2).ToString
                    Short.TryParse(reader("anio").ToString, Me._anio)
                    Short.TryParse(reader("mes").ToString, Me._mes)
                    Integer.TryParse(reader("meta").ToString, Me._meta)
                    Date.TryParse(reader("fechaRegistro").ToString, Me._fechaRegistro)
                    Integer.TryParse(reader("idUsuarioRegistra").ToString, Me._idUsuarioRegistra)
                    Boolean.TryParse(reader("estado").ToString, Me._estado)
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Publicos"

        Public MustOverride Function Registrar() As General.ResultadoProceso

        Public MustOverride Function Actualizar() As General.ResultadoProceso
#End Region

    End Class

End Namespace