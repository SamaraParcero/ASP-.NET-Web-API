using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.EntitiesConfiguration
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(p => p.Nome).HasMaxLength(100).IsRequired();
            builder.Property(p => p.ImagemUrl).HasMaxLength(100).IsRequired();

            builder.HasData(
              new Categoria(1, "Material Escolar", "material.jpg"),
              new Categoria(2, "Eletrônicos", "eletronicos.jpg"),
              new Categoria(3, "Acessórios", "acessorios.jpg")
            );
        }
    }
}
