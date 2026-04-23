# Changelog - OpenBaseNET PostgreSQL

## Versão 1.0.0 - PostgreSQL Edition

### ✨ Mudanças Principais

#### Novos Projetos
- ✅ **OpenBaseNET.Infra.Resilience.Database.Pgsql**
  - Pipeline de resiliência específico para PostgreSQL
  - Tratamento de exceções PostgresException
  - Suporte a códigos SQLSTATE do PostgreSQL

#### Projetos Removidos
- ❌ **OpenBaseNET.Infra.Resilience.Database.Mssql**
  - Substituído pela versão PostgreSQL

### 📦 Dependências Atualizadas

#### Adicionados
- `Npgsql` 9.0.2
- `Npgsql.EntityFrameworkCore.PostgreSQL` 9.0.2

#### Removidos
- `Microsoft.Data.SqlClient` 7.0.0
- `Microsoft.EntityFrameworkCore.SqlServer` 10.0.6

### 🔧 Configurações Alteradas

#### Connection Strings
- Alterado de `OpenBaseSQLServer` para `OpenBasePostgres`
- Formato de connection string atualizado para PostgreSQL

#### DbContext
- `UseSqlServer()` → `UseNpgsql()`
- Configurações otimizadas para PostgreSQL

### 📝 Arquivos Modificados

#### Código Fonte
1. **OpenBaseNET.Infra.CrossCutting/Containers/DatabaseContainer.cs**
   - SqlConnection → NpgsqlConnection
   - Connection string atualizada

2. **OpenBaseNET.Infra.Data.Context/OneBaseDataBaseContext.cs**
   - UseSqlServer() → UseNpgsql()
   - Connection string key atualizada

3. **OpenBaseNET.Infra.Settings/ConnectionStrings/OneBaseConnectionStrings.cs**
   - Constante renomeada para OpenBasePostgres

4. **OpenBaseNET.Infra.Dapper.Extension/DapperExtension.cs**
   - Namespace atualizado para usar pipeline PostgreSQL

5. **OpenBaseNET.Infra.EF.Extension/EfExtension.cs**
   - Namespace atualizado para usar pipeline PostgreSQL

#### Arquivos de Configuração
1. **appsettings.json**
   - Connection string exemplo para PostgreSQL

2. **appsettings.Development.json**
   - Connection string de desenvolvimento para PostgreSQL

#### Arquivos de Projeto (.csproj)
1. **OpenBaseNET.Infra.Data.Context.csproj**
   - Microsoft.EntityFrameworkCore.SqlServer → Npgsql.EntityFrameworkCore.PostgreSQL

2. **OpenBaseNET.Infra.CrossCutting.csproj**
   - Adicionado pacote Npgsql

3. **OpenBaseNET.Infra.Dapper.Extension.csproj**
   - Referência de projeto atualizada para Pgsql

4. **OpenBaseNET.Infra.EF.Extension.csproj**
   - Referência de projeto atualizada para Pgsql

5. **OpenBaseNET.Infra.Resilience.Database.Pgsql.csproj**
   - Novo projeto com dependência Npgsql

#### Solution
- **OpenBasePgsql.sln**
  - Projeto Mssql substituído por Pgsql
  - GUIDs atualizados

### 📚 Documentação

#### Novos Arquivos
- **MIGRATION_GUIDE.md** - Guia completo de migração
- **README.md** - Atualizado para refletir PostgreSQL

### 🔄 Namespace Changes

Todos os namespaces foram atualizados:
- `OpenBaseNET.Infra.Resilience.Database.Mssql.*` → `OpenBaseNET.Infra.Resilience.Database.Pgsql.*`

### 🎯 Compatibilidade

- ✅ .NET 10.0
- ✅ Entity Framework Core 10.0.6
- ✅ PostgreSQL 12+
- ✅ Npgsql 9.0.2

### ⚡ Melhorias de Performance

- Pipeline de resiliência otimizado para erros transientes do PostgreSQL
- Tratamento específico para códigos SQLSTATE
- Suporte a recursos avançados do PostgreSQL (preparar em próximas versões)

### 🐛 Correções

- Todas as referências ao SQL Server foram removidas
- Connection strings corrigidas para PostgreSQL
- Códigos de erro específicos do PostgreSQL implementados

### 📋 Próximos Passos Sugeridos

- [ ] Configurar pgAdmin ou outra ferramenta de administração
- [ ] Revisar e otimizar índices para PostgreSQL
- [ ] Implementar suporte a recursos avançados (JSONB, Arrays, etc.)
- [ ] Configurar backup automatizado
- [ ] Implementar monitoring com pg_stat_statements
- [ ] Adicionar suporte a migrations automáticas

### 👥 Contribuidores

Template adaptado de OpenBaseNET SQLServer para PostgreSQL

---

**Data de Release:** Abril 2025
**Versão:** 1.0.0-pgsql
