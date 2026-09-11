# Endpoints - FlightCrews

**Controlador:** `FlightCrewsController`  
**Ruta base:** `api/FlightCrews`  
**Descripción:** Controlador principal de la API. Gestiona la tripulación de vuelo, información de siguiente leg, listas de pasajeros, SSR, conteos y procesamiento de archivos.

---

## Endpoints CRUD de Base de Datos

### GET GetFlightCrews

Obtiene la lista completa de tripulación de vuelos.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/FlightCrews/GetFlightCrews` |
| **Autorización** | Requerida |
| **Respuesta** | `200 OK` - Lista de `FlightCrew` |

### GET GetFlightCrews/{Id}

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/FlightCrews/GetFlightCrews/{Id}` |
| **Autorización** | Requerida |
| **Parámetros** | `Id` (int, ruta) |
| **Respuesta** | `200 OK` / `404 Not Found` |

### POST AddFlightCrews

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/FlightCrews/AddFlightCrews` |
| **Autorización** | Requerida |
| **Body** | Objeto `FlightCrew` (JSON) |
| **Respuesta** | `200 OK` - Objeto creado |

### PUT UpdateFlightCrews

| Campo | Valor |
|-------|-------|
| **URL** | `PUT api/FlightCrews/UpdateFlightCrews` |
| **Autorización** | Requerida |
| **Body** | Objeto `FlightCrew` completo |
| **Respuesta** | `204 No Content` / `404 Not Found` |

### DELETE DeleteFlightCrews/{Id}

| Campo | Valor |
|-------|-------|
| **URL** | `DELETE api/FlightCrews/DeleteFlightCrews/{Id}` |
| **Autorización** | Requerida |
| **Respuesta** | `204 No Content` |

### Modelo `FlightCrew`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `FlightCrewId` | `int` | PK |
| `FlightNumber` | `int` | Número de vuelo |
| `Company` | `string` | Compañía aérea |
| `DateStart` | `DateTime` | Fecha/hora de inicio (formato: `yyyy/MM/dd HH:mm`) |
| `DateEnd` | `DateTime` | Fecha/hora de fin |
| `Source` | `string` | Aeropuerto de origen |
| `Destination` | `string` | Aeropuerto de destino |
| `CrewRoll` | `string` | Rol del tripulante |
| `CrewId` | `int` | ID del empleado tripulante |
| `CrewName` | `string` | Nombre del tripulante |

---

## Endpoints CRUD de FlightCrewFileLogs

### GET GetFlightCrewFileLogs

| **URL** | `GET api/FlightCrews/GetFlightCrewFileLogs` |
|---------|----------------------------------------------|
| **Respuesta** | `200 OK` - Lista de `ProccessFileLog` |

### GET GetFlightCrewFileLogs/{Id}

| **URL** | `GET api/FlightCrews/GetFlightCrewFileLogs/{Id}` |
|---------|--------------------------------------------------|

### POST AddFlightCrewFileLogs

| **URL** | `POST api/FlightCrews/AddFlightCrewFileLogs` |
|---------|----------------------------------------------|
| **Body** | Objeto `ProccessFileLog` (JSON) |

### PUT UpdateFlightCrewFileLogs

| **URL** | `PUT api/FlightCrews/UpdateFlightCrewFileLogs` |
|---------|-------------------------------------------------|

### DELETE DeleteFlightCrewFileLogs/{Id}

| **URL** | `DELETE api/FlightCrews/DeleteFlightCrewFileLogs/{Id}` |
|---------|-------------------------------------------------------|

---

## Endpoints de APIs Externas (Copa Airlines)

### GET GetApiPassengerlist

Obtiene la lista de pasajeros de un vuelo desde la API de Copa Airlines.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/FlightCrews/GetApiPassengerlist/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}/{cabinClass}` |
| **Autorización** | Requerida |

**Parámetros de ruta:**

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `flightNumber` | `string` | Número de vuelo |
| `departureDate` | `DateTime` | Fecha de salida |
| `operatorCarrier` | `string` | Código de aerolínea operadora |
| `origin` | `string` | Código IATA de origen |
| `destination` | `string` | Código IATA de destino |
| `cabinClass` | `string` | Clase de cabina |

**Respuestas:** `200 OK` (lista de pasajeros) / `400 Bad Request` (error del servicio)

---

### GET GetApiSSR

Obtiene la lista SSR (Special Service Requests) de pasajeros.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/FlightCrews/GetApiSSR/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}` |
| **Autorización** | Requerida |

**Parámetros:** `flightNumber`, `departureDate`, `operatorCarrier`, `origin`, `destination`

---

### GET GetApiPrefer

Obtiene la lista de pasajeros preferidos/prioritarios.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/FlightCrews/GetApiPrefer/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}` |
| **Autorización** | Requerida |

**Parámetros:** `flightNumber`, `departureDate`, `operatorCarrier`, `origin`, `destination`

---

### GET GetApiPassengerlistMeals

Obtiene los conteos de comidas/pasajeros de un vuelo.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/FlightCrews/GetApiPassengerlistMeals/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}` |
| **Autorización** | Requerida |

---

### GET GetApiPassengerlistStandbyList

Obtiene la lista de pasajeros en standby.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/FlightCrews/GetApiPassengerlistStandbyList/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}` |
| **Autorización** | Requerida |

---

### GET GetApiFlightInformation

