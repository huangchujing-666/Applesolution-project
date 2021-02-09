using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.BO.Modules.Gift
{
    public class GiftLocationManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private string _module = CommonConstant.Module.giftLocation;
        private AccessObject _accessObject;
        private static LogManager _logManager;

        public GiftLocationManager()
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
            int location_id,
            int status,
           
            ref string sql_remark
        )
        {
            bool create_result = false;
            AccessManager accessManager = new AccessManager();
            var privilege = accessManager.AccessModule("location");
           
            if (privilege.insert_status == 1)
            {
                int? get_sql_result = 0;
                
                var result = db.sp_CreateGiftLocation(
                    _accessObject.id,
                    _accessObject.type, 
                    gift_id,
                    location_id,
                    status,

                    ref get_sql_result, ref sql_remark);

                create_result = get_sql_result == 1 ? true : false;
            }
            
            return create_result;
        }

        public bool Delete(
            int user_id,
            int gift_id,
            
            ref string sql_remark
        )
        {
            int? get_sql_result = 0;
            
            var result = db.sp_DeleteGiftLocation(
                user_id,

                gift_id,

                ref get_sql_result, ref sql_remark);

            return get_sql_result == 1 ? true : false;
        }

        public IEnumerable<sp_GetGiftLocationListsResult> GetGiftLocationLists(int user_id, int gift_id, long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetGiftLocationListsResult> result = null;

            try
            {
                result = db.sp_GetGiftLocationLists(user_id, gift_id, ref get_sql_result, ref sql_remark); //rowIndexStart, rowLimit, searchParams, get_sql_result, get_sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "get gift error");
            }
            return result;
        }

        //public List<GiftLocationObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        //{
        //    // LINQ to SQL
        //    systemCode = CommonConstant.SystemCode.undefine;
        //    var resultList = new List<GiftLocationObject>();

        //    if (_privilege.read_status == 1)
        //    {
        //        var query = (from l in db.locations
        //                     join ll in db.location_langs on l.location_id equals ll.location_id
        //                     where (
        //                        l.record_status != (int)CommonConstant.RecordStatus.deleted
        //                        && ll.lang_id == SessionManager.Current.user_language_id)
        //                     select new GiftLocationObject
        //                     {
        //                         location_id = l.location_id,
        //                         location_no = l.location_no,
        //                         type = l.type,
        //                         photo_file_name = l.photo_file_name,
        //                         photo_file_extension = l.photo_file_extension,
        //                         latitude = l.latitude,
        //                         longitude = l.longitude,
        //                         phone = l.phone,
        //                         fax = l.fax,
        //                         address_district = l.address_district,
        //                         address_region = l.address_region,
        //                         display_order = l.display_order,
        //                         status = l.status,
        //                         crt_date = l.crt_date,
        //                         crt_by_type = l.crt_by_type,
        //                         crt_by = l.crt_by,
        //                         upd_date = l.upd_date,
        //                         upd_by_type = l.upd_by_type,
        //                         upd_by = l.upd_by,
        //                         record_status = l.record_status
        //                     });

        //        // dynamic search
        //        foreach (var f in searchParmList)
        //        {
        //            if (!String.IsNullOrEmpty(f.value))
        //            {
        //                if (f.property == "name")
        //                {
        //                    query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
        //                }
        //                else if (f.property == "email")
        //                {
        //                    query = query.Where(x => x.email.Contains(f.value));
        //                }
        //                else if (f.property == "login_id")
        //                {
        //                    query = query.Where(x => x.login_id.Contains(f.value));
        //                }
        //                else if (f.property == "status_name")
        //                {
        //                    if (!String.IsNullOrEmpty(f.value))
        //                        query = query.Where(x => x.status == int.Parse(f.value));
        //                }
        //            }
        //        }

        //        // dynamic sort
        //        var orderByColumn = "";
        //        if (sortColumn == "email" || sortColumn == "name"
        //            || sortColumn == "status_name" || sortColumn == "login_id")
        //            orderByColumn = sortColumn;
        //        else
        //        { //default
        //            sortOrder = CommonConstant.SortOrder.asc;
        //            orderByColumn = "login_id";
        //        }

        //        // row total
        //        totalRow = query.Count();

        //        if (sortOrder == CommonConstant.SortOrder.desc)
        //            resultList = query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
        //        else
        //            resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();

        //        systemCode = CommonConstant.SystemCode.normal;
        //    }
        //    else
        //    {
        //        systemCode = CommonConstant.SystemCode.no_permission;
        //    }

        //    return resultList;
        //}
    }
}