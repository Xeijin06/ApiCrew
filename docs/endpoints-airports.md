# Endpoints - Airports

**Controlador:** `AirportsController`  
**Ruta base:** `api/Airports`  
**Autorización:** Todos los endpoints requieren `[Authorize]`

---

## GET GetAllAirports

Obtiene la lista completa de aeropuertos.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/Airports/GetAllAirports` |
| **Autorización** | Requerida |
| **Parámetros** | Ninguno |
| **Respuesta exitosa** | `200 OK` - Lista de `Airport` |

### Modelo de respuesta: `Airport`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `AirportId` | `int` | Identificador único (PK) |
| `AirportCode` | `string` (3 chars) | Código IATA del aeropuerto |
| `AirportName` | `string` (max 50) | Nombre del aeropuerto |
| `AirportAbbreviation` | `string` (max 20) | Abreviación |
| `CountryCode` | `string` (2 chars) | Código ISO del país |
| `TimeZone` | `string` (max 50) | Zona horaria |
| `GTMOffset` | `int` | Offset GMT en horas |

---

## GET GetAirportById

Obtiene un aeropuerto por su ID.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/Airports/GetAirportById/{Id}` |
| **Autorización** | Requerida |
| **Parámetros** | `Id` (int, ruta) - ID del aeropuerto |
| **Respuesta exitosa** | `200 OK` - Objeto `Airport` |
| **Respuesta error** | `404 Not Found` - Si no existe |

---

## POST AddAirport

Agrega un nuevo aeropuerto.

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/Airports/AddAirport` |
| **Autorización** | Requerida |
| **Body** | Objeto `Airport` (JSON) |
| **Respuesta exitosa** | `200 OK` - Objeto creado con ID asignado |

### Body de ejemplo

```json
{
  "airportCode": "PTY",
  "airportName": "Tocumen International Airport",
  "airportAbbreviation": "Tocumen",
  "countryCode": "PA",
  "timeZone": "REPLACE",
  "gtmOffset": -5
}
```

---

## PUT UpdateAirport

Actualiza un aeropuerto existente.

| Campo | Valor |
|-------|-------|
| **URL** | `PUT api/Airports/UpdateAirport` |
| **Autorización** | Requerida |
| **Body** | Objeto `Airport` completo (JSON) |
| **Respuesta exitosa** | `204 No Content` |
| **Respuesta error** | `404 Not Found` - Si no existe |

---

## DELETE DeleteAirport

Elimina un aeropuerto por su ID.

| Campo | Valor |
|-------|-------|
| **URL** | `DELETE api/Airports/DeleteAirport/{Id}` |
| **Autorización** | Requerida |
| **Parámetros** | `Id` (int, ruta) - ID del aeropuerto |
| **Respuesta exitosa** | `204 No Content` |