Obtiene información de vuelo desde la API de Copa.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/FlightCrews/GetApiFlightInformation/{departureFrom}/{departureTo}/{flightNumber}` |
| **Autorización** | Requerida |

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `departureFrom` | `DateTime` | Fecha inicio del rango |
| `departureTo` | `DateTime` | Fecha fin del rango |
| `flightNumber` | `string` | Número de vuelo |

---

## Endpoints de Lógica de Negocio

### POST GetTimeLeft

Calcula el tiempo restante para un vuelo en hora local de la ciudad de origen.

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/FlightCrews/GetTimeLeft` |
| **Autorización** | Requerida |
| **Body** | JSON con datos del vuelo |

**Body de ejemplo:**
```json
{
  "FlightNumber": "123",
  "Date": "2025-01-15",
  "Source": "PTY",
  "Destination": "MIA"
}
```

**Respuesta:** `TimeLeftResponse`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Hours` | `int` | Horas restantes |
| `Minutes` | `int` | Minutos restantes |

---

### POST GetNextLeg

**Endpoint principal.** Obtiene toda la información del siguiente leg de vuelo para un tripulante, incluyendo lista de pasajeros, SSR, conteos de comidas, asientos, y datos del siguiente-siguiente leg.

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/FlightCrews/GetNextLeg` |
| **Autorización** | Requerida |
| **Body** | `{ "Email": "user@copa.com" }` |

**Flujo interno:**
1. Resuelve el `EmployeeId` del email (vía DB mock o Microsoft Graph)
2. Obtiene vuelos en las próximas 24 horas
3. Ejecuta en paralelo: lista de pasajeros, preferidos, SSR, conteos, asientos
4. Construye la respuesta consolidada con conteos, nombres de aeropuertos, siguiente leg

**Respuesta:** `NextLegResponse`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Source` | `FlightResponse` | Aeropuerto de origen |
| `Destination` | `FlightResponse` | Aeropuerto de destino |
| `Airline` | `string` | Aerolínea |
| `FlightNumber` | `string` | Número de vuelo |
| `Passangers` | `int` | Total de pasajeros |
| `Status` | `string` | Estado del vuelo |
| `BussinessClass` | `int` | Pasajeros en clase ejecutiva |
| `SSR` | `int` | Cantidad de SSR |
| `Preferred` | `int` | Cantidad de preferidos |
| `Meals` | `int` | Cantidad de comidas |
| `ServiceStatus` | `ServiceStatus` | Estado de cada servicio consultado |
| `TimeLeft` | `TimeLeftResponse` | Tiempo restante |
| `Thru` | `int` | Pasajeros en tránsito |
| `IsFinal` | `bool` | Si es el último leg |
| `Equipment` | `string` | Tipo de aeronave |
| `Gate` | `string` | Puerta de embarque |
| `Roll` | `string` | Rol del tripulante |
| `PreferCountsList` | `List<Count>` | Desglose de conteos preferidos |
| `SSRCountsList` | `List<Count>` | Desglose de conteos SSR |
| `MealCountsList` | `List<Count>` | Desglose de conteos de comidas |
| `BussinesCountsList` | `List<Count>` | Desglose de conteos business |
| `PassengerList` | `List<PassengerListCom>` | Lista de pasajeros |
| `NextLeg` | `NextNextLegResponse` | Información del siguiente leg |
| `EquipmentRows` | `List<EquipmentRow>` | Filas del equipo/aeronave |
| `ShowFullFare` | `bool` | Si se muestra tarifa completa |

**Códigos de error:**
| Código | Descripción |
|--------|-------------|
| `001` | Llamada incorrecta / Error general |
| `002` | No se puede recuperar información del empleado |
| `003` | No hay vuelos disponibles |
| `004` | Error en servicio de lista de pasajeros |
| `005` | Error en servicio de lista de preferidos |
| `006` | Error en servicio de SSR |
| `007` | Error en servicio de conteos |
| `008` | Error en servicio de asientos |

---

### POST GetNextLegAfterCancelled

Similar a `GetNextLeg`, pero avanza al siguiente leg después de una cancelación.

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/FlightCrews/GetNextLegAfterCancelled` |
| **Autorización** | Requerida |
| **Body** | `{ "Email": "user@copa.com" }` |
| **Respuesta** | `NextLegResponse` |

---

### POST GetFlightsWithActual

Obtiene vuelos que tienen información "Actual" (ya despegaron o tienen datos reales).

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/FlightCrews/GetFlightsWithActual` |
| **Autorización** | Requerida |
| **Respuesta** | `200 OK` - Lista de strings con información de vuelos |

---

### POST ProcessFile (Post)

Ejecuta el proceso de importación de archivos FTP para cargar la agenda de asistencia de vuelos.

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/FlightCrews` |
| **Autorización** | **No requerida** |
| **Respuesta** | `200 OK` - "Ok" |

---

## Modelo `ServiceStatus`

Indica qué servicios externos se consultaron exitosamente:

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `GotPassangerList` | `bool` | Lista de pasajeros obtenida |
| `GotPreferredList` | `bool` | Lista de preferidos obtenida |
| `GotPassangerListSSR` | `bool` | Lista SSR obtenida |
| `GotCounts` | `bool` | Conteos obtenidos |
| `GotSeats` | `bool` | Asientos obtenidos |

## Modelo `FlightResponse`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Abbreviation` | `string` | Abreviación del aeropuerto |
| `Name` | `string` | Nombre del aeropuerto |
| `Country` | `string` | País |
| `Date` | `DateTime` | Fecha del vuelo |
| `DateEstimated` | `DateTime` | Fecha estimada (excluido de JSON) |
