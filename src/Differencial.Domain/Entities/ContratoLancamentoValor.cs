using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Differencial.Domain.Entities
{
	public class ContratoLancamentoValor : IEntity
	{
		[Column("Id")]
		public int Id { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }

        [Column("IdContratoLancamento")]
		public int IdContratoLancamento { get; set; } 
         
		[Column("TipoQuantitativoVariacao")]
		public TipoQuantitativoVariacaoEnum TipoQuantitativoVariacao { get; set; }

		[Column("QuantitativoA")]
		public decimal? QuantitativoA { get; set; }

		[Column("QuantitativoB")]
		public decimal? QuantitativoB { get; set; }

		[Column("ValorLancamento")]
		public decimal? ValorLancamento { get; set; }

        [Column("ValorLancamentoQuantitativo")]
        public decimal? ValorLancamentoQuantitativo { get; set; }

        [Column("IndPreAcordo")]
        public bool IndPreAcordo { get; set; }

        [Column("SiglaUf")]
        public string SiglaUf { get; set; }

        #region "Relacionamentos"

        [ForeignKey("IdContratoLancamento")]
        public virtual ContratoLancamento ContratoLancamento { get; set; }
 
        #endregion

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new ContratoLancamentoValor();
			entidade.IdContratoLancamento = this.IdContratoLancamento;
		 
			entidade.TipoQuantitativoVariacao = this.TipoQuantitativoVariacao;
			entidade.QuantitativoA = this.QuantitativoA;
			entidade.QuantitativoB = this.QuantitativoB;
			entidade.ValorLancamento = this.ValorLancamento;
			return entidade;
		}
	}

	public enum CampoOrdenacaoContratoLancamentoValor
	{
	}
}