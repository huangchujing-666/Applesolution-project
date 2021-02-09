using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

namespace Palmary.Loyalty.Web_backend.Modules.Service
{
    public class ServicePaymentDetailExtraHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var transaction_id = prefix_id;
            var servicePaymentManager = new ServicePaymentManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = servicePaymentManager.GetList_transaction_extra(transaction_id, startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.earn_id,

                    rule_id = x.rule_id,
                    member_no = x.member_no,
                    rule_name = x.rule_name,
                    
                    point_earn = x.point_earn,
                    point_status_name = x.point_status_name,
                    point_expiry_date = x.point_expiry_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    href = "" //"new com.embraiz.tag().openNewTag('EDIT_S:" + x.payment_id + "','Service: " + x.payment_id + "','com.palmary.service.js.edit','iconRole16','iconRole16','iconRole16','" + x.payment_id + "')"
                }
            );

            rowTotal = resultDataList.Count();
            return resultDataList.ToJson();
        }
    }
}