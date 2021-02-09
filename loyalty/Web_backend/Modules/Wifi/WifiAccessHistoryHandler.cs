using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Wifi;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;

using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Wifi
{
    public class WifiAccessHistoryHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var wifiAccessHistoryManager = new WifiAccessHistoryManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = wifiAccessHistoryManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.history_id,
                    location_no = x.location_no,
                    location_name = x.location_name,
                    member_no = x.member_no,
                    client_ip = x.client_ip,
                    client_mac_address = x.client_mac_address,
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),

                    href = "new com.embraiz.tag().openNewTag('EDIT_WL:" + x.location_id + "','Location: " + x.location_no + "','com.palmary.WifiLocation.js.edit','iconRole16','iconRole16','iconRole16','" + x.location_id + "')",
                    href1 = "new com.embraiz.tag().openNewTag('EDIT_M_UID:" + x.member_id.ToString() + "','Member: " + x.member_no + "','com.palmary.memberProfile.js.edit','iconRole16','iconRole16','iconRole16','" + x.member_id + "')"
                }
            );

            return resultDataList.ToJson();
        }
    }
}