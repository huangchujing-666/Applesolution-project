using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_backend.Modules.Passcode
{
    public class PasscodePrefixHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var passcodePrefixManager = new PasscodePrefixManager();
            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = passcodePrefixManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal);
            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.prefix_id,
                    prefix_id = x.prefix_id,
                    prefix_value = x.prefix_value,
                    passcode_format = x.passcode_format,
                    product_name = x.product_name,
                    current_generated = x.current_generated,
                    maximum_generate = x.maximum_generate,
                    usage_precent = x.usage_precent.ToString() + "%",
                    action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('passcodeGenerate:" + x.prefix_id + "','Passcode:Generate','com.palmary.passcodegenerate.js.form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','com.palmary.passcodegenerate.js.index')\">Generate</button>"
                        + " <button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().openNewTag('passcodeList:" + x.prefix_id + "','Passcode List:" + x.prefix_value + "','com.palmary.passcode.js.list','iconRole16','iconRole16','iconRole16','" + x.prefix_id + "')\">Passcode List</button>",

                    href = ""
                }
            );

            return resultDataList.ToJson();
        }
    }
}