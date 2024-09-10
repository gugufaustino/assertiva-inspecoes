using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain
{
    public enum TipoContratoParametroEnum
    {
        [Display(Name = "Lançamento Honorário Fixo", ShortName = "Honorário Fixo")]
        Comum = 1,

        [Display(Name = "Lançamento Honorário por Valor do Risco", ShortName = "Valor do Risco (R$)")]
        ValorRisco = 2,

        [Display(Name = "Lançamento por Bloco Construído", ShortName = "Bloco Construído (Un)")]
        BlocoConstruido = 3,

        [Display(Name = "Lançamento por Área Construída", ShortName = "Área Construída (M²)")]
        AreaConstruida = 4,

        [Display(Name = "Lançamento por Equipamento/Maquina", ShortName = "Equipamento (Un)")]
        Equipamento = 5,

        [Display(Name = "Lançamento por Estado", ShortName = "Estado (UF)")]
        Estado = 6,

        [Display(Name = "Lançamento Custos Quilometragem", ShortName = "Quilometragem")]
        Quilometragem = 7, // valor de lançamento do parametro inserido direto no cadastro da seguradora

        [Display(Name = "Lançamento por Casa Construída", ShortName = "Casa Construída (Un)")]
        CasaConstruida = 8,

        [Display(Name = "Lançamento por Relatório adicional de melhoria", ShortName = "Relatório de melhoria")]
        RelatorioMelhoria = 9,

    }
}
