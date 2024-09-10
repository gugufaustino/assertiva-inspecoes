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
    public class ContratoLancamentoRepository : RepositoryBase<ContratoLancamento>, IContratoLancamentoRepository
	{
		public ContratoLancamentoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<ContratoLancamento> Where<F>(F filter)
        {
            var query = from contratoLancamento in _db.ContratoLancamento select contratoLancamento;
            this.AplicarFiltro(ref query, filter as ContratoLancamentoFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<ContratoLancamento> query, ContratoLancamentoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.IdContrato.HasValue)
				query = query.Where(x => filter.IdContrato == x.IdContrato);

			if (filter.TipoContratoLancamento.HasValue)
				query = query.Where(x => filter.TipoContratoLancamento == x.TipoContratoLancamento);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}