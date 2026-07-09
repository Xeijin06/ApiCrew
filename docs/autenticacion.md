# Autenticación y Seguridad

## Esquema de Autenticación

La API utiliza **Azure Active Directory (Azure AD)** con tokens **JWT Bearer** a través de la librería `Microsoft.Identity.Web`.

### Configuración en `Program.cs`

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
```

### Middleware

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

---

## Configuración de Azure AD

Los valores se definen en `appsettings.json` bajo la sección `AzureAd`:

| Propiedad | Descripción |
|-----------|-------------|
| `Instance` | URL de login de Azure AD (`https://login.microsoftonline.com/`) |
| `Domain` | Dominio del tenant |
| `TenantId` | ID del tenant de Azure AD |
| `ClientId` | ID de la aplicación registrada en Azure AD |
| `ClientSecret` | Secreto del cliente para autenticación servidor-a-servidor |
| `Scope` | Scope de la API |
| `Audience` | Audience esperado en los tokens |
| `Scope_graph` | Scopes para Microsoft Graph (`.default`) |
| `graph_version` | Versión de Microsoft Graph API (`beta` o `v1.0`) |

---

## Protección de Endpoints

La mayoría de endpoints están protegidos con el atributo `[Authorize]`:

```csharp
[Authorize]
[HttpGet]
[Route("GetAllAirports")]
public IActionResult GetAllAirports() { ... }
```

### Endpoints sin autorización

Los siguientes endpoints **NO** requieren autenticación:

| Controlador | Endpoint | Motivo |
|-------------|----------|--------|
| `ParametersController` | `GET GetSomeParameters` | Parámetros públicos para la app |
| `TimeZonesController` | `POST api/TimeZones` | Carga interna de zonas horarias |
| `FlightCrewsController` | `POST api/FlightCrews` | Proceso de carga de archivos FTP |
| `IrregularOperationsController` | `POST LoadIrrOps` | Proceso de carga de operaciones irregulares |

---

## Flujo de Autenticación

1. El cliente obtiene un token JWT de Azure AD usando el `ClientId` y `Scope` configurados.
2. El token se envía en el header `Authorization: Bearer <token>`.
3. La API valida el token contra Azure AD (issuer, audience, firma, expiración).
4. Si es válido, se permite el acceso al endpoint protegido.

---

## Integración con Microsoft Graph

El `GraphService` utiliza autenticación **Client Credentials** (servidor-a-servidor) para consultar información de usuarios en Azure AD:

- Obtiene un access token usando `ConfidentialClientApplicationBuilder` con `ClientId` y `ClientSecret`.
- Consulta la Microsoft Graph API para obtener información de usuarios por email.
- Se utiliza principalmente para resolver el `EmployeeId` de un usuario a partir de su email corporativo.
