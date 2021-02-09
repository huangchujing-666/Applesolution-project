using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Common;

using Palmary.Loyalty.BO.Modules.Gift;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels;

using Palmary.Loyalty.BO.DataTransferObjects.DB;

namespace Palmary.Loyalty.Web_backend.Modules.Transaction
{
    public class TransactionDetailRedemptionHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            var transaction_id = prefix_id;
            GiftRedemptionManager _giftRedemptionManager = new GiftRedemptionManager();
            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = _giftRedemptionManager.GetListByTransaction(transaction_id, startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                            x => new
                            {
                                id = x.redemption_id,
                                redemption_id = x.redemption_id,
                                redemption_code = x.redemption_code,
                                member_id = x.member_id,
                                gift_no = x.gift_no,
                                gift_id = x.gift_id,
                                gift_name = x.gift_name,
                                quantity = x.quantity,
                                point_used = (decimal)x.point_used,

                                redemption_status = x.redemption_status_name,
                                redemption_status_name = x.redemption_status_name,
                                collect_date = x.collect_date == null ? "NA" : x.collect_date.Value.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                location = x.location_name,
                                void_date = x.void_date == null ? "NA" : x.void_date.Value.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                void_reason = x.remark,
                                void_staff = x.void_user_id,
                                status = x.status,
                                crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                crt_by_type = x.crt_by_type,
                                crt_by = x.crt_by,
                                upd_date = x.upd_date,
                                upd_by_type = x.upd_by_type,
                                upd_by = x.upd_by,
                                record_status = x.record_status,
                                href = "new com.embraiz.tag().open_pop_up('RD:" + x.redemption_id + "','Redemption Detail','com.palmary.giftredemption.js.detailform','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')",
                                href1 = "new com.embraiz.tag().openNewTag('EDIT_G:" + x.gift_id + "','Gift: " + x.gift_name + "','com.palmary.gift.js.edit','iconRole16','iconRole16','iconRole16','" + x.gift_id + "')",
                                action = x.redemption_status == (int)CommonConstant.GiftRedeemStatus.waiting_collect ? "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('giftCollect:" + x.redemption_id + "','GiftRedemption: Collect','com.palmary.giftredemption.collect.js.form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')\">Collect</button> <button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('giftCollect:" + x.redemption_id + "','GiftRedemption: Cancel','com.palmary.giftredemption.cancel.js.form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')\">Cancel</button>" : ""
                            }
                        );

            return resultDataList.ToJson();
        }
    }
}