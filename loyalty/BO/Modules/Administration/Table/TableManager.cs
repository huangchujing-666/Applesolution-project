using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.Administration.Table
{
    public class TableManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        public IEnumerable<sp_GetExtJSSearchColumnsResult> GetExtJsSearchColumns(int user_id, string module, ref string message)
        {
            int? status = 0;
            var remark = "";
            var ip = "127.0.0.1"; // GetIPAddress(HttpContext.Current.Request);
            var result = db.sp_GetExtJSSearchColumns(SessionManager.Current.obj_id, ip, module, ref status, ref remark).ToList();
            if (status != 1)
            {
                message = remark;
            }
            return result;
        }

        public IEnumerable<sp_GetExtJSGridHeadersResult> GetExtJsGridHeaders(
                                                               string module,
                                                               ref int statusOut,
                                                               ref string titleOut,
                                                               ref string linkOut,
                                                               ref int pageSizeOut,
                                                               ref bool searchTextHiddenOut,
                                                               ref bool addHiddenOut,
                                                               ref bool deleteHiddenOut,
                                                               ref bool exportHiddenOut,
                                                               ref bool checkboxHiddenOut)
        {
            var remark = "";
            var title = "";

            int? status = 0;
            int? pageSize = 0;
            bool? searchTextHidden = false;
            bool? addHidden = false;
            bool? deleteHidden = false;
            bool? exportHidden = false;
            bool? checkboxHidden = false;

            var result = db.sp_GetExtJSGridHeaders(SessionManager.Current.obj_id, module, ref status, ref remark, ref title, ref linkOut, ref pageSize,
                                                                 ref searchTextHidden,
                                                                 ref addHidden, ref deleteHidden, ref exportHidden, ref checkboxHidden).ToList();

            titleOut = title;
            pageSizeOut = pageSize ?? 0;
            searchTextHiddenOut = searchTextHidden ?? false;
            addHiddenOut = addHidden ?? false;
            deleteHiddenOut = deleteHidden ?? false;
            exportHiddenOut = exportHidden ?? false;
            checkboxHiddenOut = checkboxHidden ?? false;
            statusOut = status ?? 0;

            return result;
        }

        public IEnumerable<sp_GetListingItemsByCodeResult> GetListingItemsByCode(int user_id, string code)
        {
            int? get_sql_result = 0;
            var sql_remark = "";
            var result = db.sp_GetListingItemsByCode(SessionManager.Current.obj_id, code, ref get_sql_result, ref sql_remark);
            return result;
        }

        // new test
        public IEnumerable<sp_GetListingItemsByCodeResult> GetListingItemsByCodeDTO(int user_id, string code)
        {
            int? get_sql_result = 0;
            var sql_remark = "";
            var result = db.sp_GetListingItemsByCode(SessionManager.Current.obj_id, code, ref get_sql_result, ref sql_remark);
            return result;
        }

        public string GetListingItemName(string code, int itemValue)
        {
            int? get_sql_result = 0;
            var sql_remark = "";
            var result = db.sp_GetListingItemName(SessionManager.Current.obj_id, code, itemValue, ref get_sql_result, ref sql_remark).FirstOrDefault();

            var name = "";
            if (result != null)
                name = result.name;

            return name;
        }
    }
}
