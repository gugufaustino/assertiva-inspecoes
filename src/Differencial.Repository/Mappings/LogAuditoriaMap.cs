using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class LogAuditoriaMap : IEntityTypeConfiguration<LogAuditoria>
    {
        

        public void Configure(EntityTypeBuilder<LogAuditoria> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).UseIdentityColumn();
            builder.HasIndex(x => x.Tabela).HasDatabaseName("IX_Tabela");
            builder.Property(t => t.Tabela).IsRequired().HasMaxLength(128);
            builder.Property(t => t.IdTabela).IsRequired();
            builder.Property(t => t.Acao).IsRequired().HasMaxLength(1);
            builder.Property(t => t.DataAcao).IsRequired();
            builder.Property(t => t.XMLDadosAnterior);
            builder.Property(t => t.XMLDadosPosterior);
            builder.Property(t => t.UsuarioBanco).HasMaxLength(128);
            
            builder.Property(t => t.UsuarioAplicacao);
            //builder.HasOne(i => i.Operador).WithMany()
            //        .HasForeignKey(fk => fk.UsuarioAplicacao);
        }
    }
}