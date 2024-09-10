using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class OperadorMap : IEntityTypeConfiguration<Operador>
	{
        public void Configure(EntityTypeBuilder<Operador> builder)
        {
			builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).UseIdentityColumn();             
			builder.Property(t => t.NomeOperador).HasMaxLength(250);
			builder.Property(t => t.Email).HasMaxLength(250);
			builder.Property(t => t.Cpf).HasMaxLength(20);
			builder.Property(t => t.Rg).HasMaxLength(20);
			builder.Property(t => t.DataNascimento);			
			builder.Property(t => t.UrlFoto).HasMaxLength(50);
			builder.Property(t => t.IndAnalista).IsRequired(); 
			builder.Property(t => t.IndGerente).IsRequired();
            builder.Property(t => t.IndVistoriador).IsRequired();
            builder.Property(t => t.IndSolicitante).IsRequired();
            builder.Property(t => t.IdEndereco);
            builder.Property(t => t.Telefone).HasMaxLength(13);
            builder.Property(t => t.IndAcessoSistema).IsRequired();
            builder.Property(t => t.IndPrimeiroAcesso).IsRequired();
            builder.Property(t => t.Senha).HasMaxLength(250);
            builder.Property(t => t.SenhaConfirmacao).HasMaxLength(250);
        }

       
    }
}