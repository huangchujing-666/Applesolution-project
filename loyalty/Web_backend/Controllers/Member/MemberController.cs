using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private MemberManager _memberManager;
        private int _member_id;

        public MemberController()
        {
            _memberManager = new MemberManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _member_id = int.Parse(id.ToString());
            }

            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string GetModule()
        {
            var resultCode = CommonConstant.SystemCode.undefine;
            var member = _memberManager.GetDetail(_member_id, false, ref resultCode);
            var formTableJSON = TableFormHandler.GetFormByModule(member);

            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var member_id = collection.GetFormValue(PayloadKeys.Id);
            var member_no = collection.GetFormValue(PayloadKeys.Member_no);
            
            var password = collection.GetFormValue(PayloadKeys.Password);
            var email = collection.GetFormValue(PayloadKeys.Email);
            var fbid = collection.GetFormValue(PayloadKeys.fbid);
            var fbemail = collection.GetFormValue(PayloadKeys.fbemail);
            var mobile_no = collection.GetFormValue(PayloadKeys.Mobile_no);
            var salutation = collection.GetFormValue(PayloadKeys.Salutation);
            var firstname = collection.GetFormValue(PayloadKeys.firstname);
            var middlename = collection.GetFormValue(PayloadKeys.middlename);
            var lastname = collection.GetFormValue(PayloadKeys.lastname);

            var birth_year = collection.GetFormValue(PayloadKeys.birth_year);
            var birth_month = collection.GetFormValue(PayloadKeys.birth_month);
            var birth_day = collection.GetFormValue(PayloadKeys.birth_day);

            var gender = collection.GetFormValue(PayloadKeys.Gender);
            var hkid = collection.GetFormValue(PayloadKeys.HKID);
            
            var address1 = collection.GetFormValue(PayloadKeys.Address1);
            var address2 = collection.GetFormValue(PayloadKeys.Address2);
            var address3 = collection.GetFormValue(PayloadKeys.Address3);
            var region = collection.GetFormValue(PayloadKeys.Region);
            var district = collection.GetFormValue(PayloadKeys.District);

            var reg_source = collection.GetFormValue(PayloadKeys.Reg_source);
            
            var reg_status = collection.GetFormValue(PayloadKeys.Reg_status);
            var reg_ip = collection.GetFormValue(PayloadKeys.Reg_ip) ?? "";
            var activate_key = collection.GetFormValue(PayloadKeys.Activate_key) ?? "";
            var hash_key = "123456"; //collection.GetFormValue(PayloadKeys.Hash_key);
            var status = collection.GetFormValue(PayloadKeys.Status);
            var opt_in = collection.GetFormValue(PayloadKeys.Opt_in);
            var referrer_member_no = collection.GetFormValue(PayloadKeys.referrer_member_no);
            
            var member_category_id = collection.GetFormValue(PayloadKeys.member_category_id);

            //var email = collection.GetFormValue(PayloadKeys.Email);
            //var status = collection.GetFormValue(PayloadKeys.Status);
            //var login_id = collection.GetFormValue(PayloadKeys.Login_id);
            //var name = collection.GetFormValue(PayloadKeys.Name);

            // change_fields
            var change_fields_string = collection.GetFormValue(PayloadKeys.change_fields);
            var jsonExtractHelper = new JsonExtractHelper();
            var changedFields = jsonExtractHelper.ExtJSFormChangedFields(change_fields_string);

            var sql_remark = "";
            MemberObject memberObject;

            var systemCode = CommonConstant.SystemCode.undefine;

            // result for ExtJS
            var inputValid = false;
            var result = false;
            var msg = "";
            var msg_prefix = member_id == 0 ? "Create" : "Update";
            var msg_prefix_result = "";
            var msg_content = "";
            var url = "";

            if (member_id > 0)
                memberObject = _memberManager.GetDetail(member_id, false, ref systemCode);
            else
            {
                memberObject = new MemberObject();
                memberObject.member_level_id = 1;
            }

            memberObject.member_id = member_id;
            memberObject.member_no = member_no;
            memberObject.password = password;
            memberObject.email = email;
            memberObject.fbid = fbid;
            memberObject.fbemail = fbemail;
            memberObject.mobile_no = mobile_no;
            memberObject.salutation = salutation;
            memberObject.firstname = firstname;
            memberObject.middlename = middlename;
            memberObject.lastname = lastname;
            memberObject.birth_year = birth_year;
            memberObject.birth_month = birth_month;
            memberObject.birth_day = birth_day;
            memberObject.gender = gender;
            memberObject.hkid = hkid;
            memberObject.address1 = address1;
            memberObject.address2 = address2;
            memberObject.address3 = address3;
            memberObject.district = district;
            memberObject.region = region;
            memberObject.reg_source = reg_source;
            
            memberObject.reg_status = reg_status;
            memberObject.reg_ip = reg_ip;
            memberObject.activate_key = activate_key;
            memberObject.hash_key = hash_key;
            memberObject.session = "";
            memberObject.status = status;
            memberObject.opt_in = opt_in;
            
            memberObject.member_category_id = member_category_id;
            
            // check referrer member no
            memberObject.referrer = 0;

            if (!String.IsNullOrEmpty(referrer_member_no))
            {
                var referrer = _memberManager.GetDetail_byMemberNo(referrer_member_no, ref systemCode);
                if (referrer.member_id > 0)
                {
                    inputValid = true;
                    memberObject.referrer = referrer.member_id;
                }
                else
                {
                    inputValid = false;
                    msg_content = "Referrer Member Code is invalid";
                }

            }
            else
                inputValid = true;

            if (inputValid)
            {
                result = false;

                if (member_id == 0)
                {
                    var new_member_id = 0;
                    var resultCode = _memberManager.Create(
                        memberObject,
                        ref sql_remark,
                        ref new_member_id
                    );

                    if (resultCode == CommonConstant.SystemCode.normal)
                        result = true;
                    else if (resultCode == CommonConstant.SystemCode.err_memberNo_exist)
                        msg_content = "Member Code is duplicate";
                    else if (resultCode == CommonConstant.SystemCode.err_email_exist)
                        msg_content = "Email is duplicate";
                }
                else
                {
                    result = _memberManager.Update(
                        memberObject,
                        changedFields,
                        ref sql_remark);
                }
            }

            // output json
            if (result)
                msg_prefix_result = "Success";
            else
                msg_prefix_result = "Fail";

            if (!String.IsNullOrEmpty(msg_content))
                msg = msg_prefix + " " + msg_prefix_result + ": <br/>" + msg_content;
            else
                msg = msg_prefix + " " + msg_prefix_result;

            return new { success = result, url = url, msg = msg }.ToJson();
        }

        public string ToolbarData()
        {   
            var toolData = new List<ExtJsButton>();
            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });
           
            //toolData.Add(new ExtJsButton("button", "passcoderegistry")
            //{
            //    text = "Passcode Registry",
            //    iconUrl = "iconRole16",

            //    newTag_method = "open_pop_up",
            //    newTag_id = "pr:" + _member_id,
            //    newTag_title = "Passcode Registry:" + _member_id,
            //    newTag_url = "com.palmary.passcoderegistry.js.insert_popupform",
            //    newTag_iconCls = "iconRole16",
            //    newTag_iconClsC = "iconRole16",
            //    newTag_iconClsE = "iconRole16",
            //    newTag_itemId = _member_id.ToString()
            //});

            toolData.Add(new ExtJsButton("button", "productpurchase")
            {
                text = "Product Purchase",
                iconUrl = "iconRole16",

                newTag_method = "open_pop_up",
                newTag_id = "pp:" + _member_id,
                newTag_title = "Product Purchase:" + _member_id,
                newTag_url = "com.palmary.ProductPurchase.js.insert_popupform",
                newTag_iconCls = "iconRole16",
                newTag_iconClsC = "iconRole16",
                newTag_iconClsE = "iconRole16",
                newTag_itemId = _member_id.ToString()
            });

            toolData.Add(new ExtJsButton("button", "pointadjustment")
            {
                text = "Point Adjustment",
                iconUrl = "iconRole16",

                newTag_method = "open_pop_up",
                newTag_id = "padj:" + _member_id,
                newTag_title = "Point Adjustment:" + _member_id,
                newTag_url = "com.palmary.pointadjustment.js.form",
                newTag_iconCls = "iconRemarkList",
                newTag_iconClsC = "iconRemarkList",
                newTag_iconClsE = "iconRemarkList",
                newTag_itemId = _member_id.ToString()
            });

            toolData.Add(new ExtJsButton("button", "giftredemption")
            {
                text = "Redemption",
                iconUrl = "iconRole16",

                newTag_method = "open_pop_up",
                newTag_id = "redem:" + _member_id,
                newTag_title = "Redemption:" + _member_id,
                newTag_url = "com.palmary.giftredemption.js.insert",
                newTag_iconCls = "iconRole16",
                newTag_iconClsC = "iconRole16",
                newTag_iconClsE = "iconRole16",
                newTag_itemId = _member_id.ToString()
            });

            //toolData.Add(new ExtJsButton("button", "pointtransfer")
            //{
            //    text = "Point Transfer",
            //    iconUrl = "iconRole16",

            //    newTag_method = "open_pop_up",
            //    newTag_id = "ptransfer:" + _member_id,
            //    newTag_title = "Point Transfer:" + _member_id,
            //    newTag_url = "com.palmary.pointtransfer.js.form",
            //    newTag_iconCls = "iconRemarkList",
            //    newTag_iconClsC = "iconRemarkList",
            //    newTag_iconClsE = "iconRemarkList",
            //    newTag_itemId = _member_id.ToString()
            //});


            //toolData.Add(new ExtJsButton("button", "membercard")
            //{
            //    text = "Member Card",
            //    iconUrl = "iconRole16",

            //    newTag_method = "open_new_tag",
            //    newTag_id = "mcard:" + _member_id,
            //    newTag_title = "Member Card:" + _member_id,
            //    newTag_url = "com.palmary.membercard.js.edit",
            //    newTag_iconCls = "iconRole16",
            //    newTag_iconClsC = "iconRole16",
            //    newTag_iconClsE = "iconRole16",
            //    newTag_itemId = _member_id.ToString()
            //});

            var result = new { toolData = toolData }.ToJson();

            // old way
            //toolData.Add(new ExtJsButton("button", "passcoderegistered") { text = "Transaction History", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().openNewTag('PR_V:" + _member_id + "','Transaction History:" + _member_id + "','com.palmary.passcoderegistry.js.index','iconRole16','iconRole16','iconRole16','" + _member_id + "')}" });
            //toolData.Add(new ExtJsButton("button", "giftredemption_view") { text = "Redemption History", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().openNewTag('RH_V:" + _member_id + "','Redemption History:" + _member_id + "','com.palmary.giftredemption.js.index','iconRole16','iconRole16','iconRole16','" + _member_id + "')}" });
            //toolData.Add(new ExtJsButton("button", "passcoderegistry") { text = "Passcode Registry", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('PR:" + _member_id + "','Passcode Registry:" + _member_id + "','com.palmary.passcoderegistry.js.insert_popupform','iconRole16','iconRole16','iconRole16','" + _member_id + "')}" });
            //toolData.Add(new ExtJsButton("button", "productpurchase") { text = "Product Purchase", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('PP:" + _member_id + "','Product Purchase:" + _member_id + "','com.palmary.ProductPurchase.js.insert_popupform','iconRole16','iconRole16','iconRole16','" + _member_id + "')}" });
            //toolData.Add(new ExtJsButton("button", "pointadjustment") { text = "Point Adjustment", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('PointAdjust:" + _member_id + "','Point:Adjustment','com.palmary.pointadjustment.js.form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')}" });
            //toolData.Add(new ExtJsButton("button", "giftredemption") { text = "Redemption", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('Redmption:" + _member_id + "','Redemption:" + _member_id + "','com.palmary.giftredemption.js.insert','iconRole16','iconRole16','iconRole16','" + _member_id + "')}" });
            //toolData.Add(new ExtJsButton("button", "pointtransfer") { text = "Point Transfer", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('PointTransfer:" + _member_id + "','Point:Transfer','com.palmary.pointtransfer.js.form','iconRemarkList','iconRemarkList','iconRemarkList',9282,'owner','')}" });
            //toolData.Add(new ExtJsButton("button", "membercard") { text = "Member Card", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().openNewTag('MemberCard:" + _member_id + "','Member Card','com.palmary.membercard.js.edit','iconRole16','iconRole16','iconRole16',9282,'owner','')}" });

            // remove double quotation for herf:function
            // result = result.Replace(@"""href"":""f", @"""href"":f");
            // result = result.Replace(@"""}]}", @"}]}");
            // result = result.Replace(@")}""},{", @")}},{");

            return result.ToJson();
        }

        public string Insert()
        {
            var formTableJSON = TableFormHandler.GetFormByModule(new MemberObject());
            return formTableJSON;
        }

        // multi delete from grid
        public string MultiDelete()
        {
            var ids = Request.Form["id"];
            var idArrary = ids.Split(',');

            
            var sql_remark = "";
            var result = false;
            var msg = "";
            var no_permission = false;

            var need_to_delete_count = 0;
            var success_delete_count = 0;

            var fail_rec_id_list = new List<string>();

            foreach (var idStr in idArrary)
            {
                var rec_id = int.Parse(idStr);
                if (rec_id > 0)
                {
                    need_to_delete_count++;

                    var systemCode = _memberManager.SoftDelete(rec_id, ref sql_remark);

                    if (systemCode == CommonConstant.SystemCode.normal)
                        success_delete_count++;
                    else if (systemCode == CommonConstant.SystemCode.no_permission)
                    {
                        no_permission = true;
                        break;
                    }
                    else if (systemCode == CommonConstant.SystemCode.record_invalid)
                        fail_rec_id_list.Add(rec_id.ToString());
                }
            }

            if (no_permission)
                msg = "Failed: no permission";
            else if (need_to_delete_count > success_delete_count)
                msg = "Deleted " + success_delete_count + " row(s). <br/>Cannot delete following id: " + string.Join(",", fail_rec_id_list.ToArray());
            else if (need_to_delete_count == success_delete_count)
            {
                result = true;
                msg = "Success deleted " + success_delete_count + " row(s).";
            }

            return new { success = result, url = "", msg = msg }.ToJson();
        }
    }
}