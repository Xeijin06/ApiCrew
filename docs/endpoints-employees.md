# Endpoints - Employees

**Controlador:** `EmployeesController`  
**Ruta base:** `api/Employees`  
**Autorización:** Todos los endpoints requieren `[Authorize]`

---

## GET GetAllEmployees

Obtiene la lista completa de empleados.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/Employees/GetAllEmployees` |
| **Respuesta exitosa** | `200 OK` - Lista de `Employee` |

### Modelo de respuesta: `Employee`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `EmployeeId` | `int` | Identificador único (PK) |
| `Email` | `string` | Email corporativo del empleado |
| `Code` | `int` | Código/ID de empleado en el sistema |

---

## GET GetEmployeeById

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/Employees/GetEmployeeById/{Id}` |
| **Parámetros** | `Id` (int, ruta) |
| **Respuesta exitosa** | `200 OK` - Objeto `Employee` |
| **Respuesta error** | `404 Not Found` |

---

## POST AddEmployee

| Campo | Valor |
|-------|-------|
| **URL** | `POST api/Employees/AddEmployee` |
| **Body** | Objeto `Employee` (JSON) |
| **Respuesta exitosa** | `200 OK` - Objeto creado |

```json
{
  "email": "john.doe@copa.com",
  "code": 1234
}
```

---

## PUT UpdateEmployee

| Campo | Valor |
|-------|-------|
| **URL** | `PUT api/Employees/UpdateEmployee` |
| **Body** | Objeto `Employee` completo (JSON) |
| **Respuesta exitosa** | `204 No Content` |
| **Respuesta error** | `404 Not Found` |

---

## DELETE DeleteEmployee

| Campo | Valor |
|-------|-------|
| **URL** | `DELETE api/Employees/DeleteEmployee/{Id}` |
| **Parámetros** | `Id` (int, ruta) |
| **Respuesta exitosa** | `204 No Content` |
