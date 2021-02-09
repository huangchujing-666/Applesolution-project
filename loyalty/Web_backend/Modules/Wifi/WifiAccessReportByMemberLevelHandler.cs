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
    public class WifiAccessReportByMemberLevelHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var wifiAccessHistoryManager = new WifiAccessHistoryManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = wifiAccessHistoryManager.ReportByMemberLevel(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.level_id,
                  
                    level_name = x.level_name,
                    count = x.count,

                    href = ""
                }
            );

            return resultDataList.ToJson();
        }
    }
}