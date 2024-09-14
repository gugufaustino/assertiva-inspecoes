using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Differencial.Repository.Mappings
{
    public class ContratoMap : IEntityTypeConfiguration<Contrato>
	{ 
        public void Configure(EntityTypeBuilder<Contrato> builder)
        {
           
            builder.Property(t => t.Id).ValueGeneratedNever();
           
            //builder
            //        .HasOne(c => c.Produto) // Configura a relação de chave estrangeira
            //        .WithMany() // Assume que Produto não possui uma coleção de Contratos, ajuste conforme necessário
            //        .HasForeignKey(c => c.Id) // Define que Id é a FK
            //        .OnDelete(DeleteBehavior.Restrict);

        }  
    }
}