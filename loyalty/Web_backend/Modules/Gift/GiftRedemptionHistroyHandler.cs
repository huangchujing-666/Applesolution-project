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

namespace Palmary.Loyalty.Web_backend.Modules.Gift
{
    public class GiftRedemptionHistroyHandler : IHandler
    {
        public string LoadListDataToExtJSJson(int prefix_id, int startRowIndex, int rowLimit, List<SearchParmObject> searchParmList, string sortColumn, CommonConstant.SortOrder sortOrder, ref int rowTotal)
        {
            GiftRedemptionManager _giftRedemptionManager = new GiftRedemptionManager();
            var resultCode = CommonConstant.SystemCode.undefine;
            var resultList = _giftRedemptionManager.GetList(startRowIndex, rowLimit, searchParmList, sortColumn, sortOrder, ref resultCode, ref rowTotal).ToList();
            var resultDataList = resultList.Select(
                            x => new
                            {
                                id = x.redemption_id,
                                redemption_code = x.redemption_code,
                                member_no = x.member_no,
                                member_name = x.member_name,                             
                            
                                gift_no = x.gift_no,
                                gift_name = x.gift_name,
                                quantity = x.quantity,
                                point_used = (decimal)x.point_used,
                                location = x.location_name,
                                redemption_status = x.redemption_status_name,
                                redemption_status_name = x.redemption_status_name,
                                collect_date = x.collect_date == null ? "NA" : x.collect_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), 
                                crt_date = x.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),

                                href = "new com.embraiz.tag().open_pop_up('RD:" + x.redemption_id + "','Redemption Detail','com.palmary.giftredemption.js.detailform','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')",
                                href1 = "new com.embraiz.tag().openNewTag('EDIT_M_UID:" + x.member_id + "','Member: " + x.member_name + "','com.palmary.memberProfile.js.edit','iconRole16','iconRole16','iconRole16','" + x.member_id + "')",
                                href2 = "new com.embraiz.tag().openNewTag('EDIT_G:" + x.gift_id + "','Gift: " + x.gift_name + "','com.palmary.gift.js.edit','iconRole16','iconRole16','iconRole16','" + x.gift_id + "')",
                                action = x.redemption_status == (int)CommonConstant.GiftRedeemStatus.waiting_collect ? "<button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('giftCollect:" + x.redemption_id + "','GiftRedemption: Collect','com.palmary.giftredemption.collect.js.form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')\">Collect</button> <button type='button' style=\"display:\" onclick=\"new com.embraiz.tag().open_pop_up('giftCollect:" + x.redemption_id + "','GiftRedemption: Cancel','com.palmary.giftredemption.cancel.js.form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')\">Cancel</button>" : ""
                            }
                        );

            return resultDataList.ToJson();
        }
    }
}