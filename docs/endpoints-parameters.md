# Endpoints - Parameters

**Controlador:** `ParametersController`  
**Ruta base:** `api/Parameters`

---

## Sección: Parameters

### GET GetSomeParameters

Obtiene parámetros esenciales para la aplicación móvil. **No requiere autorización.**

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/Parameters/GetSomeParameters` |
| **Autorización** | **No requerida** |
| **Respuesta** | `200 OK` - `ParameterResponse` |

**Respuesta `ParameterResponse`:**

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `MinimumRequiredVersion` | `double` | Versión mínima requerida de la app |
| `AvailableGroups` | `string` | GUIDs de grupos separados por coma |
| `ValidateUserGroups` | `bool` | Si se validan los grupos del usuario |
| `AutoUpdateEveryMinutes` | `int` | Intervalo de auto-actualización en minutos |

---

### GET GetParameters

| **URL** | `GET api/Parameters/GetParameters` |
|---------|------------------------------------|
| **Autorización** | Requerida |
| **Respuesta** | `200 OK` - Lista de `CMParameter` |

### GET GetParameters/{Id}

| **URL** | `GET api/Parameters/GetParameters/{Id}` |
|---------|----------------------------------------|
| **Autorización** | Requerida |
| **Respuesta** | `200 OK` / `404 Not Found` |

### POST AddParameters

| **URL** | `POST api/Parameters/AddParameters` |
|---------|-------------------------------------|
| **Autorización** | Requerida |
| **Body** | Objeto `CMParameter` (JSON) |

### PUT UpdateParameters

| **URL** | `PUT api/Parameters/UpdateParameters` |
|---------|---------------------------------------|
| **Autorización** | Requerida |
| **Body** | Objeto `CMParameter` completo |

### DELETE DeleteParameters/{Id}

| **URL** | `DELETE api/Parameters/DeleteParameters/{Id}` |
|---------|-----------------------------------------------|

### Modelo `CMParameter`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `ParameterId` | `int` | PK |
| `MinimumRequiredVersion` | `double` | Versión mínima de la app |
| `MockedEmployees` | `bool` | Usar empleados mock |
| `MockServices` | `bool` | Usar servicios mock |
| `AlwaysMockServices` | `bool` | Siempre usar mock |
| `FailOneServiceFailAllServices` | `bool` | Si falla un servicio, fallan todos |
| `ValidateUserGroups` | `bool` | Validar grupos de usuario |
| `AutoUpdateEveryMinutes` | `int` | Intervalo de auto-actualización |
| `HoursToShowLegs` | `int` | Horas para mostrar legs |
| `MinuteToKeepFlight` | `int` | Minutos para mantener vuelo actual |
| `ShowFullFare` | `bool` | Mostrar tarifa completa |
| `ParameterName` | `string` | Nombre del parámetro |

---

## Sección: Groups

### GET GetAllGroups

| **URL** | `GET api/Parameters/GetAllGroups` |
|---------|----------------------------------|
| **Autorización** | Requerida |
| **Respuesta** | `200 OK` - Lista de `Group` |

### GET GetGroupById/{Id}

| **URL** | `GET api/Parameters/GetGroupById/{Id}` |
|---------|----------------------------------------|
| **Respuesta** | `200 OK` / `404 Not Found` |

### POST AddGroup

| **URL** | `POST api/Parameters/AddGroup` |
|---------|-------------------------------|
| **Body** | Objeto `Group` (JSON) |

### PUT UpdateGroup

| **URL** | `PUT api/Parameters/UpdateGroup` |
|---------|----------------------------------|

### DELETE DeleteGroup/{Id}

| **URL** | `DELETE api/Parameters/DeleteGroup/{Id}` |
|---------|-----------------------------------------|

### Modelo `Group`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `GroupId` | `int` | PK |
| `GroupGuid` | `Guid` | GUID del grupo en Azure AD |
| `GroupName` | `string?` | Nombre del grupo |
