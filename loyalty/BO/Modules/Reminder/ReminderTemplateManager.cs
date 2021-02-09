using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Reminder;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Security;

namespace Palmary.Loyalty.BO.Modules.Reminder
{
    public class ReminderTemplateManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.reminderTemplate;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public ReminderTemplateManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            ReminderTemplateObject obj
        )
        {
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                int? new_obj_id = 0;
                var result = db.sp_CreateReminderTemplate(
                   _accessObject.id, 
                   _accessObject.type, 

                    obj.name,
                    obj.sms_template,
                    obj.email_template,
                    obj.status,
                  
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

        public CommonConstant.SystemCode Update(
            ReminderTemplateObject obj
        )
        {
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateReminderTemplate(
                     _accessObject.id,
                _accessObject.type, 

                    obj.reminder_template_id,
                    obj.name,
                    obj.sms_template,
                    obj.email_template,
                    obj.status,

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

        public List<ReminderTemplateObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<ReminderTemplateObject>();

            if (_privilege.read_status == 1)
            {
                var query = (from rt in db.reminder_templates
                             join li in db.listing_items on rt.status equals li.value

                             where (
                                rt.record_status != (int)CommonConstant.RecordStatus.deleted
                                && li.list_id == (int)CommonConstant.ListingType.Status
                             )
                             select new ReminderTemplateObject
                             {
                                 reminder_template_id = rt.reminder_template_id,
                                 name = rt.name,
                                 sms_template = rt.sms_template,
                                 email_template = rt.email_template,
                                 status = rt.status,

                                 crt_date = rt.crt_date,
                                 crt_by_type = rt.crt_by_type,
                                 crt_by = rt.crt_by,
                                 upd_date = rt.upd_date,
                                 upd_by_type = rt.upd_by_type,
                                 upd_by = rt.upd_by,
                                 record_status = rt.record_status,

                                 // additional
                                 status_name = li.name
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
                if (string.IsNullOrEmpty(sortColumn))
                {
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "name";
                }
                else if (sortColumn == "name")
                    orderByColumn = "name";
                else if (sortColumn == "status_name")
                    orderByColumn = "status_name";
                else
                    orderByColumn = sortColumn;

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

        public List<ReminderTemplateObject> GetListAll(bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<ReminderTemplateObject>();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from rt in db.reminder_templates
                             join li in db.listing_items on rt.status equals li.value

                             where (
                                rt.record_status != (int)CommonConstant.RecordStatus.deleted
                                && li.list_id == (int)CommonConstant.ListingType.Status
                             )
                             select new ReminderTemplateObject
                             {
                                 reminder_template_id = rt.reminder_template_id,
                                 name = rt.name,
                                 sms_template = rt.sms_template,
                                 email_template = rt.email_template,
                                 status = rt.status,

                                 crt_date = rt.crt_date,
                                 crt_by_type = rt.crt_by_type,
                                 crt_by = rt.crt_by,
                                 upd_date = rt.upd_date,
                                 upd_by_type = rt.upd_by_type,
                                 upd_by = rt.upd_by,
                                 record_status = rt.record_status,

                                 // additional
                                 status_name = li.name
                             });

                resultList = query.OrderBy(x => x.name).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public ReminderTemplateObject GetDetail(int reminder_template_id, ref CommonConstant.SystemCode systemCode)
        {
            // LINQ to SQL
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new ReminderTemplateObject();

            if (_privilege.read_status == 1)
            {
                var query = (from rt in db.reminder_templates
                             join li in db.listing_items on rt.status equals li.value

                             where (
                                rt.record_status != (int)CommonConstant.RecordStatus.deleted
                                && li.list_id == (int)CommonConstant.ListingType.Status
                                && rt.reminder_template_id == reminder_template_id
                             )
                             select new ReminderTemplateObject
                             {
                                 reminder_template_id = rt.reminder_template_id,
                                 name = rt.name,
                                 sms_template = rt.sms_template,
                                 email_template = rt.email_template,
                                 status = rt.status,

                                 crt_date = rt.crt_date,
                                 crt_by_type = rt.crt_by_type,
                                 crt_by = rt.crt_by,
                                 upd_date = rt.upd_date,
                                 upd_by_type = rt.upd_by_type,
                                 upd_by = rt.upd_by,
                                 record_status = rt.record_status,

                                 // additional
                                 status_name = li.name
                             });

                resultObj = query.FirstOrDefault();
                systemCode = CommonConstant.SystemCode.normal;

            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }
    }
}
