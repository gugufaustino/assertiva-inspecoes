using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Differencial.Repository.Mappings
{
    public class LaudoFotoMap : IEntityTypeConfiguration<LaudoFoto>
	{
		public LaudoFotoMap()
		{
			

		}

        public void Configure(EntityTypeBuilder<LaudoFoto> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();
            builder.Property(t => t.IdLaudo);
        }
    }
}