using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.Common.Languages;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftLangManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.gift;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public GiftLangManager()
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
            int lang_id,
            string name,
            string description,
            int status,
           
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_CreateGiftLang(
                _accessObject.id,
                _accessObject.type,
                gift_id,
                lang_id,
                name,
                description,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public bool Create(
            List<GiftLangObject> gift_lang_list,
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var sql_result = false;
            
            foreach (var theLang in gift_lang_list)
            {
                var exeSQL = db.sp_CreateGiftLang(
               _accessObject.id, 
               _accessObject.type, 
                theLang.gift_id,
                theLang.lang_id,
                theLang.name,
                theLang.description,
                theLang.status,

                ref get_sql_result, ref sql_remark);

                sql_result = get_sql_result == 1 ? true : false;

                
            }

            //if (sql_result)
            //{ // Update object table
            //    sql_result = UpdateSystemObject(gift_lang_list[0].gift_id, ref sql_remark);
            //}

            return sql_result;
        }

        public IEnumerable<sp_GetGiftLangDetailByResult> GetGiftLangDetailBy(int user_id, int gift_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetGiftLangDetailBy(SessionManager.Current.obj_id, gift_id, ref get_sql_result, ref sql_remark);

            return result;
        }

        public bool Update(
            int user_id,

            int gift_id,
            int lang_id,
            string name,
            string description,
            int status,
          
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            

            var sql_run = db.sp_UpdateGiftLang(
                _accessObject.id,
                _accessObject.type, 
                gift_id,
                lang_id,
                name,
                description,
                status,

                ref get_sql_result, ref sql_remark);

            var update_result = get_sql_result == 1 ? true : false;

            //if (update_result)
            //{ // Update object table
            //    update_result = UpdateSystemObject(gift_id, ref sql_remark);
            //}

            return update_result;
        }  


        //public bool UpdateSystemObject(int gift_id, ref string sql_remark)
        //{
        //    var sql_result = false;
        //    sql_remark = "";

        //    var gift_lang = GetGiftLangDetailBy(SessionManager.Current.obj_id, gift_id, ref sql_result);

        //    var object_name_list = new List<string>();
        //    var object_power_search_list = new List<string>();

        //    var object_name = "";
        //    foreach (var theLang in gift_lang)
        //    {
        //        object_name_list.Add(theLang.name);

        //        object_power_search_list.Add(theLang.name);
        //        object_power_search_list.Add(theLang.description);

        //        if (theLang.lang_id == (int)CommonConstant.LangCode.en)
        //            object_name = theLang.name;
        //    }

        //    var power_search = String.Join(" ", object_power_search_list.ToArray());

        //    SystemObjectManager systemObjectManager = new SystemObjectManager();
        //    var theObject = systemObjectManager.GetSystemObject_detail(SessionManager.Current.obj_id, gift_id, ref sql_result);

        //    sql_result = systemObjectManager.Update(
        //            SessionManager.Current.obj_id, //access_user_id
        //            gift_id,        //object_id
        //            object_name,
        //            theObject.status,         //status
        //            power_search,   //power_search
        //            ref sql_remark
        //        );

        //    return sql_result;
        //}
    }
}