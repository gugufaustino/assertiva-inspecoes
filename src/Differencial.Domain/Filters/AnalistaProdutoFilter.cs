using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class AnalistaProdutoFilter: Pagination
	{


		public CampoOrdenacaoAnalistaProduto CampoOrdenacao { get; set; }
	}
}