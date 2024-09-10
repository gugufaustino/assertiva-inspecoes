using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class ContratoLancamentoValorFilter: Pagination
	{

		public int? IdContratoLancamento { get; set; }
		public bool? IndParametroQuantitativoVariavel { get; set; }
		public int? TipoParametro { get; set; }
		public TipoQuantitativoVariacaoEnum? TipoQuantitativoVariacao { get; set; }

		public CampoOrdenacaoContratoLancamentoValor CampoOrdenacao { get; set; }
	}
}