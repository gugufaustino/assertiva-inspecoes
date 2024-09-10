using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class AgendamentoFilter: Pagination
	{

		public int? Id { get; set; }

		public CampoOrdenacaoAgendamento CampoOrdenacao { get; set; }
	}
}