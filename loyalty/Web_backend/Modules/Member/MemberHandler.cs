using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;

using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Member
{
    public class MemberHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            MemberManager _memberManager = new MemberManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = _memberManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            
            var list = new List<ExtJsDataRowMember> { };
            int insertCounter = 0;

            foreach (var item in resultList)
            {
                //System.Diagnostics.Debug.WriteLine(item.email, "item.email");
                //var combinedName = item.surname + " " + item.givenname + ", " + item.name;
                //if (combinedName.Trim() == ",") combinedName = "[empty]";

                list.Insert(insertCounter, new ExtJsDataRowMember
                {
                    id = item.member_id,
                    member_id = item.member_id,
                    member_no = (item.member_no == "" || item.member_no == null) ? "[empty]" : item.member_no,
                    email = (item.email == "" || item.email == null) ? "[empty]" : item.email,
                    mobile_no = (item.mobile_no == "" || item.mobile_no == null) ? "[empty]" : item.mobile_no,
                    hkid = (item.hkid == "" || item.hkid == null) ? "[empty]" : item.hkid,
                    name = item.fullname,
                    available_point = item.available_point.ToString(),
                    member_level_name = item.member_level_name,
                    href = "new com.embraiz.tag().openNewTag('EDIT_M_UID:" + item.member_id.ToString() + "','Member: " + item.fullname + "','com.palmary.memberProfile.js.edit','iconRole16','iconRole16','iconRole16','" + item.member_id + "')"
                });
                insertCounter++;
            }

          
            return list.ToJson();
        }
    }
}