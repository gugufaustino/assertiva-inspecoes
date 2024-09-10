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
	public class MovimentacaoProcessoRepository : RepositoryBase<MovimentacaoProcesso>, IMovimentacaoProcessoRepository
	{
		public MovimentacaoProcessoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<MovimentacaoProcesso> Where<F>(F filter)
        {
            var query = from movimentacaoProcesso in _db.MovimentacaoProcesso select movimentacaoProcesso;
            this.AplicarFiltro(ref query, filter as MovimentacaoProcessoFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<MovimentacaoProcesso> query, MovimentacaoProcessoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.TextoMovimentacao.IsNullOrEmpty() == false)
				query = query.Where(x => x.TextoMovimentacao.Contains(filter.TextoMovimentacao));

			if (filter.TipoSituacaoProcesso.HasValue)
				query = query.Where(x => filter.TipoSituacaoProcesso == x.TipoSituacaoProcesso);

			if (filter.IdOperadorOrigem.HasValue)
				query = query.Where(x => filter.IdOperadorOrigem == x.IdOperadorOrigem);

			if (filter.IdOperadorDestino.HasValue)
				query = query.Where(x => filter.IdOperadorDestino == x.IdOperadorDestino);

			if (filter.DthMovimentacao.HasValue)
				query = query.Where(x => filter.DthMovimentacao == x.DthMovimentacao);

			if (filter.DthApropriacao.HasValue)
				query = query.Where(x => filter.DthApropriacao == x.DthApropriacao);

			if (filter.DthConclusao.HasValue)
				query = query.Where(x => filter.DthConclusao == x.DthConclusao);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}