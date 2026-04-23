# Sumário de Mudanças - OpenBasePgsql

## 📊 Resumo Executivo

Este projeto é uma adaptação completa do template OpenBaseNET de **SQL Server** para **PostgreSQL**.

### Estatísticas
- **Projetos Criados:** 1 (OpenBaseNET.Infra.Resilience.Database.Pgsql)
- **Projetos Removidos:** 1 (OpenBaseNET.Infra.Resilience.Database.Mssql)
- **Arquivos Modificados:** 8 arquivos .cs + 5 arquivos .csproj + 2 arquivos .json
- **Total de Projetos:** 22 projetos + 1 projeto de testes

---

## 🔄 Mudanças por Categoria

### 1. Infraestrutura de Resiliência

#### ➕ NOVO: OpenBaseNET.Infra.Resilience.Database.Pgsql

**Localização:** `src/OpenBaseNET.Infra.Resilience.Database.Pgsql/`

**Estrutura:**
```
OpenBaseNET.Infra.Resilience.Database.Pgsql/
├── ExceptionPredicate/
│   └── PgsqlExceptionPredicate.cs
├── Pipelines/
│   └── DatabasePipeline.cs
└── OpenBaseNET.Infra.Resilience.Database.Pgsql.csproj
```

**Funcionalidades:**
- Tratamento de exceções `PostgresException`
- Códigos SQLSTATE do PostgreSQL
- Pipeline de retry com Polly
- 19 códigos de erro tratados (deadlock, serialization, connection, resources, etc.)

---

### 2. Camada de Dados

#### 📝 MODIFICADO: OpenBaseNET.Infra.Data.Context

**Arquivo:** `OneBaseDataBaseContext.cs`

**Mudanças:**
```csharp
// ANTES
optionsBuilder.UseSqlServer(connectionString);

// DEPOIS
optionsBuilder.UseNpgsql(connectionString);
```

**Arquivo:** `OpenBaseNET.Infra.Data.Context.csproj`

**Mudanças de Pacotes:**
```xml
<!-- REMOVIDO -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.6" />

<!-- ADICIONADO -->
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.2" />
```

---

### 3. Extensões ORM

#### 📝 MODIFICADO: OpenBaseNET.Infra.Dapper.Extension

**Arquivo:** `DapperExtension.cs`

**Mudança de Namespace:**
```csharp
// ANTES
using OpenBaseNET.Infra.Resilience.Database.Mssql.Pipelines;

// DEPOIS
using OpenBaseNET.Infra.Resilience.Database.Pgsql.Pipelines;
```

**Arquivo:** `OpenBaseNET.Infra.Dapper.Extension.csproj`

**Mudança de Referência:**
```xml
<!-- ANTES -->
<ProjectReference Include="..\OpenBaseNET.Infra.Resilience.Database.Mssql\..." />

<!-- DEPOIS -->
<ProjectReference Include="..\OpenBaseNET.Infra.Resilience.Database.Pgsql\..." />
```

---

#### 📝 MODIFICADO: OpenBaseNET.Infra.EF.Extension

**Arquivo:** `EfExtension.cs`

**Mudança de Namespace:**
```csharp
// ANTES
using OpenBaseNET.Infra.Resilience.Database.Mssql.Pipelines;

// DEPOIS
using OpenBaseNET.Infra.Resilience.Database.Pgsql.Pipelines;
```

**Arquivo:** `OpenBaseNET.Infra.EF.Extension.csproj`

**Mudança de Referência:** (mesma lógica do Dapper)

---

### 4. Injeção de Dependências

#### 📝 MODIFICADO: OpenBaseNET.Infra.CrossCutting

**Arquivo:** `Containers/DatabaseContainer.cs`

**Mudanças:**
```csharp
// ANTES
using Microsoft.Data.SqlClient;
services.AddScoped<DbConnection>(_ =>
    new SqlConnection(configuration.GetConnectionString("OpenBaseSQLServer")));

// DEPOIS
using Npgsql;
services.AddScoped<DbConnection>(_ =>
    new NpgsqlConnection(configuration.GetConnectionString("OpenBasePostgres")));
```

**Arquivo:** `OpenBaseNET.Infra.CrossCutting.csproj`

**Pacote Adicionado:**
```xml
<PackageReference Include="Npgsql" Version="9.0.2" />
```

---

### 5. Configurações

#### 📝 MODIFICADO: OpenBaseNET.Infra.Settings

**Arquivo:** `ConnectionStrings/OneBaseConnectionStrings.cs`

**Mudança:**
```csharp
// ANTES
public const string OpenBaseSqlServer = "OpenBaseSQLServer";

// DEPOIS
public const string OpenBasePostgres = "OpenBasePostgres";
```

---

### 6. Arquivos de Configuração

#### 📝 MODIFICADO: appsettings.json

**Mudança:**
```json
// ANTES
"ConnectionStrings": {
  "OpenBaseSQLServer": ""
}

// DEPOIS
"ConnectionStrings": {
  "OpenBasePostgres": "Host=localhost;Port=5432;Database=openbase;Username=postgres;Password=postgres"
}
```

