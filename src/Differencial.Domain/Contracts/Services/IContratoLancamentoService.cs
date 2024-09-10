using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface IContratoLancamentoService
    {
        IEnumerable<ContratoLancamento> Listar(ContratoLancamentoFilter filtro);

        void Salvar(ContratoLancamento entidade);

        void Salvar(IEnumerable<ContratoLancamento> lstEntidade);

        void Excluir(int id);
    }
}