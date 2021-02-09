using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

using Palmary.Loyalty.BO.Modules.MemberAdvanceSearch;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.ObjectModels;

namespace Palmary.Loyalty.Web_backend.Modules.MemberAdvanceSearch
{
    public class MemberAdvanceSearchHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
           // var member_id = prefix_id;

            var memberAdvanceSearchManager = new MemberAdvanceSearchManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var dataList = memberAdvanceSearchManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultList = dataList.Select(
                x => new
                {
                    id = x.search_id,
                    action = "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().openNewTag('MemberSearch:" + x.search_id + "','MemberSearch: " + x.name + "','com.palmary.membersearch.js.list_member','iconUser_green','iconUser_green','iconUser_green', '" + x.search_id + "')\">List</button>",
                    name = x.name,
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    href = "new com.embraiz.tag().openNewTag('VIEW_d:" + x.search_id + "','Search Detail:" + x.search_id + "','com.palmary.membersearch.js.view','iconRole16','iconRole16','iconRole16', '" + x.search_id + "')"
                }
            ).ToList();
            return resultList.ToJson();
        }
    }
}