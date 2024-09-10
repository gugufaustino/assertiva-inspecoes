using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Entities
{
	public class ContratoLancamento : IEntity
	{

        public ContratoLancamento()
        {
            ContratoLancamentoValor = new HashSet<ContratoLancamentoValor>();
        }

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

        [Column("IdContrato")]
		public int IdContrato { get; set; }

		[Column("TipoContratoLancamento")]
		public TipoContratoLancamentoEnum TipoContratoLancamento { get; set; }

        [Column("TipoParametroQuantitativoVariavel")]
        public TipoContratoParametroEnum TipoParametroQuantitativoVariavel { get; set; }
        
        #region "Relacionamentos"

        [ForeignKey("IdContrato")]
        public virtual Contrato Contrato { get; set; }

        public virtual ICollection<ContratoLancamentoValor> ContratoLancamentoValor { get; set; }


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
			var entidade = new ContratoLancamento();
			entidade.IdContrato = this.IdContrato;
			entidade.TipoContratoLancamento = this.TipoContratoLancamento;
			return entidade;
		}
	}

	public enum CampoOrdenacaoContratoLancamento
	{
	}
}