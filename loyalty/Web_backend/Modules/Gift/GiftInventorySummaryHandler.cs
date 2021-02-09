using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Gift
{
    public class GiftInventorySummaryHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            GiftInventoryManager _giftInventoryManager = new GiftInventoryManager();
            
            //var resultList = _giftInventoryManager.GetGiftInventorySummary(user_id, gift_id, startRowIndex, rowLimit, "").ToList();
            //var redemption_count = 0;
            //var stockChange_count = 0;

            //foreach (var item in resultList)
            //{
            //    if (item.type_id == 2) // Redemption
            //        redemption_count = 0 - item.count.Value; // convert to positive value
            //    else if (item.type_id == 1) // StockChange
            //        stockChange_count = item.count.Value;
            //}

            //var list = new List<ExtJsDataRow_giftInventorySummary>();

            //list.Add(new ExtJsDataRow_giftInventorySummary
            //    {
            //        id = 0,
            //        name = "Current Stock",
            //        value = (stockChange_count - redemption_count).ToString()
            //    }
            //);

            //list.Add(new ExtJsDataRow_giftInventorySummary
            //    {
            //        id = 1,
            //        name = "Redemption Count",
            //        value = (redemption_count).ToString()
            //    }
            //);

            return ""; // list.ToJson();
        }

        public class ExtJsDataRow_giftInventorySummary
        {
            public int id { get; set; }
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}