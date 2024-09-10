using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class ClienteEnderecoFilter: Pagination
	{

		public int? Id { get; set; }
		public int? IdCliente { get; set; }
		public int? IdEndereco { get; set; }

		public CampoOrdenacaoClienteEndereco CampoOrdenacao { get; set; }
	}
}