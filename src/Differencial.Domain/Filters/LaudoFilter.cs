using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class LaudoFilter: Pagination
	{

		public int? Id { get; set; }

		public CampoOrdenacaoLaudo CampoOrdenacao { get; set; }
	}
}