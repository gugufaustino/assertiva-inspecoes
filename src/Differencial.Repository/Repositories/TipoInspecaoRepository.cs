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
    public class TipoInspecaoRepository : RepositoryBase<TipoInspecao>, ITipoInspecaoRepository
	{
		public TipoInspecaoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<TipoInspecao> Where<F>(F filter)
        {
            var query = from tipoInspecao in _db.TipoInspecao select tipoInspecao;
            this.AplicarFiltro(ref query, filter as TipoInspecaoFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<TipoInspecao> query, TipoInspecaoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.Nome.IsNullOrEmpty() == false)
				query = query.Where(x => x.NomeTipoInspecao.Contains(filter.Nome));

			if (filter.IndAtivo.HasValue)
				query = query.Where(x => filter.IndAtivo == x.IndAtivo);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}