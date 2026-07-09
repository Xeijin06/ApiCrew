# Endpoints - Graph

**Controlador:** `GraphController`  
**Ruta base:** `api/Graph`

---

## GET GetUserInformation

Obtiene información de un usuario de Azure AD a través de Microsoft Graph API.

| Campo | Valor |
|-------|-------|
| **URL** | `GET api/Graph/GetUserInformation/{email}` |
| **Autorización** | Requerida |
| **Parámetros** | `email` (string, ruta) - Email del usuario |
| **Respuesta exitosa** | `200 OK` - Objeto JSON con datos del usuario de Graph |
| **Respuesta error** | `400 Bad Request` - Mensaje de error |

### Flujo interno

1. El `GraphService` obtiene un access token usando Client Credentials (`ClientId` + `ClientSecret`)
2. Consulta Microsoft Graph API: `GET https://graph.microsoft.com/{version}/users?$filter=mail eq '{email}'`
3. Retorna el JSON completo de la respuesta de Graph

### Ejemplo de respuesta

```json
{
  "@odata.context": "https://graph.microsoft.com/beta/$metadata#users",
  "value": [
	{
	  "displayName": "John Doe",
	  "mail": "john.doe@copa.com",
	  "employeeId": "1234",
	  "jobTitle": "Flight Attendant",
	  ...
	}
  ]
}
```

### Notas
- La versión de Graph API se configura en `appsettings.json` (`AzureAd:graph_version`), actualmente `beta`.
- Se utiliza el flujo Client Credentials (no delegado), lo que permite consultar cualquier usuario del tenant.
