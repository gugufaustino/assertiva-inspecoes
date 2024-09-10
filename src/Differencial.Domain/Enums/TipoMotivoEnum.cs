using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain
{
    public enum TipoMotivoEnum
    {
        [Display(Name = "Não atuo na região/cidade")]
        NãoAtuoNaRegiao,
        [Display(Name = "Estou com sobrecarga de demandas")]
        Sobrecarregado,
        [Display(Name = "Necessário corrigir laudo")]
        CorrigirLaudo,
        [Display(Name = "Necessário corrigir croqui")]
        CorrigirCroqui,

        [Display(Name = "Segurado desistiu da contratação")]
        CanceladoPeloSegurado,
    }
}
