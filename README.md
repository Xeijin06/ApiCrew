# CrewMobile API - Documentación Técnica

## Información General

| Campo | Valor |
|-------|-------|
| **Nombre** | CrewMobile API |
| **Framework** | .NET 8 (ASP.NET Core Web API) |
| **Autenticación** | Azure AD con JWT Bearer (Microsoft Identity Web) |
| **Base de datos** | SQL Server (Entity Framework Core 8) |
| **Formato de respuesta** | JSON (Newtonsoft.Json) |
| **Documentación interactiva** | Swagger/OpenAPI (solo en Development) |
| **Repositorio** | `https://copavsts.visualstudio.com/CrewMobileSolutions/_git/CrewMobile_Api` |
| **Rama principal** | `develop` |

---

## Índice General

| Documento | Descripción |
|-----------|-------------|
| [Arquitectura](./docs/arquitectura.md) | Arquitectura de la solución, proyectos y capas |
| [Autenticación y Seguridad](./docs/autenticacion.md) | Configuración de Azure AD, JWT Bearer |
| [Endpoints - Airports](./docs/endpoints-airports.md) | CRUD de aeropuertos |
| [Endpoints - Employees](./docs/endpoints-employees.md) | CRUD de empleados |
| [Endpoints - FlightCrews](./docs/endpoints-flightcrews.md) | Operaciones de tripulación y vuelos |
| [Endpoints - Parameters](./docs/endpoints-parameters.md) | Parámetros de configuración y grupos |
| [Endpoints - Descriptors](./docs/endpoints-descriptors.md) | CRUD de descriptores |
| [Endpoints - IrregularOperations](./docs/endpoints-irregularoperations.md) | Operaciones irregulares, pasajeros y logs |
| [Endpoints - SecuritySSRs](./docs/endpoints-securityssrs.md) | CRUD de códigos SSR de seguridad |
| [Endpoints - FlighStatusMocks](./docs/endpoints-flighstatusmocks.md) | CRUD de mocks de estado de vuelo |
| [Endpoints - ProcessFileLogs](./docs/endpoints-processfilelogs.md) | CRUD de logs de procesamiento de archivos |
| [Endpoints - TimeZones](./docs/endpoints-timezones.md) | Carga de zonas horarias de aeropuertos |
| [Endpoints - Graph](./docs/endpoints-graph.md) | Integración con Microsoft Graph |
| [Modelos de Datos](./docs/modelos.md) | Entidades de dominio y DTOs |
| [Servicios](./docs/servicios.md) | Servicios internos (Graph, BlobStorage, Logs) |
| [Integraciones Externas](./docs/integraciones.md) | APIs de Copa Airlines y SOAP |
| [Configuración](./docs/configuracion.md) | appsettings, cadenas de conexión y variables |

---

## Diagramas (PlantUML)

| Diagrama | Descripción |
|----------|-------------|
| [Diagrama General](./docs/diagrams/diagrama-general.puml) | Arquitectura general: componentes externos, servicios y flujos de comunicación |
| [Diagrama de Componentes](./docs/diagrams/diagrama-componentes.puml) | Componentes internos: controladores, servicios, APIs, modelos y dependencias |
| [Diagrama de Procesos](./docs/diagrams/diagrama-procesos.puml) | Secuencia de los procesos principales (GetNextLeg, ProcessFile, LoadIrrOps, TimeZones) |
| [Flujo - GetNextLeg](./docs/diagrams/diagrama-flujo-getnextleg.puml) | Flujo de actividad del endpoint principal GetNextLeg |
| [Flujo - LoadIrrOps](./docs/diagrams/diagrama-flujo-loadirrops.puml) | Flujo de actividad de la carga de operaciones irregulares |
| [Flujo - ProcessFile](./docs/diagrams/diagrama-flujo-processfile.puml) | Flujo de actividad de la importación de archivos FTP de tripulación |
