using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using Differencial.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
    public class AnalistaRepository : RepositoryBase<Analista>, IAnalistaRepository
	{
		public AnalistaRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
			: base(dbContextFactory, usuario)
		{
		}

		public override IEnumerable<Analista> Where<F>(F filter)
		{
			var query = _dbSet
						.Include(i => i.Operador)
						.AsNoTracking();

			this.AplicarFiltro(ref query, filter as AnalistaFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<Analista> query, AnalistaFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}