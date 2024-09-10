using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class TipoAssuntoMap : IEntityTypeConfiguration<TipoAssunto>
	{
		public TipoAssuntoMap()
		{
			
		}

        public void Configure(EntityTypeBuilder<TipoAssunto> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.NomeAssunto).IsRequired().HasMaxLength(250);
			builder.Property(t => t.TextoPadrao).HasMaxLength(1000);
		}
    }
}