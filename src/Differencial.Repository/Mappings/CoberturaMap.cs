using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class CoberturaMap : IEntityTypeConfiguration<Cobertura>
	{ 
        public void Configure(EntityTypeBuilder<Cobertura> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.NomeCobertura).IsRequired().HasMaxLength(250);
			builder.Property(t => t.IdSolicitacao);
		}
	}
}