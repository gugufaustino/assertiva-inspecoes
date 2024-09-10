using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
	public interface ILancamentoFinanceiroTotalService
	{
		IEnumerable<LancamentoFinanceiroTotal> Listar(LancamentoFinanceiroTotalFilter filtro);

        List<LancamentoFinanceiroTotal> ListarPorAnoMes(int mes, int ano, int tipoMovimentoSintetico);

        void Salvar(LancamentoFinanceiroTotal entidade);

		void Excluir(int id);
        IEnumerable<LancamentoFinanceiroTotal> TodosLancamentosFinanceiros();
    }
}