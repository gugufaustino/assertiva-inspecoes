using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;
using System.Collections.Generic;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;

namespace Differencial.Domain.Entities
{

    [ModelMetadataType(typeof(ClienteMetadata))]
    public class Cliente : IEntity
	{
        public Cliente()
        { 
            Solicitacao = new HashSet<Solicitacao>();
            ClienteEndereco = new HashSet<ClienteEndereco>(); 
        }

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

        [Column("CpfCnpj")]
		public string CpfCnpj { get; set; }

        [Column("NomeRazaoSocial")]
        public string NomeRazaoSocial { get; set; }

		[Column("AtividadeNome")]
		public string AtividadeNome { get; set; } 
         
        [Column("ContatoNome")]
		public string ContatoNome { get; set; }

		[Column("ContatoTelefone")]
		public string ContatoTelefone { get; set; }

		[Column("ContatoOutro")]
		public string ContatoOutro { get; set; }

        [Column("ContatoAgendamento")]
        public string ContatoAgendamento { get; set; }

        #region "Relacionamentos"

        public virtual ICollection<Solicitacao> Solicitacao { get; set; }
         
        public virtual ICollection<ClienteEndereco> ClienteEndereco { get; set; }

        #endregion


        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

            // Required 
            if (NomeRazaoSocial.IsNullOrEmpty())
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Cliente", "Nome ou Razão Social");


            // Optional
            if (CpfCnpj.IsNullOrEmpty() == false && CpfCnpj.Length > 14)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "CPF ou CNPJ", "14"));

            if (NomeRazaoSocial.IsNullOrEmpty() == false && NomeRazaoSocial.Length > 250)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Nome ou Razão Social", "250"));

            if (AtividadeNome.IsNullOrEmpty() == false && AtividadeNome.Length > 250)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Atividade", "250"));

            if (ContatoNome.IsNullOrEmpty() == false && ContatoNome.Length > 250)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Nome para Contato", "250"));

            if (ContatoTelefone.IsNullOrEmpty() == false && ContatoTelefone.Length > 13)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Telefone de Contato", "13"));

            if (ContatoOutro.IsNullOrEmpty() == false && ContatoOutro.Length > 250)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Outro Contato", "250"));
        

            if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Cliente();
			entidade.CpfCnpj = this.CpfCnpj;
			entidade.ContatoNome = this.ContatoNome;
			entidade.ContatoTelefone = this.ContatoTelefone;
			entidade.ContatoOutro = this.ContatoOutro;
			entidade.AtividadeNome = this.AtividadeNome;
			return entidade;
		}
	}

	public enum CampoOrdenacaoCliente
	{
	}
}