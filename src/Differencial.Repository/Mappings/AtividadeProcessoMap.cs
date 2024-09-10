using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Differencial.Repository.Mappings
{
    public class AtividadeProcessoMap : IEntityTypeConfiguration<AtividadeProcesso>
	{
		public void Configure(EntityTypeBuilder<AtividadeProcesso> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.NomeAtividadeProcesso).IsRequired().HasMaxLength(250);
			builder.Property(t => t.IdSolicitacao).IsRequired();
			builder.Property(t => t.IdOperadorConcluida);
			builder.Property(t => t.TipoSituacaoAtividade).IsRequired();
			builder.Property(t => t.DthAssinada);
			builder.Property(t => t.DthDelegada);
			builder.Property(t => t.DthConcluida);
			builder.Property(t => t.IndRetrabalho);
		}
	}
}