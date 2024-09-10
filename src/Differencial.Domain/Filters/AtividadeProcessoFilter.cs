using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class AtividadeProcessoFilter: Pagination
	{

		public int? Id { get; set; }
		public string NomeAtividadeProcesso { get; set; }
		public int? IdSolicitacao { get; set; }
		public int? IdMovimentacaoProcesso { get; set; }
		public int? IdOperadorConcluida { get; set; }
		public TipoSituacaoAtividadeEnum? TipoSituacaoAtividade { get; set; }
		public DateTime? DthAssinada { get; set; }
		public DateTime? DthDelegada { get; set; }
		public DateTime? DthConcluida { get; set; }
		public bool? IndRetrabalho { get; set; }

		public CampoOrdenacaoAtividadeProcesso CampoOrdenacao { get; set; }
	}
}