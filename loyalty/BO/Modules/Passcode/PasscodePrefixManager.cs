using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Passcode;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Passcode
{
    public class PasscodePrefixManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.passcodePrefix;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;
      
         // For backend, using BO Session to access
        public PasscodePrefixManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public PasscodePrefixManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }


        public List<PasscodePrefixObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<PasscodePrefixObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from p in db.passcode_prefixes
                             join li_s in db.listing_items on new { list_id = (int)CommonConstant.ListingType.Status, value = p.status } equals new { list_id = li_s.list_id, value = li_s.value }
                             join p_fo in db.passcode_formats on p.format_id equals p_fo.format_id
                             join pro_l in db.product_langs on p.product_id equals pro_l.product_id
                             where (
                                p.record_status != (int)CommonConstant.RecordStatus.deleted
                                && pro_l.lang_id == _accessObject.languageID
                                )
                             select new PasscodePrefixObject
                             {
                                 prefix_id = p.prefix_id,
                                 product_id = p.product_id,
                                 format_id = p.format_id,
                                 prefix_value = p.prefix_value,
                                 current_generated = p.current_generated,
                                 usage_precent = p.usage_precent,
                                 status = p.status,
                                 crt_date = p.crt_date,
                                 upd_date = p.upd_date,
                                 crt_by = p.crt_by,
                                 upd_by = p.upd_by,
                                 record_status = p.record_status,

                                 // additional
                                 passcode_format = p_fo.passcode_format1,
                                 maximum_generate = p_fo.maximum_generate,
                                 product_name = pro_l.name
                             });

                // dynamic search
                //foreach (var f in searchParmList)
                //{
                //    if (!String.IsNullOrEmpty(f.value))
                //    {
                //        if (f.property == "name")
                //        {
                //            query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                //        }
                //    }
                //}

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "prefix_id" || sortColumn == "prefix_value" || sortColumn == "passcode_format" || sortColumn == "product_name"
                    || sortColumn == "current_generated" || sortColumn == "maximum_generate" || sortColumn == "usage_precent")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "prefix_value";
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public sp_GetPasscodePrefixDetailByResult GetPasscodePrefixDetailBy(int user_id, int prefix_id, ref bool sql_result)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            var result = db.sp_GetPasscodePrefixDetailBy(SessionManager.Current.obj_id, prefix_id, ref get_sql_result, ref sql_remark);

            return result.FirstOrDefault() ?? new sp_GetPasscodePrefixDetailByResult();
        }

        public bool Create(
            int user_id,

            int product_id,
            long format_id,
            string prefix_value,
            int status,

            ref int prefix_id,
            ref string sql_remark
        )
        {
            int? get_prefix_id = 0;
            int? get_sql_result = 0;
            
            long current_generated = 0;
            double usage_precent = 0;

            var result = db.sp_CreatePasscodePrefix(
                SessionManager.Current.obj_id,
        
	            product_id,
	            format_id,
	            prefix_value,
	            current_generated,
	            usage_precent,
	            status,
	       
                ref get_prefix_id,
                ref get_sql_result, ref sql_remark);

            prefix_id = get_prefix_id.Value;
            return get_sql_result.Value == 1 ? true : false;
        }
    }
}