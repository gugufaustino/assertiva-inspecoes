using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class TipoInspecaoFilter: Pagination
	{

		public int? Id { get; set; }
		public string Nome { get; set; }
		public bool? IndAtivo { get; set; }

		public CampoOrdenacaoTipoInspecao CampoOrdenacao { get; set; }
	}
}