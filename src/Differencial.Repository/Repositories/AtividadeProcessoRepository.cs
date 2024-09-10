using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
	public class AtividadeProcessoRepository : RepositoryBase<AtividadeProcesso>, IAtividadeProcessoRepository
	{
		public AtividadeProcessoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<AtividadeProcesso> Where<F>(F filter)
        {
            var query = from atividadeProcesso in _db.AtividadeProcesso select atividadeProcesso;
            this.AplicarFiltro(ref query, filter as AtividadeProcessoFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<AtividadeProcesso> query, AtividadeProcessoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.NomeAtividadeProcesso.IsNullOrEmpty() == false)
				query = query.Where(x => x.NomeAtividadeProcesso.Contains(filter.NomeAtividadeProcesso));

			if (filter.IdSolicitacao.HasValue)
				query = query.Where(x => filter.IdSolicitacao == x.IdSolicitacao);

			if (filter.IdOperadorConcluida.HasValue)
				query = query.Where(x => filter.IdOperadorConcluida == x.IdOperadorConcluida);

			if (filter.TipoSituacaoAtividade.HasValue)
				query = query.Where(x => filter.TipoSituacaoAtividade ==  x.TipoSituacaoAtividade);

			if (filter.DthAssinada.HasValue)
				query = query.Where(x => filter.DthAssinada == x.DthAssinada);

			if (filter.DthDelegada.HasValue)
				query = query.Where(x => filter.DthDelegada == x.DthDelegada);

			if (filter.DthConcluida.HasValue)
				query = query.Where(x => filter.DthConcluida == x.DthConcluida);

			if (filter.IndRetrabalho.HasValue)
				query = query.Where(x => filter.IndRetrabalho == x.IndRetrabalho);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}