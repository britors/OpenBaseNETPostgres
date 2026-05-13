# OpenBaseNET PostgreSQL Template

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)

> OpenBaseNET for PostgreSQL is a template for .NET 10 projects using a PostgreSQL database.

The template was built to address the need to create projects quickly and efficiently.
A .NET project template to accelerate API development, pre-configured with Clean Architecture, Entity Framework Core, and PostgreSQL.

## About the Project

Starting a new project requires a lot of repetitive configuration: structuring folders, defining application layers, setting up data access, etc.

This template was created to eliminate that initial setup step. With a single command, you'll have a complete and robust .NET solution, ready for you to focus on what really matters: your application's business rules.

## 🏛️ Architecture Structure

The template uses Clean Architecture principles to clearly separate responsibilities, ensuring organized, testable, and maintainable code.

* **MinhaNovaApi.Domain:** The innermost layer and the heart of the application. Contains business entities, enums, and repository interfaces. It does not depend on any other layer.

* **MinhaNovaApi.Application:** Contains the business logic and use cases (also known as "interactors"). Orchestrates the data flow between the presentation and infrastructure layers, but has no knowledge of their implementation details.

* **MinhaNovaApi.Infrastructure:** Implements the abstractions defined in the inner layers. This is where the Entity Framework `DbContext` lives, along with the concrete repository implementations and integrations with any external services (such as payment gateways, email sending, etc.).

* **MinhaNovaApi.API (Presentation):** The input/output layer. Contains API Controllers, DTOs (Data Transfer Objects), and the service startup configuration (`Program.cs`). It is the only layer the end user "sees."

### Main Technologies

* **.NET 10**
* **Entity Framework Core 10**
* **Npgsql - PostgreSQL provider for .NET**
* **Clean Architecture**
* **Repository Pattern**
* **PostgreSQL-ready**

---

## 🚀 How to Use

To create a new project from this template, follow the steps below.

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download) (version 10.0 or higher).
* [PostgreSQL](https://www.postgresql.org/download/) installed and configured.

### 1. Database Configuration

Configure your connection string in the `appsettings.json` or `appsettings.Development.json` file:

```json
{
  "ConnectionStrings": {
    "OpenBasePostgres": "Host=localhost;Port=5432;Database=OpenBaseNet;Username=postgres;Password=your_password"
  }
}
```

### 2. Running the Project

Run the project and the API will be ready to use.

```bash
dotnet run --project src/OpenBaseNET.Presentation.Api/OpenBaseNET.Presentation.Api.csproj 
```

### 3. Sample Model

The project comes with a class that maps an entity called Customer.
It is not required to run your project — it serves only as a guide and can be deleted without any issues.

## 📦 Main Packages

- **Npgsql** - PostgreSQL data provider for .NET
- **Npgsql.EntityFrameworkCore.PostgreSQL** - Entity Framework Core provider for PostgreSQL
- **Entity Framework Core** - ORM for data access
- **Dapper** - Micro ORM for high-performance queries
- **MediatR** - Mediator pattern implementation
- **AutoMapper** - Object-to-object mapping
- **Serilog** - Structured logging
- **Polly** - Resilience and transient fault handling library

## Acknowledgments

Thank you for your interest in this project.

### Feedback is always welcome

Rodrigo S. Brito <rodrigo@w3ti.com.br>
