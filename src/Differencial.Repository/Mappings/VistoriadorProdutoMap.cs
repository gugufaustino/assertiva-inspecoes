using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class VistoriadorProdutoMap : IEntityTypeConfiguration<VistoriadorProduto>
    {


        public void Configure(EntityTypeBuilder<VistoriadorProduto> builder)
        {

            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).UseIdentityColumn();
            
            builder.Property(t => t.IdVistoriador).IsRequired();
            builder.Property(t => t.IdProduto).IsRequired();
            builder.Property(t => t.IdContratoLancamento).IsRequired();
            builder.Property(t => t.IdContratoLancamentoValor).IsRequired();
            
            builder.Property(t => t.VlrPagamentoVistoria);
            builder.Property(t => t.VlrQuilometroRodado);
            builder.Property(t => t.IndAtivo).IsRequired();

            builder.HasIndex(p => new { p.IdVistoriador, p.IdProduto, p.IdContratoLancamento, p.IdContratoLancamentoValor })
                .HasDatabaseName("UK_VistoriadorProduto").IsUnique();


        }
    }
}