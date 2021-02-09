using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;

using Palmary.Loyalty.Web_backend;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.BO.DataTransferObjects.Member;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Modules.Member;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.BO.DataTransferObjects.Tree;

namespace Palmary.Loyalty.Web_backend.Controllers.Member
{
    [Authorize]
    public class MemberCategoryController : Controller
    {
        private MemberCategoryManager _memberCategoryManager;
        private MemberCategoryLangManager _memberCategoryLangManager;
        private int _memberCategory_id;

        public MemberCategoryController()
        {
            _memberCategoryManager = new MemberCategoryManager();
            _memberCategoryLangManager = new MemberCategoryLangManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _memberCategory_id = int.Parse(id.ToString());       
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }


        // Create New Form
        public string Insert()
        {
            var selected_parent_id = 0;

            if (!String.IsNullOrEmpty(this.HttpContext.Request["parent_id"]) && this.HttpContext.Request["parent_id"] != "src")
                selected_parent_id = int.Parse(this.HttpContext.Request["parent_id"]);

            var member_cat = new MemberCategoryObject();
            member_cat.parent_id = selected_parent_id;

            var formTableJSON = TableFormHandler.GetFormByModule(member_cat);
            return formTableJSON;
        }

        // View and Edit detail
        public string GetModule()
        {
            if (_memberCategory_id == 0 && !String.IsNullOrEmpty(this.HttpContext.Request["id"]) && this.HttpContext.Request["id"] != "src") // also receive HTTP GET
            {
                _memberCategory_id = int.Parse(this.HttpContext.Request["id"]);
            }

            var systemCode = CommonConstant.SystemCode.undefine;
            var memberCategory = _memberCategoryManager.GetMemberCategoryDetail_withLang(_memberCategory_id, ref systemCode);
            var formTableJSON = TableFormHandler.GetFormByModule(memberCategory);
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var memberCategoryObject = new MemberCategoryObject();
            
            memberCategoryObject.category_id = collection.GetFormValue(PayloadKeys.MemberCategory.category_id);
            memberCategoryObject.parent_id = collection.GetFormValue(PayloadKeys.MemberCategory.parent_id);
            memberCategoryObject.leaf = collection.GetFormValue(PayloadKeys.MemberCategory.leaf);
            memberCategoryObject.display_order = collection.GetFormValue(PayloadKeys.MemberCategory.display_order);
            memberCategoryObject.status = collection.GetFormValue(PayloadKeys.MemberCategory.status);

            // Languages  
            var lang_list = new List<MemberCategoryLangObject>();

            var theLang_tc = new MemberCategoryLangObject();
            theLang_tc.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "tc"));
            theLang_tc.category_id = memberCategoryObject.category_id;
            theLang_tc.lang_id = (int)CommonConstant.LangCode.tc;
            theLang_tc.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "tc"));
            lang_list.Add(theLang_tc);

            var theLang_en = new MemberCategoryLangObject();
            theLang_en.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "en"));
            theLang_en.category_id = memberCategoryObject.category_id;
            theLang_en.lang_id = (int)CommonConstant.LangCode.en;
            theLang_en.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "en"));
            lang_list.Add(theLang_en);

            var theLang_sc = new MemberCategoryLangObject();
            theLang_sc.name = collection.GetFormValue(PayloadKeys.varWithLang("name", "sc"));
            theLang_sc.category_id = memberCategoryObject.category_id;
            theLang_sc.lang_id = (int)CommonConstant.LangCode.sc;
            theLang_sc.description = collection.GetFormValue(PayloadKeys.varWithLang("description", "sc"));
            lang_list.Add(theLang_sc);

            memberCategoryObject.lang_list = lang_list;

            ExtJsResult result;

            if (memberCategoryObject.category_id == 0) // Create
            {
                var system_code = _memberCategoryManager.Create(memberCategoryObject);

                return system_code == CommonConstant.SystemCode.normal ?
                                "{success:true,url:'',msg:'Add Success'}" 
                                : "{success:true,url:'',msg:'Add Failed: " + CommonConstant.SystemWord[(int)system_code] + "'}";
            }
            else if (memberCategoryObject.category_id > 0) //Update
            {
                var system_code = _memberCategoryManager.Update(memberCategoryObject);

                if (system_code == CommonConstant.SystemCode.normal)
                    result = new ExtJsResult { success = true, msg = "Update Success" };
                else
                    result = new ExtJsResult { success = false, msg = "Update Failed: " + CommonConstant.SystemWord[(int)system_code] };
            }
            else
                result = new ExtJsResult { success = false, msg = "Update Failed: Invalid Data" };

            return result.ToJson();
        }

        public string Tree()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var resultList = _memberCategoryManager.GetMemberCategory_list(ref systemCode);
            var catNodeList = new List<CategoryNode>();
            foreach (var x in resultList)
            {
                var node = new CategoryNode() { id = x.category_id, ParentID = x.parent_id, text = x.name, expanded = true };
                catNodeList.Add(node);
            }

            var nestedList = TreeManager.BuildTree(catNodeList);
            System.Diagnostics.Debug.WriteLine(nestedList.ToJson());

            return nestedList.ToJson();
        }

        public void TreeMoveUpdate(FormCollection collection)
        {
            var cat_id = int.Parse(collection["id"]);
            var new_parent_id = 0;

            if (!String.IsNullOrEmpty(collection["newParentId"]) && collection["newParentId"] != "src")
                new_parent_id = int.Parse(collection["newParentId"]);

            var system_code = CommonConstant.SystemCode.undefine;
            var theCat = _memberCategoryManager.GetMemberCategoryDetail_withLang(cat_id, ref system_code);

            theCat.parent_id = new_parent_id;

            system_code = _memberCategoryManager.Update(theCat);

        }
    }
}
