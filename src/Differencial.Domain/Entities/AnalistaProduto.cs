using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Differencial.Domain.Entities
{
    public class AnalistaProduto : IEntity, IAtivavel
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

        [Column("IndAtivo")]
		public bool IndAtivo { get; set; }

		[Column("QtdPontuacao")]
		public int? QtdPontuacao { get; set; }


		// Valida os dados da entidade
		public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new AnalistaProduto();
			entidade.IndAtivo = this.IndAtivo;
			entidade.QtdPontuacao = this.QtdPontuacao;
			return entidade;
		}
	}

	public enum CampoOrdenacaoAnalistaProduto
	{
	}
}