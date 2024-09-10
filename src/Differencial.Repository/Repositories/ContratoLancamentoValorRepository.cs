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
    public class ContratoLancamentoValorRepository : RepositoryBase<ContratoLancamentoValor>, IContratoLancamentoValorRepository
	{
		public ContratoLancamentoValorRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<ContratoLancamentoValor> Where<F>(F filter)
        {
            var query = from contratoLancamentoValor in _db.ContratoLancamentoValor select contratoLancamentoValor;
            this.AplicarFiltro(ref query, filter as ContratoLancamentoValorFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<ContratoLancamentoValor> query, ContratoLancamentoValorFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.IdContratoLancamento.HasValue)
				query = query.Where(x => filter.IdContratoLancamento == x.IdContratoLancamento);


			if (filter.TipoQuantitativoVariacao.HasValue)
				query = query.Where(x => filter.TipoQuantitativoVariacao == x.TipoQuantitativoVariacao);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}