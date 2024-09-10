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
	public class TipoAssuntoRepository : RepositoryBase<TipoAssunto>, ITipoAssuntoRepository
	{
		public TipoAssuntoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<TipoAssunto> Where<F>(F filter)
		{
			var query = from tipoAssunto in _db.TipoAssunto
						select tipoAssunto;

			this.AplicarFiltro(ref query, filter as TipoAssuntoFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<TipoAssunto> query, TipoAssuntoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.NomeAssunto.IsNullOrEmpty() == false)
				query = query.Where(x => x.NomeAssunto.Contains(filter.NomeAssunto));

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}