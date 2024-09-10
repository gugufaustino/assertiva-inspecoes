using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class NotificacaoOperadorMap : IEntityTypeConfiguration<NotificacaoOperador>
	{
		public NotificacaoOperadorMap()
		{
			
		}

        public void Configure(EntityTypeBuilder<NotificacaoOperador> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.IndLido);
			builder.Property(t => t.IdNotificacao);
			builder.Property(t => t.IdOperador);
		}
    }
}