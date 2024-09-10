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
    [ModelMetadataType(typeof(SeguradoraMetadata))]
	public class Seguradora : IEntity, IAtivavel, IEndereco
    {
        public Seguradora()
        {
            Produto = new HashSet<Produto>();
            Solicitante = new HashSet<Solicitante>();
            Filial = new HashSet<Filial>();
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

        [Column("IndAtivo")]
        public bool IndAtivo { get; set; }

        [Column("NomeSeguradora")]
        public string NomeSeguradora { get; set; }
        
        [Column("Cnpj")]
        public string Cnpj { get; set; }

        [Column("Inscricao")]
        public string Inscricao { get; set; }

        [Column("RazaoSocial")]
        public string RazaoSocial { get; set; }

        [Column("IdEndereco")]
        public int? IdEndereco { get; set; }

        [ForeignKey("IdEndereco")]
        public virtual Endereco Endereco { get; set; }

        [Column("ContabilInspecoesDiaInicio")]
        public int ContabilInspecoesDiaInicio { get; set; }

        [Column("ContabilInspecoesDiaFim")]
        public int ContabilInspecoesDiaFim { get; set; }

        [Column("ContabilInspetorDia")]
        public int ContabilInspetorDia { get; set; }

        [Column("ContabilEmpresaDia")]
        public int ContabilEmpresaDia { get; set; }

        [Column("IndIntegracaoSolicitacaoPorEmail")]
        public bool IndIntegracaoSolicitacaoPorEmail { get; set; }

        [Column("EmailRemetenteSolicitacao")]
        public string EmailRemetenteSolicitacao { get; set; }

        [Column("IndAgendaRepostaPorEmail")]
        public bool IndAgendaRepostaPorEmail { get; set; }

        [Column("IndLaudoRepostaPorEmail")]
        public bool IndLaudoRepostaPorEmail { get; set; }

        [Column("QtdQuilometroFranquia")]
        public int QtdQuilometroFranquia { get; set; }

        [Column("VlrQuilometroExcedente")]
        public decimal? VlrQuilometroExcedente { get; set; }


        #region "Relacionamentos"
        public virtual ICollection<Produto> Produto { get; set; }

        public virtual ICollection<Solicitante> Solicitante { get; set; }
        public virtual ICollection<Filial> Filial { get; set; }

        #endregion "Relacionamentos"

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

            if (NomeSeguradora.IsNullOrEmpty())
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.NomeInvalido);
            if(!NomeSeguradora.IsNullOrEmpty() && NomeSeguradora.Length > 250)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "NomeSeguradora", "250"));

			// Optional
			if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Seguradora();
			entidade.DataCadastro = this.DataCadastro;
			entidade.DataModificacao = this.DataModificacao;
			return entidade;
		}
	}

	public enum CampoOrdenacaoSeguradora
	{
		Id,
        DataCadastro,
        DataModificacao,
        NomeSeguradora
    }
}