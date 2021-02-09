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

namespace Palmary.Loyalty.BO.Modules.Location
{
    public class LocationManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
            private AccessManager _accessManager;
        private string _module = CommonConstant.Module.location;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // For backend, using BO Session to access
        public LocationManager()
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

            string location_no,
            int type,
            string photo_file_name,
            string photo_file_extension,
            double latitude,
            double longitude,
            string phone,
            string fax,
            int address_district,
            int address_region,
            int display_order,
            int status,
           
            ref int location_id,
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            int? get_location_id = 0;

            var result = db.sp_CreateLocation(
                 _accessObject.id,
                _accessObject.type, 

                location_no,
                type,
                photo_file_name,
                photo_file_extension,
                latitude,
                longitude,
                phone,
                fax,
                address_district,
                address_region,
                display_order,
                status,
               
                ref get_location_id,
                ref get_sql_result, ref sql_remark);

            
            location_id = get_location_id.Value;
            return get_sql_result == 1 ? true : false;
        }

        public bool Update(
            int user_id,

            int location_id,  
            string location_no,
            int type,
            string photo_file_name,
            string photo_file_extension,
            double latitude,
            double longitude,
            string phone,
            string fax,
            int address_district,
            int address_region,
            int display_order,
            int status,
          
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_UpdateLocation(
                 _accessObject.id,
                _accessObject.type, 

                location_id,
                location_no,
                type,
                photo_file_name,
                photo_file_extension,
                latitude,
                longitude,
                phone,
                fax,
                address_district,
                address_region,
                display_order,
                status,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public IEnumerable<sp_GetLocationListsResult> GetLocationLists(int user_id, long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetLocationListsResult> result = null;

            try
            {
                result = db.sp_GetLocationLists(user_id, ref get_sql_result, ref sql_remark); //rowIndexStart, rowLimit, searchParams, get_sql_result, get_sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "get gift error");
            }
            return result;
        }

        public sp_GetLocationDetailResult GetLocationDetail(int user_id, int location_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetLocationDetail(user_id, location_id, ref get_sql_result, ref sql_remark);

            return result.FirstOrDefault() ?? new sp_GetLocationDetailResult();
        }
    }
}