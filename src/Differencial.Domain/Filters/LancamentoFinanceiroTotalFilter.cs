using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class LancamentoFinanceiroTotalFilter: Pagination
	{


		public CampoOrdenacaoLancamentoFinanceiroTotal CampoOrdenacao { get; set; }
	}
}