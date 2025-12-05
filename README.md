# RestaurantOrders API

REST API for restaurant order management, developed with **.NET 8** following **Hexagonal Architecture** and **SOLID** principles.

## Features

- **Menu Management**: Sandwiches and extras with categories
- **Order System**: Create, update and delete orders
- **Automatic Discounts**: Combos with 10%, 15% and 20% discounts
- **JWT Authentication**: Admin and User roles
- **Business Validations**: Duplicate control in orders

## Architecture

```
RestaurantOrders/
??? RestaurantOrders.Api/           # Presentation layer (Controllers, Filters)
??? RestaurantOrders.Application/   # Use cases and DTOs
??? RestaurantOrders.Domain/        # Entities, interfaces and business rules
??? RestaurantOrders.Infrastructure/# Repositories and external services
```

## Available Menu

### Sandwiches
| Item | Price |
|------|-------|
| Burger | $5.00 |
| Egg | $4.50 |
| Bacon | $7.00 |

### Extras
| Item | Price |
|------|-------|
| Fries | $2.00 |
| Soft Drink | $2.50 |

## Discount Rules

| Combo | Discount |
|-------|----------|
| Sandwich + Fries + Soft Drink | **20%** |
| Sandwich + Soft Drink | **15%** |
| Sandwich + Fries | **10%** |

## Endpoints

### Public
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/login` | Authentication |
| `POST` | `/api/auth/seed-admin` | Create initial admin |
| `GET` | `/api/menu` | List all menu items |
| `GET` | `/api/menu/sandwiches` | List sandwiches |
| `GET` | `/api/menu/extras` | List extras |
| `GET` | `/api/menu/{id}` | Get item by ID |

### Require Authentication
| Method | Endpoint | Description | Role |
|--------|----------|-------------|------|
| `POST` | `/api/orders` | Create order | User |
| `GET` | `/api/orders` | List orders | User/Admin |
| `GET` | `/api/orders/{id}` | Get order | Owner/Admin |
| `PUT` | `/api/orders/{id}` | Update order | Owner/Admin |
| `DELETE` | `/api/orders/{id}` | Delete order | Owner/Admin |
| `GET` | `/api/users/{id}/orders` | User orders | Owner/Admin |

### Admin Only
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/menu` | Create menu item |
| `PUT` | `/api/menu/{id}` | Update item |
| `DELETE` | `/api/menu/{id}` | Deactivate item |
| `POST` | `/api/users` | Create user |
| `GET` | `/api/users/{id}` | Get user |

## Quick Start

### Prerequisites
- .NET 8 SDK
- SQL Server
- Docker (optional)

### Configuration

1. **Clone the repository**
```bash
git clone https://github.com/Sagarzon1840/RestaurantOrders.git
cd RestaurantOrders
```

2. **Configure database connection**

Edit `RestaurantOrders.Api/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=RestaurantOrders;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-at-least-32-characters",
    "Issuer": "restaurantorders",
    "Audience": "restaurantorders",
    "ExpirationMinutes": 50
  }
}
```

3. **Run migrations**
```bash
dotnet ef migrations add InitialCreate --project RestaurantOrders.Infrastructure --startup-project RestaurantOrders.Api
dotnet ef database update --project RestaurantOrders.Infrastructure --startup-project RestaurantOrders.Api
```

4. **Run the application**
```bash
cd RestaurantOrders.Api
dotnet run
```

5. **Access Swagger**
```
https://localhost:7xxx/swagger
```

### Initial Setup

1. **Create Admin user**
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

3. **Create menu items** (use admin token)
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

## Docker

```bash
docker build -t restaurant-orders .
docker run -p 8080:80 restaurant-orders
```

## Usage Examples

### Create order with full combo (20% discount)
```http
POST /api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
    "itemIds": ["{burger-id}", "{fries-id}", "{softdrink-id}"]
}
```

**Response:**
```json
{
    "id": "...",
    "subtotal": 9.50,
    "discountApplied": 1.90,
    "total": 7.60,
    "items": [...]
}
```

### Duplicate validation
```http
POST /api/orders
{
    "itemIds": ["{burger-id}", "{egg-id}"]
}
```

**Response (400):**
```json
{
    "error": "An order can only contain one sandwich. Please remove the existing sandwich before adding a new one."
}
```

## Tests

Run tests:
```bash
dotnet test
```

## Technologies

- **.NET 8**
- **Entity Framework Core 8**
- **SQL Server**
- **JWT Authentication**
- **Swagger/OpenAPI**
- **AWS Lambda** (optional)

## License

This project is under the MIT License.

## Author

**Sagarzon1840**
- GitHub: [@Sagarzon1840](https://github.com/Sagarzon1840)