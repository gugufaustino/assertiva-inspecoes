using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Entities;
using System;

namespace Differencial.Domain.Filters
{
	public class OperadorFilter: Pagination , IAtivavelFilter
	{

		public int? Id { get; set; }
		public string NomeOperador { get; set; }
		public int? TpSituacao { get; set; }
		public string Email { get; set; }
		public string Cpf { get; set; }
		public string Rg { get; set; }
		public DateTime? DataNascimento { get; set; }
		public bool? IndAnalista { get; set; }
		public bool? IndGerente { get; set; }
		public bool? IndVistoriador { get; set; }
		public bool? IndSolicitante { get; set; }
		public DateTime? DataCadastro { get; set; }
		public DateTime? DataModificacao { get; set; }

		public CampoOrdenacaoOperador CampoOrdenacao { get; set; }

        public bool? IndAtivo { get; set; }
        
    }
}