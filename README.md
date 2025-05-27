# Backend - Identity Management System

A clean architecture .NET 8.0 backend project with comprehensive Identity management, multi-tenancy support, and JWT authentication.

## 🏗️ Architecture

This project follows Clean Architecture principles with the following layers:

- **Domain** - Core entities and business logic
- **Shared** - Common utilities and shared components
- **Abstraction** - Service interfaces and DTOs
- **Application** - Application services and CQRS handlers
- **Infrastructure** - Data access and external services
- **Web.Host** - API controllers and web configuration
- **Migrators** - Database migration management

## 🚀 Features

### Identity Management
- **User Management** - Complete CRUD operations for users
- **Role Management** - Role-based access control
- **JWT Authentication** - Secure token-based authentication
- **Refresh Tokens** - Automatic token renewal
- **Multi-Factor Authentication** - Enhanced security support

### Multi-Tenancy
- **Finbuckle MultiTenant** - Complete tenant isolation
- **Tenant Management** - CRUD operations for tenants
- **Tenant-based Data Filtering** - Automatic data segregation

### Technical Stack
- **.NET 8.0** - Latest .NET framework
- **Entity Framework Core** - ORM with PostgreSQL
- **MediatR** - CQRS pattern implementation
- **Mapster** - Object mapping
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation

## 🛠️ Getting Started

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL database
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository:
```bash
git clone https://github.com/rahul143a/backend.git
cd backend
```

2. Restore packages:
```bash
dotnet restore
```

3. Update database connection string in `Web.Host/Configurations/database.json`

4. Run database migrations:
```bash
cd Migrators
dotnet run
```

5. Start the application:
```bash
cd Web.Host
dotnet run
```

The API will be available at `http://localhost:5000` with Swagger UI at `http://localhost:5000/swagger`

## 📚 API Endpoints

### Authentication
- `POST /api/tokens/login` - User login
- `POST /api/tokens/refresh` - Refresh access token

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### Roles
- `GET /api/roles` - Get all roles
- `GET /api/roles/{id}` - Get role by ID
- `POST /api/roles` - Create new role
- `PUT /api/roles/{id}` - Update role
- `DELETE /api/roles/{id}` - Delete role

### Tenants
- `GET /api/tenants` - Get all tenants
- `GET /api/tenants/{id}` - Get tenant by ID
- `POST /api/tenants` - Create new tenant
- `PUT /api/tenants/{id}` - Update tenant
- `DELETE /api/tenants/{id}` - Delete tenant

## 🔧 Configuration

### Database Configuration
Update the connection string in `Web.Host/Configurations/database.json`:

```json
{
  "DatabaseSettings": {
    "ConnectionString": "Host=localhost;Port=5432;Database=backend;Username=user;Password=password"
  }
}
```

### JWT Configuration
Configure JWT settings in `Web.Host/Configurations/jwt.json`:

```json
{
  "JwtSettings": {
    "Key": "your-secret-key",
    "Issuer": "Backend",
    "Audience": "Backend",
    "ExpirationInMinutes": 60,
    "RefreshTokenExpirationInDays": 7
  }
}
```

## 🧪 Testing

Run unit tests:
```bash
dotnet test
```

## 📝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 🤝 Support

For support and questions, please open an issue in the GitHub repository.
