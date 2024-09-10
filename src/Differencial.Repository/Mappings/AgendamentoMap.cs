using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Differencial.Repository.Mappings
{
    public class AgendamentoMap : IEntityTypeConfiguration<Agendamento>
	{ 
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
           builder.HasKey(x => x.Id);
           builder.Property(t => t.Id).UseIdentityColumn();
           builder.Property(t => t.IdSolicitacao).IsRequired();
           builder.Property(t => t.IdVistoriador).IsRequired();
           builder.Property(t => t.DthAgendamento).IsRequired(false);
           builder.Property(t => t.IndCancelado).IsRequired();
           builder.Property(t => t.MotivoCancelamentoReagendamento).HasMaxLength(1000);
        }
    }
}