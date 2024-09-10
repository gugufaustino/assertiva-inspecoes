using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace Differencial.Repository.Repositories
{
    public class NotificacaoOperadorRepository : RepositoryBase<NotificacaoOperador>, INotificacaoOperadorRepository
	{
		public NotificacaoOperadorRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

        public IEnumerable<NotificacaoOperador> MinhasNotificacoes(int idOperador)
        {
			return _dbSet
					.Include(i => i.Notificacao)
					.AsNoTracking()
					.Where(i => i.IdOperador == IdOperador).ToList();

		}

        public override IEnumerable<NotificacaoOperador> Where<F>(F filter)
        {
            var query = from notificacaoOperador in _db.NotificacaoOperador select notificacaoOperador;
            this.AplicarFiltro(ref query, filter as NotificacaoOperadorFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<NotificacaoOperador> query, NotificacaoOperadorFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.IndLido.HasValue)
				query = query.Where(x => filter.IndLido == x.IndLido);

			if (filter.IdNotificacao.HasValue)
				query = query.Where(x => filter.IdNotificacao == x.IdNotificacao);

			if (filter.IdOperador.HasValue)
				query = query.Where(x => filter.IdOperador == x.IdOperador);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}