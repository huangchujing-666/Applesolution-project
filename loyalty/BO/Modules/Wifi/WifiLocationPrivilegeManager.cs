using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Palmary.Loyalty.BO.Database;
using System.Data.Objects;
using Palmary.Loyalty.BO.DataTransferObjects.Wifi;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Wifi;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.DB;


namespace Palmary.Loyalty.BO.Modules.Wifi
{
    public class WifiLocationPrivilegeManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.wifiLocationPrivilege;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public WifiLocationPrivilegeManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            WifiLocationPrivilegeObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateWifiLocationPrivilege(
                     _accessObject.id,
                _accessObject.type, 

                    dataObject.location_id,
                    dataObject.member_level_id,
                    dataObject.status,
                  
                    ref sql_result
                    );

                system_code = (CommonConstant.SystemCode)sql_result.Value;

            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public List<WifiLocationPrivilegeObject> GetList(int location_id, bool root_access, ref CommonConstant.SystemCode systemCode)
        {
            List<WifiLocationPrivilegeObject> resultList;

            if (_privilege.read_status == 1 || root_access)
            {
                var query = (from wlp in db.wifi_location_privileges
                             join ml in db.member_levels on wlp.member_level_id equals ml.level_id
                             join ml_lang in db.member_level_langs on wlp.member_level_id equals ml_lang.level_id
                             where (
                                wlp.record_status != (int)CommonConstant.RecordStatus.deleted
                                && wlp.location_id == location_id
                                && ml_lang.lang_id == (int)CommonConstant.LangCode.en
                            )
                             select new WifiLocationPrivilegeObject
                             {
                                 privilege_id = wlp.privilege_id,
                                 location_id = wlp.location_id,
                                 member_level_id = wlp.member_level_id,
                                 status = wlp.status,
                                 crt_date = wlp.crt_date,
                                 crt_by_type = wlp.crt_by_type,
                                 crt_by = wlp.crt_by,
                                 upd_date = wlp.upd_date,
                                 upd_by_type = wlp.upd_by_type,
                                 upd_by = wlp.upd_by,
                                 record_status = wlp.record_status,

                                 //additional
                                 member_level_display_order = ml.display_order,
                                 member_level_name = ml_lang.name
                             });

                resultList = query.OrderBy(x => x.member_level_display_order).ToList();

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<WifiLocationPrivilegeObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public CommonConstant.SystemCode Delete(
           int location_id
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_DelWifiLocationPrivilege(
                    SessionManager.Current.obj_id,

                    location_id,
                    
                    ref sql_result
                    );

                system_code = (CommonConstant.SystemCode)sql_result.Value;
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

    }
}
