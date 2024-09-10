using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Differencial.Repository.Mappings
{
    public class ArquivoAnexoMap : IEntityTypeConfiguration<ArquivoAnexo>
    {
        
        public void Configure(EntityTypeBuilder<ArquivoAnexo> builder)
        {
           builder.HasKey(x => x.Id);
           builder.Property(t => t.Id).UseIdentityColumn().UseIdentityColumn();
           builder.Property(t => t.IdSolicitacao).IsRequired();

            builder.Property(t => t.ArquivoNome).HasMaxLength(250);
            builder.Property(t => t.ArquivoExtencao).HasMaxLength(5);
            builder.Property(t => t.Descricao).HasMaxLength(1000);
        }
    }
}