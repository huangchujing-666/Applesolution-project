using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Administration.Role
{
    public class RoleManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private string _module = CommonConstant.Module.Role; 
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // init and load privilege
        public RoleManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // Create object
        // With permission check and log
        // LINQ to Store Procedures
        public CommonConstant.SystemCode Create(RoleObject obj)
        {
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                // check exist name
                var dataValid = true;
                var existObj = GetDetail(obj.name, true, ref systemCode);

                if (existObj.role_id > 0)
                {
                    systemCode = CommonConstant.SystemCode.err_roleName_exist;
                    dataValid = false;
                }

                if (dataValid)
                {
                    int? new_obj_id = 0;

                    var result = db.sp_CreateRole(
                       _accessObject.id, 
                       _accessObject.type, 
                        obj.name,
                        obj.status,

                        ref new_obj_id,
                        ref sql_result);

                    systemCode = (CommonConstant.SystemCode)sql_result.Value;

                    if (systemCode == CommonConstant.SystemCode.normal)
                    {
                        obj.role_id = new_obj_id.Value;

                        // Take Log with detail
                        var oldObj = new RoleObject() { role_id = obj.role_id };
                        LogAndCompare(CommonConstant.ActionType.create, oldObj, obj);
                    }
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        // Get List with dyncmic search, paging, ordering
        // With permission check and log 
        // LINQ to SQL directly
        public List<RoleObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<RoleObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from r in db.roles

                             //Status
                             join li_s in db.listing_items on r.status equals li_s.value
                             join l_s in db.listings on li_s.list_id equals l_s.list_id

                             where (
                                r.record_status != (int)CommonConstant.RecordStatus.deleted

                                && l_s.code == "Status"
                                )
                             select new RoleObject
                             {
                                 role_id = r.role_id,
                                 name = r.name,
                                 status = r.status,
                                 crt_date = r.crt_date,
                                 crt_by_type = r.crt_by_type,
                                 crt_by = r.crt_by,
                                 upd_date = r.upd_date,
                                 upd_by_type = r.upd_by_type,
                                 upd_by = r.upd_by,
                                 record_status = r.record_status,

                                 // additional
                                 status_name = li_s.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "name")
                    {
                        query = query.Where(x => x.name.Contains(f.value));
                    }
                    else if (f.property == "status_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.status == int.Parse(f.value));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "name" || sortColumn == "status_name")
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

        // Get detail by id or no
        // With permission check and log
        public RoleObject GetDetail(int role_id, ref CommonConstant.SystemCode systemCode)
        {
            return GetDetail(role_id, false, ref systemCode);
        }

        // Get detail by id or no
        // Allow root access to override privilege
        // With permission check and log
        // LINQ to SQL directly
        public RoleObject GetDetail(int role_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new RoleObject();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from r in db.roles

                             //Status
                             join li_s in db.listing_items on r.status equals li_s.value
                             join l_s in db.listings on li_s.list_id equals l_s.list_id

                             where (
                                r.record_status != (int)CommonConstant.RecordStatus.deleted

                                && l_s.code == "Status"
                                && r.role_id == role_id
                                )
                             select new RoleObject
                             {
                                 role_id = r.role_id,
                                 name = r.name,
                                 status = r.status,
                                 crt_date = r.crt_date,
                                 crt_by_type = r.crt_by_type,
                                 crt_by = r.crt_by,
                                 upd_date = r.upd_date,
                                 upd_by_type = r.upd_by_type,
                                 upd_by = r.upd_by,
                                 record_status = r.record_status,

                                 // additional
                                 status_name = li_s.name
                             });

                if (query.Count() > 0)
                {
                    resultObj = query.FirstOrDefault();
                    systemCode = CommonConstant.SystemCode.normal;
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

        // Get Detail by name
        public RoleObject GetDetail(string name, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new RoleObject();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from r in db.roles

                             //Status
                             join li_s in db.listing_items on r.status equals li_s.value
                             join l_s in db.listings on li_s.list_id equals l_s.list_id

                             where (
                                r.record_status != (int)CommonConstant.RecordStatus.deleted

                                && l_s.code == "Status"
                                && r.name.ToLower() == name.ToLower()
                                )
                             select new RoleObject
                             {
                                 role_id = r.role_id,
                                 name = r.name,
                                 status = r.status,
                                 crt_date = r.crt_date,
                                 crt_by_type = r.crt_by_type,
                                 crt_by = r.crt_by,
                                 upd_date = r.upd_date,
                                 upd_by_type = r.upd_by_type,
                                 upd_by = r.upd_by,
                                 record_status = r.record_status,

                                 // additional
                                 status_name = li_s.name
                             });

                if (query.Count() > 0)
                {
                    resultObj = query.FirstOrDefault();
                    systemCode = CommonConstant.SystemCode.normal;
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

        // Update object
        // With permission check and log
        // LINQ to Store Procedures
        public CommonConstant.SystemCode Update(RoleObject obj)
        {
            return Update(obj, false);
        }

        // Update object
        // Allow root access to override privilege
        // With permission check and log
        // LINQ to Store Procedures
        public CommonConstant.SystemCode Update(RoleObject obj, bool rootAccess)
        {
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1 || rootAccess)
            {
                // cache original data
                var oldObj = GetDetail(obj.role_id, true, ref systemCode);

                // name changes, need to check any other exist name
                var dataValid = true;
                if (obj.name.ToLower() != oldObj.name.ToLower())
                {
                    var existObj = GetDetail(obj.name, true, ref systemCode);

                    if (existObj.role_id > 0)
                    {
                        systemCode = CommonConstant.SystemCode.err_roleName_exist;
                        dataValid = false;
                    }
                }

                if (dataValid)
                {
                    var result = db.sp_UpdateRole(
                        _accessObject.id, 
                        _accessObject.type, 

                        obj.role_id,
                        obj.name,
                        obj.status,

                        ref sql_result);

                    systemCode = (CommonConstant.SystemCode)sql_result.Value;

                    // Take Log with detail
                    LogAndCompare(CommonConstant.ActionType.update, oldObj, obj);
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        // log with change detail
        public CommonConstant.SystemCode LogAndCompare(int action, RoleObject oldObj, RoleObject newObj)
        {
            var result = CommonConstant.SystemCode.undefine;

            if (oldObj.role_id != newObj.role_id || oldObj.role_id == 0 || newObj.role_id == 0)
                result = CommonConstant.SystemCode.record_invalid;
            else
            { // valid objects
                var logDetailList = new List<LogDetailObject>();

                if (oldObj.name != newObj.name)
                {
                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.name),
                        old_value = oldObj.name,
                        new_value = newObj.name
                    });
                }

                if (oldObj.status != newObj.status)
                {
                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.status),
                        old_value = oldObj.status_name,
                        new_value = newObj.status.ToListingItemName("status")
                    });
                }

                // log with change detail
                _logManager.LogObject(action, CommonConstant.ObjectType.user_role, newObj.role_id, newObj.name, logDetailList);
            }
            return result;
        }

        // Delete object
        // With permission check and log
        // LINQ to Store Procedures
        public CommonConstant.SystemCode SoftDelete(int role_id, ref string sql_remark)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.delete_status == 1)
            {
                int? sql_result = 0;

                var obj = GetDetail(role_id, true, ref systemCode);
                var delete_result = false;

                if (obj.role_id > 0)
                {
                    db.sp_SoftDeleteByModule(_accessObject.id, _accessObject.type, CommonConstant.Module.Role, obj.role_id, ref sql_result, ref sql_remark);
                    delete_result = (int)sql_result.Value == 1 ? true : false;

                    // delete privilege
                    var rolePrivilegeManager = new RolePrivilegeManager();
                    rolePrivilegeManager.Delete(SessionManager.Current.obj_id, 
                        (int)CommonConstant.ObjectType.member,
                        role_id, ref sql_remark);
                }

                if (delete_result)
                {
                    systemCode = CommonConstant.SystemCode.normal;

                    // take log
                    _logManager.LogObject(CommonConstant.ActionType.delete, CommonConstant.ObjectType.user_role, obj.role_id, obj.name);
                }
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
                systemCode = CommonConstant.SystemCode.no_permission;

            return systemCode;
        }

    }
}