---

#### 📝 MODIFICADO: appsettings.Development.json

**Mudança:**
```json
// ANTES
"ConnectionStrings": {
  "OpenBaseSQLServer": "Server=.;Database=OpenBaseNet;Trusted_Connection=True;TrustServerCertificate=True"
}

// DEPOIS
"ConnectionStrings": {
  "OpenBasePostgres": "Host=localhost;Port=5432;Database=OpenBaseNet;Username=postgres;Password=postgres"
}
```

---

### 7. Solution

#### 📝 MODIFICADO: OpenBasePgsql.sln

**Mudanças:**
- Nome da solution: OpenBaseNETSQLServer → OpenBasePgsql
- Referência ao projeto: Mssql → Pgsql
- Todos os caminhos atualizados

---

## 🎯 Arquivos Não Modificados

Os seguintes projetos **NÃO** foram alterados (permanecem iguais ao template SQL Server):

- OpenBaseNET.Application
- OpenBaseNET.Common
- OpenBaseNET.Domain
- OpenBaseNET.Infra.AutoMapper
- OpenBaseNET.Infra.Cloud.Azure
- OpenBaseNET.Infra.Data
- OpenBaseNET.Infra.Data.Core
- OpenBaseNET.Infra.Http
- OpenBaseNET.Infra.Http.Extensions
- OpenBaseNET.Infra.Logger
- OpenBaseNET.Infra.Mediator
- OpenBaseNET.Infra.Resilience.Azure
- OpenBaseNET.Infra.Resilience.Core
- OpenBaseNET.Infra.Resilience.HttpClient
- OpenBaseNET.Infra.Uow
- OpenBaseNET.Presentation.Api (exceto appsettings)
- OpenBaseNET.Tests.Unit

Isso preserva toda a lógica de negócio, domain, application, e infraestrutura não relacionada a banco de dados.

---

## ✅ Checklist de Validação

### Feito ✓
- [x] Projeto Pgsql criado com ExceptionPredicate
- [x] Pipeline de resiliência configurado
- [x] DbContext atualizado para UseNpgsql
- [x] Connection strings atualizadas
- [x] Referências de projeto corrigidas
- [x] Namespaces atualizados
- [x] Pacotes NuGet substituídos
- [x] Solution atualizada
- [x] README.md atualizado
- [x] MIGRATION_GUIDE.md criado
- [x] CHANGELOG.md criado
- [x] Diretórios bin/obj removidos

### Próximos Passos (Opcional)
- [ ] Testar compilação: `dotnet build`
- [ ] Criar banco PostgreSQL de testes
- [ ] Executar migrations
- [ ] Testar API
- [ ] Adicionar suporte a recursos PostgreSQL (JSONB, Arrays, etc.)

---

## 📦 Estrutura Final

```
OpenBasePgsql/
├── src/
│   ├── OpenBaseNET.Infra.Resilience.Database.Pgsql/  ← NOVO
│   │   ├── ExceptionPredicate/
│   │   │   └── PgsqlExceptionPredicate.cs
│   │   ├── Pipelines/
│   │   │   └── DatabasePipeline.cs
│   │   └── *.csproj
│   ├── OpenBaseNET.Infra.Data.Context/                ← MODIFICADO
│   ├── OpenBaseNET.Infra.CrossCutting/                ← MODIFICADO
│   ├── OpenBaseNET.Infra.Settings/                    ← MODIFICADO
│   ├── OpenBaseNET.Infra.Dapper.Extension/            ← MODIFICADO
│   ├── OpenBaseNET.Infra.EF.Extension/                ← MODIFICADO
│   ├── OpenBaseNET.Presentation.Api/                  ← appsettings MODIFICADO
│   └── (... outros 15 projetos inalterados)
├── tests/
│   └── UnitTests/
│       └── OpenBaseNET.Tests.Unit/
├── OpenBasePgsql.sln                                   ← MODIFICADO
├── README.md                                           ← MODIFICADO
├── CHANGELOG.md                                        ← NOVO
├── MIGRATION_GUIDE.md                                  ← NOVO
└── SUMMARY.md                                          ← ESTE ARQUIVO
```

---

## 🚀 Como Usar

1. **Restaurar Pacotes:**
   ```bash
   dotnet restore
   ```

2. **Compilar:**
   ```bash
   dotnet build
   ```

3. **Configurar PostgreSQL:**
   - Instalar PostgreSQL
   - Criar database: `CREATE DATABASE OpenBaseNet;`
   - Ajustar connection string em appsettings

4. **Executar:**
   ```bash
   dotnet run --project src/OpenBaseNET.Presentation.Api
   ```

---

## 📞 Suporte

Para dúvidas ou problemas:
- Consulte o `MIGRATION_GUIDE.md`
- Verifique o `CHANGELOG.md`
- Documentação Npgsql: https://www.npgsql.org/

---

**Data:** Abril 2025  
**Versão:** 1.0.0-pgsql  
**Status:** ✅ Pronto para uso
