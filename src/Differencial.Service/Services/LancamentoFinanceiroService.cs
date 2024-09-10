using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Service.Services
{
    public class LancamentoFinanceiroService : Service, ILancamentoFinanceiroService
    {
        ILancamentoFinanceiroRepository _lancamentoFinanceiroRepositorio;

        public LancamentoFinanceiroService(IUnitOfWork uow, ILancamentoFinanceiroRepository lancamentoFinanceiroRepositorio)
            : base(uow)
        {
            _lancamentoFinanceiroRepositorio = lancamentoFinanceiroRepositorio;
        }

        public IEnumerable<LancamentoFinanceiro> Listar(LancamentoFinanceiroFilter filtro)
        {
            return TryCatch(() =>
            {
                return _lancamentoFinanceiroRepositorio.Where(filtro);
            });
        }

        public void Salvar(LancamentoFinanceiro entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _lancamentoFinanceiroRepositorio.Add(entidade);
                else
                    _lancamentoFinanceiroRepositorio.Update(entidade);
            });
        }

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _lancamentoFinanceiroRepositorio.Delete(id);
            });
        }

        public IEnumerable<LancamentoFinanceiro> ListarLancamentosSinteticos()
        {
            return TryCatch(() =>
            {
                var lst = _lancamentoFinanceiroRepositorio.Where(i => i.Solicitacao.TpSituacao == Domain.TipoSituacaoProcessoEnum.Concluido
                                                                || i.Solicitacao.TpSituacao == Domain.TipoSituacaoProcessoEnum.EnviadoParaFinanceiro
                                                                 || i.Solicitacao.TpSituacao == Domain.TipoSituacaoProcessoEnum.ApropriadoPeloFinanceiro);


                return lst.OrderBy(o => o.DataCadastro);
            });
        }
    }
}