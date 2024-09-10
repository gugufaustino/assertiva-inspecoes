using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class CoberturaFilter: Pagination
	{

		public int? Id { get; set; }
		public string NomeCobertura { get; set; }
		public int? IdSolicitacao { get; set; }

		public CampoOrdenacaoCobertura CampoOrdenacao { get; set; }
	}
}