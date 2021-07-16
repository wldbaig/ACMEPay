using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace API.Common
{
    public static class TransactionExtention
    {
        #region FirstOrDefault
        public static T FirstOrDefaultReadUncommitted<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }))
            {
                var record = query.FirstOrDefault();
                scope.Complete();
                return record;
            }
        }

        public static async Task<T> FirstOrDefaultReadUncommittedAsync<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var record = await query.FirstOrDefaultAsync();
                scope.Complete();
                return record;
            }
        }
        public static async Task<T> FirstOrDefaultReadUncommittedAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var record = await query.FirstOrDefaultAsync(predicate);
                scope.Complete();
                return record;
            }
        }
        #endregion FirstOrDefault

        #region ToList

        public static List<T> ToListReadUncommitted<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }))
            {
                var record = query.ToList();
                scope.Complete();
                return record;
            }
        }

        public static List<T> ToListReadUncommitted<T>(this IEnumerable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }))
            {
                var record = query.ToList();
                scope.Complete();
                return record;
            }
        }

        public static async Task<List<T>> ToListReadUncommittedAsync<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var record = await query.ToListAsync();
                scope.Complete();
                return record;
            }
        }

        #endregion ToList

        #region Any
        public static bool AnyReadUncommitted<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }))
            {
                var record = query.Any(predicate);
                scope.Complete();
                return record;
            }
        }

        public static async Task<bool> AnyReadUncommittedAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var record = await query.AnyAsync(predicate);
                scope.Complete();
                return record;
            }
        }
        #endregion Any

        #region Find
        public static T FindReadUncommitted<T>(this DbSet<T> dbSet, params object[] keyValues) where T : class
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }))
            {
                var record = dbSet.Find(keyValues);
                scope.Complete();
                return record;
            }
        }

        public static async Task<T> FindReadUncommittedAsync<T>(this DbSet<T> dbSet, params object[] keyValues) where T : class
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var record = await dbSet.FindAsync(keyValues);
                scope.Complete();
                return record;
            }
        }
        #endregion Find
    }
}
