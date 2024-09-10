using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Differencial.Domain.Entities
{
    public class ClienteEndereco : IEntity
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

        [Column("IdCliente")]
		public int IdCliente { get; set; }

		[Column("IdEndereco")]
		public int IdEndereco { get; set; }

        #region "Relacionamentos"
        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdEndereco")]
        public virtual Endereco Endereco { get; set; }

        #endregion

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required
			//if(IdCliente == 0)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.IdClienteInvalido);
			//if(IdEndereco == 0)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.IdEnderecoInvalido);

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new ClienteEndereco();
			entidade.IdCliente = this.IdCliente;
			entidade.IdEndereco = this.IdEndereco;
			return entidade;
		}
	}

	public enum CampoOrdenacaoClienteEndereco
	{
	}
}