using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class LancamentoFinanceiroMetadata : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Tipo Lancamento")]
        public TipoLancamentoFinanceiroEnum TipoLancamentoFinanceiro { get; set; }

        [Display(Name = "Valor Lançamento")]
        public decimal ValorLancamentoFinanceiro { get; set; }

        [Display(Name = "Descrição")]
        public string DescricaoLancamentoFinanceiro { get; set; }

    }
}