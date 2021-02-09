using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules.Gift;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Tree;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.Common.Languages;
using System.Web.Routing;

namespace Palmary.Loyalty.Web_backend.Controllers.Gift
{
    [Authorize]
    public class GiftCategoryController : Controller
    {
        private GiftCategoryManager _giftCategoryManager;
        private GiftCategoryLangManager _giftCategoryLangManager;
        private int _giftCategory_id;

        public GiftCategoryController()
        {
                
            _giftCategoryManager = new GiftCategoryManager();
            _giftCategoryLangManager = new GiftCategoryLangManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _giftCategory_id = int.Parse(id.ToString());       
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

      
        // Create New Form
        public string Insert()
        {
            var selected_parent_id = 0;
            
            if (!String.IsNullOrEmpty(this.HttpContext.Request["parent_id"]) && this.HttpContext.Request["parent_id"] != "src")
                selected_parent_id = int.Parse(this.HttpContext.Request["parent_id"]);

            var gift_cat = new sp_GetGiftCategoryDetailResult();
            gift_cat.parent_id = selected_parent_id;

            var formTableJSON = TableFormHandler.GetFormByModule(gift_cat, new List<sp_GetGiftCategoryLangDetailResult>());
            return formTableJSON;
        }

        // Edit detail
        public string GetModule()
        {
            if (_giftCategory_id == 0 && !String.IsNullOrEmpty(this.HttpContext.Request["id"]) && this.HttpContext.Request["id"] !="src") // also receive HTTP GET
            {
                _giftCategory_id = int.Parse(this.HttpContext.Request["id"]);
            }

            var sql_result = false;
            var giftCategory = _giftCategoryManager.GetGiftCategoryDetail(SessionManager.Current.obj_id, _giftCategory_id, ref sql_result);
            var giftCategory_lang = _giftCategoryLangManager.GetGiftCategoryLangDetail(SessionManager.Current.obj_id, _giftCategory_id, ref sql_result);
            var formTableJSON = TableFormHandler.GetFormByModule(giftCategory, giftCategory_lang);
            return formTableJSON;
        }
            
            // Edit detail toolbar
        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();
            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });

            var result = new { toolData = toolData }.ToJson();

