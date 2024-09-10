using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.EntitiesMetadata;
using Differencial.Domain.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(AnalistaMetadata))]
    public class Analista : IEntity, IAtivavel
	{
        public Analista()
        {
           // VistoriadorProduto = new HashSet<VistoriadorProduto>();
            Solicitacao = new HashSet<Solicitacao>();
        }

        [Key, ForeignKey("Operador")]
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

        [Column("IndAtivo")]
		public bool IndAtivo { get; set; }

        public virtual Operador Operador { get; set; }

        //public virtual ICollection<VistoriadorProduto> VistoriadorProduto { get; set; }

        public virtual ICollection<Solicitacao> Solicitacao { get; set; }

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

	
	}

	public enum CampoOrdenacaoAnalista
	{
	}
}