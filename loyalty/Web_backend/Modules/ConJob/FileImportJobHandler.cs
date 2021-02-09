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

namespace Palmary.Loyalty.Web_backend.Modules.ConJob
{
    public class FileImportJobHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var list = new List<ExtJsDataRow_fileimportjob> { };

            list.Add(new ExtJsDataRow_fileimportjob
            {
                id = 0,
                job_id = 0,
                name = "Transaction File From CTM Other System",
                type = "Transaction",
                schedule_date = "Every Day 00:00:00",
                last_executed_date = "NA",
                crt_date = "2014-06-05 14:25:20.356",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

           
            list.Add(new ExtJsDataRow_fileimportjob
            {
                id = 1,
                job_id = 1,
                name = "Transaction File From Billing System",
                type = "Transaction",
                schedule_date = "Every Day 00:00:00",
                last_executed_date = "2014-06-05 00:00:00.000",
                crt_date = "2014-06-01 12:25:20.356",
                
                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });


            list.Add(new ExtJsDataRow_fileimportjob
            {
                id = 2,
                job_id = 2,
                name = "New Customer From Billing System",
                type = "Member",
                schedule_date = "Every Day 00:00:00",
                last_executed_date = "2014-06-05 00:00:00.000",
                crt_date = "2014-06-01 12:25:20.356",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });
            

            return list.ToJson();
        }
    }
}