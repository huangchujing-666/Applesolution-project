using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.PromotionRule;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels;

namespace Palmary.Loyalty.Web_backend.Modules.PromotionRule
{
    public class PromotionRuleHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var ruleManager = new PromotionRuleManager();

            int skipRow = startRowIndex; // unchecked((int)startRowIndex);
            int q_rowTotal = 0;
            var list = ruleManager.GetList(skipRow, rowLimit, "", ref q_rowTotal);
            var resultList = list.Select(
                x => new
                {
                    id = x.rule_id,
                    rule_id = x.rule_id,
                    name = x.name,
                    type_name = x.type_name,
                    start_date = x.start_date == null ? "Forever" : x.start_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    end_date = x.end_date == null ? "Forever" : x.end_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    href = "new com.embraiz.tag().openNewTag('EDIT_PR:" + x.rule_id + "','Rule Detail:" + x.name + "','com.palmary.promotionrule.js.edit','iconRole16','iconRole16','iconRole16', '" + x.rule_id + "')",
                }
            );

            return resultList.ToJson();
        }
    }
}