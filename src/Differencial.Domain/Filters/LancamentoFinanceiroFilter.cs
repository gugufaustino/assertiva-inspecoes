using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class LancamentoFinanceiroFilter: Pagination
	{


		public CampoOrdenacaoLancamentoFinanceiro CampoOrdenacao { get; set; }
	}
}