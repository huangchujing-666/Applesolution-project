using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.DataTransferObjects.Reminder;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.BO.Modules.Reminder
{
    public class ReminderEngineManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.reminderEngine;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public ReminderEngineManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            ReminderEngineObject obj
        )
        {
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                int? new_obj_id = 0;
                var result = db.sp_CreateReminderEngine(
                   _accessObject.id,
                _accessObject.type, 


                    obj.name,
                    obj.target_type,
                    obj.target_value,
                    obj.status,

                    ref new_obj_id,
                    ref sql_result
                    );

                systemCode = (CommonConstant.SystemCode)sql_result.Value;

                // create schedule
                if (systemCode == CommonConstant.SystemCode.normal)
                {
                    var reminderScheduleManager = new ReminderScheduleManager();

                    foreach (var x in obj.scheduleList)
                    {
                        x.reminder_engine_id = new_obj_id.Value;

                        reminderScheduleManager.Create(x);
                    }
                }
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        public List<ReminderEngineObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<ReminderEngineObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from re in db.reminder_engines
                             join li in db.listing_items on re.target_type equals li.value
                             join li_s in db.listing_items on re.status equals li_s.value
                           
                             where (
                                re.record_status != (int)CommonConstant.RecordStatus.deleted
                                && li.list_id == (int)CommonConstant.ListingType.ReminderEngineType
                                && li_s.list_id == (int)CommonConstant.ListingType.Status
                             )
                             select new ReminderEngineObject
                             {
                                 reminder_engine_id = re.reminder_engine_id,
                                 name = re.name,
                                 target_type = re.target_type,
                                 target_value = re.target_value,
                                 status = re.status,
                                 crt_date = re.crt_date,
                                 crt_by_type = re.crt_by_type,
                                 crt_by = re.crt_by,
                                 upd_date = re.upd_date,
                                 upd_by_type = re.upd_by_type,
                                 upd_by = re.upd_by,
                                 record_status = re.record_status,

                                 // additional
                                 target_type_name = li.name,
                                 status_name = li_s.name
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
                Func<ReminderEngineObject, Object> orderByFunc = null;
                if (sortColumn == "name")
                    orderByFunc = x => x.name;
                else
                {
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByFunc = x => x.name;
                }

                // row total
                totalRow = query.Count();

                if (sortOrder == CommonConstant.SortOrder.desc)
                    resultList = query.OrderByDescending(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();
                else
                    resultList = query.OrderBy(orderByFunc).Skip(rowIndexStart).Take(rowLimit).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public ReminderEngineObject GetDetail(int reminder_engine_id, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new ReminderEngineObject();

            if (_privilege.read_status == 1)
            {
                var query = (from re in db.reminder_engines
                             join li in db.listing_items on re.target_type equals li.value
                             join li_s in db.listing_items on re.status equals li_s.value

                             where (
                                re.record_status != (int)CommonConstant.RecordStatus.deleted
                                && li.list_id == (int)CommonConstant.ListingType.ReminderEngineType
                                && li_s.list_id == (int)CommonConstant.ListingType.Status
                                && re.reminder_engine_id == reminder_engine_id
                             )
                             select new ReminderEngineObject
                             {
                                 reminder_engine_id = re.reminder_engine_id,
                                 name = re.name,
                                 target_type = re.target_type,
                                 target_value = re.target_value,
                                 status = re.status,
                                 crt_date = re.crt_date,
                                 crt_by_type = re.crt_by_type,
                                 crt_by = re.crt_by,
                                 upd_date = re.upd_date,
                                 upd_by_type = re.upd_by_type,
                                 upd_by = re.upd_by,
                                 record_status = re.record_status,

                                 // additional
                                 target_type_name = li.name,
                                 status_name = li_s.name
                             });

                resultObj = query.FirstOrDefault();
                systemCode = CommonConstant.SystemCode.normal;

                // get schedule
                var reminderScheduleManager = new ReminderScheduleManager();
                resultObj.scheduleList = reminderScheduleManager.GetListByReminder(resultObj.reminder_engine_id, ref systemCode);
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }
    }
}
