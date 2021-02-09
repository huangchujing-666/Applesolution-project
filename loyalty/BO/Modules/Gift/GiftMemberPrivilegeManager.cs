using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftMemberPrivilegeManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.giftMemberPrivileg;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public GiftMemberPrivilegeManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public IEnumerable<sp_GetGiftMemberPrivilege_ownedListResult> GetGiftMemberPrivilege_ownedList(int gift_id)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetGiftMemberPrivilege_ownedListResult> result = null;

            try
            {
                result = db.sp_GetGiftMemberPrivilege_ownedList(SessionManager.Current.obj_id, gift_id, ref get_sql_result, ref sql_remark); //rowIndexStart, rowLimit, searchParams, get_sql_result, get_sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "Error: sp_GetGiftMemberPrivilege_ownedList");
            }

            return result;
        }

        public bool Create(
           
            int gift_id,
            int member_level_id,
            int allow_redeem,
            int status,
            
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            

            var result = db.sp_CreateGiftMemberPrivilege(
               _accessObject.id, 
               _accessObject.type, 
                gift_id,
                member_level_id,
                allow_redeem,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public bool DeleteOwnedList(int gift_id, ref string sql_remark)
        {
            int? get_sql_result = 0;
            
            var result = db.sp_DelGiftMemPrivilege_ownedList(
                SessionManager.Current.obj_id,

                gift_id,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }
    }
}