            // remove double quotation for herf:function
            result = result.Replace(@"""href"":""f", @"""href"":f");
            result = result.Replace(@"""}]}", @"}]}");

            return result;
        }

        public string Update(FormCollection collection)
        {
            var parent_id = 0;// collection.GetFormValue(PayloadKeys.GiftCategory.parent_id);
            var leaf = collection.GetFormValue(PayloadKeys.GiftCategory.leaf);
            var category_id = collection.GetFormValue(PayloadKeys.GiftCategory.category_id);
            var display_order = collection.GetFormValue(PayloadKeys.GiftCategory.display_order);
            var status = collection.GetFormValue(PayloadKeys.GiftCategory.status);

            // photo file name
            var path = collection["fileData"].Split('/');
            var fileFullName = path.Last();
            var fileNameList = fileFullName.Split('.');
            var photo_file_name = fileNameList[0];
            var photo_file_extension = "." + fileNameList[1];
            photo_file_name = photo_file_name.Replace((string)CommonConstant.ImageSizeName_postfix[(int)CommonConstant.ImageSizeType.thumb], ""); // remove _thumb (for view edit form)


            // lang variables
            var name_tc = collection.GetFormValue(PayloadKeys.varWithLang("name", "tc"));
            var description_tc = collection.GetFormValue(PayloadKeys.varWithLang("description", "tc"));

            var name_en = collection.GetFormValue(PayloadKeys.varWithLang("name", "en"));
            var description_en = collection.GetFormValue(PayloadKeys.varWithLang("description", "en"));

            var name_sc = collection.GetFormValue(PayloadKeys.varWithLang("name", "sc"));
            var description_sc = collection.GetFormValue(PayloadKeys.varWithLang("description", "sc"));

            var new_giftCategory_id = 0;
            var sql_remark = "";

            if (category_id == 0)
            {
                var addFlag = _giftCategoryManager.Create(
                    SessionManager.Current.obj_id,

                    parent_id,
                    photo_file_name,
                    photo_file_extension,
                    display_order,
                    status,
                    
                    ref new_giftCategory_id,
                    ref sql_remark);

                // Create Lang TC
                _giftCategoryLangManager.Create(SessionManager.Current.obj_id,
                                            new_giftCategory_id,
                                            (int)Common.CommonConstant.LangCode.tc,
                                            name_tc,
                                            description_tc,
                                            1, //status_tc,

                                            ref sql_remark
                                            );

                // Create Lang EN
                _giftCategoryLangManager.Create(SessionManager.Current.obj_id,
                                            new_giftCategory_id,
                                            (int)Common.CommonConstant.LangCode.en,
                                            name_en,
                                            description_en,
                                            1, //status_en,

                                            ref sql_remark
                                            );

                // Create Lang SC
                _giftCategoryLangManager.Create(SessionManager.Current.obj_id,
                                            new_giftCategory_id,
                                            (int)Common.CommonConstant.LangCode.sc,
                                            name_sc,
                                            description_sc,
                                            1, //status_sc,

                                            ref sql_remark
                                            );

                return addFlag ? "{success:true,url:'',msg:'Add Success'}" : "{success:true,url:'',msg:'Add Failed: " + sql_remark + "'}";
            }
            else if (category_id > 0)
            {
                var updateFlag = _giftCategoryManager.Update(
                    SessionManager.Current.obj_id,

                    parent_id,
                    leaf,
                    category_id,
                    photo_file_name,
                    photo_file_extension,
                    display_order,
                    status,

                    ref sql_remark);

                // Update Lang TC
                _giftCategoryLangManager.Update(SessionManager.Current.obj_id,
                                            category_id,
                                            (int)Common.CommonConstant.LangCode.tc,
                                            name_tc,
                                            description_tc,
                                            1, //status_tc,

                                            ref sql_remark
                                            );

                // Update Lang EN
                _giftCategoryLangManager.Update(SessionManager.Current.obj_id,
                                            category_id,
                                            (int)Common.CommonConstant.LangCode.en,
                                            name_en,
                                            description_en,
                                            1, //status_en,

                                            ref sql_remark
                                            );

                // Update Lang SC
                _giftCategoryLangManager.Update(SessionManager.Current.obj_id,
                                            category_id,
                                            (int)Common.CommonConstant.LangCode.sc,
                                            name_sc,
                                            description_sc,
                                            1, //status_sc,

                                            ref sql_remark
                                            );

                return updateFlag ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Saved Failed: " + sql_remark + "'}";
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }

        public string Tree()
        {
            var resultList = _giftCategoryManager.GetGiftCategoryLists(SessionManager.Current.obj_id, 0, 0, "").ToList();
            var catNodeList = new List<CategoryNode>();
            foreach (var x in resultList)
            {
                var node = new CategoryNode() { id = x.category_id, ParentID = x.parent_id, text = x.name, expanded = true };
                catNodeList.Add(node);
            }
          
            var r = TreeManager.BuildTree(catNodeList);
            
            return r.ToJson();
        }

        public void TreeMoveUpdate(FormCollection collection)
        {
            var cat_id = int.Parse(collection["id"]);
            var new_parent_id = 0;

            if (!String.IsNullOrEmpty(collection["newParentId"]) && collection["newParentId"] != "src")
                new_parent_id = int.Parse(collection["newParentId"]);

            var sql_result = false;
            var sql_remark = "";
            var theCat = _giftCategoryManager.GetGiftCategoryDetail(SessionManager.Current.obj_id, cat_id, ref sql_result);

            sql_result = _giftCategoryManager.Update(
                SessionManager.Current.obj_id,
                new_parent_id,
                theCat.leaf,
                theCat.category_id,
                theCat.photo_file_name,
                theCat.photo_file_extension,
                theCat.display_order,
                theCat.status,

                ref sql_remark
            );
        }

        /// <summary>
        /// 删除category方法
        /// </summary>
        /// <returns></returns>
        public string MultiDelete()
        {
            string retult = string.Empty;
            if (!String.IsNullOrEmpty(this.HttpContext.Request["parent_id"]) && this.HttpContext.Request["parent_id"] != "src")
            {
                int selected_parent_id = int.Parse(this.HttpContext.Request["parent_id"]);
                var systemCode = _giftCategoryManager.DelCategory(selected_parent_id,SessionManager.Current.obj_id, ref retult);
                if (systemCode == CommonConstant.SystemCode.normal)
                {
                    return "successful";
                }
            }
            return "unsuccessful";
        }
    }
}