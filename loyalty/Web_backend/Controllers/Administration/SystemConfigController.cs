using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using Palmary.Loyalty.Common;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.BO.DataTransferObjects.Security;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using Palmary.Loyalty.Web_backend.Modules.ExtJS;
using Palmary.Loyalty.Web_backend;

using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.Web_backend.Modules.Administration;
using Palmary.Loyalty.BO.DataTransferObjects.SystemConfig;

namespace Palmary.Loyalty.Web_backend.Controllers.Administration
{
    [Authorize]
    public class SystemConfigController : Controller
    {
        private int _id;

        public SystemConfigController()
        {
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

        // Edit pop up Form
        public string EditPopForm()
        {
            var config_id = _id;

            // Access BO to retrieve real data
            var systemCode = CommonConstant.SystemCode.undefine;
            var systemConfigManager = new SystemConfigManager();
            var obj = systemConfigManager.GetDetail(config_id, ref systemCode);

            var handler = new SystemConfigHandler();
            var formTableJSON = handler.GetPopForm(obj);
            return formTableJSON;
        }

        public string Update(FormCollection collection)
        {
            var config_id = collection.GetFormValue(PayloadKeys.SystemConfig.config_id);

            var value = collection.GetFormValue(PayloadKeys.SystemConfig.value);

            var systemConfigObject = new SystemConfigObject()
            {
                config_id = config_id,
                value = value
            };


            var sql_remark = "";
            if (config_id == 0)
            {
                var result = false;
                var msg = "";
                var url = "";

                return new { success = result, url = url, msg = msg }.ToJson();
            }
            else
            {
                var result = false;
                var msg = "";
                var url = "";

                var systemConfigManager = new SystemConfigManager();

                var systemCode = systemConfigManager.Update(
                    systemConfigObject);

                if (systemCode == CommonConstant.SystemCode.normal)
                {
                    msg = "Update Success";
                    result = true;
                }
                else if (systemCode == CommonConstant.SystemCode.no_permission)
                    msg = "Update Failed: no permission";
                else
                    msg = "Update Failed";

                return new { success = result, url = url, msg = msg }.ToJson();
            }

        }
    }
}
