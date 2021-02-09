using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Administration.Role
{
    public class RoleAccessManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        public IEnumerable<sp_GetRoleAccessDetailResult> GetRoleAccessDetail(int user_id, int role_id)
        {
            int? sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetRoleAccessDetail(SessionManager.Current.obj_id, role_id, ref sql_result, ref sql_remark);

            return result;
        }

        public IEnumerable<sp_GetUserOwnedRole_listsResult> GetUserOwnedRole_lists(int access_user_id, int user_id)
        {
            int? sql_result = 0;
            var sql_remark = "";
            var result = db.sp_GetUserOwnedRole_lists(SessionManager.Current.obj_id, user_id, ref sql_result, ref sql_remark);

            return result;
        }

        public List<RoleObject> GetListOwnByUser(int access_user_id, int user_id)
        {
            var resultList = new List<RoleObject>();
            var dbList = GetUserOwnedRole_lists(access_user_id, user_id);

            foreach (var x in dbList)
            {
                resultList.Add(new RoleObject()
                {
                    role_id = x.role_id,
                    status = x.status,
                    name = x.role_name
                });
            }

            return resultList;
        }

        public IEnumerable<sp_GetUserSectionAccess_listResult> GetUserSectionAccess(int user_id, string module)
        {
            int? sql_result = 0;
            var sql_remark = "";
            var result = db.sp_GetUserSectionAccess_list(SessionManager.Current.obj_id, user_id, module, ref sql_result, ref sql_remark);

            return result;
        }
    }
}