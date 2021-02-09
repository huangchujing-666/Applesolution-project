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
    public class ServiceContractHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var serviceContractManager = new ServiceContractManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = serviceContractManager.GetList(prefix_id, startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.contract_id,
                    contract_id = x.contract_id,
                    contract_no = x.contract_no,
                    service_name = x.service_name,
                    fee = x.fee,
                    start_date = x.start_date.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                    end_date = x.end_date.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                
                    href = "new com.embraiz.tag().openNewTag('EDIT_S:" + x.service_id + "','Service: " + x.service_name + "','com.palmary.service.js.edit','iconRole16','iconRole16','iconRole16','" + x.service_id + "')"
                }
            );

            rowTotal = 0; // resultList[0].rowTotal;
            return resultDataList.ToJson();
        }
    }
}