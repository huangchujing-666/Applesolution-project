using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Administration.User
{
    public class UserRoleLinkManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

             
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.Role;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public UserRoleLinkManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public bool Create(
            int user_id,
            int role_id,
            int status,

            ref string sql_remark
        )
        {
            var create_result = false;

            int? get_sql_result = 0;

            var result = db.sp_CreateUserOwnedRole(
               _accessObject.id, 
               _accessObject.type, 
                user_id,
                role_id,
                status,
                ref get_sql_result, ref sql_remark);

            create_result = get_sql_result == 1 ? true : false;

            return create_result;
        }

        public bool Delete(
            int user_id,

            ref string sql_remark
        )
        {
            int? get_sql_result = 0;

            var result = db.sp_DeleteUserOwnedRole(
               _accessObject.id, 

                user_id,

                ref get_sql_result,
                ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }
    }
}