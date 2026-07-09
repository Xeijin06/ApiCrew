# Endpoints - TimeZones

**Controlador:** `TimeZonesController`  
**Ruta base:** `api/TimeZones`

---

## POST (Cargar TimeZones)

Carga las zonas horarias de todos los aeropuertos registrados, consultando la API de Copa Airlines y actualizando los campos `GTMOffset` y `TimeZone` de cada aeropuerto en la base de datos.

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/TimeZones` |
| **Autorización** | **No requerida** |
| **Body** | Ninguno |
| **Respuesta exitosa** | `200 OK` - "Ok" |

### Flujo interno

1. Obtiene todos los aeropuertos de la base de datos
2. Llama a `ICopaAPIs.GetListTimeZone(airports)` para consultar zonas horarias
3. Deserializa la respuesta como `TimeZoneResponse`
4. Por cada aeropuerto:
   - Busca la zona horaria correspondiente por `LocationCode`
   - Extrae el offset GMT del campo `Offset` (formato `HH:mm`)
   - Actualiza `GTMOffset` y `TimeZone` (`IdTimeZone`) del aeropuerto
5. Guarda los cambios en la base de datos
