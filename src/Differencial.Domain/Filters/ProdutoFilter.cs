using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class ProdutoFilter: Pagination
	{

		public int? Id { get; set; }
		public int? IdSeguradora { get; set; }
		public int? IdTipoInspecao { get; set; }
		public string NomeProduto { get; set; }

		public CampoOrdenacaoProduto CampoOrdenacao { get; set; }
	}
}