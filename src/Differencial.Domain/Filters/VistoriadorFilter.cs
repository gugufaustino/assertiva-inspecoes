using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class VistoriadorFilter: Pagination
	{

		public int? Id { get; set; }
		public int? IdOperador { get; set; }

		public CampoOrdenacaoVistoriador CampoOrdenacao { get; set; }
	}
}