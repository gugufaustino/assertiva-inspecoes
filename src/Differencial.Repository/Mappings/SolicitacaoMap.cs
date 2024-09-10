using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class SolicitacaoMap : IEntityTypeConfiguration<Solicitacao>
	{
		public SolicitacaoMap()
		{
			
        }

        public void Configure(EntityTypeBuilder<Solicitacao> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.IdSeguradora).IsRequired();
            builder.Property(t => t.IdProduto).IsRequired();
            builder.Property(t => t.IdVistoriador);
            builder.Property(t => t.IdEnderecoCliente).IsRequired();
            builder.Property(t => t.IdCliente);
            builder.Property(t => t.CodSeguradora).HasMaxLength(50);
            builder.Property(t => t.TpSituacao);
            builder.Property(t => t.IdAnalista);
            builder.Property(t => t.TxtInformacoesAdicionais).HasMaxLength(1000);
            builder.Property(t => t.CorretorNome).HasMaxLength(250);
            builder.Property(t => t.CorretorTelefone).HasMaxLength(13);
            builder.Property(t => t.VistoriadorCidadeBase).HasMaxLength(100);             
            builder.Property(t => t.TxtJustificativaVistoriadorDefinido).HasMaxLength(1000);
            builder.Property(t => t.TxtJustificativaDeslocamentoRealizado).HasMaxLength(1000);
            builder.Property(t => t.IndRotaDeVolta).IsRequired();
            builder.Property(t => t.IndUrgente).IsRequired();
            builder.Property(t => t.IdFilial);
            builder.Property(t => t.ControleDthEmailCobrancaVistoria);

            // 1 : N =>
            builder.HasMany(f => f.AtividadeProcesso)
                   .WithOne(p => p.Solicitacao)
                   .HasForeignKey(prop => prop.IdSolicitacao);
            
            builder.HasMany(f => f.MovimentacaoProcesso)
                   .WithOne(p => p.Solicitacao)
                   .HasForeignKey(prop => prop.IdSolicitacao);
        }
    }
}