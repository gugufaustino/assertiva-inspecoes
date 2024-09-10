using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class ClienteFilter: Pagination
	{

		public int? Id { get; set; }
		public string CpfCnpj { get; set; }

		public CampoOrdenacaoCliente CampoOrdenacao { get; set; }
	}
}