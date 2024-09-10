using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Differencial.Domain.Entities
{
	public class NotificacaoOperador : IEntity
	{
		[Column("Id")]
		public int Id { get; set; }

		[Column("IndLido")]
		public bool IndLido { get; set; }

		[Column("IdNotificacao")]
		public int IdNotificacao { get; set; }

		[Column("IdOperador")]
		public int IdOperador { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }


        [ForeignKey("IdNotificacao")]
        public virtual Notificacao Notificacao { get; set; }

        public void Validate()
        {
            var validationResultsManager = new ValidationResultsManager();

            // Required
            if (IdNotificacao < 1 && Notificacao.IsNull())
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRelacionamentoInvalido, nameof(Notificacao));

            // Optional
            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException();
        }
    }

	public enum CampoOrdenacaoNotificacaoOperador
	{
	}
}