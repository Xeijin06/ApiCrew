# Modelos de Datos

## Diagrama de Entidades

```
ApplicationDbContext (IdentityDbContext)
├── FlightCrews          : FlightCrew
├── Parameters           : CMParameter
├── Descriptors          : Descriptor
├── Employees            : Employee
├── Airports             : Airport
├── Passangers           : Passanger ──┐
├── IrregularOperations  : IrregularOperation ◄──┘ (1:N)
├── SecuritySSRs         : SecuritySSR
├── Groups               : Group
├── ProccessFileLogs     : ProccessFileLog
├── IrregularOperationsLogs : IrregularOperationsLog
└── FlighStatusMocks     : FlighStatusMock
```

---

## Entidades de Dominio (`CrewMobile.Domain`)

### Airport

| Campo | Tipo | Validación | Descripción |
|-------|------|------------|-------------|
| `AirportId` | `int` | PK | Identificador |
| `AirportCode` | `string` | Required, 3 chars | Código IATA |
| `AirportName` | `string` | Required, max 50 | Nombre |
| `AirportAbbreviation` | `string` | Required, max 20 | Abreviación |
| `CountryCode` | `string` | Required, 2 chars | Código ISO país |
| `TimeZone` | `string` | max 50 | Zona horaria |
| `GTMOffset` | `int` | - | Offset GMT (horas) |

### Employee

| Campo | Tipo | Validación | Descripción |
|-------|------|------------|-------------|
| `EmployeeId` | `int` | PK | Identificador |
| `Email` | `string` | Required, DataType.EmailAddress | Email corporativo |
| `Code` | `int` | Required | Código empleado |

### FlightCrew

| Campo | Tipo | Validación | Descripción |
|-------|------|------------|-------------|
| `FlightCrewId` | `int` | PK | Identificador |
| `FlightNumber` | `int` | - | Número de vuelo |
| `Company` | `string` | - | Compañía |
| `DateStart` | `DateTime` | Format: yyyy/MM/dd HH:mm | Inicio |
| `DateEnd` | `DateTime` | Format: yyyy/MM/dd HH:mm | Fin |
| `Source` | `string` | - | Origen |
| `Destination` | `string` | - | Destino |
| `CrewRoll` | `string` | - | Rol |
| `CrewId` | `int` | - | ID empleado |
| `CrewName` | `string` | - | Nombre |

### CMParameter

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `ParameterId` | `int` | PK |
| `MinimumRequiredVersion` | `double` | Versión mínima app |
| `MockedEmployees` | `bool` | Usar empleados mock |
| `MockServices` | `bool` | Usar servicios mock |
| `AlwaysMockServices` | `bool` | Siempre mock |
| `FailOneServiceFailAllServices` | `bool` | Fallo en cascada |
| `ValidateUserGroups` | `bool` | Validar grupos |
| `AutoUpdateEveryMinutes` | `int` | Auto-actualización |
| `HoursToShowLegs` | `int` | Horas para legs |
| `MinuteToKeepFlight` | `int` | Minutos vuelo actual |
| `ShowFullFare` | `bool` | Mostrar tarifa completa |
| `ParameterName` | `string` | Nombre |

### Descriptor

| Campo | Tipo | Validación | Descripción |
|-------|------|------------|-------------|
| `DescriptorId` | `int` | PK | Identificador |
| `Name` | `string` | Required, max 50 | Nombre |
| `Abbreviation` | `string` | Required, max 20 | Abreviación |
| `Order` | `int` | - | Orden |

### IrregularOperation

| Campo | Tipo | Validación | Descripción |
|-------|------|------------|-------------|
| `IrregularOperationId` | `int` | PK | Identificador |
| `FlightNumber` | `string` | Required | Número de vuelo |
| `Origin` | `string` | Required | Origen |
| `Destination` | `string` | Required | Destino |
| `Description` | `string` | Required | Descripción |
| `DepartureDate` | `DateTime` | - | Fecha salida |
| `Passangers` | `ICollection<Passanger>` | JsonIgnore | Pasajeros (navegación) |

