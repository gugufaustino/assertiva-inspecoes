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
            //        .HasOne(c => c.Produto) // Configura a rela��o de chave estrangeira
            //        .WithMany() // Assume que Produto n�o possui uma cole��o de Contratos, ajuste conforme necess�rio
            //        .HasForeignKey(c => c.Id) // Define que Id � a FK
            //        .OnDelete(DeleteBehavior.Restrict);

        }  
    }
}