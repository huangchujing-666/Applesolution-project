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

namespace Palmary.Loyalty.Web_backend.Modules.Transaction
{
    public class TransactionGroupHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {

            var list = new List<ExtJsDataRow_memberGroup> { };

            list.Add(new ExtJsDataRow_memberGroup
            {
                id = 0,
                group_name = "CTM_tran_1",
                description = "CTM special tran",

                crt_date = "2014-06-06 12:25:11.256",
                crt_by = "admin",
                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberGroup
            {
                id = 1,
                group_name = "Loyalty_tran_1",
                description = "Testing group",

                crt_date = "2014-06-04 12:15:11.256",
                crt_by = "admin",
                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberGroup
            {
                id = 2,
                group_name = "Loyalty_tran_2",
                description = "Testing group",

                crt_date = "2014-06-04 12:18:31.532",
                crt_by = "admin",
                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            return list.ToJson();
        }
    }
}