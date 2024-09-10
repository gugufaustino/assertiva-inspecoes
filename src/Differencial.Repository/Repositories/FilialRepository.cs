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
	public class FilialRepository : RepositoryBase<Filial>, IFilialRepository
	{
		public FilialRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
			: base(dbContextFactory, usuario)
		{
		}

		public override IEnumerable<Filial> Where<F>(F filter)
        {
            var query = from filial in _db.Filial select filial;
            this.AplicarFiltro(ref query, filter as FilialFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<Filial> query, FilialFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.IdSeguradora.HasValue)
				query = query.Where(x => filter.IdSeguradora == x.IdSeguradora);

			if (filter.NomeFilial.IsNullOrEmpty() == false)
				query = query.Where(x => x.NomeFilial.Contains(filter.NomeFilial));

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}