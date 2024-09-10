using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class ClienteEnderecoMap : IEntityTypeConfiguration<ClienteEndereco>
	{
        public void Configure(EntityTypeBuilder<ClienteEndereco> builder)
        {
           builder.HasKey(x => x.Id);
           builder.Property(t => t.Id).UseIdentityColumn();
           builder.Property(t => t.IdCliente).IsRequired();
           builder.Property(t => t.IdEndereco).IsRequired();
        }
    }
}