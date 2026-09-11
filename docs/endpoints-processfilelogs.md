# Endpoints - ProcessFileLogs

**Controlador:** `ProcessFileLogsController`  
**Ruta base:** `api/ProcessFileLogs`  
**Autorización:** Todos los endpoints requieren `[Authorize]`

---

## GET GetAllProcessFileLogs

| **URL** | `GET api/ProcessFileLogs/GetAllProcessFileLogs` |
|---------|--------------------------------------------------|
| **Respuesta** | `200 OK` - Lista de `ProccessFileLog` |

## GET GetProcessFileLogById/{Id}

| **URL** | `GET api/ProcessFileLogs/GetProcessFileLogById/{Id}` |
|---------|------------------------------------------------------|

## POST AddProcessFileLog

| **URL** | `POST api/ProcessFileLogs/AddProcessFileLog` |
|---------|-----------------------------------------------|

## PUT UpdateProcessFileLog

| **URL** | `PUT api/ProcessFileLogs/UpdateProcessFileLog` |
|---------|------------------------------------------------|

## DELETE DeleteProcessFileLog/{Id}

| **URL** | `DELETE api/ProcessFileLogs/DeleteProcessFileLog/{Id}` |
|---------|--------------------------------------------------------|

### Modelo `ProccessFileLog`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `ProccessFileLogId` | `int` | PK |
| `Date` | `DateTime` | Fecha del log (formato: `yyyy/MM/dd HH:mm:ss`) |
| `WasSuccess` | `bool` | Si el procesamiento fue exitoso |
| `Steps` | `string` | Descripción de los pasos ejecutados |
