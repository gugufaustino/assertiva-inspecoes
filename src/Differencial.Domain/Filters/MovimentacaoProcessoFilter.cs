using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class MovimentacaoProcessoFilter: Pagination
	{ 
        public int? Id { get; set; }
		public string TextoMovimentacao { get; set; }
		public TipoSituacaoProcessoEnum? TipoSituacaoProcesso { get; set; }  
        public int? IdOperadorOrigem { get; set; }
		public int? IdOperadorDestino { get; set; }
		public DateTime? DthMovimentacao { get; set; }
		public DateTime? DthApropriacao { get; set; }
		public DateTime? DthConclusao { get; set; }

		public CampoOrdenacaoMovimentacaoProcesso CampoOrdenacao { get; set; }
	}
}