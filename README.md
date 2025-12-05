# ?? RestaurantOrders API

API REST para gestión de pedidos de un restaurante, desarrollada con **.NET 8** siguiendo **Arquitectura Hexagonal** y principios **SOLID**.

## ?? Características

- **Gestión de Menú**: Sandwiches y extras con categorías
- **Sistema de Órdenes**: Creación, actualización y eliminación de pedidos
- **Descuentos Automáticos**: Combos con descuentos del 10%, 15% y 20%
- **Autenticación JWT**: Roles de Admin y Usuario
- **Validaciones de Negocio**: Control de duplicados en órdenes

## ??? Arquitectura

```
RestaurantOrders/
??? RestaurantOrders.Api/           # Capa de presentación (Controllers, Filters)
??? RestaurantOrders.Application/   # Casos de uso y DTOs
??? RestaurantOrders.Domain/        # Entidades, interfaces y reglas de negocio
??? RestaurantOrders.Infrastructure/# Repositorios y servicios externos
```

## ??? Menú Disponible

### Sandwiches
| Item | Precio |
|------|--------|
| Burger | $5.00 |
| Egg | $4.50 |
| Bacon | $7.00 |

### Extras
| Item | Precio |
|------|--------|
| Fries | $2.00 |
| Soft Drink | $2.50 |

## ?? Reglas de Descuento

| Combo | Descuento |
|-------|-----------|
| Sandwich + Fries + Soft Drink | **20%** |
| Sandwich + Soft Drink | **15%** |
| Sandwich + Fries | **10%** |

## ?? Endpoints

### ?? Públicos
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/auth/login` | Autenticación |
| `POST` | `/api/auth/seed-admin` | Crear admin inicial |
| `GET` | `/api/menu` | Listar todo el menú |
| `GET` | `/api/menu/sandwiches` | Listar sandwiches |
| `GET` | `/api/menu/extras` | Listar extras |
| `GET` | `/api/menu/{id}` | Obtener item por ID |

### ?? Requieren Autenticación
| Método | Endpoint | Descripción | Rol |
|--------|----------|-------------|-----|
| `POST` | `/api/orders` | Crear orden | Usuario |
| `GET` | `/api/orders` | Listar órdenes | Usuario/Admin |
| `GET` | `/api/orders/{id}` | Obtener orden | Owner/Admin |
| `PUT` | `/api/orders/{id}` | Actualizar orden | Owner/Admin |
| `DELETE` | `/api/orders/{id}` | Eliminar orden | Owner/Admin |
| `GET` | `/api/users/{id}/orders` | Órdenes de usuario | Owner/Admin |

### ??? Solo Admin
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/menu` | Crear item de menú |
| `PUT` | `/api/menu/{id}` | Actualizar item |
| `DELETE` | `/api/menu/{id}` | Desactivar item |
| `POST` | `/api/users` | Crear usuario |
| `GET` | `/api/users/{id}` | Obtener usuario |

## ?? Inicio Rápido

### Prerrequisitos
- .NET 8 SDK
- SQL Server
- Docker (opcional)

### Configuración

1. **Clonar el repositorio**
```bash
git clone https://github.com/Sagarzon1840/RestaurantOrders.git
cd RestaurantOrders
```

2. **Configurar conexión a base de datos**

Editar `RestaurantOrders.Api/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=RestaurantOrders;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "tu-clave-secreta-de-al-menos-32-caracteres",
    "Issuer": "restaurantorders",
    "Audience": "restaurantorders",
    "ExpirationMinutes": 50
  }
}
```

3. **Ejecutar migraciones**
```bash
dotnet ef migrations add InitialCreate --project RestaurantOrders.Infrastructure --startup-project RestaurantOrders.Api
dotnet ef database update --project RestaurantOrders.Infrastructure --startup-project RestaurantOrders.Api
```

4. **Ejecutar la aplicación**
```bash
cd RestaurantOrders.Api
dotnet run
```

5. **Acceder a Swagger**
```
https://localhost:7xxx/swagger
```

### Configuración Inicial

1. **Crear usuario Admin**
```http
POST /api/auth/seed-admin
```

2. **Login**
```http
POST /api/auth/login
Content-Type: application/json

{
    "username": "admin",
    "password": "Hola1234"
}
```

3. **Crear items del menú** (usar token de admin)
```http
POST /api/menu
Authorization: Bearer {token}
Content-Type: application/json

{
    "name": "Burger",
    "basePrice": 5.00,
    "category": 0,
    "subCategory": 0
}
```

## ?? Docker

```bash
docker build -t restaurant-orders .
docker run -p 8080:80 restaurant-orders
```

## ?? Ejemplos de Uso

### Crear una orden con combo completo (20% descuento)
```http
POST /api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
    "itemIds": ["{burger-id}", "{fries-id}", "{softdrink-id}"]
}
```

**Respuesta:**
```json
{
    "id": "...",
    "subtotal": 9.50,
    "discountApplied": 1.90,
    "total": 7.60,
    "items": [...]
}
```

### Validación de duplicados
```http
POST /api/orders
{
    "itemIds": ["{burger-id}", "{egg-id}"]
}
```

**Respuesta (400):**
```json
{
    "error": "An order can only contain one sandwich. Please remove the existing sandwich before adding a new one."
}
```

## ?? Tests

Ejecutar pruebas:
```bash
dotnet test
```

## ?? Tecnologías

- **.NET 8**
- **Entity Framework Core 8**
- **SQL Server**
- **JWT Authentication**
- **Swagger/OpenAPI**
- **AWS Lambda** (opcional)

## ?? Licencia

Este proyecto está bajo la Licencia MIT.

## ?? Autor

**Sagarzon1840**
- GitHub: [@Sagarzon1840](https://github.com/Sagarzon1840)