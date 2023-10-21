Imports NotusExpressBusinessLayer.Enumerados

Namespace Estructuras

    Public Structure FiltroConfigValues
        Dim IdConfig As Integer
        Dim ConfigKeyName As String
    End Structure

End Namespace

Namespace Enumerados

    Public Enum CodigoPais
        Colombia = 170
    End Enum

    Public Enum EstadosGestionDeVenta
        RegistroParcial = 7
        Registrada = 8
        VentaDeclinada = 10
    End Enum

    Public Enum EstadosNovedades
        Pendiente = 28
    End Enum

    Public Enum EstrategiasComerciales
        CentrosComerciales = 1
        presencial = 16
    End Enum

    Public Enum TipoServicio
        Reposicion = 1
        Venta = 2
        CesionContrato = 3
        Migracion = 4
        ServicioTecnico = 5
        Portacion = 6
        OrdenCompra = 7
        Siembra = 8
        VentaWeb = 9
        ServiciosFinancieros = 10
        ServicioFinancieroDavivienda = 18
    End Enum

    Public Enum Sistema
        NotusIls = 1
        NotusExpress = 2
    End Enum

    Public Enum AgrupacionServicio
        VentaTelefonica = 1
        Reposiciones_Ordenes_ST = 2
        Siembra = 3
        ServiciosFinancieros = 4
        DaviviendaVentaExterna = 14
    End Enum

    Public Enum EstadosServicioMensajeria
        EnCuarentena = 99
        Creado = 100
        Confirmado = 101
        Despachado = 102
        Entregado = 103
        Cerrado = 104
        DespachadoconCambio = 106
        Legalizado = 107
        Tránsito = 111
        Devolución = 112
        AsignadoRuta = 113
        Recuperacion = 114
        Cancelado = 115
        RecibidoCliente = 119
        RevisionServicioTécnico = 120
        ServicioTécnico = 121
        RecibidoST = 122
        Preventa = 162
        Anulado = 163
        Radicado = 164
        DevoluciónCallCenter = 165
        PendienteRecolección = 208
        PendientedeCierre = 228
        Entregadolegalización = 231
        NoAsignado = 0
        Rechazado = 234
        PendienteAprobacionCalidad = 271
        RechazadoCalidadContactCenter = 274
    End Enum

    Public Enum ResuladoProcesoVenta
        Aprobada = 1
        Negada = 2
    End Enum

    Public Enum Entidad
        General = 1
        InventarioBodega = 2
        GestionDeVenta = 3
        TomaFisica = 4
        Auditoria = 5
        Novedad = 6
        Archivopoliza = 7
        NovedadServicio = 8
		AgendamientoAsesor = 10
    End Enum

    Public Enum ClienteExterno
        COMCEL = 1
        DAVIVIENDA = 3
        DAVIVIENDAEXTERNO = 7
        COLPATRIA = 5
    End Enum

    Public Enum TipoRegistroVisitaAsesor
        Entrada = 1
        Salida = 2
    End Enum

    Public Enum Jornada
        Manana = 1
        Tarde = 2
    End Enum

    Public Enum EstadoAgendamientoVisitaAsesor
        Agendado = 36
        Declinado = 37
        Reagendado = 38
        Visitado = 39
    End Enum

    Public Enum Perfil
        SuperAdministrador = 1
        AsesorComercial = 2
        AsesorComercialSupernumerario = 3
        AsesorComercialCallCenter = 20
    End Enum
End Namespace