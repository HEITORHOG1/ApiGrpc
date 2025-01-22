# Arquitetura DDD/CQRS com gRPC

Implementação de API gRPC seguindo DDD (Domain-Driven Design) e CQRS (Command Query Responsibility Segregation).

## Estrutura 

### Domain
```
ProductService.Domain/
├── Entities/         # Entidades e classes base
├── Events/           # Eventos de domínio
├── ValueObjects/     # Objetos de valor
├── Enums/           # Enumeradores
├── Exceptions/      # Exceções personalizadas
├── Specifications/  # Regras de especificação
├── Services/        # Serviços de domínio
└── Repositories/    # Interfaces de repositórios
```

### Application
```
ProductService.Application/
├── Commands/         # Comandos CQRS
│   ├── AddProduct/                   
│   └── UpdateProduct/                
├── Queries/          # Consultas CQRS
│   ├── GetProductById/               
│   └── GetAllProducts/               
├── DTOs/             # DTOs
├── Mappings/         # AutoMapper
├── Behaviors/        # MediatR behaviors
├── Validations/      # FluentValidation
├── EventHandlers/    # Handlers de eventos
└── Interfaces/       # Interfaces de serviços
```

### Infrastructure
```
ProductService.Infrastructure/
├── Persistence/      # EF Core
│   ├── Configurations/ # Fluent API
│   └── Migrations/     # Migrações
├── Repositories/     # Implementações
├── Services/         # Serviços externos
├── Logging/          # Logs
├── MessageBus/       # Mensageria
└── Context/          # DbContext
```

### API
```
ProductService.Api/
├── Controllers/      # Controllers REST
├── Middlewares/      # Middlewares
├── Filters/          # Filtros
├── Extensions/       # Extensões
├── Swagger/          # Configuração Swagger
└── Program.cs        # Configuração da aplicação
```

### Shared & Tests
```
ProductService.Shared/
├── Constants/        # Constantes
└── Extensions/       # Extensões utilitárias

ProductService.Tests/
├── UnitTests/        # Testes unitários
└── IntegrationTests/ # Testes integrados
```

## Tecnologias
- .NET 8
- gRPC
- Entity Framework Core
- SQLite
- MediatR
- AutoMapper
- FluentValidation
- xUnit

## Setup
```bash
dotnet restore
dotnet ef database update
dotnet run --project ProductService.Api
```

## Endpoints
- gRPC: https://localhost:7252
- Swagger: https://localhost:7252/swagger

## Serviços Disponíveis
### gRPC
- GetCustomer
- GetAllCustomers
- CreateCustomer
- UpdateCustomer

### REST
- GET /api/customers
- GET /api/customers/{id}
- POST /api/customers
- PUT /api/customers/{id}
