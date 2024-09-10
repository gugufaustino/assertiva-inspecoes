using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class ComunicacaoFilter: Pagination
	{
		public int? Id { get; set; }
		public int? IdTipoAssunto { get; set; }
		public TipoComunicacaoEnum? TipoComunicacao { get; set; }
		public string Assunto { get; set; }
		public string TextoComunicacao { get; set; }
		public int? IdSolicitacao { get; set; }

		public CampoOrdenacaoComunicacao CampoOrdenacao { get; set; }
	}
}