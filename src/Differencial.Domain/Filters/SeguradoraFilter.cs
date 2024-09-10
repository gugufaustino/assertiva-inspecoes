using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class SeguradoraFilter: Pagination
	{

		public int? Id { get; set; }
		public DateTime? DataCadastro { get; set; }
		public DateTime? DataModificacao { get; set; }

        public bool? IndIntegracaoSolicitacaoPorEmail { get; set; }

        public bool? IndAtivo { get; set; }

        public CampoOrdenacaoSeguradora CampoOrdenacao { get; set; }
	}
}