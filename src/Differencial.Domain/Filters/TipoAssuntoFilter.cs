using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class TipoAssuntoFilter: Pagination
	{

		public int? Id { get; set; }
		public string NomeAssunto { get; set; }

		public CampoOrdenacaoTipoAssunto CampoOrdenacao { get; set; }
	}
}