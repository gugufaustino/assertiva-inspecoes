using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain
{
    public enum TipoContratoLancamentoEnum
    {
        [Display(Name = "Receita/Honorário", ShortName = "")]
        Honorario = 1,

        [Display(Name = "Receita/Quilometros", ShortName = "")]
        Quilometros = 2,

        [Display(Name = "Receita/Custeio", ShortName = "")]
        Custeio = 2,

        [Display(Name = "Receita/Bonificacoes", ShortName = "")]
        Bonificacoes = 4,
    }
}
