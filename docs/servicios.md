# Servicios Internos

## GraphService

**Namespace:** `CrewMobileApi.Services`  
**Registro DI:** `HttpClient` (Transient via `AddHttpClient<GraphService>()`)

### Descripción

Servicio para interactuar con Microsoft Graph API. Utiliza autenticación Client Credentials (flujo de aplicación) para consultar información de usuarios en Azure AD.

### Métodos

#### `GetUserInformationByEmailAsync(string email)`

Obtiene información de un usuario por su email desde Microsoft Graph.

| Campo | Valor |
|-------|-------|
| **Retorno** | `Task<JObject>` |
| **Graph endpoint** | `GET /users?$filter=mail eq '{email}'` |
| **Versión Graph** | Configurable (`AzureAd:graph_version`) |

#### `GetAccessTokenAsync()` (privado)

Obtiene un access token para Microsoft Graph usando Client Credentials flow.

- Scopes: configurados en `AzureAd:Scope_graph` (por defecto `.default`)
- Usa `ConfidentialClientApplicationBuilder` con `ClientId` y `ClientSecret`

#### `GetExtendedProperties(User user)`

Obtiene las propiedades extendidas de un objeto `User` de Microsoft Graph.

---

## BlobStorageService

**Namespace:** `CrewMobile.Api.Services`  
**Interfaz:** `IBlobStorageService`  
**Registro DI:** Singleton

### Descripción

Servicio para gestionar archivos en Azure Blob Storage.

### Configuración

Se configura mediante `AzureStorageOptions` desde la sección `AzureStorage` del `appsettings.json`:

| Campo | Descripción |
|-------|-------------|
| `ConnectionString` | Cadena de conexión al Storage Account |
| `DefaultContainer` | Contenedor por defecto |
| `TimeZoneId` | Zona horaria para timestamps |

### Métodos

#### `UploadAsync<T>(T content, string fileName, string? containerName, string? contentType, CancellationToken ct)`

Sube contenido al Blob Storage.

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `content` | `T` | Contenido a subir |
| `fileName` | `string` | Nombre del archivo |
| `containerName` | `string?` | Contenedor (usa default si null) |
| `contentType` | `string?` | Tipo MIME |
| **Retorno** | `Task<Uri>` | URI del blob creado |

#### `DeleteOldFilesAsync(ILogStorageAccountService logStorageAccount, string? containerName, int? retentionDays, CancellationToken ct)`

Elimina archivos antiguos del Blob Storage según la política de retención.

---

## LogStorageAccountService

**Namespace:** `CrewMobile.Api.Services`  
**Interfaz:** `ILogStorageAccountService`  
**Registro DI:** Singleton

### Descripción

Servicio de logging en memoria que permite registrar, consultar y exportar logs.

### Métodos

| Método | Retorno | Descripción |
|--------|---------|-------------|
| `Log(LogLevel level, string message)` | `void` | Registra un log |
| `GetAllLogs()` | `List<LogEntry>` | Obtiene todos los logs |
| `ClearLogs()` | `void` | Limpia los logs |
| `ExportToTextFormatAsync()` | `Task<string>` | Exporta logs a texto |
