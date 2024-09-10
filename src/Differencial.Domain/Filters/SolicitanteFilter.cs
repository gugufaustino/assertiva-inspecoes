using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class SolicitanteFilter: Pagination
	{

		public int? Id { get; set; }
		public TipoSolicitanteEnum? TipoSolicitante { get; set; }
		public CampoOrdenacaoSolicitante CampoOrdenacao { get; set; }
        public int? IdSeguradora { get; set; }
    }
}