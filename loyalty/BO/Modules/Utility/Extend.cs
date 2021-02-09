using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.Modules.Administration.Table;

namespace Palmary.Loyalty.BO.Modules.Utility
{
    public static class Extend
    {
        // Get name of listing item by int value
        public static string ToListingItemName(this int itemValue, string itemCode)
        {
            TableManager _tableManager = new TableManager();
            string itemName = _tableManager.GetListingItemName(itemCode, itemValue);

            return itemName;
        }

        // Linq to SQL, dynamic order by
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                      bool desc) where TEntity : class
        {

            string command = desc ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);

            var property = type.GetProperty(orderByProperty);

            var parameter = Expression.Parameter(type, "p");

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },

                                   source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<TEntity>(resultExpression);

        }
    }
}