### Passanger

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `PassangerId` | `int` | PK |
| `IrregularOperationId` | `int` | FK |
| `ConfirmationID` | `string` | ID de confirmación |
| `Surname` | `string` | Apellido |
| `GivenName` | `string` | Nombre |
| `FinalPassengerDestination` | `string` | Destino final |
| `CabinClass` | `string` | Clase cabina |
| `IsFullFare` | `bool` | Tarifa completa |
| `IsDiscountedFare` | `bool` | Tarifa descuento |
| `FareKind` | `string` | Tipo tarifa |
| `SeatLeg` | `int` | Leg asiento |
| `BookingClass` | `string` | Clase reserva |
| `Seat` | `string` | Asiento |
| `Treatment` | `string` | Tratamiento |
| `IrregularOperation` | `IrregularOperation` | Navegación (JsonIgnore) |

### SecuritySSR

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `SecuritySSId` | `int` | PK |
| `Code` | `string` | Código SSR |

### Group

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `GroupId` | `int` | PK |
| `GroupGuid` | `Guid` | GUID en Azure AD |
| `GroupName` | `string?` | Nombre |

### ProccessFileLog

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `ProccessFileLogId` | `int` | PK |
| `Date` | `DateTime` | Fecha (yyyy/MM/dd HH:mm:ss) |
| `WasSuccess` | `bool` | Éxito |
| `Steps` | `string` | Pasos |

### IrregularOperationsLog

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `IrregularOperationsLogId` | `int` | PK |
| `Date` | `DateTime` | Fecha |
| `WasSuccess` | `bool` | Éxito |
| `Steps` | `string` | Pasos |

### FlighStatusMock

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `FlighStatusMockId` | `int` | PK |
| `FlighNumber` | `int` | Número de vuelo |
| `Date` | `DateTime` | Fecha |
| `Status` | `string` | ON-TIME, DELAYED, CANCELLED |

---

## Modelos de Respuesta (`CrewMobile.Api.Models`)

### Response (genérico)

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `IsSuccess` | `bool` | Indica éxito |
| `Message` | `string` | Mensaje |
| `Result` | `object` | Resultado |

### NextLegResponse (hereda de NextNextLegResponse)

Respuesta completa del endpoint `GetNextLeg`. Ver detalle en [endpoints-flightcrews.md](./endpoints-flightcrews.md).

### NextNextLegResponse

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Source` | `FlightResponse` | Origen |
| `Destination` | `FlightResponse` | Destino |
| `Airline` | `string` | Aerolínea |
| `FlightNumber` | `string` | Número vuelo |
| `Passangers` | `int` | Total pasajeros |
| `Status` | `string` | Estado |
| `BussinessClass` | `int` | Clase ejecutiva |
| `SSR` | `int` | SSR |
| `Preferred` | `int` | Preferidos |
| `Meals` | `int` | Comidas |

### ParameterResponse

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `MinimumRequiredVersion` | `double` | Versión mínima |
| `AvailableGroups` | `string` | GUIDs separados por coma |
| `ValidateUserGroups` | `bool` | Validar grupos |
| `AutoUpdateEveryMinutes` | `int` | Auto-actualización |

### LogEntry

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Timestamp` | `DateTime` | Marca de tiempo |
| `Level` | `LogLevel` | Nivel de log |
| `Message` | `string` | Mensaje |

### LogLevel (enum)

`Trace (0)`, `Debug (1)`, `Information (2)`, `Warning (3)`, `Error (4)`, `Critical (5)`

---

## Relaciones

| Entidad padre | Entidad hija | Relación | FK |
|--------------|-------------|----------|-----|
| `IrregularOperation` | `Passanger` | 1:N | `IrregularOperationId` |
