# Endpoints - IrregularOperations

**Controlador:** `IrregularOperationsController`  
**Ruta base:** `api/IrregularOperations`

---

## Sección: Irregular Operations (CRUD)

### GET GetIrregularOperations

| **URL** | `GET api/IrregularOperations/GetIrregularOperations` |
|---------|------------------------------------------------------|
| **Autorización** | Requerida |
| **Respuesta** | `200 OK` - Lista de `IrregularOperation` |

### GET GetIrregularOperations/{Id}

| **URL** | `GET api/IrregularOperations/GetIrregularOperations/{Id}` |
|---------|-----------------------------------------------------------|
| **Respuesta** | `200 OK` / `404 Not Found` |

### POST AddIrregularOperations

| **URL** | `POST api/IrregularOperations/AddIrregularOperations` |
|---------|-------------------------------------------------------|
| **Body** | Objeto `IrregularOperation` (JSON) |

### PUT UpdateIrregularOperations

| **URL** | `PUT api/IrregularOperations/UpdateIrregularOperations` |
|---------|----------------------------------------------------------|

### DELETE DeleteIrregularOperations/{Id}

| **URL** | `DELETE api/IrregularOperations/DeleteIrregularOperations/{Id}` |
|---------|-----------------------------------------------------------------|

### Modelo `IrregularOperation`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `IrregularOperationId` | `int` | PK |
| `FlightNumber` | `string` | Número de vuelo |
| `Origin` | `string` | Aeropuerto de origen |
| `Destination` | `string` | Aeropuerto de destino |
| `Description` | `string` | Descripción de la irregularidad |
| `DepartureDate` | `DateTime` | Fecha de salida |
| `Passangers` | `ICollection<Passanger>` | Pasajeros asociados (no incluido en JSON) |

---

## Sección: Passengers (CRUD)

### GET GetPassengers

| **URL** | `GET api/IrregularOperations/GetPassengers` |
|---------|----------------------------------------------|
| **Autorización** | Requerida |
| **Respuesta** | `200 OK` - Lista de `Passanger` |

### GET GetPassengers/{Id}

| **URL** | `GET api/IrregularOperations/GetPassengers/{Id}` |
|---------|--------------------------------------------------|

### POST AddPassengers

| **URL** | `POST api/IrregularOperations/AddPassengers` |
|---------|-----------------------------------------------|

### PUT UpdatePassengers

| **URL** | `PUT api/IrregularOperations/UpdatePassengers` |
|---------|--------------------------------------------------|

### DELETE DeletePassangers/{Id}

| **URL** | `DELETE api/IrregularOperations/DeletePassangers/{Id}` |
|---------|--------------------------------------------------------|

### Modelo `Passanger`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `PassangerId` | `int` | PK |
| `IrregularOperationId` | `int` | FK a IrregularOperation |
| `ConfirmationID` | `string` | ID de confirmación |
| `Surname` | `string` | Apellido |
| `GivenName` | `string` | Nombre |
| `FinalPassengerDestination` | `string` | Destino final |
| `CabinClass` | `string` | Clase de cabina |
| `IsFullFare` | `bool` | Tarifa completa |
| `IsDiscountedFare` | `bool` | Tarifa con descuento |
| `FareKind` | `string` | Tipo de tarifa |
| `SeatLeg` | `int` | Leg del asiento |
| `BookingClass` | `string` | Clase de reserva |
| `Seat` | `string` | Asiento asignado |
| `Treatment` | `string` | Tratamiento/Protocolo |

---

## Sección: IrregularOperations Logs (CRUD)

### GET GetIrropsLogs

| **URL** | `GET api/IrregularOperations/GetIrropsLogs` |
|---------|----------------------------------------------|

### GET GetIrropsLogs/{Id}

| **URL** | `GET api/IrregularOperations/GetIrropsLogs/{Id}` |
|---------|--------------------------------------------------|

### POST AddIrrOpsLogs

| **URL** | `POST api/IrregularOperations/AddIrrOpsLogs` |
|---------|-----------------------------------------------|

### PUT UpdateIrrOpsLogs

| **URL** | `PUT api/IrregularOperations/UpdateIrrOpsLogs` |
|---------|------------------------------------------------|

### DELETE DeleteIrrOpsLog/{Id}

| **URL** | `DELETE api/IrregularOperations/DeleteIrrOpsLog/{Id}` |
|---------|------------------------------------------------------|

### Modelo `IrregularOperationsLog`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `IrregularOperationsLogId` | `int` | PK |
| `Date` | `DateTime` | Fecha del log |
| `WasSuccess` | `bool` | Si fue exitoso |
| `Steps` | `string` | Pasos ejecutados |

---

## Endpoint de Proceso

### POST LoadIrrOps

Carga operaciones irregulares desde la API externa de Copa Airlines, crea registros en BD y asocia la lista de pasajeros.

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/IrregularOperations/LoadIrrOps` |
| **Autorización** | **No requerida** |
| **Respuesta exitosa** | `200 OK` - "Ok" |
| **Respuesta error** | `400 Bad Request` - "No irregular operations found" |

**Flujo interno:**
1. Registra log de inicio
2. Consulta operaciones irregulares desde la API de Copa (`GetIrregularOperations`)
3. Por cada operación:
   - Si no existe en BD: crea el registro
   - Si existe: elimina pasajeros anteriores
   - Obtiene la lista de pasajeros actualizada
   - Inserta los nuevos pasajeros
4. Registra log de finalización
