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
    public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
	{
		public ClienteRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<Cliente> Where<F>(F filter)
        {
            var query = from cliente in _db.Cliente select cliente;
            this.AplicarFiltro(ref query, filter as ClienteFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<Cliente> query, ClienteFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.CpfCnpj.IsNullOrEmpty() == false)
				query = query.Where(x => x.CpfCnpj.Contains(filter.CpfCnpj));

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}