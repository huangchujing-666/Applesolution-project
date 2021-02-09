using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Passcode;

namespace Palmary.Loyalty.Web_backend.Modules.Product
{
    public class PasscodeHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var _passcodeManager = new PasscodeManager();
            var resultCode = CommonConstant.SystemCode.undefine;
            List<PasscodeObject> resultList;
            if (prefix_id > 0)
                resultList = _passcodeManager.GetListByPasscodePrefix(prefix_id, startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            else
                resultList = _passcodeManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var list = resultList.Select(x => new
                {
                    id = x.passcode_id,
                    passcode_prefix_id = x.passcode_prefix_id,
                    passcode_id = x.passcode_id,
                    pin_value = x.pin_value,
                    active_date_str = x.active_date.ToString("yyyy-MM-dd"),
                    expiry_date_str = x.expiry_date.ToString("yyyy-MM-dd"),
                    point = x.point,
                    product_name = x.product_name,
                    registered_name = x.registered_name,

                    member_no = x.member_no ?? "",
                    href = "new com.embraiz.tag().openNewTag()",
                    href1 = (x.member_no == null) ? "" : "new com.embraiz.tag().openNewTag('EDIT_M_UID:" + x.member_id.ToString() + "','Member: " + x.member_no + "','com.palmary.memberProfile.js.edit','iconRole16','iconRole16','iconRole16','" + x.member_id + "')"
                }
            ); 

            return list.ToJson();
        }
    }
}