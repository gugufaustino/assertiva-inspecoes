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
    public class VistoriadorRepository : RepositoryBase<Vistoriador>, IVistoriadorRepository
    {
        public VistoriadorRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
        }

        public Vistoriador Buscar(int idVistoriador)
        {
            return _dbSet
                    .Include(i => i.Operador)
                    .Include(i => i.VistoriadorProduto)
                    .Include(i => i.EnderecoBase)
                    .FirsNoTrackingt(i => i.Id == idVistoriador);
        }

        public IEnumerable<Vistoriador> ListarVistoriadorOperador(List<int> lstIdOperadores)
        {
            return _dbSet
                        .Include(i=> i.EnderecoBase)
                        .Include(i=> i.VistoriadorProduto)
                        .Include(i => i.Operador) 
                        .Where(op => lstIdOperadores.Contains(op.Id)).ToList();
        }

        public IEnumerable<Vistoriador> ListarVistoriadorPorProduto(int idProduto)
        {
            return _dbSet
                        .Include(i => i.Operador)
                        .Include(i => i.EnderecoBase)
                        .Where(w => w.IndAtivo == true && w.VistoriadorProduto.Any(v => v.IndAtivo && v.IdProduto == idProduto));
        }

        public override IEnumerable<Vistoriador> Where<F>(F filter)
        {
            var query = from vistoriador in _db.Vistoriador
                        select vistoriador;

            this.AplicarFiltro(ref query, filter as VistoriadorFilter);

            return query.ToList();
        }

        private void AplicarFiltro(ref IQueryable<Vistoriador> query, VistoriadorFilter filter)
        {
            // Ordenação
            string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
            query = query.OrderBy(order);

            if (filter.Id.HasValue)
                query = query.Where(x => filter.Id == x.Id);

            if (filter.IdOperador.HasValue)
                query = query.Where(x => filter.IdOperador == x.IdOperador);

            // Filtro
            base.ApplyBasicFilter(ref query, ref filter);
        }
    }
}