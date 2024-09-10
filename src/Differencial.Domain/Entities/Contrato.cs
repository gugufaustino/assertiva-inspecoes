using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.Entities
{
	public class Contrato : IEntity
	{
        public Contrato()
        {
            ContratoLancamento = new HashSet<ContratoLancamento>();
        }

        [Key, ForeignKey("Produto")]
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
         

        #region "Relacionamentos"         
        public virtual Produto Produto { get; set; }
                 
        public virtual ICollection<ContratoLancamento> ContratoLancamento { get; set; } 
    

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

		 
		 
	}

	public enum CampoOrdenacaoContrato
	{
	}
}