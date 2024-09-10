using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Differencial.Repository.Mappings
{
    public class SolicitanteMap : IEntityTypeConfiguration<Solicitante>
	{
		public SolicitanteMap()
		{
			
		}

        public void Configure(EntityTypeBuilder<Solicitante> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).ValueGeneratedNever();
			builder.Property(t => t.IdOperador).IsRequired();
			builder.Property(t => t.TipoSolicitante);
			builder.Property(t => t.IdSeguradora);
		}
    }
}