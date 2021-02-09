using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;
using Palmary.Loyalty.Common.Languages;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;

namespace Palmary.Loyalty.BO.Modules.Location
{
    public class LocationLangManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();

            private AccessManager _accessManager;
        private string _module = CommonConstant.Module.location;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public LocationLangManager()
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

            int location_id,
            int lang_id,
            string name,
            string description,
            string operation_info,
            string address_unit,
            string address_building,
            string address_street,
            int status,
          
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_CreateLocationLang(
               _accessObject.id, 
               _accessObject.type, 

                location_id,
                lang_id,
                name,
                description,
                operation_info,
                address_unit,
                address_building,
                address_street,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public bool Update(
            int user_id,

            int location_id,
            int lang_id,
            string name,
            string description,
            string operation_info,
            string address_unit,
            string address_building,
            string address_street,
            int status,

            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            

            var result = db.sp_UpdateLocationLang(
                 _accessObject.id,
                _accessObject.type, 

                location_id,
                lang_id,
                name,
                description,
                operation_info,
                address_unit,
                address_building,
                address_street,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public IEnumerable<sp_GetLocationLangDetailResult> GetLocationLangDetail(int user_id, int location_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetLocationLangDetail(SessionManager.Current.obj_id, location_id, ref get_sql_result, ref sql_remark);

            return result;
        }
    }
}
