using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Differencial.Repository.Mappings
{
    public class ContratoLancamentoValorMap : IEntityTypeConfiguration<ContratoLancamentoValor>
	{
 
        public void Configure(EntityTypeBuilder<ContratoLancamentoValor> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.IdContratoLancamento);
			builder.Property(t => t.TipoQuantitativoVariacao);
			builder.Property(t => t.QuantitativoA);
			builder.Property(t => t.QuantitativoB);
			builder.Property(t => t.ValorLancamento);
		}
    }
}