using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Services
{
	public interface ILancamentoFinanceiroTotalService
	{
		IEnumerable<LancamentoFinanceiroTotal> Listar(LancamentoFinanceiroTotalFilter filtro);

        List<LancamentoFinanceiroTotal> ListarPorAnoMes(int mes, int ano, int tipoMovimentoSintetico);

        void Salvar(LancamentoFinanceiroTotal entidade);

		void ExcluirPorSolicitacao(int idSolicitacao);
        IEnumerable<LancamentoFinanceiroTotal> TodosLancamentosFinanceiros();
		Task Faturar(int[] id, int ano, int mes);
		Task Liquidar(int[] id, int ano, int mes);
		void IncluirSomarTotal(LancamentoFinanceiro lancamentoFinanceiro);
	}
}