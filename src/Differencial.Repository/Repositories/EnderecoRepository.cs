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
    public class EnderecoRepository : RepositoryBase<Endereco>, IEnderecoRepository
	{
		public EnderecoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

		public override IEnumerable<Endereco> Where<F>(F filter)
		{
			var query = from endereco in _db.Endereco
						select endereco;

			this.AplicarFiltro(ref query, filter as EnderecoFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<Endereco> query, EnderecoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.Cep.IsNullOrEmpty() == false)
				query = query.Where(x => x.Cep.Contains(filter.Cep));

			if (filter.Logradouro.IsNullOrEmpty() == false)
				query = query.Where(x => x.Logradouro.Contains(filter.Logradouro));

			if (filter.Numero.HasValue)
				query = query.Where(x => filter.Numero == x.Numero);

			if (filter.Bairro.IsNullOrEmpty() == false)
				query = query.Where(x => x.Bairro.Contains(filter.Bairro));

			if (filter.NomeMunicipio.IsNullOrEmpty() == false)
				query = query.Where(x => x.NomeMunicipio.Contains(filter.NomeMunicipio));

			if (filter.SiglaUf.IsNullOrEmpty() == false)
				query = query.Where(x => x.SiglaUf.Contains(filter.SiglaUf));

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}