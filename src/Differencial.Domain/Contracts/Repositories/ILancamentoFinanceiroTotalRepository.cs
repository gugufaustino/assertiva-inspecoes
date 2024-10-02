using Differencial.Domain.Entities;
using Differencial.Domain.EntitiesDTO;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface ILancamentoFinanceiroTotalRepository : IRepository<LancamentoFinanceiroTotal>
    {
        IEnumerable<LancamentoFinanceiroTotal> TodosLancamentosFinanceiros();
        IEnumerable<FinanceiroReceberDto> FinanceiroReceber(int ano, int mes);
        IEnumerable<FinanceiroLancamentosReceberDto> FinanceiroLancamentosReceber(int id, int ano, int mes);
		void DeleteBySolicitacao(int idSolicitacao);
        Task<List<LancamentoFinanceiroTotal>> LancamentoParaSensibilizarInd(int idSeguradora, Domain.TipoLancamentoFinanceiroEnum tipoLancamentoFinanceiro, int ano, int mes);
        IEnumerable<FinanceiroPagarDto> FinanceiroPagar(int ano, int mes);
        IEnumerable<FinanceiroLancamentosPagarDto> FinanceiroLancamentosPagar(int idVistoriador, int ano, int mes);
	}
}