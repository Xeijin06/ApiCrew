# Endpoints - SecuritySSRs

**Controlador:** `SecuritySSRsController`  
**Ruta base:** `api/SecuritySSRs`  
**Autorización:** Todos los endpoints requieren `[Authorize]`

---

## GET GetAllSecuritySSRs

| **URL** | `GET api/SecuritySSRs/GetAllSecuritySSRs` |
|---------|-------------------------------------------|
| **Respuesta** | `200 OK` - Lista de `SecuritySSR` |

## GET GetSecuritySSRById/{Id}

| **URL** | `GET api/SecuritySSRs/GetSecuritySSRById/{Id}` |
|---------|------------------------------------------------|
| **Respuesta** | `200 OK` / `404 Not Found` |

## POST AddSecuritySSR

| **URL** | `POST api/SecuritySSRs/AddSecuritySSR` |
|---------|----------------------------------------|
| **Body** | Objeto `SecuritySSR` (JSON) |

## PUT UpdateSecuritySSR

| **URL** | `PUT api/SecuritySSRs/UpdateSecuritySSR` |
|---------|------------------------------------------|

## DELETE DeleteSecuritySSR/{Id}

| **URL** | `DELETE api/SecuritySSRs/DeleteSecuritySSR/{Id}` |
|---------|--------------------------------------------------|

### Modelo `SecuritySSR`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `SecuritySSId` | `int` | PK |
| `Code` | `string` | Código SSR de seguridad |
