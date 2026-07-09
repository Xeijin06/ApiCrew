# Configuración

## appsettings.json

### Secciones de Configuración

---

### ConnectionStrings

| Clave | Descripción |
|-------|-------------|
| `DefaultConnection` | Cadena de conexión a SQL Server (Azure SQL Database) |
| `LocalConnection` | Cadena de conexión local para desarrollo |

**Proveedor:** SQL Server via Entity Framework Core  
**DbContext:** `ApplicationDbContext` (hereda de `IdentityDbContext<ApplicationUser>`)

---

### AzureAd

Configuración de Azure Active Directory para autenticación JWT y Microsoft Graph.

| Clave | Descripción | Ejemplo |
|-------|-------------|---------|
| `Instance` | URL base de Azure AD | `https://login.microsoftonline.com/` |
| `Domain` | Dominio del tenant | `REPLACE` |
| `TenantId` | ID del tenant | GUID |
| `ClientId` | ID de la aplicación | GUID |
| `ClientSecret` | Secreto del cliente | String |
| `Scope` | Scope de la API | `api://{ClientId}/Graph` |
| `Audience` | Audience del token | `api://{ClientId}` |
| `Scope_graph` | Scopes para Graph API | `[".default"]` |
| `graph_version` | Versión de Graph API | `beta` o `v1.0` |

---

### CopaApi

Configuración de las APIs REST de Copa Airlines.

| Clave | Descripción | Ejemplo |
|-------|-------------|---------|
| `ApiUrl` | URL base de la API | `REPLACE` |
| `URLCopaSecured` | URL segura de Copa | `REPLACE` |
| `ChannelIDKey` | Nombre del header de canal | `REPLACE` |
| `ChannelIDValue` | Valor del header de canal | `Kiosk` |
| `SubscriptionKeyHeader` | Nombre del header de suscripción | `REPLACE` |
| `Timeout` | Timeout en segundos | `30` |

---

### RestApiSuscriptionKeys

Claves de suscripción API Management para cada servicio de Copa.

| Clave | Servicio |
|-------|----------|
| `Checkin` | API de Check-in (lista de pasajeros, preferidos, SSR) |
| `FlightOperations` | API de operaciones de vuelo |
| `CheckinSeats` | API de asientos |
| `IrregularOperations` | API de operaciones irregulares |
| `ListTimeZones` | API de zonas horarias |

---

### Flifo

Configuración del servicio SOAP FLIFO (Flight Information).

| Clave | Descripción |
|-------|-------------|
| `URL` | URL del servicio FLIFO |
| `Authorization` | Header de autorización Basic (Base64) |
| `Action` | SOAPAction header |

---

### AzureStorage

Configuración de Azure Blob Storage.

| Clave | Descripción |
|-------|-------------|
| `ConnectionString` | Cadena de conexión al Storage Account |
| `DefaultContainer` | Nombre del contenedor por defecto |
| `TimeZoneId` | ID de zona horaria para timestamps |

---

### Logging

Configuración estándar de logging de ASP.NET Core:

```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Information",
	  "Microsoft.AspNetCore": "Warning"
	}
  }
}
```

---

## Swagger / OpenAPI

Disponible **solo en entorno Development**:

- **Swagger UI:** `https://localhost:{port}/swagger`
- **OpenAPI JSON:** `https://localhost:{port}/swagger/v1/swagger.json`

Configuración en `Program.cs`:

```csharp
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
```

---

## Perfiles de Lanzamiento

Configurados en `Properties/launchSettings.json` para desarrollo local.

## Perfiles de Publicación

- `AZR-APP-CREWM-API-INT-E1-001 - Web Deploy.pubxml` - Publicación a Azure App Service (integración)
- `FolderProfile.pubxml` - Publicación a carpeta local
