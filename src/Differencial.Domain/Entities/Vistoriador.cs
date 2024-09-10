using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(VistoriadorMetadata))]
    public class Vistoriador : IEntity, IAtivavel
    {
        public Vistoriador()
        {
            VistoriadorProduto = new HashSet<VistoriadorProduto>();
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

        [Column("IdEnderecoBase")]
		public int IdEnderecoBase { get; set; }

		[Column("IndEnderecoBaseIgual")]
		public bool IndEnderecoBaseIgual { get; set; }
        
        #region "Relacionamentos"

        [ForeignKey("IdEnderecoBase")]
        public virtual Endereco EnderecoBase { get; set; }

        public virtual ICollection<VistoriadorProduto> VistoriadorProduto { get; set; }

        public virtual ICollection<Solicitacao> Solicitacao { get; set; }

        public virtual Operador Operador { get; set; }

        #endregion "Relacionamentos"

		// Valida os dados da entidade
		public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required
            //if(IdOperador == 0)
            //    validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoRelacionamentoInvalido, "Operador", "Operador"));
            //if(IdEnderecoBase == 0)
            //    validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoRelacionamentoInvalido, "Endereço Base", "Endereço Base"));

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Vistoriador();
			entidade.IdOperador = this.IdOperador;
			entidade.IdEnderecoBase = this.IdEnderecoBase;
			entidade.IndEnderecoBaseIgual = this.IndEnderecoBaseIgual;
			return entidade;
		}
	}

	public enum CampoOrdenacaoVistoriador
	{
		Id,
		IdOperador,
		IdEnderecoBase,
	}
}