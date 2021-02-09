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
    public class MemberAdvanceSearchListMemberHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var search_id = prefix_id;

            var listManager = new MemberAdvanceSearchListMemberManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var dataList = listManager.GetListBySearch(search_id, startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultList = dataList.Select(
                x => new
                {
                    id = x.member_id,
                    member_no = x.member_no,
                    name = x.fullname,
                    email = x.email,

                    href = "new com.embraiz.tag().openNewTag('EDIT_M_UID:" + x.member_id.ToString() + "','Member: " + x.fullname + "','com.palmary.memberProfile.js.edit','iconRole16','iconRole16','iconRole16','" + x.member_id + "')"
                }
            ).ToList();
            return resultList.ToJson();
        }
    }
}