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
    public class VistoriadorProdutoRepository : RepositoryBase<VistoriadorProduto>, IVistoriadorProdutoRepository
	{
		public VistoriadorProdutoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

        public void Ativar(int Id)
        {
            var entity = base.Find(Id);
            entity.IndAtivo = true;
            base.Update(entity);
        }

        public void Desativar(int Id)
        {
            var entity = base.Find(Id);
            entity.IndAtivo = false;
            base.Update(entity);
        }

        public override IEnumerable<VistoriadorProduto> Where<F>(F filter)
		{
			var query = from vistoriadorProduto in _db.VistoriadorProduto
						select vistoriadorProduto;

			this.AplicarFiltro(ref query, filter as VistoriadorProdutoFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<VistoriadorProduto> query, VistoriadorProdutoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);

			if (filter.Id.HasValue)
				query = query.Where(x => filter.Id == x.Id);

			if (filter.IdVistoriador.HasValue)
				query = query.Where(x => filter.IdVistoriador == x.IdVistoriador);

			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}

	}
}