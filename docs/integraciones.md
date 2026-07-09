# Integraciones Externas

## Copa Airlines REST API (`ICopaAPIs`)

**Implementación:** `CopaAPIs`  
**Registro DI:** Scoped  
**Base URL:** Configurada en `CopaApi:ApiUrl` (producción: `REPLACE`)

### Autenticación

Las APIs de Copa utilizan suscripción por clave API (header `REPLACE`):

| Servicio | Clave (sección `RestApiSuscriptionKeys`) |
|----------|------------------------------------------|
| Check-in | `Checkin` |
| Operaciones de vuelo | `FlightOperations` |
| Asientos de check-in | `CheckinSeats` |
| Operaciones irregulares | `IrregularOperations` |
| Zonas horarias | `ListTimeZones` |

**Headers comunes:**
- `REPLACE`: `Kiosk`
- `REPLACE`: valor según servicio

### Métodos

#### GetPassengerList

Obtiene la lista de pasajeros de un vuelo.

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `flightNumber` | `string` | Número de vuelo |
| `departureDate` | `DateTime` | Fecha de salida |
| `operatorCarrier` | `string` | Aerolínea operadora |
| `origin` | `string` | Código IATA origen |
| `destination` | `string` | Código IATA destino |
| `cabinClass` | `string` | Clase de cabina ("All" para todas) |

#### GetSSR

Obtiene la lista de Special Service Requests de pasajeros.

**Parámetros:** `flightNumber`, `departureDate`, `operatorCarrier`, `origin`, `destination`

#### GetPrefer

Obtiene la lista de pasajeros preferidos/prioritarios.

**Parámetros:** `flightNumber`, `departureDate`, `operatorCarrier`, `origin`, `destination`

#### GetCounts

Obtiene conteos de pasajeros y comidas.

**Parámetros:** `flightNumber`, `departureDate`, `operatorCarrier`, `origin`, `destination`

#### GetPassengerListStandbyList

Obtiene la lista de pasajeros en standby.

**Parámetros:** `flightNumber`, `departureDate`, `operatorCarrier`, `origin`, `destination`

#### GetSeats

Obtiene el mapa de asientos del vuelo.

**Parámetros:** `flightNumber`, `departureDate`, `operatorCarrier`, `origin`, `destination`

#### GetApiFlightInformation

Obtiene información detallada de un vuelo en un rango de fechas.

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `departureFrom` | `DateTime` | Inicio del rango |
| `departureTo` | `DateTime` | Fin del rango |
| `flightNumber` | `string` | Número de vuelo |

#### GetIrregularOperations

Obtiene todas las operaciones irregulares activas.

**Parámetros:** Ninguno

#### GetListTimeZone

Obtiene las zonas horarias para una lista de aeropuertos.

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `airports` | `List<Airport>` | Lista de aeropuertos |

### Timeout

Configurable en `CopaApi:Timeout` (valor por defecto: 30 segundos).

### Modelo de respuesta

Todas las llamadas retornan un objeto `Response`:

```json
{
  "isSuccess": true,
  "message": "Success",
  "result": { ... }
}
```

---

## Copa Airlines SOAP API (`ICopaSoap`)

**Implementación:** `CopaSoap`  
**Registro DI:** Scoped  
**Descripción:** Interfaz SOAP para consultar información de vuelos (FLIFO - Flight Information).

### Configuración (sección `Flifo`)

| Campo | Descripción |
|-------|-------------|
| `URL` | URL del servicio FLIFO |
| `Authorization` | Header de autorización Basic |
| `Action` | SOAPAction header |

### Métodos

#### GetFlightInformation

Obtiene información de un vuelo via SOAP (FLIFO).

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `flightNumber` | `string` | Número de vuelo |
| `date` | `string` | Fecha del vuelo |
| **Retorno** | `FlightDetailSoapCom2` | Detalle completo del vuelo |

La respuesta incluye:
- Estado del vuelo (scheduled, estimated, actual)
- Legs con aeropuertos de salida/llegada
- Horarios UTC de salida/llegada (programado, estimado, actual)

#### GetFlightInformation2

Versión que retorna el XML crudo como string. Utilizada para verificar si un vuelo tiene información "Actual".

#### GetListTimeZone

Obtiene zonas horarias via SOAP para una lista de aeropuertos.

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `airports` | `List<Airport>` | Lista de aeropuertos |
| **Retorno** | `TimeZoneSoapCom` | Zonas horarias |

---

## Sistema de Mocks

La API tiene un sistema de fallback con datos mock configurable a través de `CMParameter`:

| Parámetro | Comportamiento |
|-----------|----------------|
| `MockedEmployees = true` | Usa la tabla `Employees` de BD en lugar de Graph para resolver el ID |
| `MockServices = true` | Si un servicio de Copa falla, usa datos mock como fallback |
| `AlwaysMockServices = true` | Siempre usa datos mock sin llamar a servicios reales |
| `FailOneServiceFailAllServices = true` | Si un servicio falla, retorna error (sin fallback) |

Los mocks están definidos en `CrewMobile.Api/Mocks/` como archivos JSON y la clase `MockHelper`.
