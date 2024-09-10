using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(LaudoFotoMetadata))]
    public class LaudoFoto : IEntity
	{
        [Key, ForeignKey("ArquivoAnexo")]
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

        [Column("IdLaudo")]
		public int? IdLaudo { get; set; }

		//[Column("IdArquivoAnexo")]
		//public int IdArquivoAnexo { get; set; }

        [Column("QuadroFotosPosicao")]
        public int QuadroFotosPosicao { get; set; }

        [Column("IndQuadroFoto")]
        public bool IndQuadroFoto { get; set; }

        #region Relacionamentos

        public virtual ArquivoAnexo ArquivoAnexo { get; set; }

        [ForeignKey("IdLaudo")]
        public virtual Laudo Laudo { get; set; }

        #endregion
        // Valida os dados da entidade
        public void Validate()
		{
            //var validationResultsManager = new ValidationResultsManager();

            ////Required

            //if (IdLaudo == 0)
            //    validationResultsManager.AddValidationResultNotValid(MensagensValidacao.IdLaudoInvalido);
            //if (IdFoto == 0)
            //    validationResultsManager.AddValidationResultNotValid(MensagensValidacao.IdFotoInvalido);

            ////Optional

            //if (validationResultsManager.HasError)
            //    validationResultsManager.ThrowBusinessValidationError();
        }

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new LaudoFoto();
			 
			return entidade;
		}
	}

	public enum CampoOrdenacaoLaudoFoto
	{
	}
}