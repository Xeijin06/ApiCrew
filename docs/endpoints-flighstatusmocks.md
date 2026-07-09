# Endpoints - FlighStatusMocks

**Controlador:** `FlighStatusMocksController`  
**Ruta base:** `api/FlighStatusMocks`  
**Autorización:** Todos los endpoints requieren `[Authorize]`

---

## GET GetAllFlighStatusMocks

| **URL** | `GET api/FlighStatusMocks/GetAllFlighStatusMocks` |
|---------|---------------------------------------------------|
| **Respuesta** | `200 OK` - Lista de `FlighStatusMock` |

## GET GetFlighStatusMockById/{Id}

| **URL** | `GET api/FlighStatusMocks/GetFlighStatusMockById/{Id}` |
|---------|--------------------------------------------------------|

## POST AddFlighStatusMock

| **URL** | `POST api/FlighStatusMocks/AddFlighStatusMock` |
|---------|------------------------------------------------|

## PUT UpdateFlighStatusMock

| **URL** | `PUT api/FlighStatusMocks/UpdateFlighStatusMock` |
|---------|--------------------------------------------------|

## DELETE DeleteFlighStatusMock/{Id}

| **URL** | `DELETE api/FlighStatusMocks/DeleteFlighStatusMock/{Id}` |
|---------|----------------------------------------------------------|

### Modelo `FlighStatusMock`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `FlighStatusMockId` | `int` | PK |
| `FlighNumber` | `int` | Número de vuelo |
| `Date` | `DateTime` | Fecha del vuelo |
| `Status` | `string` | Estado: `ON-TIME`, `DELAYED`, `CANCELLED` |
