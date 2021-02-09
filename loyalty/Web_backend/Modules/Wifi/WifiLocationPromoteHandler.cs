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
using Palmary.Loyalty.BO.DataTransferObjects.Wifi;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Modules.Wifi
{
    public class WifiLocationPromoteHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            //var wifiLocationManager = new WifiLocationManager();

            //var resultCode = CommonConstant.SystemCode.undefine;
            //var resultList = wifiLocationManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            //var resultDataList = resultList.Select(
            //    x => new
            //    {
            //        name = x.name,
            //        location_no = x.location_no,
            //        id = x.location_id,
            //        mac_address = x.mac_address,

            //        href = "new com.embraiz.tag().openNewTag('EDIT_WL:" + x.location_id + "','Location: " + x.location_no + "','com.palmary.WifiLocation.js.edit','iconRole16','iconRole16','iconRole16','" + x.location_id + "')"
            //    }
            //);


            FileHandler _fileHandler = new FileHandler();

            List<dynamic> tempList = new List<dynamic>();

            tempList.Add(new
            {
                id = 1,
                href = "new com.embraiz.tag().openNewTag('EDIT_G:" + 242 + "','Gift: " + "CH01" + "','com.palmary.gift.js.edit','iconRole16','iconRole16','iconRole16','" + 242 + "')",
                action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('wifiLocationPromote:" + 1 + "','WifiLocation: Promote','com.palmary.wifilocation.promote_form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')\">View</button>",
                member_level = "Normal, Silver, Gold, Platinum",
                member_category = "Normal",
                pop_up_image = _fileHandler.GetImagePath("Coupon_01", ".jpg", (string)CommonConstant.Module.promote),
                earn_point = "0",
                earn_gift_name = "CH01"
            });

            tempList.Add(new
            {
                id = 2,
                href = "new com.embraiz.tag().openNewTag('EDIT_G:" + 197 + "','Gift: " + "BW01" + "','com.palmary.gift.js.edit','iconRole16','iconRole16','iconRole16','" + 197 + "')",
                action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('wifiLocationPromote:" + 2 + "','WifiLocation: Promote','com.palmary.wifilocation.promote_form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')\">View</button>",
                member_level = "Platinum",
                member_category = "Normal",
                pop_up_image = _fileHandler.GetImagePath("Coupon_02", ".jpg", (string)CommonConstant.Module.promote),
                earn_point = "1",
                earn_gift_name = "BW01"
            });

            return tempList.ToJson();
        }
    }
}