using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Differencial.Domain.Entities
{
	public class Solicitante : IEntity, IAtivavel
    {
        public Solicitante()
        {
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

		[Column("IdOperador")]
		public int IdOperador { get; set; }

        [Column("IndAtivo")]
        public bool IndAtivo { get; set; }

        [Column("TipoSolicitante")]
		public TipoSolicitanteEnum TipoSolicitante { get; set; }

		[Column("IdSeguradora")]
		public int? IdSeguradora { get; set; }

        [ForeignKey("IdSeguradora")]
        public virtual Seguradora Seguradora { get; set; }

        public virtual Operador Operador { get; set; }

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

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Solicitante();
			entidade.IdOperador = this.IdOperador;
			entidade.TipoSolicitante = this.TipoSolicitante;
			entidade.IdSeguradora = this.IdSeguradora;
			return entidade;
		}
	}

	public enum CampoOrdenacaoSolicitante
	{
		Id,
        NomeSolicitante
	}
}