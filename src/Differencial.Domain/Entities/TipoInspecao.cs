using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;
using System.Collections.Generic;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(TipoInspecaoMetadata))]
    public class TipoInspecao : IEntity, IAtivavel
    {
        public TipoInspecao(){
            Produto = new HashSet<Produto>(); 
        }

		[Column("IdTipoInspecao")]
		public int Id { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }

        [Column("NomeTipoInspecao")]
		public string NomeTipoInspecao { get; set; }

		[Column("DescricaoTipoInspecao")]
		public string DescricaoTipoInspecao { get; set; }

        [Column("IndAtivo")]
        public bool IndAtivo { get; set; }

        public virtual ICollection<Produto> Produto { get; set; }


        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required
			if(NomeTipoInspecao.IsNullOrEmpty() || NomeTipoInspecao.Length == 0 )
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.NomeInvalido);

			// Optional
			if(DescricaoTipoInspecao.IsNullOrEmpty() == false && DescricaoTipoInspecao.Length > 1000)
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DescricaoInvalido);
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new TipoInspecao();
			entidade.NomeTipoInspecao = this.NomeTipoInspecao;
			entidade.DescricaoTipoInspecao = this.DescricaoTipoInspecao;
			 
			return entidade;
		}
	}

	public enum CampoOrdenacaoTipoInspecao
	{
	}
}