using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class LaudoMap : IEntityTypeConfiguration<Laudo>
	{
		public LaudoMap()
		{
			
		}

        public void Configure(EntityTypeBuilder<Laudo> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.IdSolicitacao).IsRequired();
		}
    }
}