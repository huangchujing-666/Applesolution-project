using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;

using Palmary.Loyalty.BO.Modules.Gift;

using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.Common.Languages;

using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.Media;
using Palmary.Loyalty.BO.Modules.Administration.Security;

namespace Palmary.Loyalty.Web_backend.Controllers.Gift
{
    [Authorize]
    public class GiftController : Controller
    {
        private GiftManager _giftManager;
        private GiftLangManager _giftLangManager;
        private GiftLocationManager _giftLocationManager;
        private int _gift_id;

        public GiftController()
        {
            // Keep Access Object Data

            _giftManager = new GiftManager();
            _giftLangManager = new GiftLangManager();
            _giftLocationManager = new GiftLocationManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
       
            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _gift_id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

      
        // Create New Form
        public string Insert()
        {
            var dataObject = new sp_GetGiftDetailByResult();
            var formTableJSON = TableFormHandler.GetFormByModule(dataObject, new List<sp_GetGiftLangDetailByResult>());
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            // new gift object
            GiftObject giftObject = new GiftObject();
            giftObject.gift_id = collection.GetFormValue(PayloadKeys.Gift.gift_id);
            giftObject.gift_no = collection.GetFormValue(PayloadKeys.Gift.gift_no);
            
            giftObject.point = collection.GetFormValue(PayloadKeys.Gift.point);
            giftObject.alert_level = collection.GetFormValue(PayloadKeys.Gift.alert_level);
            giftObject.cost = collection.GetFormValue(PayloadKeys.Gift.cost);

            giftObject.discount = collection.GetFormValue(PayloadKeys.Gift.discount);
            giftObject.discount_point = collection.GetFormValue(PayloadKeys.Gift.discount_point);

            // Category
            var categoryString_list = collection.GetFormValue(PayloadKeys.Product.category).Split(',');
            var category_list = new List<GiftCategoryObject>();

            foreach (var c in categoryString_list)
            {
                var obj = new GiftCategoryObject
                {
                    category_id = int.Parse(c)
                };

                category_list.Add(obj);
            }

            giftObject.category_list = category_list;

            // discount_dateTime_from
            var theDiscountDate_from = collection.GetFormValue(PayloadKeys.Gift.discount_date_range_from);
            if (theDiscountDate_from.Year == 1)
                giftObject.discount_active_date = null;
            else
                giftObject.discount_active_date = theDiscountDate_from;

            // discount_dateTime_to
            var theDiscountDate_to = collection.GetFormValue(PayloadKeys.Gift.discount_date_range_to);
            if (theDiscountDate_to.Year == 1)
                giftObject.discount_expiry_date = null;
            else
                giftObject.discount_expiry_date = theDiscountDate_to;

            // Hot Item
            giftObject.hot_item = collection.GetFormValue(PayloadKeys.Gift.hotItem);

            // hotItem_dateTime_from
            var theHotDate_from = collection.GetFormValue(PayloadKeys.Gift.hotItem_date_range_from);
            if (theHotDate_from.Year == 1)
                giftObject.hot_item_active_date = null;
            else
                giftObject.hot_item_active_date = theHotDate_from;

            // hotItem_dateTime_to
            var theHotDate_to = collection.GetFormValue(PayloadKeys.Gift.hotItem_date_range_to);
            if (theHotDate_to.Year == 1)
                giftObject.hot_item_expiry_date = null;
            else
                giftObject.hot_item_expiry_date = theHotDate_to;

            giftObject.hot_item_display_order = collection.GetFormValue(PayloadKeys.Gift.hotItem_display_order);

            giftObject.display_public = collection.GetFormValue(PayloadKeys.Gift.display_public);

            // display_dateTime_from
            var theDisplayDate_from = collection.GetFormValue(PayloadKeys.Gift.display_date_range_from);
            var display_dateTime_from = new DateTime(theDisplayDate_from.Year, theDisplayDate_from.Month, theDisplayDate_from.Day, theDisplayDate_from.Hour, theDisplayDate_from.Minute, theDisplayDate_from.Second);
            giftObject.display_active_date = display_dateTime_from;

            // display_dateTime_to
            var theDisplayDate_to = collection.GetFormValue(PayloadKeys.Gift.display_date_range_to);
            var display_dateTime_to = new DateTime(theDisplayDate_to.Year, theDisplayDate_to.Month, theDisplayDate_to.Day, theDisplayDate_to.Hour, theDisplayDate_to.Minute, theDisplayDate_to.Second);
            giftObject.display_expiry_date = display_dateTime_to;

            // redeem_dateTime_from
            var theRedeemDate_form = collection.GetFormValue(PayloadKeys.Gift.redeem_date_range_from);
            var redeem_dateTime_from = new DateTime(theRedeemDate_form.Year, theRedeemDate_form.Month, theRedeemDate_form.Day, theRedeemDate_form.Hour, theRedeemDate_form.Minute, theRedeemDate_form.Second);
            giftObject.redeem_active_date = redeem_dateTime_from;

            // redeem_dateTime_to
            var theRedeemDate_to = collection.GetFormValue(PayloadKeys.Gift.redeem_date_range_to);
            var redeem_dateTime_to = new DateTime(theRedeemDate_to.Year, theRedeemDate_to.Month, theRedeemDate_to.Day, theRedeemDate_to.Hour, theRedeemDate_to.Minute, theRedeemDate_to.Second);
            giftObject.redeem_expiry_date = redeem_dateTime_to;
            
            giftObject.status = collection.GetFormValue(PayloadKeys.Gift.status);

            // Location
            var locationString_list = collection.GetFormValue(PayloadKeys.Gift.location).Split(',');
            var location_list = new List<GiftLocationObject>();

            foreach (var locationString in locationString_list)
            {
                GiftLocationObject gifLocationObject = new GiftLocationObject();
                gifLocationObject.location_id = int.Parse(locationString);
                location_list.Add(gifLocationObject);
            }

            giftObject.location_list = location_list;

            // Member Privilege
            var memberPrivilegeString_list = collection.GetFormValue(PayloadKeys.Gift.member_privilege).Split(',');
            var member_privilege_list = new List<GiftMemberPrivilegeObject>();

            foreach (var privilegeString in memberPrivilegeString_list)
            {
                GiftMemberPrivilegeObject privilegeObject = new GiftMemberPrivilegeObject();
                privilegeObject.member_level_id = int.Parse(privilegeString);
                member_privilege_list.Add(privilegeObject);
            }

            giftObject.member_privilege_list = member_privilege_list;

            // Languages  
            var lang_list = new List<GiftLangObject>();

            var theLang_tc = new GiftLangObject();
            theLang_tc.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "tc"));
            theLang_tc.gift_id = giftObject.gift_id;
            theLang_tc.lang_id = (int)CommonConstant.LangCode.tc;
            theLang_tc.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "tc"));
            lang_list.Add(theLang_tc);

            var theLang_en = new GiftLangObject();
            theLang_en.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "en"));
            theLang_en.gift_id = giftObject.gift_id;
            theLang_en.lang_id = (int)CommonConstant.LangCode.en;
            theLang_en.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "en"));
            lang_list.Add(theLang_en);

            var theLang_sc = new GiftLangObject();
            theLang_sc.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "sc"));
            theLang_sc.gift_id = giftObject.gift_id;
            theLang_sc.lang_id = (int)CommonConstant.LangCode.sc;
            theLang_sc.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "sc"));
            lang_list.Add(theLang_sc);


            giftObject.lang_list = lang_list;

            // Photo
            var photosJSON = collection.GetFormValue(PayloadKeys.Gift.photos);

            var jsonExtractHelper = new JsonExtractHelper();

            var error_msg = "";
            var dataValid = true;

            if (photosJSON == "[" || String.IsNullOrEmpty(photosJSON))
            {
                error_msg = "Please upload gift photo.";
                dataValid = false;
            }else
            {
                // Extract
                var photoField_list = jsonExtractHelper.ExtJSFormPhotosField(photosJSON);

                var photos_list = new List<PhotoObject>();
                foreach (var p in photoField_list)
                { 
                    var photo = new PhotoObject();
                    photo.photo_id = int.Parse(p.id);
              
                    // photo file name
                    var path = p.src.Split('/');
                    var fileFullName = path.Last();
                    var fileNameList = fileFullName.Split('.');
                    var photo_file_name = fileNameList[0];

                    photo.file_extension = "." + fileNameList[1];
                    photo.file_name = photo_file_name.Replace((string)CommonConstant.ImageSizeName_postfix[(int)CommonConstant.ImageSizeType.thumb], ""); // remove _thumb (for view edit form)

                    photo.display_order = int.Parse(p.orderedID);
                    photo.status = 1;
                    photo.name = "";
                    photo.caption = "";
                    photos_list.Add(photo);
                }
                giftObject.photo_list = photos_list;
            }

            if (giftObject.redeem_active_date.ToString("yyyy") == "0001")
            {
                error_msg = "Please input redeem active date.";
                dataValid = false;
            }

            if (giftObject.redeem_expiry_date.ToString("yyyy") == "0001")
            {
                error_msg = "Please input redeem expiry date.";
                dataValid = false;
            }

            if (giftObject.display_active_date.ToString("yyyy") == "0001")
            {
                error_msg = "Please input display active date.";
                dataValid = false;
            }

            if (giftObject.display_expiry_date.ToString("yyyy") == "0001")
            {
                error_msg = "Please input display expiry date.";
                dataValid = false;
            }

            if (dataValid)
            {
                var sql_remark = "";
                if (giftObject.gift_id == 0)
                {
                    var addFlag = _giftManager.Create(
                        giftObject,

                        ref sql_remark,
                        ref _gift_id);

                    return addFlag ? "{success:true,url:'',msg:'Add Success'}" : "{success:true,url:'',msg:'Add Failed: " + sql_remark + "'}";
                }
                else if (giftObject.gift_id > 0)
                {
                    // change_fields
                    var change_fields = collection.GetFormValue(PayloadKeys.change_fields);

                    var changedFields = jsonExtractHelper.ExtJSFormChangedFields(change_fields);

                    var updateFlag = _giftManager.Update(
                        giftObject,

                        changedFields,
                        ref sql_remark);

                    return updateFlag ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Saved Failed: " + sql_remark + "'}";
                }
                else
                    return "{success:false, url:'', msg:'Saved Failed'}";
            }
            else
                return "{success:false,url:'', msg:'Saved Failed: " + error_msg + "'}";
        }

        public string GetModule()
        {
            var sql_result = false;
            var gift = _giftManager.GetGiftDetailBy(SessionManager.Current.obj_id, _gift_id, ref sql_result);
            var gift_lang = _giftLangManager.GetGiftLangDetailBy(SessionManager.Current.obj_id, _gift_id, ref sql_result);
            var formTableJSON = TableFormHandler.GetFormByModule(gift, gift_lang);
            return formTableJSON;
        }

        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();

            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });

            toolData.Add(new ExtJsButton("button", "inventory")
            {
                text = "Inventory",
                iconUrl = "iconRole16",

                newTag_method = "open_new_tag",
                newTag_id = "V_GIS:" + _gift_id,
                newTag_title = "Inventory:" + _gift_id,
                newTag_url = "com.palmary.giftinventory.js.index",
                newTag_iconCls = "iconRole16",
                newTag_iconClsC = "iconRole16",
                newTag_iconClsE = "iconRole16",
                newTag_itemId = _gift_id.ToString()
            });

            toolData.Add(new ExtJsButton("button", "add_inventory_record")
            {
                text = "Add Inventory Change",
                iconUrl = "iconRole16",

                newTag_method = "open_pop_up",
                newTag_id = "V_GI_add:" + _gift_id,
                newTag_title = "Add Inventory Change:" + _gift_id,
                newTag_url = "com.palmary.giftinventory.js.insert",
                newTag_iconCls = "iconHeadOffice",
                newTag_iconClsC = "iconHeadOffice",
                newTag_iconClsE = "iconHeadOffice",
                newTag_itemId = _gift_id.ToString()
            });

            var result = new { toolData = toolData }.ToJson();

            // old way
            //toolData.Add(new ExtJsButton("button", "inventory") { text = "Inventory", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().openNewTag('V_GIS:" + _gift_id + "','Gift Inventory:" + _gift_id + "','com.palmary.giftinventory.js.index','iconRole16','iconRole16','iconRole16','" + _gift_id + "')}" });
            //toolData.Add(new ExtJsButton("button", "add_inventory_record") { text = "Add Inventory Change", iconUrl = "iconRole16", href = "function(){new com.embraiz.tag().open_pop_up('V_GI_add:" + _gift_id + "', 'Gift Inventory Adjustment', 'com.palmary.giftinventory.js.insert', 'iconHeadOffice', 'iconHeadOffice', 'iconHeadOffice', '', '', '')}" });

            //// remove double quotation for herf:function
            //result = result.Replace(@"""href"":""f", @"""href"":f");
            //result = result.Replace(@"""}]}", @"}]}");
            //result = result.Replace(@")}""},{", @")}},{");
            
            return result;
        }

        public string CheckDuplicateGiftNo(FormCollection collection)
        {
            var giftNo = collection["value"];


            var duplicate = _giftManager.CheckDuplicateGiftNo(giftNo);
            var message = "";
            if (duplicate)
                message = "Gift code is duplicate";

            return new { success = true, duplicate = duplicate, message = message }.ToJson();
        }

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

                    // request relative BO Manager to perform DELETE
                    var giftManager = new GiftManager();
                    var systemCode = giftManager.SoftDelete(rec_id, ref sql_remark);

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