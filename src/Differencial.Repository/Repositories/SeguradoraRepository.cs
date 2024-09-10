using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Repositories
{
    public class SeguradoraRepository : RepositoryBase<Seguradora>, ISeguradoraRepository
    {
        public SeguradoraRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
        }

        public override IEnumerable<Seguradora> Where<F>(F filter)
        {
            var query = from seguradora in _db.Seguradora select seguradora;
            this.AplicarFiltro(ref query, filter as SeguradoraFilter);
            return query.ToList();
        }

        private void AplicarFiltro(ref IQueryable<Seguradora> query, SeguradoraFilter filter)
        {
            // Ordenação
            string order = string.Format("{0} {1}", filter.CampoOrdenacao.ToString(), filter.Order.ToString());
            query = query.OrderBy(order);

            if (filter.Id.HasValue)
                query = query.Where(x => filter.Id == x.Id);

            if (filter.DataCadastro.HasValue)
                query = query.Where(x => filter.DataCadastro == x.DataCadastro);

            if (filter.DataModificacao.HasValue)
                query = query.Where(x => filter.DataModificacao == x.DataModificacao);

            if (filter.IndIntegracaoSolicitacaoPorEmail.HasValue)
                query = query.Where(x => filter.IndIntegracaoSolicitacaoPorEmail == x.IndIntegracaoSolicitacaoPorEmail);

            if (filter.IndAtivo.HasValue)
                query = query.Where(x => filter.IndAtivo == x.IndAtivo);


            // Filtro
            base.ApplyBasicFilter(ref query, ref filter);
        }

        public override Seguradora Find(int id)
        {
            return _dbSet
                    .Include(i => i.Endereco)
                    .FirsNoTrackingt(i => i.Id == id);

        }
    }
}