using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
	{
		 
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.CpfCnpj).HasMaxLength(14);
			builder.Property(t => t.NomeRazaoSocial).HasMaxLength(250);
			builder.Property(t => t.ContatoNome).HasMaxLength(250);
			builder.Property(t => t.ContatoTelefone).HasMaxLength(13);
			builder.Property(t => t.ContatoOutro).HasMaxLength(250);
			builder.Property(t => t.AtividadeNome).HasMaxLength(250);
			builder.Property(t => t.ContatoAgendamento).HasMaxLength(250);
		}
    }
}