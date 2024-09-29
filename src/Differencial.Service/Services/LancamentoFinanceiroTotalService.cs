using Differencial.Domain;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Exceptions;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Differencial.Service.Services
{
	public class LancamentoFinanceiroTotalService : Service, ILancamentoFinanceiroTotalService
	{
		ILancamentoFinanceiroTotalRepository _lancamentoFinanceiroTotalRepositorio;
		private readonly ILancamentoFinanceiroRepository _lancamentoFinanceiroRepository;

		public LancamentoFinanceiroTotalService(IUnitOfWork uow,
					ILancamentoFinanceiroTotalRepository lancamentoFinanceiroTotalRepositorio,
					ILancamentoFinanceiroRepository lancamentoFinanceiroRepository)
			: base(uow)
		{
			_lancamentoFinanceiroTotalRepositorio = lancamentoFinanceiroTotalRepositorio;
			_lancamentoFinanceiroRepository = lancamentoFinanceiroRepository;
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

		public void ExcluirPorSolicitacao(int idSolicitacao)
		{

			var lstlancamentos = _lancamentoFinanceiroRepository.Where(l => l.IdSolicitacao == idSolicitacao);
			foreach (var lancamento in lstlancamentos)
			{
				_lancamentoFinanceiroRepository.Delete(lancamento);
			}
			_lancamentoFinanceiroTotalRepositorio.DeleteBySolicitacao(idSolicitacao);

		}

		public IEnumerable<LancamentoFinanceiroTotal> TodosLancamentosFinanceiros()
		{
			return TryCatch(() =>
			{
				return _lancamentoFinanceiroTotalRepositorio.TodosLancamentosFinanceiros();
			});
		}

		public async Task Faturar(int[] id, int ano, int mes)
		{
			foreach (var isLancTotal in id)
			{
				var valoresFaturar = await _lancamentoFinanceiroTotalRepositorio.SensibilizarLancamentos(isLancTotal, TipoLancamentoFinanceiroEnum.ReceitaProdutoReceberSeguradora, ano, mes);
				foreach (var lancamento in valoresFaturar)
				{
					lancamento.IndFaturado = true;
					_lancamentoFinanceiroTotalRepositorio.Update(lancamento);
				}
			}
		}

		public async Task Liquidar(int[] id, int ano, int mes)
		{
			foreach (var isLancTotal in id)
			{
				var valoresFaturar = await _lancamentoFinanceiroTotalRepositorio.SensibilizarLancamentos(isLancTotal, TipoLancamentoFinanceiroEnum.ReceitaProdutoReceberSeguradora, ano, mes);
				foreach (var lancamento in valoresFaturar)
				{
					lancamento.IndLiquidado = true;
					_lancamentoFinanceiroTotalRepositorio.Update(lancamento);
				}
			}
		}

		public void IncluirSomarTotal(LancamentoFinanceiro lancamentoFinanceiro)
		{
			var valores = _lancamentoFinanceiroTotalRepositorio.Where(i => i.IdSolicitacao == lancamentoFinanceiro.IdSolicitacao
																		&& i.TipoLancamentoFinanceiro == lancamentoFinanceiro.TipoLancamentoFinanceiro);

			if (valores.Count() > 1)
			{
				throw new ValidationException("Contate o suporte técnico - Falha no Incluir e Somar Total");
			}
			//solicitacao.VlrPagamentoVistoria.Value
			var valorTotal = valores.FirstOrDefault();
			if (valorTotal != null)
			{
				valorTotal.ValorLancamentoFinanceiroTotal = valorTotal.ValorLancamentoFinanceiroTotal + lancamentoFinanceiro.ValorLancamentoFinanceiro;
				Salvar(valorTotal);
			}
			_lancamentoFinanceiroRepository.Add(lancamentoFinanceiro);
		}
	}
}