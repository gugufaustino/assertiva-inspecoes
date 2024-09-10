using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class LancamentoFinanceiroMap : IEntityTypeConfiguration<LancamentoFinanceiro>
	{
		public LancamentoFinanceiroMap()
		{
			
		}

        public void Configure(EntityTypeBuilder<LancamentoFinanceiro> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.TipoLancamentoFinanceiro).IsRequired();
			builder.Property(t => t.ValorLancamentoFinanceiro).IsRequired();
			builder.Property(t => t.DescricaoLancamentoFinanceiro).IsRequired().HasMaxLength(250);
			builder.Property(t => t.IdSolicitacao);
		}
    }
}