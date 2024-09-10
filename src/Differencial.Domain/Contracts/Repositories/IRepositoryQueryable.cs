using System;
using System.Linq.Expressions;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface IRepositoryQueryable<T>
        where T : class
    {
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T First(Expression<Func<T, bool>> predicate);
        T GetById(int Id);


    }
}