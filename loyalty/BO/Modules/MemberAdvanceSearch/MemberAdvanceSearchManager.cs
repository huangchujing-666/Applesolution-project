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
    public class MemberAdvanceSearchManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.memberAdvanceSearch;

        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

         // For backend, using BO Session to access
        public MemberAdvanceSearchManager()
        {
            // access object from session
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public MemberAdvanceSearchManager(AccessObject accessObject)
        {
            // access object from passing in
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        private CommonConstant.SystemCode Create(
           MemberAdvanceSearchObject theObj,

           ref int new_obj_id)
        {
            // LINQ to Store Procedures
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var dataValid = true;
               
                if (dataValid)
                {
                    int? sql_result = 0;
                    
                    int? get_new_obj_id = 0;

                    var result = db.sp_CreateMemberAdvanceSearch(
                        _accessObject.id,
                         _accessObject.type,
                        theObj.name,
                        theObj.status,

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

        public CommonConstant.SystemCode CreateWithRule(
          MemberAdvanceSearchObject theObj,

          ref int new_obj_id)
        {
             // LINQ to Store Procedures
            var systemCode = CommonConstant.SystemCode.undefine;

            systemCode = Create(theObj, ref new_obj_id);

            if (systemCode == CommonConstant.SystemCode.normal)
            {
                var memberAdvanceSearchRuleManager = new MemberAdvanceSearchRuleManager();

                foreach(var theRule in theObj.ruleList)
                {
                    theRule.search_id = new_obj_id;
                    long new_rule_id = 0;
                    systemCode = memberAdvanceSearchRuleManager.Create(theRule, ref new_rule_id);
                }
            }

            return systemCode;
        }

        public List<MemberAdvanceSearchObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<MemberAdvanceSearchObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from s in db.member_advance_searches
                             
                             where (
                                s.record_status != (int)CommonConstant.RecordStatus.deleted
                                )
                             select new MemberAdvanceSearchObject
                             {
                                 search_id = s.search_id,
                                 name = s.name,
                                 status = s.status,
                                 crt_date = s.crt_date,
                                 crt_by_type = s.crt_by_type,
                                 crt_by = s.crt_by,
                                 upd_date = s.upd_date,
                                 upd_by_type = s.upd_by_type,
                                 upd_by = s.upd_by,
                                 record_status = s.record_status
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (!String.IsNullOrEmpty(f.value))
                    {
                        if (f.property == "name")
                        {
                            query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                        }
                        
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "name")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "name";
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

        public MemberAdvanceSearchObject GetDetail(int search_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new MemberAdvanceSearchObject();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from s in db.member_advance_searches

                             where (
                                s.record_status != (int)CommonConstant.RecordStatus.deleted
                                && s.search_id == search_id
                            )
                             select new MemberAdvanceSearchObject
                             {
                                 search_id = s.search_id,
                                 name = s.name,
                                 status = s.status,
                                 crt_date = s.crt_date,
                                 crt_by_type = s.crt_by_type,
                                 crt_by = s.crt_by,
                                 upd_date = s.upd_date,
                                 upd_by_type = s.upd_by_type,
                                 upd_by = s.upd_by,
                                 record_status = s.record_status
                             });

                if (query.Count() > 0)
                {
                    resultObj = query.FirstOrDefault();
                    systemCode = CommonConstant.SystemCode.normal;

                    // Get rule list
                    var memberAdvanceSearchRuleManager = new MemberAdvanceSearchRuleManager();
                    var ruleList = memberAdvanceSearchRuleManager.GetListBySearch(search_id);

                    resultObj.ruleList = ruleList;
                }
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }
    }

}
