using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class NotificacaoMap : IEntityTypeConfiguration<Notificacao>
	{ 

        public void Configure(EntityTypeBuilder<Notificacao> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.Descricao).HasMaxLength(500);
		}
    }
}