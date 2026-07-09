# Endpoints - Descriptors

**Controlador:** `DescriptorsController`  
**Ruta base:** `api/Descriptors`  
**Autorización:** Todos los endpoints requieren `[Authorize]`

---

## GET GetAllDescriptors

| **URL** | `GET api/Descriptors/GetAllDescriptors` |
|---------|----------------------------------------|
| **Respuesta** | `200 OK` - Lista de `Descriptor` |

## GET GetDescriptorById/{Id}

| **URL** | `GET api/Descriptors/GetDescriptorById/{Id}` |
|---------|----------------------------------------------|
| **Respuesta** | `200 OK` / `404 Not Found` |

## POST AddDescriptor

| **URL** | `POST api/Descriptors/AddDescriptor` |
|---------|--------------------------------------|
| **Body** | Objeto `Descriptor` (JSON) |

## PUT UpdateDescriptor

| **URL** | `PUT api/Descriptors/UpdateDescriptor` |
|---------|----------------------------------------|

## DELETE DeleteDescriptor/{Id}

| **URL** | `DELETE api/Descriptors/DeleteDescriptor/{Id}` |
|---------|------------------------------------------------|

### Modelo `Descriptor`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `DescriptorId` | `int` | PK |
| `Name` | `string` (max 50) | Nombre del descriptor |
| `Abbreviation` | `string` (max 20) | Abreviación |
| `Order` | `int` | Orden de visualización |
