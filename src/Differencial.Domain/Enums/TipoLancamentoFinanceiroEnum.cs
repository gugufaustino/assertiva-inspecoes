using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain
{
    public enum TipoLancamentoFinanceiroEnum
    {
        [Display(Name = "Despesa", ShortName = "")]
        DespesaPagamentoVistoriador = 1,
        [Display(Name = "Receita", ShortName = "")]
        ReceitaProdutoReceberSeguradora = 2

    }
}
