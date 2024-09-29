using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class LancamentoFinanceiroTotalMap : IEntityTypeConfiguration<LancamentoFinanceiroTotal>
	{
		 

        public void Configure(EntityTypeBuilder<LancamentoFinanceiroTotal> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.IdSolicitacao).IsRequired();
			builder.Property(t => t.TipoLancamentoFinanceiro).IsRequired();
			builder.Property(t => t.ValorLancamentoFinanceiroTotal).IsRequired();
			builder.Property(t => t.DthLancamentoPagamento).IsRequired();
		
			builder.Property(t => t.IndLiquidado);
			builder.Property(t => t.IndFaturado);
		}
    }
}