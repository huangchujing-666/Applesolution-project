using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

namespace Palmary.Loyalty.Web_backend.Modules.Product
{
    public class PasscodeRegistryHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            PasscodeManager _passcodeManager = new PasscodeManager();

            var resultList = _passcodeManager.GetPasscodeRegistryLists(SessionManager.Current.obj_id, prefix_id, startRowIndex, rowLimit, "").ToList();
            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.transaction_id,
                    passcode_pin_value = x.pin_value,
                    member_name = x.member_name,
                    point_earn = x.point_earn,
                    point_used = x.point_used,
                    status = x.status_name,
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss:fff")
                }
            );

            return resultDataList.ToJson();
        }
    }
}