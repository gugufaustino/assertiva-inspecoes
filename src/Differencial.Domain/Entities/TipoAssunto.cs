using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Differencial.Domain.EntitiesMetadata;
using Microsoft.AspNetCore.Mvc;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(TipoAssuntoMetadata))]
    public class TipoAssunto : IEntity
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

        [Column("NomeAssunto")]
		public string NomeAssunto { get; set; }

		[Column("TextoPadrao")]
		public string TextoPadrao { get; set; }


		// Valida os dados da entidade
		public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			//// Required
			//if(NomeAssunto.IsNullOrEmpty() || NomeAssunto.Length  250)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.NomeAssuntoInvalido);

			//// Optional
			//if(TextoPadrao.IsNullOrEmpty() == false && TextoPadrao.Length > 1000)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.TextoPadraoInvalido);
			//if (validationResultsManager.HasError)
			//	validationResultsManager.ThrowBusinessValidationError();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new TipoAssunto();
			entidade.NomeAssunto = this.NomeAssunto;
			entidade.TextoPadrao = this.TextoPadrao;
			return entidade;
		}
	}

	public enum CampoOrdenacaoTipoAssunto
	{
		Id,
		NomeAssunto,
	}
}