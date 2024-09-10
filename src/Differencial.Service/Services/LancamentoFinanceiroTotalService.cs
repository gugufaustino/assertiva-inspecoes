using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System.Linq;
using System;
using Differencial.Domain;

namespace Differencial.Service.Services
{
	public class LancamentoFinanceiroTotalService : Service, ILancamentoFinanceiroTotalService
	{
		ILancamentoFinanceiroTotalRepository _lancamentoFinanceiroTotalRepositorio;

		public LancamentoFinanceiroTotalService(IUnitOfWork uow, ILancamentoFinanceiroTotalRepository lancamentoFinanceiroTotalRepositorio)
			: base(uow)
		{
			_lancamentoFinanceiroTotalRepositorio = lancamentoFinanceiroTotalRepositorio;
		}

		public IEnumerable<LancamentoFinanceiroTotal> Listar(LancamentoFinanceiroTotalFilter filtro)
		{
			return TryCatch(() =>
			{
				return _lancamentoFinanceiroTotalRepositorio.Where(filtro);
			});
		}

        public List<LancamentoFinanceiroTotal> ListarPorAnoMes(int mes, int ano, int tipoMovimentoSintetico)
        {
            return TryCatch(() =>
            {
                return _lancamentoFinanceiroTotalRepositorio.Where(w => w.TipoLancamentoFinanceiro == (TipoLancamentoFinanceiroEnum)tipoMovimentoSintetico
                                                                    && w.DthLancamentoPagamento.Month == mes
                                                                    && w.DthLancamentoPagamento.Year == ano).ToList();
            });

        }

        public void Salvar(LancamentoFinanceiroTotal entidade)
        {
			TryCatch(() =>
			{
				entidade.Validate();

				if (entidade.Id == 0)
					_lancamentoFinanceiroTotalRepositorio.Add(entidade);
				else
					_lancamentoFinanceiroTotalRepositorio.Update(entidade);
			});
		}

		public void Excluir(int id)
        {
			TryCatch(() =>
			{
				_lancamentoFinanceiroTotalRepositorio.Delete(id);
			});
		}

        public IEnumerable<LancamentoFinanceiroTotal> TodosLancamentosFinanceiros()
        {
			return TryCatch(() =>
			{
				return _lancamentoFinanceiroTotalRepositorio.TodosLancamentosFinanceiros();
			});
		}
    }
}