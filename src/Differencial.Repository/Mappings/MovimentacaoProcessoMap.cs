using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class MovimentacaoProcessoMap : IEntityTypeConfiguration<MovimentacaoProcesso>
	{
		public MovimentacaoProcessoMap()
		{
		}

        public void Configure(EntityTypeBuilder<MovimentacaoProcesso> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.TextoMovimentacao).HasMaxLength(1000);
			builder.Property(t => t.TipoSituacaoProcesso).IsRequired();
			builder.Property(t => t.TipoSituacaoMovimento).IsRequired();
			builder.Property(t => t.IdOperadorOrigem).IsRequired();
			builder.Property(t => t.DthMovimentacao).IsRequired();
			builder.Property(t => t.IdOperadorDestino);
			builder.Property(t => t.DthApropriacao);
			builder.Property(t => t.DthConclusao);
		}
    }
}