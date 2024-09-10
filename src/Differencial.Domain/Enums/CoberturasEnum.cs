using Differencial.Domain.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain
{
    public enum CoberturasEnum
    {
        [CoberturaAttribute( Name = "INCENDIO", IndPadraoInclusao = true)]
        Incendio,
        [CoberturaAttribute(Name = "DANOS ELÉTRICOS", IndPadraoInclusao = true)]
        DanosEletrico,
        [CoberturaAttribute(Name = "EQUIPAMENTOS ELETRÔNICOS", IndPadraoInclusao = true)]
        EquipamentosEletrônicos,

        [CoberturaAttribute(Name = "QUEBRA DE VIDROS", IndPadraoInclusao = true)]
        QuebraVidros,
        [CoberturaAttribute(Name = "PERDA OU PAGTO ALUGUEL", IndPadraoInclusao = true)]
        PerdaPagtoAluguel,
        [CoberturaAttribute(Name = "RC ESTABELECIMENTO / OPERAÇÕES", IndPadraoInclusao = true)]
        RcEstabelecimento,
        [CoberturaAttribute(Name = "ROUBO DE BENS", IndPadraoInclusao = true)]
        RouboBens,
        [CoberturaAttribute(Name = "TUMULTOS", IndPadraoInclusao = false)]
        Tumultos,
        [CoberturaAttribute(Name = "VENDAVAL", IndPadraoInclusao = true)]
        Vendaval,
    }
}
