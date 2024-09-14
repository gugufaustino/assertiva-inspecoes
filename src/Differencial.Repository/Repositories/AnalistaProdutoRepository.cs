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
    public class AnalistaProdutoRepository : RepositoryBase<AnalistaProduto>, IAnalistaProdutoRepository
	{
		public AnalistaProdutoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
			: base(dbContextFactory, usuario)
		{
		}
        public override IEnumerable<AnalistaProduto> Where<F>(F filter)
        {
            var query = from analistaProduto in _db.AnalistaProduto
                        select analistaProduto;

            this.AplicarFiltro(ref query, filter as AnalistaProdutoFilter);

            return query.ToList();
        }

        private void AplicarFiltro(ref IQueryable<AnalistaProduto> query, AnalistaProdutoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}