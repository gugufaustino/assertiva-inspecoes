using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Service.Services
{
    public class ContratoLancamentoValorService : Service, IContratoLancamentoValorService
    {
        IContratoLancamentoValorRepository _contratoLancamentoValorRepositorio;

        public ContratoLancamentoValorService(IUnitOfWork uow, IContratoLancamentoValorRepository contratoLancamentoValorRepositorio)
            : base(uow)
        {
            _contratoLancamentoValorRepositorio = contratoLancamentoValorRepositorio;
        }

        public IEnumerable<ContratoLancamentoValor> Listar(ContratoLancamentoValorFilter filtro)
        {
            return TryCatch(() =>
            {
                return _contratoLancamentoValorRepositorio.Where(filtro);
            });
        }

        public void Salvar(ContratoLancamentoValor entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.IndPreAcordo)
                {
                    entidade.ValorLancamento = null;
                }

                if (entidade.Id == 0)
                    _contratoLancamentoValorRepositorio.Add(entidade);
                else
                {
                    var oldEntidade = _contratoLancamentoValorRepositorio.Find(entidade.Id);
                    oldEntidade.QuantitativoA = entidade.QuantitativoA;
                    oldEntidade.QuantitativoB = entidade.QuantitativoB;
                    oldEntidade.TipoQuantitativoVariacao = entidade.TipoQuantitativoVariacao;
                    oldEntidade.ValorLancamento = entidade.ValorLancamento;
                    oldEntidade.IndPreAcordo = entidade.IndPreAcordo;
                    oldEntidade.ValorLancamentoQuantitativo = entidade.ValorLancamentoQuantitativo;
                    _contratoLancamentoValorRepositorio.Update(oldEntidade);
                }

            });
        }

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _contratoLancamentoValorRepositorio.Delete(id);
            });
        }

        public void Salvar(ICollection<ContratoLancamentoValor> contratoLancamentoValor, int IdContratoLancamento)
        {
            TryCatch(() =>
            {
                foreach (var item in contratoLancamentoValor)
                {
                    item.IdContratoLancamento = IdContratoLancamento;
                    Salvar(item);
                }
            });
        }

        public void ExcluirNaoContidos(int IdContratoLancamento,   ICollection<ContratoLancamentoValor> contratoLancamentoValor)
        {
            TryCatch(() =>
            {
              
                var lstIdLancamentosValor = contratoLancamentoValor.Select(i => i.Id).ToList();
                var lstIdsLancamentosValorRemovidos = _contratoLancamentoValorRepositorio.Where(w => w.IdContratoLancamento == IdContratoLancamento && !lstIdLancamentosValor.Contains(w.Id)).Select(i => i.Id).ToList();


                foreach (var id in lstIdsLancamentosValorRemovidos)
                {
                    Excluir(id);
                }
            });
        }
    }
}