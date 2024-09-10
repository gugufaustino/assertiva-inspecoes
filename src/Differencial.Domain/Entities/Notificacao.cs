using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Entities
{
	public class Notificacao : IEntity
	{
        public Notificacao()
        {
            NotificacaoOperador = new HashSet<NotificacaoOperador>();
        }
		[Column("Id")]
		public int Id { get; set; }

		[Column("Descricao")]
		public string Descricao { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }

        [Column("IdSolicitacao")]
        public int IdSolicitacao { get; set; }
        public virtual ICollection<NotificacaoOperador> NotificacaoOperador { get; set; }

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required

			// Optional
			if(Descricao.IsNullOrEmpty() == false && Descricao.Length > 500)
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DescricaoInvalido);
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		 
	}

	public enum CampoOrdenacaoNotificacao
	{
	}
}