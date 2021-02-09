using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

namespace Palmary.Loyalty.Web_backend.Modules.Product
{
    public class ProductHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            ProductManager _productManager = new ProductManager();
            FileHandler _fileHandler = new FileHandler();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = _productManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    product_no = x.product_no,
                    name = x.name,
                    id = x.product_id,
                    category = "",
                    
                    photo = _fileHandler.GetImagePath(x.file_name, x.file_extension, (string)CommonConstant.Module.product, (int)CommonConstant.ImageSizeType.thumb),
                    status_name = x.status_name,
                    href = "new com.embraiz.tag().openNewTag('EDIT_P:" + x.product_id + "','Product: " + x.name + "','com.palmary.product.js.edit','iconRole16','iconRole16','iconRole16','" + x.product_id + "')"
                }
            );

           
            return resultDataList.ToJson();
        }
    }
}