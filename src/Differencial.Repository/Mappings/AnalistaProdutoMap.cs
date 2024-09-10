using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class AnalistaProdutoMap : IEntityTypeConfiguration<AnalistaProduto>
	{
		 

        public void Configure(EntityTypeBuilder<AnalistaProduto> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.IndAtivo);
			builder.Property(t => t.QtdPontuacao);
		}
    }
}