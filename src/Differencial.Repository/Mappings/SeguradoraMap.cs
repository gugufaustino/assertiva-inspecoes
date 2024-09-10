using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class SeguradoraMap : IEntityTypeConfiguration<Seguradora>
	{
		public SeguradoraMap()
		{
			 
        }

        public void Configure(EntityTypeBuilder<Seguradora> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.IdEndereco);
            builder.Property(t => t.NomeSeguradora).IsRequired().HasMaxLength(250);
            builder.Property(t => t.Cnpj).HasMaxLength(18);
            builder.Property(t => t.Inscricao).HasMaxLength(250);
            builder.Property(t => t.RazaoSocial).HasMaxLength(1000);
            builder.Property(t => t.EmailRemetenteSolicitacao).HasMaxLength(250);
            builder.Property(t => t.IndAgendaRepostaPorEmail).IsRequired();
            builder.Property(t => t.IndLaudoRepostaPorEmail).IsRequired();
            builder.Property(t => t.IndIntegracaoSolicitacaoPorEmail).IsRequired();

            // 1 : N => 
            builder.HasMany(f => f.Filial)
                   .WithOne(p => p.Seguradora)
                   .HasForeignKey(prop => prop.IdSeguradora);

        }
    }
}