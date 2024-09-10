using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class FilialFilter: Pagination
	{

		public int? Id { get; set; }
		public int? IdSeguradora { get; set; }
		public string NomeFilial { get; set; }

		public CampoOrdenacaoFilial CampoOrdenacao { get; set; }
	}
}