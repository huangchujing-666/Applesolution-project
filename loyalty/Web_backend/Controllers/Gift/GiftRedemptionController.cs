using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.Modules.Transaction;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.TableData;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Common.Languages;
using System.Web.Routing;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.Web_backend.Controllers.Gift
{
    [Authorize]
    public class GiftRedemptionController : Controller
    {
        private GiftRedemptionManager _giftRedemptionManager;
        private GiftManager _giftManager;
        
        private PasscodeManager _passcodeManager;
        private TransactionEarnManager _transactionEarnManager;
        private TransactionUseManager _transactionUseManager;

        private int _id;

        public GiftRedemptionController()
        {
            _giftRedemptionManager = new GiftRedemptionManager();
            _passcodeManager = new PasscodeManager();
            _giftManager = new GiftManager();
            _transactionEarnManager = new TransactionEarnManager();
            _transactionUseManager = new TransactionUseManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        // Create new
        public string Insert()
        {
            var giftRedemption = new GiftRedemptionObject();

            giftRedemption.member_id = _id;

            var formTableJSON = TableFormHandler.GetFormByModule(giftRedemption);
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var redemption_id = collection.GetFormValue(PayloadKeys.GiftRedemption.redemption_id);
            var member_id = collection.GetFormValue(PayloadKeys.GiftRedemption.member_id);
            
            var redeemList = new List<GiftRedemptionObject>();

            var gift_id = collection.GetFormValue(PayloadKeys.GiftRedemption.gift_id);
            var quantity = collection.GetFormValue(PayloadKeys.GiftRedemption.quantity);
            var location_id = collection.GetFormValue(PayloadKeys.GiftRedemption.location_id);
            if (gift_id > 0 && quantity > 0)
            {
                var giftRedemption = new GiftRedemptionObject()
                {
                    redemption_id = redemption_id,

                    redemption_channel = (int)CommonConstant.GiftRedeemChannel.cms,
                    member_id = member_id,
                    gift_id = gift_id,
                    quantity = quantity,

                    redemption_status = (int)CommonConstant.GiftRedeemStatus.waiting_collect,
                    collect_date = null,
                    collect_location_id = location_id,
                    void_date = null,
                    void_user_id = null,
                    remark = null,
                    status = (int)CommonConstant.Status.active
                };
                redeemList.Add(giftRedemption);
            }

            var gift_id_2 = collection.GetFormValue(PayloadKeys.GiftRedemption.gift_id_2);
            var quantity_2 = collection.GetFormValue(PayloadKeys.GiftRedemption.quantity_2);
            var location_id_2 = collection.GetFormValue(PayloadKeys.GiftRedemption.location_id_2);
            if (gift_id_2 > 0 && quantity_2 > 0)
            {
                var giftRedemption = new GiftRedemptionObject()
                {
                    redemption_id = redemption_id,

                    redemption_channel = (int)CommonConstant.GiftRedeemChannel.cms,
                    member_id = member_id,
                    gift_id = gift_id_2,
                    quantity = quantity_2,

                    redemption_status = (int)CommonConstant.GiftRedeemStatus.waiting_collect,
                    collect_date = null,
                    collect_location_id = location_id_2,
                    void_date = null,
                    void_user_id = null,
                    remark = null,
                    status = (int)CommonConstant.Status.active
                };
                redeemList.Add(giftRedemption);
            }

            var gift_id_3 = collection.GetFormValue(PayloadKeys.GiftRedemption.gift_id_3);
            var quantity_3 = collection.GetFormValue(PayloadKeys.GiftRedemption.quantity_3);
            var location_id_3 = collection.GetFormValue(PayloadKeys.GiftRedemption.location_id_3);
            if (gift_id_3 > 0 && quantity_3 > 0)
            {
                var giftRedemption = new GiftRedemptionObject()
                {
                    redemption_id = redemption_id,

                    redemption_channel = (int)CommonConstant.GiftRedeemChannel.cms,
                    member_id = member_id,
                    gift_id = gift_id_3,
                    quantity = quantity_3,

                    redemption_status = (int)CommonConstant.GiftRedeemStatus.waiting_collect,
                    collect_date = null,
                    collect_location_id = location_id_3,
                    void_date = null,
                    void_user_id = null,
                    remark = null,
                    status = (int)CommonConstant.Status.active
                };
                redeemList.Add(giftRedemption);
            }

            var gift_id_4 = collection.GetFormValue(PayloadKeys.GiftRedemption.gift_id_4);
            var quantity_4 = collection.GetFormValue(PayloadKeys.GiftRedemption.quantity_4);
            var location_id_4 = collection.GetFormValue(PayloadKeys.GiftRedemption.location_id_4);
            if (gift_id_4 > 0 && quantity_4 > 0)
            {
                var giftRedemption = new GiftRedemptionObject()
                {
                    redemption_id = redemption_id,

                    redemption_channel = (int)CommonConstant.GiftRedeemChannel.cms,
                    member_id = member_id,
                    gift_id = gift_id_4,
                    quantity = quantity_4,

                    redemption_status = (int)CommonConstant.GiftRedeemStatus.waiting_collect,
                    collect_date = null,
                    collect_location_id = location_id_4,
                    void_date = null,
                    void_user_id = null,
                    remark = null,
                    status = (int)CommonConstant.Status.active
                };
                redeemList.Add(giftRedemption);
            }

            var gift_id_5 = collection.GetFormValue(PayloadKeys.GiftRedemption.gift_id_5);
            var quantity_5 = collection.GetFormValue(PayloadKeys.GiftRedemption.quantity_5);
            var location_id_5 = collection.GetFormValue(PayloadKeys.GiftRedemption.location_id_5);
            if (gift_id_5 > 0 && quantity_5 > 0)
            {
                var giftRedemption = new GiftRedemptionObject()
                {
                    redemption_id = redemption_id,

                    redemption_channel = (int)CommonConstant.GiftRedeemChannel.cms,
                    member_id = member_id,
                    gift_id = gift_id_5,
                    quantity = quantity_5,

                    redemption_status = (int)CommonConstant.GiftRedeemStatus.waiting_collect,
                    collect_date = null,
                    collect_location_id = location_id_5,
                    void_date = null,
                    void_user_id = null,
                    remark = null,
                    status = (int)CommonConstant.Status.active
                };
                redeemList.Add(giftRedemption);
            }

            var sql_remark = "";
            if (redemption_id == 0)
            {
                var giftRedemptionManager = new GiftRedemptionManager();
                var message = "";
                var redeemFlag = giftRedemptionManager.Redeem(redeemList, ref message);

                return redeemFlag ? "{success:true,url:'',msg:'Gift Redemption Complete'}" : "{success:false,url:'',msg:'Redeem Failed:<br/>" + message + "'}";
            }
            else if (redemption_id > 0)
            {
                var updateFlag = false;

                return updateFlag ? "{success:true,url:'',msg:'Saved Success'}" : "{success:false,url:'',msg:'Redeem Failed: " + sql_remark + "'}";
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }

        public string Collect()
        {
            var redemption_id = _id; //should be redemption_id
            var sql_result = false;
            var resultCode = CommonConstant.SystemCode.undefine;
            var redeem_record = _giftRedemptionManager.GetDetail(redemption_id, ref resultCode);

            var memberManager = new MemberManager();
            
            var member = memberManager.GetDetail(redeem_record.member_id, false, ref resultCode);

            var giftManager = new GiftLangManager();
            var gift_lang = giftManager.GetGiftLangDetailBy(SessionManager.Current.obj_id, redeem_record.gift_id, ref sql_result);
            var gift_name = "";
            foreach (var lang in gift_lang)
            {
                if (lang.lang_id == 2)
                {
                    gift_name = lang.name;
                    break;
                }
            }

            // Fields
            List<ExtJSField> fieldList = new List<ExtJSField>();
            fieldList.Add(new ExtJSField
            {
                name = "redemption_id",
                fieldLabel = "Redemption ID",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = redemption_id.ToString(),
                display_value = redemption_id.ToString(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "redemption_code",
                fieldLabel = "Redemption Code",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = redeem_record.redemption_code,
                display_value = redeem_record.redemption_code,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "member_code",
                fieldLabel = "Member Code",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = member.member_no,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "member_name",
                fieldLabel = "Member Name",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = member.GetFullname(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "gift_name",
                fieldLabel = "Gift Name",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = gift_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "quantity",
                fieldLabel = "Quantity",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.quantity.ToString(),
                readOnly = true
            });

            // Hidden Fields
            List<ExtJSField_hidden> hiddenList = new List<ExtJSField_hidden>();
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "redemption_id",
                value = redemption_id.ToString()
            });
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "status",
                value = ((int)CommonConstant.GiftRedeemStatus.collected).ToString()
            });

            var formTableJSON = new
            {

                row = fieldList,
                rowhidden = hiddenList,

                column = 2,
                post_url = "modules/owner/list_data.jsp",  //<-
                post_header = "modules/owner/grid_header.jsp", //<-
                title = "Collect Form",
                icon = "iconRemarkList",
                post_params = Url.Action("CollectOrCancel_perform"),

                button_text = "Confirm Collect",
                button_icon = "iconSave",
                value_changes = true
            }.ToJson();

            return formTableJSON;
        }

        public string DetailForm()
        {
            var redemption_id = _id; 
            var sql_result = false;
            var resultCode = CommonConstant.SystemCode.undefine;
            var redeem_record = _giftRedemptionManager.GetDetail(redemption_id, ref resultCode);

            var memberManager = new MemberManager();

            var member = memberManager.GetDetail(redeem_record.member_id, false, ref resultCode);

            var giftManager = new GiftLangManager();
            var gift_lang = giftManager.GetGiftLangDetailBy(SessionManager.Current.obj_id, redeem_record.gift_id, ref sql_result);
            var gift_name = "";
            foreach (var lang in gift_lang)
            {
                if (lang.lang_id == 2)
                {
                    gift_name = lang.name;
                    break;
                }
            }

            // Fields
            List<ExtJSField> fieldList = new List<ExtJSField>();
            fieldList.Add(new ExtJSField
            {
                name = "redemption_id",
                fieldLabel = "Redemption ID",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = redemption_id.ToString(),
                display_value = redemption_id.ToString(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "redemption_code",
                fieldLabel = "Redemption Code",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = redeem_record.redemption_code,
                display_value = redeem_record.redemption_code,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "member_code",
                fieldLabel = "Member Code",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = member.member_no,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "member_name",
                fieldLabel = "Member Name",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = member.GetFullname(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "gift_name",
                fieldLabel = "Gift Name",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = gift_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "quantity",
                fieldLabel = "Quantity",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.quantity.ToString(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "point_used",
                fieldLabel = "Point Used",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.point_used.ToString(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "location",
                fieldLabel = "Location",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.location_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "redemption_status_name",
                fieldLabel = "Redemption Status",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.redemption_status_name,
                readOnly = true
            });


            fieldList.Add(new ExtJSField
            {
                name = "crt_date",
                fieldLabel = "Redeem Date",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.crt_date.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "collect_date",
                fieldLabel = "Collect Date",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.collect_date == null ? "NA" : redeem_record.collect_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "void_date",
                fieldLabel = "Void Date",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.void_date == null ? "NA" : redeem_record.void_date.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "remark",
                fieldLabel = "Remark",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.remark,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "crt_by_name",
                fieldLabel = "Create By",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.crt_by_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "upd_by_name",
                fieldLabel = "Update By",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.upd_by_name,
                readOnly = true
            });


            // Hidden Fields
            List<ExtJSField_hidden> hiddenList = new List<ExtJSField_hidden>();
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "redemption_id",
                value = redemption_id.ToString()
            });
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "status",
                value = ((int)CommonConstant.GiftRedeemStatus.collected).ToString()
            });

            var formTableJSON = new
            {

                row = fieldList,
                rowhidden = hiddenList,

                column = 2,
                //post_url = "modules/owner/list_data.jsp",  //<-
                //post_header = "modules/owner/grid_header.jsp", //<-
                title = "Detail",
                icon = "iconRemarkList",
                post_params = Url.Action("CollectOrCancel_perform"),

                //button_text = "Confirm Collect",
                //button_icon = "iconSave",
                value_changes = true
            }.ToJson();

            return formTableJSON;
        }

        public string Cancel()
        {
            var redemption_id = _id; //should be redemption_id
            var sql_result = false;
            var resultCode = CommonConstant.SystemCode.undefine;
            var redeem_record = _giftRedemptionManager.GetDetail(redemption_id, ref resultCode);

            var memberManager = new MemberManager();
           
            var member = memberManager.GetDetail(redeem_record.member_id, false, ref resultCode);

            var giftManager = new GiftLangManager();
            var gift_lang = giftManager.GetGiftLangDetailBy(SessionManager.Current.obj_id, redeem_record.gift_id, ref sql_result);
            var gift_name = "";
            foreach (var lang in gift_lang)
            {
                if (lang.lang_id == 2)
                {
                    gift_name = lang.name;
                    break;
                }
            }

            // Fields
            List<ExtJSField> fieldList = new List<ExtJSField>();
            fieldList.Add(new ExtJSField
            {
                name = "redemption_id",
                fieldLabel = "Redemption ID",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = redemption_id.ToString(),
                display_value = redemption_id.ToString(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "redemption_code",
                fieldLabel = "Redemption Code",
                type = "input",
                colspan = 2,
                tabIndex = "1",
                value = redeem_record.redemption_code,
                display_value = redeem_record.redemption_code,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "member_code",
                fieldLabel = "Member Code",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = member.member_no,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "member_name",
                fieldLabel = "Member Name",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = member.GetFullname(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "gift_name",
                fieldLabel = "Gift Name",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = gift_name,
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "quantity",
                fieldLabel = "Quantity",
                type = "date",
                colspan = 2,
                tabIndex = "2",
                value = redeem_record.quantity.ToString(),
                readOnly = true
            });

            fieldList.Add(new ExtJSField
            {
                name = "void_reason",
                fieldLabel = "Void Reason",
                type = "input",
                colspan = 2,
                tabIndex = "2",
                value = ""
               
            });

            // Hidden Fields
            List<ExtJSField_hidden> hiddenList = new List<ExtJSField_hidden>();
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "redemption_id",
                value = redemption_id.ToString()
            });
            hiddenList.Add(new ExtJSField_hidden
            {
                type = "hidden",
                name = "status",
                value = ((int)CommonConstant.GiftRedeemStatus.voided).ToString()
            });

            var formTableJSON = new
            {
                row = fieldList,
                rowhidden = hiddenList,

                column = 2,
                post_url = "",  //<-
                post_header = "", //<-
                title = "Collect Form",
                icon = "iconRemarkList",
                post_params = Url.Action("CollectOrCancel_perform"),

                button_text = "Cancel Redemption",
                button_icon = "iconSave",
                value_changes = true
            }.ToJson();

            return formTableJSON;
        }

        public string CollectOrCancel_perform(FormCollection collection)
        { 
            var redemption_id = Convert.ToInt32(collection["redemption_id"].Trim());
            var redemption_status = Convert.ToInt32(collection["status"].Trim());
            
            var resultCode = CommonConstant.SystemCode.undefine;

            var msg_prefix = "";
            if (redemption_status == (int)CommonConstant.GiftRedeemStatus.collected)
            {
                resultCode = _giftRedemptionManager.RedemptionCollect(redemption_id);
                msg_prefix = "Collect";
            }
            else if (redemption_status == (int)CommonConstant.GiftRedeemStatus.voided)
            {
                var void_reason = collection["void_reason"].Trim();
                resultCode = _giftRedemptionManager.RedemptionCancel(redemption_id, void_reason);
                msg_prefix = "Cancel";
            }
            
            var result = "";
            if (resultCode == CommonConstant.SystemCode.normal)
                result = new { success = true, msg = msg_prefix + " Complete" }.ToJson();
            else
                result = new { success = false, msg = msg_prefix + " Fail" }.ToJson();

            return result;
        }
    }
}

