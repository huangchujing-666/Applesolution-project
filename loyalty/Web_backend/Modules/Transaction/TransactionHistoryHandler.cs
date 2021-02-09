using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.DataTransferObjects.DB;

using Palmary.Loyalty.Web_backend.ObjectModels;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;

namespace Palmary.Loyalty.Web_backend.Modules.Transaction
{
    public class TransactionHistoryHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var _transactionManager = new TransactionManager();

            var resultCode = CommonConstant.SystemCode.undefine;

            var dataList = _transactionManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();

            var resultDataList = new List<ExtJsDataRow_transactionHistory>();

            foreach (var x in dataList)
            {
                var href = "";

                if (x.type == (int)CommonConstant.TransactionType.purchase_product)
                    href = "new com.embraiz.tag().openNewTag('PD_UID:" + x.transaction_id + "','Purchase Detail: " + x.transaction_id + "','com.palmary.ProductPurchaseDetail.js.index','iconRole16','iconRole16','iconRole16', '" + x.transaction_id + "')";
                else if (x.type == (int)CommonConstant.TransactionType.redemption)
                    href = "new com.embraiz.tag().openNewTag('RD_UID:" + x.transaction_id + "','Transaction Detail - Redemption: " + x.transaction_id + "','com.palmary.TransactionDetailRedemption.js.index','iconRole16','iconRole16','iconRole16', '" + x.transaction_id + "')";
                else if (x.type == (int)CommonConstant.TransactionType.point_adjustment)
                    href = "new com.embraiz.tag().open_pop_up('pavf:" + x.transaction_id + "','Point Adjustment','com.palmary.pointadjustment.js.viewform','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')";
                else if (x.type == (int)CommonConstant.TransactionType.postpaidservice)
                    href = "new com.embraiz.tag().openNewTag('PPSD:" + x.transaction_id + "','Post Paid Service Detail: " + x.transaction_id + "','com.palmary.servicepaymentdetail.js.index','iconRole16','iconRole16','iconRole16','" + x.transaction_id + "')";
                else if (x.type == (int)CommonConstant.TransactionType.point_transfer)
                    href = "new com.embraiz.tag().open_pop_up('ptvf:" + x.transaction_id + "','Point Transfer','com.palmary.pointtransfer.js.viewform','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')";
                else if (x.type == (int)CommonConstant.TransactionType.redemption_cancel)
                    href = "new com.embraiz.tag().open_pop_up('RD:" + x.source_id + "','Redemption Detail','com.palmary.giftredemption.js.detailform','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')";
                else if (x.type == (int)CommonConstant.TransactionType.promotion_rule)
                    href = "new com.embraiz.tag().openNewTag('EDIT_PR:" + x.source_id + "','Rule Detail:" + x.source_id + "','com.palmary.promotionrule.js.edit','iconRole16','iconRole16','iconRole16', '" + x.source_id + "')";

                var obj = new ExtJsDataRow_transactionHistory()
                {
                    id = x.transaction_id,
                    transaction_id = x.transaction_id,
                    type_name = x.type_name,
                    member_no = x.member_no,
                    member_name = x.member_name,
                    point_change = x.point_change > 0 ? "+" + x.point_change.ToString() : x.point_change.ToString(),
                    remark = x.remark,
                    crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    href = href,
                    href1 = "new com.embraiz.tag().openNewTag('EDIT_M_UID:" + x.member_id + "','Member: " + x.member_name + "','com.palmary.memberProfile.js.edit','iconRole16','iconRole16','iconRole16','" + x.member_id + "')"
                };

                resultDataList.Add(obj);
            }

            return resultDataList.ToJson();
        }
    }
}