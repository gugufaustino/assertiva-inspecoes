using System;
using System.ComponentModel.DataAnnotations;

namespace Differencial.Domain.EntitiesMetadata
{
    public class LancamentoFinanceiroMetadata : BaseAuditoriaRegistroMetadata
    {

        [Display(Name = "C�digo")]
        public int Id { get; set; }

        [Display(Name = "Tipo Lancamento")]
        public TipoLancamentoFinanceiroEnum TipoLancamentoFinanceiro { get; set; }

        [Display(Name = "Valor Lan�amento")]
        public decimal ValorLancamentoFinanceiro { get; set; }

        [Display(Name = "Descri��o")]
        public string DescricaoLancamentoFinanceiro { get; set; }

    }
}