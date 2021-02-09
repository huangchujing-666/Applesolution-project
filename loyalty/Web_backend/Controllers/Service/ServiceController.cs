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
using Palmary.Loyalty.BO.DataTransferObjects.Service;
using Palmary.Loyalty.BO.Modules.Service;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;

namespace Palmary.Loyalty.Web_backend.Controllers.Service
{
    [Authorize]
    public class ServicePlanController : Controller
    {
        private ServicePlanManager _servicePlanManager;
        private int _id;

        public ServicePlanController()
        {
            _servicePlanManager = new ServicePlanManager();            
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

        public string Insert()
        {
            var obj = new ServicePlanObject();
            var formTableJSON = TableFormHandler.GetFormByModule(obj);
            return formTableJSON;
        }

        public string GetModule()
        {
            var system_code = CommonConstant.SystemCode.undefine;
            var service = _servicePlanManager.GetDetail(_id, ref system_code);
            var formTableJSON = TableFormHandler.GetFormByModule(service);
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var plan_id = collection.GetFormValue(PayloadKeys.ServicePlan.plan_id);
            var plan_no = collection.GetFormValue(PayloadKeys.ServicePlan.plan_no);
            var type = collection.GetFormValue(PayloadKeys.ServicePlan.type);
            var name = collection.GetFormValue(PayloadKeys.ServicePlan.name);
            var description = collection.GetFormValue(PayloadKeys.ServicePlan.description);
            var fee = collection.GetFormValue(PayloadKeys.ServicePlan.fee);
            var point_expiry_month = collection.GetFormValue(PayloadKeys.ServicePlan.point_expiry_month);
            var ratio_payment = collection.GetFormValue(PayloadKeys.ServicePlan.ratio_payment);
            var ratio_point = collection.GetFormValue(PayloadKeys.ServicePlan.ratio_point);
            var status = collection.GetFormValue(PayloadKeys.ServicePlan.status);
            
            var servicePlan = new ServicePlanObject() 
            {
                plan_id = plan_id,
                plan_no = plan_no,
                type = type,
                name = name,
                description = description,
                fee = fee,
                point_expiry_month = point_expiry_month,
                ratio_payment = ratio_payment,
                ratio_point = ratio_point,
                status = status
            };

            if (servicePlan.plan_id == 0)
            {
                var addFlag = _servicePlanManager.Create(servicePlan);
                return addFlag == CommonConstant.SystemCode.normal ? "{success:true,url:'',msg:'Add Success'}" : "{success:true,url:'',msg:'Add Failed'}";
            }
            else
            {
                var addFlag = _servicePlanManager.Update(servicePlan);
                return addFlag == CommonConstant.SystemCode.normal ? "{success:true,url:'',msg:'Saved Success'}" : "{success:true,url:'',msg:'Saved Failed'}";
            }
        }

        public string ToolbarData()
        {
            var toolData = new List<ExtJsButton>();
            toolData.Add(new ExtJsButton("button", "view") { text = "View", hidden = true, iconUrl = "iconView" });
            toolData.Add(new ExtJsButton("button", "edit") { text = "Edit", iconUrl = "iconRole16" });
       
            var result = new { toolData = toolData }.ToJson();

            // remote double quotation for herf:function
            result = result.Replace(@"""href"":""f", @"""href"":f");
            result = result.Replace(@"""}]}", @"}]}");

            return result;
        }
    }
}
