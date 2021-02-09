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
    public class TransactionFieldHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {

            var memberColumnManager = new MemberColumnManager();

            var resultCode = CommonConstant.SystemCode.undefine;
            var dataList = memberColumnManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var list = new List<ExtJsDataRow_memberField> { };

            list.Add(new ExtJsDataRow_memberField
            {
                id = 0,
                column_id = 1,
                udd_column_name = "Yes",
                udd_column_id = 0,
                datatype_name = "INT",
                datalength = "Max",
                display_name = "CTM_counter",
                remark = "CTM special counter",

                crt_date = "2014-06-06 12:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });


            list.Add(new ExtJsDataRow_memberField
            {
                id = 1,
                column_id = 1,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "INT",
                datalength = "Max",
                display_name = "Transaction ID",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberField
            {
                id = 2,
                column_id = 2,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "NVARCHAR",
                datalength = "20",
                display_name = "Member No",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberField
            {
                id = 3,
                column_id = 3,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "NVARCHAR",
                datalength = "20",
                display_name = "Member Service No",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberField
            {
                id = 4,
                column_id = 4,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "NVARCHAR",
                datalength = "20",
                display_name = "Service Plan No",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberField
            {
                id = 5,
                column_id = 5,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "FLOAT",
                datalength = "Max",
                display_name = "Amount",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberField
            {
                id = 6,
                column_id = 6,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "FLOAT",
                datalength = "Max",
                display_name = "Paid Amount",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberField
            {
                id = 7,
                column_id = 7,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "SELECT",
                datalength = "Default",
                display_name = "Payment Status",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberField
            {
                id = 8,
                column_id = 8,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "SELECT",
                datalength = "Default",
                display_name = "Payment Method",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            list.Add(new ExtJsDataRow_memberField
            {
                id = 9,
                column_id = 9,
                udd_column_name = "No",
                udd_column_id = 0,
                datatype_name = "DATETIME",
                datalength = "Default",
                display_name = "Payment Date",
                remark = "",

                crt_date = "2014-06-05 13:15:52.521",

                href = "" //"new com.embraiz.tag().open_pop_up('EDIT_M_UID:" + x.card_id.ToString() + "','Member Card: " + x.card_no + "','com.palmary.membercard.js.form','iconRole16','iconRole16','iconRole16','" + x.card_id + "')"
            });

            return list.ToJson();
        }
    }
}