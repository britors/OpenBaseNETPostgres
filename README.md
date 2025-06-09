# OpenBaseNET para Postgresql
![GitHub repo size](https://img.shields.io/github/repo-size/britors/OpenBaseNETPostgres)
![GitHub top language](https://img.shields.io/github/languages/top/britors/OpenBaseNETPostgres)
![GitHub language count](https://img.shields.io/github/languages/count/britors/OpenBaseNETPostgres)
![GitHub last commit](https://img.shields.io/github/last-commit/britors/OpenBaseNETPostgres)
![GitHub issues](https://img.shields.io/github/issues/britors/OpenBaseNETPostgres)
![GitHub](https://img.shields.io/github/license/britors/OpenBaseNETPostgres)
![GitHub forks](https://img.shields.io/github/forks/britors/OpenBaseNETPostgres?style=social)
![GitHub Repo stars](https://img.shields.io/github/stars/britors/OpenBaseNETPostgres?style=social)
![GitHub watchers](https://img.shields.io/github/watchers/britors/OpenBaseNETPostgres?style=social)
![GitHub followers](https://img.shields.io/github/followers/britors?style=social)


![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Postgresql](https://img.shields.io/badge/Postgresql-C?style=for-the-badge&logo=postgresql&logoColor=white)

![img.png](img.png)

> OpenBaseNET para Postgresql é um template para projetos .net 9 usando base de dados Postgresql.
O template foi construído devido a necessidade de criar projetos  forma rápida e prática.

## Para criar um projeto, basta seguir os passos abaixo:

#### Crie seu projeto usando o template OpenBaseNET <br/>
![image](https://github.com/britors/OpenBaseNETSqlServer/assets/183213/aaade65c-e31e-4dfb-ac4f-2d3e85e2d8a5)


#### Baixe seu projeto para sua máquina <br/>
```bash
git clone <projeto>
```
#### Dentro da Pasta _01-Presentation_, acesse o arquivo appsettings.json e altere a string de conexão para a sua base de dados <br/>
```json
{
  "ConnectionStrings": {
    "OpenBasePostgresql": "Server=127.0.0.1;Port=5432;Database=openbase;User Id=openbase;Password=openbase;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
#### No projeto OpenBaseNET.Application acesse a pasta Entities e crie suas classes para representar as suas entidades (existe um modelo chamado Customer, use como exemplo) <br/>
   É extemamente importante que a classe implemente a interface _IEntityOrQueryResult_ <br/>
```csharp
namespace OpenBaseNET.Domain.Entities;

public sealed class Customer : IEntityOrQueryResult
{
    public int Id { get; set; }
    public Name Name { set; get; } = null!;
 
}
```
#### No Projeto OpenBaseNET.Infra.Data.Context acesse a pasta Configurations e crie a classe de mapeamento da sua entidade (existe um modelo chamado CustomerMapping, use como exemplo) <br/>
```csharp
namespace OpenBaseNET.Infra.Data.Context.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("CLITAB");

        builder.HasKey(c => c.Id)
            .HasName("PK_CLITAB");

        builder
            .Property(c => c.Id)
            .HasColumnName("CLIID");
        
        builder
            .OwnsOne(
                c => c.Name, 
                name =>
            {
                    name.Property(n => n.Value)
                    .HasColumnName("CLINM")
                    .HasMaxLength(255)
                    .IsRequired();
            });
    }
}
```

# Pronto!
>A partir de agora você pode criar suas classes de serviço, repositório e controlador para sua entidade <br/>
Caso você siga o padrão de nomenclatura do template não precisará fazer nenhuma configuração adicional <br/>
