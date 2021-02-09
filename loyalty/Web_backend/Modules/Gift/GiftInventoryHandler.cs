using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;

namespace Palmary.Loyalty.Web_backend.Modules.Gift
{
    public class GiftInventoryHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            GiftInventoryManager _giftInventoryManager = new GiftInventoryManager();
            
            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = _giftInventoryManager.GetList(prefix_id, startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultDataList = resultList.Select(
                x => new
                {
                    inventory_id = x.inventory_id,
                    gift_no = x.gift_no,
                    stock_change = x.stock_change > 0 ? "+" + x.stock_change : x.stock_change.ToString(),
                    stock_change_type_name = x.stock_change_type_name,
                    gift_name = x.gift_name,
                    remark = x.remark,
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss:fff")
                    //href = ""
                }
            );
            return resultDataList.ToJson();
        }
    }
}