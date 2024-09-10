using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class TipoInspecaoMap : IEntityTypeConfiguration<TipoInspecao>
	{
        public void Configure(EntityTypeBuilder<TipoInspecao> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.NomeTipoInspecao).IsRequired().HasMaxLength(250);
			builder.Property(t => t.DescricaoTipoInspecao).HasMaxLength(1000);
		}
	}
}