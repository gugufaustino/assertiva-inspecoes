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
    [ModelMetadataType(typeof(FilialMetadata))]
	public class Filial : IEntity
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

        [Column("IdSeguradora")]
		public int IdSeguradora { get; set; }

		[Column("NomeFilial")]
		public string NomeFilial { get; set; } 
        public virtual Seguradora Seguradora { get; set; }


        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required
			if(IdSeguradora == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, "Filial", "Seguradora");
            if (NomeFilial.IsNullOrEmpty())
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoRequeridoInvalido, "Filial", "Nome Filial"));

            if ( !NomeFilial.IsNullOrEmpty() && NomeFilial.Length > 250)
				validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Nome Filial", "250"));

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Filial();
			entidade.IdSeguradora = this.IdSeguradora;
			entidade.NomeFilial = this.NomeFilial;
			return entidade;
		}
	}

	public enum CampoOrdenacaoFilial
	{
		Id,
		IdSeguradora,
		NomeFilial,
	}
}