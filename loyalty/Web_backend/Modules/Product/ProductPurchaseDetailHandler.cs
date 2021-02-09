using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;

namespace Palmary.Loyalty.Web_backend.Modules.Product
{
    public class ProductPurchaseDetailHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var transaction_id = prefix_id;
            var productPurchaseManager = new ProductPurchaseManager();
            var resultCode = CommonConstant.SystemCode.undefine;

            var resultList = productPurchaseManager.GetListByTransaction(transaction_id, startRowIndex, rowLimit, "", ref resultCode, ref rowTotal);
            var resultDataList = resultList.Select(
            x => new
            {
                id = x.purchase_id,
                product_no = x.product_no,
                product_name = x.product_name,
                quantity = x.quantity,
                point_earned = (decimal)x.point_earned,
                point_status_name = x.point_status_name,
                point_expiry_date = x.point_expiry_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),

                href = "new com.embraiz.tag().openNewTag('EDIT_P:" + x.product_id + "','Product: " + x.product_name + "','com.palmary.product.js.edit','iconRole16','iconRole16','iconRole16','" + x.product_id + "')"

            }
            );

            return resultDataList.ToJson();
        }
    }
}