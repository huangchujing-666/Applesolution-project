using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Gift
{
    public class GiftCategoryHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            GiftCategoryManager _giftCategoryManager = new GiftCategoryManager();
            FileHandler _fileHandler = new FileHandler();

            var resultList = _giftCategoryManager.GetGiftCategoryLists(SessionManager.Current.obj_id, startRowIndex, rowLimit, "").ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    category_id = x.category_id,

                    display_order = x.display_order,
                    status = x.status,
                    name = x.name,
                    photo = _fileHandler.GetImagePath(x.photo_file_name, x.photo_file_extension, (string)CommonConstant.Module.giftCategory, (int)CommonConstant.ImageSizeType.thumb),
                    href = "new com.embraiz.tag().openNewTag('EDIT_GC:" + x.category_id + "','GiftCategory: " + x.name + "','com.palmary.giftcategory.js.edit','iconRole16','iconRole16','iconRole16','" + x.category_id + "')"
                }
            );

            return resultDataList.ToJson();
        }
    }
}