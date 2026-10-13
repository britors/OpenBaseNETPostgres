# OpenBaseNET PostgreSQL Template

A .NET 10 API template with PostgreSQL, EF Core, Dapper, MediatR, FluentValidation,
AutoMapper, Serilog and Polly. Distributed as `w3ti.OpenBaseNET.Postgres.Template`.

## Requirements

- .NET SDK 10.0.401 (see `global.json`).
- PostgreSQL; integration tests run against PostgreSQL 18.

Package versions are maintained in `Directory.Packages.props`. Preview packages
are not used. Application references its own library dependencies directly;
infrastructure adapters are wired by the composition root in `Infra.CrossCutting`.

## Create an application

```bash
dotnet new install w3ti.OpenBaseNET.Postgres.Template
dotnet new openbasenet-pgsql --name MyApi
cd MyApi
dotnet restore MyApi.sln
dotnet tool restore
```

The generated solution, projects, namespaces, reflection registration strings and
migration metadata use your application name.

## Run the repository

Set the connection string through an environment variable (or local configuration):

```bash
export ConnectionStrings__OpenBasePostgres='Host=localhost;Port=5432;Database=openbase;Username=postgres;Password=your_password'
dotnet tool restore
dotnet ef database update \
  --project src/OpenBaseNET.Infra.Data.Context \
  --startup-project src/OpenBaseNET.Presentation.Api
dotnet run --project src/OpenBaseNET.Presentation.Api
```

When running a generated application, substitute its name for `OpenBaseNET` in
these paths. Apply migrations explicitly during deployment. The API does not
create or migrate the database on startup. In Development, Swagger is available
at `/swagger` (the launch profile uses HTTPS port 7215 and HTTP port 5039).

The example exposes:

- `POST /api/customers`, with JSON `{ "name": "Ada Lovelace" }`: creates a customer.
- `GET /api/customers/{id}`: reads a customer, or returns 404.
- `GET /api/customers?pageNumber=1&pageSize=20`: lists customers ordered by name and ID.

Validation failures return 422 with `application/problem+json`. Pages start at 1;
the example limits page size to 100. These example endpoints are public. Configure
an authentication scheme and authorization policies before adding protected data.
Configure `Mediator:LicenseKey` and `AutoMapper:LicenseKey` according to your use of
those libraries; the template does not provision license keys.

## Data access and transactions

`DbSession` and EF Core share the same scoped `DbConnection`. Connections open
only when needed. The DI scope owns the connection; the unit of work owns its
transaction. Do not run concurrent operations on the same scoped context/session.

```csharp
await unitOfWork.BeginTransactionAsync(cancellationToken);
try
{
    // EF changes and Dapper commands using session.Transaction share one transaction.
    await unitOfWork.CommitAsync(cancellationToken);
}
catch
{
    await unitOfWork.RollbackAsync(CancellationToken.None);
    throw;
}
```

`CommitAsync` saves pending EF changes. Commit, rollback and disposal release the
transaction and clear tracked entities, allowing a new transaction in the same
scope. Disposing an uncommitted unit of work rolls it back. Repositories still
save each write; group multiple writes in an explicit unit of work for atomicity.
Do not retry an individual command in an aborted transaction: restart the entire
operation with a new unit of work.

Repository pagination requires both a positive page number and page size, plus
an explicit deterministic ordering. Include a unique tie-breaker:

```csharp
await repository.FindAsync(cancellationToken, noTracking: true,
    pageNumber: 1, pageSize: 20,
    orderBy: query => query.OrderBy(x => x.Name).ThenBy(x => x.Id));
```

Dapper receives cancellation through `CommandDefinition`. HTTP GET, PUT and
DELETE helpers retry transient responses (408, 429 and 5xx) and transport errors.
POST retries require explicit `allowRetry: true`; enable this only for an operation
whose server supports an idempotency key. Cancellation is never retried.

## Verification

```bash
dotnet test OpenBasePgsql.sln --configuration Release
```

Tests cover API startup, validation, mediator registration, pagination and HTTP
retries. PostgreSQL tests are explicitly skipped unless the following variable
is set to a disposable test server with permission to create databases:

```bash
export OPENBASE_TEST_POSTGRES='Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres'
bash scripts/validate-template.sh
```

Each PostgreSQL test creates and drops its own database. Tests verify migrations,
EF/Dapper commit and rollback, disposal rollback, in-flight command cancellation,
and the customer API against the real database.

The validation script builds and tests the repository, packages the template,
installs it into an isolated template hive, generates `RenamedApi`, verifies naming
and package contents, then builds and tests that generated solution. The validated
NuGet package is copied to `artifacts/packages`. This script belongs to the template
repository and is not included in generated applications.

CI provides PostgreSQL and runs the entire validation before publishing. Pull
requests validate only; pushes to `main` publish the validated artifact using the
configured NuGet secret.

## License

The template is MIT licensed. Dependencies retain their respective licenses.
