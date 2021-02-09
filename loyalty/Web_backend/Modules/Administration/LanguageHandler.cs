using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.BO.Modules.Administration.Section;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Administration
{
    public class LanguageHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            LanguageManager languageManager = new LanguageManager();
            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = languageManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal);

            var resultDataList = resultList.Select(
                x => new
                {
                    id = x.lang_id,
                    lang_id = x.lang_id,
                    name = x.name,
                    code = x.code
                   
                   // href = "new com.embraiz.tag().openNewTag('EDIT_UID:" + x.user_id.ToString() + "','User: " + x.name + "','com.palmary.user.js.edit','iconRole16','iconRole16','iconRole16', '" + x.user_id + "')"
                }
            );

            return resultDataList.ToJson();
        }

    }
}