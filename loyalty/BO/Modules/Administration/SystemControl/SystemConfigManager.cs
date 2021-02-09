using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.SystemConfig;
using System.Data.Objects;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Administration.SystemControl
{
    public class SystemConfigManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
      
        private AccessManager _accessManager;
        private RolePrivilegeObject _privilege;
        private string _module = CommonConstant.Module.systemConfig;
        private AccessObject _accessObject;
        private static LogManager _logManager;

        // For backend, using BO Session to access
        public SystemConfigManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        // For Webservices or other not using BO Session to access
        public SystemConfigManager(AccessObject accessObject)
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = accessObject;

            // privilege and log
            _privilege = _accessManager.AccessModule(_module, _accessObject.type, _accessObject.id);
            _logManager = new LogManager(_accessObject);
        }

        public List<SystemConfigObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            int? get_sql_result = 0;
            var sql_remark = "";

            List<SystemConfigObject> resultList = new List<SystemConfigObject>(); ;

            systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.read_status == 1)
            {
                try
                {
                    var result = db.sp_GetSystemConfigList(_accessObject.id, ref get_sql_result, ref sql_remark);

                    resultList = result.Select(x => new SystemConfigObject
                    {
                        config_id = x.config_id,
                        name = x.name,
                        value = x.value,
                        data_type = x.data_type,
                        display = x.display,
                        display_order = x.display_order,
                        edit = (_privilege.update_status == 1) ? x.edit : false,
                        crt_date = x.crt_date,
                        crt_by_type = x.crt_by_type,
                        crt_by = x.crt_by,
                        upd_date = x.upd_date,
                        upd_by_type = x.upd_by_type,
                        upd_by = x.upd_by,
                        record_status = x.record_status
                    }).ToList();

                    systemCode = CommonConstant.SystemCode.normal;

                }
                catch (Exception ex)
                {
                    systemCode = CommonConstant.SystemCode.record_invalid;
                }
            }
            else
                systemCode = CommonConstant.SystemCode.no_permission;

            return resultList;
        }

        public SystemConfigObject GetDetail(int config_id, ref CommonConstant.SystemCode systemCode)
        {
            return GetDetail(config_id, false, ref systemCode);
        }

        public SystemConfigObject GetDetail(int config_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            //Linq to sql
            var resultObj = new SystemConfigObject(); ;

            systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from s in db.system_configs

                             where (
                                s.record_status != (int)CommonConstant.RecordStatus.deleted
                                && s.config_id == config_id
                                )
                             select new SystemConfigObject
                             {
                                 config_id = s.config_id,
                                 name = s.name,
                                 value = s.value,
                                 data_type = s.data_type,
                                 display = s.display,
                                 display_order = s.display_order,
                                 edit = (_privilege.update_status == 1) ? s.edit : false,
                                 regex = s.regex,
                                 regex_text = s.regex_text,
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

        public CommonConstant.SystemCode Update(SystemConfigObject obj)
        {
            // Linq to store prod
            int? sql_result = 0;
            var systemCode = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                // cache original data
                var oldObj = GetDetail(obj.config_id, true, ref systemCode);

                var result = db.sp_UpdateSystemConfig(
                    _accessObject.id,
                          _accessObject.type,

                    obj.config_id,
                    obj.value,

                    ref sql_result);

                systemCode = (CommonConstant.SystemCode)sql_result.Value;

                // log with change detail
                if (systemCode == CommonConstant.SystemCode.normal)
                {
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
        public CommonConstant.SystemCode LogAndCompare(int action, SystemConfigObject oldObj, SystemConfigObject newObj)
        {
            var result = CommonConstant.SystemCode.undefine;

            if (oldObj.config_id != newObj.config_id || oldObj.config_id == 0 || newObj.config_id == 0)
                result = CommonConstant.SystemCode.record_invalid;
            else
            { // valid objects
                var logDetailList = new List<LogDetailObject>();

                if (oldObj.value != newObj.value)
                {
                    logDetailList.Add(new LogDetailObject()
                    {
                        field_name = UtilityManager.GetPropertyDisplayName(() => newObj.value),
                        old_value = oldObj.value,
                        new_value = newObj.value
                    });
                }

                // log with change detail
                _logManager.LogObject(action, CommonConstant.ObjectType.system_config, newObj.config_id, oldObj.name, logDetailList);
            }
            return result;
        }

        public string GetSystemConfigValue(
           string name,
           ref string sql_remark
        )
        {
            var config_value = "";
            int? get_sql_result = 0;

            var result = db.sp_GetSystemConfigValue(
                _accessObject.id,
                name,

                ref config_value,
                ref get_sql_result,
                ref sql_remark);

            return config_value;
        }
    }
}
