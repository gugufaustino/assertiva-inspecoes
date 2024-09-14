using Differencial.Domain.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Differencial.Repository.Repositories.Base
{
    public static class RepositoryQueryableExtensions
    {
        public static T FirstOrDefaultNoTracking<T>(this DbSet<T> _dbSet, Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return _dbSet.AsNoTracking().FirstOrDefault(predicate);
        }
        public static T FirstOrDefaultNoTracking<T>(this IQueryable<T> _query, Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return _query.AsNoTracking().FirstOrDefault(predicate);
        }
        public static async Task<T> FirstOrDefaultNoTrackingAsync<T>(this IQueryable<T> _query, Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return await _query.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public static T FirsNoTrackingt<T>(this DbSet<T> _dbSet, Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return _dbSet.AsNoTracking().First(predicate);
        }
        public static T FirsNoTrackingt<T>(this IQueryable<T> _query, Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return _query.AsNoTracking().First(predicate);
        }
        public static T GetById<T>(this IQueryable<T> _query, int id) where T : class, IEntity
        {
            return _query.AsNoTracking().First(i => i.Id == id);
        }
        public static T GetById<T>(this DbSet<T> _dbSet, int id) where T : class, IEntity
        {
            return _dbSet.AsNoTracking().First(i => i.Id == id);
        }

        public static IQueryable<T> Filter<T, TFilter>(this IQueryable<T> query,
                                                        TFilter filter,
                                                        Func<IQueryable<T>, TFilter, IQueryable<T>> filterLogicFunc)
            where T : class, IEntity
            where TFilter : class, IPagination
        {
            query = filterLogicFunc(query, filter);
            var pg = filter as Domain.Filters.Pagination; // TODO criar interface para não precisar fazer esse cast
            query = ApplyBasePagination(query, ref pg);

            return query;
        }


        public static IQueryable<T> ApplyBasePagination<T, TPagination>(IQueryable<T> query, ref TPagination pagination)
            where T : class, IEntity
            where TPagination : class, IPagination, new()
        {
            pagination ??= new();

            if (pagination.EnablePaging)
            {
                pagination.TotalRecords = query.Count();
                query = query.Skip(pagination.Skip).Take(pagination.Take);
            }

            return query;
        }

    }

}
