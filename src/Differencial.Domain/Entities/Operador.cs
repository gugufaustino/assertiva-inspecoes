using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;

namespace Differencial.Domain.Entities
{

    [ModelMetadataType(typeof(OperadorMetadata))]
    public class Operador : IEntity, IAtivavel, IEndereco
    {
		[Column("Id")]
		public int Id { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }

        [Column("IndAtivo")]
        public bool IndAtivo { get; set; }

        [Column("NomeOperador")]
		public string NomeOperador { get; set; }

        [Column("Telefone")]
        public string Telefone { get; set; }

        [Column("UrlFoto")]
		public string UrlFoto { get; set; }

		[Column("Email")]
		public string Email { get; set; }

		[Column("Cpf")]
		public string Cpf { get; set; }

		[Column("Rg")]
		public string Rg { get; set; }

		[Column("DataNascimento")]
		public DateTime? DataNascimento { get; set; }

		[Column("IndAnalista")]
		public bool IndAnalista { get; set; }

		[Column("IndGerente")]
		public bool IndGerente { get; set; }

		[Column("IndVistoriador")]
		public bool IndVistoriador { get; set; }

		[Column("IndSolicitante")]
		public bool IndSolicitante { get; set; }

        [Column("IndFinanceiro")]
        public bool IndFinanceiro { get; set; }

        [Column("IndAssessor")]
        public bool IndAssessor { get; set; }

        [Column("IdEndereco")]
        public int? IdEndereco { get; set; }

        [Column("IndAcessoSistema")]
        public bool IndAcessoSistema { get; set; }

        [Column("IndPrimeiroAcesso")]
        public bool IndPrimeiroAcesso { get; set; }

        [Column("Senha")]
        public string Senha { get; set; }
        public string SenhaConfirmacao { get; set; }

        [Column("IndUsuarioSistema")]
        public bool IndUsuarioSistema { get; set; }

        #region "Relacionamentos"

        [ForeignKey("IdEndereco")]
        public virtual Endereco Endereco { get; set; }

        public virtual Vistoriador Vistoriador { get; set; }

        public virtual Solicitante Solicitante { get; set; }

        public virtual Analista Analista { get; set; }

        #endregion "Relacionamentos"

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

			// Required
			if(NomeOperador.IsNullOrEmpty() || NomeOperador.Length > 250)
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.NomeInvalido);
			if(Email.IsNullOrEmpty() || (Email.IsValidEmailAddress() == false || Email.Length > 250))
				validationResultsManager.AddValidationResultNotValid(MensagensValidacao.EmailInvalido);
			//if(Cpf.IsNullOrEmpty() || Cpf.Length > 20)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CpfInvalido);
			//if(Rg.IsNullOrEmpty() || Rg.Length > 20)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.RGInvalido);
   //         if (DataNascimento.Value.IsValid() == false)
			//	validationResultsManager.AddValidationResultNotValid(MensagensValidacao.DataNascimentoInvalido);

			// Optional
			if(UrlFoto.IsNullOrEmpty() == false && UrlFoto.Length > 50)
				validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "UrlFoto", "50"));
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Operador();
			entidade.NomeOperador = this.NomeOperador; 
			entidade.UrlFoto = this.UrlFoto;
			entidade.Email = this.Email;
			entidade.Cpf = this.Cpf;
			entidade.Rg = this.Rg;
			entidade.DataNascimento = this.DataNascimento;
			entidade.IndAnalista = this.IndAnalista;
			entidade.IndGerente = this.IndGerente;
			entidade.IndVistoriador = this.IndVistoriador;
			entidade.IndSolicitante = this.IndSolicitante;
			entidade.DataCadastro = this.DataCadastro;
			entidade.DataModificacao = this.DataModificacao;
			return entidade;
		}
	}

	public enum CampoOrdenacaoOperador
	{
		Id,
		NomeOperador,
		TpSituacao,
		Email,
		Cpf,
		RG,
		DataNascimento,
		IndAnalista,
		IndGerente,
		IndVistoriador,
		IndSolicitante,
		DataCadastro,
		DataModificacao,
	}
}