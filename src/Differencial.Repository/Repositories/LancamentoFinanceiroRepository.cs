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
    public class LancamentoFinanceiroRepository : RepositoryBase<LancamentoFinanceiro>, ILancamentoFinanceiroRepository
	{
		public LancamentoFinanceiroRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
			: base(dbContextFactory, usuario)
		{
		}

		public override IEnumerable<LancamentoFinanceiro> Where<F>(F filter)
        {
            var query = from lancamentoFinanceiro in _db.LancamentoFinanceiro select lancamentoFinanceiro;
            this.AplicarFiltro(ref query, filter as LancamentoFinanceiroFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<LancamentoFinanceiro> query, LancamentoFinanceiroFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}