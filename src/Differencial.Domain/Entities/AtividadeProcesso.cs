using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Differencial.Domain.Entities
{
	public class AtividadeProcesso : IEntity
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

        [Column("NomeAtividadeProcesso")]
		public string NomeAtividadeProcesso { get; set; }

		[Column("IdSolicitacao")]
		public int IdSolicitacao { get; set; }

		[Column("IdOperadorConcluida")]
		public int? IdOperadorConcluida { get; set; }

        [Column("TipoAtividade")]
        public TipoAtividadeEnum TipoAtividade { get; set; }

        [Column("TipoSituacaoAtividade")]
		public TipoSituacaoAtividadeEnum TipoSituacaoAtividade { get; set; }

		[Column("DthAssinada")]
		public DateTime? DthAssinada { get; set; }

		[Column("DthDelegada")]
		public DateTime? DthDelegada { get; set; }

		[Column("DthConcluida")]
		public DateTime? DthConcluida { get; set; }         

        [Column("IndRetrabalho")]
		public bool? IndRetrabalho { get; set; }

        [ForeignKey("IdSolicitacao")]
        public virtual Solicitacao Solicitacao { get; set; }

        [ForeignKey("IdOperadorConcluida")]
        public virtual Operador OperadorConcluida { get; set; }


        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required
			if(NomeAtividadeProcesso.IsNullOrEmpty() || NomeAtividadeProcesso.Length > 250)
				validationResultsManager.AddValidationResultNotValid(String.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Nome Atividade Processo", "250"));
 
			// Optional
			if(DthAssinada.HasValue && DthAssinada.Value.IsValid() == false)
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DataInvalida, "Data Assinada");
			if(DthDelegada.HasValue && DthDelegada.Value.IsValid() == false)
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DataInvalida, "Data Delegada");
			if(DthConcluida.HasValue && DthConcluida.Value.IsValid() == false)
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DataInvalida, "Data Concluida");
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new AtividadeProcesso();
			entidade.NomeAtividadeProcesso = this.NomeAtividadeProcesso;
			entidade.IdSolicitacao = this.IdSolicitacao;
			entidade.IdOperadorConcluida = this.IdOperadorConcluida;
			entidade.TipoSituacaoAtividade = this.TipoSituacaoAtividade;
			entidade.DthAssinada = this.DthAssinada;
			entidade.DthDelegada = this.DthDelegada;
			entidade.DthConcluida = this.DthConcluida;
			entidade.IndRetrabalho = this.IndRetrabalho;
			return entidade;
		}
	}

	public enum CampoOrdenacaoAtividadeProcesso
	{
		NomeAtividadeProcesso,
		IdSolicitacao, 
		IdOperadorDestino,
		TipoSituacaoAtividade,
		DthAssinada,
		DthDelegada,
		DthConcluida,
		IndRetrabalho,
	}
}