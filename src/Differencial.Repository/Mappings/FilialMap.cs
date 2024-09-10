using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class FilialMap : IEntityTypeConfiguration<Filial>
	{
		public FilialMap()
		{

        }

        public void Configure(EntityTypeBuilder<Filial> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.IdSeguradora).IsRequired();
			builder.Property(t => t.NomeFilial).IsRequired().HasMaxLength(250);

            builder.HasOne(i => i.Seguradora)
                    .WithOne()
                    .HasForeignKey<Filial>(fk => fk.IdSeguradora);
        }
    }
}