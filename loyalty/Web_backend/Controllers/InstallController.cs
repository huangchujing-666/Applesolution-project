using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Palmary.Loyalty.BO.Modules.Administration.Security;
using Palmary.Loyalty.BO.Modules.Administration.Section;
using Palmary.Loyalty.BO.Modules.Passcode;
using Palmary.Loyalty.BO.Modules.Administration.SystemControl;
using Palmary.Loyalty.BO.Modules.Product;
using Palmary.Loyalty.BO.Database;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.ExtJsEntity;
using Palmary.Loyalty.Web_backend.Modules.GlobalHandlers;
using Palmary.Loyalty.Web_backend.ObjectModels.Infrastructure.PayoadKey;
using Palmary.Loyalty.Web_backend.Modules.Utility;
using System.Web.Routing;

using Palmary.Loyalty.Common;
using Palmary.Loyalty.Web_backend;

namespace Palmary.Loyalty.Web_backend.Controllers
{
    
    public class InstallController : Controller
    {
        //
        // GET: /Install/

        private PasscodeFormatManager _passcodFormatManager;
        private SystemConfigManager _systemConfigManager;
       // private SectionManager _sectionManager;

        public InstallController()
        {
            _passcodFormatManager = new PasscodeFormatManager();
            _systemConfigManager = new SystemConfigManager();
          //  _sectionManager = new SectionManager();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            SystemHandler.InitSystemParameters(this.HttpContext.ApplicationInstance as MvcApplication);
        }

        [AllowAnonymous]
        public ActionResult Start()
        {
            return View("Install");
        }

         [AllowAnonymous]
        public string CreatePasscodeFormat(FormCollection collection)
        {
            var passcode_insert_method = collection["passcode_insert_method"].Trim();
            var passcode_format = collection["passcode_format"].Trim();
            var result = "";
            var addFlag = false;
            var sql_remark = "";

            //addFlag = _systemConfigManager.Create(1, "passcode_insert_method", passcode_insert_method, true, 1, ref sql_remark);

            if (passcode_insert_method == "AutoGenerate")
            {
                var safetyLimit_precent = Convert.ToDouble(collection["safetyLimit_precent"].Trim());
                var noOfyear_expiry = int.Parse(collection["noOfyear_expiry"].Trim());
                var noOfmonth_expiry = noOfyear_expiry * 12;
                var format_id = 1; // force to 1
                var status = 1; //active

                //addFlag = _sectionManager.Create(

                //    1, //user_id
                //    304, //section_id,
                //    3, //parent,
                //    "Passcode Generate", //name,
                //    26, //icon,
                //    "com.palmary.passcodegenerate.js.index", //link,
                //    1, //display,
                //    4, //display_order,
                //    "passcodegenerate", //module,
                //    1, //status,

                //    1, //leaf,
                //    ref sql_remark);

                if (addFlag)
                {
                    addFlag = _passcodFormatManager.Create(
                    SessionManager.Current.obj_id,

                    format_id,
                    passcode_format,

                    safetyLimit_precent,
                    noOfmonth_expiry,
                    status,
                    ref sql_remark);
                }
                
            }
            else if (passcode_insert_method == "Excel")
            {
                //addFlag = _sectionManager.Create(
                    
                //    1, //user_id
                //    304, //section_id,
                //    3, //parent,
                //    "Generate Passcode", //name,
                //    26, //icon,
                //    "com.palmary.passcodeexcel.js.index", //link,
                //    1, //display,
                //    4, //display_order,
                //    "passcodegenerate", //module,
                //    1, //status,
                
                //    1, //leaf,
                //    ref sql_remark);
            }
           
            if (addFlag)
                result = new { success = true, result = new { msg = sql_remark } }.ToJson();
            else
                result = new { success = false, result = new { msg = sql_remark } }.ToJson();

            return result;
        }
    }
}
