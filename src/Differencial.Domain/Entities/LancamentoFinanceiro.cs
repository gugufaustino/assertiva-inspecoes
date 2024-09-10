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
    [ModelMetadataType(typeof(LancamentoFinanceiroMetadata))]
    public class LancamentoFinanceiro : IEntity
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

        [Column("IdSolicitacao")]
        public int IdSolicitacao { get; set; }

        [Column("TipoLancamentoFinanceiro")]
        public TipoLancamentoFinanceiroEnum TipoLancamentoFinanceiro { get; set; }

        [Column("ValorLancamentoFinanceiro")]
        public decimal ValorLancamentoFinanceiro { get; set; }

        [Column("DescricaoLancamentoFinanceiro")]
        public string DescricaoLancamentoFinanceiro { get; set; }

        [ForeignKey("IdSolicitacao")]
        public virtual Solicitacao Solicitacao { get; set; }

        [ForeignKey("IdOperadorCadastro")]
        public virtual Operador OperadorCadastro { get; set; }

        [Column("IdLancamentoFinanceiroTotal")]
        public int IdLancamentoFinanceiroTotal { get; set; }

        [ForeignKey("IdLancamentoFinanceiroTotal")]
        public virtual LancamentoFinanceiroTotal LancamentoFinanceiroTotal { get; set; }

        // Valida os dados da entidade
        public void Validate()
        {
            var validationResultsManager = new ValidationResultsManager();

            // Required
            if (TipoLancamentoFinanceiro == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Lancamento Financeiro", "Tipo Lancamento Financeiro");

            if (DescricaoLancamentoFinanceiro.IsNullOrEmpty() || DescricaoLancamentoFinanceiro.Length > 250)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoTamanhoMaximoInvalido.Formata("Descrição Lançamento Financeiro", "250"));

            // Optional
            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException();
        }

        // Clona os dados da entidade
        public object Clone()
        {
            var entidade = new LancamentoFinanceiro();
            entidade.IdSolicitacao = this.IdSolicitacao;
            entidade.TipoLancamentoFinanceiro = this.TipoLancamentoFinanceiro;
            entidade.ValorLancamentoFinanceiro = this.ValorLancamentoFinanceiro;
            entidade.DescricaoLancamentoFinanceiro = this.DescricaoLancamentoFinanceiro;
            return entidade;
        }
    }

    public enum CampoOrdenacaoLancamentoFinanceiro
    {
    }
}