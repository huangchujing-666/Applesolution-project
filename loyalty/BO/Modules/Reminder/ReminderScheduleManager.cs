using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Reminder;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.BO.Modules.Reminder
{
    public class ReminderScheduleManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.reminderSchedule;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public ReminderScheduleManager()
        {

            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();

        }

        public CommonConstant.SystemCode Create(
            ReminderScheduleObject obj
        )
        {
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                int? new_obj_id = 0;
                var result = db.sp_CreateReminderSchedule(
                    _accessObject.id,
                _accessObject.type, 

                    obj.reminder_engine_id,
                    obj.day,
                    obj.template_type,
                    obj.template_id,

                    ref new_obj_id,
                    ref sql_result
                    );

                systemCode = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return systemCode;
        }

        public List<ReminderScheduleObject> GetListByReminder(int reminder_engine_id, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<ReminderScheduleObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from rs in db.reminder_schedules
                             join rt in db.reminder_templates on rs.template_id equals rt.reminder_template_id
                             join li in db.listing_items on rs.template_type equals li.value

                             where (
                                rs.record_status != (int)CommonConstant.RecordStatus.deleted
                                && rs.reminder_engine_id == reminder_engine_id
                                && li.list_id == (int)CommonConstant.ListingType.ReminderTemplateType
                             )
                             select new ReminderScheduleObject
                             {
                                 reminder_schedule_id = rs.reminder_schedule_id,
                                 reminder_engine_id = rs.reminder_engine_id,
                                 day = rs.day,
                                 template_type = rs.template_type,
                                 template_id = rs.template_id,

                                 // additional
                                 template_type_name = li.name,
                                 template_name = rt.name
                             });

                resultList = query.OrderBy(x=> x.day).ToList();
                
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }
    }
}
