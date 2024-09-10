using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
    public class LaudoRepository : RepositoryBase<Laudo>, ILaudoRepository
	{
		public LaudoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<Laudo> Where<F>(F filter)
        {
            var query = from laudo in _db.Laudo select laudo;
            this.AplicarFiltro(ref query, filter as LaudoFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<Laudo> query, LaudoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}