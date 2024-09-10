using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class VistoriadorProdutoFilter: Pagination
	{

		public int? Id { get; set; }
		public int? IdVistoriador { get; set; }

		public CampoOrdenacaoVistoriadorProduto CampoOrdenacao { get; set; }
	}
}