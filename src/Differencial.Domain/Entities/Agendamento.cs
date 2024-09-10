using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(AgendamentoMetadata))]
    public class Agendamento : IEntity
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

        [Column("IdSolicitacao")]
		public int IdSolicitacao { get; set; }

		[Column("IdVistoriador")]
		public int IdVistoriador { get; set; }

		[Column("DthAgendamento")]
		public DateTime? DthAgendamento { get; set; }

		[Column("IndCancelado")]
		public bool IndCancelado { get; set; }

        [Column("TipoAgendamento")]
        public TipoAgendamentoEnum TipoAgendamento { get; set; }

        [Column("MotivoCancelamentoReagendamento")]
        public string MotivoCancelamentoReagendamento { get; set; }

        [ForeignKey("IdVistoriador")]
        public virtual Vistoriador Vistoriador { get; set; }

        [ForeignKey("IdSolicitacao")]
        public virtual Solicitacao Solicitacao { get; set; }

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required
			if(IdSolicitacao == 0)
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, "Agendamento", "Solicitação");
            if (IdVistoriador == 0)
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, "Agendamento", "Vistoriador"); ;
			//if(DthAgendamento.IsValid() == false)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DthAgendamentoInvalido);

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Agendamento();
			entidade.IdSolicitacao = this.IdSolicitacao;
			entidade.IdVistoriador = this.IdVistoriador;
			entidade.DthAgendamento = this.DthAgendamento;
			entidade.IndCancelado = this.IndCancelado;
			return entidade;
		}
	}

	public enum CampoOrdenacaoAgendamento
	{
		DthAgendamento,
	}
}