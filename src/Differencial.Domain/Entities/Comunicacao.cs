using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;

namespace Differencial.Domain.Entities
{

    [ModelMetadataType(typeof(ComunicacaoMetadata))]
    public class Comunicacao : IEntity
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

        [Column("IdTipoAssunto")]
		public int? IdTipoAssunto { get; set; }

		[Column("TipoComunicacao")]
		public TipoComunicacaoEnum TipoComunicacao { get; set; }

		[Column("Assunto")]
		public string Assunto { get; set; }

		[Column("TextoComunicacao")]
		public string TextoComunicacao { get; set; }

		[Column("IdSolicitacao")]
		public int IdSolicitacao { get; set; }

        [ForeignKey("IdSolicitacao")]
        public virtual Solicitacao Solicitacao { get; set; }
         
        [ForeignKey("IdOperadorCadastro")]
        public virtual Operador Operador { get; set; }

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

           
            if (IdSolicitacao == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido);

            // Optional
            if (Assunto.IsNullOrEmpty() == false && Assunto.Length > 250)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoTamanhoMaximoInvalido.Formata("Assunto", "250"));
            if (TextoComunicacao.IsNullOrEmpty() == false && TextoComunicacao.Length > 1000)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoTamanhoMaximoInvalido.Formata("Texto Comunicação", "1000"));
            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException();
        }

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Comunicacao();
			entidade.IdTipoAssunto = this.IdTipoAssunto;
			entidade.TipoComunicacao = this.TipoComunicacao;
			entidade.Assunto = this.Assunto;
			entidade.TextoComunicacao = this.TextoComunicacao;
			entidade.IdSolicitacao = this.IdSolicitacao;
			return entidade;
		}
	}

	public enum CampoOrdenacaoComunicacao
	{
		Id,
		IdTipoAssunto,
		IdTipoComunicacao,
		Assunto,
		TextoComunicacao,
		IdSolicitacao,
	}
}