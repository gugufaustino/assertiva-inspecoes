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
    [ModelMetadataType(typeof(CoberturaMetadata))]
    public class Cobertura : IEntity
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

        [Column("NomeCobertura")]
		public string NomeCobertura { get; set; }

        [Column("VlrCobertura")]
        public decimal? VlrCobertura { get; set; }

        [Column("IdSolicitacao")]
		public int? IdSolicitacao { get; set; }

        [ForeignKey("IdSolicitacao")]
        public virtual Solicitacao Solicitacao { get; set; }


        // Valida os dados da entidade
        public void Validate()
		{
			 
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Cobertura();
			entidade.NomeCobertura = this.NomeCobertura;
			entidade.IdSolicitacao = this.IdSolicitacao;
			return entidade;
		}
	}

	public enum CampoOrdenacaoCobertura
	{
		NomeCobertura,
	}
}