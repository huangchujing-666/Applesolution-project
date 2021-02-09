using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.DataTransferObjects.Form;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Administration.Security
{
    public class LogManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private string _module = CommonConstant.Module.log;
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private AccessObject _accessObject;
        
        // For backend, using BO Session to access
        public LogManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege 
            _privilege = _accessManager.AccessModule(_module);
        }

        // For Webservices or other not using BO Session to access
        public LogManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
        }

        public IEnumerable<sp_GetLog_listsResult> GetLogLists_sp(long rowIndexStart, int rowLimit, string searchParams)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            IEnumerable<sp_GetLog_listsResult> result = null;

            try
            {
                result = db.sp_GetLog_lists(_accessObject.id, rowIndexStart, rowLimit, ref get_sql_result, ref sql_remark); //rowIndexStart, rowLimit, searchParams, get_sql_result, get_sql_remark);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "get log error");
            }
            return result;
        }

        public List<LogObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<LogObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from l in db.logs
                             join ao in db.v_accessObjects on new { type = l.crt_by_type, id = l.crt_by } equals new { type = ao.type, id = ao.target_id }
                             // for access object type name
                             join ao_t in db.listing_items on new { list_id = (int)CommonConstant.ListingType.ObjectType, value = l.crt_by_type } equals new { list_id = ao_t.list_id, value = ao_t.value}

                             // for action_channel name 
                             join li_ac in db.listing_items on l.action_channel equals li_ac.value
                             join l_ac in db.listings on li_ac.list_id equals l_ac.list_id

                             // for action_type name 
                             join li_at in db.listing_items on l.action_type equals li_at.value
                             join l_at in db.listings on li_at.list_id equals l_at.list_id
                             
                             // for target object type name 
                             join li_tot in db.listing_items on l.target_obj_type_id equals li_tot.value into li_tot_table
                             from li_tot in li_tot_table.DefaultIfEmpty() // left outer join
                             join l_tot in db.listings on li_tot.list_id equals l_tot.list_id into l_tot_table
                             from l_tot in l_tot_table.DefaultIfEmpty() // left outer join

                             // join li_to in db.listing_items on so_t.type equals li_to.value
                             // join l_to in db.listings on li_to.list_id equals l_to.list_id

                             where (l.record_status != CommonConstant.RecordStatus.deleted
                                    && l_ac.code == "ActionChannel"
                                    && l_at.code == "ActionType"
                                    && ((l_tot.code == "ObjectType" && l.target_obj_id != null) || l.target_obj_id == null)
                                    )
                             select new LogObject
                             {
                                 log_id = l.log_id,
                                 action_ip = l.action_ip,
                                 action_channel_name = li_ac.name,
                                 action_channel = l.action_channel,
                                 action_type_name = li_at.name,
                                 action_type = l.action_type,
                                 target_obj_id = l.target_obj_id.Value,

                                 action_detail = l.action_detail,
                                 crt_date = l.crt_date,
                                 crt_by_type = l.crt_by_type,
                                 crt_by = l.crt_by,
                                 crt_by_type_name = ao_t.name,
                                 crt_by_name = ao.name,
                                 target_obj_name = l.target_obj_name ?? "",
                                 target_obj_type_id = l.target_obj_type_id,
                                 target_obj_type_name = li_tot.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "crt_by_name")
                    {
                        query = query.Where(x => x.crt_by_name.Contains(f.value));
                    }
                    else if (f.property == "crt_by_type_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_by_type == int.Parse(f.value));
                    }
                    else if (f.property == "action_channel_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.action_channel == int.Parse(f.value));
                    }
                    else if (f.property == "action_type_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.action_type == int.Parse(f.value));
                    }
                    else if (f.property == "target_obj_type_name")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.target_obj_type_id == int.Parse(f.value));
                    }
                    else if (f.property == "target_obj_name")
                    {
                        query = query.Where(x => x.target_obj_name.Contains(f.value));
                    }
                    else if (f.property == "action_ip")
                    {
                        query = query.Where(x => x.action_ip.Contains(f.value));
                    }
                    else if (f.property == "crt_date_larger")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date >= Convert.ToDateTime(f.value));
                    }
                    else if (f.property == "crt_date_lower")
                    {
                        if (!String.IsNullOrEmpty(f.value))
                            query = query.Where(x => x.crt_date <= Convert.ToDateTime(f.value + " 23:59:59.999"));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "crt_by_name"
                    || sortColumn == "crt_by_type_name"
                    || sortColumn == "action_channel_name"
                    || sortColumn == "action_type_name"
                    || sortColumn == "target_obj_type_name"
                    || sortColumn == "target_obj_name"
                    || sortColumn == "action_ip"
                    )
                    orderByColumn = sortColumn;
                else if (sortColumn == "log_date")
                    orderByColumn = "crt_date";
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByColumn = "crt_date";
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderBy(orderByColumn, true).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByColumn, false).Skip(rowIndexStart).Take(rowLimit).ToList();
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }


        // Get detail by id
        // With permission check
        public LogObject GetDetail(int log_id, ref CommonConstant.SystemCode systemCode)
        {
            return GetDetail(log_id, false, ref systemCode);
        }

        // Get detail by id
        // Allow root access to override privilege
        // With permission check
        // LINQ to SQL directly
        public LogObject GetDetail(int log_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new LogObject();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from l in db.logs
                             join so_a in db.system_objects on l.crt_by equals so_a.object_id

                             // for action_channel name 
                             join li_ac in db.listing_items on l.action_channel equals li_ac.value
                             join l_ac in db.listings on li_ac.list_id equals l_ac.list_id

                             // for action_type name 
                             join li_at in db.listing_items on l.action_type equals li_at.value
                             join l_at in db.listings on li_at.list_id equals l_at.list_id

                             // for target object type name 
                             join li_tot in db.listing_items on l.target_obj_type_id equals li_tot.value into li_tot_table
                             from li_tot in li_tot_table.DefaultIfEmpty() // left outer join
                             join l_tot in db.listings on li_tot.list_id equals l_tot.list_id into l_tot_table
                             from l_tot in l_tot_table.DefaultIfEmpty() // left outer join

                             // join li_to in db.listing_items on so_t.type equals li_to.value
                             // join l_to in db.listings on li_to.list_id equals l_to.list_id

                             where (l.record_status != CommonConstant.RecordStatus.deleted
                                    && l.log_id == log_id
                                    && l_ac.code == "ActionChannel"
                                    && l_at.code == "ActionType"
                                    && ((l_tot.code == "ObjectType" && l.target_obj_id != null) || l.target_obj_id == null)
                                    )
                             select new LogObject
                             {
                                 log_id = l.log_id,
                                 action_ip = l.action_ip,
                                 action_channel_name = li_ac.name,
                                 action_channel = l.action_channel,
                                 action_type_name = li_at.name,
                                 action_type = l.action_type,
                                 target_obj_id = l.target_obj_id.Value,

                                 action_detail = l.action_detail,
                                 crt_date = l.crt_date,
                                 crt_by_type = l.crt_by_type,
                                 crt_by = l.crt_by,
                                 crt_by_name = so_a.name,
                                 target_obj_name = l.target_obj_name ?? "",
                                 target_obj_type_id = l.target_obj_type_id,
                                 target_obj_type_name = li_tot.name
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

        private CommonConstant.SystemCode Create(
            LogObject dataObject,
            ref int new_obj_id
        )
        {
            int? sql_result = 0;
            int? new_log_id = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            var result = db.sp_CreateLog(
                _accessObject.id,
                         _accessObject.type,

                dataObject.action_ip,
                dataObject.action_channel,
                dataObject.action_type,

                dataObject.target_obj_type_id,
                dataObject.target_obj_id,
                dataObject.target_obj_name,

                dataObject.action_detail,

                ref new_log_id,
                ref sql_result
            );
            new_obj_id = new_log_id.Value;
            system_code = (CommonConstant.SystemCode)sql_result.Value;

            return system_code;
        }

        // log system object and other general object
        public CommonConstant.SystemCode LogObject(int action_type, int target_obj_type_id, int target_obj_id, string target_obj_name)
        {
            int new_log_id = 0;

            var systemCode = Create(new LogObject()
            {
                action_ip = _accessObject.ip,
                action_channel = _accessObject.actionChannel,
                action_type = action_type,

                target_obj_type_id = target_obj_type_id,
                target_obj_id = target_obj_id,
                target_obj_name = target_obj_name,
                action_detail = null
            },
            ref new_log_id);

            return systemCode;
        }

        // log system object and other general object
        // with change detail
        public CommonConstant.SystemCode LogObject(int action_type, int target_obj_type_id, int target_obj_id, string target_obj_name, List<LogDetailObject> detailList)
        {
            int new_log_id = 0;

            var systemCode = Create(new LogObject()
            {
                action_ip = _accessObject.ip,
                action_channel = _accessObject.actionChannel,
                action_type = action_type,

                target_obj_type_id = target_obj_type_id,
                target_obj_id = target_obj_id,
                target_obj_name = target_obj_name,
                action_detail = null
            },
            ref new_log_id);

            if (systemCode == CommonConstant.SystemCode.normal)
            {
                var logDetailManager = new LogDetailManager();

                foreach (var x in detailList)
                {
                    x.log_id = new_log_id;
                    logDetailManager.Create(x);
                }
            }

            return systemCode;
        }

        public void LogAction(int action_type)
        {
            int new_log_id = 0;

            Create(new LogObject()
            {
                action_ip = _accessObject.ip,
                action_channel = _accessObject.actionChannel,
                action_type = action_type,

                target_obj_type_id = null,
                target_obj_id = null,
                target_obj_name = null,
                action_detail = null
            },
            ref new_log_id);
        }

        public void LogActionWithTargetID(int action_type, int target_obj_id)
        {
            int new_log_id = 0;

            Create(new LogObject()
            {
                action_ip = _accessObject.ip,
                action_channel = _accessObject.actionChannel,
                action_type = action_type,

                target_obj_type_id = null,
                target_obj_id = target_obj_id,
                target_obj_name = null,
                action_detail = null
            },
            ref new_log_id);
        }
    }
}