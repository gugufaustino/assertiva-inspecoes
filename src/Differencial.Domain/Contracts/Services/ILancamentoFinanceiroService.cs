using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface ILancamentoFinanceiroService
    {
        IEnumerable<LancamentoFinanceiro> Listar(LancamentoFinanceiroFilter filtro);

        void Salvar(LancamentoFinanceiro entidade);

        void Excluir(int id);

        IEnumerable<LancamentoFinanceiro> ListarLancamentosSinteticos();
    }
}