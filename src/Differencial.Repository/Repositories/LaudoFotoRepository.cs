using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using Differencial.Repository.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
    public class LaudoFotoRepository : RepositoryBase<LaudoFoto>, ILaudoFotoRepository
	{
		public LaudoFotoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<LaudoFoto> Where<F>(F filter)
		{
			var query = from laudoFoto in _db.LaudoFoto
						select laudoFoto;

			this.AplicarFiltro(ref query, filter as LaudoFotoFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<LaudoFoto> query, LaudoFotoFilter filter)
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