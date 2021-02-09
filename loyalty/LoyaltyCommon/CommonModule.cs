using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Palmary.Loyalty.Common
{
    public static class Extend
    {
        public static string ToJson<T>(this T roleSearchFilter)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(roleSearchFilter,timeFormat);
        }

        //Extend string function
        public static string ToJson(this string data)
        {
            data = data.StartsWith("{") ? data : string.Format("{{{0}", data);
            data = data.EndsWith("}") ? data : string.Format("{0}}}", data);
            return data;
        }

        // Get name of property
        // suggest to use UtilityManager, GetPropertyName() or 
        // example of use: 
        // var value = new { user_profile.name }.GetName();
        // var value2 = new { new UserObject().name }.GetName();
        public static string GetPropertyName<T>(this T item) where T : class
        {
            if (item == null)
                return string.Empty;

            return typeof(T).GetProperties()[0].Name;
        }


    }
}