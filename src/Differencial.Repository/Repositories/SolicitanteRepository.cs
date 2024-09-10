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
    public class SolicitanteRepository : RepositoryBase<Solicitante>, ISolicitanteRepository
    {
        public SolicitanteRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
        }

        public IEnumerable<Solicitante> ddlSolicitante(int idSeguradora)
        {
            return _dbSet.Include(i => i.Operador)
                            .AsNoTracking()
                            .OrderBy(o => o.Operador.NomeOperador)
                            .ToList();
        }

        public override IEnumerable<Solicitante> Where<F>(F filter)
        {
            var query = from solicitante in _db.Solicitante select solicitante;
            this.AplicarFiltro(ref query, filter as SolicitanteFilter);
            return query.ToList();
        }

        private void AplicarFiltro(ref IQueryable<Solicitante> query, SolicitanteFilter filter)
        {
            // Ordenação
            string order = string.Empty;
            if (filter.CampoOrdenacao == CampoOrdenacaoSolicitante.NomeSolicitante)
            {
                query = filter.Order == Order.Ascending ? query.OrderBy(i => i.Operador.NomeOperador)
                                                        : query.OrderByDescending(i => i.Operador.NomeOperador);
            }
            else
            {
                order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
                query = query.OrderBy(order);
            }

            if (filter.Id.HasValue)
                query = query.Where(x => filter.Id == x.Id);

            if (filter.IdSeguradora.HasValue)
                query = query.Where(x => filter.IdSeguradora == x.IdSeguradora);

            if (filter.TipoSolicitante.HasValue)
                query = query.Where(x => filter.TipoSolicitante == x.TipoSolicitante);

            // Filtro
            base.ApplyBasicFilter(ref query, ref filter);
        }
    }
}