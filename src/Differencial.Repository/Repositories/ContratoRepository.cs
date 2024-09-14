using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using Differencial.Repository.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
    public class ContratoRepository : RepositoryBase<Contrato>, IContratoRepository
	{
		public ContratoRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
		}

        public Contrato BuscarPorProduto(int idProduto)
        {
			return _dbSet
						.Include(i => i.Produto).ThenInclude(i =>i.Seguradora)
						.Include(i => i.ContratoLancamento).ThenInclude(i=> i.ContratoLancamentoValor)
						.Single(i => i.Produto.Id == idProduto);
        }

        public override IEnumerable<Contrato> Where<F>(F filter)
		{
			var query = from contrato in _db.Contrato
						select contrato;

			this.AplicarFiltro(ref query, filter as ContratoFilter);

			return query.ToList();
		}

		private void AplicarFiltro(ref IQueryable<Contrato> query, ContratoFilter filter)
		{
			// Ordenação
			string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
			query = query.OrderBy(order);
             
 
			// Filtro
			base.ApplyBasicFilter(ref query, ref filter);
		}
	}
}