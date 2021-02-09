using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Palmary.Loyalty.BO.DataTransferObjects.DB;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.Common;

namespace Palmary.Loyalty.Web_backend.Modules.Member
{
    public class MemberLevelChangeHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var memberLevelChangeManager = new MemberLevelChangeManager();
            var systemCode = CommonConstant.SystemCode.undefine;
            var resultList = memberLevelChangeManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref systemCode, ref rowTotal).ToList();
            
            var resultDataList = resultList.Select(
               x => new
               {
                   id = x.change_id,
                   change_id = x.change_id,
                   member_no = x.member_no,
                   source_type_name = x.source_type_name,
                   change_type_name = x.change_type_name,
                   reason_type_name = x.reason_type_name,

                   old_member_level_name = x.old_member_level_name,
                   new_member_level_name = x.new_member_level_name,
                   remark = x.remark,
                   crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                
                   href = "" //"new com.embraiz.tag().openNewTag('EDIT_RTID:" + x.reminder_template_id.ToString() + "','Reminder Template: " + x.name + "','com.palmary.remindertemplate.js.edit','iconRole16','iconRole16','iconRole16', '" + x.reminder_template_id + "')"
               }
           );

            return resultDataList.ToJson();
        }
    }
}