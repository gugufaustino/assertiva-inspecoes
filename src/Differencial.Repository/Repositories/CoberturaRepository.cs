using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Repository.Context;
using Differencial.Repository.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
    public class CoberturaRepository : RepositoryBase<Cobertura>, ICoberturaRepository
	{
		public CoberturaRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<Cobertura> Where<F>(F filter)
        {
            var query = from cobertura in _db.Cobertura select cobertura;
            this.AplicarFiltro(ref query, filter as CoberturaFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<Cobertura> query, CoberturaFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.NomeCobertura.IsNullOrEmpty() == false)
				query = query.Where(x => x.NomeCobertura.Contains(filter.NomeCobertura));

			if (filter.IdSolicitacao.HasValue)
				query = query.Where(x => filter.IdSolicitacao == x.IdSolicitacao);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}