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
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Wifi
{
    public class WifiAccessHistoryManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.wifiAccessHistory;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public WifiAccessHistoryManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            WifiAccessHistoryObject dataObject
        )
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            var result = db.sp_CreateWifiAccessHistory(
                _accessObject.id,
                _accessObject.type, 

                dataObject.location_id,
                dataObject.member_id,
                dataObject.client_ip,
                dataObject.client_mac_address,
                dataObject.status,
                    
                ref sql_result
                );

            system_code = (CommonConstant.SystemCode)sql_result.Value;

           
            return system_code;
        }

        // Get List with paging, dynamic search, dynamic sorting
        public List<WifiAccessHistoryObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<WifiAccessHistoryObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from h in db.wifi_access_histories
                             join m in db.member_profiles on h.member_id equals m.member_id
                             join l in db.wifi_locations on h.location_id equals l.location_id
                             
                             where (
                                h.record_status != (int)CommonConstant.RecordStatus.deleted
                                
                            )
                             select new WifiAccessHistoryObject
                             {
                                 history_id = h.history_id,
                                 location_id = h.location_id,
                                 member_id = h.member_id,
                                 client_ip = h.client_ip,
                                 client_mac_address = h.client_mac_address,
                                 status = h.status,
                                 crt_date = h.crt_date,
                                 crt_by_type = h.crt_by_type,
                                 crt_by = h.crt_by,
                                 upd_date = h.upd_date,
                                 upd_by_type = h.upd_by_type,
                                 upd_by = h.upd_by,
                                 record_status = h.record_status,

                                 // additional
                                 member_no = m.member_no,
                                 location_no = l.location_no,
                                 location_name = l.name

                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "location_no")
                    {
                        query = query.Where(x => x.location_no.Contains(f.value));
                    }
                    else if (f.property == "location_name")
                    {
                        query = query.Where(x => x.location_name.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "member_no")
                    {
                        query = query.Where(x => x.member_no.Contains(f.value));
                    }
                    else if (f.property == "client_ip")
                    {
                        query = query.Where(x => x.client_ip.Contains(f.value));
                    }
                    else if (f.property == "client_mac_address")
                    {
                        query = query.Where(x => x.client_mac_address.Contains(f.value));
                    }
                }

                // dynamic sort
                Func<WifiAccessHistoryObject, Object> orderByFunc = null;
                if (sortColumn == "location_no")
                    orderByFunc = x => x.location_no;
                else if (sortColumn == "location_name")
                    orderByFunc = x => x.location_name;
                else if (sortColumn == "member_no")
                    orderByFunc = x => x.member_no;
                else if (sortColumn == "client_ip")
                    orderByFunc = x => x.client_ip;
                else if (sortColumn == "client_mac_address")
                    orderByFunc = x => x.client_mac_address;
                else if (sortColumn == "crt_date")
                    orderByFunc = x => x.crt_date;
                else
                {
                    sortOrder = CommonConstant.SortOrder.desc;
                    orderByFunc = x => x.crt_date;
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "location_no"
                    || sortColumn == "location_name"
                    || sortColumn == "member_no"
                    || sortColumn == "client_ip"
                    || sortColumn == "client_mac_address"
                    || sortColumn == "crt_date"
                    )
                    orderByColumn = sortColumn;
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

                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultList = new List<WifiAccessHistoryObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<WifiAccessReportObject.ByLocation> ReportByLocation(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<WifiAccessReportObject.ByLocation> resultList;

            if (_privilege.read_status == 1)
            {
                 var query = (from l in db.wifi_locations
                              join sub in (
                                             from h in db.wifi_access_histories
                                                group h by h.location_id into g
                                                select new {
                                                    location_id = g.Key,
                                                    count = g.Count()
                                                }
                                           )
                               on l.location_id equals sub.location_id into g_table
                               from sub in g_table.DefaultIfEmpty()

                             where (
                                l.record_status != (int)CommonConstant.RecordStatus.deleted
                            )
                             select new WifiAccessReportObject.ByLocation
                             {                                
                                 location_id = l.location_id,

                                 location_no = l.location_no,
                                 location_name = l.name,
                                 count = sub.count == null ? 0 : sub.count
                             });

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "location_no"
                    || sortColumn == "location_name"
                    || sortColumn == "count")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "location_no";
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
                resultList = new List<WifiAccessReportObject.ByLocation>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public List<WifiAccessReportObject.ByMemberLevel> ReportByMemberLevel(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<WifiAccessReportObject.ByMemberLevel> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from l in db.member_levels
                             join sub in
                                 (
                                    from h in db.wifi_access_histories
                                    join m in db.member_profiles on h.member_id equals m.member_id
                                    join ml in db.member_levels on m.member_level_id equals ml.level_id
                                    group h by ml.level_id into g
                                    select new
                                    {
                                        level_id = g.Key,
                                        count = g.Count()
                                    }
                                  )
                              on l.level_id equals sub.level_id into g_table
                             from sub in g_table.DefaultIfEmpty()

                            // where (
                            //    l.record_status != (int)CommonConstant.RecordStatus.deleted

                            //)
                             select new WifiAccessReportObject.ByMemberLevel
                             {
                                 level_id = l.level_id,
                                 level_name = l.name,
                                 display_order = l.display_order,
                                 count = sub.count == null ? 0 : sub.count
                             });

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "level_name"
                    || sortColumn == "count")
                    orderByColumn = sortColumn;
                else
                { //default
                    sortOrder = CommonConstant.SortOrder.asc;
                    orderByColumn = "display_order";
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
                resultList = new List<WifiAccessReportObject.ByMemberLevel>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }
    }
}