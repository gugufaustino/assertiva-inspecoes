using Dapper;
using Dapper.Contrib.Extensions;
using Dapper.FluentMap;
using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Util;
using Differencial.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Differencial.Queries
{
    public class QueriesBase<T, F> : IQueryBase<T, F>
        where T : class, IKeyId
        where F : Pagination, new()
    {
        private readonly IUsuarioService _usuario;
        protected readonly IDbConnection _conn;
        protected string _sqlTemplatePaging;
        protected SqlBuilder _builder;
        protected string _sqlTemplate;
        private bool disposed;


        public int IdOperador
        {
            get
            {    //TODO: tratamento de catch' temporario 
                try { return _usuario.Id; } catch (Exception) { return 1; }
            }
        }

        public QueriesBase(IUsuarioService usuario, IConfiguracaoAplicativo configuracao)
        {
            #region templates sql
            _sqlTemplatePaging = @" SELECT /**select**/ ,TotalRecords = (COUNT(*) OVER())"
                              + @" FROM " + typeof(T).Name
                              + @" /**join**/ /**leftjoin**/ /**rigthjoin**/"
                              + @" /**where**/"
                              + @" /**orderby**/"
                              + @" OFFSET @Skip ROWS"
                              + @" FETCH NEXT @Take ROWS ONLY ";

            _sqlTemplate = @" SELECT /**select**/"
                   + @" FROM " + typeof(T).Name
                   + @" /**join**/ /**leftjoin**/ /**rigthjoin**/"
                   + @" /**where**/"
                   + @" /**orderby**/ ";
            #endregion

            if (FluentMapper.EntityMaps.IsEmpty)
            {
                FluentMapper.Initialize(c =>
                {
                    //c.AddMap(new LivroDapperMap());
                    //c.AddMap(new AutorDapperMap());
                    //c.ForDommel();
                });
            }

            SqlMapperExtensions.TableNameMapper = (T) =>  // do something here to pluralize the name of the type
            {
                return T.Name;
            };

            _usuario = usuario;
            //_conn = new SqlConnection(configuracao.ConnectionString);
            //_conn.Open();
        }

        protected SqlBuilder.Template BuildTemplate(bool enablePaging)
        {
            _builder = new SqlBuilder();
            return _builder.AddTemplate(enablePaging ? _sqlTemplatePaging : _sqlTemplate);
        }

        public T GetById(int id)
        {
            return _conn.Get<T>(id);
        }
        public IEnumerable<T> All()
        {
            return _conn.GetAll<T>();
        }

        public virtual IEnumerable<T> Where(F filter)
        {
            throw new NotImplementedException("Implements on concrete class");
        }

        public string MontarQuery(Expression<Func<T, bool>> predicate)
        {
            StringBuilder p = new(predicate.Body.ToString());
            var pName = predicate.Parameters.First();
            p.Replace(pName.Name + ".", "");
            p.Replace("==", "=");
            p.Replace("AndAlso", "and");
            p.Replace("OrElse", "or");
            p.Replace("\"", "\'");
            p.Replace("Convert", "");

            return p.ToString();
        }

        protected void AplicarOrdenacao(ref SqlBuilder builder, F filter)
        {
            builder.OrderBy(string.Format("{0} {1}"
                            , filter.OrderField
                            , filter.Order == Order.Ascending ? "ASC" : "DESC"));
        }
        protected void AplicarPaginacao(ref SqlBuilder builder, F filter, SqlBuilder.Template selector)
        {
            if (filter.EnablePaging)
            {
                builder.AddParameters(new { PageIndex = filter.Skip, PageSize = filter.Take });
                builder.AddParameters(new { filter.Skip, filter.Take });
                filter.TotalRecords = _conn.QueryFirst<F>(selector.RawSql, selector.Parameters).TotalRecords;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _conn.Close();
                    _conn.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            if (_conn != null)
                _conn.Close();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
