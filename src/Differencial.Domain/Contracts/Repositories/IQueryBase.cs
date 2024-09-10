using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace Differencial.Domain.Contracts.Repositories
{
    public interface IQueryBase<TEntity, TFilter> : IDisposable
        where TEntity : class
        where TFilter : class
    {
        int IdOperador { get; }
        TEntity GetById(int id);
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> Where(TFilter filter);
         
    }
}