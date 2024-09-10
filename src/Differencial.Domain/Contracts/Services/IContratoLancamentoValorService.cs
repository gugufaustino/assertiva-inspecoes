using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface IContratoLancamentoValorService
    {
        IEnumerable<ContratoLancamentoValor> Listar(ContratoLancamentoValorFilter filtro);
        void Salvar(ContratoLancamentoValor entidade);
        void Excluir(int id);
        void ExcluirNaoContidos(int IdContratoLancamento, ICollection<ContratoLancamentoValor> contratoLancamentoValor);
        void Salvar(ICollection<ContratoLancamentoValor> contratoLancamentoValor, int IdContratoLancamento);
    }
}