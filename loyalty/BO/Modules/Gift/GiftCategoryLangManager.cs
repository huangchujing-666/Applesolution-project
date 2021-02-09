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

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftCategoryLangManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.giftCategory;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public GiftCategoryLangManager()
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

            int category_id,
            int lang_id,
            string name,
            string description,
            int status,
          
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_CreateGiftCategoryLang(
                              _accessObject.id,
                  _accessObject.type, 
                category_id,
                lang_id,
                name,
                description,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public bool Update(
            int user_id,

            int category_id,
            int lang_id,
            string name,
            string description,
            int status,
           
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_UpdateGiftCategoryLang(
                                 _accessObject.id,
                  _accessObject.type, 
                category_id,
                lang_id,
                name,
                description,
                status,

                ref get_sql_result, ref sql_remark);

            
            return (int.Parse(get_sql_result.Value.ToString()) == 1 ? true : false);
        }  

        public IEnumerable<sp_GetGiftCategoryLangDetailResult> GetGiftCategoryLangDetail(int user_id, int giftCategory_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            System.Diagnostics.Debug.WriteLine(user_id, "user_id");
            
            var result = db.sp_GetGiftCategoryLangDetail(user_id, giftCategory_id, ref get_sql_result, ref sql_remark);
            sql_result = get_sql_result == 1 ? true : false;

            return result;
        }
    }
}
