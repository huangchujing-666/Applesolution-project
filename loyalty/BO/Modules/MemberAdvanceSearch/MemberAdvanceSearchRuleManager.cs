using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.MemberAdvanceSearch;

using Palmary.Loyalty.BO.Modules.Utility;


namespace Palmary.Loyalty.BO.Modules.MemberAdvanceSearch
{
    public class MemberAdvanceSearchRuleManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberAdvanceSearch;

        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

         // For backend, using BO Session to access
        public MemberAdvanceSearchRuleManager()
        {
            // access object from session
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public MemberAdvanceSearchRuleManager(AccessObject accessObject)
        {
            // access object from passing in
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public CommonConstant.SystemCode Create(
           MemberAdvanceSearchRuleObject theObj,

           ref long new_obj_id)
        {
            // LINQ to Store Procedures
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var dataValid = true;
               
                if (dataValid)
                {
                    int? sql_result = 0;
                    
                    long? get_new_obj_id = 0;

                    var result = db.sp_CreateMemberAdvanceSearchRule(
                        _accessObject.id,
 _accessObject.type,

                        theObj.search_id,
                        theObj.group_id,
                        theObj.row_id,
                        theObj.target_field,
                        theObj.target_condition,
                        theObj.target_value,
                    
                        ref get_new_obj_id,
                        ref sql_result);

                    systemCode = (CommonConstant.SystemCode)sql_result.Value;

                    if (systemCode == CommonConstant.SystemCode.normal)
                    {
                        new_obj_id = get_new_obj_id.Value;
                       
                        systemCode = CommonConstant.SystemCode.normal;
                    }
                    else
                    {
                        systemCode = CommonConstant.SystemCode.record_invalid;
                    }
                }
                else
                {
                    systemCode = CommonConstant.SystemCode.data_invalid;
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        public List<MemberAdvanceSearchRuleObject> GetListBySearch(int search_id)
        {
            // LINQ to SQL
            
            var systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<MemberAdvanceSearchRuleObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from s in db.member_advance_search_rules
                             join li_f in db.listing_items on new { v1 = (int)CommonConstant.ListingType.MemberAdvanceSearchField, v2 = s.target_field } equals new { v1 = li_f.list_id, v2 = li_f.value }
                             join li_c in db.listing_items on new { v1 = (int)CommonConstant.ListingType.CompareCondition, v2 = s.target_condition } equals new { v1 = li_c.list_id, v2 = li_c.value }
                             
                             where (
                                s.record_status != (int)CommonConstant.RecordStatus.deleted
                                && s.search_id == search_id
                                )
                             select new MemberAdvanceSearchRuleObject
                             {
                                 rec_id = s.rec_id,
                                 search_id = s.search_id,
                                 group_id = s.group_id,
                                 row_id = s.row_id,
                                 target_field = s.target_field,
                                 target_condition = s.target_condition,
                                 target_value = s.target_value,
                                 crt_date = s.crt_date,
                                 crt_by_type = s.crt_by_type,
                                 crt_by = s.crt_by,
                                 upd_date = s.upd_date,
                                 upd_by_type = s.upd_by_type,
                                 upd_by = s.upd_by,
                                 record_status = s.record_status,

                                 // additional
                                 target_field_name = li_f.name,
                                 target_condition_name = li_c.name
                             });

                // dynamic sort
                var orderByColumn = "group_id";
                var sortOrder = CommonConstant.SortOrder.asc;

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderBy(orderByColumn, true).ToList();
                else
                    resultList = query.OrderBy(orderByColumn, false).ToList();

                systemCode = CommonConstant.SystemCode.normal;

                foreach (var r in resultList)
                {
                    if (r.target_field == (int)CommonConstant.MemberAdvanceSearchField.availablePoint
                        || r.target_field == (int)CommonConstant.MemberAdvanceSearchField.email)
                        r.target_value_name = r.target_value;
                    else if (r.target_field == (int)CommonConstant.MemberAdvanceSearchField.birthdayMonth)
                        r.target_value_name = GetBirthdayListName(r.target_value);
                    else if (r.target_field == (int)CommonConstant.MemberAdvanceSearchField.gender)
                        r.target_value_name = int.Parse(r.target_value).ToListingItemName("Gender");
                    else
                        r.target_value_name = "vv";
                }

            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public string GetBirthdayListName(string months)
        {
            var nameList = new List<string>();

            var inputList_int = new List<int>();
            
            var inputList_str = months.Split(',');

            foreach(var x in inputList_str)
            {
                inputList_int.Add(int.Parse(x));
            }

            // LINQ to SQL
            var query = (from li in db.listing_items
                         where li.list_id == (int)CommonConstant.ListingType.Month
                         && inputList_int.Contains(li.value)
                         select li);

            var resultList = query.ToList();
            foreach(var x in resultList)
            {
                nameList.Add(x.name);
            }

            return String.Join(", ", nameList);
        }

    }

}
