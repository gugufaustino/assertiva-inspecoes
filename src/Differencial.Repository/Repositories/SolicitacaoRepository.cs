using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Differencial.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Differencial.Repository.Repositories.Base;

namespace Differencial.Repository.Repositories
{
	public class SolicitacaoRepository : RepositoryBase<Solicitacao>, ISolicitacaoRepository
	{
		public SolicitacaoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
			: base(dbContextFactory, usuario)
		{
		}

		public Task<Solicitacao> BuscarUI(int id)
		{
			return _dbSet
					.Include(i => i.Vistoriador.Operador)
					.Include(i => i.Vistoriador.EnderecoBase)
					.Include(i => i.Analista.Operador)
					.Include(i => i.Solicitante.Operador)
					.Include(i => i.Produto.Seguradora)
					.Include(i => i.Produto.Contrato.ContratoLancamento).ThenInclude(c => c.ContratoLancamentoValor)
					.Include(i => i.Endereco)
					.Include(i => i.Cliente.ClienteEndereco)
					.Include(i => i.Cobertura)
					.Include(i => i.Comunicacao)
					.Include(i => i.Agendamento)
					.Include(i => i.LancamentoFinanceiro)
					.Include(i => i.OperadorCadastro)
					.Include(i => i.OperadorApropriado)
					.Include(i => i.MovimentacaoProcesso).ThenInclude(e => e.OperadorOrigem)
					.Include(i => i.AtividadeProcesso)
					.AsNoTracking()
					.FirstOrDefaultNoTrackingAsync(w => w.Id == id);
		}

		public Task<List<Solicitacao>> ListarSolicitacoesGerencia()
		{
			return _dbSet
					  .Include(i => i.Cliente)
					  .Include(i => i.Endereco)
					  .Include(i => i.Produto)
					  .Include(i => i.Seguradora)
					  .Include(i => i.AtividadeProcesso)
					   .Where(w => (w.TpSituacao == TipoSituacaoProcessoEnum.DevolvidoParaGerencia
								   || w.TpSituacao == TipoSituacaoProcessoEnum.ApropriadoGerente
								   || w.TpSituacao == TipoSituacaoProcessoEnum.EmElaboracao
								   || w.TpSituacao == TipoSituacaoProcessoEnum.EnviadoParaGerencia))
						   .ToListAsync();
		}
		public Task<List<Solicitacao>> ListarSolicitacoesAnalista()
		{
			return _dbSet
					  .Include(i => i.Cliente)
					  .Include(i => i.Endereco)
					  .Include(i => i.Produto.Seguradora)
					  .Include(i => i.Analista.Operador)
					  .Include(i => i.OperadorApropriado)
					  .Include(i => i.AtividadeProcesso)
					  .Include(i => i.MovimentacaoProcesso)
					   .Where(w => (w.IdOperadorApropriado != _usuario.Id && w.TpSituacao == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise)
								   || (w.TpSituacao == TipoSituacaoProcessoEnum.EnviadoParaAnalise))
					  .ToListAsync();
		}

		public Task<List<Solicitacao>> ListarSolicitacoesAnalistaMinhas()
		{
			return _dbSet
					   .Include(i => i.Cliente)
					   .Include(i => i.Endereco)
					   .Include(i => i.Produto.Seguradora)
					   .Include(i => i.AtividadeProcesso)
					   .Include(i => i.MovimentacaoProcesso)
					   .Where(w => (w.IdOperadorApropriado == _usuario.Id && w.TpSituacao == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise))
					   .ToListAsync();
		}

		public IEnumerable<Solicitacao> ListarSolicitacoesFinanceiro()
		{
			return _dbSet
						.Include(i => i.Seguradora)
						.Include(i => i.Produto)
						.Include(i => i.OperadorApropriado)
						.Include(i => i.MovimentacaoProcesso)
						.AsNoTracking()
						.Where(w => w.TpSituacao == TipoSituacaoProcessoEnum.ApropriadoPeloFinanceiro ||
								w.TpSituacao == TipoSituacaoProcessoEnum.EnviadoParaFinanceiro)
						.ToList();
		}
		public IEnumerable<Solicitacao> ListarTodasAgendas()
		{
			return _dbSet
						.Include(i => i.MovimentacaoProcesso).ThenInclude(t => t.OperadorOrigem)
						.Include(i => i.Agendamento)
						.Include(i => i.Vistoriador.Operador)
						.Include(i => i.Vistoriador.EnderecoBase)
						.Include(i => i.Produto.Seguradora)
						.Include(i => i.Cliente)
						.Include(i => i.Endereco)
						.AsNoTracking()
						.ToList();
		}
		public IEnumerable<Solicitacao> ListarTodasSolicitacoes()
		{
			return _dbSet
						.Include(i => i.MovimentacaoProcesso).ThenInclude(t => t.OperadorOrigem)
						.Include(i => i.Produto.Seguradora)
						.Include(i => i.Cliente)
						.Include(i => i.Endereco)
						.Include(i => i.OperadorApropriado)
						.AsNoTracking()
						.ToList();
		}

