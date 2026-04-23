# OpenBaseNET PostgreSQL Template

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)

> OpenBaseNET para PostgreSQL é um template para projetos .NET 10 usando base de dados PostgreSQL.

O template foi construído devido a necessidade de criar projetos de forma rápida e prática.
Um template de projeto .NET para acelerar o desenvolvimento de APIs, já configurado com Arquitetura Limpa, Entity Framework Core e PostgreSQL.

## Sobre o Projeto

Iniciar um novo projeto exige muita configuração repetitiva: estruturar as pastas, definir as camadas da aplicação, configurar o acesso a dados, etc.

Este template foi criado para eliminar essa etapa inicial. Com um único comando, você terá uma solução .NET completa e robusta, pronta para você focar no que realmente importa: as regras de negócio da sua aplicação.

## 🏛️ Estrutura da Arquitetura

O template utiliza os princípios da Clean Architecture para separar as responsabilidades de forma clara, garantindo um código organizado, testável e de fácil manutenção.

* **MinhaNovaApi.Domain:** A camada mais interna e o coração da aplicação. Contém as entidades de negócio, enums e as interfaces dos repositórios. Não depende de nenhuma outra camada.

* **MinhaNovaApi.Application:** Contém a lógica de negócio e os casos de uso (também conhecidos como "interactors"). Orquestra o fluxo de dados entre a apresentação e a infraestrutura, mas não conhece os detalhes de implementação de nenhum deles.

* **MinhaNovaApi.Infrastructure:** Implementa as abstrações definidas nas camadas internas. É aqui que reside o `DbContext` do Entity Framework, a implementação concreta dos repositórios e a integração com quaisquer outros serviços externos (como gateways de pagamento, envio de e-mails, etc.).

* **MinhaNovaApi.API (Presentation):** A camada de entrada e saída. Contém os Controllers da API, DTOs (Data Transfer Objects) e a configuração da inicialização do serviço (`Program.cs`). É a única camada que o usuário final "vê".

### Tecnologias Principais

* **.NET 10**
* **Entity Framework Core 10**
* **Npgsql - PostgreSQL provider for .NET**
* **Arquitetura Limpa (Clean Architecture)**
* **Padrão de Repositório (Repository Pattern)**
* **Pronto para PostgreSQL**

---

## 🚀 Como Usar

Para criar um novo projeto a partir deste template, siga os passos abaixo.

### Pré-requisitos

* [.NET SDK](https://dotnet.microsoft.com/download) (versão 10.0 ou superior).
* [PostgreSQL](https://www.postgresql.org/download/) instalado e configurado.

### 1. Configuração do Banco de Dados

Configure sua connection string no arquivo `appsettings.json` ou `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "OpenBasePostgres": "Host=localhost;Port=5432;Database=OpenBaseNet;Username=postgres;Password=sua_senha"
  }
}
```

### 2. Rodando o Projeto

Rode o projeto e a API estará pronta para uso.

```bash
dotnet run --project src/OpenBaseNET.Presentation.Api/OpenBaseNET.Presentation.Api.csproj 
```

### 3. Modelo a ser seguido

O Projeto vem com uma classe que mapeia uma entidade chamada cliente.
Não é necessário para rodar seu projeto, serve apenas como Guia e pode ser excluído sem problemas.

## 📦 Pacotes Principais

- **Npgsql** - PostgreSQL data provider for .NET
- **Npgsql.EntityFrameworkCore.PostgreSQL** - Entity Framework Core provider for PostgreSQL
- **Entity Framework Core** - ORM para acesso a dados
- **Dapper** - Micro ORM para consultas performáticas
- **MediatR** - Implementação do padrão Mediator
- **AutoMapper** - Mapeamento objeto-objeto
- **Serilog** - Logging estruturado
- **Polly** - Biblioteca de resiliência e tratamento de falhas transientes

## Agradecimentos

Grato a você que se interessou pelo projeto.

### Feedbacks são sempre bem vindos

Rodrigo S. Brito <rodrigo@w3ti.com.br>

