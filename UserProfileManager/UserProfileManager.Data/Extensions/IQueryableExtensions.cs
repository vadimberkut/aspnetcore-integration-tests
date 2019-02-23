using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UserProfileManager.Data.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string sortField, string orderType)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            string method = String.IsNullOrEmpty(orderType) || orderType == "asc" ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType,exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}
