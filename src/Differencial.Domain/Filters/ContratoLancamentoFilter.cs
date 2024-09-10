using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class ContratoLancamentoFilter: Pagination
	{

		public int? IdContrato { get; set; }
		public TipoContratoLancamentoEnum? TipoContratoLancamento { get; set; }

		public CampoOrdenacaoContratoLancamento CampoOrdenacao { get; set; }
	}
}