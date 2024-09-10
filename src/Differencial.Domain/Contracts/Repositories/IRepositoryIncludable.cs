using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Differencial.Domain.Contracts.Repositories
{
    public interface IRepositoryIncludable<T> : IRepositoryQueryable<T>
        where T : class
    {   
        IRepositoryIncludable<T, TProperty> Include<TProperty>([NotNull] Expression<Func<T, TProperty>> navigation);
        IRepositoryIncludable<T, TProperty> Include<TProperty>([NotNull] Expression<Func<T, ICollection<TProperty>>> navigation);
        
    }

    public interface IRepositoryIncludable<T, TPreviousProperty> : IRepositoryIncludable<T>
     where T : class
    {
        IRepositoryIncludable<T> ThenInclude<TProperty>([NotNull] Expression<Func<TPreviousProperty, TProperty>> navigation);

    }

}