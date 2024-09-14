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
    public class ClienteEnderecoRepository : RepositoryBase<ClienteEndereco>, IClienteEnderecoRepository
	{
		public ClienteEnderecoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<ClienteEndereco> Where<F>(F filter)
        {
            var query = from clienteEndereco in _db.ClienteEndereco select clienteEndereco;
            this.AplicarFiltro(ref query, filter as ClienteEnderecoFilter);
            return query.ToList();
        }

		private void AplicarFiltro(ref IQueryable<ClienteEndereco> query, ClienteEnderecoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.IdCliente.HasValue)
				query = query.Where(x => filter.IdCliente == x.IdCliente);

			if (filter.IdEndereco.HasValue)
				query = query.Where(x => filter.IdEndereco == x.IdEndereco);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}