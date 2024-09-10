using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Differencial.Repository
{
    public class Includes<T> : IRepositoryIncludable<T>, IRepositoryQueryable<T>
      where T : class, IEntity
    {

        protected IRepository<T> _repository;
        protected IQueryable<T> _query;

        public Includes(IRepository<T> repository)
        {
            _repository = repository;
            _query = ((Repositories.RepositoryBase<T>)repository)._dbSet.AsQueryable();
        }

        public IRepositoryIncludable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigation)
        {
            var queryThan = _query.Include(navigation);

            return new IncludesThan<T, TProperty>(_repository, queryThan);
        }

        public IRepositoryIncludable<T, TProperty> Include<TProperty>([NotNull] Expression<Func<T, ICollection<TProperty>>> navigation)
        {
            var queryThan = _query.Include(navigation);

            return new IncludesThan<T, TProperty>(_repository, queryThan);
        }


        public T FirstOrDefault(Expression<Func<T, bool>> predicate) => _query.FirstOrDefaultNoTracking(predicate);
        public T First(Expression<Func<T, bool>> predicate) => _query.FirsNoTrackingt(predicate);
        public T GetById(int Id) => _query.GetById(Id);

    }


    public class IncludesThan<T, TPreviousProperty> : Includes<T>, IRepositoryIncludable<T, TPreviousProperty>
        where T : class, IEntity
    {
        public IncludesThan(IRepository<T> repository, IIncludableQueryable<T, TPreviousProperty> queryThen) : base(repository)
        {
            _query = queryThen;
        }

        public IncludesThan(IRepository<T> repository, IIncludableQueryable<T, ICollection<TPreviousProperty>> queryThan) : base(repository)
        {
            _query = queryThan;
        }

        public IRepositoryIncludable<T> ThenInclude<TProperty>([NotNull] Expression<Func<TPreviousProperty, TProperty>> navigation)
        {

            if (_query is IIncludableQueryable<T, TPreviousProperty>)
                _query = (_query as IIncludableQueryable<T, TPreviousProperty>).ThenInclude(navigation);

            else if (_query is IIncludableQueryable<T, ICollection<TPreviousProperty>>)
                _query = (_query as IIncludableQueryable<T, ICollection<TPreviousProperty>>).ThenInclude(navigation);
            else
                throw new NotImplementedException("Case Not Implemented at ThenInclude");
  

            return this;
        }
        public IRepositoryIncludable<T> ThenInclude<TProperty>([NotNull] Expression<Func<ICollection<TPreviousProperty>, TProperty>> navigation)
        {
            throw new NotImplementedException();
        }
    }



}
