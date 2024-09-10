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
	public class NotificacaoRepository : RepositoryBase<Notificacao>, INotificacaoRepository
	{
		public NotificacaoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<Notificacao> Where<F>(F filter)
		{
			var query = from notificacao in _db.Notificacao
						select notificacao;

			this.AplicarFiltro(ref query, filter as NotificacaoFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<Notificacao> query, NotificacaoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.Descricao.IsNullOrEmpty() == false)
				query = query.Where(x => x.Descricao.Contains(filter.Descricao));

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}