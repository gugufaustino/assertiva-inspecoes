using Differencial.Domain.Entities;

namespace Differencial.Domain.Filters
{
    public class SolicitacaoFilter: Pagination
	{

		public int? Id { get; set; }
		public int? IdSeguradora { get; set; }
		public int? IdProduto { get; set; }
		public int? IdVistoriador { get; set; } 
        public TipoSituacaoProcessoEnum? TpSituacao { get; set; }

        public CampoOrdenacaoSolicitacao CampoOrdenacao { get; set; }
        public int? IdSolicitante { get; set; }
    }
}