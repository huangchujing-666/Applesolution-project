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
using Palmary.Loyalty.BO.Modules.Member;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Role;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.PointEngine;
using Palmary.Loyalty.BO.Modules.Utility;

namespace Palmary.Loyalty.BO.Modules.Wifi
{
    public class WifiLocationManager
    {
        private static LoyaltyDBDataContext db = new LoyaltyDBDataContext();
        private AccessManager _accessManager;
        private string _module = CommonConstant.Module.wifiLocation;
        private RolePrivilegeObject _privilege;
        private static LogManager _logManager;
        private AccessObject _accessObject;

        public WifiLocationManager()
        {
            // access object
            _accessManager = new AccessManager();
            _accessObject = _accessManager.GetAccessObjectBySession();

            // privilege and log
            _privilege = _accessManager.AccessModule(_module);
            _logManager = new LogManager();
        }

        public CommonConstant.SystemCode Create(
            WifiLocationObject dataObject
        )
        {
            int? sql_result = 0;
            int? new_location_id = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.insert_status == 1)
            {
                var result = db.sp_CreateWifiLocation(
                    _accessObject.id,
                _accessObject.type, 


                    dataObject.location_no,
                    dataObject.name,
                    dataObject.mac_address,
                    dataObject.point,
                    dataObject.status,
                    
                    ref new_location_id,
                    ref sql_result
                    );

                system_code = (CommonConstant.SystemCode)sql_result.Value;

                if (system_code == CommonConstant.SystemCode.normal)
                { 
                    // create member_level
                    var wifiLocationPrivilegeManager = new WifiLocationPrivilegeManager();

                    foreach (var ml in dataObject.privilege_list)
                    {
                        ml.location_id = new_location_id.Value;
                        ml.status = CommonConstant.Status.active;

                        wifiLocationPrivilegeManager.Create(ml);
                    }
                }
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        public CommonConstant.SystemCode Update(WifiLocationObject dataObject)
        {
            int? sql_result = 0;
            var system_code = CommonConstant.SystemCode.undefine;

            if (_privilege.update_status == 1)
            {
                var result = db.sp_UpdateWifiLocation(
                     _accessObject.id,
                _accessObject.type, 

                    dataObject.location_id,
                    dataObject.location_no,
                    dataObject.name,
                    dataObject.mac_address,
                    dataObject.point,
                    dataObject.status,

                    ref sql_result);

                system_code = (CommonConstant.SystemCode)sql_result.Value;

                if (system_code == CommonConstant.SystemCode.normal)
                {
                    // update member_level
                    var wifiLocationPrivilegeManager = new WifiLocationPrivilegeManager();

                    wifiLocationPrivilegeManager.Delete(dataObject.location_id);
                    foreach (var ml in dataObject.privilege_list)
                    {
                        ml.location_id = dataObject.location_id;
                        ml.status = CommonConstant.Status.active;

                        wifiLocationPrivilegeManager.Create(ml);
                    }
                }
            }
            else
            {
                system_code = CommonConstant.SystemCode.no_permission;
            }

            return system_code;
        }

        // Get List with paging, dynamic search, dynamic sorting
        public List<WifiLocationObject> GetList(int rowIndexStart, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref CommonConstant.SystemCode systemCode, ref int totalRow)
        {
            List<WifiLocationObject> resultList;

            if (_privilege.read_status == 1)
            {
                var query = (from wl in db.wifi_locations
                             
                             join li in db.listing_items on wl.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id
                             
                             where (
                                wl.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status"
                            )
                             select new WifiLocationObject
                             {
                                 location_id = wl.location_id,
                                 location_no = wl.location_no,
                                 name = wl.name,
                                 mac_address = wl.mac_address,
                                 point = wl.point,
                                 status = wl.status,
                                 crt_date = wl.crt_date,
                                 crt_by_type = wl.crt_by_type,
                                 crt_by = wl.crt_by,
                                 upd_date = wl.upd_date,
                                 upd_by_type = wl.upd_by_type,
                                 upd_by = wl.upd_by,
                                 record_status = wl.record_status,

                                 // additional
                                 status_name = li.name
                             });

                // dynamic search
                foreach (var f in searchParmList)
                {
                    if (f.property == "location_no")
                    {
                        query = query.Where(x => x.location_no.Contains(f.value));
                    }
                    else if (f.property == "name")
                    {
                        query = query.Where(x => x.name.ToLower().Contains(f.value.ToLower()));
                    }
                    else if (f.property == "mac_address")
                    {
                        query = query.Where(x => x.mac_address.Contains(f.value));
                    }
                }

                // dynamic sort
                var orderByColumn = "";
                if (sortColumn == "location_no"
                    || sortColumn == "name"
                    || sortColumn == "mac_address"
                    )
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
                resultList = new List<WifiLocationObject>();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultList;
        }

        public WifiLocationObject GetDetail(int location_id, bool rootAccess, ref CommonConstant.SystemCode systemCode)
        {
            WifiLocationObject resultObj;

            if (_privilege.read_status == 1 || rootAccess)
            {
                var query = (from wl in db.wifi_locations

                             join li in db.listing_items on wl.status equals li.value
                             join l in db.listings on li.list_id equals l.list_id

                             where (
                                wl.record_status != (int)CommonConstant.RecordStatus.deleted
                                && l.code == "Status"
                                &&wl.location_id == location_id
                            )
                             select new WifiLocationObject
                             {
                                 location_id = wl.location_id,
                                 location_no = wl.location_no,
                                 name = wl.name,
                                 mac_address = wl.mac_address,
                                 point = wl.point,
                                 status = wl.status,
                                 crt_date = wl.crt_date,
                                 crt_by_type = wl.crt_by_type,
                                 crt_by = wl.crt_by,
                                 upd_date = wl.upd_date,
                                 upd_by_type = wl.upd_by_type,
                                 upd_by = wl.upd_by,
                                 record_status = wl.record_status,

                                 // additional
                                 status_name = li.name
                             });

                resultObj = query.FirstOrDefault();
                
                systemCode = CommonConstant.SystemCode.normal;
            }
            else
            {
                resultObj = new WifiLocationObject();
                systemCode = CommonConstant.SystemCode.no_permission;
            }

            return resultObj;
        }

         public WifiLocationObject GetDetail(string node_mac, ref CommonConstant.SystemCode systemCode)
        {
            WifiLocationObject resultObj;

            var query = (from wl in db.wifi_locations

                            join li in db.listing_items on wl.status equals li.value
                            join l in db.listings on li.list_id equals l.list_id

                            where (
                            wl.record_status != (int)CommonConstant.RecordStatus.deleted
                            && l.code == "Status"
                            &&wl.mac_address == node_mac
                        )
                            select new WifiLocationObject
                            {
                                location_id = wl.location_id,
                                location_no = wl.location_no,
                                name = wl.name,
                                mac_address = wl.mac_address,
                                point = wl.point,
                                status = wl.status,
                                crt_date = wl.crt_date,
                                crt_by_type = wl.crt_by_type,
                                crt_by = wl.crt_by,
                                upd_date = wl.upd_date,
                                upd_by_type = wl.upd_by_type,
                                upd_by = wl.upd_by,
                                record_status = wl.record_status,

                                // additional
                                status_name = li.name
                            });

            if (query.Count() == 0)
                resultObj = new WifiLocationObject();
            else
                resultObj = query.FirstOrDefault();
                
            systemCode = CommonConstant.SystemCode.normal;
           

            return resultObj;
        }

        public CommonConstant.SystemCode MemberAccessWifi(int member_id, string node_mac, string client_ip, string client_mac)
        {
            var systemCode = CommonConstant.SystemCode.undefine;

            var memberManager = new MemberManager();
            var member = memberManager.GetDetail(member_id, true, ref systemCode);

            if (member.member_id > 0)
            {
                var wifiLocation = GetDetail(node_mac, ref systemCode);

                if (wifiLocation.location_id > 0)
                {
                    var wifiLocationPrivilegeManager = new WifiLocationPrivilegeManager();
                    var location_pList = wifiLocationPrivilegeManager.GetList(wifiLocation.location_id, true, ref systemCode);

                    foreach (var p in location_pList)
                    {
                        if (p.member_level_id == member.member_level_id)
                        {
                            systemCode = CommonConstant.SystemCode.normal;
                            break;
                        }
                    }

                    if (systemCode == CommonConstant.SystemCode.normal)
                    { 
                        // wifi access history
                        var historyObject = new WifiAccessHistoryObject()
                        {
                            location_id = wifiLocation.location_id,
                            member_id = member_id,
                            client_ip = client_ip,
                            client_mac_address = client_mac,
                            status = CommonConstant.Status.active
                        };

                        var wifiAccessHistoryManager = new WifiAccessHistoryManager();
                        wifiAccessHistoryManager.Create(historyObject);

                        // location gain point
                        var pointEngineManager  = new PointEngineManager();
                        systemCode = pointEngineManager.WifiLocationPresence(wifiLocation.location_id, member_id);
                    }
                    else
                        systemCode = CommonConstant.SystemCode.no_permission;
                }
                else
                    systemCode = CommonConstant.SystemCode.err_location_not_exist;
            }else
                systemCode = CommonConstant.SystemCode.err_member_not_exist;

            return systemCode;
        }
    }
}