		public Task<List<Solicitacao>> ListarSolicitacoesGerenciaAgendamento()
		{


			return _dbSet
						.Include(i => i.Agendamento)
						.Include(i => i.Cliente)
						.Include(i => i.Produto.Seguradora)
						.Include(i => i.Vistoriador.Operador)
						.Include(i => i.Vistoriador.EnderecoBase)
						.Include(i => i.MovimentacaoProcesso)
						.AsNoTracking()
						.Where(w => w.IndRelacionamentoAgendaInformada == false
								&& w.MovimentacaoProcesso.Any(m => m.TipoSituacaoProcesso == TipoSituacaoProcessoEnum.EnviadoParaVistoria)).
						ToListAsync();
		}

		public IEnumerable<Solicitacao> ListarSolicitacoesVistoriador()
		{

			return _dbSet
					.Include(i => i.Cliente)
					.Include(i => i.Endereco)
					.Include(i => i.Produto.TipoInspecao)
					.Include(i => i.Produto.Seguradora)
					.Include(i => i.MovimentacaoProcesso)
					.Include(i => i.Agendamento)
					.Where(w => (w.IdOperadorApropriado == _usuario.Id && w.TpSituacao == TipoSituacaoProcessoEnum.ApropriadoVistoriador)
									|| (w.IdVistoriador == _usuario.Id && w.TpSituacao == TipoSituacaoProcessoEnum.EnviadoParaVistoria)
									|| (w.IdVistoriador == _usuario.Id && !w.CustoDeslocamentoRealizado.HasValue && (w.TpSituacao == TipoSituacaoProcessoEnum.EnviadoParaAnalise
																																|| w.TpSituacao == TipoSituacaoProcessoEnum.ApropriadoPelaAnalise))
									)
									.OrderByDescending(o => o.IdOperadorApropriado)
									.ThenByDescending(o => o.DataCadastro)
									.ToList();
		}

		public override IEnumerable<Solicitacao> Where<F>(F filter)
		{
			var query = from solicitacao in _db.Solicitacao select solicitacao;
			this.AplicarFiltro(ref query, filter as SolicitacaoFilter);
			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<Solicitacao> query, SolicitacaoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.IdSeguradora.HasValue)
				query = query.Where(x => filter.IdSeguradora == x.IdSeguradora);

			if (filter.IdProduto.HasValue)
				query = query.Where(x => filter.IdProduto == x.IdProduto);

			if (filter.IdVistoriador.HasValue)
				query = query.Where(x => filter.IdVistoriador == x.IdVistoriador);

			if (filter.TpSituacao.HasValue)
				query = query.Where(x => filter.TpSituacao == x.TpSituacao);

			if (filter.IdSolicitante.HasValue)
				query = query.Where(x => filter.IdSolicitante == x.IdSolicitante);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}

		public Task<Solicitacao> BuscarParaEnviar(int id)
		{
			return _dbSet
				   .Include(i => i.Cliente)
				   .Include(i => i.Endereco)
				   .Include(i => i.Agendamento)
				   .Include(i => i.Produto.Seguradora)
				   .Include(i => i.Vistoriador.Operador)
				   .Include(i => i.MovimentacaoProcesso)
				   .FirstAsync(i => i.Id == id);
		}
		public Task<Solicitacao> BuscarComContrato(int id)
		{
			return _dbSet.Include(i => i.Produto).ThenInclude(t => t.Contrato).ThenInclude(e => e.ContratoLancamento)
						.Include(c => c.Produto.Contrato.Produto.Seguradora)
						.FirstAsync(i => i.Id == id);
		}

		public Task<Solicitacao> BuscarParaExcluir(int id)
		{
			return _dbSet
					.Include(i => i.Vistoriador.Operador)
					.Include(i => i.Vistoriador.EnderecoBase)
					.Include(i => i.Analista.Operador)
					.Include(i => i.Solicitante.Operador)
					.Include(i => i.Produto.Seguradora)
					.Include(i => i.Produto.Contrato.ContratoLancamento).ThenInclude(c => c.ContratoLancamentoValor)
					.Include(i => i.Endereco)
					.Include(i => i.Cliente.ClienteEndereco)
					.Include(i => i.Cobertura)
					.Include(i => i.Comunicacao)
					.Include(i => i.Agendamento)
					.Include(i => i.LancamentoFinanceiro)
					.Include(i => i.OperadorCadastro)
					.Include(i => i.OperadorApropriado)
					.Include(i => i.MovimentacaoProcesso).ThenInclude(e => e.OperadorOrigem)
					.Include(i => i.AtividadeProcesso)
					.Include(i => i.Foto).ThenInclude(e => e.LaudoFoto)
					.AsNoTracking()
					.FirstAsync(w => w.Id == id);
		}
	}
}