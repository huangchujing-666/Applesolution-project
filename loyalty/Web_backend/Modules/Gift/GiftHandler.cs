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
    public class GiftHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            GiftManager _giftManager = new GiftManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = _giftManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var list = new List<ExtJsDataRowGift> { };
   
            FileHandler _fileHandler = new FileHandler();

            foreach (var item in resultList)
            {
                rowTotal = 0; // item.rowTotal ?? 0;

                list.Add( new ExtJsDataRowGift
                {
                    id = item.gift_id,
                    
                    gift_id = item.gift_id,
                    gift_no = item.gift_no,
                    redeem_active_date = item.redeem_active_date.ToString("yyyy-MM-dd HH:mm:ss"),
                    redeem_expiry_date = item.redeem_expiry_date.ToString("yyyy-MM-dd HH:mm:ss"),
                    point = item.point,
                    name = item.name,
                    available_stock = item.available_stock,
                    alert_level = item.alert_level,
                    photo = _fileHandler.GetImagePath(item.file_name, item.file_extension, (string)CommonConstant.Module.gift, (int)CommonConstant.ImageSizeType.thumb),
                    status_name = item.status_name,
                    href = "new com.embraiz.tag().openNewTag('EDIT_G:" + item.gift_id + "','Gift: " + item.name + "','com.palmary.gift.js.edit','iconRole16','iconRole16','iconRole16','" + item.gift_id + "')"
                });
            }

            return list.ToJson();
        }
    }
}