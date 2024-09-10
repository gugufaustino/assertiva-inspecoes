using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.EntitiesDTO
{
    public record FinanceiroReceberDto(int Id, string NomeSeguradora, TipoLancamentoFinanceiroEnum tipoLancamentoFinanceiro, TipoSituacaoProcessoEnum? TpSituacao, decimal ValorLancamentoFinanceiro, int Ano, int Mes);
}
