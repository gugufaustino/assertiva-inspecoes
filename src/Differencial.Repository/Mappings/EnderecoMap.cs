using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Differencial.Repository.Mappings
{
    public class EnderecoMap : IEntityTypeConfiguration<Endereco>
	{
		 
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
			builder.HasKey(x => x.Id);
			builder.Property(t => t.Id).UseIdentityColumn();
			builder.Property(t => t.Cep).HasMaxLength(9);
			builder.Property(t => t.Logradouro).HasMaxLength(250);
			builder.Property(t => t.Numero);
			builder.Property(t => t.Complemento).HasMaxLength(80);
			builder.Property(t => t.Bairro).HasMaxLength(80);
			builder.Property(t => t.NomeMunicipio).HasMaxLength(80);
			builder.Property(t => t.SiglaUf).HasMaxLength(2);
			builder.Property(t => t.Latitude);
			builder.Property(t => t.Longitude);
		}
    }
}