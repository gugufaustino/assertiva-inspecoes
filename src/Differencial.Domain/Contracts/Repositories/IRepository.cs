using Differencial.Domain.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface IRepository<T> : IDisposable, IRepositoryIncludable<T>, IRepositoryQueryable<T>
        where T : class                
    {
        int IdOperador { get; }
        void Add(T entity);
        void AddRange(IEnumerable<T> entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(int[] ids);
        void Delete(T entity);

        T Find(int id);
        Task<T> FindAsync(int id);
        IEnumerable<T> All();
        IEnumerable<T> All(int skip = 0, int take = 25);
        IEnumerable<T> Where<F>(F filter) where F : class, IPagination;
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
   
    }

 
}