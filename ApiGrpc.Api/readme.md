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
  - Descrição: Obtém todos os clientes.
  - Respostas:
    - 200: Retorna uma lista de `CustomerDto`.
    - 401: Não autorizado.

- GET /api/customers/{id}
  - Descrição: Obtém um cliente pelo ID.
  - Parâmetros:
    - `id` (Guid): ID do cliente.
  - Respostas:
    - 200: Retorna um `CustomerDto`.
    - 404: Cliente não encontrado.
    - 401: Não autorizado.

- POST /api/customers
  - Descrição: Cria um novo cliente.
  - Corpo da Requisição: `CreateCustomerDto`
  - Respostas:
    - 201: Cliente criado com sucesso.
    - 400: Requisição inválida.
    - 401: Não autorizado.

- PUT /api/customers/{id}
  - Descrição: Atualiza um cliente existente.
  - Parâmetros:
    - `id` (Guid): ID do cliente.
  - Corpo da Requisição: `UpdateCustomerDto`
  - Respostas:
    - 204: Cliente atualizado com sucesso.
    - 400: Requisição inválida.
    - 404: Cliente não encontrado.
    - 401: Não autorizado.

- POST /api/auth/register
  - Descrição: Registra um novo usuário.
  - Corpo da Requisição: `RegisterDto`
  - Respostas:
    - 201: Usuário registrado com sucesso.
    - 400: Requisição inválida.

- POST /api/auth/login
  - Descrição: Realiza login de um usuário.
  - Corpo da Requisição: `LoginDto`
  - Respostas:
    - 200: Retorna um token de autenticação.
    - 400: Requisição inválida.

- POST /api/auth/refresh
  - Descrição: Atualiza o token de autenticação.
  - Corpo da Requisição: `RefreshTokenDto`
  - Respostas:
    - 200: Retorna um novo token de autenticação.
    - 400: Requisição inválida.

- GET /api/auth/users
  - Descrição: Obtém todos os usuários.
  - Respostas:
    - 200: Retorna uma lista de `UserDto`.
    - 401: Não autorizado.