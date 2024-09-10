using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class ComunicacaoMap : IEntityTypeConfiguration<Comunicacao>
	{
	 

        public void Configure(EntityTypeBuilder<Comunicacao> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.TipoComunicacao).IsRequired();
			builder.Property(t => t.IdSolicitacao).IsRequired();
			builder.Property(t => t.IdTipoAssunto);
			builder.Property(t => t.Assunto).HasMaxLength(250);
			builder.Property(t => t.TextoComunicacao).HasMaxLength(1000);
		}
    }
}