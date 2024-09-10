using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
	public class AnalistaMap : IEntityTypeConfiguration<Analista>
	{
		public void Configure(EntityTypeBuilder<Analista> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(t => t.IndAtivo);
		}
	}
}