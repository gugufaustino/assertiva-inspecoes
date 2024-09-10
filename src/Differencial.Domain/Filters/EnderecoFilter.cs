using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class EnderecoFilter: Pagination
	{

		public int? Id { get; set; }
		public string Cep { get; set; }
		public string Logradouro { get; set; }
		public int? Numero { get; set; }
		public string Bairro { get; set; }
		public string NomeMunicipio { get; set; }
		public string SiglaUf { get; set; }

		public CampoOrdenacaoEndereco CampoOrdenacao { get; set; }
	}
}