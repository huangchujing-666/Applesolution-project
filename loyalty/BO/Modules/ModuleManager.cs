using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.Section;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules
{
    public class ModuleManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.user;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public ModuleManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }


        public bool SoftDelete(string module, int rec_id, int access_user_id, ref string delete_remark)
        {
            module = module.ToLower();

            AccessManager accessManager = new AccessManager();
            var sql_remark = "";
            var privilege = accessManager.AccessModule(module);
            var delete_result = false;

            if (privilege.delete_status == 1)
            {
                int? get_sql_result = 0;

                db.sp_SoftDeleteByModule(_accessObject.id, _accessObject.type , module, rec_id, ref get_sql_result, ref sql_remark);
                delete_result = (int)get_sql_result.Value == 1 ? true : false;
                delete_remark = sql_remark;
            }
            else
            {
                delete_remark = "No Access";
            }

            return delete_result;
        }

        public static int GetTableNextIdentity(string tableName)
        {
            int? get_id = 0;
            var result = db.sp_GetNextID(tableName, ref get_id);
            return get_id.Value;
        }

        public static int GetTableCurrentIdentity(string tableName)
        {
            int? get_id = 0;
            var result = db.sp_GetTableCurrentIdentity(tableName, ref get_id);
            return get_id.Value;
        }

        // set table identity id to 
        public static void TableRESEED(string tableName, int identity)
        {
            var result = db.sp_TableRESEED(tableName, identity);
        }
    }
}
