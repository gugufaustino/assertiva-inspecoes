using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
	{
	  
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.IdSeguradora).IsRequired();
			builder.Property(t => t.IdTipoInspecao).IsRequired();
			builder.Property(t => t.NomeProduto).IsRequired().HasMaxLength(250);
			builder.Property(t => t.NomeProdutoSeguradora).HasMaxLength(250);
			builder.Property(t => t.IndFranquiaQuilometragem);
			builder.Property(t => t.QtdFranquiaQuilometragem);
			builder.Property(t => t.VlrDespesa);
			builder.Property(t => t.VlrReceber);
			builder.Property(t => t.VlrQuilometragem);
			builder.Property(t => t.IndBlocoExtra);
			builder.Property(t => t.VlrBlocoExtr);
			builder.Property(t => t.IndQuilometragemVariavel);
			builder.Property(t => t.QtdQuilometragemFinal);
			builder.Property(t => t.QtdQuilometragemInicial);
			builder.Property(t => t.VlrQuilometragemReceber);
			builder.Property(t => t.IndMetragemVariavel);
			builder.Property(t => t.QtdMetragemM2Inicial);
			builder.Property(t => t.QtdMetragemM2Final);
			builder.Property(t => t.VlrMetragemReceber);
		}
    }
}