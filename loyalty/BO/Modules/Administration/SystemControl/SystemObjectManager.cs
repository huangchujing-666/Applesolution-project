using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.BO.Modules.Administration.SystemControl
{
    public class SystemObjectManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        //public bool Create(
        //    int access_user_id,

        //    int type,
        //    int target_id,
        //    string name,
        //    int status,
        //    string power_search,
           
        //    ref int new_object_id,
        //    ref string sql_remark
        //)
        //{
        //    int? get_new_object_id = 0;
        //    int? get_sql_result = 0;
            
        //    var result = db.sp_CreateSystemObject(
        //        SessionManager.Current.obj_id,

        //        type,
        //        target_id,
        //        name,
        //        status,
        //        power_search,

        //        ref get_new_object_id,
        //        ref get_sql_result, ref sql_remark);

            
        //    var create_result = get_sql_result == 1 ? true : false;

        //    if (create_result)
        //        new_object_id = get_new_object_id ?? 0;

        //    return create_result;
        //}

        //public bool Update(
        //    int access_user_id,

        //    int object_id,
        //    string name,
        //    int status,
        //    string power_search,
           
        //    ref string sql_remark
        //)
        //{
        //    int? get_sql_result = 0;
            
        //    var result = db.sp_UpdateSystemObject(
        //        SessionManager.Current.obj_id,

        //        object_id,
        //        name,
        //        status,
        //        power_search,
                
        //        ref get_sql_result,
        //        ref sql_remark);

        //    return get_sql_result == 1 ? true : false;
        //}

        //public bool Delete(int access_user_id, int object_id)
        //{
        //    int? get_sql_result = 0;
        //    var sql_remark = "";

        //    var result = db.sp_DeleteSystemObject(
        //        SessionManager.Current.obj_id,
        //        object_id,

        //        ref get_sql_result,
        //        ref sql_remark);

        //    return get_sql_result == 1 ? true : false;
        //}

        //public bool SoftDelete(int access_user_id, int object_id)
        //{
        //    int? get_sql_result = 0;
        //    var sql_remark = "";

        //    var result = db.sp_SoftDeleteSystemObject(
        //        access_user_id,
        //        object_id,

        //        ref get_sql_result,
        //        ref sql_remark);

        //    return get_sql_result == 1 ? true : false;
        //}

        public sp_GetSystemObject_detailResult GetSystemObject_detail(int access_object_id, int object_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetSystemObject_detail(access_object_id, object_id, ref get_sql_result, ref sql_remark);

            return result.FirstOrDefault() ?? new sp_GetSystemObject_detailResult();
        }
    }
}
