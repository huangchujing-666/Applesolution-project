using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// basic
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend;
using System.Web.Routing;

// additional
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.BO.DataTransferObjects.Tree;
using Palmary.Loyalty.BO.DataTransferObjects.Service;
using Palmary.Loyalty.BO.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;

namespace Palmary.Loyalty.Web_backend.Controllers.Service
{
    [Authorize]
    public class ServiceCategoryController : Controller
    {
        private ServiceCategoryManager _serviceCategoryManager;
        private int _category_id;

        public ServiceCategoryController()
        {
            _serviceCategoryManager = new ServiceCategoryManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var id = RouteData.Values["id"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                _category_id = int.Parse(id.ToString());
            }
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        public string Tree()
        {
            var systemCode = CommonConstant.SystemCode.undefine;
            var resultList = _serviceCategoryManager.GetList(ref systemCode);

            var catNodeList = new List<CategoryNode>();
            foreach (var x in resultList)
            {
                var node = new CategoryNode() { id = x.category_id, ParentID = x.parent_id, text = x.name, expanded = true };
                catNodeList.Add(node);
            }

            var r = TreeManager.BuildTree(catNodeList);
            System.Diagnostics.Debug.WriteLine(r.ToJson());

            return r.ToJson();
        }

        public string Insert()
        {
            var selected_parent_id = 0;

            if (!String.IsNullOrEmpty(this.HttpContext.Request["parent_id"]) && this.HttpContext.Request["parent_id"] != "src")
                selected_parent_id = int.Parse(this.HttpContext.Request["parent_id"]);

            var category = new ServiceCategoryObject();
            category.parent_id = selected_parent_id;

            var formTableJSON = TableFormHandler.GetFormByModule(category);
            return formTableJSON;
        }

        // Edit detail
        public string GetModule()
        {
            if (_category_id == 0 && !String.IsNullOrEmpty(this.HttpContext.Request["id"]) && this.HttpContext.Request["id"] != "src") // also receive HTTP GET
            {
                System.Diagnostics.Debug.WriteLine("this.HttpContext.Request id: " + this.HttpContext.Request["id"]);
                _category_id = int.Parse(this.HttpContext.Request["id"]);
            }

            var systemCode = CommonConstant.SystemCode.undefine;
          
            ServiceCategoryObject category;
            if (_category_id > 0)
                category = _serviceCategoryManager.GetDetail(_category_id, ref systemCode);
            else
                category = new ServiceCategoryObject();

            var formTableJSON = TableFormHandler.GetFormByModule(category);
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var parent_id = collection.GetFormValue(PayloadKeys.ServiceCategory.parent_id);
            var leaf = collection.GetFormValue(PayloadKeys.ServiceCategory.leaf);
            var category_id = collection.GetFormValue(PayloadKeys.ServiceCategory.category_id);
            var display_order = collection.GetFormValue(PayloadKeys.ServiceCategory.display_order);
            var status = collection.GetFormValue(PayloadKeys.ServiceCategory.status);

            // lang variables
            var name = collection.GetFormValue(PayloadKeys.ServiceCategory.name);
            var description = collection.GetFormValue(PayloadKeys.ServiceCategory.description);

            var serviceCategory = new ServiceCategoryObject()
            {
                category_id = category_id,
                parent_id = parent_id,
                leaf = leaf,
                display_order = display_order,
                name = name,
                description = description,
                status = status
            };

            if (category_id == 0)
            {
                var systemCode = _serviceCategoryManager.Create(serviceCategory);

                var result = "";

                if(systemCode == CommonConstant.SystemCode.normal)
                    result = new { success = true, url = "", msg = "Add Success"}.ToJson();
                else
                    result = new { success = true, url = "", msg = "Add Failed" }.ToJson();

                return result;
            }
            else if (category_id > 0)
            {
                var systemCode = _serviceCategoryManager.Update(serviceCategory);
                var result = "";

                if (systemCode == CommonConstant.SystemCode.normal)
                    result = new { success = true, url = "", msg = "Saved Success" }.ToJson();
                else
                    result = new { success = true, url = "", msg = "Save Failed" }.ToJson();

                return result;
            }
            else
                return "{success:true,url:'',msg:'Saved Failed: Invalid Data'}";
        }
    }
}