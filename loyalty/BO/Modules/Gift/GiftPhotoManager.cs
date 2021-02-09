using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftPhotoManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
                
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.giftPhoto;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public GiftPhotoManager()
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

            int gift_id,
            string file_name,
            string file_extension,
            string name,
            string caption,
            int display_order,
            int status,

            ref string sql_remark
        )
        {
            int? get_sql_result = 0;

            var result = db.sp_CreateGiftPhoto(_accessObject.id,
                _accessObject.type, 

                gift_id,
                file_name,
                file_extension,
                name,
                caption,
                display_order,
                status,
                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }
        
        public IEnumerable<sp_GetGiftPhotoListByResult> GetGiftPhotoListBy(int user_id, int gift_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetGiftPhotoListBy(user_id, gift_id, ref get_sql_result, ref sql_remark);

            return result;
        }

        // UPDATE
        public bool Update(
           
            int gift_photo_id,
            int gift_id,
            string file_name,
            string file_extension,
            string name,
            string caption,
            int display_order,
            int status,
            
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_UpdateGiftPhoto(
_accessObject.id, _accessObject.type, 
                gift_photo_id,
                gift_id,
                file_name,
                file_extension,
                name,
                caption,
                display_order,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }  
    
    }
}
