using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Repository.Context;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System;
using Differencial.Domain.Contracts.Entities;

namespace Differencial.Repository.Repositories
{
    public class LogAuditoriaRepository : RepositoryBase<LogAuditoria>, ILogAuditoriaRepository
    {
        public LogAuditoriaRepository(IDbContextFactory dbContextFactory, IUsuarioService usuario)
            : base(dbContextFactory, usuario)
        {
        }



        public override IEnumerable<LogAuditoria> Where<TFilter>(TFilter filter)
        {
            var query = _db.LogAuditoria
                        .Include(i => i.Operador)
                        .AsNoTracking()
                        .Filter(filter, LogicFilter);

            return query.ToList();
        }


        public Func<IQueryable<LogAuditoria>, IPagination, IQueryable<LogAuditoria>> LogicFilter = (query, pagin) =>
        {
            LogAuditoriaFilter filter = (LogAuditoriaFilter)pagin;

            //Ordenação
            string order = string.Format("{0} {1}", filter.OrderField, filter.Order);
            query = query.OrderBy(order);

            if (filter.IdTabela.HasValue)
                query = query.Where(x => filter.IdTabela == x.IdTabela);

            if (!string.IsNullOrEmpty(filter.Tabela))
                query = query.Where(x => filter.Tabela == x.Tabela);

            if (filter.UsuarioAplicacao.HasValue)
                query = query.Where(x => filter.UsuarioAplicacao == x.UsuarioAplicacao);

            return query;

        };
 
    } 


}