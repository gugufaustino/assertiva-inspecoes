using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Differencial.Repository.Mappings
{
    public class VistoriadorMap : IEntityTypeConfiguration<Vistoriador>
	{
        public void Configure(EntityTypeBuilder<Vistoriador> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).ValueGeneratedNever();
			builder.Property(t => t.IdOperador).IsRequired();
			builder.Property(t => t.IdEnderecoBase).IsRequired();
			builder.Property(t => t.IndEnderecoBaseIgual).IsRequired();
		}
    }
}