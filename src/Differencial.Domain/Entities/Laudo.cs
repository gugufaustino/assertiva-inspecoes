using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;
using System.Collections.Generic;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(LaudoMetadata))]
    public class Laudo : IEntity
	{
        public Laudo()
        {
            LaudoFoto = new HashSet<LaudoFoto>();
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

        [Column("IdSolicitacao")]
		public int IdSolicitacao { get; set; }

        public virtual ICollection<LaudoFoto> LaudoFoto { get; set; }

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required
			//if(IdSolicitacao == 0)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.IdSolicitacaoInvalido);

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Laudo();
			entidade.IdSolicitacao = this.IdSolicitacao;
			return entidade;
		}
	}

	public enum CampoOrdenacaoLaudo
	{
	}
}