using Differencial.Domain.Entities;
using Differencial.Domain.EntitiesDTO;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface ILancamentoFinanceiroTotalRepository : IRepository<LancamentoFinanceiroTotal>
    {
        IEnumerable<LancamentoFinanceiroTotal> TodosLancamentosFinanceiros();
        IEnumerable<FinanceiroReceberDto> FinanceiroReceber(int ano, int mes);
        IEnumerable<FinanceiroLancamentosReceberDto> FinanceiroLancamentosReceber(int id, int ano, int mes);
		void DeleteBySolicitacao(int idSolicitacao);
	}
}