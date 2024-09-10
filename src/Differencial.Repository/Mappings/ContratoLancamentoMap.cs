using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class ContratoLancamentoMap : IEntityTypeConfiguration<ContratoLancamento>
	{
		public ContratoLancamentoMap()
		{
			
        }

        public void Configure(EntityTypeBuilder<ContratoLancamento> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.IdContrato);
            builder.Property(t => t.TipoContratoLancamento);            
            builder.Property(t => t.TipoParametroQuantitativoVariavel);
        }
    }
}