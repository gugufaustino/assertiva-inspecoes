using Dapper;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.Queries;
using Differencial.Queries;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace Differencial.Queries.Queries
{
    public class LogAuditoriaQueries : QueriesBase<LogAuditoria, LogAuditoriaFilter>, ILogAuditoriaQueries
    {
        public LogAuditoriaQueries(IUsuarioService usuario,
                                        IConfiguracaoAplicativo configuracaoAplicativo)
            : base(usuario, configuracaoAplicativo)
        {
        }
        public LogAuditoria FirstOrDefault(Expression<Func<LogAuditoria, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        new public IEnumerable<LogAuditoria> Where(LogAuditoriaFilter filter)
        {
            var selector = BuildTemplate(filter.EnablePaging);
            _builder.Select("*");
            AplicarOrdenacao(ref _builder, filter);
            AplicarFiltro(ref _builder, filter);
            AplicarPaginacao(ref _builder, filter, selector);
            var sql = selector.RawSql;
            return _conn.Query<LogAuditoria>(sql, selector.Parameters);
        }
        private void AplicarFiltro(ref SqlBuilder builder, LogAuditoriaFilter filter)
        {
            List<string> where = new();
            List<dynamic> parameters = new();

            if (filter.IdTabela.HasValue)
                builder.Where("IdTabela = @IdTabela", new { filter.IdTabela });

            if (!string.IsNullOrEmpty(filter.Tabela))
                builder.Where("Tabela = @Tabela", new { filter.Tabela });

            if (filter.UsuarioAplicacao.HasValue)
                builder.Where("UsuarioAplicacao = @UsuarioAplicacao", new { filter.UsuarioAplicacao });

            //montarQuery(x => x.IdTabela > 9);
        }
    }
}