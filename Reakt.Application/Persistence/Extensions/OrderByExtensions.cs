using System;
using System.Linq;
using System.Linq.Expressions;

namespace Reakt.Application.Persistence.Extensions
{
    public static class OrderByExtensions
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string propertyName, bool ascending)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return q;
            }
            try
            {
                var param = Expression.Parameter(typeof(T), "p");
                var exp = Expression.Lambda(Expression.Property(param, propertyName), param);
                string method = ascending ? "OrderBy" : "OrderByDescending";
                Type[] types = new Type[] { q.ElementType, exp.Body.Type };
                return q.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable), method, types, q.Expression, exp));
            }
            catch (ArgumentException)
            {
                return q;
            }
        }
    }
}