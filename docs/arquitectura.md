# Arquitectura de la Solución

## Estructura de Proyectos

La solución está compuesta por **3 proyectos**:

```
CrewMobile_API/
├── CrewMobile.Api/          # Proyecto principal - Web API (.NET 8)
├── CrewMobile.Domain/       # Entidades de dominio (persistencia)
└── CrewMobile.Common/       # Modelos compartidos (DTOs, comunicación)
```

---

## CrewMobile.Api

Proyecto principal que expone la API REST. Contiene:

| Carpeta | Contenido |
|---------|-----------|
| `Controllers/` | 11 controladores REST |
| `Apis/` | Clientes HTTP para APIs externas de Copa Airlines |
| `Apis/Interfaces/` | Contratos `ICopaAPIs`, `ICopaSoap` |
| `Services/` | Servicios internos (Graph, BlobStorage, Logs) |
| `Services/Interface/` | Contratos de servicios |
| `Models/` | Modelos de respuesta y DbContext |
| `Business/` | Lógica de procesamiento de archivos |
| `Mocks/` | Datos mock para pruebas y fallback |
| `Content/Files/` | Archivos de contenido y logs |

### Controladores

| Controlador | Ruta base | Descripción |
|-------------|-----------|-------------|
| `AirportsController` | `api/Airports` | CRUD de aeropuertos |
| `EmployeesController` | `api/Employees` | CRUD de empleados |
| `FlightCrewsController` | `api/FlightCrews` | Tripulación, vuelos, información de siguiente leg |
| `ParametersController` | `api/Parameters` | Parámetros de configuración y grupos |
| `DescriptorsController` | `api/Descriptors` | CRUD de descriptores |
| `IrregularOperationsController` | `api/IrregularOperations` | Operaciones irregulares y pasajeros |
| `SecuritySSRsController` | `api/SecuritySSRs` | Códigos SSR de seguridad |
| `FlighStatusMocksController` | `api/FlighStatusMocks` | Mocks de estado de vuelo |
| `ProcessFileLogsController` | `api/ProcessFileLogs` | Logs de procesamiento |
| `TimeZonesController` | `api/TimeZones` | Carga de zonas horarias |
| `GraphController` | `api/Graph` | Integración Microsoft Graph |

---

## CrewMobile.Domain

Entidades de dominio que representan las tablas en la base de datos. Cada entidad hereda de un modelo base en `CrewMobile.Common` y agrega la clave primaria (`[Key]`).

| Entidad | Hereda de | Clave primaria |
|---------|-----------|----------------|
| `Airport` | `AirportCom` | `AirportId` |
| `Employee` | `EmployeeCom` | `EmployeeId` |
| `FlightCrew` | `FlightCrewCom` | `FlightCrewId` |
| `CMParameter` | `ParameterCom` | `ParameterId` |
| `Descriptor` | `DescriptorCom` | `DescriptorId` |
| `IrregularOperation` | `IrregularOperationCom` | `IrregularOperationId` |
| `Passanger` | `PassangerCom` | `PassangerId` |
| `SecuritySSR` | `SecuritySSRCom` | `SecuritySSId` |
| `Group` | `GroupCom` | `GroupId` |
| `ProccessFileLog` | `ProccessFileLogCom` | `ProccessFileLogId` |
| `IrregularOperationsLog` | `IrregularOperationsLogCom` | `IrregularOperationsLogId` |
| `FlighStatusMock` | `FlighStatusMockCom` | `FlighStatusMockId` |

---

## CrewMobile.Common

Modelos base (DTOs) utilizados para comunicación entre capas y con APIs externas. No contienen claves primarias de persistencia.

---

## Diagrama de Dependencias

```
CrewMobile.Api
  ├── CrewMobile.Domain
  │     └── CrewMobile.Common
  └── CrewMobile.Common
```

---

## Inyección de Dependencias

Configurada en `Program.cs`:

| Servicio | Implementación | Lifetime |
|----------|---------------|----------|
| `ICopaAPIs` | `CopaAPIs` | Scoped |
| `ICopaSoap` | `CopaSoap` | Scoped |
| `IBlobStorageService` | `BlobStorageService` | Singleton |
| `ILogStorageAccountService` | `LogStorageAccountService` | Singleton |
| `GraphService` | `GraphService` | HttpClient (Transient) |
| `ApplicationDbContext` | EF Core DbContext | Scoped |

---

## Paquetes NuGet Principales

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 8.0.8 | Autenticación JWT |
| `Microsoft.Identity.Web` | 3.8.4 | Integración Azure AD |
| `Microsoft.EntityFrameworkCore.SqlServer` | 8.0.8 | ORM - SQL Server |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | 8.0.8 | Identity Framework |
| `Microsoft.Graph` | 5.58.0 | Microsoft Graph API |
| `Azure.Storage.Blobs` | 12.25.1 | Azure Blob Storage |
| `Newtonsoft.Json` | 13.0.3 | Serialización JSON |
| `SSH.NET` | 2024.1.0 | Conexión SFTP/SSH |
| `Swashbuckle.AspNetCore` | 6.8.1 | Swagger/OpenAPI |
| `Microsoft.ApplicationInsights` | 2.23.0 | Telemetría |
