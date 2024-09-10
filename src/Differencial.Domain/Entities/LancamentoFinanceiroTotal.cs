using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Entities
{
    public class LancamentoFinanceiroTotal : IEntity
    {
        public LancamentoFinanceiroTotal()
        {
            LancamentoFinanceiro = new HashSet<LancamentoFinanceiro>();
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

        [Column("IdSolicitacao")]
        public int IdSolicitacao { get; set; }

        [Column("TipoLancamentoFinanceiro")]
        public TipoLancamentoFinanceiroEnum TipoLancamentoFinanceiro { get; set; }

        [Column("ValorLancamentoFinanceiroTotal")]
        public decimal ValorLancamentoFinanceiroTotal { get; set; }

        [Column("DthLancamentoPagamento")]
        public DateTime DthLancamentoPagamento { get; set; }

        [ForeignKey("IdSolicitacao")]
        public virtual Solicitacao Solicitacao { get; set; }

        public virtual ICollection<LancamentoFinanceiro> LancamentoFinanceiro { get; set; }

        // Valida os dados da entidade
        public void Validate()
        {
            var validationResultsManager = new ValidationResultsManager();

            // Required
            if (IdSolicitacao == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Lancamento Financeiro", "Solicitação");
            if (TipoLancamentoFinanceiro == 0)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Lancamento Financeiro", "Tipo Lancamento Financeiro");

            if (DthLancamentoPagamento.IsValid() == false)
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Lancamento Financeiro", "Data Lançamento Pagamento");

            // Optional
            if (validationResultsManager.HasError)
                validationResultsManager.ThrowException();
        }

        // Clona os dados da entidade
        public object Clone()
        {
            var entidade = new LancamentoFinanceiroTotal();
            entidade.IdSolicitacao = this.IdSolicitacao;
            entidade.TipoLancamentoFinanceiro = this.TipoLancamentoFinanceiro;
            entidade.ValorLancamentoFinanceiroTotal = this.ValorLancamentoFinanceiroTotal;
            entidade.DthLancamentoPagamento = this.DthLancamentoPagamento;
            return entidade;
        }
    }

    public enum CampoOrdenacaoLancamentoFinanceiroTotal
    {
    }
}