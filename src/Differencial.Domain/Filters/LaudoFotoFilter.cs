using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class LaudoFotoFilter: Pagination
	{

		public int? Id { get; set; }

		public CampoOrdenacaoLaudoFoto CampoOrdenacao { get; set; }
	}
}