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
    public class ServicePlanHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var serviceManager = new ServicePlanManager();
     
            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = serviceManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.plan_id,
                    plan_id = x.plan_id,
                    plan_no = x.plan_no,
                    type_name = x.type_name,
                    name = x.name,
                    status_name = x.status_name,

                    href = "new com.embraiz.tag().openNewTag('EDIT_S:" + x.plan_id + "','Service Plan: " + x.name + "','com.palmary.serviceplan.js.edit','iconRole16','iconRole16','iconRole16','" + x.plan_id + "')"
                }
            );

            rowTotal = 0; // resultList[0].rowTotal;
            return resultDataList.ToJson();
        }
    }
}