using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Administration.Table;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.Web_backend.Modules.Utility
{
    public static class Extend
    {
      
        //Extend int function
        //public static string ToItemName(string itemCode, int itemValue)
        //{
        //    System.Diagnostics.Debug.WriteLine("GetStatus(this int status, string itemCode)");
        //    System.Diagnostics.Debug.WriteLine("itemCode: " + itemCode + ", itemValue: " + itemValue);
        //    TableManager _tableManager = new TableManager();
        //    string itemName = _tableManager.GetListingItemName(SessionManager.Current.user_id, itemCode, itemValue);
        //    System.Diagnostics.Debug.WriteLine("itemName: " + itemName);
        //    return itemName;
        //}

        // usage: 
        // display_value = member.status.ToItemName("Status")
        // or Extend.ToItemName("Salutation", member.salutation)
        public static string ToItemName(this int itemValue, string itemCode)
        {      
            TableManager _tableManager = new TableManager();   
            string itemName = _tableManager.GetListingItemName(itemCode, itemValue);

            return itemName;
        }

        //public static string GetDistrict(this int districtId)
        //{
        //    return GetListItemName("district", districtId);
        //}
        //private static string GetListItemName(string code, int id)
        //{

        //    prLoyaltyDBDataContext db = new LoyaltyDBDataContext();

        //    var district = _entities.listing.FirstOrDefault(x => x.code.ToLower() == code.ToLower() && x.status == 1);
        //    if (district != null)
        //    {
        //        var districtName =
        //            _entities.listing_item.FirstOrDefault(x => x.list_id == district.list_id && x.value == id && x.status == 1);
        //        return districtName == null ? "" : districtName.name;
        //    }
        //    return "";
        //}

        //public static T ToOjbect<T>(this string str) where T : class , new()
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(str))
        //        {
        //            return null;
        //        }
        //        if (typeof(SearchFilterDTO[]) != typeof(T))
        //        {
        //            return JsonConvert.DeserializeObject<T>(str);
        //        }
        //        var searchFilterArray = JsonConvert.DeserializeObject<SearchFilterDTO[]>(str);
        //        var temp = new T();
        //        if (searchFilterArray.Count() > 0)
        //        {
        //            var properties =
        //                typeof(T).GetProperties();
        //            foreach (var searchFilter in searchFilterArray)
        //            {
        //                var propertie = properties.FirstOrDefault(x => x.Name.Equals(searchFilter.property));
        //                if (propertie != null)
        //                {
        //                    propertie.SetValue(temp, Convert.ChangeType(searchFilter.value, propertie.PropertyType), null);
        //                }
        //            }
        //        }
        //        return temp;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public static T GetFormValue<T>(this FormCollection collection, PayloadKey<T> payloadKey)
        {
            // Convert.ChangeType just plain does not support nullable types.

            if (collection[payloadKey.ToString()] == null || collection[payloadKey.ToString()] == "")
            {
                if (typeof(T) == typeof(int))
                {
                    return (T)Convert.ChangeType(0, typeof(T));
                }
            }
            if (typeof(T) == typeof(bool))
            {
                try
                {
                    return (T)Convert.ChangeType(int.Parse(collection[payloadKey.ToString()]), typeof(T)); //collection[payloadKey.ToString()], typeof(T));
                }
                catch (Exception)
                {
                    return (T)Convert.ChangeType(false, typeof(T));
                }
            }
            if (typeof(T) == typeof(DateTime))
            {
                try
                {
                    return (T)Convert.ChangeType(collection[payloadKey.ToString()], typeof(T));
                }
                catch (Exception)
                {
                    return default(T); // 0001-01-01
                }
            }
            if (typeof(T) == typeof(DateTime?))
            {
                try
                {
                    var dateTime_str = collection[payloadKey.ToString()];
                
                    Type t_dateTime = Type.GetType("System.Nullable`1[[System.DateTime, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]");

                    if (String.IsNullOrEmpty(dateTime_str))
                        return default(T);
                    else
                        return (T)Convert.ChangeType(dateTime_str, t_dateTime.GetGenericArguments()[0]);
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            if (typeof(T) == typeof(double))
            {
                try
                {
                    return (T)Convert.ChangeType(collection[payloadKey.ToString()], typeof(T));
                }
                catch (Exception)
                {
                    return default(T); // 0001-01-01
                }
            }
           
            return (T)Convert.ChangeType(collection[payloadKey.ToString()], typeof(T));
        }

        public static T GetQueryValue<T>(this HttpRequestBase request, PayloadKey<T> payloadKey)
        {
            if (request[payloadKey.ToString()] == null || request[payloadKey.ToString()] == "")
            {
                if (typeof(T) == typeof(int))
                {
                    return (T)Convert.ChangeType(0, typeof(T));
                }
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType("", typeof(T));
                }
            }
            return (T)Convert.ChangeType(request[payloadKey.ToString()], typeof(T));
        }
    }
}