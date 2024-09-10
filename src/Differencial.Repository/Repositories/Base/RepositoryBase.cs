using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Filters;
using Differencial.Domain.Resources;
using Differencial.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Differencial.Repository.Repositories
{
    public class RepositoryBase<T> : IRepository<T>
        where T : class, IEntity
    {
        internal DifferencialContext _db;
        public DbSet<T> _dbSet;
        public IUsuarioService _usuario;

        public int IdOperador
        {//TODO: tratamento de catch' temporario 
            get { try { return _usuario.Id; } catch (Exception) { return 1; } }
        }

        public RepositoryBase(IDbContextFactory context, IUsuarioService usuario)
        {
            _db = context.GetDbContext();
            _dbSet = _db.Set<T>();

            _usuario = usuario;
        }

        public void Add(T entity)
        {
            entity.DataCadastro = DateTime.Now;
            entity.DataModificacao = DateTime.Now;
            entity.IdOperadorCadastro = IdOperador;
            entity.IdOperadorModificacao = IdOperador;
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entity)
        {
            foreach (var item in entity)
            {
                item.DataCadastro = DateTime.Now;
                item.DataModificacao = DateTime.Now;
                item.IdOperadorCadastro = IdOperador;
                item.IdOperadorModificacao = IdOperador;
            }
            _dbSet.AddRange(entity);
        }

        public void Update(T entity)
        {
            entity.DataModificacao = DateTime.Now;
            entity.IdOperadorModificacao = IdOperador;

            var old = _dbSet.Find(entity.Id);
            if (old == null)// Case seja adicionado entidade com entidade herdada
            {
                Add(entity);
            }
            else
            {
                entity.IdOperadorCadastro = old.IdOperadorCadastro;
                entity.DataCadastro = old.DataCadastro;
                _db.Entry(old).CurrentValues.SetValues(entity);
                _db.Entry(old).State = EntityState.Modified;
            }
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _db.Entry(entity).State = EntityState.Deleted;
        }
        public virtual void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            _db.Entry(entity).State = EntityState.Deleted;

        }

        public void Delete(int[] ids)
        {
            var entity = _dbSet.Where<T>(x => ids.Contains(x.Id)).AsEnumerable().ToList();
            _dbSet.RemoveRange(entity);

        }

        internal void Delete(Expression<Func<T, bool>> predicate)
        {
            var entity = _dbSet.First(predicate);
            _dbSet.Remove(entity);
            _db.Entry(entity).State = EntityState.Deleted;
        }

        public virtual T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public async virtual Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefaultNoTracking(predicate);
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirsNoTrackingt(predicate);
        }

        public T GetById(int Id)
        {
            return _dbSet.GetById(Id);
        }

        public IEnumerable<T> All(int skip = 0, int take = 25)
        {
            return _dbSet.OrderBy(x => x.Id).Skip(skip).Take(take).AsNoTracking();
        }

        public IEnumerable<T> All()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking();
        }

        public virtual IEnumerable<T> Where<F>(F filter) where F : class, IPagination
        {
            throw new NotImplementedException(MensagensErro.ErroRepositorioBaseNaoImplementado);
        }

        /// <summary>
        /// Filtro básico que incluir ordem, numero de registros e situação diferente de Excluido para retorno e posição
        /// </summary>
        /// <param name="query">Query de consulta</param>
        /// <param name="pagination">Filtro de consulta</param>
        protected virtual void ApplyBasicFilter<F>(ref IQueryable<T> query, ref F pagination)
              where F : Pagination, new()
        {
            query = RepositoryQueryableExtensions.ApplyBasePagination(query, ref pagination);
        }

        private bool disposed = false;

        public IRepositoryIncludable<T, TProperty> Include<TProperty>([NotNull] Expression<Func<T, TProperty>> navigation)
        {
            var queryThan = _dbSet.AsQueryable().Include(navigation);
            return new IncludesThan<T, TProperty>(this, queryThan);
        }
        public IRepositoryIncludable<T, TProperty> Include<TProperty>([NotNull] Expression<Func<T, ICollection<TProperty>>> navigation)
        {
            var queryThan = _dbSet.AsQueryable().Include(navigation);
            return new IncludesThan<T, TProperty>(this, queryThan);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

  
    }


}