using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class ContratoFilter: Pagination
	{

		public int? IdProduto { get; set; }
		public bool? IndRiscoDanos { get; set; }

		public CampoOrdenacaoContrato CampoOrdenacao { get; set; }
	}
}