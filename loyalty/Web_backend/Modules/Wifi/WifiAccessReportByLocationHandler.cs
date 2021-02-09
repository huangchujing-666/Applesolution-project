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
    public class WifiAccessReportByLocationHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var wifiAccessHistoryManager = new WifiAccessHistoryManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = wifiAccessHistoryManager.ReportByLocation(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.location_id,
                    location_no = x.location_no,
                    location_name = x.location_name,
                    count = x.count,

                    href = "new com.embraiz.tag().openNewTag('EDIT_WL:" + x.location_id + "','Location: " + x.location_no + "','com.palmary.WifiLocation.js.edit','iconRole16','iconRole16','iconRole16','" + x.location_id + "')",
                }
            );

            return resultDataList.ToJson();
        }
    }
}