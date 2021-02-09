using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;


namespace Palmary.Loyalty.BO.Modules.Administration.Security
{
    public class LogDetailManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private string _module = CommonConstant.Module.logDetail; 
        private static LogManager _logManager;
        private AccessObject _accessObject;

        // init and load privilege
        public LogDetailManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // Create object
        // no need check permission
        // LINQ to Store Procedures
        public CommonConstant.SystemCode Create(
            LogDetailObject obj
        )
        {
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            int? new_obj_id = 0;

            var result = db.sp_CreateLogDetail(
                _accessObject.id,
                _accessObject.type, 

                obj.log_id,
                obj.field_name,
                obj.old_value,
                obj.new_value,

                ref new_obj_id,
                ref sql_result
                );

            systemCode = (CommonConstant.SystemCode)sql_result.Value;

            return systemCode;
        }


        // Get list of detail by id
        // With permission check
        // LINQ to SQL directly
        public List<LogDetailObject> GetList(int log_id, ref CommonConstant.SystemCode systemCode)
        {
            return GetList(log_id, false, ref systemCode);
        }

        // Get list of detail by id
        // Allow root access to override privilege
        // With permission check
        // LINQ to SQL directly
        public List<LogDetailObject> GetList(int log_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultList = new List<LogDetailObject>();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from d in db.log_details

                             where (
                                d.record_status != (int)CommonConstant.RecordStatus.deleted
                                && d.log_id == log_id
                                )
                             select new LogDetailObject
                             {
                                 log_detail_id = d.log_detail_id,
                                 log_id = d.log_id,
                                 field_name = d.field_name,
                                 old_value = d.old_value,
                                 new_value = d.new_value,
                                 crt_date = d.crt_date,
                                 crt_by_type = d.crt_by_type,
                                 crt_by = d.crt_by,
                                 upd_date = d.upd_date,
                                 upd_by_type = d.upd_by_type,
                                 upd_by = d.upd_by,
                                 record_status = d.record_status,
                             });

                if (query.Count() > 0)
                {
                    resultList = query.ToList();
                    systemCode = CommonConstant.SystemCode.normal;
                }
                else
                    systemCode = CommonConstant.SystemCode.record_invalid;
            }
            else
            {
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        // Get detail by id
        // With permission check
        public LogDetailObject GetDetail(int log_id, ref CommonConstant.SystemCode systemCode)
        {
            return GetDetail(log_id, ref systemCode);
        }

        // Get detail by id
        // Allow root access to override privilege
        // With permission check
        // LINQ to SQL directly
        public LogDetailObject GetDetail(int log_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            systemCode = CommonConstant.SystemCode.undefine;
            var resultObj = new LogDetailObject();

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from d in db.log_details

                             where (
                                d.record_status != (int)CommonConstant.RecordStatus.deleted
                                && d.log_id == log_id
                                )
                             select new LogDetailObject
                             {
                                 log_detail_id = d.log_detail_id,
                                 log_id = d.log_id,
                                 field_name = d.field_name,
                                 old_value = d.old_value,
                                 new_value = d.new_value,
                                 crt_date = d.crt_date,
                                 crt_by_type = d.crt_by_type,
                                 crt_by = d.crt_by,
                                 upd_date = d.upd_date,
                                 upd_by_type = d.upd_by_type,
                                 upd_by = d.upd_by,
                                 record_status = d.record_status,
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
    }
}