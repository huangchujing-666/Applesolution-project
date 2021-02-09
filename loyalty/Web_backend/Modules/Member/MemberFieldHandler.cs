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
    public class MemberFieldHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            
            var memberColumnManager = new MemberColumnManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var dataList = memberColumnManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var list = new List<ExtJsDataRow_memberField> { };

            foreach (var x in dataList)
            {
                var the_length = "";
                if (x.datalength == -1)
                    the_length = "Max";
                else if (x.datalength == 0)
                    the_length = "Default";
                else
                    the_length = x.datalength.ToString();

                list.Add(new ExtJsDataRow_memberField
                {
                    id = x.column_id,
                    column_id = x.column_id,
                    udd_column_name = x.udd_column_name,
                    udd_column_id = x.udd_column_id,
                    datatype_name = x.datatype_name,
                    datalength = the_length,
                    display_name = x.display_name,
                    remark = x.remark,

                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),

                    href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
                });

            }

            return list.ToJson();
        }
    }
}