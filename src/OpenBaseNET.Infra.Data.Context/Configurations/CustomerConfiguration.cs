using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBaseNET.Domain.Entities;

namespace OpenBaseNET.Infra.Data.Context.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("clitab");

        builder.HasKey(c => c.Id)
            .HasName("pk_clitab");

        builder
            .Property(c => c.Id)
            .HasColumnName("cliid");
        
        builder
            .OwnsOne(
                c => c.Name, 
                name =>
            {
                    name.Property(n => n.Value)
                    .HasColumnName("clinm")
                    .HasMaxLength(255)
                    .IsRequired();
            });
    }
}