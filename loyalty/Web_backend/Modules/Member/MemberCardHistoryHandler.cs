using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Member;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.ObjectModels;

namespace Palmary.Loyalty.Web_backend.Modules.Member
{
    public class MemberCardHistoryHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var memberCardManager = new MemberCardManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var dataList = memberCardManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultList = dataList.Select(
                x => new
                {
                    id = x.card_id,
                    card_id = x.card_id,
                    member_no = x.member_no,
                    card_no = x.card_no,
                    card_type_name = x.card_type_name,
                    card_status_name = x.card_status_name,
                    issue_date = x.issue_date == null ? "" : x.issue_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    remark = x.remark,

                    href = "new com.embraiz.tag().openNewTag('EDIT_M_UID:" + x.member_id.ToString() + "','Member: " + x.member_no + "','com.palmary.membercard.js.edit','iconRole16','iconRole16','iconRole16','" + x.member_id + "')"
                }
            ).ToList();

            return resultList.ToJson();
        }
    }
}