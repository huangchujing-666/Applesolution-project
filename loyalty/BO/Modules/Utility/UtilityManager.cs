using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.Modules.Utility
{
    public static class UtilityManager
    {
        // eg: var propertyName = GetPropertyName(() => userA.name);
        public static string GetPropertyName<TValue>(Expression<Func<TValue>> propertyId)
        {
            return ((MemberExpression)propertyId.Body).Member.Name;
        }

        // Get value of DisplayNameAttribute
        // eg: GetPropertyDisplayName(() => userA.name);
        public static string GetPropertyDisplayName<TValue>(Expression<Func<TValue>> propertyId)
        {
            var body = ((MemberExpression)propertyId.Body);

            var objectClass = body.Expression.Type;
            var propertyName = body.Member.Name;

            PropertyInfo[] propInfos = objectClass.GetProperties();
            var theProp = propInfos.Where(x => x.Name == propertyName).First();
            if (theProp == null)
                return "";

            DisplayNameAttribute theAttr = (DisplayNameAttribute)Attribute.GetCustomAttribute(theProp, typeof(DisplayNameAttribute));

            if (theAttr == null)
                return "";
            else
                return theAttr.DisplayName;
        }
    }
}

